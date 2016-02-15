/*
 * SunUO
 *
 * (c) 2006 Max Kellermann <max@duempel.org>
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; version 2 of the License.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program; if not, write to the Free Software
 *  Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
 *
 */

using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Threading;
using Server;
using MySql.Data.MySqlClient;

namespace Server.Accounting {
	public class SyncDB {
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly string m_ConnectString = StaticConfiguration.AccountDatabaseConnectString;

		private static readonly IFormatProvider m_Culture = new CultureInfo("en-GB", true);

		private static long m_LatestUpdate = 0;
		private static Timer m_Timer;
		private static Thread m_Thread = null;

		public static void Initialize() {
			m_Timer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromMinutes(5.0),
									  new TimerCallback(StartSync));

			Server.Commands.Register("ReloadAccountDB", AccessLevel.Administrator,
									 new CommandEventHandler(ManualSync));

            Server.Commands.Register("SyncDBFull", AccessLevel.Administrator,
                                     new CommandEventHandler(ManualSync));
            Server.Commands.Register("SyncDBDifferential", AccessLevel.Administrator,
									 new CommandEventHandler(ManualDifferentialSync));
        }

		private static DateTime ParseDateTime(string username, string value) {
			try {
				return DateTime.Parse(value, m_Culture);
			} catch (Exception e) {
				log.Error(String.Format("invalid DateTime in account record '{0}'", username), e);
				return DateTime.MinValue;
			}
		}

		private static TimeSpan ParseTimeSpan(string username, string value) {
			try {
				return TimeSpan.Parse(value);
			} catch (Exception e) {
				log.Error(String.Format("invalid TimeSpan in account record '{0}'", username), e);
				return TimeSpan.Zero;
			}
		}

		private static void Sync(IDataReader reader) {
			int numCreated = 0, numUpdated = 0;

			while (reader.Read()) {
				string username = reader.GetString(0);
				string password = reader.GetString(1);
				int flags = reader.GetInt32(2);
				int accessLevel = reader.GetInt32(3);
				DateTime created = ParseDateTime(username, reader.GetString(4));
				DateTime lastLogin = ParseDateTime(username, reader.GetString(5));
				DateTime donationStart = ParseDateTime(username, reader.GetString(6));
				TimeSpan donationDuration = ParseTimeSpan(username, reader.GetString(7));
				long latestUpdate = reader.GetInt64(8);

				/* skip invalid accounts */
				if (username == null || username == "" ||
					password == null || password == "")
					continue;

				/* we should better pause during a world save */
				if (World.Saving) {
					log.Info("cancelling account sync because of world save");
					break;
				}

				/* get or create an account */
				Account account = Accounts.GetAccount(username);
				if (account == null) {
					log.Info(String.Format("creating account '{0}'", username));
					account = Accounts.AddAccount(username, "");
					numCreated++;
                    //Console.WriteLine(String.Format("Debug: Created account:{0}, latestupdate:{1}", username,latestUpdate));
                }
                else
                {
					numUpdated++;
                    //Console.WriteLine(String.Format("Debug: Updated account:{0}, latestupdate:{1}", username, latestUpdate));
				}

				/* transform hashed password into RunUO format */
				password = password.ToUpper();
				for (int i = 2; i < 32 + 15; i += 3)
					password = password.Insert(i, "-");

				/* update Account properties */
				account.CryptPassword = password;
				account.Flags = flags;
				account.AccessLevel = (AccessLevel)accessLevel;
				account.Created = created;
				account.LastLogin = lastLogin;

				/* update donation status */
				//if (donationStart != DateTime.MinValue && donationDuration != TimeSpan.Zero)
				//	Server.Misc.DonationSystem.SetSubscriptionStatus(account, donationStart, donationDuration);

				if (latestUpdate > m_LatestUpdate)
					m_LatestUpdate = latestUpdate;
			}
            //Console.WriteLine(String.Format("Debug: m_LatestUpdate={0}", m_LatestUpdate));

			log.Info(String.Format("account sync finished, {0} new accounts, {1} updated accounts",
								   numCreated, numUpdated));
            Console.WriteLine(String.Format("account sync finished, {0} new accounts, {1} updated accounts",
                                   numCreated, numUpdated));
        }

		private static void Sync() {
			log.Info(String.Format("starting account sync, LatestUpdate={0}", m_LatestUpdate));
            Console.WriteLine(String.Format("starting account sync, LatestUpdate={0}", m_LatestUpdate));

			/* connect to the database */

			IDbConnection dbcon;
			dbcon = new MySqlConnection(m_ConnectString);
			dbcon.Open();

			/* send query */

			IDbCommand dbcmd = dbcon.CreateCommand();
            /*
            dbcmd.CommandText = "SELECT Username,Password,Flags,AccessLevel," +
				"Created,LastLogin,DonationStarted,DonationDuration," +
				"LatestUpdate" +
				" FROM users WHERE LatestUpdate>=?LatestUpdate" +
				" ORDER BY LatestUpdate";
            */
            //Al: Update all accounts that have been updated 1 hour before m_latestUpdate or later
			dbcmd.CommandText = "SELECT Username,Password,Flags,AccessLevel," +
				"Created,LastLogin,DonationStarted,DonationDuration," +
				"LatestUpdate" +
                " FROM users WHERE (TIMESTAMPADD(HOUR,1,LatestUpdate)+0)>=?LatestUpdate" +
				" ORDER BY LatestUpdate";

			IDataParameter p = (IDataParameter)dbcmd.CreateParameter();
			p.ParameterName = "?LatestUpdate";
			p.DbType = DbType.Int64;
			p.Value = m_LatestUpdate;
			dbcmd.Parameters.Add(p);

			/* receive results */

			using (IDataReader reader = dbcmd.ExecuteReader()) {
				Sync(reader);
			}

			dbcmd.Dispose();
			dbcon.Dispose();
		}

		private static void RunSync() {
			if (World.Saving) {
				log.Warn("refusing to sync accounts during world save");
				m_Thread = null;
				return;
			}

			try {
				Sync();
			} catch (Exception e) {
				log.Error(e);
			} finally {
				m_Thread = null;
			}
		}

		private static void StartSync() {
			if (m_Thread != null && !m_Thread.IsAlive)
				m_Thread = null;

			if (m_Thread != null) {
				log.Warn("account sync already running");
				return;
			}

			if (World.Saving) {
				log.Warn("refusing to sync accounts during world save");
				return;
			}

			//Al: Quickfix for the problem that not all accounts are updated
			//m_LatestUpdate = 0;
			//--


			m_Thread = new Thread(new ThreadStart(RunSync));
			m_Thread.Name = "Server.Accounting.SyncDB";
			m_Thread.Priority = ThreadPriority.BelowNormal;
			m_Thread.Start();
		}

        [Usage("SyncDBFull")]
        [Description("Performs full Database sync.")]
        public static void ManualSync(CommandEventArgs e)
        {
			if (m_Thread != null && !m_Thread.IsAlive)
				m_Thread = null;

			if (m_Thread != null) {
				if (e != null && e.Mobile != null)
					e.Mobile.SendMessage("account sync already running");
				return;
			}

			m_LatestUpdate = 0;
			if (e != null && e.Mobile != null)
				e.Mobile.SendMessage("Reloading the account database...");
			StartSync();
		}

        [Usage("SyncDBDifferential")]
        [Description("Performs differential Database sync.")]
        public static void ManualDifferentialSync(CommandEventArgs e)
        {
            if (m_Thread != null && !m_Thread.IsAlive)
                m_Thread = null;

            if (m_Thread != null)
            {
                if (e != null && e.Mobile != null)
                    e.Mobile.SendMessage("account sync already running");
                return;
            }

            if (e != null && e.Mobile != null)
                e.Mobile.SendMessage("Performing manual sync on updated accounts...");
            StartSync();
        }

        public static void PullAccount(string username)
        {
			if (username == null || username == "" || World.Saving)
				return;

			log.Info(String.Format("pulling account '{0}' from database",
								   username));

			try {
				/* connect to the database */

				IDbConnection dbcon;
				dbcon = new MySqlConnection(m_ConnectString);
				dbcon.Open();

				/* send query */

				IDbCommand dbcmd = dbcon.CreateCommand();
				dbcmd.CommandText = "SELECT Username,Password,Flags,AccessLevel," +
					"Created,LastLogin,DonationStarted,DonationDuration," +
					"LatestUpdate" +
					" FROM users WHERE Username=?Username";

				IDataParameter p = (IDataParameter)dbcmd.CreateParameter();
				p.ParameterName = "?Username";
				p.DbType = DbType.String;
				p.Value = username;
				dbcmd.Parameters.Add(p);

				/* receive results */

				using (IDataReader reader = dbcmd.ExecuteReader()) {
					Sync(reader);
				}

				dbcmd.Dispose();
				dbcon.Dispose();
			} catch (Exception e) {
				log.Error(e);
			}
		}
        public static bool PullEmail(string username)
        {
            if (username == null || username == "" || World.Saving)
                return false;

            bool hasEmail = false;
            try
            {
                /* connect to the database */

                IDbConnection dbcon;
                dbcon = new MySqlConnection(m_ConnectString);
                dbcon.Open();

                /* send query */

                IDbCommand dbcmd = dbcon.CreateCommand();
                dbcmd.CommandText = "SELECT Email FROM users WHERE Username=?Username";

                IDataParameter p = (IDataParameter)dbcmd.CreateParameter();
                p.ParameterName = "?Username";
                p.DbType = DbType.String;
                p.Value = username;
                dbcmd.Parameters.Add(p);

                /* receive results */

                using (IDataReader reader = dbcmd.ExecuteReader())
                {
                    if (reader.Read())
                        if (reader.GetString(0) != "" && reader.GetString(0) != null) hasEmail = true;
                }

                dbcmd.Dispose();
                dbcon.Dispose();
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            return hasEmail;
        }

        public static bool PushEmail(string user, string emailaddress)
        {
            if (user == null || user == "" || emailaddress == null)
                return false;

            /* connect to the database */

            try
            {
                IDbConnection dbcon;
                dbcon = new MySqlConnection(m_ConnectString);
                dbcon.Open();

                /* send query */

                IDbCommand dbcmd = dbcon.CreateCommand();
                dbcmd.CommandText = "UPDATE users SET email=?email," +
                    " LatestUpdate = CURRENT_TIMESTAMP()+0" +
                    " WHERE username=?username AND email is null";

                IDataParameter email = (IDataParameter)dbcmd.CreateParameter();
                IDataParameter username = (IDataParameter)dbcmd.CreateParameter();
                email.ParameterName = "?email";
                email.DbType = DbType.String;
                email.Value = emailaddress;
                dbcmd.Parameters.Add(email);
                username.ParameterName = "?username";
                username.DbType = DbType.String;
                username.Value = user;
                dbcmd.Parameters.Add(username);

                int RowsAffected = dbcmd.ExecuteNonQuery();

                dbcmd.Dispose();
                dbcon.Dispose();

                if (RowsAffected < 1)
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}