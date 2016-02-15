using System;
using Server;
using Server.Misc;
using Server.Items;
using System.Collections;
using Server.Spells.Seventh;
using Server.Spells.Fifth;
//using Server.Engines.SilenceAddon;

namespace Server.Mobiles
{
	[CorpseName( "the fading remains of a spirit" )]
	public class GhostPast : BaseCreature
	{
		private Timer m_Timer;
		private static bool m_Active;

		[Constructable]
		public GhostPast() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
		{
			Name = "Ghost of the Past";
			Body = 970;
			Hue = 22222;
			BaseSoundID = 959;
			Kills = 5;

			SetStr( 750 );
			SetDex( 300 );
			SetInt( 1500 );

			SetHits( 10000 );
			SetMana( 20000 );

			SetDamage( 5 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Energy, 50 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 20, 40 );
			SetResistance( ResistanceType.Cold, 10, 30 );
			SetResistance( ResistanceType.Poison, 5, 10 );
			SetResistance( ResistanceType.Energy, 50, 70 );

			SetSkill( SkillName.MagicResist, 180.0 );
			SetSkill( SkillName.EvalInt, 150.0 );
			SetSkill( SkillName.Magery, 150.0 );
			SetSkill( SkillName.Tactics, 150 );
			SetSkill( SkillName.Wrestling, 150 );
			SetSkill( SkillName.DetectHidden, 200 );

			Fame = 12000;
			Karma = 12000;

			VirtualArmor = 20;

			m_Active = true;

			HoodedShroudOfShadows robe = new HoodedShroudOfShadows();
			robe.Hue = 22222;
			robe.Name = "";
			robe.Movable = false;
			robe.LootType = LootType.Blessed;
			AddItem( robe );

			m_Timer = new AppearTimer( this );
			m_Timer.Start();
			m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 2, 3 ) );
		}

		public override bool BardImmune{ get{ return true;} }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool CanDestroyObstacles { get { return true; } }
		public override bool AutoDispel { get { return true; } }

		public static bool Active
		{
			get { return m_Active; }
			//set { m_Active = value; }
		}

		private class AppearTimer : Timer
		{
			private Mobile m_Unhide;

			public AppearTimer( Mobile unhide ) : base( TimeSpan.FromSeconds( 1.2 ))
			{
				Priority = TimerPriority.OneSecond;
				m_Unhide = unhide;
			}

			protected override void OnTick()
			{
				m_Unhide.Hidden = false;
				m_Unhide.BoltEffect( 0 );
				m_Unhide.PlaySound( 1475 );
			}
		}

		public override int GetIdleSound()
		{
			return 1480;
		}

		public override int GetAngerSound()
		{
			return 0x107;
		}

		public override int GetDeathSound()
		{
			return 0xFD;
		}

		public override void OnDeath( Container c )
		{
			m_Active = false;
			c.DropItem( new CellKey() );

			if ( Utility.Random( 10 ) < 1 )
			c.DropItem( new GrayShirt() );

			base.OnDeath( c );
		}

		private DateTime m_NextAbilityTime;

		public override void OnThink()
		{
			if ( DateTime.Now >= m_NextAbilityTime )
			{
				Mobile combatant = this.Combatant;

				if ( combatant != null && combatant.Map == this.Map && combatant.InRange( this, 12 ) && combatant is PlayerMobile )
				{
					m_NextAbilityTime = DateTime.Now + TimeSpan.FromMinutes( Utility.RandomMinMax( 10, 12 ) );

					if ( this.CanBeHarmful( combatant ) )
					{
						this.Say("So mortal, you dare to challenge me!?!");
						this.MovingParticles( combatant, 0x379F, 7, 0, false, true, 3043, 4043, 0x211 );
						this.PlaySound( 1471 );
						Timer.DelayCall( TimeSpan.FromSeconds( 0.8 ), new TimerStateCallback( Kill ), combatant );
					}
				}
			}

			base.OnThink();
		}

		public void Kill( object state )
		{
			Mobile combatant = (Mobile)state;

			this.DoHarmful( combatant );
			int toDrain = combatant.Hits + 10;
			Hits += toDrain;
			combatant.Damage( toDrain, this );
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			if ( 0.80 >= Utility.RandomDouble())
				Polymorph( defender );

			base.OnGaveMeleeAttack( defender );
			{
				DoHarmful( defender );
				defender.PlaySound( 1301 );
				defender.SendMessage( "Some of your life force has been stolen!" );
				int toDrain = Utility.RandomMinMax( 20, 25 );
				Hits += toDrain;
				defender.Damage( toDrain, this );
			}
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			if ( 0.10 >= Utility.RandomDouble() && attacker is BaseCreature )
			{
				BaseCreature c = (BaseCreature)attacker;

				if ( c.Controlled && c.ControlMaster != null )
				{
					c.ControlTarget = c.ControlMaster;
					c.ControlOrder = OrderType.Attack;
					c.Combatant = c.ControlMaster;
				}
			}
		}

		public override void OnDamagedBySpell( Mobile caster )
		{
			base.OnDamagedBySpell( caster );

			if ( 0.25 >= Utility.RandomDouble() )
				Teleport( caster );
		}

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			if ( caster is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)caster;

				if ( bc.Controlled || bc.Summoned || bc.BardTarget == this )
				scalar = 0.33;
			}
		}

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)to;

				if ( bc.Controlled || bc.Summoned || bc.BardTarget == this )
				damage *= 10;
			}
		}

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Controlled || bc.Summoned || bc.BardTarget == this )
				damage /= 4;
			}
		}

		public void Polymorph( Mobile m )
		{
			if ( !m.CanBeginAction( typeof( PolymorphSpell) ) || !m.CanBeginAction( typeof( IncognitoSpell ) ) || m.IsBodyMod )
				return;

			IMount mount = m.Mount;

			if ( mount != null )
				mount.Rider = null;

			if ( m.Mounted )
				return;

			if ( m.BeginAction( typeof( PolymorphSpell) ) )
			{
				Item disarm = m.FindItemOnLayer( Layer.OneHanded );

				if ( disarm != null && disarm.Movable )
					m.AddToBackpack( disarm );

				disarm = m.FindItemOnLayer( Layer.TwoHanded );

				if ( disarm != null && disarm.Movable )
					m.AddToBackpack( disarm );

				m.BodyMod = 26;
				m.HueMod = 0;
				new ExpirePolymorphTimer( m ).Start();
			}
		}

		private class ExpirePolymorphTimer : Timer
		{
			private Mobile m_Owner;

			public ExpirePolymorphTimer( Mobile owner ) : base( TimeSpan.FromMinutes( 1.5 ) )
			{
				m_Owner = owner;

				Priority = TimerPriority.OneSecond;
			}

			protected override void OnTick()
			{
				if ( !m_Owner.CanBeginAction( typeof( PolymorphSpell ) ) )
				{
					m_Owner.BodyMod = 0;
					m_Owner.HueMod = -1;
					m_Owner.EndAction( typeof( PolymorphSpell ) );
				}
			}
		}

       		public void Teleport( Mobile caster )
        	{
          		Map map = this.Map;

			if ( map != null )
			{
	                   	for (int i = 0; i < 10; ++i)
                    		{
                	       		int x = X + (Utility.RandomMinMax(-1, 1));
        	              		int y = Y + (Utility.RandomMinMax(-1, 1));
	                      		int z = Z;

        	                	if (!map.CanFit(x, y, z, 16, false, false))
		                            continue;

		                        Point3D from = caster.Location;
	        	                Point3D to = new Point3D(x, y, z);

		                        if (!InLOS(to))
	        	                    continue;

		                        caster.Location = to;
        	                	caster.ProcessDelta();
                		        caster.Combatant = null;
					this.Combatant = caster;
		                        caster.Freeze(TimeSpan.FromSeconds(2.0));

		                        Effects.SendLocationParticles(EffectItem.Create(from, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
	        	                Effects.SendLocationParticles(EffectItem.Create(to, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);

	                	        Effects.PlaySound(to, map, 0x1FE);
        		        }
			}
                }

		public GhostPast( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
			m_Timer = new AppearTimer( this );
					m_Timer.Start();
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			/* m_Active <=> A ghost of the past exists
			 * which is the case when we deserialize one.
			 */
			m_Active = true;
		}
	}
}