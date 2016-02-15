using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	public class ConfusedFairy : BaseBoss
	{
		private Mobile m_Victim;

		[CommandProperty( AccessLevel.Seer )]
		public Mobile Victim
		{
			get{ return m_Victim; }
			set{ m_Victim = value; }
		}

		[Constructable]
		public ConfusedFairy() : base( AIType.AI_Mage)
		{
			Name = "a confused fairy";
			Body = 264;
			BaseSoundID = 0x467;
			m_Victim = null;

			SetStr( 100 );
			SetDex( 50 );
			SetInt( 1000 );

			SetHits( 150 );

			SetDamage( 5, 15 );

			SetSkill( SkillName.MagicResist, 160.0 );
			SetSkill( SkillName.Wrestling, 60.0 );
			SetSkill( SkillName.Magery, 140.0 );
			SetSkill( SkillName.Meditation, 140.0 );
			SetSkill( SkillName.EvalInt, 140.0 );

			Fame = 1000;
			Karma = -1000;

			VirtualArmor = 1;
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 10 ) < 1 )
				c.DropItem( new SnowDrop( Utility.RandomMinMax( 1, 3 ) ) );

			base.OnDeath( c );
		}

		private DateTime m_NextPlayer;

		public override void OnThink()
		{
            if (m_Victim != null && m_Victim.InRange(this, 8) && CanBeHarmful(m_Victim))
            {
                this.Freeze(TimeSpan.FromSeconds(2.0));
                m_Victim.Freeze(TimeSpan.FromSeconds(2.0));
                m_Victim.MovingParticles(this, 0x36F4, 1, 0, false, false, 32, 0, 9535, 1, 0, (EffectLayer)255, 0x100);
                m_Victim.Hits -= 2;
                this.Hits += 2;

                if (m_Victim.Hits == 0)
                {
                    m_Victim.Kill();
                    m_Victim = null;
                }
            }
            else
                m_Victim = null;

			if ( m_Victim == null && DateTime.Now >= m_NextPlayer )
			{
				Mobile combatant = this.Combatant;

				if ( combatant != null && combatant.Player && combatant.Map == this.Map && combatant.InRange( this, 20 ) )
				{
					m_NextPlayer = DateTime.Now + TimeSpan.FromSeconds( 20.0 );

					m_Victim = combatant;
				}
			}
			base.OnThink();
		}

		public override void OnDamagedBySpell( Mobile caster )
		{
			base.OnDamagedBySpell( caster );

			if ( m_Victim != null )
			{
				m_Victim = null;
			}
		}

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( m_Victim != null )
			{
				damage = 0;
			}
		}

		public ConfusedFairy( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}