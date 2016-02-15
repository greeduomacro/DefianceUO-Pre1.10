using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a cloned ooze corpse" )]
	public class ClonedOoze : BaseCreature
	{
		[Constructable]
		public ClonedOoze() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.08, 0.08 )
		{
			Name = "a cloned ooze";
			Body = 94;
			BaseSoundID = 456;
			Hue = 1160;
			Kills = 10;
			ShortTermMurders = 10;

			SetStr( 20, 25 );
			SetDex( 10, 20 );
			SetInt( 10, 20 );

			SetHits( 10, 15 );

			SetDamage( 3, 5 );

			SetSkill( SkillName.MagicResist, 50.1, 55.0 );
			SetSkill( SkillName.Wrestling, 90.4, 95.7);
			SetSkill( SkillName.Tactics, 95.7, 98.4);

			Fame = 1500;
			Karma = -1500;

			VirtualArmor = 20;

			PackGold( 50, 150 );

		}

		public override Poison PoisonImmune{ get{ return Poison.Regular; } }
		public override Poison HitPoison{ get{ return Poison.Regular; } }

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.40 >= Utility.RandomDouble() ) // 40% chance to clone itself.
				AddClonedOoze( attacker, 0.25 );
		}

		public void AddClonedOoze( Mobile target, double chanceToThrow )
		{
			if ( chanceToThrow >= Utility.RandomDouble() )
			{
				Direction = GetDirectionTo( target );
				MovingEffect( target, 0xF7E, 10, 1, true, false, 0x496, 0 );
				new DelayTimer( this, target ).Start();
			}
			else
			{
				new ClonedOoze().MoveToWorld( Location, Map );
			}
		}

		private class DelayTimer : Timer
		{
			private Mobile m_Mobile;
			private Mobile m_Target;

			public DelayTimer( Mobile m, Mobile target ) : base( TimeSpan.FromSeconds( 1.0 ) )
			{
				m_Mobile = m;
				m_Target = target;
			}

			protected override void OnTick()
			{
				if ( m_Mobile.CanBeHarmful( m_Target ) )
				{
					m_Mobile.DoHarmful( m_Target );
					AOS.Damage( m_Target, m_Mobile, Utility.RandomMinMax( 10, 20 ), 100, 0, 0, 0, 0 );
					new ClonedOoze().MoveToWorld( m_Target.Location, m_Target.Map );
				}
			}
		}

		public ClonedOoze( Serial serial ) : base( serial )
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

		public override void OnDeath( Container c )
		{
			Item item = null;
			switch( Utility.Random(750) )
				{
			case 0: c.DropItem( item = new ContainerBones() ); break;
			case 1: c.DropItem( item = new ContainerBones2() ); break;
			case 2: c.DropItem( item = new ContainerBones3() ); break;
			        }
			base.OnDeath( c );
		}
	}
}