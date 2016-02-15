using System;
using Server;
using Server.Misc;
using Server.Items;
using System.Collections;
//using Server.Engines.SilenceAddon;

namespace Server.Mobiles
{
	public class Krog : BaseBellBoss
	{
		static bool m_Active;

		[CommandProperty( AccessLevel.GameMaster )]
		public static bool Active
		{
			get{ return m_Active; }
			set{ m_Active = value; }
		}

		[Constructable]
		public Krog() : base( AIType.AI_Mage )
		{
			Name = "Krog the fallen king";
			Body = 772;
			Kills = 5;
			Hue = 22222;
			m_Active = true;

			SetStr( 800 );
			SetDex( 280 );
			SetInt( 3000 );

			SetHits( 10000 );
			SetMana( 20000 );

			SetDamage( 40, 60 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Energy, 50 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 20, 40 );
			SetResistance( ResistanceType.Cold, 10, 30 );
			SetResistance( ResistanceType.Poison, 5, 10 );
			SetResistance( ResistanceType.Energy, 50, 70 );

			SetSkill( SkillName.MagicResist, 250 );
			SetSkill( SkillName.EvalInt, 250.0 );
			SetSkill( SkillName.Magery, 250.0 );
			SetSkill( SkillName.Tactics, 120 );
			SetSkill( SkillName.Wrestling, 120 );

			Fame = 8000;
			Karma = 8000;

			VirtualArmor = 140;
		}

		public override void OnDeath( Container c )
		{
			m_Active = false;

			if ( Utility.Random( 2 ) < 1 )
			c.DropItem( new MysticKeySinglePart(3) );

			if ( Utility.Random( 10 ) < 1 )
			c.DropItem( new ThievesCloak() );

			base.OnDeath( c );
		}

		private DateTime m_NextAbilityTime;

		private void DoAreaLeech() // This leeching ability was taken from evilvampire.cs and the text and values changed
		{
			m_NextAbilityTime += TimeSpan.FromSeconds( 2.5 );

			this.Say( true, "So much life in this room, I must have it!" );
			this.FixedParticles( 0x376A, 10, 10, 9537, 33, 0, EffectLayer.Waist );

			Timer.DelayCall( TimeSpan.FromSeconds( 5.0 ), new TimerCallback( DoAreaLeech_Finish ) );
		}

		private void DoAreaLeech_Finish()
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 6 ) )
			{
				if ( this.CanBeHarmful( m ) && this.IsEnemy( m ) )
					list.Add( m );
			}

			if ( list.Count == 0 )
			{
				this.Say( true, "So you flee from my power do you?" );
			}
			else
			{
				double scalar;

				if ( list.Count == 1 )
					scalar = 0.50;
				else if ( list.Count == 2 )
					scalar = 0.50;
				else
					scalar = 0.20;

				for ( int i = 0; i < list.Count; ++i )
				{
					Mobile m = (Mobile)list[i];

					int damage = (int)(m.Hits * scalar);

					damage += Utility.RandomMinMax( -5, 5 );

					if ( damage < 1 )
						damage = 1;

					m.MovingParticles( this, 0x36F4, 1, 0, false, false, 32, 0, 9535,    1, 0, (EffectLayer)255, 0x100 );
					m.MovingParticles( this, 0x0001, 1, 0, false,  true, 32, 0, 9535, 9536, 0, (EffectLayer)255, 0 );

					this.DoHarmful( m );
					this.Hits += AOS.Damage( m, this, damage, 100, 0, 0, 0, 0 );
				}

				this.Say( true, "Yes, I devour your essence, muhahahaha!" );
			}
		}

		private void DoFocusedLeech( Mobile combatant, string message )
		{
			this.Say( true, message );

			Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), new TimerStateCallback( DoFocusedLeech_Stage1 ), combatant );
		}

		private void DoFocusedLeech_Stage1( object state )
		{
			Mobile combatant = (Mobile)state;

			if ( this.CanBeHarmful( combatant ) )
			{
				this.MovingParticles( combatant, 0x36FA, 1, 0, false, false, 1108, 0, 9533, 1,    0, (EffectLayer)255, 0x100 );
				this.MovingParticles( combatant, 0x0001, 1, 0, false,  true, 1108, 0, 9533, 9534, 0, (EffectLayer)255, 0 );
				this.PlaySound( 0x1FB );

				Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( DoFocusedLeech_Stage2 ), combatant );
			}
		}

		private void DoFocusedLeech_Stage2( object state )
		{
			Mobile combatant = (Mobile)state;

			if ( this.CanBeHarmful( combatant ) )
			{
				combatant.MovingParticles( this, 0x36F4, 1, 0, false, false, 32, 0, 9535, 1,    0, (EffectLayer)255, 0x100 );
				combatant.MovingParticles( this, 0x0001, 1, 0, false,  true, 32, 0, 9535, 9536, 0, (EffectLayer)255, 0 );

				this.PlaySound( 0x209 );
				this.DoHarmful( combatant );
				this.Hits += AOS.Damage( combatant, this, Utility.RandomMinMax( 20, 30 ) - (Core.AOS ? 0 : 10), 100, 0, 0, 0, 0 );
			}
		}

		public override void OnThink()
		{
			if ( DateTime.Now >= m_NextAbilityTime )
			{
				Mobile combatant = this.Combatant;

				if ( combatant != null && combatant.Map == this.Map && combatant.InRange( this, 15 ) )
				{
					m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 8, 12 ) );

					int ability = Utility.Random( 5 );

					switch ( ability )
					{
						case 0: DoFocusedLeech( combatant, "Feel weaker mortal?" ); break;
						case 1: DoFocusedLeech( combatant, "Watch as you grow weaker as i grow stronger!" ); break;
						case 2: DoFocusedLeech( combatant, "Begone mortal!" ); break;
						case 3: DoFocusedLeech( combatant, "Your essence is mine for the taking!" ); break;
						case 4: DoAreaLeech(); break;
					}
				}
			}

			base.OnThink();}

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar ) // taken from Questboss.cs and the m_ability activation thing removed
		{
			if ( caster is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)caster;

				if ( bc.Controlled || bc.Summoned || bc.BardTarget == this )
				scalar = 0.33;
			}
		}

		public override void AlterMeleeDamageTo( Mobile to, ref int damage ) // Taken from DragChamp.cs and values changed
		{
			if ( to is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)to;

				if ( bc.Controlled || bc.Summoned || bc.BardTarget == this )
				damage *= 2;
			}
		}

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage ) // Taken from DragChamp.cs and values changed
		{
			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Controlled || bc.Summoned || bc.BardTarget == this )
				damage /= 2;
			}
		}

		public override void OnGotMeleeAttack( Mobile attacker ) // taken from Questboss.cs and the m_ability activation thing removed
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.1 >= Utility.RandomDouble() )
				Earthquake();

			if ( 0.33 >= Utility.RandomDouble() && attacker is BaseCreature )
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

		public override void OnDamagedBySpell( Mobile caster ) // taken from Questboss.cs and the m_ability activation thing removed
		{
			base.OnDamagedBySpell( caster );

			if ( 0.1 >= Utility.RandomDouble() )
				Earthquake();
		}

		public void Earthquake() // taken from Questboss.cs
		{
			Map map = this.Map;

			if ( map == null )
				return;

			ArrayList targets = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 8 ) )
			{
				if ( m == this || !CanBeHarmful( m ) )
					continue;

				if ( m is BaseCreature && (((BaseCreature)m).Controlled ||
				((BaseCreature)m).Summoned || ((BaseCreature)m).Team != this.Team) )
					targets.Add( m );
				else if ( m.Player )
					targets.Add( m );
			}

			PlaySound( 0x2F3 );

			for ( int i = 0; i < targets.Count; ++i )
			{
				Mobile m = (Mobile)targets[i];

				double damage = m.Hits * 0.6;

				if ( damage < 10.0 )
					damage = 10.0;
				else if ( damage > 75.0 )
					damage = 75.0;

				DoHarmful( m );

				AOS.Damage( m, this, (int)damage, 100, 0, 0, 0, 0 );

				if ( m.Alive && m.Body.IsHuman && !m.Mounted )
					m.Animate( 20, 7, 1, true, false, 0 ); // take hit
			}
		}

		public Krog( Serial serial ) : base( serial )
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
			//Explanation see GhostPast
			m_Active = true;
		}
	}
}