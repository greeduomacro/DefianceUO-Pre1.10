using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Accounting;
using System.Data;
using System.Collections;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using Server.Targeting;
using Server.Mobiles;
using System.Threading;
using Server.Scripts;

namespace Server.Scripts
{
	public class EmailSystem
	{
		private static ArrayList ms_EmailCommunicators;
		public static void Initialize()
		{
			Server.Commands.Register("email", AccessLevel.Administrator, new CommandEventHandler(Email_OnCommand));
		}

		[Usage("Email")]
		[Description("Sends the targeted Playermobile the emailconfiguration gump.")]
		private static void Email_OnCommand(CommandEventArgs e)
		{
			if (e.Mobile == null || e.Mobile.Deleted) return;
			e.Mobile.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(Email_OnTarget));
			e.Mobile.SendMessage("Target a playermobile that should get the email configuration gump.");
		}

		public static void Email_OnTarget(Mobile from, object obj)
		{
			if (from == null || from.Deleted || !(obj is PlayerMobile)) return;
			SendEmailGump((PlayerMobile) obj);
		}

		public static void SendEmailGump(object o)
		{
			PlayerMobile pm = o as PlayerMobile;
			if (pm == null || pm.NetState == null || pm.Account == null) return;
			AddEmailCommunicator(new EmailPuller(pm));
		}

		public static void AddEmailCommunicator(EmailCommunicator ec)
		{
			if (ms_EmailCommunicators == null) ms_EmailCommunicators = new ArrayList();
			ms_EmailCommunicators.Add(ec);
		}
		public static void RemoveEmailCommunicator(EmailCommunicator ec)
		{
			ms_EmailCommunicators.Remove(ec);
		}

		public class EmailCommunicator
		{
			protected PlayerMobile m_playerMobile;
			protected Thread m_thread;
			public EmailCommunicator(PlayerMobile playerMobile)
			{
				m_playerMobile = playerMobile;
			}
		}
		public class EmailPuller : EmailCommunicator
		{
			public EmailPuller(PlayerMobile playerMobile) : base(playerMobile)
			{
				m_thread = new Thread(new ThreadStart(Pull));
				m_thread.Name = "Server.Scripts.EmailPuller";
				m_thread.Priority = ThreadPriority.BelowNormal;
				m_thread.Start();
			}
			public void Pull()
			{
				if (m_playerMobile == null || m_playerMobile.NetState == null) return;
				if (!(SyncDB.PullEmail(((Account)m_playerMobile.Account).Username)))
				{
					m_playerMobile.CloseGump(typeof(emailgump));
					m_playerMobile.SendGump(new emailgump());
				}
				EmailSystem.RemoveEmailCommunicator(this);
			}
		}

		public class EmailPusher : EmailCommunicator
		{
			private string m_email;
			public EmailPusher(PlayerMobile playerMobile, string email) : base(playerMobile)
			{
				m_email = email;
				m_thread = new Thread(new ThreadStart(Push));
				m_thread.Name = "Server.Scripts.EmailPusher";
				m_thread.Priority = ThreadPriority.BelowNormal;
				m_thread.Start();
			}
			public void Push()
			{
				if (m_playerMobile == null || m_playerMobile.NetState == null) return;
				if (SyncDB.PushEmail( ((Account)m_playerMobile.Account).Username, m_email))
						m_playerMobile.SendMessage("Your email address has been successfully set.");
				else
						m_playerMobile.SendMessage("There has been a problem setting your email address.");
				EmailSystem.RemoveEmailCommunicator(this);
			}
		}
	}
}



namespace Server.Gumps
{
	public class emailgump : Gump
	{
		private static readonly string m_ConnectString = StaticConfiguration.AccountDatabaseConnectString;

		public emailgump()
			: base( 0, 0 )
		{
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
						AddBlackAlpha(0, 0, 425, 262);

						this.AddHtml( 12, 13, 400, 25, "<center>Register Email Address</center>", (bool)true, (bool)false);
						this.AddHtml(12, 41, 400, 85, "Please specify an email address.<br>This address will be used to recover lost passwords.<br>Please visit the defiance <a href=\"http://accounts.defianceuo.com/account.pl\">account management</a> system.<br>Enjoy Defiance!", (bool)true, (bool)false);

						this.AddHtml(20, 150, 150, 25, Color("Email Address:", 0xFFFFFF), false, false);
						AddTextField(190, 150, 220, 20, 1);

						this.AddHtml(20, 180, 150, 25, Color("Confirm Email:", 0xFFFFFF), false, false);
						AddTextField(190, 180, 220, 20, 2);

						AddButtonLabeled(20, 220, 1, Color("Submit", 0xFFFFFF));
		}

				public override void OnResponse(NetState sender, RelayInfo info)
				{
						if (sender == null || sender.Mobile == null || sender.Mobile.Deleted || info.ButtonID != 1 || !(sender.Mobile is PlayerMobile) )
								return;

						Mobile from = sender.Mobile;

						if (info.GetTextEntry(1) != null && info.GetTextEntry(2) != null &&
								info.GetTextEntry(1).Text == info.GetTextEntry(2).Text &&
								info.GetTextEntry(1).Text != "" && info.GetTextEntry(2).Text != "" &&
								IsValidEmail(info.GetTextEntry(1).Text.ToString()))
						{
								string email = info.GetTextEntry(1).Text as string;
								Account acc = from.Account as Account;
								EmailSystem.AddEmailCommunicator(new EmailSystem.EmailPusher((PlayerMobile)sender.Mobile, email));
						}
						else
						{
								from.SendMessage("Please enter your valid emailaddress twice. Try again.");
								from.SendGump(this);
						}
						return;
				}

				public bool IsValidEmail(string email)
				{
						if (email == null)
								return false;

						Regex rx = new Regex(@"^\w[\w|\.\-]+@\w[\w\.\-]+\.[a-zA-Z]{2,4}$");
						Match m = rx.Match(email);

						return m.Success;
				}

				public void AddTextField(int x, int y, int width, int height, int index)
				{
						AddBackground(x - 2, y - 2, width + 4, height + 4, 0x2486);
						AddTextEntry(x + 2, y + 2, width - 4, height - 4, 0, index, "");
				}

				public void AddBlackAlpha(int x, int y, int width, int height)
				{
						AddImageTiled(x, y, width, height, 2624);
						AddAlphaRegion(x, y, width, height);
				}

				public string Color(string text, int color)
				{
						return String.Format("<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", color, text);
				}

				public void AddButtonLabeled(int x, int y, int buttonID, string text)
				{
						AddButton(x, y - 1, 4005, 4007, buttonID, GumpButtonType.Reply, 0);
						AddHtml(x + 35, y, 240, 20, text, false, false);
				}
	}
}