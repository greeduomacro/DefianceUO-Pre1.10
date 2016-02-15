using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a female necromancess's corpse" )]
	public class FemaleNecromancer : BaseCreature
	{
		[Constructable]
		public FemaleNecromancer() : base( AIType.AI_Mage, FightMode.Weakest, 10, 1, 0.1, 0.2 )
		{
			Name = NameList.RandomName( "female" );
			Title = "the necromancess";
			Body = 401;
			Hue = 0x83EC;

			SetStr( 305, 425 );
			SetDex( 82, 130 );
			SetInt( 505, 750 );

			SetHits( 2000, 3500 );
			SetStam( 200, 300 );

			SetDamage( 20, 30 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 25, 30 );
			SetResistance( ResistanceType.Fire, 35, 45 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 20, 30 );

			SetSkill( SkillName.EvalInt, 105.0 );
			SetSkill( SkillName.Magery, 100.0 );
			SetSkill( SkillName.Meditation, 500.0 );
			SetSkill( SkillName.MagicResist, 150.0 );
			SetSkill( SkillName.Tactics, 100.0, 100.0 );
			SetSkill( SkillName.Wrestling, 100.0, 100.0 );

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 55;
			Female = true;

			Item shroud = new HoodedShroudOfShadows();

			shroud.Movable = false;
			shroud.Hue = 1157;
			AddItem( shroud );
			AddItem( new NecromaticMask() );
			HeavyCrossbow weapon = new HeavyCrossbow();

			weapon.Skill = SkillName.Wrestling;
			weapon.Hue = 38;
			weapon.Movable = false;

			AddItem( weapon );

			new NecroMount().Rider = this;
		}

		public override bool CanDestroyObstacles{ get{ return true; } }

		public override bool CanMoveOverObstacles{ get{ return true; } }

		public override bool OnBeforeDeath()
		{
			IMount mount = this.Mount;

			if ( mount != null )
				mount.Rider = null;

			if ( mount is Mobile )
				((Mobile)mount).Delete();

			return base.OnBeforeDeath();
		}


		public override int GetIdleSound()
		{
			return 0x106;
		}

		public override int GetAngerSound()
		{
			return 0x104;
		}

		public override int GetDeathSound()
		{
			return 0x103;
		}

		public override int GetAttackSound()
		{
			return 0x102;
		}

		public override int GetHurtSound()
		{
			return 0x101;
		}

		public override void GenerateLoot( bool spawning )
		{
			AddLoot( LootPack.MedScrolls, 2 );
			AddLoot( LootPack.FilthyRich, 3 );
			if ( !spawning )
				if ( Utility.Random( 100 ) < 10 ) PackItem( new CarpetAddonDeed() );
		}

		private class TeleportTimer : Timer
		{
			private Mobile m_Owner;

			private static int[] m_Offsets = new int[]
			{
				-1, -1,
				-1,  0,
				-1,  1,
				0, -1,
				0,  1,
				1, -1,
				1,  0,
				1,  1
			};

			public TeleportTimer( Mobile owner ) : base( TimeSpan.FromSeconds( 5.0 ), TimeSpan.FromSeconds( 5.0 ) )
			{
				m_Owner = owner;
			}

			protected override void OnTick()
			{
				if ( m_Owner.Deleted )
				{
					Stop();
					return;
				}

				Map map = m_Owner.Map;

				if ( map == null )
					return;

				if ( 0.25 < Utility.RandomDouble() )
					return;

				Mobile toTeleport = null;

				foreach ( Mobile m in m_Owner.GetMobilesInRange( 16 ) )
				{
					if ( m != m_Owner && m.Player && m_Owner.CanBeHarmful( m ) && m_Owner.CanSee( m ) )
					{
						toTeleport = m;
						break;
					}
				}

				if ( toTeleport != null )
				{
					int offset = Utility.Random( 8 ) * 2;

					Point3D to = m_Owner.Location;

					for ( int i = 0; i < m_Offsets.Length; i += 2 )
					{
						int x = m_Owner.X + m_Offsets[(offset + i) % m_Offsets.Length];
						int y = m_Owner.Y + m_Offsets[(offset + i + 1) % m_Offsets.Length];

						if ( map.CanSpawnMobile( x, y, m_Owner.Z ) )
						{
							to = new Point3D( x, y, m_Owner.Z );
							break;
						}
						else
						{
							int z = map.GetAverageZ( x, y );

							if ( map.CanSpawnMobile( x, y, z ) )
							{
								to = new Point3D( x, y, z );
								break;
							}
						}
					}

					Mobile m = toTeleport;

					Point3D from = m.Location;

					m.Location = to;

					Server.Spells.SpellHelper.Turn( m_Owner, toTeleport );
					Server.Spells.SpellHelper.Turn( toTeleport, m_Owner );

					m.ProcessDelta();

					Effects.SendLocationParticles( EffectItem.Create( from, m.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
					Effects.SendLocationParticles( EffectItem.Create(   to, m.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 5023 );

					m.PlaySound( 0x1FE );

					m_Owner.Combatant = toTeleport;
				}
			}
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool BardImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }

		public override bool ShowFameTitle{ get{ return false; } }
		public override bool ClickTitle{ get{ return false; } }


		public FemaleNecromancer( Serial serial ) : base( serial )
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