using System;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a savage bear corpse" )]
	public class FishingBear : BaseCreature
	{

		[Constructable]
		public FishingBear() : base( AIType.AI_Animal, FightMode.Agressor, 10, 1, 0.4, 0.6 )
		{
			Name = "a savage fishing bear";
			Body = 212;
			BaseSoundID = 0xA3;
			Hue = Utility.RandomList( 2419, 2420, 2421, 2422, 2423, 2424 );

			SetStr( 1026, 1054 );
			SetDex( 10, 15 );
			SetInt( 10, 20 );

			SetDamage( 25, 30 );

			SetSkill( SkillName.MagicResist, 55.1, 60.0 );
			SetSkill( SkillName.Tactics, 130.1, 137.0 );
			SetSkill( SkillName.Wrestling, 116.1, 122.0 );

			Fame = 10000;
			Karma = 0;

			VirtualArmor = 84;

			PackGold( 800, 950 );
			PackItem( new Fish( Utility.Random( 2, 10 ) ) );

			switch ( Utility.Random( 2 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 10 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 10 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

		}

		public override int Meat{ get{ return 14; } }
		public override int Hides{ get{ return 40; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Bear; } }

		public FishingBear( Serial serial ) : base( serial )
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}