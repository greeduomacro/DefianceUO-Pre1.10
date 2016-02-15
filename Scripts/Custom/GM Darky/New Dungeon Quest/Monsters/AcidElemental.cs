using System;
using System.Collections;
using Server.Mobiles;
using Server.Targeting;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an acid elemental corpse" )]
	public class AcidElemental : BaseCreature
	{
		private Timer BurnTimer;
		public int MaxHits;
		public bool Sacked;

		[Constructable]
		public AcidElemental () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an acid elemental";
			Body = 16;
			Hue = 159;
			BaseSoundID = 278;
			Sacked = true;
			Kills = 5;

			SetStr( 327, 355 );
			SetDex( 66, 85 );
			SetInt( 271, 295 );

			SetDamage( 13, 16 );

			SetSkill( SkillName.Magery, 74.5, 87.2 );
			SetSkill( SkillName.MagicResist, 125.2, 127.1 );
			SetSkill( SkillName.Tactics, 80.1, 90.0 );
			SetSkill( SkillName.Wrestling, 73.2, 90.9 );

			Fame = 10500;
			Karma = -10500;

			VirtualArmor = 50;
			CanSwim = true;

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackGem(); break;
				case 1: PackPotion(); break;
				case 2: PackScroll( 1, 8 ); break;
			}

			PackGold( 450, 650 );
			PackArmor( 0, 5 );
			PackWeapon( 0, 5 );
		}

		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		public override Poison HitPoison{ get{ return Poison.Greater; } }

		public AcidElemental( Serial serial ) : base( serial )
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
					attacker.SendMessage("The elemental's body bursts open!");
					AcidPool acid = new AcidPool(attacker);
					acid.Map = attacker.Map;
					acid.Location = attacker.Location;
					attacker.PlaySound( 0x25 );
				}
				else if ( !this.Sacked )
				{
					attacker.SendMessage("Acid spills from the damaged elemental!");
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
				m_NextFirebreath = DateTime.Now + TimeSpan.FromSeconds( Utility.Random( 5, 15 ) ); // 5-15 seconds
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
				AcidElemental warrior = m_Mobile as AcidElemental;

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