using System;
using Server.Items;
using Server.Targeting;
using Server.Misc;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "a terathan blademaster corpse" )]
	public class TerathanBlademaster : BaseCreature
	{
		[Constructable]
		public TerathanBlademaster() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.5, 0.8 )
		{
			Name = "a terathan blademaster";
			Body = 70;
			BaseSoundID = 589;
			Hue = Utility.RandomBlueHue();

			SetStr( 400 );
			SetDex( 400 );
			SetInt( 100 );

			SetHits( 5000 );
			SetMana( 0 );

			SetDamage( 50, 50 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 30, 35 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 25, 35 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 25, 35 );

			SetSkill( SkillName.Poisoning, 100.0 );
			SetSkill( SkillName.MagicResist, 200.0 );
			SetSkill( SkillName.Tactics, 160.0 );
			SetSkill( SkillName.Wrestling, 160.0 );

			Fame = 9000;
			Karma = -9000;

			VirtualArmor = 100;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 1 );
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool BardImmune{ get{ return true; } }
		public override bool AutoDispel{ get{ return true; } }
		public override Poison HitPoison{ get{ return Poison.Lethal; } }
		//public override InhumanSpeech SpeechType{ get{ return InhumanSpeech.Terr; } }

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			scalar = 0.50;
		}

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			damage *= 2;
		}

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.TerathansAndOphidians; }
		}

		public TerathanBlademaster( Serial serial ) : base( serial )
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