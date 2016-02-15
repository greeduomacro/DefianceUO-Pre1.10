using System;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a snake corpse" )]
	public class SilverSnake : BaseCreature
	{
		[Constructable]
		public SilverSnake() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.2 )
		{
			Name = "a silver serpent";
			Body = 52;
			Hue = 1072;
			BaseSoundID = 0xDB;

			SetStr( 22, 34 );
			SetDex( 36, 55 );
			SetInt( 6, 10 );

			SetHits( 22, 34 );
			SetMana( 0 );

			SetDamage( 3, 7 );

			SetSkill( SkillName.Poisoning, 100.1, 120.0 );
			SetSkill( SkillName.MagicResist, 35.1, 40.0 );
			SetSkill( SkillName.Tactics, 39.3, 44.0 );
			SetSkill( SkillName.Wrestling, 39.3, 44.0 );

			Fame = 1000;
			Karma = -1000;

			VirtualArmor = 36;

			Tamable = false;
			ControlSlots = 1;
			MinTameSkill = 79.1;

			switch( Utility.Random(300) )
	{
			case 0: PackItem( new EnchantedWood() ); break;
	}

		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Lethal; } }
		public override bool DeathAdderCharmable{ get{ return true; } }

		public override int Meat{ get{ return 1; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Eggs; } }

		public SilverSnake(Serial serial) : base(serial)
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