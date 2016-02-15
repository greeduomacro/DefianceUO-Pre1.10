using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a monstrous toad corpse" )]
	public class MonstrousToad : BaseCreature
	{
		[Constructable]
		public MonstrousToad() : base( AIType.AI_Animal, FightMode.Agressor, 10, 1, 0.15, 0.2 )
		{
			Name = "a monstrous toad";
			Body = 80;
			BaseSoundID = 0x26B;
			Hue = Utility.RandomList( 1428, 1310, 1246, 1419, 1410, 1401, 1337, 1328 );

			SetStr( 276, 300 );
			SetDex( 96, 105 );
			SetInt( 20 );

			SetDamage( 19, 22 );

			SetSkill( SkillName.MagicResist, 65.1, 70.0 );
			SetSkill( SkillName.Tactics, 80.1, 82.0 );
			SetSkill( SkillName.Wrestling, 80.1, 83.0 );

			Fame = 7500;
			Karma = -7500;

			VirtualArmor = 54;

			PackGold( 300, 400 );
			PackItem( new Fish( Utility.RandomMinMax( 2, 15 ) ) );

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

		public override Poison PoisonImmune{ get{ return Poison.Regular; } }
		public override Poison HitPoison{ get{ return Poison.Regular; } }
		public override int Meat{ get{ return 2; } }
		public override int Hides{ get{ return 25; } }
		public override HideType HideType{ get{ return HideType.Horned; } }

		public MonstrousToad(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}