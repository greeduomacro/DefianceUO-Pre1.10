using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a frost troll corpse" )]
	public class FrostTroll : BaseCreature
	{
		[Constructable]
		public FrostTroll() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a frost troll";
			Body = 55;
			BaseSoundID = 461;

			SetStr( 227, 258 );
			SetDex( 66, 85 );
			SetInt( 46, 70 );

			SetDamage( 14, 20 );

			SetSkill( SkillName.MagicResist, 65.1, 80.0 );
			SetSkill( SkillName.Tactics, 82.1, 99.7 );
			SetSkill( SkillName.Wrestling, 82.1, 95.8 );

			Fame = 4000;
			Karma = -4000;

			VirtualArmor = 50;

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackGem(); break;
				case 1: PackPotion(); break;
				case 2: PackItem( new Arrow( Utility.Random( 10, 15 ) ) ); break;
			}

			PackGold( 50, 150 );

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackItem( Loot.RandomWeapon() ); break;
				case 2: PackItem( Loot.RandomWeapon() ); break;
				case 3: PackItem( Loot.RandomWeapon() ); break;
				case 4: PackItem( Loot.RandomWeapon() ); break;
			}
		}

		public override int Meat{ get{ return 2; } }
		public override int TreasureMapLevel{ get{ return 1; } }

		public FrostTroll( Serial serial ) : base( serial )
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