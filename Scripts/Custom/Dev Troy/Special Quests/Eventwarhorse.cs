using System;
using Server;
using Server.Mobiles;

namespace Server.Factions
{
	[CorpseName( "a war horse corpse" )]
	public class EventWarHorse : BaseMount
	{
		private Faction m_Faction;

		[CommandProperty( AccessLevel.GameMaster, AccessLevel.Seer )]
		public Faction Faction
		{
			get{ return m_Faction; }
			set
			{
				m_Faction = value;

				Body = ( m_Faction == null ? 0xE2 : m_Faction.Definition.WarHorseBody );
				ItemID = ( m_Faction == null ? 0x3EA0 : m_Faction.Definition.WarHorseItem );
			}
		}

		public const int SilverPrice = 2000;
		public const int GoldPrice = 8000;

		[Constructable]
		public EventWarHorse() : this( null )
		{
		}

		public EventWarHorse( Faction faction ) : base( "a war horse", 0xE2, 0x3EA0, AIType.AI_Melee, FightMode.Agressor, 10, 1, 0.2, 0.4 )
		{
			BaseSoundID = 0xA8;

			SetStr( 400 );
			SetDex( 180 );
			SetInt( 100 );

			SetHits( 300 );
			SetMana( 100 );
			SetStam( 180 );

			SetDamage( 15, 25 );

			SetSkill( SkillName.MagicResist, 100 );
			SetSkill( SkillName.Tactics, 120 );
			SetSkill( SkillName.Wrestling, 120 );

			Fame = 300;
			Karma = 300;

			Tamable = true;
			ControlSlots = 1;

			Faction = faction;
		}

		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }
		public override bool IsBondable{ get{ return false; } }

		public EventWarHorse( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			PlayerState pl = PlayerState.Find( from );

			if ( pl == null )
				from.SendLocalizedMessage( 1010366 ); // You cannot mount a faction war horse!
			else if ( pl.Faction != this.Faction )
				from.SendLocalizedMessage( 1010367 ); // You cannot ride an opposing faction's war horse!
			else if ( pl.Rank.Rank < 2 )
				from.SendLocalizedMessage( 1010368 ); // You must achieve a faction rank of at least two before riding a war horse!
			else
				base.OnDoubleClick( from );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			Faction.WriteReference( writer, m_Faction );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					Faction = Faction.ReadReference( reader );
					break;
				}
			}
		}
	}
}