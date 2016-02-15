using System;
using System.Collections;
using Server.Mobiles;
using Server.Targeting;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a turtle corpse" )]
	public class TurtleWarrior : BaseCreature
	{
		private Timer BurnTimer;
		public int MaxHits;
		public bool Sacked;

		[Constructable]
		public TurtleWarrior () : base( AIType.AI_Melee, FightMode.Closest, 25, 1, 0.4, 0.6 )
		{
			Name = "a turtle warrior";
			Body = 240;
			BaseSoundID = 1289;
			Sacked = true;
			Kills = 5;

			SetStr( 1300, 1450 );
			SetDex( 26, 35 );
			SetInt( 71, 95 );

			SetHits( 1950, 2050 );

			SetDamage( 29, 36 );

			SetSkill( SkillName.Anatomy, 74.5, 87.2 );
			SetSkill( SkillName.MagicResist, 95.2, 97.1 );
			SetSkill( SkillName.Tactics, 105.1, 110.0 );
			SetSkill( SkillName.Wrestling, 96.2, 100.9 );

			Fame = 24000;
			Karma = -24000;

			VirtualArmor = 20;

			switch ( Utility.Random( 3 ) )
			{
				case 0: PackGem(); break;
				case 1: PackPotion(); break;
			}

			PackGold( 2250, 3650 );
			PackArmor( 0, 5 );
			PackWeapon( 0, 5 );

			switch ( Utility.Random( 2 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 10 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}

			switch ( Utility.Random( 10 ) )
			{
				case 0: PackWeapon( 3, 5 ); break;
				case 1: PackArmor( 3, 5 ); break;
			}

			switch ( Utility.Random( 10 ) )
			{
				case 0: PackWeapon( 3, 5 ); break;
				case 1: PackArmor( 3, 5 ); break;
			}
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Regular; } }
     		public override bool Unprovokable { get { return true; } }
		public override int Meat{ get{ return 25; } }
		public override int Scales{ get{ return 20; } }
		public override ScaleType ScaleType{ get{ return (ScaleType)Utility.Random( 4 ); } }

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
				damage *= 7;
		}

		public TurtleWarrior( Serial serial ) : base( serial )
		{
			this.MaxHits=this.HitsMax;
		}

		public override void OnDamage( int amount, Mobile attacker, bool willKill )
		{
			if ( attacker != null && !willKill && amount > 5 && 50 > Utility.Random( 100 ) )
			{
				if ( this.Hits < (this.HitsMax/2) && this.Sacked )
				{
					attacker.SendMessage("The turtle's shell burst open!");
					AcidPool acid = new AcidPool(attacker);
					acid.Map = attacker.Map;
					acid.Location = attacker.Location;
					attacker.PlaySound( 0x25 );
				}
				else if ( !this.Sacked )
				{
					attacker.SendMessage("Acid spills from the turtle shell!");
					AcidPool acid = new AcidPool(attacker);
					acid.Map = attacker.Map;
					acid.Location = attacker.Location;
					attacker.PlaySound( 0x25 );
				}
				BurnTimer = new m_Timer( attacker );
				BurnTimer.Start();
			}

			base.OnDamage( amount, attacker, willKill );
		}

		private class InternalTimer : Timer
		{
			private Mobile m_Mobile;
			private Mobile a_Mobile;
			private int m_hits;

			public InternalTimer( Mobile mobile, Mobile attacker, int hits ) : base( TimeSpan.FromSeconds( 1.5 ) )
			{
				Priority = TimerPriority.FiftyMS;
				m_Mobile = mobile;
				a_Mobile = attacker;
			}

			protected override void OnTick()
			{
				TurtleWarrior warrior = m_Mobile as TurtleWarrior;

				if ( warrior != null )
				{
					if ( a_Mobile != null && a_Mobile.Alive )
					{
						int damage = Utility.Random( 15,20 );
						warrior.DoHarmful( a_Mobile );
						AOS.Damage( a_Mobile, warrior, damage, 50, 0, 0, 50, 0 ); //Fire damage 100%
						warrior.MovingEffect( a_Mobile, 0x36E4, 7, 0, false, false, 0x73, 1 );
						AcidPool acid = new AcidPool(a_Mobile);
						acid.Map = a_Mobile.Map;
						acid.Location = a_Mobile.Location;
						a_Mobile.PlaySound( 0x25 );

						if ( a_Mobile.Alive && a_Mobile.Body.IsHuman && !a_Mobile.Mounted )
						{
							a_Mobile.Animate( 20, 7, 1, true, false, 0 ); // take hit
						}
						Stop();
					}
				}
			}
		}

		public override void OnMovement(Mobile m, Point3D oldLocation )
		{
			if ( BurnTimer != null )
				BurnTimer.Stop();

			BurnTimer = null;
		}

		public override void OnAfterDelete()
		{
			if ( BurnTimer != null )
				BurnTimer.Stop();

			BurnTimer = null;

			base.OnAfterDelete();
		}

		private class m_Timer: Timer
		{
			private Mobile m_Mobile;

			public m_Timer( Mobile from ) : base( TimeSpan.FromSeconds( 1.0),TimeSpan.FromSeconds( 1.0 ) )
			{
				Priority = TimerPriority.FiftyMS;
				m_Mobile = from;
			}

			protected override void OnTick()
			{
				ArrayList list = new ArrayList();

				foreach ( Item m in m_Mobile.GetItemsInRange( 15 ) )
				{
					if( m is AcidPool )
					{
						list.Add(m);
					}
				}
				if( list.Count > 0 && m_Mobile.Alive )
				{
					m_Mobile.Damage( Utility.Random( 5 ) );
					m_Mobile.PlaySound(0x1dE);
				}
				else
					Stop();
			}
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
			switch( Utility.Random(500) )
				{
			case 0: c.DropItem( item = new MagicalCompositeBow() ); break;
			case 1: c.DropItem( item = new MagicalCompositeBow() ); break;
			        }
			base.OnDeath( c );
		}
	}
}