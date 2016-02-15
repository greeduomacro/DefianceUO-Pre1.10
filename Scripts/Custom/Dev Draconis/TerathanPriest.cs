using System;
using Server.Items;
using Server.Targeting;
using Server.Misc;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "a terathan priest corpse" )]
	public class TerathanPriest : BaseCreature
	{
		[Constructable]
		public TerathanPriest() : base( AIType.AI_Healer, FightMode.Agressor, 10, 1, 2.0, 2.0 )
		{
			Name = "a terathan priest";
			Body = 71;
			BaseSoundID = 594;
			Hue = 1281;

			SetStr( 100 );
			SetDex( 100 );
			SetInt( 500 );

			SetHits( 1000 );
			SetMana( 10000 );

			SetDamage( 10, 15 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 20, 25 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 15, 25 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 15, 25 );

			SetSkill( SkillName.Poisoning, 100.0 );
			SetSkill( SkillName.EvalInt, 120.0 );
			SetSkill( SkillName.Magery, 120.0 );
			SetSkill( SkillName.MagicResist, 300.0 );
			SetSkill( SkillName.Meditation, 120.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 120.0 );

			Fame = 6000;
			Karma = -6000;

			VirtualArmor = 60;

			switch ( Utility.Random( 8 ) )
			{
				case 0: PackItem( new SpidersSilk( 30 ) ); break;
				case 1: PackItem( new BlackPearl( 30 ) ); break;
				case 2: PackItem( new Bloodmoss( 30 ) ); break;
				case 3: PackItem( new Garlic( 30 ) ); break;
				case 4: PackItem( new Ginseng( 30 ) ); break;
				case 5: PackItem( new MandrakeRoot( 30 ) ); break;
				case 6: PackItem( new Nightshade ( 30 ) ); break;
				case 7: PackItem( new SulfurousAsh( 30 ) ); break;
			}
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 1 );
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool BardImmune{ get{ return true; } }
		public override bool AutoDispel{ get{ return true; } }
		//public override InhumanSpeech SpeechType{ get{ return InhumanSpeech.Terathan; } }

		public override void OnThink()
		{
				ArrayList list = new ArrayList();

				foreach ( Mobile m in this.GetMobilesInRange( 18 ) )
				{
					if ( m.Hits == m.HitsMax )
						continue;

					if ( m is TerathanWarrior || m is TerathanDrone || m is TerathanAvenger || m is TerathanMatriarch || m is TerathanDefender || m is TerathanBlademaster || m is TerathanBattlemage)
						list.Add( m );
				}

				foreach ( Mobile m in list )
				{
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.Hits += 25;

				}
			base.OnThink();
		}

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.TerathansAndOphidians; }
		}

		public TerathanPriest( Serial serial ) : base( serial )
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