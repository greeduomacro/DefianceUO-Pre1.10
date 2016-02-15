using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Misc;
using Server.Items;
using Server.Gumps;
using Server.Multis;
using Server.Engines.Help;
using Server.ContextMenus;
using Server.Network;
using Server.Spells;
using Server.Spells.First;
using Server.Spells.Second;
using Server.Spells.Fourth;
using Server.Spells.Fifth;
using Server.Spells.Sixth;
using Server.Spells.Seventh;
using Server.Targeting;
using Server.Engines.Quests;
using Server.Factions;
using Server.Regions;
using Server.Accounting;
using Server.Ladder; // added for ladder
using Server.FSPvpPointSystem;
using Xanthos.Evo;
using Server.Scripts.Commands; //Logging


namespace Server.Mobiles
{
	[Flags]
	public enum PlayerFlag // First 16 bits are reserved for default-distro use, start custom flags at 0x00010000
	{
		None			= 0x00000000,
		Glassblowing		= 0x00000001,
		Masonry			= 0x00000002,
		SandMining		= 0x00000004,
		StoneMining		= 0x00000008,
		ToggleMiningStone	= 0x00000010,
		KarmaLocked		= 0x00000020,
		AutoRenewInsurance	= 0x00000040,
		UseOwnFilter		= 0x00000080,
		PublicMyRunUO		= 0x00000100,
		PagingSquelched		= 0x00000200,
		Young			= 0x00000400
	}

	public enum NpcGuild
	{
		None,
		MagesGuild,
		WarriorsGuild,
		ThievesGuild,
		RangersGuild,
		HealersGuild,
		MinersGuild,
		MerchantsGuild,
		TinkersGuild,
		TailorsGuild,
		FishermensGuild,
		BardsGuild,
		BlacksmithsGuild
	}

	public enum SolenFriendship
	{
		None,
		Red,
		Black
	}

	//This interface is implemented for Ladder system
	public class PlayerMobile : Mobile, IComparable
	{
		  private static readonly ArrayList ms_EmptyArrayList = new ArrayList(0); //Workaround to use optimizations without SunUO Core

		[CommandProperty(AccessLevel.Administrator)]
		public string ClientVersion
		{
			get
			{
				if (NetState != null)
				{
					return NetState.Version.ToString();
				}
				else
				{
					return "";
				}
			}
		}

		// Kamron added this
		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Seer)]
		public int FactionScore
		{
			get
			{
				PlayerState pl = PlayerState.Find( this );
				if ( pl == null )
					return 0;
				else
					return pl.KillPoints;
			}
			set
			{
				PlayerState pl = PlayerState.Find( this );
				if ( pl != null )
					pl.KillPoints = value;
			}
		}
        // end

        #region FastWalk
        //Used to get rid of those false positives.

        //Fast walk messages are only triggered if occuring repeatedly in 10 seconds.
        private static readonly TimeSpan FASTWALK_TRIGGER_INTERVAL = TimeSpan.FromSeconds(10);
        private const int FASTWALK_TRIGGER_COUNT = 1;

        private DateTime m_lastFastWalk = DateTime.MinValue;
        private int m_fastWalkCount = 0;
        public int FastWalkCount { get { return m_fastWalkCount; } }

        public bool CheckFastWalk()
        {
            if ((DateTime.Now - m_lastFastWalk) <= FASTWALK_TRIGGER_INTERVAL)
                m_fastWalkCount++;
            else
                m_fastWalkCount = 1;

            m_lastFastWalk = DateTime.Now;
            return (m_fastWalkCount >= FASTWALK_TRIGGER_COUNT);

        }
        #endregion

        // Al: Pagespy
		  private bool m_pagespy;
		  [CommandProperty(AccessLevel.Administrator)]
		  public bool PageSpy
		  {
				get
				{
					 return m_pagespy;
				}
				set
				{
					 m_pagespy = value;
				}
		  }

		  //Al: Added this property wrapper to be able to
		  //		verify character age ingame.
		  [CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		  public DateTime CharacterCreationTime
		  {
				get
				{
					 return CreationTime;//Wrapped Mobile property
				}
				set
				{
					 CreationTime = value;
				}
		  }

		//Al: Added this property wrapper to be able to
		//		verify total game time ingame.
		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public TimeSpan AccountTotalGameTime
		{
			get
			{
				return (Account != null && Account is Account) ? ((Account)Account).TotalGameTime : TimeSpan.Zero;
			}
			set
			{
				if (Account != null && Account is Account && value >= TimeSpan.Zero)
					((Account)Account).TotalGameTime = value;
			}
		}

		//Al: Added this property wrapper to be able to verify Created ingame.
		[CommandProperty(AccessLevel.Administrator)]
		public DateTime AccountCreated
		{
			get
			{
				return (Account != null && Account is Account) ? ((Account)Account).Created : DateTime.MinValue;
			}
		}

		//Al: Added this property wrapper to be able to verify the account age ingame.
		//    This is only for testing purposes because Created is not synced back to the DB
		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public TimeSpan AccountAge
		{
			get
			{
				if (Account != null && Account is Account && ((Account)Account).Created != DateTime.MinValue)
					return DateTime.Now - ((Account)Account).Created;
				return TimeSpan.Zero;
			}
			set
			{
				if (Account != null && Account is Account && value >= TimeSpan.Zero)
					((Account)Account).Created = DateTime.Now - value;
			}
		}

		//Minkio: Added Ethereal Stamina. Used in the stamsystem
		  private int m_ethstam = StamSystem.etherealstam;
		  public int EtherealStam
		  {
				get
				{
					 return m_ethstam;
				}
				set
				{
					 m_ethstam = value;
				}
		  }

		  //Minkio: Used in the stamsystem
		  private DateTime m_LastMountStamGain = DateTime.MinValue;
		  public DateTime LastMountStamGain
		  {
				get
				{
					 return m_LastMountStamGain;
				}
				set
				{
					 m_LastMountStamGain = value;
				}
		  }

		// Minkio: Added for stamsystem (onlypvp option)
		// This attribute will be set everytime a player hurts someone or gets hurt by a player
		private DateTime m_LastPVP = DateTime.MinValue;
		[CommandProperty(AccessLevel.GameMaster)]
		public DateTime LastPVP
		{
			get
			{
				return m_LastPVP;
			}
			set
			{
				m_LastPVP = value;
			}
		}

		  [CommandProperty(AccessLevel.GameMaster, AccessLevel.Seer)]
		  public TimeSpan ShortTermElapse
		  {
				get { return m_ShortTermElapse; }
				set { m_ShortTermElapse = value; }
		  }

		  [CommandProperty(AccessLevel.GameMaster, AccessLevel.Seer)]
		  public TimeSpan LongTermElapse
		  {
				get { return m_LongTermElapse; }
				set { m_LongTermElapse = value; }
		  }

		private DateTime m_AnkhNextUse;
		[CommandProperty(AccessLevel.GameMaster)]
		public DateTime AnkhNextUse{ get{ return m_AnkhNextUse; } set{ m_AnkhNextUse = value; } }

		// Added Teleport Timer
		private DateTime m_LastTeleTime;

		public DateTime LastTeleTime
		{
		 get { return m_LastTeleTime; }
		 set { m_LastTeleTime = value; }
		}

		private bool m_Attackable;
		private Timer m_ExpireAttackable;

		private static TimeSpan m_ExpireAttackableDelay = TimeSpan.FromMinutes( 2.0 );

		public static TimeSpan ExpireAttackableDelay
		{
			get{ return m_ExpireAttackableDelay; }
			set{ m_ExpireAttackableDelay = value; }
		}

		private class ExpireAttackableTimer : Timer
		{
			private PlayerMobile m_Mobile;

			public ExpireAttackableTimer( PlayerMobile m ) : base( m_ExpireAttackableDelay )
			{
				this.Priority = TimerPriority.FiveSeconds;
				m_Mobile = m;
			}

			protected override void OnTick()
			{
				m_Mobile.Attackable = false;
			}
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public bool Attackable
		{
			get
			{
				return m_Attackable;
			}
			set
			{
				if ( m_Attackable != value )
				{
					m_Attackable = value;
					Delta( MobileDelta.Noto );
					InvalidateProperties();
				}

				if ( m_Attackable )
				{
					if ( m_ExpireAttackable == null )
						m_ExpireAttackable = new ExpireAttackableTimer( this );
					else
						m_ExpireAttackable.Stop();

					m_ExpireAttackable.Start();
				}
				else if ( m_ExpireAttackable != null )
				{
					m_ExpireAttackable.Stop();
					m_ExpireAttackable = null;
				}
			}
		}

		//****** ADDED LADDER SYSTEM***********//
		private int m_Honor;
		private int m_Wins;
		private int m_Losses;
		private int m_HonorChangeInInterval;

		private bool m_AllowChallenge = true;

		public int CompareTo(Object rhs)
		{
			return -1 * (this.m_Honor.CompareTo(((PlayerMobile)rhs).m_Honor));
		}

		[CommandProperty(AccessLevel.Seer)]
		public int Honor
		{
			get { return m_Honor; }
			set { m_Honor = value; }
		}

		[CommandProperty(AccessLevel.Seer)]
		public int Wins
		{
			get { return m_Wins; }
			set { m_Wins = value; }
		}

		[CommandProperty(AccessLevel.Seer)]
		public int Losses
		{
			get { return m_Losses; }
			set { m_Losses = value; }
		}

		public int HonorChange
		{
			get { return m_HonorChangeInInterval; }
			set { m_HonorChangeInInterval = value; }
		}

		public bool AllowChallenge
		{
			get { return m_AllowChallenge; }
			set { m_AllowChallenge = value; }
		}

		public override bool OnEquip(Item item)
		{
			if (item is BaseWeapon && !Ladder.Ladder.WeapAllowed(this, (BaseWeapon)item))
			{
				SendMessage("You cannot wield that weapon now.");
				return false;
			}

			/*
			if (this.Region is CustomRegion && item is BaseWeapon)
			{
				BaseWeapon weap = (BaseWeapon)item;
				RegionControl rc = ((CustomRegion)this.Region).Controller;
				if (rc is ArenaControl && rc.IsRestrictedSkill((int)weap.DefSkill) && !(weap is Dagger))
				{
					this.SendMessage("You are not allowed to wield that now");
					return false;
				}
				if (rc is ArenaControl && weap.Poison != null && weap.PoisonCharges > 0)
				{
					this.SendMessage("Poisoned weapons not allowed in duels");
					return false;
				}
				if (rc is ArenaControl && weap is TribalSpear)
				{
					this.SendMessage("Tribal spears not allowed in duels");
					return false;
				}
				if (rc is ArenaControl && weap is IFactionItem &&((IFactionItem)weap).FactionItemState != null)
				{
					this.SendMessage("Faction items not allowed in duels");
					return false;
				}
				if (rc is ArenaControl && (weap.AccuracyLevel != WeaponAccuracyLevel.Regular ||
					weap.DamageLevel != WeaponDamageLevel.Regular ||
					weap.DurabilityLevel != WeaponDurabilityLevel.Regular))
				{
					this.SendMessage("Magical weapons not allowed in duels");
					return false;
				}

			}*/
			return true;
		}

		//****** END LADDER SYSTEM***********//
		//*********ADDED POINT SYSTEM*************//

		private int m_Points = 0;
		private int minPoints = -3;
		private ArrayList killers = new ArrayList();

		//here are your variables to store the needed info

		public void AddKiller( PlayerMobile killer)
		{
			killers.Add(killer);
		}

		//this function is to add a killer to the list to check later if he has killed you within 4 hours

		public void RemoveKiller( PlayerMobile killer)
		{
			killers.Remove(killer);
		}

		//after 4 hours, remove the killer

		public bool KillerIsTimed( PlayerMobile killer)
		{
			return killers.Contains(killer);
		}

		//this is the function that looks to see if the killer is already in the list

		[CommandProperty(AccessLevel.GameMaster)]
		public int Points
		{
			get{return m_Points;}
			set{m_Points = value;}
		}

		//this is the in game properties set up, when you [props on a player you will see Points as one of setable variables on each player

		public int GetPoints(){ return m_Points; }
		public void AddPoint(){ m_Points++; }
		public void SubPoint()
		{
			if(m_Points > minPoints)
			{
				m_Points--;
			}
		}

		//these are the standard adding and subtracting functions for regular combat scenarios

		public void SubPoints(int chunk)
		{
			if((m_Points - chunk) > minPoints)
			{
				m_Points -= chunk;
			}
			else
			{
				m_Points = minPoints;
			}
		}
		public void AddPoints( int chunk )
		{
			m_Points += chunk;
		}

		//these two functions are for later when we impliment being able to officially set a duel for points with other players.  You will be bet how many points (up to as many as you have) will be wagered on the duel.

		public bool HasEnoughPoints(int chunk)
		{
			if((m_Points - chunk) < 0)
			{
				return false;  //does not have enough points
			}
			else
			{
				return true;   //has enough points
			}
		}

		//this is a check for our points vendor, which you can use your points to buy cool stuff from

		public bool HasMoreThanMin(int chunk)
		{
			if((m_Points - chunk) < minPoints)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		//this is a check to see if the player has hit minimum points yet, if he has, no more points will be removed.




		// ************** END OF POINT SYSTEM ******************//
		//*********ADDED MURDERREPORT 5 MIN TIMEOUT*************//
		public class Report
		{
			private Mobile reporter;
			private DateTime reportTime;

			public Report(Mobile reporter, DateTime reportTime)
			{
				this.reporter = reporter;
				this.reportTime = reportTime;
			}
			public Mobile Reporter
			{
				get { return reporter; }
				//set { reporter = value; }
			}
			public DateTime ReportTime
			{
				get { return reportTime; }
				//set { reportTime = value; }
			}
		}


// *** Added for Valor ***
		private DateTime m_LastValorLoss;
		private double m_ValorGain;

		public DateTime LastValorLoss
		{
			get{ return m_LastValorLoss; }
			set{ m_LastValorLoss = value; }
		}

		public double ValorGain
		{
			get{ return m_ValorGain; }
			set{ m_ValorGain = value; }
		}
// *** ***

		private ArrayList reports = new ArrayList();

		public ArrayList Reports
		{
			get { return reports; }
			set { reports = value; }
		}

		//********END OF MURDERREPORT 5 MIN TIMEOUT*************//

		[CommandProperty( AccessLevel.GameMaster )]
		public bool FactionSkillLoss
		{
			get
			{
				return Faction.HasSkillLoss( this );
			}
			set
			{
				if ( value )
					Faction.ApplySkillLoss( this );
				else
					Faction.ClearSkillLoss( this );
			}
		}

		private class CountAndTimeStamp
		{
			private int m_Count;
			private DateTime m_Stamp;

			public CountAndTimeStamp()
			{
			}

			public DateTime TimeStamp { get{ return m_Stamp; } }
			public int Count
			{
				get { return m_Count; }
				set	{ m_Count = value; m_Stamp = DateTime.Now; }
			}
		}

		private DesignContext m_DesignContext;

		private NpcGuild m_NpcGuild;
		private DateTime m_NpcGuildJoinTime;
		private TimeSpan m_NpcGuildGameTime;
		private PlayerFlag m_Flags;
		private int m_StepsTaken;
		private int m_Profession;


		private int m_TotalPoints;
		private int m_TotalWins;
		private int m_TotalLoses;
		private int m_TotalResKills;
		private int m_TotalResKilled;
		private Mobile m_LastPwner;
		private Mobile m_LastPwned;
		private DateTime m_ResKillTime;
		private int m_TotalPointsLost;
		private int m_TotalPointsSpent;
		private string m_PvpRank;

		private DateTime m_LastKill = DateTime.Now;
		private int m_Rank;
		private bool m_ShowPvpTitle;
		private bool m_IsResKillProtected;

		private Mobile m_LastToDamage;

		[CommandProperty( AccessLevel.GameMaster )]
		public int Profession
		{
			get{ return m_Profession; }
			set{ m_Profession = value; }
		}

		public int StepsTaken
		{
			get{ return m_StepsTaken; }
			set{ m_StepsTaken = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public NpcGuild NpcGuild
		{
			get{ return m_NpcGuild; }
			set{ m_NpcGuild = value; }
		}

		[CommandProperty( AccessLevel.Administrator )]
		public bool HasDonated
		{
			get
			{
				DateTime DonationStart;
				TimeSpan DonationDuration;

				try
				{
					DonationStart = DateTime.Parse( ((Account)this.Account).GetTag( "DonationStart" ) );
					DonationDuration = TimeSpan.Parse( ((Account)this.Account).GetTag( "DonationDuration" ) );
				}
				catch
				{
					return false;
				}

				return ( ( DateTime.Now - DonationStart ) < DonationDuration ) || AccessLevel >= AccessLevel.GameMaster;
			}
		}

		[CommandProperty( AccessLevel.Administrator )]
		public TimeSpan DonationDuration
		{
			get
			{
				TimeSpan donationduration;

				try
				{
					donationduration = TimeSpan.Parse( ((Account)this.Account).GetTag( "DonationDuration" ) );
				}
				catch
				{
					return TimeSpan.Zero;
				}

				return donationduration;
			}
			set
			{
				try
				{
					((Account)this.Account).SetTag( "DonationDuration", value.ToString() );
				}
				catch
				{
				}
			}
		}

		[CommandProperty( AccessLevel.Administrator )]
		public DateTime DonationStart
		{
			get
			{
				DateTime donationstart;

				try
				{
					donationstart = DateTime.Parse( ((Account)this.Account).GetTag( "DonationStart" ) );
				}
				catch
				{
					return DateTime.MinValue;
				}

				return donationstart;
			}
			set
			{
				try
				{
					((Account)this.Account).SetTag( "DonationStart", value.ToString() );
				}
				catch
				{
				}
			}
		}

		[CommandProperty( AccessLevel.Administrator )]
		public TimeSpan DonationTimeLeft
		{
			get
			{
				DateTime donationstart;
				TimeSpan donationduration;

				try
				{
					donationstart = DateTime.Parse( ((Account)this.Account).GetTag( "DonationStart" ) );
					donationduration = TimeSpan.Parse( ((Account)this.Account).GetTag( "DonationDuration" ) );
				}
				catch
				{
					return TimeSpan.Zero;
				}

				if ( donationduration == TimeSpan.MaxValue )
					return TimeSpan.MaxValue;

				return donationstart + donationduration - DateTime.Now;

				//TimeSpan timeleft = donationstart + donationduration - DateTime.Now;
				//if ( timeleft < TimeSpan.Zero )
				//	return TimeSpan.Zero;
				//else
				//	return timeleft;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime NpcGuildJoinTime
		{
			get{ return m_NpcGuildJoinTime; }
			set{ m_NpcGuildJoinTime = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan NpcGuildGameTime
		{
			get{ return m_NpcGuildGameTime; }
			set{ m_NpcGuildGameTime = value; }
		}

		public PlayerFlag Flags
		{
			get{ return m_Flags; }
			set{ m_Flags = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool PagingSquelched
		{
			get{ return GetFlag( PlayerFlag.PagingSquelched ); }
            set { SetFlag(PlayerFlag.PagingSquelched, value); m_pagingSquelchedTill = DateTime.MinValue; }
        }

        #region Temporary Paging Squelch
        //Intentionally not saved, temporary pagingsquelches are removed upon restart.
        private DateTime m_pagingSquelchedTill = DateTime.MinValue;
        public DateTime PagingSquelchedTill
        {
            get
            {
                if (DateTime.Now > m_pagingSquelchedTill)
                    m_pagingSquelchedTill = DateTime.MinValue;
                return m_pagingSquelchedTill;
            }
        }

        [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
        public TimeSpan PagingSquelchedFor
        {
            //Showing TimeSpan.Zero is just a cosmetic fix
            get { return (PagingSquelchedTill!=DateTime.MinValue) ? PagingSquelchedTill - DateTime.Now : TimeSpan.Zero; }
            set { m_pagingSquelchedTill = DateTime.Now + value; }
        }
        #endregion

        #region Temporary Squelch
        //Intentionally not saved, temporary squelches are removed upon restart.
        private DateTime m_squelchedTill = DateTime.MinValue;
        public DateTime SquelchedTill
        {
            get
            {
                if (DateTime.Now > m_squelchedTill)
                    m_squelchedTill = DateTime.MinValue;
                return m_squelchedTill;
            }
        }

        [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
        public TimeSpan SquelchedFor
        {
            //Showing TimeSpan.Zero is just a cosmetic fix
            get { return (SquelchedTill != DateTime.MinValue) ? SquelchedTill - DateTime.Now : TimeSpan.Zero; }
            set { m_squelchedTill = DateTime.Now + value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public new bool Squelched
        {
            get { return base.Squelched; }
            set { base.Squelched = value; m_squelchedTill = DateTime.MinValue; }
        }
        #endregion

        [CommandProperty( AccessLevel.GameMaster )]
		public bool Glassblowing
		{
			get{ return GetFlag( PlayerFlag.Glassblowing ); }
			set{ SetFlag( PlayerFlag.Glassblowing, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Masonry
		{
			get{ return GetFlag( PlayerFlag.Masonry ); }
			set{ SetFlag( PlayerFlag.Masonry, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool SandMining
		{
			get{ return GetFlag( PlayerFlag.SandMining ); }
			set{ SetFlag( PlayerFlag.SandMining, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool StoneMining
		{
			get{ return GetFlag( PlayerFlag.StoneMining ); }
			set{ SetFlag( PlayerFlag.StoneMining, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool ToggleMiningStone
		{
			get{ return GetFlag( PlayerFlag.ToggleMiningStone ); }
			set{ SetFlag( PlayerFlag.ToggleMiningStone, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool KarmaLocked
		{
			get{ return GetFlag( PlayerFlag.KarmaLocked ); }
			set{ SetFlag( PlayerFlag.KarmaLocked, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool AutoRenewInsurance
		{
			get{ return GetFlag( PlayerFlag.AutoRenewInsurance ); }
			set{ SetFlag( PlayerFlag.AutoRenewInsurance, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool UseOwnFilter
		{
			get{ return GetFlag( PlayerFlag.UseOwnFilter ); }
			set{ SetFlag( PlayerFlag.UseOwnFilter, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool PublicMyRunUO
		{
			get{ return GetFlag( PlayerFlag.PublicMyRunUO ); }
			set{ SetFlag( PlayerFlag.PublicMyRunUO, value ); InvalidateMyRunUO(); }
		}

		public static Direction GetDirection4( Point3D from, Point3D to )
		{
			int dx = from.X - to.X;
			int dy = from.Y - to.Y;

			int rx = dx - dy;
			int ry = dx + dy;

			Direction ret;

			if ( rx >= 0 && ry >= 0 )
				ret = Direction.West;
			else if ( rx >= 0 && ry < 0 )
				ret = Direction.South;
			else if ( rx < 0 && ry < 0 )
				ret = Direction.East;
			else
				ret = Direction.North;

			return ret;
		}

		  public override bool OnDragLift(Item item)
		  {
				//Al: Additional logging
				if (AccessLevel >= AccessLevel.GameMaster && item.RootParent != null)
				{
					 if (item.RootParent is PlayerMobile && ((PlayerMobile)item.RootParent) != this)
						  CommandLogging.WriteLine(this, "{0} {1} lifting item from character: {2}, amount: {3}, owner: {4}", this.AccessLevel, CommandLogging.Format(this), CommandLogging.Format(item), item.Amount, CommandLogging.Format(((PlayerMobile)item.RootParent)));
					 else if (item.RootParent is Container && ((Container)item.RootParent).IsSecure)
						  CommandLogging.WriteLine(this, "{0} {1} lifting item in secure container: {2}, amount: {3}, container: {4}", this.AccessLevel, CommandLogging.Format(this), CommandLogging.Format(item), item.Amount, CommandLogging.Format(item.Parent));
				}
				return base.OnDragLift(item);
		  }

		public override bool OnDroppedItemToWorld( Item item, Point3D location )
		{
			if ( !base.OnDroppedItemToWorld( item, location ) )
				return false;

			BounceInfo bi = item.GetBounce();

			if ( bi != null )
			{
				Type type = item.GetType();

				if ( type.IsDefined( typeof( FurnitureAttribute ), true ) || type.IsDefined( typeof( DynamicFlipingAttribute ), true ) )
				{
					object[] objs = type.GetCustomAttributes( typeof( FlipableAttribute ), true );

					if ( objs != null && objs.Length > 0 )
					{
						FlipableAttribute fp = objs[0] as FlipableAttribute;

						if ( fp != null )
						{
							int[] itemIDs = fp.ItemIDs;

							Point3D oldWorldLoc = bi.m_WorldLoc;
							Point3D newWorldLoc = location;

							if ( oldWorldLoc.X != newWorldLoc.X || oldWorldLoc.Y != newWorldLoc.Y )
							{
								Direction dir = GetDirection4( oldWorldLoc, newWorldLoc );

								if ( itemIDs.Length == 2 )
								{
									switch ( dir )
									{
										case Direction.North:
										case Direction.South: item.ItemID = itemIDs[0]; break;
										case Direction.East:
										case Direction.West: item.ItemID = itemIDs[1]; break;
									}
								}
								else if ( itemIDs.Length == 4 )
								{
									switch ( dir )
									{
										case Direction.South: item.ItemID = itemIDs[0]; break;
										case Direction.East: item.ItemID = itemIDs[1]; break;
										case Direction.North: item.ItemID = itemIDs[2]; break;
										case Direction.West: item.ItemID = itemIDs[3]; break;
									}
								}
							}
						}
					}
				}
			}

			return true;
		}

		public bool GetFlag( PlayerFlag flag )
		{
			return ( (m_Flags & flag) != 0 );
		}

		public void SetFlag( PlayerFlag flag, bool value )
		{
			if ( value )
				m_Flags |= flag;
			else
				m_Flags &= ~flag;
		}

		public DesignContext DesignContext
		{
			get{ return m_DesignContext; }
			set{ m_DesignContext = value; }
		}

		public static void Initialize()
		{
			if ( FastwalkPrevention )
			{
				PacketHandler ph = PacketHandlers.GetHandler( 0x02 );

				ph.ThrottleCallback = new ThrottlePacketCallback( MovementThrottle_Callback );
			}

			EventSink.Login += new LoginEventHandler( OnLogin );
			EventSink.Logout += new LogoutEventHandler( OnLogout );
			EventSink.Connected += new ConnectedEventHandler( EventSink_Connected );
			EventSink.Disconnected += new DisconnectedEventHandler( EventSink_Disconnected );
		}

		public override void OnSkillInvalidated( Skill skill )
		{
			if ( Core.AOS && skill.SkillName == SkillName.MagicResist )
				UpdateResistances();
		}

		public override int GetMaxResistance( ResistanceType type )
		{
			int max = base.GetMaxResistance( type );

			if ( type != ResistanceType.Physical && 60 < max && Spells.Fourth.CurseSpell.UnderEffect( this ) )
				max = 60;

			return max;
		}

		private int m_LastGlobalLight = -1, m_LastPersonalLight = -1;

		public override void OnNetStateChanged()
		{
			m_LastGlobalLight = -1;
			m_LastPersonalLight = -1;
		}

		//XLX added.
		private ArrayList m_Cowards;
		private ExpireCowardsTimer m_ExpireCowardsTimer;

		public ArrayList Cowards
		{
			get { return m_Cowards; }
		}

		public void AddCoward( PlayerMobile coward, int notoriety )
		{
			bool addCoward = true;

			ArrayList list = m_Cowards;
			CowardiceInfo info;

			for ( int i = 0; i < list.Count; ++i )
			{
				info = (CowardiceInfo)list[i];

				if ( info.Coward == coward )
				{
					info.Notoriety = notoriety;
					info.Refresh();

					addCoward = false;
				}
			}

			if ( addCoward )
			{
				m_Cowards.Add( CowardiceInfo.Create( coward, notoriety ) );

				UpdateCowardExpire();

				if ( this.CanSee( coward ) && NetState != null )
					NetState.Send( new MobileIncoming( this, coward ) );
			}
		}

		public override bool IsBeneficialCriminal( Mobile target )
		{
			int n = Notoriety.Compute( this, target );

			if ( n == Notoriety.CanBeAttacked )
				target.Delta( MobileDelta.Noto );

			if ( this == target )
				return false;

			return ( n == Notoriety.Criminal || ( !target.Player && ( n == Notoriety.CanBeAttacked || n == Notoriety.Murderer ) ) );
		}

		public override void OnBeneficialAction( Mobile target, bool isCriminal )
		{
			PlayerMobile pm = target as PlayerMobile;

			if ( target != this && !isCriminal )
			{
				int notoriety;
				if ( pm != null )
				{
					ArrayList list = target.Aggressors;
					AggressorInfo info;
					PlayerMobile hero;
					int cowardCount = 0;

					for ( int i = 0; i < list.Count; ++i )
					{
						info = (AggressorInfo)list[i];

						if( !(info.Attacker is PlayerMobile) )
						  continue;

						hero = (PlayerMobile)(info.Attacker);

						notoriety = NotorietyHandlers.MobileNotoriety(hero, target);

						if ( notoriety == Notoriety.CanBeAttacked || notoriety == Notoriety.Enemy )
						{
						  if(hero == this)
							 continue;

						  hero.AddCoward( this, notoriety );
						  cowardCount++;
						}
					}

					list = target.Aggressed;

					for ( int i = 0; i < list.Count; ++i )
					{
						info = (AggressorInfo)list[i];

						if( !(info.Defender is PlayerMobile) )
						{
						  continue;
						}

						hero = (PlayerMobile)(info.Defender);

						notoriety = NotorietyHandlers.MobileNotoriety(hero, target);

						if ( notoriety == Notoriety.CanBeAttacked || notoriety == Notoriety.Enemy )
						{
						  if(hero == this)
							 continue;

						  hero.AddCoward( this, notoriety );
						  cowardCount++;
						}
					}

					string battleGroup;

					if(cowardCount > 5)
					{
						battleGroup = "wars";
					}
					else if(cowardCount > 1)
					{
						battleGroup = "enemies";
					}
					else if(cowardCount > 0)
					{
						battleGroup = "foe";
					}
					else
					{
						battleGroup = "imagination"; // No one was attacking him
					}

					if(cowardCount > 0)
					{
						LocalOverheadMessage(MessageType.Regular, 0x22, false, "Benefiting " + target.Name + " has forfeited your innocence to his " + battleGroup + ".");
					}
				}

				notoriety = NotorietyHandlers.MobileNotoriety( this, target );

				if ( Kills < 5 && !Criminal && notoriety == Notoriety.Murderer )
					Attackable = true;
			}

			base.OnBeneficialAction(target, isCriminal);
		}

		private void UpdateCowardExpire()
		{
			if ( Deleted || (m_Cowards.Count == 0) )
			{
				StopCowardExpire();
			}
			else if ( m_ExpireCowardsTimer == null )
			{
				m_ExpireCowardsTimer = new ExpireCowardsTimer( this );
				m_ExpireCowardsTimer.Start();
			}
		}

		private void StopCowardExpire()
		{
			if ( m_ExpireCowardsTimer != null )
				m_ExpireCowardsTimer.Stop();

			m_ExpireCowardsTimer = null;
		}

		private void CheckCowardExpire()
		{
			for ( int i = m_Cowards.Count - 1; i >= 0; --i )
			{
				if ( i >= m_Cowards.Count )
					continue;

				CowardiceInfo info = (CowardiceInfo)m_Cowards[i];

				if ( info.Expired )
				{
					PlayerMobile coward = (PlayerMobile)info.Coward;

					m_Cowards.RemoveAt( i );
					info.Free();

					if ( NetState != null && this.CanSee( coward ) && Utility.InUpdateRange( Location, coward.Location ) )
						NetState.Send( new MobileIncoming( this, coward ) );
				}
			}

			UpdateCowardExpire();
		}

		private class ExpireCowardsTimer : Timer
		{
			private PlayerMobile m_Mobile;

			public ExpireCowardsTimer( PlayerMobile m ) : base( TimeSpan.FromSeconds( 5.0 ), TimeSpan.FromSeconds( 5.0 ) )
			{
				m_Mobile = m;
				Priority = TimerPriority.FiveSeconds;
			}

			protected override void OnTick()
			{
				if ( m_Mobile.Deleted || (m_Mobile.Cowards.Count == 0) )
					m_Mobile.StopCowardExpire();
				else
					m_Mobile.CheckCowardExpire();
			}
		}
//XLX End.

		public override void ComputeBaseLightLevels( out int global, out int personal )
		{
			global = LightCycle.ComputeLevelFor( this );

			if ( this.LightLevel < 21 && AosAttributes.GetValue( this, AosAttribute.NightSight ) > 0 )
				personal = 21;
			else
				personal = this.LightLevel;

			//Always light for staff members
			if (AccessLevel >= AccessLevel.Counselor) personal = 50;
		}

		public override void CheckLightLevels( bool forceResend )
		{
			NetState ns = this.NetState;

			if ( ns == null )
				return;

			int global, personal;

			ComputeLightLevels( out global, out personal );

			if ( !forceResend )
				forceResend = ( global != m_LastGlobalLight || personal != m_LastPersonalLight );

			if ( !forceResend )
				return;

			m_LastGlobalLight = global;
			m_LastPersonalLight = personal;

			ns.Send( GlobalLightLevel.Instantiate( global ) );
			ns.Send( new PersonalLightLevel( this, personal ) );
		}

		public override int GetMinResistance( ResistanceType type )
		{
			int magicResist = (int)(Skills[SkillName.MagicResist].Value * 10);
			int min = int.MinValue;

			if ( magicResist >= 1000 )
				min = 40 + ((magicResist - 1000) / 50);
			else if ( magicResist >= 400 )
				min = (magicResist - 400) / 15;

			if ( min > MaxPlayerResistance )
				min = MaxPlayerResistance;

			int baseMin = base.GetMinResistance( type );

			if ( min < baseMin )
				min = baseMin;

			return min;
		}

					public override bool RetainPackLocsOnDeath
				{
			 get{ return true; }
				}

		private static void OnLogin( LoginEventArgs e )
		{
			Mobile from = e.Mobile;

			SacrificeVirtue.CheckAtrophy( from );
			JusticeVirtue.CheckAtrophy( from );
			CompassionVirtue.CheckAtrophy( from );

			//*****Added for valor
			ValorVirtue.CheckAtrophy( from );
			//*****Added for valor

			if ( AccountHandler.LockdownLevel > AccessLevel.Player )
			{
				string notice;

				Accounting.Account acct = from.Account as Accounting.Account;

				if ( acct == null || !acct.HasAccess( from.NetState ) )
				{
					if ( from.AccessLevel == AccessLevel.Player )
						notice = "The server is currently under lockdown. No players are allowed to log in at this time.";
					else
						notice = "The server is currently under lockdown. You do not have sufficient access level to connect.";

					Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( Disconnect ), from );
				}
				else if ( from.AccessLevel == AccessLevel.Administrator )
				{
					notice = "The server is currently under lockdown. As you are an administrator, you may change this from the [Admin gump.";
				}
				else
				{
					notice = "The server is currently under lockdown. You have sufficient access level to connect.";
				}

				from.SendGump( new NoticeGump( 1060637, 30720, notice, 0xFFC000, 300, 140, null, null ) );
			}
		}

		private bool m_NoDeltaRecursion;

		public void ValidateEquipment()
		{
			if ( m_NoDeltaRecursion || Map == null || Map == Map.Internal )
				return;

			if ( this.Items == null )
				return;

			m_NoDeltaRecursion = true;
			Timer.DelayCall( TimeSpan.Zero, new TimerCallback( ValidateEquipment_Sandbox ) );
		}

		private void ValidateEquipment_Sandbox()
		{
			try
			{
				if ( Map == null || Map == Map.Internal )
					return;

				ArrayList items = this.Items;

				if ( items == null )
					return;

				bool moved = false;

				int str = this.Str;
				int dex = this.Dex;
				int intel = this.Int;

				#region Factions
				int factionItemCount = 0;
				#endregion

				Mobile from = this;

				for ( int i = items.Count - 1; i >= 0; --i )
				{
					if ( i >= items.Count )
						continue;

					Item item = (Item)items[i];

					if ( item is BaseWeapon )
					{
						BaseWeapon weapon = (BaseWeapon)item;

						bool drop = false;

						if ( dex < weapon.DexRequirement )
							drop = true;
						else if ( str < AOS.Scale( weapon.StrRequirement, 100 - weapon.GetLowerStatReq() ) )
							drop = true;
						else if ( intel < weapon.IntRequirement )
							drop = true;

						if ( drop )
						{
							string name = weapon.Name;

							if ( name == null )
								name = String.Format( "#{0}", weapon.LabelNumber );

							from.SendLocalizedMessage( 1062001, name ); // You can no longer wield your ~1_WEAPON~
							from.AddToBackpack( weapon );
							moved = true;
						}
					}
					else if ( item is BaseArmor )
					{
						BaseArmor armor = (BaseArmor)item;

						bool drop = false;

						if ( !armor.AllowMaleWearer && from.Body.IsMale && from.AccessLevel < AccessLevel.GameMaster )
						{
							drop = true;
						}
						else if ( !armor.AllowFemaleWearer && from.Body.IsFemale && from.AccessLevel < AccessLevel.GameMaster )
						{
							drop = true;
						}
						else
						{
							int strBonus = armor.ComputeStatBonus( StatType.Str ), strReq = armor.ComputeStatReq( StatType.Str );
							int dexBonus = armor.ComputeStatBonus( StatType.Dex ), dexReq = armor.ComputeStatReq( StatType.Dex );
							int intBonus = armor.ComputeStatBonus( StatType.Int ), intReq = armor.ComputeStatReq( StatType.Int );

							if ( dex < dexReq || (dex + dexBonus) < 1 )
								drop = true;
							else if ( str < strReq || (str + strBonus) < 1 )
								drop = true;
							else if ( intel < intReq || (intel + intBonus) < 1 )
								drop = true;
						}

						if ( drop )
						{
							string name = armor.Name;

							if ( name == null )
								name = String.Format( "#{0}", armor.LabelNumber );

							if ( armor is BaseShield )
								from.SendLocalizedMessage( 1062003, name ); // You can no longer equip your ~1_SHIELD~
							else
								from.SendLocalizedMessage( 1062002, name ); // You can no longer wear your ~1_ARMOR~

							from.AddToBackpack( armor );
							moved = true;
						}
					}

					FactionItem factionItem = FactionItem.Find( item );

					if ( factionItem != null )
					{
						bool drop = false;

						Faction ourFaction = Faction.Find( this );

						if ( ourFaction == null || ourFaction != factionItem.Faction )
							drop = true;
						else if ( ++factionItemCount > FactionItem.GetMaxWearables( this ) )
							drop = true;

						if ( drop )
						{
							from.AddToBackpack( item );
							moved = true;
						}
					}
				}

				if ( moved )
					from.SendLocalizedMessage( 500647 ); // Some equipment has been moved to your backpack.
			}
			catch ( Exception e )
			{
				Console.WriteLine( e );
			}
			finally
			{
				m_NoDeltaRecursion = false;
			}
		}

		public override void Delta( MobileDelta flag )
		{
			base.Delta( flag );

			if ( (flag & MobileDelta.Stat) != 0 )
				ValidateEquipment();

			if ( (flag & (MobileDelta.Name | MobileDelta.Hue)) != 0 )
				InvalidateMyRunUO();
		}

		private static void Disconnect( object state )
		{
			NetState ns = ((Mobile)state).NetState;

			if ( ns != null )
				ns.Dispose();
		}

		private static void OnLogout( LogoutEventArgs e )
		{
		}

		private static void EventSink_Connected( ConnectedEventArgs e )
		{
			PlayerMobile pm = e.Mobile as PlayerMobile;

			if ( pm != null )
			{
				pm.m_SessionStart = DateTime.Now;

				if ( pm.m_Quest != null )
					pm.m_Quest.StartTimer();

				pm.BedrollLogout = false;
			}
		}

		private static void EventSink_Disconnected( DisconnectedEventArgs e )
		{
			Mobile from = e.Mobile;
			DesignContext context = DesignContext.Find( from );

			if ( context != null )
			{
				/* Client disconnected
				 *  - Remove design context
				 *  - Eject all from house
				 *  - Restore relocated entities
				 */

				// Remove design context
				DesignContext.Remove( from );

				// Eject all from house
				from.RevealingAction();

				foreach ( Item item in context.Foundation.GetItems() )
					item.Location = context.Foundation.BanLocation;

				foreach ( Mobile mobile in context.Foundation.GetMobiles() )
					mobile.Location = context.Foundation.BanLocation;

				// Restore relocated entities
				context.Foundation.RestoreRelocatedEntities();
			}

			PlayerMobile pm = e.Mobile as PlayerMobile;

			if ( pm != null )
			{
				pm.m_GameTime += (DateTime.Now - pm.m_SessionStart);

				if ( pm.m_Quest != null )
					pm.m_Quest.StopTimer();

				pm.m_SpeechLog = null;
			}
		}

		public override void RevealingAction()
		{
			if ( m_DesignContext != null )
				return;

			Spells.Sixth.InvisibilitySpell.RemoveTimer( this );

			base.RevealingAction();
		}

        public override void OnSaid(SpeechEventArgs e)
        {
            if (SquelchedTill != DateTime.MinValue)
            {
                this.SendLocalizedMessage(500168); // You can not say anything, you have been squelched.
                this.SendMessage("Time remaining: {0}.", Misc.AutoRestart.FormatTimeSpan(SquelchedTill - DateTime.Now).ToString());
                e.Blocked = true;
            }
            else
                base.OnSaid(e);
        }

		public override void OnSubItemAdded( Item item )
		{
			if ( AccessLevel < AccessLevel.GameMaster && item.IsChildOf( this.Backpack ) )
			{
				int maxWeight = WeightOverloading.GetMaxWeight( this );
				int curWeight = Mobile.BodyWeight + this.TotalWeight;

				if ( curWeight > maxWeight )
					this.SendLocalizedMessage( 1019035, true, String.Format( " : {0} / {1}", curWeight, maxWeight ) );
			}
		}

		public override bool CanBeHarmful( Mobile target, bool message, bool ignoreOurBlessedness )
		{
			if ( m_DesignContext != null || (target is PlayerMobile && ((PlayerMobile)target).m_DesignContext != null) )
				return false;

			if ( (target is BaseVendor && ((BaseVendor)target).IsInvulnerable) || target is PlayerVendor || target is TownCrier )
			{
				if ( message )
				{
					if ( target.Title == null )
						SendMessage( "{0} the vendor cannot be harmed.", target.Name );
					else
						SendMessage( "{0} {1} cannot be harmed.", target.Name, target.Title );
				}

				return false;
			}

			return base.CanBeHarmful( target, message, ignoreOurBlessedness );
		}

		public override bool CanBeBeneficial( Mobile target, bool message, bool allowDead )
		{
			if ( m_DesignContext != null || (target is PlayerMobile && ((PlayerMobile)target).m_DesignContext != null) )
				return false;

			return base.CanBeBeneficial( target, message, allowDead );
		}

		public override bool CheckContextMenuDisplay( IEntity target )
		{
			return ( m_DesignContext == null );
		}

		public override void OnItemAdded( Item item )
		{
			base.OnItemAdded( item );

			if ( item is BaseArmor || item is BaseWeapon )
			{
				Hits=Hits; Stam=Stam; Mana=Mana;
			}

			if ( this.NetState != null )
				CheckLightLevels( false );

			InvalidateMyRunUO();
		}

		public override void OnItemRemoved( Item item )
		{
			base.OnItemRemoved( item );

			if ( item is BaseArmor || item is BaseWeapon )
			{
				Hits=Hits; Stam=Stam; Mana=Mana;
			}

			if ( this.NetState != null )
				CheckLightLevels( false );

			InvalidateMyRunUO();
		}

		public override double ArmorRating
		{
			get
			{
				BaseArmor ar;
				double rating = 0.0;

				ar = NeckArmor as BaseArmor;
				if ( ar != null )
					rating += ar.ArmorRatingScaled;

				ar = HandArmor as BaseArmor;
				if ( ar != null )
					rating += ar.ArmorRatingScaled;

				ar = HeadArmor as BaseArmor;
				if ( ar != null )
					rating += ar.ArmorRatingScaled;

				ar = ArmsArmor as BaseArmor;
				if ( ar != null )
					rating += ar.ArmorRatingScaled;

				ar = LegsArmor as BaseArmor;
				if ( ar != null )
					rating += ar.ArmorRatingScaled;

				ar = ChestArmor as BaseArmor;
				if ( ar != null )
					rating += ar.ArmorRatingScaled;

				ar = ShieldArmor as BaseArmor;
				if ( ar != null )
					rating += ar.ArmorRatingScaled;

				return VirtualArmor + VirtualArmorMod + rating;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public override int HitsMax
		{
			get
			{
				int strBase;
				int strOffs = GetStatOffset( StatType.Str );

				if ( Core.AOS )
				{
					strBase = this.Str;
					strOffs += AosAttributes.GetValue( this, AosAttribute.BonusHits );
				}
				else
				{
					strBase = this.RawStr;
				}

				return (strBase / 2) + 50 + strOffs;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public override int StamMax
		{
			get{ return base.StamMax + AosAttributes.GetValue( this, AosAttribute.BonusStam ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public override int ManaMax
		{
			get{ return base.ManaMax + AosAttributes.GetValue( this, AosAttribute.BonusMana ); }
		}

		public override bool Move( Direction d )
		{
			NetState ns = this.NetState;

			if ( ns != null )
			{
				GumpCollection gumps = ns.Gumps;

				for ( int i = 0; i < gumps.Count; ++i )
				{
					if ( gumps[i] is ResurrectGump )
					{
						if ( Alive )
						{
							CloseGump( typeof( ResurrectGump ) );
						}
						else
						{
							SendLocalizedMessage( 500111 ); // You are frozen and cannot move.
							return false;
						}
					}
				}
			}

			TimeSpan speed = ComputeMovementSpeed( d );

			if ( !base.Move( d ) )
				return false;

			m_NextMovementTime += speed;

			return true;
		}

		public override bool CheckMovement( Direction d, out int newZ )
		{
				// ADDED FOR FUN. NOT SUPPOSED TO BE PERMANENT (Ridable dragon... sort of)
				if (Mounted && Mount != null && Mount is EtherealSpecial)
				{
					 EtherealSpecial e = Mount as EtherealSpecial;
					 newZ = e.Move(this);
					 return true;
				}
				// END ADDED FOR FUN




				DesignContext context = m_DesignContext;

			if ( context == null )
				return base.CheckMovement( d, out newZ );

			HouseFoundation foundation = context.Foundation;

			newZ = foundation.Z + HouseFoundation.GetLevelZ( context.Level );

			int newX = this.X, newY = this.Y;
			Movement.Movement.Offset( d, ref newX, ref newY );

			int startX = foundation.X + foundation.Components.Min.X + 1;
			int startY = foundation.Y + foundation.Components.Min.Y + 1;
			int endX = startX + foundation.Components.Width - 1;
			int endY = startY + foundation.Components.Height - 2;

			return ( newX >= startX && newY >= startY && newX < endX && newY < endY && Map == foundation.Map );
		}

		public override bool AllowItemUse( Item item )
		{
			return DesignContext.Check( this );
		}

		public override bool AllowSkillUse( SkillName skill )
		{
			return DesignContext.Check( this );
		}

		private bool m_LastProtectedMessage;
		private int m_NextProtectionCheck = 10;

		public virtual void RecheckTownProtection()
		{
			m_NextProtectionCheck = 10;

			Regions.GuardedRegion reg = this.Region as Regions.GuardedRegion;
			bool isProtected = ( reg != null && !reg.IsDisabled() );

			if ( isProtected != m_LastProtectedMessage )
			{
				if ( isProtected )
					SendLocalizedMessage( 500112 ); // You are now under the protection of the town guards.
				else
					SendLocalizedMessage( 500113 ); // You have left the protection of the town guards.

				m_LastProtectedMessage = isProtected;
			}
		}

		public override void MoveToWorld( Point3D loc, Map map )
		{
			base.MoveToWorld( loc, map );

			RecheckTownProtection();
		}

		public override void SetLocation( Point3D loc, bool isTeleport )
		{
			if ( !isTeleport && AccessLevel == AccessLevel.Player )
			{
				// moving, not teleporting
				int zDrop = ( this.Location.Z - loc.Z );

				if ( zDrop > 20 ) // we fell more than one story
					Hits -= ((zDrop / 20) * 10) - 5; // deal some damage; does not kill, disrupt, etc
			}

			base.SetLocation( loc, isTeleport );

			if ( isTeleport || --m_NextProtectionCheck == 0 )
				RecheckTownProtection();
		}

		public override void GetContextMenuEntries( Mobile from, ArrayList list )
		{
			base.GetContextMenuEntries( from, list );

			if ( from == this )
			{
				if ( m_Quest != null )
					m_Quest.GetContextMenuEntries( list );

				if ( Alive && InsuranceEnabled )
				{
					list.Add( new CallbackEntry( 6201, new ContextCallback( ToggleItemInsurance ) ) );

					if ( AutoRenewInsurance )
						list.Add( new CallbackEntry( 6202, new ContextCallback( CancelRenewInventoryInsurance ) ) );
					else
						list.Add( new CallbackEntry( 6200, new ContextCallback( AutoRenewInventoryInsurance ) ) );
				}

				// TODO: Toggle champ titles

				BaseHouse house = BaseHouse.FindHouseAt( this );

				if ( house != null )
				{
					if ( Alive && house.InternalizedVendors.Count > 0 && house.IsOwner( this ) )
						list.Add( new CallbackEntry( 6204, new ContextCallback( GetVendor ) ) );

					//if ( house.IsAosRules )
						list.Add( new CallbackEntry( 6207, new ContextCallback( LeaveHouse ) ) );
				}

				if (m_JusticeProtectors != null && m_JusticeProtectors.Count > 0)
					list.Add( new CallbackEntry( 6157, new ContextCallback( CancelProtection ) ) );
			}
		}

		private void CancelProtection()
		{
			if (m_JusticeProtectors == null) return;
			for (int i = 0; i < m_JusticeProtectors.Count; ++i)
			{
				Mobile prot = (Mobile)m_JusticeProtectors[i];

				string args = String.Format( "{0}\t{1}", this.Name, prot.Name );

				prot.SendLocalizedMessage( 1049371, args ); // The protective relationship between ~1_PLAYER1~ and ~2_PLAYER2~ has been ended.
				this.SendLocalizedMessage( 1049371, args ); // The protective relationship between ~1_PLAYER1~ and ~2_PLAYER2~ has been ended.
			}

			m_JusticeProtectors.Clear();
			m_JusticeProtectors = null;
		}

		private void ToggleItemInsurance()
		{
			if ( !CheckAlive() )
				return;

			BeginTarget( -1, false, TargetFlags.None, new TargetCallback( ToggleItemInsurance_Callback ) );
			SendLocalizedMessage( 1060868 ); // Target the item you wish to toggle insurance status on <ESC> to cancel
		}

		private bool CanInsure( Item item )
		{
			if ( item is Container || item is BagOfSending )
				return false;

			if ( item is Spellbook || item is Runebook || item is PotionKeg || item is Sigil )
				return false;

			if ( item.Stackable )
				return false;

			if ( item.LootType == LootType.Cursed )
				return false;

			if ( item.ItemID == 0x204E ) // death shroud
				return false;

			return true;
		}

		private void ToggleItemInsurance_Callback( Mobile from, object obj )
		{
			if ( !CheckAlive() )
				return;

			Item item = obj as Item;

			if ( item == null || !item.IsChildOf( this ) )
			{
				BeginTarget( -1, false, TargetFlags.None, new TargetCallback( ToggleItemInsurance_Callback ) );
				SendLocalizedMessage( 1060871, "", 0x23 ); // You can only insure items that you have equipped or that are in your backpack
			}
			else if ( item.Insured )
			{
				item.Insured = false;

				SendLocalizedMessage( 1060874, "", 0x35 ); // You cancel the insurance on the item

				BeginTarget( -1, false, TargetFlags.None, new TargetCallback( ToggleItemInsurance_Callback ) );
				SendLocalizedMessage( 1060868, "", 0x23 ); // Target the item you wish to toggle insurance status on <ESC> to cancel
			}
			else if ( !CanInsure( item ) )
			{
				BeginTarget( -1, false, TargetFlags.None, new TargetCallback( ToggleItemInsurance_Callback ) );
				SendLocalizedMessage( 1060869, "", 0x23 ); // You cannot insure that
			}
			else if ( item.LootType == LootType.Blessed || item.LootType == LootType.Newbied || item.BlessedFor == from )
			{
				BeginTarget( -1, false, TargetFlags.None, new TargetCallback( ToggleItemInsurance_Callback ) );
				SendLocalizedMessage( 1060870, "", 0x23 ); // That item is blessed and does not need to be insured
				SendLocalizedMessage( 1060869, "", 0x23 ); // You cannot insure that
			}
			else
			{
				if ( !item.PayedInsurance )
				{
					if ( Banker.Withdraw( from, 600 ) )
					{
						SendLocalizedMessage( 1060398, "600" ); // ~1_AMOUNT~ gold has been withdrawn from your bank box.
						item.PayedInsurance = true;
					}
					else
					{
						SendLocalizedMessage( 1061079, "", 0x23 ); // You lack the funds to purchase the insurance
						return;
					}
				}

				item.Insured = true;

				SendLocalizedMessage( 1060873, "", 0x23 ); // You have insured the item

				BeginTarget( -1, false, TargetFlags.None, new TargetCallback( ToggleItemInsurance_Callback ) );
				SendLocalizedMessage( 1060868, "", 0x23 ); // Target the item you wish to toggle insurance status on <ESC> to cancel
			}
		}

		private void AutoRenewInventoryInsurance()
		{
			if ( !CheckAlive() )
				return;

			SendLocalizedMessage( 1060881, "", 0x23 ); // You have selected to automatically reinsure all insured items upon death
			AutoRenewInsurance = true;
		}

		private void CancelRenewInventoryInsurance()
		{
			if ( !CheckAlive() )
				return;

			SendLocalizedMessage( 1061075, "", 0x23 ); // You have cancelled automatically reinsuring all insured items upon death
			AutoRenewInsurance = false;
		}

		// TODO: Champ titles, toggle

		private void GetVendor()
		{
			BaseHouse house = BaseHouse.FindHouseAt( this );

			if ( CheckAlive() && house != null && house.IsOwner( this ) && house.InternalizedVendors.Count > 0 )
			{
				CloseGump( typeof( ReclaimVendorGump ) );
				SendGump( new ReclaimVendorGump( house ) );
			}
		}

		private void LeaveHouse()
		{
			BaseHouse house = BaseHouse.FindHouseAt( this );

			if ( house != null )
				this.Location = house.BanLocation;
		}

		private delegate void ContextCallback();

		private class CallbackEntry : ContextMenuEntry
		{
			private ContextCallback m_Callback;

			public CallbackEntry( int number, ContextCallback callback ) : this( number, -1, callback )
			{
			}

			public CallbackEntry( int number, int range, ContextCallback callback ) : base( number, range )
			{
				m_Callback = callback;
			}

			public override void OnClick()
			{
				if ( m_Callback != null )
					m_Callback();
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( this == from && !Warmode )
			{
				IMount mount = Mount;

				if ( mount != null && !DesignContext.Check( this ) )
					return;
			}

			base.OnDoubleClick( from );
		}

		public override void DisplayPaperdollTo( Mobile to )
		{
			if ( DesignContext.Check( this ) )
				base.DisplayPaperdollTo( to );
		}

		private static bool m_NoRecursion;

		public override bool CheckEquip( Item item )
		{
			if ( !base.CheckEquip( item ) )
				return false;

			#region Factions
			FactionItem factionItem = FactionItem.Find( item );

			if ( factionItem != null )
			{
				Faction faction = Faction.Find( this );

				if ( faction == null )
				{
					SendLocalizedMessage( 1010371 ); // You cannot equip a faction item!
					return false;
				}
				else if ( faction != factionItem.Faction )
				{
					SendLocalizedMessage( 1010372 ); // You cannot equip an opposing faction's item!
					return false;
				}
				else
				{
					int maxWearables = FactionItem.GetMaxWearables( this );

					for ( int i = 0; i < Items.Count; ++i )
					{
						Item equiped = (Item)Items[i];

						if ( item != equiped && FactionItem.Find( equiped ) != null )
						{
							if ( --maxWearables == 0 )
							{
								SendLocalizedMessage( 1010373 ); // You do not have enough rank to equip more faction items!
								return false;
							}
						}
					}
				}
			}
			#endregion

			if ( this.AccessLevel < AccessLevel.GameMaster && item.Layer != Layer.Mount && this.HasTrade )
			{
				BounceInfo bounce = item.GetBounce();

				if ( bounce != null )
				{
					if ( bounce.m_Parent is Item )
					{
						Item parent = (Item) bounce.m_Parent;

						if ( parent == this.Backpack || parent.IsChildOf( this.Backpack ) )
							return true;
					}
					else if ( bounce.m_Parent == this )
					{
						return true;
					}
				}

				SendLocalizedMessage( 1004042 ); // You can only equip what you are already carrying while you have a trade pending.
				return false;
			}

			return true;
		}

		public override bool CheckTrade( Mobile to, Item item, SecureTradeContainer cont, bool message, bool checkItems, int plusItems, int plusWeight )
		{
			int msgNum = 0;

				if (this.Region is CustomRegion && ((CustomRegion)this.Region).CannotTrade)
				{
					 this.SendMessage("You cannot trade here!");
					 return false;
				}

			if ( cont == null )
			{
				if ( to.Holding != null )
					msgNum = 1062727; // You cannot trade with someone who is dragging something.
				else if ( this.HasTrade )
					msgNum = 1062781; // You are already trading with someone else!
				else if ( to.HasTrade )
					msgNum = 1062779; // That person is already involved in a trade
			}

			if ( msgNum == 0 )
			{
				if ( cont != null )
				{
					plusItems += cont.TotalItems;
					plusWeight += cont.TotalWeight;
				}

				if ( this.Backpack == null || !this.Backpack.CheckHold( this, item, false, checkItems, plusItems, plusWeight ) )
					msgNum = 1004040; // You would not be able to hold this if the trade failed.
				else if ( to.Backpack == null || !to.Backpack.CheckHold( to, item, false, checkItems, plusItems, plusWeight ) )
					msgNum = 1004039; // The recipient of this trade would not be able to carry this.
				else
					msgNum = CheckContentForTrade( item );
			}

			if ( msgNum != 0 )
			{
				if ( message )
					this.SendLocalizedMessage( msgNum );

				return false;
			}

			return true;
		}

		private static int CheckContentForTrade( Item item )
		{
			if ( item is TrapableContainer && ((TrapableContainer)item).TrapType != TrapType.None )
				return 1004044; // You may not trade trapped items.

			if ( SkillHandlers.StolenItem.IsStolen( item ) )
				return 1004043; // You may not trade recently stolen items.

			if ( item is Container )
			{
				foreach ( Item subItem in item.Items )
				{
					int msg = CheckContentForTrade( subItem );

					if ( msg != 0 )
						return msg;
				}
			}

			return 0;
		}

		public override bool CheckNonlocalDrop( Mobile from, Item item, Item target )
		{
			if ( !base.CheckNonlocalDrop( from, item, target ) )
				return false;

			if ( from.AccessLevel >= AccessLevel.GameMaster )
				return true;

			Container pack = this.Backpack;
			if ( from == this && this.HasTrade && ( target == pack || target.IsChildOf( pack ) ) )
			{
				BounceInfo bounce = item.GetBounce();

				if ( bounce != null && bounce.m_Parent is Item )
				{
					Item parent = (Item) bounce.m_Parent;

					if ( parent == pack || parent.IsChildOf( pack ) )
						return true;
				}

				SendLocalizedMessage( 1004041 ); // You can't do that while you have a trade pending.
				return false;
			}

			return true;
		}

		protected override void OnLocationChange( Point3D oldLocation )
		{
			CheckLightLevels( false );

			DesignContext context = m_DesignContext;

			if ( context == null || m_NoRecursion )
				return;

			m_NoRecursion = true;

			HouseFoundation foundation = context.Foundation;

			int newX = this.X, newY = this.Y;
			int newZ = foundation.Z + HouseFoundation.GetLevelZ( context.Level );

			int startX = foundation.X + foundation.Components.Min.X + 1;
			int startY = foundation.Y + foundation.Components.Min.Y + 1;
			int endX = startX + foundation.Components.Width - 1;
			int endY = startY + foundation.Components.Height - 2;

			if ( newX >= startX && newY >= startY && newX < endX && newY < endY && Map == foundation.Map )
			{
				if ( Z != newZ )
					Location = new Point3D( X, Y, newZ );

				m_NoRecursion = false;
				return;
			}

			Location = new Point3D( foundation.X, foundation.Y, newZ );
			Map = foundation.Map;

			m_NoRecursion = false;
		}

		public override bool OnMoveOver( Mobile m )
		{
			if ( m is BaseCreature && !((BaseCreature)m).Controlled )
				return ( !Alive || !m.Alive || IsDeadBondedPet || m.IsDeadBondedPet ) || ( Hidden && m.AccessLevel > AccessLevel.Player );

			return base.OnMoveOver( m );
		}

		protected override void OnMapChange( Map oldMap )
		{
			if ( Faction.IsFactionMap(Map) != Faction.IsFactionMap(oldMap))
				InvalidateProperties();

			DesignContext context = m_DesignContext;

			if ( context == null || m_NoRecursion )
				return;

			m_NoRecursion = true;

			HouseFoundation foundation = context.Foundation;

			if ( Map != foundation.Map )
				Map = foundation.Map;

			m_NoRecursion = false;
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			int disruptThreshold;

			if ( !Core.AOS )
				disruptThreshold = 0;
			else if ( from != null && from.Player )
				disruptThreshold = 18;
			else
				disruptThreshold = 25;

			if ( amount > disruptThreshold )
			{
				BandageContext c = BandageContext.GetContext( this );

				if ( c != null )
					c.Slip();
			}

			WeightOverloading.FatigueOnDamage( this, amount );

			// last pvp action update (used in stamsystem)
			if (from is PlayerMobile && this != from)
			{
				PlayerMobile victim = from as PlayerMobile;
				if (StamSystem.active && StamSystem.debug)
				{
					this.SendMessage("PVP timer updated.");
					victim.SendMessage("PVP timer updated.");
				}
					 if (StamSystem.active)
					 {
						  if (m_LastPVP == DateTime.MinValue && Mounted)
								LocalOverheadMessage(MessageType.Regular, 0x49, false, "Your mount is battle weary and can tired.");
						  if (victim.m_LastPVP == DateTime.MinValue && victim.Mounted)
								victim.LocalOverheadMessage(MessageType.Regular, 0x49, false, "Your mount is battle weary and can tired.");
					 }
					 m_LastPVP = DateTime.Now;
					 victim.m_LastPVP = DateTime.Now;
				}

			base.OnDamage( amount, from, willKill );
		}

		public static int ComputeSkillTotal( Mobile m )
		{
			int total = 0;

			for ( int i = 0; i < m.Skills.Length; ++i )
				total += m.Skills[i].BaseFixedPoint;

			return ( total / 10 );
		}

		public override void Resurrect()
		{
			bool wasAlive = this.Alive;

			base.Resurrect();

			if ( this.Alive && !wasAlive )
			{
				//Item deathRobe = new DeathRobe();
				//
				//if ( !EquipItem( deathRobe ) )
				//	deathRobe.Delete();
			}
		}

		private Mobile m_InsuranceAward;
		private int m_InsuranceCost;
		private int m_InsuranceBonus;

		public override bool OnBeforeDeath()
		{
			m_InsuranceCost = 0;
			m_InsuranceAward = base.FindMostRecentDamager( false );

			if ( m_InsuranceAward is BaseCreature )
			{
				Mobile master = ((BaseCreature)m_InsuranceAward).GetMaster();

				if ( master != null )
					m_InsuranceAward = master;
			}

			if ( m_InsuranceAward != null && (!m_InsuranceAward.Player || m_InsuranceAward == this) )
				m_InsuranceAward = null;

			if ( m_InsuranceAward is PlayerMobile )
				((PlayerMobile)m_InsuranceAward).m_InsuranceBonus = 0;

				//Al: Find out if we are in a Custom Region which has disabled PvP counts
				bool isNoPvPPointsCustomRegion = ((this.Region is CustomRegion) && (((CustomRegion)this.Region).NoPvPPoints));

			if ( this.Region is Server.Regions.GameRegion || isNoPvPPointsCustomRegion)
				return base.OnBeforeDeath();
			else
				FSPvpSystem.PvpDeathCheck( this );

			return base.OnBeforeDeath();
		}

		private bool CheckInsuranceOnDeath( Item item )
		{
			if ( InsuranceEnabled && item.Insured )
			{
				if ( AutoRenewInsurance )
				{
					int cost = ( m_InsuranceAward == null ? 600 : 300 );

					if ( Banker.Withdraw( this, cost ) )
					{
						m_InsuranceCost += cost;
						item.PayedInsurance = true;
					}
					else
					{
						SendLocalizedMessage( 1061079, "", 0x23 ); // You lack the funds to purchase the insurance
						item.PayedInsurance = false;
						item.Insured = false;
					}
				}
				else
				{
					item.PayedInsurance = false;
					item.Insured = false;
				}

				if ( m_InsuranceAward != null )
				{
					if ( Banker.Deposit( m_InsuranceAward, 300 ) )
					{
						if ( m_InsuranceAward is PlayerMobile )
							((PlayerMobile)m_InsuranceAward).m_InsuranceBonus += 300;
					}
				}

				return true;
			}

			return false;
		}

		/* Start FSGov Edits
		//private CityManagementStone m_City;
		//private string m_CityTitle;
		//private bool m_ShowCityTitle;
		//private bool m_OwesBackTaxes;
		//private int m_BackTaxesAmount;

		[CommandProperty( AccessLevel.GameMaster )]
		public CityManagementStone City
		{
			get{ return m_City; }
			set{ m_City = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string CityTitle
		{
			get{ return m_CityTitle; }
			set{ m_CityTitle = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool ShowCityTitle
		{
			get{ return m_ShowCityTitle; }
			set{ m_ShowCityTitle = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool OwesBackTaxes
		{
			get{ return m_OwesBackTaxes; }
			set{ m_OwesBackTaxes = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int BackTaxesAmount
		{
			get{ return m_BackTaxesAmount; }
			set{ m_BackTaxesAmount = value; }
		}
		//End FSGov Edits */

		public override DeathMoveResult GetParentMoveResultFor( Item item )
		{
			if ( CheckInsuranceOnDeath( item ) )
				return DeathMoveResult.MoveToBackpack;

			DeathMoveResult res = base.GetParentMoveResultFor( item );

			if ( res == DeathMoveResult.MoveToCorpse && item.Movable && this.Young )
				res = DeathMoveResult.MoveToBackpack;

			return res;
		}

		public override DeathMoveResult GetInventoryMoveResultFor( Item item )
		{
			if ( CheckInsuranceOnDeath( item ) )
				return DeathMoveResult.MoveToBackpack;

			DeathMoveResult res = base.GetInventoryMoveResultFor( item );

			if ( res == DeathMoveResult.MoveToCorpse && item.Movable && this.Young )
				res = DeathMoveResult.MoveToBackpack;

			return res;
		}

		public override void OnDeath( Container c )
		{
			base.OnDeath( c );

			if ( Region is GameRegion || (Region is CustomRegion && ((CustomRegion)Region).DeleteCorpsesOnDeath ))
				c.Delete();

			HueMod = -1;
			NameMod = null;
			SavagePaintExpiration = TimeSpan.Zero;

			SetHairMods( -1, -1 );

			PolymorphSpell.StopTimer( this );
			IncognitoSpell.StopTimer( this );
			DisguiseGump.StopTimer( this );

			EndAction( typeof( PolymorphSpell ) );
			EndAction( typeof( IncognitoSpell ) );

			MeerMage.StopEffect( this, false );

			SkillHandlers.StolenItem.ReturnOnDeath( this, c );

			if (m_PermaFlags != null && m_PermaFlags.Count > 0)
			{
				m_PermaFlags.Clear();
				m_PermaFlags = null;

				if ( c is Corpse )
					((Corpse)c).Criminal = true;

				if ( SkillHandlers.Stealing.ClassicMode )
					Criminal = true;
			}

			if ( this.Kills >= 5 && DateTime.Now >= m_NextJustAward )
			{
				Mobile m = FindMostRecentDamager( false );

				if( m is BaseCreature )
					m = ((BaseCreature)m).GetMaster();

				if ( m != null && m.Player && m != this )
				{
					bool gainedPath = false;

					int theirTotal = ComputeSkillTotal( m );
					int ourTotal = ComputeSkillTotal( this );

					int pointsToGain = 1 + ((theirTotal - ourTotal) / 50);

					if ( pointsToGain < 1 )
						pointsToGain = 1;
					else if ( pointsToGain > 4 )
						pointsToGain = 4;

					if ( VirtueHelper.Award( m, VirtueName.Justice, pointsToGain, ref gainedPath ) )
					{
						if ( gainedPath )
							m.SendLocalizedMessage( 1049367 ); // You have gained a path in Justice!
						else
							m.SendLocalizedMessage( 1049363 ); // You have gained in Justice.

						m.FixedParticles( 0x375A, 9, 20, 5027, EffectLayer.Waist );
						m.PlaySound( 0x1F7 );

						m_NextJustAward = DateTime.Now + TimeSpan.FromMinutes( pointsToGain * 2 );
					}
				}
			}

			if ( m_InsuranceCost > 0 )
				SendLocalizedMessage( 1060398, m_InsuranceCost.ToString() ); // ~1_AMOUNT~ gold has been withdrawn from your bank box.

			if ( m_InsuranceAward is PlayerMobile )
			{
				PlayerMobile pm = (PlayerMobile)m_InsuranceAward;

				if ( pm.m_InsuranceBonus > 0 )
					pm.SendLocalizedMessage( 1060397, pm.m_InsuranceBonus.ToString() ); // ~1_AMOUNT~ gold has been deposited into your bank box.
			}

			Mobile killer = this.FindMostRecentDamager( true );

			if ( killer is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)killer;

				Mobile master = bc.GetMaster();
				if( master != null )
					killer = master;
			}

			if ( this.Young && this.Map != null && this.Map == Map.Felucca ) //Al: Youngs are only teleported in Felucca.
			{
				Point3D dest = GetYoungDeathDestination();

				if ( dest != Point3D.Zero )
				{
					this.Location = dest;
					Timer.DelayCall( TimeSpan.FromSeconds( 2.5 ), new TimerCallback( SendYoungDeathNotice ) );
				}
			}

			Faction.HandleDeath( this, killer );
		}

		private ArrayList m_PermaFlags;
		private ArrayList m_VisList;
		private Hashtable m_AntiMacroTable;
		private TimeSpan m_GameTime;
		private TimeSpan m_ShortTermElapse;
		private TimeSpan m_LongTermElapse;
		private DateTime m_SessionStart;
		private DateTime m_LastEscortTime;
		private DateTime m_NextSmithBulkOrder;
		private DateTime m_NextTailorBulkOrder;
		private DateTime m_SavagePaintExpiration;
		private SkillName m_Learning = (SkillName)(-1);

		public SkillName Learning
		{
			get{ return m_Learning; }
			set{ m_Learning = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan SavagePaintExpiration
		{
			get
			{
				TimeSpan ts = m_SavagePaintExpiration - DateTime.Now;

				if ( ts < TimeSpan.Zero )
					ts = TimeSpan.Zero;

				return ts;
			}
			set
			{
				m_SavagePaintExpiration = DateTime.Now + value;
			}
		}

// *** Added by Eclipse ***
		  #region Hunting Bods
		  private DateTime m_NextHuntContract;
		  [CommandProperty(AccessLevel.GameMaster)]
		  public TimeSpan NextHuntContract
		  {
				get
				{
					 TimeSpan ts = m_NextHuntContract - DateTime.Now;

					 if (ts < TimeSpan.Zero)
						  ts = TimeSpan.Zero;

					 return ts;
				}
				set
				{
					 try { m_NextHuntContract = DateTime.Now + value; }
					 catch { }
				}
		  }
		  #endregion
// *** ***

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan NextSmithBulkOrder
		{
			get
			{
				TimeSpan ts = m_NextSmithBulkOrder - DateTime.Now;

				if ( ts < TimeSpan.Zero )
					ts = TimeSpan.Zero;

				return ts;
			}
			set
			{
				try{ m_NextSmithBulkOrder = DateTime.Now + value; }
				catch{}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan NextTailorBulkOrder
		{
			get
			{
				TimeSpan ts = m_NextTailorBulkOrder - DateTime.Now;

				if ( ts < TimeSpan.Zero )
					ts = TimeSpan.Zero;

				return ts;
			}
			set
			{
				try{ m_NextTailorBulkOrder = DateTime.Now + value; }
				catch{}
			}
		}

		public DateTime LastEscortTime
		{
			get{ return m_LastEscortTime; }
			set{ m_LastEscortTime = value; }
		}

		public PlayerMobile()
		{
				m_Cowards = new ArrayList( 1 ); // XLX added and End.
				m_LastTeleTime = DateTime.Now;

			m_BOBFilter = new Engines.BulkOrders.BOBFilter();

			m_GameTime = TimeSpan.Zero;
			m_ShortTermElapse = TimeSpan.FromHours( 4.0 );
			m_LongTermElapse = TimeSpan.FromHours( 30.0 );

			InvalidateMyRunUO();
		}

		public override bool MutateSpeech( ArrayList hears, ref string text, ref object context )
		{
			if ( Alive )
				return false;

			if ( Core.AOS )
			{
				for ( int i = 0; i < hears.Count; ++i )
				{
					object o = hears[i];

					if ( o != this && o is Mobile && ((Mobile)o).Skills[SkillName.SpiritSpeak].Value >= 100.0 )
						return false;
				}
			}

			return base.MutateSpeech( hears, ref text, ref context );
		}

		public override void Damage( int amount, Mobile from )
		{
			if ( Spells.Necromancy.EvilOmenSpell.CheckEffect( this ) )
				amount = (int)(amount * 1.25);

			Mobile oath = Spells.Necromancy.BloodOathSpell.GetBloodOath( from );

			if ( oath == this )
			{
				amount = (int)(amount * 1.1);
				from.Damage( amount, from );
			}

			// Fix for LichSteed/Evo Dragon damage. By Dev Minkio
			if (from is EvoHiryu)
				 amount = (int)((amount / 2) + 2);
			else if ( ( from is IEvoCreature || from is EvolutionDragon ) )
				amount = (int)(amount * 0.5);
			// ---

			base.Damage( amount, from );
		}

		public override ApplyPoisonResult ApplyPoison( Mobile from, Poison poison )
		{
			if ( !Alive )
				return ApplyPoisonResult.Immune;

			if ( Spells.Necromancy.EvilOmenSpell.CheckEffect( this ) )
				return base.ApplyPoison( from, PoisonImpl.IncreaseLevel( poison ) );

			return base.ApplyPoison( from, poison );
		}

		public override bool CheckPoisonImmunity( Mobile from, Poison poison )
		{
			if ( this.Young )
				return true;

			return base.CheckPoisonImmunity( from, poison );
		}

		public override bool ClickTitle{ get{ return false; } }

		public override void OnPoisonImmunity( Mobile from, Poison poison )
		{
			if ( this.Young )
				SendLocalizedMessage( 502808 ); // You would have been poisoned, were you not new to the land of Britannia. Be careful in the future.
			else
				base.OnPoisonImmunity( from, poison );
		}

		  public override void OnPoisoned(Mobile from, Poison poison, Poison oldPoison)
		  {
				if (poison != null)
				{
					 if (poison.Level >= 0 && poison.Level <= 4)
					 {
						  LocalOverheadMessage(MessageType.Regular, 0x22, 1042857 + (poison.Level * 2));
						  NonlocalOverheadMessage(MessageType.Regular, 0x22, 1042858 + (poison.Level * 2), Name);
					 }
					 else if (poison.Level == 5)
					 {
						  LocalOverheadMessage(MessageType.Regular, 0x22, true, "You are in very extreme pain, and require immediate aid");
						  NonlocalOverheadMessage(MessageType.Regular, 0x22, true, ""+from.Name+"begins to spasm absolutly uncontrollably");
					 }
				}
		  }

		public PlayerMobile( Serial s ) : base( s )
		{
				m_Cowards = new ArrayList( 1 ); // XLX added and end.
				m_LastTeleTime = DateTime.Now;
			InvalidateMyRunUO();
		}

		public ArrayList VisibilityList
		{
				get
				{
					 if (m_VisList == null) m_VisList = new ArrayList(4);
					 return m_VisList;
				}
		  }

		public ArrayList PermaFlags
		{
			get
			{
				if (m_PermaFlags == null)
					m_PermaFlags = new ArrayList(4);
				return m_PermaFlags;
			}
		}

		public override int Luck{ get{ return AosAttributes.GetValue( this, AosAttribute.Luck ); } }

		public override bool IsHarmfulCriminal( Mobile target )
		{
			if ( SkillHandlers.Stealing.ClassicMode && target is PlayerMobile &&
				((PlayerMobile)target).m_PermaFlags != null &&
				((PlayerMobile)target).m_PermaFlags.Count > 0)
			{
				int noto = Notoriety.Compute( this, target );

				if ( noto == Notoriety.Innocent )
					target.Delta( MobileDelta.Noto );

				return false;
			}

			if ( target is BaseCreature && ((BaseCreature)target).InitialInnocent && !((BaseCreature)target).Controlled )
				return false;

			return base.IsHarmfulCriminal( target );
		}

		public bool AntiMacroCheck( Skill skill, object obj )
		{
			if (obj == null || this.AccessLevel != AccessLevel.Player)
				return true;

			if (m_AntiMacroTable == null) m_AntiMacroTable = new Hashtable();

			Hashtable tbl = (Hashtable)m_AntiMacroTable[skill];
			if ( tbl == null )
				m_AntiMacroTable[skill] = tbl = new Hashtable();

			CountAndTimeStamp count = (CountAndTimeStamp)tbl[obj];
			if ( count != null )
			{
				if ( count.TimeStamp + SkillCheck.AntiMacroExpire <= DateTime.Now )
				{
					count.Count = 1;
					return true;
				}
				else
				{
					++count.Count;
					if ( count.Count <= SkillCheck.Allowance )
						return true;
					else
						return false;
				}
			}
			else
			{
				tbl[obj] = count = new CountAndTimeStamp();
				count.Count = 1;

				return true;
			}
		}

		private void RevertHair()
		{
			SetHairMods( -1, -1 );
		}

		private Engines.BulkOrders.BOBFilter m_BOBFilter;

		public Engines.BulkOrders.BOBFilter BOBFilter
		{
			get{ return m_BOBFilter; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan ResKillTime
		{
			get
			{
				TimeSpan ts = m_ResKillTime - DateTime.Now;

				if ( ts < TimeSpan.Zero )
					ts = TimeSpan.Zero;

				return ts;
			}
			set
			{
				try{ m_ResKillTime = DateTime.Now + value; }
				catch{}
			}
		}

		public override void Deserialize( GenericReader reader )
		{
			m_LastPVP = DateTime.MinValue;

			base.Deserialize( reader );
			int version = reader.ReadInt();

			switch ( version )
			{
				case 28: //Removed player government shit
				case 27:
				{
					m_Attackable = reader.ReadBool();
					goto case 26;
				}
// *** Added by Eclipse
				case 26:
				{
					m_NextHuntContract = reader.ReadDateTime();
					goto case 25;
				}
// *** ***
					 case 25:
					 {
						  m_pagespy = reader.ReadBool();
						  goto case 24;
					 }
// *** Added for Valor ***
				case 24:
				{
					m_ValorGain = reader.ReadDouble();
					m_LastValorLoss = reader.ReadDeltaTime();
					goto case 23;
				}
// *** ***
				case 23:
				{
					m_LastKill = reader.ReadDeltaTime();
					m_Rank = reader.ReadInt();
					m_ShowPvpTitle = reader.ReadBool();
					m_IsResKillProtected = reader.ReadBool();
					goto case 22;
				}
				case 22:
				{
					m_TotalPointsLost = reader.ReadInt();
					m_TotalPointsSpent = reader.ReadInt();
					ResKillTime = reader.ReadTimeSpan();
					m_LastPwned = reader.ReadMobile();
					m_TotalResKills = reader.ReadInt();
					m_TotalResKilled = reader.ReadInt();
					m_LastPwner = reader.ReadMobile();
					m_TotalPoints = reader.ReadInt();
					m_TotalWins = reader.ReadInt();
					m_TotalLoses = reader.ReadInt();
					m_PvpRank = reader.ReadString();
					goto case 21;
				}
				case 21:
				{
					m_Points = reader.ReadInt();
					minPoints = reader.ReadInt();
					goto case 20;
				}

				case 20:
				{
					if ( version < 28 )
					{
						/*m_City = (CityManagementStone)*/reader.ReadItem();
						/*m_CityTitle = */reader.ReadString();
						/*m_ShowCityTitle = */reader.ReadBool();
						/*m_OwesBackTaxes = */reader.ReadBool();
						/*m_BackTaxesAmount = */reader.ReadInt();
					}

					goto case 19;
				}

				case 19:
				{
					m_Points = reader.ReadInt();
					minPoints = reader.ReadInt();
					goto case 18;
				}
				case 18:
				{
					m_SolenFriendship = (SolenFriendship) reader.ReadEncodedInt();

					goto case 17;
				}
				case 17: // changed how DoneQuests is serialized
				case 16:
				{
					m_Quest = QuestSerializer.DeserializeQuest( reader );

					if ( m_Quest != null )
						m_Quest.From = this;

					int count = reader.ReadEncodedInt();

					m_DoneQuests = new ArrayList( count );

					for ( int i = 0; i < count; ++i )
					{
						Type questType = QuestSerializer.ReadType( QuestSystem.QuestTypes, reader );
						DateTime restartTime;

						//Console.WriteLine( "Player: {0} Quest: {1}", Name, questType.FullName );

						if ( version < 17 )
							restartTime = DateTime.MaxValue;
						else
							restartTime = reader.ReadDateTime();

						m_DoneQuests.Add( new QuestRestartInfo( questType, restartTime ) );
					}

					m_Profession = reader.ReadEncodedInt();
					goto case 15;
				}
				case 15:
				{
					m_LastCompassionLoss = reader.ReadDeltaTime();
					goto case 14;
				}
				case 14:
				{
					//reader.ReadMobile();
					//reader.ReadBool();
					//reader.ReadBool();
					//m_Points = reader.ReadInt();
					//minPoints = reader.ReadInt();
					//reader.ReadInt();
					//reader.ReadItem();
					m_CompassionGains = reader.ReadEncodedInt();

					if ( m_CompassionGains > 0 )
						m_NextCompassionDay = reader.ReadDeltaTime();

					goto case 13;
				}
				case 13: // just removed m_PayedInsurance list
				case 12:
				{
					m_BOBFilter = new Engines.BulkOrders.BOBFilter( reader );
					goto case 11;
				}
				case 11:
				{
					if ( version < 13 )
					{
						ArrayList payed = reader.ReadItemList();

						for ( int i = 0; i < payed.Count; ++i )
							((Item)payed[i]).PayedInsurance = true;
					}

					goto case 10;
				}
				case 10:
				{
					if ( reader.ReadBool() )
					{
						m_HairModID = reader.ReadInt();
						m_HairModHue = reader.ReadInt();
						m_BeardModID = reader.ReadInt();
						m_BeardModHue = reader.ReadInt();

						// We cannot call SetHairMods( -1, -1 ) here because the items have not yet loaded
						Timer.DelayCall( TimeSpan.Zero, new TimerCallback( RevertHair ) );
					}

					goto case 9;
				}
				case 9:
				{
					SavagePaintExpiration = reader.ReadTimeSpan();

					if ( SavagePaintExpiration > TimeSpan.Zero )
					{
						BodyMod = ( Female ? 184 : 183 );
						HueMod = 0;
					}

					goto case 8;
				}
				case 8:
				{
					m_NpcGuild = (NpcGuild)reader.ReadInt();
					m_NpcGuildJoinTime = reader.ReadDateTime();
					m_NpcGuildGameTime = reader.ReadTimeSpan();
					goto case 7;
				}
				case 7:
				{
					m_PermaFlags = reader.ReadMobileList(); //SunUO: ReadMobileListOrNull()
					if (m_PermaFlags.Count == 0) m_PermaFlags = null; //Workaround
					goto case 6;
				}
				case 6:
				{
					NextTailorBulkOrder = reader.ReadTimeSpan();
					goto case 5;
				}
				case 5:
				{
					NextSmithBulkOrder = reader.ReadTimeSpan();
					goto case 4;
				}
				case 4:
				{
					m_LastJusticeLoss = reader.ReadDeltaTime();
					m_JusticeProtectors = reader.ReadMobileList(); //SunUO: ReadMobileListOrNull()
					if (m_JusticeProtectors.Count == 0) m_JusticeProtectors = null; //Workaround
					goto case 3;
				}
				case 3:
				{
					m_LastSacrificeGain = reader.ReadDeltaTime();
					m_LastSacrificeLoss = reader.ReadDeltaTime();
					m_AvailableResurrects = reader.ReadInt();
					goto case 2;
				}
				case 2:
				{
					m_Flags = (PlayerFlag)reader.ReadInt();
					goto case 1;
				}
				case 1:
				{
					m_LongTermElapse = reader.ReadTimeSpan();
					m_ShortTermElapse = reader.ReadTimeSpan();
					m_GameTime = reader.ReadTimeSpan();
					goto case 0;
				}
				case 0:
				{
					break;
				}
			}

			// Professions weren't verified on 1.0 RC0
//			if ( !CharacterCreation.VerifyProfession( m_Profession ) )
//				m_Profession = 0;

			if ( m_BOBFilter == null )
				m_BOBFilter = new Engines.BulkOrders.BOBFilter();

			ArrayList list = this.Stabled;

			for ( int i = 0; i < list.Count; ++i )
			{
				BaseCreature bc = list[i] as BaseCreature;

				if ( bc != null )
					bc.IsStabled = true;
			}

			if ( m_Attackable )
			{
				if ( m_ExpireAttackable == null )
					m_ExpireAttackable = new ExpireAttackableTimer( this );

				m_ExpireAttackable.Start();
			}
		}

		private void CleanupAntiMacro()
		{
			if (m_AntiMacroTable == null)
				return;

			//cleanup our anti-macro table
			foreach (Hashtable t in m_AntiMacroTable.Values)
			{
				ArrayList remove = new ArrayList();
				foreach (CountAndTimeStamp time in t.Values)
				{
					if (time.TimeStamp + SkillCheck.AntiMacroExpire <= DateTime.Now)
						remove.Add(time);
				}

				for (int i = 0; i < remove.Count; ++i)
					t.Remove(remove[i]);
			}
			if (m_AntiMacroTable.Count == 0)
				m_AntiMacroTable = null;
		}

		public override void Serialize( GenericWriter writer )
		{
			CleanupAntiMacro();

			//decay our kills
			if ( m_ShortTermElapse < this.GameTime )
			{
				m_ShortTermElapse += TimeSpan.FromHours( 4.0 );
				if ( ShortTermMurders > 0 )
					--ShortTermMurders;
			}

			if ( m_LongTermElapse < this.GameTime )
			{
				m_LongTermElapse += TimeSpan.FromHours( 30.0 );
				if ( Kills > 0 )
					--Kills;
			}

			base.Serialize( writer );

			writer.Write( (int) 28 ); // version

			writer.Write( (bool) m_Attackable );

// *** Added by Eclipse ***
				// Version 26
				writer.Write(m_NextHuntContract);
// *** ***

				writer.Write(m_pagespy);

// *** Added for Valor ***
			writer.Write( (double) m_ValorGain );
			writer.WriteDeltaTime( m_LastValorLoss );
// *** ***

			writer.WriteDeltaTime( m_LastKill );

			writer.Write( m_Rank );

			writer.Write( m_ShowPvpTitle );

			writer.Write( m_IsResKillProtected );

			writer.Write( m_TotalPointsLost );

			writer.Write( m_TotalPointsSpent );

			writer.Write( ResKillTime );

			writer.Write( m_LastPwned );

			writer.Write( m_TotalResKills );

			writer.Write( m_TotalResKilled );

			writer.Write( m_LastPwner );

			writer.Write( m_TotalPoints );

			writer.Write( m_TotalWins );

			writer.Write( m_TotalLoses );

			writer.Write( m_PvpRank );

			writer.Write( m_Points );  //Unknown if should be in or

			writer.Write( minPoints ); //Unknown if should be in or not
/*
			writer.Write( m_City );

			writer.Write( m_CityTitle );

			writer.Write( m_ShowCityTitle );

			writer.Write( m_OwesBackTaxes );

			writer.Write( m_BackTaxesAmount );
*/
			writer.Write( m_Points );
			writer.Write( minPoints );


			writer.WriteEncodedInt( (int) m_SolenFriendship );

			QuestSerializer.Serialize( m_Quest, writer );

			if ( m_DoneQuests == null )
			{
				writer.WriteEncodedInt( (int) 0 );
			}
			else
			{
				writer.WriteEncodedInt( (int) m_DoneQuests.Count );

				for ( int i = 0; i < m_DoneQuests.Count; ++i )
				{
					QuestRestartInfo restartInfo = (QuestRestartInfo)m_DoneQuests[i];

					//Console.WriteLine( "Player: {0} Quest {1}", Name, restartInfo.QuestType.FullName );

					QuestSerializer.Write( restartInfo.QuestType, QuestSystem.QuestTypes, writer );
					writer.Write( (DateTime) restartInfo.RestartTime );
				}
			}

			writer.WriteEncodedInt( (int) m_Profession );

			writer.WriteDeltaTime( m_LastCompassionLoss );

			writer.WriteEncodedInt( m_CompassionGains );

			if ( m_CompassionGains > 0 )
				writer.WriteDeltaTime( m_NextCompassionDay );

			m_BOBFilter.Serialize( writer );

			bool useMods = ( m_HairModID != -1 || m_BeardModID != -1 );

			writer.Write( useMods );

			if ( useMods )
			{
				writer.Write( (int) m_HairModID );
				writer.Write( (int) m_HairModHue );
				writer.Write( (int) m_BeardModID );
				writer.Write( (int) m_BeardModHue );
			}

			writer.Write( SavagePaintExpiration );

			writer.Write( (int) m_NpcGuild );
			writer.Write( (DateTime) m_NpcGuildJoinTime );
			writer.Write( (TimeSpan) m_NpcGuildGameTime );

			writer.WriteMobileList( m_PermaFlags==null ? ms_EmptyArrayList : m_PermaFlags, true ); //Workaround

			writer.Write( NextTailorBulkOrder );

			writer.Write( NextSmithBulkOrder );

			writer.WriteDeltaTime( m_LastJusticeLoss );
			writer.WriteMobileList( m_JusticeProtectors==null ? ms_EmptyArrayList : m_JusticeProtectors, true );

			writer.WriteDeltaTime( m_LastSacrificeGain );
			writer.WriteDeltaTime( m_LastSacrificeLoss );
			writer.Write( m_AvailableResurrects );

			writer.Write( (int) m_Flags );

			writer.Write( m_LongTermElapse );
			writer.Write( m_ShortTermElapse );
			writer.Write( this.GameTime );
		}

		public void ResetKillTime()
		{
			m_ShortTermElapse = this.GameTime + TimeSpan.FromHours( 4 );
			m_LongTermElapse = this.GameTime + TimeSpan.FromHours( 30 );
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime SessionStart
		{
			get{ return m_SessionStart; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan GameTime
		{
			get
			{
				if ( NetState != null )
					return m_GameTime + (DateTime.Now - m_SessionStart);
				else
					return m_GameTime;
			}
		}

		public override bool CanSee( Mobile m )
		{
			if ( m is CharacterStatue )
				((CharacterStatue) m).OnRequestedAnimation( this );

			if ( m is PlayerMobile && ((PlayerMobile)m).m_VisList != null && ((PlayerMobile)m).m_VisList.Contains( this ) )
				return true;

			return base.CanSee( m );
		}

		public override bool CanSee( Item item )
		{
			if ( m_DesignContext != null && m_DesignContext.Foundation.IsHiddenToCustomizer( item ) )
				return false;

			return base.CanSee( item );
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			Faction faction = Faction.Find( this );

			if ( faction != null )
				faction.RemoveMember( this );
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( Faction.IsFactionMap(Map) )
			{
				PlayerState pl = PlayerState.Find( this );

				if ( pl != null )
				{
					Faction faction = pl.Faction;

					if ( faction.Commander == this )
						list.Add( 1042733, faction.Definition.PropName ); // Commanding Lord of the ~1_FACTION_NAME~
					else if ( pl.Sheriff != null )
						list.Add( 1042734, "{0}\t{1}", pl.Sheriff.Definition.FriendlyName, faction.Definition.PropName ); // The Sheriff of  ~1_CITY~, ~2_FACTION_NAME~
					else if ( pl.Finance != null )
						list.Add( 1042735, "{0}\t{1}", pl.Finance.Definition.FriendlyName, faction.Definition.PropName ); // The Finance Minister of ~1_CITY~, ~2_FACTION_NAME~
					else if ( pl.MerchantTitle != MerchantTitle.None )
						list.Add( 1060776, "{0}\t{1}", MerchantTitles.GetInfo( pl.MerchantTitle ).Title, faction.Definition.PropName ); // ~1_val~, ~2_val~
					else
						list.Add( 1060776, "{0}\t{1}", pl.Rank.Title, faction.Definition.PropName ); // ~1_val~, ~2_val~
				}
			}
		}

		public override void OnSingleClick( Mobile from )
		{
				bool RegionIsNoFactions = (Region is CustomRegion && ((CustomRegion)Region).NoFactionEffects);
				if (Faction.IsFactionMap(Map) && !RegionIsNoFactions)
			{
				PlayerState pl = PlayerState.Find( this );

				if ( pl != null )
				{
					string text;
					//bool ascii = false;

					Faction faction = pl.Faction;

					if ( faction.Commander == this )
						text = String.Concat( this.Female ? "(Commanding Lady of the " : "(Commanding Lord of the ", faction.Definition.FriendlyName, ")" );

					// jakob, added text for deputy commander
					else if ( faction.DeputyCommander == this )
						text = String.Concat( "(Deputy Commander of ", faction.Definition.FriendlyName, ")" );
					// end

					else if ( pl.Sheriff != null )
						text = String.Concat( "(The Sheriff of ", pl.Sheriff.Definition.FriendlyName, ", ", faction.Definition.FriendlyName, ")" );
					else if ( pl.Finance != null )
						text = String.Concat( "(The Finance Minister of ", pl.Finance.Definition.FriendlyName, ", ", faction.Definition.FriendlyName, ")" );
					else
					{
						//ascii = true;

						if ( pl.MerchantTitle != MerchantTitle.None )
							text = String.Concat( "(", MerchantTitles.GetInfo( pl.MerchantTitle ).Title.String, ", ", faction.Definition.FriendlyName, ")" );
						else
							text = String.Concat( "(", pl.Rank.Title.String, ", ", faction.Definition.FriendlyName, ")" );
					}

					int hue = ( Faction.Find( from ) == faction ? 98 : 38 );

					// jakob, made all messages use old ascii font
					//PrivateOverheadMessage( MessageType.Label, hue, ascii, text, from.NetState );
					PrivateOverheadMessage( MessageType.Label, hue, true, text, from.NetState );
					// end

				}
			}

			base.OnSingleClick( from );
		}


		protected override bool OnMove( Direction d )
		{
			if( !Core.SE )
				return base.OnMove( d );

			if( AccessLevel != AccessLevel.Player )
				return true;

			if( Hidden )
			{
				if( !Mounted && Skills.Stealth.Value >= 25.0 )
				{
					bool running = (d & Direction.Running) != 0;

					if( running )
					{
						if( (AllowedStealthSteps -= 2) <= 0 )
							RevealingAction();
					}
					else if( AllowedStealthSteps-- <= 0 )
					{
						Server.SkillHandlers.Stealth.OnUse( this );
					}
				}
				else
				{
					RevealingAction();
				}
			}

			return true;
		}


		private bool m_BedrollLogout;

		public bool BedrollLogout
		{
			get{ return m_BedrollLogout; }
			set{ m_BedrollLogout = value; }
		}

		#region Factions
		private PlayerState m_FactionPlayerState;

		public PlayerState FactionPlayerState
		{
			get{ return m_FactionPlayerState; }
			set{ m_FactionPlayerState = value; }
		}
		#endregion

		#region Quest stuff
		private QuestSystem m_Quest;
		private ArrayList m_DoneQuests;
		private SolenFriendship m_SolenFriendship;

		public QuestSystem Quest
		{
			get{ return m_Quest; }
			set{ m_Quest = value; }
		}

		public ArrayList DoneQuests
		{
			get{ return m_DoneQuests; }
			set{ m_DoneQuests = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public SolenFriendship SolenFriendship
		{
			get{ return m_SolenFriendship; }
			set{ m_SolenFriendship = value; }
		}
		#endregion

		#region MyRunUO Invalidation
		private bool m_ChangedMyRunUO;

		public bool ChangedMyRunUO
		{
			get{ return m_ChangedMyRunUO; }
			set{ m_ChangedMyRunUO = value; }
		}

		public void InvalidateMyRunUO()
		{
			if ( !Deleted && !m_ChangedMyRunUO )
			{
				m_ChangedMyRunUO = true;
				Engines.MyRunUO.MyRunUO.QueueMobileUpdate( this );
			}
		}

		public override void OnKillsChange( int oldValue )
		{
			if ( this.Young && this.Kills > oldValue )
			{
				Account acc = this.Account as Account;

				if ( acc != null )
					acc.RemoveYoungStatus( 0 );
			}

			InvalidateMyRunUO();
		}

		public override void OnGenderChanged( bool oldFemale )
		{
			InvalidateMyRunUO();
		}

		public override void OnGuildChange( Server.Guilds.BaseGuild oldGuild )
		{
			InvalidateMyRunUO();
		}

		public override void OnGuildTitleChange( string oldTitle )
		{
			InvalidateMyRunUO();
		}

		public override void OnKarmaChange( int oldValue )
		{
			InvalidateMyRunUO();
		}

		public override void OnFameChange( int oldValue )
		{
			InvalidateMyRunUO();
		}

		public override void OnSkillChange( SkillName skill, double oldBase )
		{
			if ( this.Young && this.SkillsTotal >= 4500 )
			{
				Account acc = this.Account as Account;

				if ( acc != null )
					acc.RemoveYoungStatus( 1019036 ); // You have successfully obtained a respectable skill level, and have outgrown your status as a young player!
			}

			InvalidateMyRunUO();
		}

		public override void OnAccessLevelChanged( AccessLevel oldLevel )
		{
			InvalidateMyRunUO();
		}

		public override void OnRawStatChange( StatType stat, int oldValue )
		{
			InvalidateMyRunUO();
		}

		public override void OnDelete()
		{
			InvalidateMyRunUO();
		}
		#endregion

		#region Fastwalk Prevention
		private static bool FastwalkPrevention = true; // Is fastwalk prevention enabled?
		private static TimeSpan FastwalkThreshold = TimeSpan.FromSeconds( 0.4 ); // Fastwalk prevention will become active after 0.4 seconds

		private DateTime m_NextMovementTime;

		public virtual bool UsesFastwalkPrevention{ get{ return ( AccessLevel < AccessLevel.GameMaster ); } }

		public virtual TimeSpan ComputeMovementSpeed( Direction dir )
		{
			if ( (dir & Direction.Mask) != (this.Direction & Direction.Mask) )
				return TimeSpan.FromSeconds( 0.1 );

			bool running = ( (dir & Direction.Running) != 0 );

			bool onHorse = ( this.Mount != null );

			if ( onHorse )
				return ( running ? TimeSpan.FromSeconds( 0.1 ) : TimeSpan.FromSeconds( 0.2 ) );

			return ( running ? TimeSpan.FromSeconds( 0.2 ) : TimeSpan.FromSeconds( 0.4 ) );
		}

		public static bool MovementThrottle_Callback( NetState ns )
		{
			PlayerMobile pm = ns.Mobile as PlayerMobile;

			if ( pm == null || !pm.UsesFastwalkPrevention )
				return true;

			if ( pm.m_NextMovementTime == DateTime.MinValue )
			{
				// has not yet moved
				pm.m_NextMovementTime = DateTime.Now;
				return true;
			}

			TimeSpan ts = pm.m_NextMovementTime - DateTime.Now;

			if ( ts < TimeSpan.Zero )
			{
				// been a while since we've last moved
				pm.m_NextMovementTime = DateTime.Now;
				return true;
			}

			return ( ts < FastwalkThreshold );
		}
		#endregion

		#region Enemy of One
		private Type m_EnemyOfOneType;
		private bool m_WaitingForEnemy;

		public Type EnemyOfOneType
		{
			get{ return m_EnemyOfOneType; }
			set
			{
				Type oldType = m_EnemyOfOneType;
				Type newType = value;

				if ( oldType == newType )
					return;

				m_EnemyOfOneType = value;

				DeltaEnemies( oldType, newType );
			}
		}

		public bool WaitingForEnemy
		{
			get{ return m_WaitingForEnemy; }
			set{ m_WaitingForEnemy = value; }
		}

		private void DeltaEnemies( Type oldType, Type newType )
		{
			foreach ( Mobile m in this.GetMobilesInRange( 18 ) )
			{
				Type t = m.GetType();

				if ( t == oldType || t == newType )
					Send( new MobileMoving( m, Notoriety.Compute( this, m ) ) );
			}
		}
		#endregion

		#region Hair and beard mods
		private int m_HairModID = -1, m_HairModHue;
		private int m_BeardModID = -1, m_BeardModHue;

		public void SetHairMods( int hairID, int beardID )
		{
			if ( hairID == -1 )
				InternalRestoreHair( true, ref m_HairModID, ref m_HairModHue );
			else if ( hairID != -2 )
				InternalChangeHair( true, hairID, ref m_HairModID, ref m_HairModHue );

			if ( beardID == -1 )
				InternalRestoreHair( false, ref m_BeardModID, ref m_BeardModHue );
			else if ( beardID != -2 )
				InternalChangeHair( false, beardID, ref m_BeardModID, ref m_BeardModHue );
		}

		private Item CreateHair( bool hair, int id, int hue )
		{
			if ( hair )
				return Server.Items.Hair.CreateByID( id, hue );
			else
				return Server.Items.Beard.CreateByID( id, hue );
		}

		private void InternalRestoreHair( bool hair, ref int id, ref int hue )
		{
			if ( id == -1 )
				return;

			Item item = FindItemOnLayer( hair ? Layer.Hair : Layer.FacialHair );

			if ( item != null )
				item.Delete();

			if ( id != 0 )
				AddItem( CreateHair( hair, id, hue ) );

			id = -1;
			hue = 0;
		}

		private void InternalChangeHair( bool hair, int id, ref int storeID, ref int storeHue )
		{
			Item item = FindItemOnLayer( hair ? Layer.Hair : Layer.FacialHair );

			if ( item != null )
			{
				if ( storeID == -1 )
				{
					storeID = item.ItemID;
					storeHue = item.Hue;
				}

				item.Delete();
			}
			else if ( storeID == -1 )
			{
				storeID = 0;
				storeHue = 0;
			}

			if ( id == 0 )
				return;

			AddItem( CreateHair( hair, id, 0 ) );
		}
		#endregion

		#region Virtue stuff
		private DateTime m_LastSacrificeGain;
		private DateTime m_LastSacrificeLoss;
		private int m_AvailableResurrects;

		[CommandProperty( AccessLevel.Administrator )]
		public DateTime LastSacrificeGain{ get{ return m_LastSacrificeGain; } set{ m_LastSacrificeGain = value; } }
		[CommandProperty( AccessLevel.Administrator )]
		public DateTime LastSacrificeLoss{ get{ return m_LastSacrificeLoss; } set{ m_LastSacrificeLoss = value; } }
		[CommandProperty( AccessLevel.Administrator )]
		public int AvailableResurrects{ get{ return m_AvailableResurrects; } set{ m_AvailableResurrects = value; } }

		private DateTime m_NextJustAward;
		private DateTime m_LastJusticeLoss;
		private ArrayList m_JusticeProtectors;

		[CommandProperty( AccessLevel.Administrator )]
		public DateTime LastJusticeLoss{ get{ return m_LastJusticeLoss; } set{ m_LastJusticeLoss = value; } }

		public ArrayList JusticeProtectors
		{
			get
			{
				if (m_JusticeProtectors == null)
					m_JusticeProtectors = new ArrayList(4);

				return m_JusticeProtectors;
			}
		}

		private DateTime m_LastCompassionLoss;
		private DateTime m_NextCompassionDay;
		private int m_CompassionGains;

		[CommandProperty( AccessLevel.Administrator )]
		public DateTime LastCompassionLoss{ get{ return m_LastCompassionLoss; } set{ m_LastCompassionLoss = value; } }
		[CommandProperty( AccessLevel.Administrator )]
		public DateTime NextCompassionDay{ get{ return m_NextCompassionDay; } set{ m_NextCompassionDay = value; } }
		[CommandProperty( AccessLevel.Administrator )]
		public int CompassionGains{ get{ return m_CompassionGains; } set{ m_CompassionGains = value; } }
		#endregion

		#region Young system
		[CommandProperty( AccessLevel.GameMaster )]
		public bool Young
		{
			get{ return GetFlag( PlayerFlag.Young ); }
			set{ SetFlag( PlayerFlag.Young, value ); InvalidateProperties(); }
		}

		public override string ApplyNameSuffix( string suffix )
		{
			FSPvpSystem.PvpStats ps = FSPvpSystem.GetPvpStats( this );
			if ( Young )
			{
				if ( suffix.Length == 0 )
					suffix = "(Young)";
				else
					suffix = String.Concat( suffix, " (Young)" );
			}

			if ( ps.ShowPvpTitle )
			{
				if ( suffix.Length == 0 )
					suffix = PvpRankInfo.GetInfo( ps.RankType ).Abbreviation;
				else
					suffix = String.Concat( suffix, PvpRankInfo.GetInfo( ps.RankType ).Abbreviation );
			}

			if ( ps.NoResKill )
			{
				//if ( suffix.Length == 0 )
				//	suffix = "(RKP)";
				//else
				//	suffix = String.Concat( suffix, " (RKP)" );
			}

			return base.ApplyNameSuffix( suffix );
		}
		public override TimeSpan GetLogoutDelay()
		{
			if ( Young || BedrollLogout )
				return TimeSpan.Zero;

			return base.GetLogoutDelay();
		}

		private DateTime m_LastYoungMessage = DateTime.MinValue;

		public bool CheckYoungProtection( Mobile from )
		{
			if ( !this.Young )
				return false;

			if ( Region is DungeonRegion )
				return false;

			if ( this.Quest != null && this.Quest.IgnoreYoungProtection( from ) )
				return false;

			if ( DateTime.Now - m_LastYoungMessage > TimeSpan.FromMinutes( 1.0 ) )
			{
				m_LastYoungMessage = DateTime.Now;
				SendLocalizedMessage( 1019067 ); // A monster looks at you menacingly but does not attack.  You would be under attack now if not for your status as a new citizen of Britannia.
			}

			return true;
		}

		private DateTime m_LastYoungHeal = DateTime.MinValue;

		public bool CheckYoungHealTime()
		{
			if ( DateTime.Now - m_LastYoungHeal > TimeSpan.FromMinutes( 5.0 ) )
			{
				m_LastYoungHeal = DateTime.Now;
				return true;
			}

			return false;
		}

		private static Point3D[] m_TrammelDeathDestinations = new Point3D[]
			{
				new Point3D( 1481, 1612, 20 ),
				new Point3D( 2708, 2153,  0 ),
				new Point3D( 2249, 1230,  0 ),
				new Point3D( 5197, 3994, 37 ),
				new Point3D( 1412, 3793,  0 ),
				new Point3D( 3688, 2232, 20 ),
				new Point3D( 2578,  604,  0 ),
				new Point3D( 4397, 1089,  0 ),
				new Point3D( 5741, 3218, -2 ),
				new Point3D( 2996, 3441, 15 ),
				new Point3D(  624, 2225,  0 ),
				new Point3D( 1916, 2814,  0 ),
				new Point3D( 2929,  854,  0 ),
				new Point3D(  545,  967,  0 ),
				new Point3D( 3665, 2587,  0 )
			};

		private static Point3D[] m_IlshenarDeathDestinations = new Point3D[]
			{
				new Point3D( 1216,  468, -13 ),
				new Point3D(  723, 1367, -60 ),
				new Point3D(  745,  725, -28 ),
				new Point3D(  281, 1017,   0 ),
				new Point3D(  986, 1011, -32 ),
				new Point3D( 1175, 1287, -30 ),
				new Point3D( 1533, 1341,  -3 ),
				new Point3D(  529,  217, -44 ),
				new Point3D( 1722,  219,  96 )
			};

		private static Point3D[] m_MalasDeathDestinations = new Point3D[]
			{
				new Point3D( 2079, 1376, -70 ),
				new Point3D(  944,  519, -71 )
			};

		private static Point3D[] m_TokunoDeathDestinations = new Point3D[]
			{
				new Point3D( 1166,  801, 27 ),
				new Point3D(  782, 1228, 25 ),
				new Point3D(  268,  624, 15 )
			};

		public Point3D GetYoungDeathDestination()
		{
			if ( this.Region is Jail )
				return Point3D.Zero;

			Point3D[] list;

			if ( this.Map == Map.Trammel || this.Map == Map.Felucca )
				list = m_TrammelDeathDestinations;
			else if ( this.Map == Map.Ilshenar )
				list = m_IlshenarDeathDestinations;
			else if ( this.Map == Map.Malas )
				list = m_MalasDeathDestinations;
			else if ( this.Map == Map.Tokuno )
				list = m_TokunoDeathDestinations;
			else
				return Point3D.Zero;

			IPoint2D loc;

			if ( this.Region == null )
			{
				loc = this.Location;
			}
			else
			{
				string regName = this.Region.Name;

				// If the character is in a dungeon, get the entrance location
				switch ( regName )
				{
					case "Covetous":
						loc = new Point2D( 2499, 916 );
						break;
					case "Deceit":
						loc = new Point2D( 4111, 429 );
						break;
					case "Despise":
						loc = new Point2D( 1296, 1082 );
						break;
					case "Destard":
						loc = new Point2D( 1176, 2635 );
						break;
					case "Hythloth":
						loc = new Point2D( 4722, 3814 );
						break;
					case "Shame":
						loc = new Point2D( 512, 1559 );
						break;
					case "Wrong":
						loc = new Point2D( 2042, 226 );
						break;
					case "Terathan Keep":
						loc = new Point2D( 5426, 3120 );
						break;
					case "Fire":
						loc = new Point2D( 2922, 3402 );
						break;
					case "Ice":
						loc = new Point2D( 1996, 80 );
						break;
					case "Orc Cave":
						loc = new Point2D( 1014, 1434 );
						break;
					case "Misc Dungeons":
						loc = new Point2D( 1492, 1641 );
						break;
					case "Rock Dungeon":
						loc = new Point2D( 1788, 571 );
						break;
					case "Spider Cave":
						loc = new Point2D( 1420, 910 );
						break;
					case "Spectre Dungeon":
						loc = new Point2D( 1362, 1031 );
						break;
					case "Blood Dungeon":
						loc = new Point2D( 1745, 1236 );
						break;
					case "Wisp Dungeon":
						loc = new Point2D( 652, 1301 );
						break;
					case "Ankh Dungeon":
						loc = new Point2D( 668, 928 );
						break;
					case "Exodus Dungeon":
						loc = new Point2D( 827, 777 );
						break;
					case "Sorcerer's Dungeon":
						loc = new Point2D( 546, 455 );
						break;
					case "Ancient Lair":
						loc = new Point2D( 938, 494 );
						break;
					case "Doom":
					case "Doom Gauntlet":
						loc = new Point2D( 2357, 1268 );
						break;
					default:
						loc = this.Location;
						break;
				}
			}

			Point3D dest = Point3D.Zero;
			int sqDistance = int.MaxValue;

			for ( int i = 0; i < list.Length; i++ )
			{
				Point3D curDest = list[i];

				int width = loc.X - curDest.X;
				int height = loc.Y - curDest.Y;
				int curSqDistance = width * width + height * height;

				if ( curSqDistance < sqDistance )
				{
					dest = curDest;
					sqDistance = curSqDistance;
				}
			}

			return dest;
		}

		private void SendYoungDeathNotice()
		{
			this.SendGump( new YoungDeathNotice() );
		}
		#endregion

		#region Speech log
		private SpeechLog m_SpeechLog;

		public SpeechLog SpeechLog{ get{ return m_SpeechLog; } }

		public override void OnSpeech( SpeechEventArgs e )
		{
			if ( SpeechLog.Enabled && this.NetState != null )
			{
				if ( m_SpeechLog == null )
					m_SpeechLog = new SpeechLog();

				m_SpeechLog.Add( e.Mobile, e.Speech );
			}
		}
		#endregion


		  //Al: Clear Aggressed and Aggressors list.
		  public void ClearAggression()
		  {
				//Temporary ArrayLists needed because otherwise it would be modified while it is read.
				ArrayList toDelete = new ArrayList();
				foreach (AggressorInfo ai in Aggressed) toDelete.Add(ai.Defender);
				foreach (Mobile m in toDelete) RemoveAggressed(m);

				toDelete = new ArrayList();
				foreach (AggressorInfo ai in Aggressors) toDelete.Add(ai.Attacker);
				foreach (Mobile m in toDelete) RemoveAggressor(m);
		  }

	}
}