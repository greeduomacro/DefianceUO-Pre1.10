using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a lava serpent corpse" )]
	[TypeAlias( "Server.Mobiles.Lavaserpant" )]
	public class LavaSerpent : BaseCreature
	{
		[Constructable]
		public LavaSerpent() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a lava serpent";
			Body = 90;
			BaseSoundID = 219;

			SetStr( 385, 415 );
			SetDex( 50, 80 );
			SetInt( 66, 85 );

			SetDamage( 10, 22 );

			SetSkill( SkillName.MagicResist, 25.3, 70.0 );
			SetSkill( SkillName.Tactics, 65.1, 70.0 );
			SetSkill( SkillName.Wrestling, 60.1, 80.0 );

			Fame = 4500;
			Karma = -4500;

			VirtualArmor = 32;

			switch ( Utility.Random( 6 ) )
			{
				case 0: PackItem( new Head() ); break;
				case 1: PackItem( new LeftArm() ); break;
				case 2: PackItem( new LeftLeg() ); break;
				case 3: PackItem( new RightArm() ); break;
				case 4: PackItem( new RightLeg() ); break;
				case 5: PackItem( new Torso() ); break;
			}

			PackItem( new SulfurousAsh( 3 ) );
			PackItem( new Bone() );
		}

		public override bool DeathAdderCharmable{ get{ return true; } }

		public override bool HasBreath{ get{ return true; } } // fire breath enabled
		public override int Meat{ get{ return 4; } }
		public override int Hides{ get{ return 30; } }
		public override HideType HideType{ get{ return HideType.Spined; } }

		public LavaSerpent(Serial serial) : base(serial)
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

			if ( BaseSoundID == -1 )
				BaseSoundID = 219;
		}
	}
}