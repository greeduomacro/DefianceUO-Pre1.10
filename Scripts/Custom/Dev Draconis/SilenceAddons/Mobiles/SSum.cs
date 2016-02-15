using System;
using System.Collections;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.ContextMenus;
using Server.Engines.Quests;

//Its a modified Chyloth to work with an enum

namespace Server.Engines.SilenceAddon
{
	public class SSum : BaseQuester
	{
		private BellType m_Type;

		[CommandProperty(AccessLevel.GameMaster)]
    		public BellType Type
       		{
            		get
            		{
            			return m_Type;
           		}
        	}

		[Constructable]
		public SSum( BellType type ) : base( null )
		{
			m_Type = type;
			Hue = 0x8455;
			Body = 0x190;

			switch ( type )
			{
				case BellType.DarkIron: Name = "Ardal"; break;
				case BellType.Wooden: Name = "Bracca"; break;
				case BellType.Blood: Name = "CylDra"; break;
				case BellType.Beast: Name = "Darius"; break;
				case BellType.Noxious: Name = "Ernal"; break;
			}

			EquipItem( new SSumShroud( m_Type ) );
			EquipItem( new SSumStaff() );
		}

		public SSum( Serial serial ) : base( serial )
		{
		}

		private Mobile m_AngryAt;
		private SilenceBell m_Bell;

		public SilenceBell Bell
		{
			get{ return m_Bell; }
			set{ m_Bell = value; }
		}

		public Mobile AngryAt
		{
			get{ return m_AngryAt; }
			set{ m_AngryAt = value; }
		}

		public virtual void BeginGiveWarning()
		{
			if ( Deleted || m_AngryAt == null )
				return;

			Timer.DelayCall( TimeSpan.FromSeconds( 4.0 ), new TimerCallback( EndGiveWarning ) );
		}

		public virtual void EndGiveWarning()
		{
			if ( Deleted || m_AngryAt == null )
				return;

			this.Say("Mortal, why have you summoned me?", m_AngryAt.Name );
			this.Say("Give me the item I need or be damned!" );

			BeginSummonFake();
		}

		public virtual void BeginSummonFake()
		{
			if ( Deleted || m_AngryAt == null )
				return;

			Timer.DelayCall( TimeSpan.FromSeconds( 30.0 ), new TimerCallback( EndSummonFake ) );
		}

		public virtual void BeginRemove( TimeSpan delay )
		{
			Timer.DelayCall( delay, new TimerCallback( EndRemove ) );
		}

		public virtual void EndRemove()
		{
			if ( Deleted )
				return;

			Point3D loc = this.Location;
			Map map = this.Map;

			Effects.SendLocationParticles( EffectItem.Create( loc, map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 0, 0, 2023, 0 );
			Effects.PlaySound( loc, map, 0x1FE );

			Delete();
		}

		private static int[] m_Offsets = new int[]
			{
				-1, -1,
				-1,  0,
				-1,  1,
				0, -1,
				0,  1,
				1, -1,
				1,  0,
				1,  1
			};

		private BaseFakeMob m_Mob;

		public virtual void EndSummonFake()
		{
			if ( Deleted || m_AngryAt == null )
				return;

			Map map = m_AngryAt.Map;

			if ( map == null )
				return;

			if ( m_AngryAt.Region != this.Region )
				return;
			this.Say("Now you shall suffer at the hands of my pet!" );
			this.Say( "MUHAHAHAHA, DIE NOW MORTAL!!!" );

			switch ( m_Type )
				{
					case BellType.DarkIron: m_Mob = new FakeManaWisp(); break;
					case BellType.Wooden: m_Mob = new FakeWindcaller(); break;
					case BellType.Blood: m_Mob = new FakeFallenHero(); break;
					case BellType.Beast: m_Mob = new FakeSkeletalDragon(); break;
					case BellType.Noxious: m_Mob = new FakeWarlord(); break;
				}

			int offset = Utility.Random( 8 ) * 2;

			bool foundLoc = false;

			for ( int i = 0; i < m_Offsets.Length; i += 2 )
			{
				int x = m_AngryAt.X + m_Offsets[(offset + i) % m_Offsets.Length];
				int y = m_AngryAt.Y + m_Offsets[(offset + i + 1) % m_Offsets.Length];

				if ( map.CanSpawnMobile( x, y, m_AngryAt.Z ) )
				{
					m_Mob.MoveToWorld( new Point3D( x, y, m_AngryAt.Z ), map );
					foundLoc = true;
					break;
				}
				else
				{
					int z = map.GetAverageZ( x, y );

					if ( map.CanSpawnMobile( x, y, z ) )
					{
						m_Mob.MoveToWorld( new Point3D( x, y, z ), map );
						foundLoc = true;
						break;
					}
				}
			}

			if ( !foundLoc )
				m_Mob.MoveToWorld( m_AngryAt.Location, map );

			m_Mob.Combatant = m_AngryAt;

			if ( m_Bell != null )
				m_Bell.Fake = m_Mob;
		}

		private BaseBellBoss m_Boss;

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			bool IsQuestItem = false;

			if ( dropped is BloodKey && m_Type == BellType.DarkIron &&  Tallon.Active == false )
			{
				IsQuestItem = true;
			}
			if ( dropped is WoodenKey && m_Type == BellType.Wooden &&  Zirux.Active == false)
			{
				IsQuestItem = true;
			}
			if ( dropped is ShimmeringKey && m_Type == BellType.Blood &&  Krog.Active == false)
			{
				IsQuestItem = true;
			}
			if ( dropped is ClawKey && m_Type == BellType.Beast &&  Alfirix.Active == false)
			{
				IsQuestItem = true;
			}
			if ( dropped is VenomKey && m_Type == BellType.Noxious &&  Ignis.Active == false)
			{
				IsQuestItem = true;
			}
			if (
                dropped is BloodKey && m_Type != BellType.DarkIron
                || dropped is WoodenKey && m_Type != BellType.Wooden
                || dropped is ShimmeringKey && m_Type != BellType.Blood
                || dropped is ClawKey && m_Type != BellType.Beast
                || dropped is VenomKey && m_Type != BellType.Noxious
               )
               {
                SayTo(from, "This is not the key I desire you pathetic mortal!");
                from.Poison = Poison.Lethal;
                from.Freeze (TimeSpan.FromSeconds(15.0));
                return base.OnDragDrop( from, dropped );
               }

			if ( IsQuestItem )
			{
				dropped.Delete();

				BeginRemove( TimeSpan.FromSeconds( 4.0 ) );

				if ( m_AngryAt == from )
					m_AngryAt = null;

				switch ( m_Type )
				{
					case BellType.DarkIron: m_Boss = new Tallon(); break;
					case BellType.Wooden: m_Boss = new Zirux(); break;
					case BellType.Blood: m_Boss = new Krog(); break;
					case BellType.Beast: m_Boss = new Alfirix(); break;
					case BellType.Noxious: m_Boss = new Ignis(); break;
				}

				m_Boss.MoveToWorld( this.Location, this.Map );
				SayTo(from, "So mortal you bring me the correct item, now go slay the creature!");
				from.AddToBackpack( new ItemClaimer() );
				from.SendMessage( "You have recieved an item that is used to collect your reward, if there is one..." );
				return false;
			}
			else
			{
        		Atomic bomb = new Atomic();
        		bomb.MoveToWorld(from.Location, from.Map);
				return base.OnDragDrop( from, dropped );
			}
		}

		public override bool CanTalkTo( PlayerMobile to )
		{
			return false;
		}

		public override void OnTalk( PlayerMobile player, bool contextMenu )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_Type );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_Type = (BellType)reader.ReadInt();
		}
	}

	public class SSumShroud : Item
	{
		private BellType m_Type;

		[CommandProperty(AccessLevel.GameMaster)]
    		public BellType Type
       		{
            		get
            		{
            			return m_Type;
           		}
        	}

		[Constructable]
		public SSumShroud( BellType type ) : base( 0x204E )
		{
			m_Type = type;
			Layer = Layer.OuterTorso;
			switch ( type )
			{
				case BellType.DarkIron: Hue = 0x83A; Name = "Ardal's Shroud"; break;
				case BellType.Wooden: Hue = 0x6D7; Name = "Bracca's Shroud"; break;
				case BellType.Blood: Hue = 0x485; Name = "CylDra's Shroud"; break;
				case BellType.Beast: Hue = 0x3E8; Name = "Darius's Shroud"; break;
				case BellType.Noxious: Hue = 0x4F3; Name = "Ernal's Shroud"; break;
			}
		}

		public SSumShroud( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_Type );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_Type = (BellType)reader.ReadInt();
		}
	}

	public class SSumStaff : BlackStaff
	{
		[Constructable]
		public SSumStaff()
		{
			Hue = 0x482;
			Name = "a magical staff";
		}

		public SSumStaff( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}