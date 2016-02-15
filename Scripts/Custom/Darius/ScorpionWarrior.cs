using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a scorpion corpse" )]
	public class ScorpionWarrior : BaseCreature
	{
		[Constructable]
		public ScorpionWarrior() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a scorpion warrior";
			Body = 48;
			BaseSoundID = 397;
			Hue = 1863;
			ActiveSpeed = 0.23;

			SetStr( 73, 115 );
			SetDex( 76, 95 );
			SetInt( 80, 114 );

			SetHits( 150, 200 );

			SetDamage( 12, 17 );

			SetDamageType( ResistanceType.Physical, 60 );
			SetDamageType( ResistanceType.Poison, 40 );

			SetResistance( ResistanceType.Physical, 20, 25 );
			SetResistance( ResistanceType.Fire, 10, 15 );
			SetResistance( ResistanceType.Cold, 20, 25 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 10, 15 );

			SetSkill( SkillName.Poisoning, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 80.1, 90.0 );
			SetSkill( SkillName.Tactics, 80.3, 85.0 );
			SetSkill( SkillName.Wrestling, 80.3, 95.0 );



			Fame = 3000;
			Karma = -3000;

			VirtualArmor = 30;

			Tamable = false;


			PackItem( new GreaterPoisonPotion() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
		}

		public override int Meat{ get{ return 3; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override Poison HitPoison{ get{ return (0.9 >= Utility.RandomDouble() ? Poison.Greater : Poison.Deadly); } }
		public override int TreasureMapLevel{ get{ return 1; } }

		public ScorpionWarrior( Serial serial ) : base( serial )
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