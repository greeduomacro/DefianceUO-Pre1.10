using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a dire wolf corpse" )]
	[TypeAlias( "Server.Mobiles.Direwolf" )]
	public class DireWolf : BaseCreature
	{
		[Constructable]
		public DireWolf() : base( AIType.AI_Melee,FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a dire wolf";
			Body = 23;
			BaseSoundID = 0xE5;

			SetStr( 96, 119 );
			SetDex( 81, 105 );
			SetInt( 36, 60 );

			SetDamage( 11, 17 );

			SetSkill( SkillName.MagicResist, 58.6, 75.0 );
			SetSkill( SkillName.Tactics, 50.1, 69.0 );
			SetSkill( SkillName.Wrestling, 64.1, 82.0 );

			Fame = 1500;
			Karma = -1500;

			VirtualArmor = 22;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 83.1;
		}

		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 14; } }
		public override HideType HideType{ get{ return HideType.Spined; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Canine; } }

		public DireWolf(Serial serial) : base(serial)
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