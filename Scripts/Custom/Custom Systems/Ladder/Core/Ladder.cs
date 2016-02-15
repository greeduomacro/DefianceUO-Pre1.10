/**
*	Ladder system by Morten Legarth (c)taunted.dk ( legarth@taunted.dk )
*	Version: v0.10 -  26-02-2005
*
*	This system has been written for use at the Blitzkrieg frees-shard
*	http://blitzkrieg.dorijan.de . Unauthorized reproduction or redistribution
*	is prohibited.
*
*								Ladder.cs
*						-------------------------
*
*	File Description:	This is the core file for the ladder system, it
*						contains the data-model and handles saving and
*						loading of the data.
*
*
*	Updated: 02-03-2005 (v0.15)
*
*	Extra featues added:
*	-	Countdown of 5 second before the duel begins.
*	-	Automatic placing of gates so people can leave.
*	-	Arenas clean after 1 minute now
*	-	Arenas clean automatically if people leave
*	-	Force dismounting of players
*	-	Allow Dagger in 5x mage
*	-	Force removal of RA and Reflect
*	-	Unique wearables for top 3 that automatically gets put on.
*	-	Ladder Areas that can contain multiple Arneas
*	-	A Ladder gate that shows the current Ladder Areas and details about them
*	-
*
*
*	Updated: 26-02-2005 (v0.10)
*	Complete revamp of the system, little of the original code remains.
*
*	Extra features added:
*	-	Ability to select duel type (5xmage or 7xmage), selecting
*		5xmage will make it impossible to wield a weapon in duel.
*	-	Ability to allow/ban potions
*	-	Ability to allow/ban summoning
*	-	Ability to allow/ban looting
*	-	Automatic teleportation of duellers to a free arena
*	-	Dynamic setup of Arenas. GM's can build arenas and add them
*		with an Arena Controller ( [add arenacontrol )
*	-	Ability to wager money in the duel
*	-	New entry in the ranking board showing honor change for each fighter
*	-	New choice of viewing all fights in the las 24 hours
*
*	Updated: 15-02-2005 (v0.05)
*	Modified version that DOES NOT make use of a database. The DB was dropped
*	to ensure more rapid development of the system for Defiance Shard. The
*	new system saves data in binary files, which is less flexible but faster
*	to work with.
*
*	Original: 25-05-2004 (v0.01)
*	First publish.
*/

using System;
using System.IO;
using System.Collections;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Factions;

namespace Server.Ladder
{
	public class Fight
	{
		Mobile winner;
		Mobile loser;
		DateTime startTime;
		DateTime endTime;
		int honorGain;
		int honorLoss;
		int difficulty;

        // Constructor for deserilization
        public Fight()
        {
            // perhaps some kind of initialization
        }

		public Fight(Mobile winner, Mobile loser, DateTime startTime, DateTime endTime, int honorGain, int honorLoss, int difficulty)
		{
			this.winner = winner;
			this.loser = loser;
			this.startTime = startTime;
			this.endTime = endTime;
			this.honorGain = honorGain;
			this.honorLoss = honorLoss;
			this.difficulty = difficulty;
		}
		public Mobile Winner
		{
			get { return winner; }
			set { winner = value; }
		}
		public Mobile Loser
		{
			get { return loser; }
			set { loser = value; }
		}
		public DateTime Start
		{
			get { return startTime; }
			set { startTime = value; }
		}
		public DateTime End
		{
			get { return endTime; }
			set { endTime = value; }
		}
		public int Gain
		{
			get { return honorGain; }
			set { honorGain = value; }
		}
		public int Loss
		{
			get { return honorLoss; }
			set { honorLoss = value; }
		}
		public int Difficulty
		{
			get { return difficulty; }
			set { difficulty = value; }
		}

		public void Serialize( GenericWriter writer )
		{
			writer.Write( (int) 0 );
            writer.Write( winner );
            writer.Write( loser );
            writer.Write( startTime );
            writer.Write( endTime );
            writer.Write( honorGain );
            writer.Write( honorLoss );
            writer.Write( difficulty );

        }

		public void Deserialize( GenericReader reader )
		{
			int version = reader.ReadInt();
            winner = reader.ReadMobile();
			loser = reader.ReadMobile();
			startTime = reader.ReadDateTime();
            endTime = reader.ReadDateTime();
            honorGain = reader.ReadInt();
            honorLoss = reader.ReadInt();
            difficulty = reader.ReadInt();
        }
	}

	public class Ladder
	{
        private static string fightIdxPath = Path.Combine("Saves/Ladder/", "Fights.idx");
		private static string fightBinPath = Path.Combine("Saves/Ladder/", "Fights.bin");

		private static string duelIdxPath = Path.Combine("Saves/Ladder/", "Duels.idx");
		private static string duelBinPath = Path.Combine("Saves/Ladder/", "Duels.bin");

		private static string arenaBinPath = Path.Combine("Saves/Ladder/", "Arenas.bin");

		private static string duellersBinPath = Path.Combine("Saves/Ladder/", "Duellers.bin");

		// The entire list of fights (Loaded from binaries)
		private static ArrayList fights;
		// List of players who used the system (No need to search through all mobiles in server)
		private static ArrayList players;

		// Dynamic lists.
        private static ArrayList duels;
        private static ArrayList duellers;
        private static ArrayList arenas;

		//The interval that honorchanges is shown for
		private static TimeSpan interval = TimeSpan.FromDays(1);
		//Index of the current last fight within the interval. (Optimization variable)
		private static int lastIntervalIndex;


		public static void Initialize()
        {
			fights = new ArrayList();
			players = new ArrayList();

			duels = new ArrayList();
            duellers = new ArrayList();
            arenas = new ArrayList();

            LoadLadder();
			EventSink.PlayerDeath += new PlayerDeathEventHandler(EventSink_PlayerDeath);

			Server.Commands.Register("ToggleLadder", AccessLevel.Player, new CommandEventHandler(ToggleLadder_OnCommand));


		}

		[Usage("ToggleLadder")]
		[Description("Toggles if you can be challnged or not.")]
		public static void ToggleLadder_OnCommand(CommandEventArgs e)
		{
			if(e.Mobile != null && e.Mobile is PlayerMobile)
			{
				PlayerMobile pm = (PlayerMobile)e.Mobile;
				pm.AllowChallenge = !pm.AllowChallenge;
				pm.SendMessage("You are now {0} accepting challenges", pm.AllowChallenge ? "" : "not");
			}
		}

		public static DuelObject GetDuel(Mobile from)
		{
			foreach(DuelObject d in duels)
			{
				if(d.Player1 == from || d.Player2 == from)
					return d;
			}
			return null;
		}

		public static bool WeapAllowed(PlayerMobile m, BaseWeapon weap)
		{
			DuelObject duel = GetDuel(m);
			if (duel != null)
			{
				bool factionBan = weap is IFactionItem && ((IFactionItem)weap).FactionItemState != null;
				bool magicBan = !duel.MagicWeapons && (weap.AccuracyLevel != WeaponAccuracyLevel.Regular || weap.DamageLevel != WeaponDamageLevel.Regular || weap.DurabilityLevel != WeaponDurabilityLevel.Regular);
				bool poisonBan = !duel.PoisonedWeapons && (weap.Poison != null && weap.PoisonCharges > 0);
				bool typeBan = duel.DuelType == 1 && !(weap is Dagger);
				bool tribalBan = weap is TribalSpear;

				return !factionBan && !magicBan && !poisonBan && !typeBan && !tribalBan;
			}

			return true;
		}

		public static bool IsLootable(Corpse target)
		{
			foreach (LadderAreaControl LAC in arenas)
			{
				if (LAC != null)
				{
					foreach (ArenaControl AC in LAC.Arenas)
					{
						if (AC != null && AC.MyRegion != null && AC.MyRegion.Contains(target.Location))
							return true;
					}
				}

			}
			return false;
		}

		public static bool IsDuelling(Mobile one, Mobile two)
		{
			/*if (!duellers.Contains(one) || !duellers.Contains(two))
				return false;
			*/
			foreach (DuelObject d in duels)
			{
				if ((d.Player1 == one && d.Player2 == two) || (d.Player1 == two && d.Player2 == one))
					return true;
			}
			return false;
		}

        public static ArrayList Duels
        {
            get { return duels; }
            set { duels = value; }
        }
        public static ArrayList Duellers
        {
            get { return duellers; }
            set { duellers = value; }
        }
        public static ArrayList Arenas
        {
            get { return arenas; }
            set { arenas = value; }
        }
        public static ArrayList Fights
		{
			get { return fights; }
			set { fights = value; }
		}
		public static ArrayList Players
		{
			get { return players; }
			set { players = value; }
		}


		public static int IntervalIndex
		{
			get { return lastIntervalIndex; }
			set { lastIntervalIndex = value; }
		}

		public static TimeSpan Interval
		{
			get { return interval; }
			set { interval = value; }
		}



		public static void EventSink_PlayerDeath(PlayerDeathEventArgs e)
		{
			if (!duellers.Contains(e.Mobile))
				return;

			foreach(DuelObject duel in duels)
			{
				if (duel.Player1 == e.Mobile)
				{
					if (duel.Timer != null)
					{
						duel.Timer.Stop();
						duel.Finished(2, DateTime.Now);
						return;
					}
					else
					{
						duel.Finished(-2, DateTime.Now);
						return;
					}
				}
				else if (duel.Player2 == e.Mobile)
				{
					if (duel.Timer != null)
					{
						duel.Timer.Stop();
						duel.Finished(1, DateTime.Now);
						return;
					}
					else
					{
						duel.Finished(-2, DateTime.Now);
						return;
					}
				}
			}
		}

		public static void SaveLadder()
		{

            if (!Directory.Exists("Saves/Ladder/"))
                Directory.CreateDirectory("Saves/Ladder/");

			SaveFights();
			SaveDuels();
			SaveArenas();
			SaveDuellers();

		}
		#region Saver functions
		public static void SaveFights()
		{
			GenericWriter idx = new BinaryFileWriter(fightIdxPath, false);
			GenericWriter bin = new BinaryFileWriter(fightBinPath, true);

			idx.Write((int)fights.Count);
			foreach (Fight f in fights)
			{
				long startPos = bin.Position;
				f.Serialize(bin);
				idx.Write((long)startPos);
				idx.Write((int)(bin.Position - startPos));
			}

			idx.Close();
			bin.Close();
		}
		public static void SaveDuels()
		{
			GenericWriter idx = new BinaryFileWriter(duelIdxPath, false);
			GenericWriter bin = new BinaryFileWriter(duelBinPath, true);

			idx.Write((int)duels.Count);
			foreach (DuelObject d in duels)
			{
				if (!d.Player1.Deleted && !d.Player2.Deleted)
				{
					long startPos = bin.Position;
					d.Serialize(bin);
					idx.Write((long)startPos);
					idx.Write((int)(bin.Position - startPos));
				}
			}

			idx.Close();
			bin.Close();
		}

		public static void SaveArenas()
		{
			GenericWriter bin = new BinaryFileWriter(arenaBinPath, true);

			bin.WriteItemList(arenas, true);

			bin.Close();
		}
		public static void SaveDuellers()
		{
			GenericWriter bin = new BinaryFileWriter(duellersBinPath, true);

			bin.WriteMobileList(duellers, true);

			bin.Close();
		}
#endregion


		public static void LoadLadder()
        {
            Console.Write("Ladder: Loading...");
			LoadFights();
			LoadDuels();
			LoadArenas();
			LoadDuellers();
			Console.WriteLine("done");
		}

		#region Loader functions
		public static void LoadFights()
		{
			if (File.Exists(fightIdxPath) && File.Exists(fightBinPath))
            {
                // Declare and initialize reader objects.
                FileStream idx = new FileStream(fightIdxPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                FileStream bin = new FileStream(fightBinPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryReader idxReader = new BinaryReader(idx);
                BinaryFileReader binReader = new BinaryFileReader(new BinaryReader(bin));

                // Start by reading the number of figts from an index file
                int fightCount = idxReader.ReadInt32();
                //Console.WriteLine("Fight objects: {0}", fightCount);
				bool indexSet = false;
				for (int i = 0; i < fightCount; ++i)
                {
                    Fight fight = new Fight();
                    // Read start-position and length of current fight from index file
                    long startPos = idxReader.ReadInt64();
                    int length = idxReader.ReadInt32();
                    // Point the reading stream to the proper position
                    binReader.Seek(startPos, SeekOrigin.Begin);

                    try
                    {
                        fight.Deserialize(binReader);

                        if (binReader.Position != (startPos + length))
                            throw new Exception(String.Format("***** Bad serialize on Fight[{0}] *****", i));
                    }
                    catch (Exception e)
                    {
                        //handle
                    }
                    fights.Add(fight);
					// Read data into fast-access variables
					// This is done to optimize perfomance
					// Searching through long arrays repeatedly
					// Could slow down the server


					// Add mobile to the list of active players
					if (fight.Winner != null && !players.Contains(fight.Winner))
						players.Add(fight.Winner);
					if (fight.Loser != null && !players.Contains(fight.Loser))
						players.Add(fight.Loser);

					// Adjust Honor, win and loss variables
					if (fight.Winner != null)
					{
						((PlayerMobile)fight.Winner).Wins++;
						((PlayerMobile)fight.Winner).Honor += fight.Gain;
					}
					if (fight.Loser != null)
					{
						((PlayerMobile)fight.Loser).Losses++;
						((PlayerMobile)fight.Loser).Honor -= fight.Loss;
					}


					// Adjust HonorChange variable and set the index of the last fight in interval
					if ( fight.Start > DateTime.Now - interval)
					{
						if (fight.Winner != null)
							((PlayerMobile)fight.Winner).HonorChange += fight.Gain;
						if (fight.Loser != null)
							((PlayerMobile)fight.Loser).HonorChange -= fight.Loss;
						if(!indexSet)
						{
							indexSet = true;
							lastIntervalIndex = fights.IndexOf(fight);
						}
					}

				}
				// Only old fights were found, or there was no fights to load.... so we set the index to last fight
				if (!indexSet && fights.Count != 0)
					lastIntervalIndex = fights.Count - 1;
				players.Sort();
				// Remember to close the streams
                idxReader.Close();
                binReader.Close();
            }
		}

		public static void LoadDuels()
		{
			if (File.Exists(duelIdxPath) && File.Exists(duelBinPath))
			{
				// Declare and initialize reader objects.
				FileStream idx = new FileStream(duelIdxPath, FileMode.Open, FileAccess.Read, FileShare.Read);
				FileStream bin = new FileStream(duelBinPath, FileMode.Open, FileAccess.Read, FileShare.Read);
				BinaryReader idxReader = new BinaryReader(idx);
				BinaryFileReader binReader = new BinaryFileReader(new BinaryReader(bin));

				// Start by reading the number of duels from an index file
				int duelCount = idxReader.ReadInt32();

				for (int i = 0; i < duelCount; ++i)
				{
					DuelObject d = new DuelObject();
					// Read start-position and length of current fight from index file
					long startPos = idxReader.ReadInt64();
					int length = idxReader.ReadInt32();
					// Point the reading stream to the proper position
					binReader.Seek(startPos, SeekOrigin.Begin);

					try
					{
						d.Deserialize(binReader);

						if (binReader.Position != (startPos + length))
							throw new Exception(String.Format("***** Bad serialize on DuelObject[{0}] *****", i));
					}
					catch (Exception e)
					{
						//handle
					}
					duels.Add(d);
				}
				// Remember to close the streams
				idxReader.Close();
				binReader.Close();
			}
		}

		public static void LoadArenas()
		{
			if (File.Exists(arenaBinPath))
			{
				// Declare and initialize reader objects.
				FileStream bin = new FileStream(arenaBinPath, FileMode.Open, FileAccess.Read, FileShare.Read);
				BinaryFileReader binReader = new BinaryFileReader(new BinaryReader(bin));

				try
				{
					arenas = binReader.ReadItemList();
				}
				catch (Exception e)
				{

				}

				binReader.Close();
			}
		}

		public static void LoadDuellers()
		{
			if (File.Exists(duellersBinPath))
			{
				// Declare and initialize reader objects.
				FileStream bin = new FileStream(duellersBinPath, FileMode.Open, FileAccess.Read, FileShare.Read);
				BinaryFileReader binReader = new BinaryFileReader(new BinaryReader(bin));

				try
				{
					duellers = binReader.ReadMobileList();
				}
				catch (Exception e)
				{

				}

				binReader.Close();
			}
		}
		#endregion
	}

}