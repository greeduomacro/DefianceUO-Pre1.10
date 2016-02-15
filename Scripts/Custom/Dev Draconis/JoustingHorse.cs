using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a horse corpse" )]
	public class JoustingHorse : BaseMount
	{
		[Constructable]
		public JoustingHorse() : this( "a jousters horse" )
		{
		}

		[Constructable]
		public JoustingHorse( string name ) : base( name, 0xE2, 0x3EA0, AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4 )
		{
			Body = 226;
			ItemID = 16032;
			BaseSoundID = 0xA8;

			SetStr( 100 );
			SetDex( 75 );
			SetInt( 20 );

			SetHits( 100 );
			SetMana( 0 );

			SetDamage( 5, 8 );

			SetSkill( SkillName.MagicResist, 100 );
			SetSkill( SkillName.Tactics, 100 );
			SetSkill( SkillName.Wrestling, 100 );

			Fame = 3000;
			Karma = 3000;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsDeadPet )
				return;

			if ( from.IsBodyMod && !from.Body.IsHuman )
			{
				from.SendLocalizedMessage( 1061628 ); // You can't do that while polymorphed.
				return;
			}

			if ( from.Mounted )
			{
				from.SendLocalizedMessage( 1005583 ); // Please dismount first.
				return;
			}

			if ( from.InRange( this, 1 ) )
			{
				Rider = from;
			}
			else
			{
				from.SendLocalizedMessage( 500206 ); // That is too far away to ride.
			}
		}

		public JoustingHorse( Serial serial ) : base( serial )
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