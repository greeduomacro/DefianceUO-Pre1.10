using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a scorpion corpse" )]
	public class ScorpionQueen : BaseCreature
	{
		[Constructable]
		public ScorpionQueen() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a scorpion queen";
			Body = 48;
			BaseSoundID = 397;
			Hue = 2407;
			ActiveSpeed = 0.22;

			SetStr( 73, 115 );
			SetDex( 76, 95 );
			SetInt( 80, 114 );

			SetHits( 200, 250 );
			SetMana( 250 );

			SetDamage( 10, 15 );

			SetDamageType( ResistanceType.Physical, 60 );
			SetDamageType( ResistanceType.Poison, 40 );

			SetResistance( ResistanceType.Physical, 20, 25 );
			SetResistance( ResistanceType.Fire, 10, 15 );
			SetResistance( ResistanceType.Cold, 20, 25 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 10, 15 );

			SetSkill( SkillName.Poisoning, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 70.1, 75.0 );
			SetSkill( SkillName.Tactics, 60.3, 75.0 );
			SetSkill( SkillName.Wrestling, 50.3, 65.0 );
			SetSkill( SkillName.Magery, 85.3, 70.0 );
			SetSkill( SkillName.EvalInt, 75.1, 80.0 );


			Fame = 3500;
			Karma = -2000;

			VirtualArmor = 35;

			Tamable = false;


			PackItem( new LesserPoisonPotion() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
		}

		public override int Meat{ get{ return 2; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override Poison HitPoison{ get{ return (0.8 >= Utility.RandomDouble() ? Poison.Greater : Poison.Deadly); } }
		public override int TreasureMapLevel{ get{ return 1; } }

		public ScorpionQueen( Serial serial ) : base( serial )
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