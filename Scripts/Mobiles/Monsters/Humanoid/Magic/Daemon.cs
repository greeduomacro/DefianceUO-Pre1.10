using System;
using Server;
using Server.Items;
using Server.Factions;

namespace Server.Mobiles
{
	[CorpseName( "a daemon corpse" )]
	public class Daemon : BaseCreature
	{
		public override double DispelDifficulty{ get{ return 125.0; } }
		public override double DispelFocus{ get{ return 45.0; } }

		public override Faction FactionAllegiance{ get{ return Shadowlords.Instance; } }

		[Constructable]
		public Daemon () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "daemon" );
			Body = 9;
			BaseSoundID = 357;

			SetStr( 476, 505 );
			SetDex( 79, 94 );
			SetInt( 291, 325 );

			SetDamage( 9, 14 );

			SetSkill( SkillName.Magery, 75.1, 82.0 );
			SetSkill( SkillName.MagicResist, 86.1, 95.0 );
			SetSkill( SkillName.Tactics, 70.1, 79.0 );
			SetSkill( SkillName.Wrestling, 64.1, 80.0 );

			Fame = 12500;
			Karma = -12500;

			VirtualArmor = 55;
			ControlSlots = 5;

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackGem(); break;
				case 1: PackScroll( 3, 7 ); break;
				case 2: PackGem(); break;
			}

			PackGold( 300, 600 );
			PackArmor( 0, 4 );
			PackWeapon( 0, 4 );
                        PackJewel( 0.01 );
		}

		public override void OnDeath( Container c )
	  	{
			if ( Utility.Random( 50 ) <  1 )
				c.DropItem( new BasicPinkCarpet( PieceType.NWCorner ) );

			base.OnDeath( c );
	  	}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Regular; } }
		public override int TreasureMapLevel{ get{ return 4; } }
		public override int Meat{ get{ return 1; } }

		public Daemon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}