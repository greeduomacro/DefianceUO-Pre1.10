using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a hell hound corpse" )]
	public class HellHound : BaseCreature
	{
		[Constructable]
		public HellHound() : base( AIType.AI_Melee, FightMode.Closest, 13, 1, 0.2, 0.4 )
		{
			Name = "a hell hound";
			Body = 98;
			BaseSoundID = 229;

			SetStr( 132, 174 );
			SetDex( 81, 105 );
			SetInt( 36, 60 );

			SetDamage( 14, 21 );

			SetSkill( SkillName.MagicResist, 77.8, 89.5 );
			SetSkill( SkillName.Tactics, 71.8, 89.2 );
			SetSkill( SkillName.Wrestling, 74.4, 90.3 );

			Fame = 3400;
			Karma = -3400;

			VirtualArmor = 34;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 85.5;

			PackItem( new SulfurousAsh( 5 ) );
			PackGold( 0, 150 );
		}

		public override int Meat{ get{ return 1; } }
                public override bool HasBreath{ get{ return true; } } // fire breath enabled
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Canine; } }

		public HellHound( Serial serial ) : base( serial )
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