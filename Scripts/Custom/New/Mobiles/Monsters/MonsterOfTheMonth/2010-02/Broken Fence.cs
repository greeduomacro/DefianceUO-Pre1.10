using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Targeting;
using Server.Misc;

namespace Server.Mobiles
{
	[CorpseName( "a metallic corpse" )]
	public class BrokenFenceMOTM : BaseCreature
	{
		private DrainTimer m_Timer;

		[Constructable]
		public BrokenFenceMOTM() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 0.4 )
		{

			Name = "piece of a broken fence";
			Body = 574;
			Hue = 1175;
			BaseSoundID = 0;

			SetStr( 550, 600 );
			SetDex( 150 );
			SetInt( 25, 50 );

			SetHits( 400, 450 );

			SetDamage( 25, 40 );

			SetSkill( SkillName.MagicResist, 150.0 );
			SetSkill( SkillName.Wrestling, 100.0 );
			SetSkill( SkillName.Tactics, 200.0 );
			SetSkill( SkillName.Anatomy, 100.0 );


			Fame = 3500;
			Karma = -3500;

			VirtualArmor = 30;
			PackReg( 30 );
			PackItem( new Gold( 300, 700 ) );

			if ( Utility.Random( 200 ) < 1 ) PackItem( new BrokenFenceMOTMRare() );


			m_Timer = new DrainTimer( this );
			m_Timer.Start();


		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 1 );
			AddLoot( LootPack.FilthyRich, 1 );
			AddLoot( LootPack.Gems, 5 );
			AddLoot( LootPack.MedScrolls );
		}

		//Spell damage from controlled mobiles is scaled down by 0.01
		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			if ( caster is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)caster;

				if ( bc.Controlled )
				scalar = 0.01;
			}
		}

		//Melee damage from controlled mobiles is divided by 30
		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Controlled )
				damage /= 30;
			}
		}

				//Melee damage to controlled mobiles is multiplied by 5
				public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)to;

				if ( bc.Controlled )
				damage *= 5;
			}
		}


		public override int TreasureMapLevel{ get{ return 5; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool BardImmune{ get{ return true; } }
		public override bool AutoDispel{ get{ return true; } }



		public BrokenFenceMOTM( Serial serial ) : base( serial )
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
			m_Timer = new DrainTimer( this );
			m_Timer.Start();
		}


		public override void OnAfterDelete()
		{
			if ( m_Timer != null )
				m_Timer.Stop();

			m_Timer = null;

			base.OnAfterDelete();
		}

		private class DrainTimer : Timer
		{
			private BrokenFenceMOTM m_Owner;

			public DrainTimer( BrokenFenceMOTM owner ) : base( TimeSpan.FromSeconds( 5.0 ), TimeSpan.FromSeconds( 5.0 ) )
			{
				m_Owner = owner;
				Priority = TimerPriority.TwoFiftyMS;
			}

			private static ArrayList m_ToDrain = new ArrayList();

			protected override void OnTick()
			{
				if ( m_Owner.Deleted )
				{
					Stop();
					return;
				}

				foreach ( Mobile m in m_Owner.GetMobilesInRange( 9 ) )
				{
					if ( m == m_Owner || !m_Owner.CanBeHarmful( m ) )
						continue;

					if ( m is BaseCreature )
					{
						BaseCreature bc = m as BaseCreature;

						if ( bc.Controlled || bc.Summoned )
							m_ToDrain.Add( m );
					}
					else if ( m.Player )
					{
						m_ToDrain.Add( m );
					}
				}

				foreach ( Mobile m in m_ToDrain )
				{
					m_Owner.DoHarmful( m );

					m.FixedParticles( 0x374A, 10, 15, 5013, 1174, 0, EffectLayer.Waist );
					m.PlaySound( 0x1F1 );
					m.SendMessage( "An intense headache hits your head!" );
					int drain = Utility.RandomMinMax( 14, 30 );

					m_Owner.Hits += drain;

					m.Damage( drain, m_Owner );
				}

				m_ToDrain.Clear();
			}
		}
	}
}