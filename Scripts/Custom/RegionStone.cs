using System;
using Server;
using Server.Mobiles;
using Server.Spells;
using Server.Items;
using Server.Regions;
using System.Collections;
using Server.SkillHandlers;
using Server.Gumps;

namespace Server.Items
{
	public enum RegionFlag
	{
		None				= 0x00000000,
		AllowBenifitPlayer	= 0x00000001,
		AllowHarmPlayer		= 0x00000002,
		AllowHousing		= 0x00000004,
		AllowSpawn			= 0x00000008,

		CanBeDamaged		= 0x00000010,
		CanHeal				= 0x00000020,
		CanRessurect		= 0x00000040,
		CanUseStuckMenu		= 0x00000080,
		ItemDecay			= 0x00000100,

		ShowEnterMessage	= 0x00000200,
		ShowExitMessage		= 0x00000400,

		AllowBenifitNPC		= 0x00000800,
		AllowHarmNPC		= 0x00001000,

		CanMountEthereal	= 0x000002000,
		CannotEnter			= 0x000004000,

		CanLootPlayerCorpse	= 0x000008000,
		CanLootNPCCorpse	= 0x000010000,
		CannotLootOwnCorpse = 0x000020000,

		CanUsePotions		= 0x000040000,

		IsGuarded			= 0x000080000,

		GameRegion			= 0x000100000,
		DeleteCorpsesOnDeath= 0x000200000,

		IsDungeonRegion		= 0x000400000,

		IsGuardedRegion		= 0x000400000
	}

	public enum CustomRegionPriority
	{
		HighestPriority	= 0x96,
		HousePriority	= 0x96,
		HighPriority	= 0x90,
		MediumPriority	= 0x64,
		LowPriority		= 0x60,
		InnPriority		= 0x33,
		TownPriority	= 0x32,
		LowestPriority	= 0x0
	}

	public class RegionControl : Item
	{
		#region Flags

        public bool GetFlag(RegionFlag flag)
        {
            return ((m_Flags & flag) != 0);
        }

        public void SetFlag(RegionFlag flag, bool value)
        {
            if (value)
                m_Flags |= flag;
            else
                m_Flags &= ~flag;
        }


		public RegionFlag Flags
		{
			get{ return m_Flags; }
			set{ m_Flags = value; }
		}

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CannotTrade
        {
            get { return m_CannotTrade; }
            set { m_CannotTrade = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool AlwaysGrey
        {
            get { return m_AlwaysGrey; }
            set { m_AlwaysGrey = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool NoMurderCounts
        {
            get { return m_NoMurderCounts; }
            set { m_NoMurderCounts = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool NoFactionEffects
        {
            get { return m_NoFactionEffects; }
            set { m_NoFactionEffects = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool NoPvPPoints
        {
            get { return m_NoPvPPoints; }
            set { m_NoPvPPoints = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool NoFameKarma
        {
            get { return m_NoFameKarma; }
            set { m_NoFameKarma = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CannotTakeRewards
        {
            get { return m_CannotTakeRewards; }
            set { m_CannotTakeRewards = value; }
        }

		[CommandProperty(AccessLevel.GameMaster)]
		public bool IsGameRegion
		{
			get { return GetFlag( RegionFlag.GameRegion ); }
			set
			{
				SetFlag( RegionFlag.GameRegion, value );
				if (value)
				{
					//Setting GameRegion to true also sets some standard settings for GameRegions
					AllowBenifitPlayer = true;
					AllowHarmPlayer = true;
					AllowSpawn = true;//Spell teleport
					CanBeDamaged = true;
					CanHeal = true;
					CanMountEthereal = false;
					CanRessurect = true;
					CanUsePotions = true;
					CanUseStuckMenu = false;
					CannotEnter = true;
					CannotTakeRewards = true;
					DeleteCorpsesOnDeath = true;
					IsGuarded = false; //For some reason IsGuarded defaults to true
					ItemDecay = true;
					NoFactionEffects = true;
					NoFameKarma = true;
					NoMurderCounts = true;
					NoPvPPoints = true;

					PlayerLogoutDelay = TimeSpan.FromHours(2);
					Priority = CustomRegionPriority.HighestPriority;

					SetFlag(RegionFlag.AllowHousing, false);

					m_RestrictedSpells[GetRegistryNumber(typeof(Spells.Fourth.FireFieldSpell))] = true;
					m_RestrictedSpells[GetRegistryNumber(typeof(Spells.Fourth.RecallSpell))] = true;
					m_RestrictedSpells[GetRegistryNumber(typeof(Spells.Fifth.BladeSpiritsSpell))] = true;
					m_RestrictedSpells[GetRegistryNumber(typeof(Spells.Fifth.IncognitoSpell))] = true;
					m_RestrictedSpells[GetRegistryNumber(typeof(Spells.Fifth.SummonCreatureSpell))] = true;
					m_RestrictedSpells[GetRegistryNumber(typeof(Spells.Sixth.MarkSpell))] = true;
					m_RestrictedSpells[GetRegistryNumber(typeof(Spells.Seventh.EnergyFieldSpell))] = true;
					m_RestrictedSpells[GetRegistryNumber(typeof(Spells.Seventh.GateTravelSpell))] = true;
					m_RestrictedSpells[GetRegistryNumber(typeof(Spells.Seventh.PolymorphSpell))] = true;
					m_RestrictedSpells[GetRegistryNumber(typeof(Spells.Eighth.SummonDaemonSpell))] = true;
					m_RestrictedSpells[GetRegistryNumber(typeof(Spells.Eighth.AirElementalSpell))] = true;
					m_RestrictedSpells[GetRegistryNumber(typeof(Spells.Eighth.EarthElementalSpell))] = true;
					m_RestrictedSpells[GetRegistryNumber(typeof(Spells.Eighth.EnergyVortexSpell))] = true;
					m_RestrictedSpells[GetRegistryNumber(typeof(Spells.Eighth.FireElementalSpell))] = true;
					m_RestrictedSpells[GetRegistryNumber(typeof(Spells.Eighth.WaterElementalSpell))] = true;
					m_RestrictedSpells[GetRegistryNumber(typeof(Spells.Eighth.ResurrectionSpell))] = true;
				}
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool DeleteCorpsesOnDeath
		{
			get { return GetFlag(RegionFlag.DeleteCorpsesOnDeath); }
			set { SetFlag(RegionFlag.DeleteCorpsesOnDeath, value); }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool AllowBenifitPlayer
		{
			get{ return GetFlag( RegionFlag.AllowBenifitPlayer ); }
			set{ SetFlag( RegionFlag.AllowBenifitPlayer, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool AllowHarmPlayer
		{
			get{ return GetFlag( RegionFlag.AllowHarmPlayer ); }
			set{ SetFlag( RegionFlag.AllowHarmPlayer, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool AllowHousing
		{
			get{ return GetFlag( RegionFlag.AllowHousing ); }
			set{ SetFlag( RegionFlag.AllowHousing, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool AllowSpawn
		{
			get{ return GetFlag( RegionFlag.AllowSpawn ); }
			set{ SetFlag( RegionFlag.AllowSpawn, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanBeDamaged
		{
			get{ return GetFlag( RegionFlag.CanBeDamaged ); }
			set{ SetFlag( RegionFlag.CanBeDamaged, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanMountEthereal
		{
			get{ return GetFlag( RegionFlag.CanMountEthereal ); }
			set{ SetFlag( RegionFlag.CanMountEthereal, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CannotEnter
		{
			get{ return GetFlag( RegionFlag.CannotEnter ); }
			set{ SetFlag( RegionFlag.CannotEnter, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanHeal
		{
			get{ return GetFlag( RegionFlag.CanHeal ); }
			set{ SetFlag( RegionFlag.CanHeal, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanRessurect
		{
			get{ return GetFlag( RegionFlag.CanRessurect ); }
			set{ SetFlag( RegionFlag.CanRessurect, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanUseStuckMenu
		{
			get{ return GetFlag( RegionFlag.CanUseStuckMenu ); }
			set{ SetFlag( RegionFlag.CanUseStuckMenu, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool ItemDecay
		{
			get{ return GetFlag( RegionFlag.ItemDecay ); }
			set{ SetFlag( RegionFlag.ItemDecay, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool AllowBenifitNPC
		{
			get{ return GetFlag( RegionFlag.AllowBenifitNPC ); }
			set{ SetFlag( RegionFlag.AllowBenifitNPC, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool AllowHarmNPC
		{
			get{ return GetFlag( RegionFlag.AllowHarmNPC ); }
			set{ SetFlag( RegionFlag.AllowHarmNPC, value ); }
		}


		[CommandProperty( AccessLevel.GameMaster )]
		public bool ShowEnterMessage
		{
			get{ return GetFlag( RegionFlag.ShowEnterMessage ); }
			set{ SetFlag( RegionFlag.ShowEnterMessage, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool ShowExitMessage
		{
			get{ return GetFlag( RegionFlag.ShowExitMessage ); }
			set{ SetFlag( RegionFlag.ShowExitMessage, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanLootPlayerCorpse
		{
			get{ return GetFlag( RegionFlag.CanLootPlayerCorpse ); }
			set{ SetFlag( RegionFlag.CanLootPlayerCorpse, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanLootNPCCorpse
		{
			get{ return GetFlag( RegionFlag.CanLootNPCCorpse ); }
			set{ SetFlag( RegionFlag.CanLootNPCCorpse, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CannotLootOwnCorpse
		{
			get{ return GetFlag( RegionFlag.CannotLootOwnCorpse ); }
			set{ SetFlag( RegionFlag.CannotLootOwnCorpse, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanUsePotions
		{
			get{ return GetFlag( RegionFlag.CanUsePotions ); }
			set{ SetFlag( RegionFlag.CanUsePotions, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsGuarded
		{
			get{
				if( m_Region != null )
					return !m_Region.IsDisabled();
				else
					return GetFlag( RegionFlag.IsGuarded );
			}
			set{
				SetFlag( RegionFlag.IsGuarded, value );

				UpdateRegion();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool IsDungeonRegion
		{
			get { return GetFlag( RegionFlag.IsDungeonRegion ); }
			set
			{
				SetFlag( RegionFlag.IsDungeonRegion, value );
			}
		}
		#endregion



		[CommandProperty(AccessLevel.GameMaster)]
		public bool IsGuardedRegion
		{
			get { return GetFlag( RegionFlag.IsGuardedRegion ); }
			set
			{
				SetFlag( RegionFlag.IsGuardedRegion, value );
			}
		}
		//#endregion

		private CustomRegion m_Region;

		private RegionFlag m_Flags;
		private BitArray m_RestrictedSpells;
		private BitArray m_RestrictedSkills;

		private string m_RegionName;
		private CustomRegionPriority m_Priority;

		private MusicName m_Music;

		private TimeSpan m_PlayerLogoutDelay;

		private ArrayList m_Coords;

		private int m_LightLevel;

        private bool m_NoMurderCounts;
        private bool m_NoPvPPoints;
        private bool m_NoFameKarma;
        private bool m_NoFactionEffects;

        private bool m_CannotTakeRewards;

        private bool m_AlwaysGrey;
        private bool m_CannotTrade;

        private Point3D m_LogoutMoveLocation;
        private Map m_LogoutMoveMap = Map.Internal;

        private bool m_CastWithoutReagents;

        public CustomRegion MyRegion
		{
			get{ return m_Region; }
		}


		public BitArray RestrictedSpells
		{
			get{ return m_RestrictedSpells; }
			set{ m_RestrictedSpells = value; }
		}

		public BitArray RestrictedSkills
		{
			get{ return m_RestrictedSkills; }
			set{ m_RestrictedSkills = value; }
		}


		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan PlayerLogoutDelay
		{
			get{ return m_PlayerLogoutDelay; }
			set{ m_PlayerLogoutDelay = value; }
		}


		public ArrayList Coords
		{
			get{ return m_Coords; }
			set{ m_Coords = value; UpdateRegion(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string RegionName
		{
			get{ return m_RegionName; }
			set{
				m_RegionName = value;
				UpdateRegion();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public MusicName Music
		{
			get{ return m_Music; }
			set
			{
				m_Music = value;
				UpdateRegion();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int LightLevel
		{
			get{ return m_LightLevel; }
			set{ m_LightLevel = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public CustomRegionPriority Priority
		{
			get{ return m_Priority; }
			set
			{
				m_Priority = value;
				UpdateRegion();
			}
		}

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D LogoutMoveLocation
        {
            get { return m_LogoutMoveLocation; }
            set { m_LogoutMoveLocation = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map LogoutMoveMap
        {
            get { return m_LogoutMoveMap; }
            set { m_LogoutMoveMap = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CastWithoutReagents
        {
            get { return m_CastWithoutReagents; }
            set { m_CastWithoutReagents = value; }
        }

		[Constructable]
		public RegionControl() : base ( 5609 )
		{
			Visible = false;
			Movable = false;
			Name = "Region Controller";
			m_RegionName = "Custom Region";
			m_Priority = CustomRegionPriority.HighPriority;

			m_RestrictedSpells = new BitArray( SpellRegistry.Types.Length );
			m_RestrictedSkills = new BitArray( SkillInfo.Table.Length );

            //Some defaults to avoid mistakes
            CannotEnter = true;
            m_RestrictedSpells[GetRegistryNumber(typeof(Spells.Sixth.MarkSpell))] = true;

			Coords = new ArrayList();
			UpdateRegion();
		}

		[Constructable]
		public RegionControl( Rectangle2D rect ) : base ( 5609 )
		{
			Coords = new ArrayList();

			Coords.Add( rect );

			m_RestrictedSpells = new BitArray( SpellRegistry.Types.Length );
			m_RestrictedSkills = new BitArray( SkillInfo.Table.Length );

			Visible = false;
			Movable = false;
			Name = "Region Controller";
			m_RegionName = "Custom Region";
			m_Priority = CustomRegionPriority.HighPriority;

			UpdateRegion();
		}

		public RegionControl( Serial serial ) : base( serial )
		{
		}

        public static void Initialize()
        {
            EventSink.Logout += new LogoutEventHandler(OnLogout);
        }

        public static void OnLogout(LogoutEventArgs e)
        {
            if (e.Mobile == null) return;
            CustomRegion customRegion = e.Mobile.Region as CustomRegion;
            if (customRegion != null && customRegion.LogoutMoveMap != Map.Internal && e.Mobile.AccessLevel==AccessLevel.Player )
                e.Mobile.MoveToWorld(customRegion.LogoutMoveLocation, customRegion.LogoutMoveMap);
        }

		public override void OnDoubleClick( Mobile m )
		{
			if( m.AccessLevel >= AccessLevel.GameMaster)
			{
			//	m.SendGump( new RestrictGump( m_RestrictedSpells, RestrictType.Spells ) );
			//	m.SendGump( new RestrictGump( m_RestrictedSkills, RestrictType.Skills ) );

				m.CloseGump( typeof( RegionControlGump ) );
				m.SendGump( new RegionControlGump( this ) );
				m.SendMessage( "Don't forget to props this object for more options!" );

				m.CloseGump( typeof( RemoveAreaGump ) );
				m.SendGump( new RemoveAreaGump( this ) );
			}
		}

		public override void OnMapChange()
		{
			UpdateRegion();
			base.OnMapChange();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

            writer.Write( (int) 7 ); // version

            //Version 7
            writer.Write((bool)m_CastWithoutReagents);

            //Version 6
            writer.Write((Point3D)m_LogoutMoveLocation);
            writer.Write((Map)m_LogoutMoveMap);

            //Version 5
            writer.Write((bool) m_AlwaysGrey);
            writer.Write((bool) m_CannotTrade);

            //Version 4
            writer.Write((bool)m_NoFameKarma);
            writer.Write((bool)m_NoPvPPoints);
            writer.Write((bool)m_NoMurderCounts);
            writer.Write((bool)m_NoFactionEffects);
            writer.Write((bool)m_CannotTakeRewards);

            //Version 3
            writer.Write( (int)m_LightLevel );

			writer.Write( (int) m_Music );

			WriteRect2DArray( writer, Coords );
			writer.Write( (int)m_Priority );
			writer.Write( (TimeSpan)m_PlayerLogoutDelay );


			//writer.Write( m_Area );
			WriteBitArray( writer, m_RestrictedSpells );
			WriteBitArray( writer, m_RestrictedSkills );

			writer.Write( (int) m_Flags );
			writer.Write( m_RegionName );
		}
		#region Serialization Helpers
		public static void WriteBitArray( GenericWriter writer, BitArray ba )
		{
			writer.Write( ba.Length );

				for( int i = 0; i < ba.Length; i++ )
				{
					writer.Write( ba[i] );
				}
			return;
		}

		public static BitArray ReadBitArray( GenericReader reader )
		{
			int size = reader.ReadInt();
			BitArray newBA = new BitArray( size );

			for( int i = 0; i < size; i++ )
			{
				newBA[i] = reader.ReadBool();
			}

			return newBA;
		}


		public static void WriteRect2DArray( GenericWriter writer, ArrayList ary )
		{
			writer.Write( ary.Count );

			for( int i = 0; i < ary.Count; i++ )
			{
				writer.Write( (Rectangle2D)ary[i] );	//Rect2D
			}
			return;
		}

		public static ArrayList ReadRect2DArray( GenericReader reader )
		{
			int size = reader.ReadInt();
			ArrayList newAry = new ArrayList();

			for( int i = 0; i < size; i++ )
			{
				newAry.Add( reader.ReadRect2D() );
			}

            return newAry;
		}

		#endregion
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
            m_LogoutMoveMap = Map.Internal;

			int version = reader.ReadInt();

			switch ( version )
			{
                case 7:
                {
                    m_CastWithoutReagents=reader.ReadBool();
                    goto case 6;
                }
                case 6:
                {
                    m_LogoutMoveLocation = reader.ReadPoint3D();
                    m_LogoutMoveMap = reader.ReadMap();
                    goto case 5;
                }
                case 5:
                {
                    m_AlwaysGrey = reader.ReadBool();
                    m_CannotTrade = reader.ReadBool();
                    goto case 4;
                }
                case 4:
                {
                    m_NoFameKarma = reader.ReadBool();
                    m_NoPvPPoints = reader.ReadBool();
                    m_NoMurderCounts = reader.ReadBool();
                    m_NoFactionEffects = reader.ReadBool();
                    m_CannotTakeRewards = reader.ReadBool();
                    goto case 3;
                }
                case 3:
				{
					m_LightLevel = reader.ReadInt();
					goto case 2;
				}
				case 2:
				{
					m_Music = (MusicName)reader.ReadInt();
					goto case 1;
				}
				case 1:
				{
					Coords = ReadRect2DArray( reader );
					m_Priority = (CustomRegionPriority) reader.ReadInt();
					m_PlayerLogoutDelay = reader.ReadTimeSpan();

					m_RestrictedSpells = ReadBitArray( reader );
					m_RestrictedSkills = ReadBitArray( reader );

					m_Flags = (RegionFlag)reader.ReadInt();

					m_RegionName = reader.ReadString();
					break;
				}
				case 0:
				{
					Coords = new ArrayList();

					Coords.Add( reader.ReadRect2D() );
					m_RestrictedSpells = ReadBitArray( reader );
					m_RestrictedSkills = ReadBitArray( reader );

					m_Flags = (RegionFlag)reader.ReadInt();

					m_RegionName = reader.ReadString();
					break;
				}
			}

			UpdateRegion();

		}


		public void UpdateRegion()
		{
			if( Coords != null && Coords.Count != 0 )
			{
				if( m_Region == null )
				{
					m_Region = new CustomRegion( this, this.Map );
					//Region.AddRegion( m_Region );	//Maybe not needed cause setting map will call this?
				}

				Region.RemoveRegion( m_Region );

				m_Region.Coords = Coords;

				m_Region.Disabled = !(GetFlag( RegionFlag.IsGuarded ));

				m_Region.Music = Music;
				m_Region.Name = m_RegionName;

				m_Region.Priority = (int)m_Priority;

				m_Region.Map = this.Map;

				Region.AddRegion( m_Region );
			}

			return;
		}


		public static int GetRegistryNumber( ISpell s )
		{
			Type[] t = SpellRegistry.Types;

			for( int i = 0; i < t.Length; i++ )
			{
				if( s.GetType() == t[i] )
					return i;
			}

			return -1;
		}

		public static int GetRegistryNumber( Type type )
		{
			Type[] t = SpellRegistry.Types;

			for (int i = 0; i < t.Length; i++)
				if ( type == t[i] )
					return i;

			return -1;
		}

		public bool IsRestrictedSpell( ISpell s )
		{
			int regNum = GetRegistryNumber( s );

			if( regNum < 0 )	//Happens with unregistered Spells
				return false;

			return m_RestrictedSpells[regNum];
		}

		public bool IsRestrictedSpell( Type type )
		{
			int regNum = GetRegistryNumber( type );

			if( regNum < 0 )	//Happens with unregistered Spells
				return false;

			return m_RestrictedSpells[regNum];
		}

		public bool IsRestrictedSkill( int skill )
		{
			if( skill < 0 )
				return false;

			return m_RestrictedSkills[skill];
		}


		public void ChooseArea( Mobile m )
		{
			BoundingBoxPicker.Begin( m, new BoundingBoxCallback( CustomRegion_Callback ), this );
		}

		private static void CustomRegion_Callback( Mobile from, Map map, Point3D start, Point3D end, object state )
		{
			DoChooseArea( from, map, start, end, state );
		}

		private static void DoChooseArea( Mobile from, Map map, Point3D start, Point3D end, object control )
		{
			RegionControl r = (RegionControl)control;
			Rectangle2D rect = new Rectangle2D( start.X, start.Y, end.X - start.X + 1, end.Y - start.Y + 1 );

			r.m_Coords.Add( rect );

			r.UpdateRegion();
		}

		public override void OnDelete()
		{
			if( m_Region != null )
				Region.RemoveRegion( m_Region );

			base.OnDelete();
		}
	}
}