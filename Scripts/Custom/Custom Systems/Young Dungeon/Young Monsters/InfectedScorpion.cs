using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a scorpion corpse" )]
	public class InfectedScorpion : BaseCreature
	{
		[Constructable]
		public InfectedScorpion() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a infected scorpion";
			Body = 48;
			BaseSoundID = 397;
			Hue = 1196;

			SetStr( 73, 115 );
			SetDex( 76, 95 );
			SetInt( 16, 30 );

			SetHits( 70, 90 );
			SetMana( 0 );

			SetDamage( 15, 25 );

			SetDamageType( ResistanceType.Physical, 60 );
			SetDamageType( ResistanceType.Poison, 40 );

			SetResistance( ResistanceType.Physical, 20, 25 );
			SetResistance( ResistanceType.Fire, 10, 15 );
			SetResistance( ResistanceType.Cold, 20, 25 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 10, 15 );

			SetSkill( SkillName.Poisoning, 80.1, 100.0 );
			SetSkill( SkillName.MagicResist, 30.1, 35.0 );
			SetSkill( SkillName.Tactics, 60.3, 75.0 );
			SetSkill( SkillName.Wrestling, 50.3, 65.0 );

			Fame = 2000;
			Karma = -2000;

			VirtualArmor = 28;

			Tamable = false;
			PackItem( new LesserPoisonPotion() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager, 3 );
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 15 ) < 1 )
			c.AddItem( new GreenBall() );

			base.OnDeath( c );
		}

		public override int Meat{ get{ return 1; } }
		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		public override Poison HitPoison{ get{ return (0.8 >= Utility.RandomDouble() ? Poison.Greater : Poison.Deadly); } }
		public override int TreasureMapLevel{ get{ return 3; } }

		public InfectedScorpion( Serial serial ) : base( serial )
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