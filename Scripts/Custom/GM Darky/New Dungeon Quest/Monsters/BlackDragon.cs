using System;
using System.Collections;
using Server.Mobiles;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a black dragon corpse" )]
	public class BlackDragon : BaseCreature
	{
		private Timer BurnTimer;
		public int MaxHits;
		public bool Sacked;

		[Constructable]
		public BlackDragon() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.25, 0.5 )
		{
			Name = "a black dragon";
			Body = Utility.RandomList( 12, 59 );
			BaseSoundID = 362;
			Hue = 0x455;
			Sacked = true;
			SetStr( 871, 915 );
			SetDex( 86, 123 );
			SetInt( 499, 550 );

			SetDamage( 19, 25 );

			SetSkill( SkillName.EvalInt, 42.6, 55.0 );
			SetSkill( SkillName.Magery, 42.6, 55.0 );
			SetSkill( SkillName.Meditation, 13.1, 18.8 );
			SetSkill( SkillName.MagicResist, 99.5, 112.5 );
			SetSkill( SkillName.Tactics, 97.6, 100.0 );
			SetSkill( SkillName.Wrestling, 92.0, 94.4 );

			Fame = 16875;
			Karma = -16875;

			VirtualArmor = 63;

			PackGold( 1300, 1650 );
			PackArmor( 0, 5 );
			PackArmor( 0, 5 );

			switch ( Utility.Random( 3 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 4 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 6 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}


		}

		public override bool AutoDispel{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 4; } }
     	public override bool Unprovokable { get { return true; } }
		public override int Meat{ get{ return 19; } }
		public override int Hides{ get{ return 40; } }
		public override HideType HideType{ get{ return HideType.Barbed; } }
		public override int Scales{ get{ return 27; } }
		public override ScaleType ScaleType{ get{ return ( Body == 12 ? ScaleType.Black : ScaleType.Black ); } }

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
				damage *= 2;
		}

		public BlackDragon( Serial serial ) : base( serial )
		{
			this.MaxHits=this.HitsMax;
		}

		private DateTime m_NextFirebreath;

		public override void OnDamage( int amount, Mobile attacker, bool willKill )
		{
			if ( attacker != null && !willKill && amount > 5 && 50 > Utility.Random( 100 ) )
			{
				if ( this.Hits < (this.HitsMax/3) && this.Sacked )
				{
					attacker.SendMessage("The dragon's acid sack bursts open!");
					AcidPool acid = new AcidPool(attacker);
					acid.Map = attacker.Map;
					acid.Location = attacker.Location;
					attacker.PlaySound( 0x25 );
				}
				else if ( !this.Sacked )
				{
					attacker.SendMessage("Acid spills from the damaged sack!");
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

		public override void OnActionCombat()
		{

			Mobile combatant = Combatant;

			if ( !Sacked || combatant == null || combatant.Deleted || combatant.Map != Map || !InRange( combatant, 12 ) || !CanBeHarmful( combatant ) || !InLOS( combatant ) )
				return;
			if ( DateTime.Now > m_NextFirebreath )
			{
				m_NextFirebreath = DateTime.Now + TimeSpan.FromSeconds( Utility.Random( 10, 20 ) ); // 10-20 seconds
				this.GetDirectionTo( combatant.Location ); //check direction
				this.Freeze( TimeSpan.FromSeconds( 2 ) ); //freeze for animation
				this.Animate( 12, 5, 1, true, false, 0 ); //animation
				this.PlaySound( GetAngerSound() ); // Sound
				new InternalTimer( this, combatant, (int)this.Hits ).Start(); // start timer with saved current hits value
				BurnTimer = new m_Timer( combatant );
				BurnTimer.Start();
			}
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
				BlackDragon warrior = m_Mobile as BlackDragon;

				if ( warrior != null )
				{
					if ( a_Mobile != null && a_Mobile.Alive )
					{
						int damage = Utility.Random( 26,40 );
						warrior.DoHarmful( a_Mobile );
						AOS.Damage( a_Mobile, warrior, damage, 50, 0, 0, 50, 0 ); //Fire damage 100%
						warrior.MovingEffect( a_Mobile, 0x36D4, 7, 0, false, false, 0x73, 1 );
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

				foreach ( Item m in m_Mobile.GetItemsInRange( 10 ) )
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
	}
}