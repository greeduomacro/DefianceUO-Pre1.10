using System;
using Server;
using Server.Mobiles;
using Server.Items;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "a royal corpse" )]
	public class HojanKing : BaseCreature
	{
		[Constructable]
		public HojanKing() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.05, 0.1 )
		{
			Name = "King Of Hojans";
			Body = 253;
			Hue = 1717;

			SetStr( 200 );
			SetDex( 300 );
			SetInt( 200 );

			SetHits( 10000 );
			SetMana( 0 );

			SetDamage( 20 );

			SetSkill( SkillName.MagicResist, 280.0 );
			SetSkill( SkillName.Tactics, 120.0 );
			SetSkill( SkillName.Wrestling, 120.0 );

			Fame = 9000;
			Karma = -9000;

			VirtualArmor = 500;
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 125 ) <  1 )
				c.DropItem( new HojanRoyalFlag() );

            base.OnDeath( c );
		}


		public override int Meat{ get{ return 1; } }
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
				defender.Freeze( TimeSpan.FromSeconds( 3.0 ) );
				defender.SendMessage( "You feel completely numb!" );
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

		private DateTime m_CheckBombTime;

		public override void OnThink()
		{
			if ( DateTime.Now >= m_CheckBombTime )
			{
				m_CheckBombTime = DateTime.Now + TimeSpan.FromSeconds( 1.2 );

				foreach ( Item item in this.GetItemsInRange( 2 ) )
				{
					if ( item is KrofinBOMB )
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

						this.Say( "*Dodges the bomb*" );
                        Point3D to = loc;
						this.Location = to;
                        this.ProcessDelta();
						this.Freeze(TimeSpan.FromSeconds(2.0));
					}
				}
			}

			base.OnThink();
		}


		public HojanKing(Serial serial) : base(serial)
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