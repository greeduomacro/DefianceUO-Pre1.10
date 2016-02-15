using System;
using System.Collections;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Spells;
using Server.Multis;
using Server.Network;
using Server.Targeting;
using Server.Accounting;
using Server.ContextMenus;
using Server.Engines.VeteranRewards;

namespace Server.Mobiles
{
    public enum StatueType
    {
    	Marble,
    	Jade,
    	Bronze,
		Bloodstone,
		Alabaster,
		Granite,
		Gold
    }

    public enum StatuePose
    {
        Ready,
        Casting,
        Salute,
        AllPraiseMe,
        Fighting,
        HandsOnHips
    }

    public enum StatueMaterial
    {
    	Antique,
    	Dark,
    	Medium,
    	Light,
		Extra1,
		Extra2
    }

	public class CharacterStatue : Mobile, IRewardItem
	{
		private StatueType m_Type;
		private StatuePose m_Pose;
		private StatueMaterial m_Material;

		[CommandProperty( AccessLevel.GameMaster )]
		public StatueType StatueType
		{
			get { return m_Type; }
			set { m_Type = value; InvalidateHues(); InvalidatePose(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public StatuePose Pose
		{
			get { return m_Pose; }
			set { m_Pose = value; InvalidatePose(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public StatueMaterial Material
		{
			get { return m_Material; }
			set { m_Material = value; InvalidateHues(); InvalidatePose(); }
		}

		private Mobile m_SculptedBy;
		private DateTime m_SculptedOn;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile SculptedBy
		{
			get{ return m_SculptedBy; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime SculptedOn
		{
			get{ return m_SculptedOn; }
		}

		private CharacterStatuePlinth m_Plinth;

		public CharacterStatuePlinth Plinth
		{
			get { return m_Plinth; }
			set { m_Plinth = value; }
		}

		private bool m_IsRewardItem;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsRewardItem
		{
			get{ return m_IsRewardItem; }
			set{ m_IsRewardItem = value; }
		}

		public CharacterStatue( Mobile from, StatueType type ) : base()
		{
			m_Type = type;
			m_Pose = StatuePose.Ready;
			m_Material = StatueMaterial.Antique;

			m_SculptedBy = from;
			m_SculptedOn = DateTime.Now;

			Direction = Direction.South;
			AccessLevel = AccessLevel.Counselor;
			Hits = HitsMax;
			Blessed = true;
			Frozen = true;
			NameHue = 67;

			CloneBody( from );
			CloneClothes( from );
			InvalidateHues();
		}

		public CharacterStatue( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			DisplayPaperdollTo( from );
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_SculptedBy != null )
			{
				if ( m_SculptedBy.Title != null )
					list.Add( 1076202, m_SculptedBy.Title + " " + m_SculptedBy.Name ); // Sculpted by ~1_Name~
				else
					list.Add( 1076202, m_SculptedBy.Name ); // Sculpted by ~1_Name~
			}
		}

		public override void GetContextMenuEntries( Mobile from, ArrayList list )
		{
			base.GetContextMenuEntries( from, list );

			if ( from.Alive )
			{
				BaseHouse house = BaseHouse.FindHouseAt( this );

				if ( ( house != null && house.IsCoOwner( from ) ) || from.AccessLevel > AccessLevel.Counselor )
					list.Add( new DemolishEntry( this ) );
			}
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			if ( m_Plinth != null && !m_Plinth.Deleted )
				m_Plinth.Delete();
		}

		protected override void OnMapChange( Map oldMap )
		{
			InvalidatePose();

			if ( m_Plinth != null )
				m_Plinth.Map = Map;
		}

		protected override void OnLocationChange( Point3D oldLocation )
		{
			InvalidatePose();

			if ( m_Plinth != null )
				m_Plinth.Location = new Point3D( X, Y, Z - 5 );
		}

		public override bool CanBeRenamedBy( Mobile from )
		{
			return false;
		}

		public override bool CanBeDamaged()
		{
			return false;
		}

		public void OnRequestedAnimation( Mobile from )
		{
			from.Send( new UpdateStatueAnimation( this, 1, m_Animation, m_Frames ) );
		}

		public override void OnAosSingleClick( Mobile from )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version

			writer.Write( (int) m_Type );
			writer.Write( (int) m_Pose );
			writer.Write( (int) m_Material );

			writer.Write( (Mobile) m_SculptedBy );
			writer.Write( (DateTime) m_SculptedOn );

			writer.Write( (Item) m_Plinth );
			writer.Write( (bool) m_IsRewardItem );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();

			m_Type = (StatueType) reader.ReadInt();
			m_Pose = (StatuePose) reader.ReadInt();
			m_Material = (StatueMaterial) reader.ReadInt();

			m_SculptedBy = reader.ReadMobile();
			m_SculptedOn = reader.ReadDateTime();

			m_Plinth = reader.ReadItem() as CharacterStatuePlinth;
			m_IsRewardItem = reader.ReadBool();

			InvalidatePose();

			Frozen = true;
		}

		public void Sculpt( Mobile by )
		{
			m_SculptedBy = by;
			m_SculptedOn = DateTime.Now;

			InvalidateProperties();
		}

		public static void CloseGump( Mobile from )
		{
			CharacterStatueGump gump = from.FindGump( typeof( CharacterStatueGump ) ) as CharacterStatueGump;
			if ( gump != null )
			{
				gump.CloseGump();
				from.CloseGump( typeof( CharacterStatueGump ) );
			}
		}

		public void Demolish( Mobile by )
		{
			CharacterStatueDeed deed = new CharacterStatueDeed( null );

			if ( by.PlaceInBackpack( deed ) )
			{
				by.CloseGump( typeof( CharacterStatueGump ) ); //They don't get the deed back twice
				Internalize();

				deed.Statue = this;
				deed.IsRewardItem = m_IsRewardItem;

				if ( m_Plinth != null )
					m_Plinth.Internalize();
			}
			else
			{
				by.SendLocalizedMessage( 500720 ); // You don't have enough room in your backpack!
				deed.Delete();
			}
		}

		public void Restore( CharacterStatue from )
		{
			m_Material = from.Material;
			m_Pose = from.Pose;

			Direction = from.Direction;

			CloneBody( from );
			CloneClothes( from );

			InvalidateHues();
			InvalidatePose();
		}

		public void CloneBody( Mobile from )
		{
			Name = from.Name;
			BodyValue = from.BodyValue;

			//HairItemID = from.HairItemID;
			//FacialHairItemID = from.FacialHairItemID;
		}

		public void CloneClothes( Mobile from )
		{
			for ( int i = Items.Count - 1; i >= 0; i -- )
				((Item)Items[i]).Delete();

			for ( int i = from.Items.Count - 1; i >= 0; i -- )
			{
				Item item = (Item)from.Items[i];

				if ( item.Layer != Layer.Backpack && item.Layer != Layer.Mount && item.Layer != Layer.Bank )
					EquipItem( CloneItem( item ) );
			}
		}

		public Item CloneItem( Item item )
		{
			Item cloned = new Item( item.ItemID );
			cloned.Layer = item.Layer;
			cloned.Name = item.Name;
			cloned.Hue = item.Hue;
			cloned.Weight = item.Weight;
			cloned.Movable = false;

			return cloned;
		}

		public void InvalidateHues()
		{
			if ( m_Type <= StatueType.Bronze && m_Material <= StatueMaterial.Light )
				Hue = 0xB8F + ((int)m_Type * 4) + (int)m_Material;
			else if ( m_Type == StatueType.Jade )
				Hue = 0xB83;
			else if ( m_Type == StatueType.Bronze )
				Hue = 0xB87 + (int)m_Material - (int)StatueMaterial.Extra1;
			else if ( m_Type == StatueType.Marble )
				Hue = 0xB8B + (int)m_Material - (int)StatueMaterial.Extra1;
			else if ( m_Type == StatueType.Alabaster )
				Hue = 0xB89;
			else if ( m_Type == StatueType.Bloodstone )
				Hue = 0xB85;
			else if ( m_Type == StatueType.Granite )
				Hue = 0xB8E;
			else if ( m_Type == StatueType.Gold )
				Hue = 1281;

			//HairHue = Hue;

			//if ( FacialHairItemID > 0 )
			//	FacialHairHue = Hue;

			for ( int i = Items.Count - 1; i >= 0; i -- )
				((Item)Items[i]).Hue = Hue;

			if ( m_Plinth != null )
				m_Plinth.Hue = Hue;
			//	m_Plinth.InvalidateHue();
		}

		private int m_Animation;
		private int m_Frames;

		public void InvalidatePose()
		{
			switch ( m_Pose )
            {
            	case StatuePose.Ready:
            			m_Animation = 4;
                        m_Frames = 0;
                        break;
                case StatuePose.Casting:
                        m_Animation = 16;
                        m_Frames = 2;
                        break;
                case StatuePose.Salute:
                        m_Animation = 33;
                        m_Frames = 1;
                        break;
                case StatuePose.AllPraiseMe:
                        m_Animation = 17;
                        m_Frames = 4;
                        break;
                case StatuePose.Fighting:
                        m_Animation = 31;
                        m_Frames = 5;
                        break;
                case StatuePose.HandsOnHips:
                        m_Animation = 6;
                        m_Frames = 1;
                        break;
            }

			if( Map != null )
			{
				ProcessDelta();

				//Packet p = null;

				IPooledEnumerable eable = Map.GetClientsInRange( Location );

				foreach( NetState state in eable )
				{
					state.Mobile.ProcessDelta();

					//if( p == null )
					//	p = Packet.Acquire( new UpdateStatueAnimation( this, 1, m_Animation, m_Frames ) );

					state.Send( new UpdateStatueAnimation( this, 1, m_Animation, m_Frames ) );
				}

				//Packet.Release( p );

				eable.Free();
			}
	    }

		private class DemolishEntry : ContextMenuEntry
		{
			private CharacterStatue m_Statue;

			public DemolishEntry( CharacterStatue statue ) : base( 6275, 2 )
			{
				m_Statue = statue;
			}

			public override void OnClick()
			{
				if ( m_Statue.Deleted )
					return;

				m_Statue.Demolish( Owner.From );
			}
		}
	}

	public class CharacterStatueDeed : Item, IRewardItem
	{
		private CharacterStatue m_Statue;
		private bool m_IsRewardItem;

		[CommandProperty( AccessLevel.GameMaster )]
		public CharacterStatue Statue
		{
			get { return m_Statue; }
			set { m_Statue = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public StatueType StatueType
		{
			get
			{
				if ( m_Statue != null )
					return m_Statue.StatueType;

				return StatueType.Marble;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsRewardItem
		{
			get{ return m_IsRewardItem; }
			set{ m_IsRewardItem = value; InvalidateProperties(); }
		}

		public CharacterStatueDeed( CharacterStatue statue ) : base( 0x14F0 )
		{
			m_Statue = statue;

			LootType = LootType.Blessed;
			Weight = 1.0;
		}

		public CharacterStatueDeed( Serial serial ) : base( serial )
		{
		}

		public override void AddNameProperty( ObjectPropertyList list )
		{
			if ( m_Statue != null )
			{
				switch ( m_Statue.StatueType )
				{
					case StatueType.Marble: list.Add( 1076189 ); break;
					case StatueType.Jade: list.Add( 1076188 ); break;
					case StatueType.Bronze: list.Add( 1076190 ); break;
					case StatueType.Alabaster: list.Add( "Alabaster Character Statue Maker" ); break;
					case StatueType.Granite: list.Add( "Granite Character Statue Maker" ); break;
					case StatueType.Bloodstone: list.Add( "Bloodstone Character Statue Maker" ); break;
					case StatueType.Gold: list.Add( "Gold Character Statue Maker" ); break;
				}
			}
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_IsRewardItem )
				list.Add( 1076222 ); // 6th Year Veteran Reward

			if ( m_Statue != null )
				list.Add( 1076231, m_Statue.Name ); // Statue of ~1_Name~
		}

		public override void OnSingleClick( Mobile from )
		{
			if ( m_Statue != null )
			{
				switch ( m_Statue.StatueType )
				{
					case StatueType.Marble: LabelTo( from, 1076189 ); break;
					case StatueType.Jade: LabelTo( from, 1076188 ); break;
					case StatueType.Bronze: LabelTo( from, 1076190 ); break;
					case StatueType.Alabaster: LabelTo( from, "Alabaster Character Statue Maker" ); break;
					case StatueType.Granite: LabelTo( from, "Granite Character Statue Maker" ); break;
					case StatueType.Bloodstone: LabelTo( from, "Bloodstone Character Statue Maker" ); break;
					case StatueType.Gold: LabelTo( from, "Gold Character Statue Maker" ); break;
				}

				LabelTo( from, 1076231, m_Statue.Name ); // Statue of ~1_Name~
			}
			else
				LabelTo( from, 1076173 );
		}

		public override void OnDoubleClick( Mobile from )
		{
			Account acct = from.Account as Account;

			if ( m_IsRewardItem && acct != null && from.AccessLevel == AccessLevel.Player && !Engines.VeteranRewards.RewardSystem.CheckIsUsableBy( from, this, null ) )
				return;

			if ( IsChildOf( from.Backpack ) )
			{
				if ( !from.IsBodyMod )
				{
					from.SendLocalizedMessage( 1076194 ); // Select a place where you would like to put your statue.
					from.Target = new CharacterStatueTarget( this, StatueType );
				}
				else
					from.SendLocalizedMessage( 1073648 ); // You may only proceed while in your original state...
			}
			else
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
		}

		public void Delete( bool keepstatue )
		{
			if ( keepstatue )
				m_Statue = null;

			Delete();
		}

		public override void OnDelete()
		{
			base.OnDelete();

			if ( m_Statue != null )
				m_Statue.Delete();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version

			writer.Write( (Mobile) m_Statue );
			writer.Write( (bool) m_IsRewardItem );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();

			m_Statue = reader.ReadMobile() as CharacterStatue;
			m_IsRewardItem = reader.ReadBool();
		}
	}

	public class CharacterStatueTarget : Target
	{
		private Item m_Maker;
		private StatueType m_Type;

		public CharacterStatueTarget( Item maker, StatueType type ) : base( -1, true, TargetFlags.None )
		{
			m_Maker = maker;
			m_Type = type;
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			IPoint3D p = targeted as IPoint3D;
			Map map = from.Map;

			if ( p == null || map == null || m_Maker == null || m_Maker.Deleted )
				return;

			if ( m_Maker.IsChildOf( from.Backpack ) )
			{
				SpellHelper.GetSurfaceTop( ref p );
				BaseHouse house = null;
				Point3D loc = new Point3D( p );

				if ( targeted is Item && !((Item) targeted).IsLockedDown && !((Item) targeted).IsSecure && !(targeted is AddonComponent) )
				{
					from.SendLocalizedMessage( 1076191 ); // Statues can only be placed in houses.
					return;
				}
				else if ( from.IsBodyMod )
				{
					from.SendLocalizedMessage( 1073648 ); // You may only proceed while in your original state...
					return;
				}

				AddonFitResult result = CouldFit( loc, map, from, ref house );

				if ( result == AddonFitResult.Valid || from.AccessLevel >= AccessLevel.GameMaster )
				{
					CharacterStatue statue = new CharacterStatue( from, m_Type );
					CharacterStatuePlinth plinth = new CharacterStatuePlinth( statue );

					if ( house != null )
						house.Addons.Add( plinth );

					if ( m_Maker is IRewardItem )
						statue.IsRewardItem = ( (IRewardItem) m_Maker).IsRewardItem;

					statue.Plinth = plinth;
					plinth.Hue = statue.Hue;
					plinth.MoveToWorld( loc, map );
					statue.InvalidatePose();

					CharacterStatue backup = null;
					CharacterStatueDeed deed = m_Maker as CharacterStatueDeed;

					if ( deed != null )
					{
						backup = deed.Statue;
						deed.Delete( true );
					}
					else
						m_Maker.Delete();

					CharacterStatue.CloseGump( from );

					from.SendGump( new CharacterStatueGump( statue, from, backup ) );
				}
				else if ( result == AddonFitResult.Blocked )
					from.SendLocalizedMessage( 500269 ); // You cannot build that there.
				else if ( result == AddonFitResult.NotInHouse )
					from.SendLocalizedMessage( 1076192 ); // Statues can only be placed in houses where you are the owner or co-owner.
				else if ( result == AddonFitResult.DoorsNotClosed )
					from.SendMessage( "You must close all house doors before placing this." );
				else if ( result == AddonFitResult.DoorTooClose )
					from.SendLocalizedMessage( 500271 ); // You cannot build near the door.
			}
			else
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
		}

		public static AddonFitResult CouldFit( Point3D p, Map map, Mobile from, ref BaseHouse house )
		{
			ArrayList houses = new ArrayList();
			if ( house != null )
				houses.Add( house );

			if ( !map.CanFit( p.X, p.Y, p.Z, 20, true, true, true ) )
				return AddonFitResult.Blocked;
			else if ( !BaseAddon.CheckHouse( from, p, map, 20, houses ) )
				return AddonFitResult.NotInHouse;
			else
				return CheckDoors( p, 20, house );
		}

		public static AddonFitResult CheckDoors( Point3D p, int height, BaseHouse house )
		{
			if ( house != null )
			{
				ArrayList doors = house.Doors;

				for ( int i = 0; i < doors.Count; i ++ )
				{
					BaseDoor door = doors[ i ] as BaseDoor;

					if ( door != null && door.Open )
						return AddonFitResult.DoorsNotClosed;

					Point3D doorLoc = door.GetWorldLocation();
					int doorHeight = door.ItemData.CalcHeight;

					if ( Utility.InRange( doorLoc, p, 1 ) && (p.Z == doorLoc.Z || ((p.Z + height) > doorLoc.Z && (doorLoc.Z + doorHeight) > p.Z)) )
						return AddonFitResult.DoorTooClose;
				}
			}

			return AddonFitResult.Valid;
		}
	}
}