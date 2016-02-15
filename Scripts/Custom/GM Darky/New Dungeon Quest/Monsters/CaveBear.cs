using System;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a cave bear corpse" )]
	public class CaveBear : BaseCreature
	{

		[Constructable]
		public CaveBear() : base( AIType.AI_Animal, FightMode.Agressor, 10, 1, 0.15, 0.2 )
		{
			Name = "an unearthed cave bear";
			Body = 212;
			BaseSoundID = 0xA3;
			Hue = Utility.RandomList( 1879, 1880, 1881, 1888, 1889, 1890 );

			SetStr( 226, 354 );
			SetDex( 405, 415 );
			SetInt( 10, 20 );

			SetHits( 426, 554 );

			SetDamage( 10, 15 );

			SetSkill( SkillName.MagicResist, 155.1, 160.0 );
			SetSkill( SkillName.Tactics, 150.1, 157.0 );
			SetSkill( SkillName.Wrestling, 96.1, 98.0 );

			Fame = 10000;
			Karma = 0;

			VirtualArmor = 70;

			PackGold( 500, 750 );

			switch ( Utility.Random( 3 ) )
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


		}

		public override int Meat{ get{ return 14; } }
		public override int Hides{ get{ return 40; } }
		public override HideType HideType{ get{ return HideType.Horned; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Bear; } }

		public CaveBear( Serial serial ) : base( serial )
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