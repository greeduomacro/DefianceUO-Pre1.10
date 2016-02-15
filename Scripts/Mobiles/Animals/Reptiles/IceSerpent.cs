using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "an ice serpent corpse" )]
	[TypeAlias( "Server.Mobiles.Iceserpant" )]
	public class IceSerpent : BaseCreature
	{
		[Constructable]
		public IceSerpent() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a giant ice serpent";
			Body = 89;
			BaseSoundID = 219;

			SetStr( 222, 244 );
			SetDex( 26, 49 );
			SetInt( 66, 85 );

			SetDamage( 7, 17 );

			SetSkill( SkillName.Anatomy, 35.1, 38.7 );
			SetSkill( SkillName.MagicResist, 25.1, 26.0 );
			SetSkill( SkillName.Tactics, 75.1, 76.0 );
			SetSkill( SkillName.Wrestling, 69.1, 80.0 );

			Fame = 3500;
			Karma = -3500;

			VirtualArmor = 32;

			switch ( Utility.Random( 75 ) )
			{
                                case 0: PackItem( new IceStaff() ); break;
			}

			PackGold( 50 );
			PackItem( Loot.RandomArmorOrShieldOrWeapon() );
		}

		public override bool DeathAdderCharmable{ get{ return true; } }

		public override int Meat{ get{ return 4; } }
		public override int Hides{ get{ return 30; } }
		public override HideType HideType{ get{ return HideType.Spined; } }

		public IceSerpent(Serial serial) : base(serial)
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