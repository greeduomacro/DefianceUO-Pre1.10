using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "an ostard corpse" )]
	public class FrenziedOstard : BaseMount
	{
		[Constructable]
		public FrenziedOstard() : this( "a frenzied ostard" )
		{
		}

		[Constructable]
		public FrenziedOstard( string name ) : base( name, 0xDA, 0x3EA4, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 0.2 )
		{
			Hue = Utility.RandomHairHue() | 0x8000;

			BaseSoundID = 0x275;

			SetStr( 101, 161 );
			SetDex( 106, 125 );
			SetInt( 10 );

			SetDamage( 11, 17 );

			SetSkill( SkillName.MagicResist, 71.1, 80.0 );
			SetSkill( SkillName.Tactics, 79.3, 92.0 );
			SetSkill( SkillName.Wrestling, 81.3, 93.0 );

			Fame = 1250;
			Karma = -500;

			VirtualArmor = 30;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 77.1;
		}

		public override int Meat{ get{ return 3; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat | FoodType.Fish | FoodType.Eggs | FoodType.FruitsAndVegies; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Ostard; } }

		public FrenziedOstard( Serial serial ) : base( serial )
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