using System;
using Server;
using Server.Misc;
using Server.Items;
using System.Collections;

namespace Server.Mobiles
{
	public class ExplodingSanta : BaseCreature
	{
		public override bool DeleteCorpseOnDeath{ get{ return true; } }

		private Mobile m_Target;

		[CommandProperty( AccessLevel.Seer )]
		public Mobile ExplodeTarget
		{
			get{ return m_Target; }
			set{ m_Target = value; }
		}

		[Constructable]
		public ExplodingSanta() : base( AIType.AI_Melee, FightMode.None, 10, 1, 0.001, 0.001 )
		{
			Name = "a santa";
			Body = 400;
			Hue = 33814;
			SpeechHue = 1150;

			m_Target = null;

			SetStr( 100 );
			SetDex( 100 );
			SetInt( 100 );

			SetHits( 200 );

			SetDamage( 1, 2 );

			SetSkill( SkillName.MagicResist, 150.0 );

			Robe robe = new Robe();
			robe.Hue = 37;
			robe.Movable = false;
			AddItem( robe );

			WizardsHat hat = new WizardsHat();
			hat.Hue = 37;
			hat.Movable = false;
			AddItem( hat );

			LongBeard b = new LongBeard();
			b.Hue = 1150;
			b.Movable = false;
			AddItem( b );

			LongHair h = new LongHair();
			h.Hue = 1150;
			h.Movable = false;
			AddItem( h );

			AddItem( new Boots() );
		}

		private DateTime m_HoHoHo;

		public override void OnThink()
		{
			if ( DateTime.Now >= m_HoHoHo )
			{
				m_HoHoHo = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 10, 15 ) );

				this.Say( "HO HO HO HO" );
			}

			if ( m_Target == null )
			{
				ArrayList list = new ArrayList();

            			foreach ( Mobile m in this.GetMobilesInRange( 20 ) )
            			{
                			if ( m is PlayerMobile && m.AccessLevel == AccessLevel.Player && m.Hidden != true)
						        list.Add( m );
            			}

            			int index = Utility.Random( list.Count );

				if ( list.Count > 0 )
                    	m_Target = (Mobile)list[index];
			} else {
                if (!m_Target.Hidden && m_Target.Alive && m_Target.InRange(this, 20))
				{
					int x = m_Target.X;
					int y = m_Target.Y;

					Point2D loc = new Point2D( x, y );
					this.TargetLocation = loc;
					this.Say( "HO HO HO HO" );

					if ( m_Target.InRange( this, 1 ) )
					{
						ArrayList list = new ArrayList();

						foreach ( Mobile m in this.GetMobilesInRange( 2 ) )
						{
							if ( m == this || !CanBeHarmful( m ) )
								continue;

							if ( m is BaseCreature || m is PlayerMobile )
								list.Add( m );
						}

						foreach ( Mobile m in list )
						{
							DoHarmful( m );

							int xx = this.X;
							int yy = this.Y;
							int zz = this.Z;

							Point3D loca = new Point3D( xx, yy, zz );

							Effects.SendLocationParticles( EffectItem.Create( loca, this.Map, EffectItem.DefaultDuration ), 0x36BD, 20, 10, 5044 );
							Effects.PlaySound( loca, this.Map, 0x307 );

							m.SendMessage( "You are hurt by the exploding santa!" );

							m.Damage( 50, this );

							this.Delete();
						}
					}
				}
				else
				{
					Point2D loc = new Point2D( 0, 0 );
					this.TargetLocation = loc;
					m_Target = null;
				}
			}
			base.OnThink();
		}

		public ExplodingSanta( Serial serial ) : base( serial )
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