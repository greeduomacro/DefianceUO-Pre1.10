using System;
using Server;
using Server.Spells.Seventh;
using Server.Spells.Fifth;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a murderous orc corpse" )]
	public class GurukkOrc : BaseCreature
	{
		public override InhumanSpeech SpeechType{ get{ return InhumanSpeech.Orc; } }

		[Constructable]
		public GurukkOrc () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.3 )
		{
			Name = "Gurukk";
			Title = "the Murderous Orc";
			Body = 140;
			BaseSoundID = 0x45A;

			SetStr( 950, 1100 );
			SetDex( 175, 255 );
			SetInt( 500, 750 );

			SetHits( 700, 1100 );

			SetDamage( 25, 35 );

			SetSkill( SkillName.EvalInt, 100.0, 120.0 );
			SetSkill( SkillName.Magery, 100.0, 120.0 );
			SetSkill( SkillName.MagicResist, 75.5, 90.0 );
			SetSkill( SkillName.Tactics, 50.1, 65.0 );
			SetSkill( SkillName.Wrestling, 50.1, 75.0 );
			SetSkill( SkillName.Meditation, 80.0, 100.0 );

			Fame = 2400;
			Karma = -2400;

			VirtualArmor = 200;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 1 );
			AddLoot( LootPack.MedScrolls, 2 );
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool AutoDispel{ get{ return true; } }
		public override bool CanDestroyObstacles { get { return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Regular; } }
		public override int Meat{ get{ return 1; } }
		

		public override void OnDeath( Container c )
   		{
   			if (Utility.Random( 150 ) <  1 )
   			c.DropItem( new OrcishBannerEastDeed() );
			
			if (Utility.Random( 150 ) <  1 )
   			c.DropItem( new OrcishBannerSouthDeed() );

             		base.OnDeath( c );
  	 	}

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
  		{
  			 if ( from is BaseCreature )
  		 	{
    			BaseCreature bc = (BaseCreature)from;

    			if ( bc.Controlled )
    			damage /= 10;
  		 	}

   			if ( from is BaseCreature )
   			{
    			BaseCreature bc = (BaseCreature)from;

    			if ( bc.Summoned )
    			damage *= 3;
   			}
  		}

		public void Polymorph( Mobile m )
		{
			if ( !m.CanBeginAction( typeof( PolymorphSpell ) ) || !m.CanBeginAction( typeof( IncognitoSpell ) ) || m.IsBodyMod )
				return;

			IMount mount = m.Mount;

			if ( mount != null )
				mount.Rider = null;

			if ( m.Mounted )
				return;

			if ( m.BeginAction( typeof( PolymorphSpell ) ) )
			{
				Item disarm = m.FindItemOnLayer( Layer.OneHanded );

				if ( disarm != null && disarm.Movable )
					m.AddToBackpack( disarm );

				disarm = m.FindItemOnLayer( Layer.TwoHanded );

				if ( disarm != null && disarm.Movable )
					m.AddToBackpack( disarm );

				m.BodyMod = 17;
				m.HueMod = 0;

				new ExpirePolymorphTimer( m ).Start();
			}
		}

		private class ExpirePolymorphTimer : Timer
		{
			private Mobile m_Owner;

			public ExpirePolymorphTimer( Mobile owner ) : base( TimeSpan.FromMinutes( 3.0 ) )
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

		public void SpawnOrc( Mobile target )
		{
			Map map = this.Map;

			if ( map == null )
				return;

			int orcs = 0;

			foreach ( Mobile m in this.GetMobilesInRange( 10 ) )
			{
				if ( m is Orc || m is OrcBomber || m is OrcishMage )
					++orcs;
			}

			if ( orcs < 16 )
			{
				PlaySound( 0x3D );

				int neworcs = Utility.RandomMinMax( 3, 6 );

				for ( int i = 0; i < neworcs; ++i )
				{
					BaseCreature orc;

					switch ( Utility.Random( 5 ) )
					{
						default:
						case 0: case 1:	orc = new Orc(); break;
						case 2: case 3:	orc = new OrcBomber(); break;
						case 4:			orc = new OrcishMage(); break;
					}

					orc.Team = this.Team;

					bool validLocation = false;
					Point3D loc = this.Location;

					for ( int j = 0; !validLocation && j < 10; ++j )
					{
						int x = X + Utility.Random( 3 ) - 1;
						int y = Y + Utility.Random( 3 ) - 1;
						int z = map.GetAverageZ( x, y );

						if ( validLocation = map.CanFit( x, y, this.Z, 16, false, false ) )
							loc = new Point3D( x, y, Z );
						else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
							loc = new Point3D( x, y, z );
					}

					orc.MoveToWorld( loc, map );
					orc.Combatant = target;
				}
			}
		}

		public void DoSpecialAbility( Mobile target )
		{
			if ( target == null || target.Deleted ) //sanity
				return;
			if ( 0.5 >= Utility.RandomDouble() ) // 50% chance to polymorph attacker into a orc
				Polymorph( target );

			if ( 0.2 >= Utility.RandomDouble() ) // 20% chance to more orcs
				SpawnOrc( target );
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			DoSpecialAbility( attacker );
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			DoSpecialAbility( defender );
		}

		public override void AggressiveAction( Mobile aggressor, bool criminal )
		{
			base.AggressiveAction( aggressor, criminal );

			Item item = aggressor.FindItemOnLayer( Layer.Helm );

			if ( item is OrcishKinMask )
			{
				AOS.Damage( aggressor, 50, 0, 100, 0, 0, 0 );
				item.Delete();
				aggressor.FixedParticles( 0x36BD, 20, 10, 5044, EffectLayer.Head );
				aggressor.PlaySound( 0x307 );
			}
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
				Priority = TimerPriority.TwoFiftyMS;

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

		public GurukkOrc( Serial serial ) : base( serial )
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