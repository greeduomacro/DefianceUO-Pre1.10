using System;
using Server.Mobiles;
using Server.Factions;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("a silver serpent corpse")]
	[TypeAlias( "Server.Mobiles.Silverserpant" )]
	public class SilverSerpent : BaseCreature
	{
		public override Faction FactionAllegiance{ get{ return TrueBritannians.Instance; } }

		[Constructable]
		public SilverSerpent() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Body = 92;
			Name = "a silver serpent";
			BaseSoundID = 219;

			SetStr( 196, 335 );
			SetDex( 172, 280 );
			SetInt( 21, 37 );

			SetDamage( 5, 21 );

			SetSkill( SkillName.Poisoning, 95.1, 99.7 );
			SetSkill( SkillName.MagicResist, 95.1, 98.0 );
			SetSkill( SkillName.Tactics, 84.1, 94.0 );
			SetSkill( SkillName.Wrestling, 87.1, 98.0 );

			Fame = 7000;
			Karma = -7000;

			VirtualArmor = 40;

			PackGold( 100, 150 );
			PackGem();
			PackGem();

				if ( Utility.Random( 100 ) == 0 )
				PackItem( new RareCreamCarpet( PieceType.NorthEdge ));

		}

		public override bool DeathAdderCharmable{ get{ return true; } }

		public override int Meat{ get{ return 1; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Lethal; } }

		public SilverSerpent(Serial serial) : base(serial)
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