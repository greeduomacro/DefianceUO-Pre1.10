using System;
using Server.Items;
using Server.Misc;
using Server.Targeting;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "a terathan defender corpse" )]
	public class TerathanDefender : BaseCreature
	{
		[Constructable]
		public TerathanDefender() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 5.0, 4.5 )
		{
			Name = "a terathan defender";
			Body = 70;
			BaseSoundID = 589;
			Hue = 1109;

			SetStr( 200 );
			SetDex( 200 );
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
			SetSkill( SkillName.MagicResist, 250.0 );
			SetSkill( SkillName.Tactics, 150.0 );
			SetSkill( SkillName.Wrestling, 150.0 );

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
		//public override InhumanSpeech SpeechType{ get{ return InhumanSpeech.Terathan; } }

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			if ( caster is PlayerMobile )
			{
			scalar = 0.10;
			}
		}

		public override void CheckReflect( Mobile caster, ref bool reflect )
		{
				reflect = true;
		}

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
				damage /= 10;
		}

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.TerathansAndOphidians; }
		}

		public TerathanDefender( Serial serial ) : base( serial )
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