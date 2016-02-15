using System;
using Server;
using Server.Mobiles;
using Server.Items;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "an evil rabbit corpse" )]
	public class BigTeethRabbit : BaseCreature
	{
		static bool m_Active;
		public static bool Active
		{
			get{ return m_Active; }
			set{ m_Active = value; }
		}

		[Constructable]
		public BigTeethRabbit() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.05, 0.1 )
		{
			Name = "a rabbit with big teeth";
			Body = 205;
			m_Active = true;

			SetStr( 200 );
			SetDex( 200 );
			SetInt( 200 );

			SetHits( 10000 );
			SetMana( 0 );

			SetDamage( 20, 25 );

			SetSkill( SkillName.MagicResist, 280.0 );
			SetSkill( SkillName.Tactics, 120.0 );
			SetSkill( SkillName.Wrestling, 110.0 );

			Fame = 9000;
			Karma = -9000;

			VirtualArmor = 500;
		}

		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 1; } }
		public override bool BardImmune{ get{ return true; } }
		public override bool CanDestroyObstacles{ get{ return true; } }
		public override bool IsScaryToPets{ get{ return true; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public override void OnGaveMeleeAttack(Mobile defender)
		{
			base.OnGaveMeleeAttack(defender);

			BaseCreature c = defender as BaseCreature;

			if ( c != null && c.SummonMaster != null )
			{
				c.ControlTarget = c.SummonMaster;
				c.ControlOrder = OrderType.Attack;
				c.Combatant = c.SummonMaster;
				this.Combatant = c.SummonMaster;
			}

			if ( defender.Player )
			{
				defender.Freeze( TimeSpan.FromSeconds( 5.0 ) );
				defender.SendMessage( "You are frozen in sheer terror!" );
			}
		}

		public override void OnGotMeleeAttack(Mobile attacker)
		{
			base.OnGaveMeleeAttack(attacker);

			BaseCreature c = attacker as BaseCreature;

			if (c != null && c.SummonMaster != null)
			{
				c.ControlTarget = c.SummonMaster;
				c.ControlOrder = OrderType.Attack;
				c.Combatant = c.SummonMaster;
				this.Combatant = c.SummonMaster;
			}
		}

		public override bool IsEnemy( Mobile m )
		{
			BaseCreature c = m as BaseCreature;

			if ( c != null && c.Controlled )
				return false;

			return base.IsEnemy( m );
		}

		private DateTime m_CheckGrenadeTime;
		private DateTime m_CarpeJugularaTime;

		public override void OnThink()
		{
			if ( DateTime.Now >= m_CheckGrenadeTime )
			{
				m_CheckGrenadeTime = DateTime.Now + TimeSpan.FromSeconds( 1.2 );

				foreach ( Item item in this.GetItemsInRange( 2 ) )
				{
					if ( item is HolyHandGrenade )
					{
						Map map = this.Map;
						bool validLocation = false;
		        		Point3D loc = this.Location;

						for ( int j = 0; !validLocation && j < 10; ++j )
                        			{
                           				int x = item.X + Utility.RandomMinMax( -5, 5 );
                            				int y = item.Y + Utility.RandomMinMax( -5, 5 );
                           				int z = map.GetAverageZ( x, y );

                            				if ( validLocation = map.CanFit( x, y, this.Z, 16, false, false ) )
	                            				loc = new Point3D( x, y, Z );
                            				else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
	                            				loc = new Point3D( x, y, z );
                        			}

						this.Say( "*Dives for cover*" );
						Point3D from = this.Location;
                        			Point3D to = loc;
						this.Location = to;
                        			this.ProcessDelta();
						this.Freeze(TimeSpan.FromSeconds(2.0));
					}
				}
			}

			if ( DateTime.Now >= m_CarpeJugularaTime )
			{
				Mobile combatant = this.Combatant;

				if ( combatant != null && combatant.Player && combatant.Map == this.Map && combatant.InRange( this, 5 ) )
				{
					m_CarpeJugularaTime = DateTime.Now + TimeSpan.FromSeconds( 10.0 );

					Item item = combatant.FindItemOnLayer( Layer.Neck );

					if ( item is LeatherGorget ||
						item is PhoenixGorget ||
						item is BattleGorget ||
						item is PlateGorget ||
						item is AssassinationGorget ||
						item is FieldGorget ||
						item is PlateGorget ||
						item is RangerGorget ||
						item is StuddedGorget )
						return;
					else
					{
						this.Say( "*Leaps*" );
						Point3D from = this.Location;
                        			Point3D to = combatant.Location;
                        			this.Location = to;
                        			this.ProcessDelta();
						combatant.Kill();
						this.Combatant = null;
					}
				}
			}
			base.OnThink();
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 4 ) < 1 )
			c.DropItem( new PoemPartsThree() );

			if ( Utility.Random( 100 ) < 1 )
			c.DropItem( new SeptBook() );

            		m_Active = false;
			BrotherMaynard.Attempts = 0;
			base.OnDeath( c );
		}

		public BigTeethRabbit(Serial serial) : base(serial)
		{
		}

		public override int GetAttackSound()
		{
			return 0xC9;
		}

		public override int GetHurtSound()
		{
			return 0xCA;
		}

		public override int GetDeathSound()
		{
			return 0xCB;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}