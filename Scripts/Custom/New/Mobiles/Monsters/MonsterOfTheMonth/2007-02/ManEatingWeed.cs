using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a corpser corpse" )]
	public class ManEatingWeed : BaseCreature
	{
		[Constructable]
		public ManEatingWeed() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a man eating weed";
			Body = 8;
			Hue = Utility.RandomRedHue();
			BaseSoundID = 684;

			SetStr( 500 );
			SetDex( 200 );
			SetInt( 100 );

			SetHits( 7500 );
			SetMana( 0 );

			SetDamage( 20, 30 );

			SetDamageType( ResistanceType.Physical, 60 );
			SetDamageType( ResistanceType.Poison, 40 );

			SetResistance( ResistanceType.Physical, 15, 20 );
			SetResistance( ResistanceType.Fire, 15, 25 );
			SetResistance( ResistanceType.Cold, 10, 20 );
			SetResistance( ResistanceType.Poison, 20, 30 );

			SetSkill( SkillName.MagicResist, 100.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 120.0 );

			Fame = 10000;
			Karma = -10000;

			VirtualArmor = 40;

			m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( 30 );
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 5 ) < 1 )
			c.DropItem( new PoemParts() );

			base.OnDeath( c );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool BardImmune{ get{ return true; } }
		public override bool DisallowAllMoves{ get{ return true; } }

		private DateTime m_NextAbilityTime;

		public override void OnThink()
		{
			if ( DateTime.Now >= m_NextAbilityTime )
			{
				Mobile toTeleport = null;

				foreach ( Mobile m in this.GetMobilesInRange( 6 ) )
				{
					if ( m.Map == this.Map && m.Player && CanBeHarmful( m ) )
					{
						toTeleport = m;
						break;
					}
				}

				if ( toTeleport != null )
				{
					m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 10, 12 ) );

					Map map = this.Map;

					if ( map != null )
					{

                   				for (int i = 0; i < 10; ++i)
				                {
                        				int x = X;
                       					int y = Y;
                        				int z = Z;

                        				if (!map.CanFit(x, y, z, 16, false, false))
                            				continue;
										Point3D from = toTeleport.Location;
                        				Point3D to = new Point3D(x, y, z);

                        				if (!InLOS(to))
                           				continue;

										toTeleport.Location = to;
										toTeleport.ProcessDelta();
										toTeleport.Kill();

                        				Effects.SendLocationParticles(EffectItem.Create(from, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);

                        				Effects.PlaySound(from, map, 0x1FE);
                    				}
					}
				}
			}
			base.OnThink();
		}

		public override void Damage( int amount, Mobile from )
		{
			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc is BladeSpirits || bc is EnergyVortex )
				{
					amount = (int)(amount * 5);
				}
				else
				{
					amount = (int)(0);
				}
			}
			else
			{
				amount = (int)(0);
			}
			base.Damage( amount, from );
		}

		public ManEatingWeed( Serial serial ) : base( serial )
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