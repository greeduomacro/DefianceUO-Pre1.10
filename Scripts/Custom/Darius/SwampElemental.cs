using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a swamp elemental corpse" )]
	public class SwampElemental : BaseCreature
	{
		[Constructable]
		public SwampElemental () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a swamp elemental";
			Body = 159;
			BaseSoundID = 278;
			Hue = 2004;


			SetStr( 526, 615 );
			SetDex( 80, 105 );
			SetInt( 2478, 3415 );

			SetHits( 2848, 3541 );

			SetDamage( 58, 70 );

			SetDamageType( ResistanceType.Physical, 0 );
			SetDamageType( ResistanceType.Poison, 50 );
			SetDamageType( ResistanceType.Energy, 50 );

			SetResistance( ResistanceType.Physical, 55, 65 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 50, 60 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.EvalInt, 150.1, 178.0 );
			SetSkill( SkillName.Magery, 120.1, 130.0 );
			SetSkill( SkillName.Meditation, 100.4, 110.0 );
			SetSkill( SkillName.MagicResist, 100.1, 110.0 );
			SetSkill( SkillName.Tactics, 100.0, 100.0 );
			SetSkill( SkillName.Wrestling, 100.0, 100.0 );

			Fame = 25000;
			Karma = -25000;

			VirtualArmor = 80;

			PackItem( new Nightshade( 10 ) );
			PackItem( new MandrakeRoot( 4 ) );
			PackItem( new Garlic( 4 ) );
			PackItem( new Ginseng( 4 ) );
		}

		public override void GenerateLoot( bool spawning )
		{
			AddLoot( LootPack.UltraRich, 2 );
			if ( !spawning )
			{
				if ( Utility.Random( 100 ) < 1 ) PackItem( new SpecialHairDye() );
				if ( Utility.Random( 100 ) < 1 ) PackItem( new SpecialBeardDye() );
			}
		}

		public override bool Unprovokable{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override int TreasureMapLevel{ get{ return 5; } }
		public override Poison HitPoison{ get{ return Poison.Deadly; } }
		public override double HitPoisonChance{ get{ return 0.50; } }


		public SwampElemental( Serial serial ) : base( serial )
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