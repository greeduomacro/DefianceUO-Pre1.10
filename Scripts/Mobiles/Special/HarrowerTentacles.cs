using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "lifeless tentacles" )] // TODO: Corpse name?
	public class HarrowerTentacles : BaseCreature
	{
		private Mobile m_Harrower;

		private DrainTimer m_Timer;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Harrower
		{
			get
			{
				return m_Harrower;
			}
			set
			{
				m_Harrower = value;
			}
		}

		[Constructable]
		public HarrowerTentacles() : this( null )
		{
		}

		public HarrowerTentacles( Mobile harrower ) : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			m_Harrower = harrower;

			Name = "tentacles of the harrower";
			Body = 129;
			BaseSoundID = 352;

			SetStr( 901, 1000 );
			SetDex( 126, 140 );
			SetInt( 1001, 1200 );

			SetHits( 541, 600 );

			SetDamage( 25, 25 );

			SetSkill( SkillName.Meditation, 100.0 );
			SetSkill( SkillName.MagicResist, 120.1, 140.0 );
			SetSkill( SkillName.Swords, 90.1, 100.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.Wrestling, 90.1, 100.0 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 60;

			PackReg( Utility.RandomMinMax( 25, 50 ) );
			PackGold( 100, 1000 );
			PackArmor( 0, 5 );
			PackWeapon( 3, 5 ); //Force, Power, Vanq
			PackArmor( 0, 5 );
			PackWeapon( 1, 3 ); //Ruin, Might, Force

			m_Timer = new DrainTimer( this );
			m_Timer.Start();

		}

		public override bool Unprovokable{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool DisallowAllMoves{ get{ return true; } }

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
				damage *= 3;
		}

		public override void CheckReflect( Mobile caster, ref bool reflect )
		{
			reflect = true; // Every spell is reflected back to the caster
		}

		public HarrowerTentacles( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_Harrower );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_Harrower = reader.ReadMobile();

					m_Timer = new DrainTimer( this );
					m_Timer.Start();

					break;
				}
			}
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
			private HarrowerTentacles m_Owner;

			public DrainTimer( HarrowerTentacles owner ) : base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
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

				if ( 0.4 < Utility.RandomDouble() )
					return;

				foreach ( Mobile m in m_Owner.GetMobilesInRange( 8 ) )
				{
					if ( m != m_Owner && m.Player && m_Owner.CanBeHarmful( m ) )
						m_ToDrain.Add( m );
				}

				foreach ( Mobile m in m_ToDrain )
				{
					m_Owner.DoHarmful( m );

					m.FixedParticles( 0x374A, 10, 15, 5013, 0x455, 0, EffectLayer.Waist );
					m.PlaySound( 0x231 );

					m.SendMessage( "You feel the life drain out of you!" );

					m_Owner.Hits += 30;

					if ( m_Owner.Harrower != null )
						m_Owner.Harrower.Hits += 30;

					m.Damage( 30, m_Owner );
				}

				m_ToDrain.Clear();
			}
		}
	}
}