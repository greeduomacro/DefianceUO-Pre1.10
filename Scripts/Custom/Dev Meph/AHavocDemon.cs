using System;
using System.Collections;
using Server;
using Server.Misc;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
	[CorpseName( "a demon corpse" )]
	public class AHavocDemon : BaseCreature
	{
		[Constructable]
		public AHavocDemon () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 3, 3 )
		{
			Name = "a havoc demon";
			Body = 40;
			BaseSoundID = 357;
			Kills = 23;
			Hue = 842;

			SetStr( 986, 1185 );
			SetDex( 177, 255 );
			SetInt( 3000, 3000 );

			SetHits( 1500, 1500 );

			SetDamage( 30, 30 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Fire, 25 );
			SetDamageType( ResistanceType.Energy, 25 );

			SetResistance( ResistanceType.Physical, 65, 80 );
			SetResistance( ResistanceType.Fire, 60, 80 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Anatomy, 25.1, 50.0 );
			SetSkill( SkillName.EvalInt, 200.1, 205.0 );
			SetSkill( SkillName.Magery, 200.5, 201.0 );
			SetSkill( SkillName.Meditation, 25.1, 50.0 );
			SetSkill( SkillName.MagicResist, 0.5, 1.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.Wrestling, 200.1, 200.5 );

			Fame = 24000;
			Karma = -24000;

			VirtualArmor = 90;

			PackItem( new Longsword() );
		}

		public override void GenerateLoot( bool spawning )
		{
			AddLoot( LootPack.FilthyRich, 3 );
			AddLoot( LootPack.MedScrolls, 2 );
			if ( !spawning )
				if ( Utility.Random( 100 ) < 25 ) PackItem( new BagOfReagents() );
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override int TreasureMapLevel{ get{ return 5; } }
		public override int Meat{ get{ return 1; } }
		public override bool BardImmune{ get{ return true; } }
		public override bool AutoDispel{ get{ return true; } }

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			if ( caster is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)caster;

				if ( bc.Controlled || bc.BardTarget == this )
				scalar = 0.25;
			}
		}

		public override void AlterMeleeDamageFrom( Mobile to, ref int damage )
		{
			if ( to is Dragon || to is WhiteWyrm || to is Drake || to is Nightmare || to is Wyvern || to is EvolutionDragon)
				damage /= 10;
		}

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is Dragon || to is WhiteWyrm || to is Drake || to is Nightmare || to is EvolutionDragon )
				damage *= 4;
		}

		public AHavocDemon( Serial serial ) : base( serial )
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