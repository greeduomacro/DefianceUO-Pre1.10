using System;
using Server.Items;
using Server.Targeting;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "a Undead Rat corpse" )]
	public class UndeadRat : BaseCreature
	{
		[Constructable]
		public UndeadRat() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a undead rat";
			Body = 0xD7;
			BaseSoundID = 0x188;
			Hue = 930;
			SetStr( 176, 200 );
			SetDex( 176, 195 );
			SetInt( 136, 160 );

			SetHits( 146, 160 );
			SetMana( 110 );

			SetDamage( 5, 13 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 15, 20 );
			SetResistance( ResistanceType.Poison, 25, 35 );

			SetSkill( SkillName.Poisoning, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 125.1, 140.0 );
			SetSkill( SkillName.Tactics, 135.1, 150.0 );
			SetSkill( SkillName.Wrestling, 150.1, 165.0 );

			Fame = 600;
			Karma = -600;

			VirtualArmor = 76;

			Tamable = false;

			switch ( Utility.Random( 30 ))
			{
				case 0: PackItem( new MouldyCheese() ); break;
			}
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
		}

		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Arachnid; } }
		public override Poison PoisonImmune{ get{ return Poison.Regular; } }
		public override Poison HitPoison{ get{ return Poison.Regular; } }

		public UndeadRat( Serial serial ) : base( serial )
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