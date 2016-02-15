using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a howling nightmare corpse" )]
	public class HowlingNightmare : BaseCreature
	{
		[Constructable]
		public HowlingNightmare() : base( AIType.AI_Melee,FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a howling nightmare";
			Body = 225;
			Hue = 1175;
			BaseSoundID = 0xE5;

			SetStr( 196, 219 );
			SetDex( 81, 105 );
			SetInt( 36, 60 );

			SetDamage( 12, 18 );

			SetSkill( SkillName.MagicResist, 58.6, 75.0 );
			SetSkill( SkillName.Tactics, 50.1, 69.0 );
			SetSkill( SkillName.Wrestling, 64.1, 82.0 );

			Fame = 1500;
			Karma = -1500;

			VirtualArmor = 22;
		}

		public override bool OnBeforeDeath()
		{
			if ( Utility.Random( 100 ) == 0 )
				PackItem( new ChaosToken() );

			return base.OnBeforeDeath();
		}

		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 14; } }
		public override HideType HideType{ get{ return HideType.Spined; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override Poison HitPoison{ get{ return Poison.Deadly; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Canine; } }

		public HowlingNightmare(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.WriteEncodedInt((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadEncodedInt();
		}
	}
}