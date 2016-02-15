using System;
using Server.Items;
using Server.Targeting;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "a vampiric spider corpse" )]
	public class VampiricSpider : BaseCreature
	{
		[Constructable]
		public VampiricSpider() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a vampiric spider";
			Body = 28;
			Hue = 1157;
			BaseSoundID = 0x388;

			SetStr( 76, 100 );
			SetDex( 76, 95 );
			SetInt( 36, 60 );

			SetHits( 8000 );
			SetMana( 0 );

			SetDamage( 20, 20 );

			/*SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 15, 20 );
			SetResistance( ResistanceType.Poison, 25, 35 );*/

			SetSkill( SkillName.Poisoning, 100.0, 100.0 );
			SetSkill( SkillName.MagicResist, 80.0, 80.0 );
			SetSkill( SkillName.Magery, 80.0, 80.0 );
			SetSkill( SkillName.Meditation, 80.0, 80.0 );
			SetSkill( SkillName.EvalInt, 80.0, 80.0 );
			SetSkill( SkillName.Wrestling, 150.0, 150.0 );

			Fame = 600;
			Karma = -600;

			VirtualArmor = 16;

			Tamable = false;

			if ( Utility.Random( 0, 100 ) == 0 )
				PackItem( new SexChangeDeed() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Poor );
		}

		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Arachnid; } }
		public override Poison PoisonImmune{ get{ return Poison.Regular; } }
		public override Poison HitPoison{ get{ return Poison.Deadly; } }

		public VampiricSpider( Serial serial ) : base( serial )
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