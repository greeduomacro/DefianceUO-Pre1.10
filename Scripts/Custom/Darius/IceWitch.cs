using System;
using System.Collections;
using Server;
using Server.Misc;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
	[CorpseName( "a glowing ice corpse" )]
	public class IceWitch : BaseCreature
	{
		[Constructable]
		public IceWitch () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "female" );
			Body = 401;
			Title = "the Ice Witch";
			BaseSoundID = 0x482;
			Hue = 1154;
			Kills = 5;
			SetStr( 900, 1000 );
			SetDex( 60, 80 );
			SetInt( 10000, 12000 );

			SetHits( 6000, 8000 );

			SetDamage( 90, 100 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Cold, 60 );
			SetDamageType( ResistanceType.Energy, 40 );

			SetResistance( ResistanceType.Physical, 40, 50 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 50, 60 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.EvalInt, 140.0, 150.0 );
			SetSkill( SkillName.Magery, 140, 150.0 );
			SetSkill( SkillName.MagicResist, 180.5, 200.0 );
			SetSkill( SkillName.Tactics, 110.0, 120.0 );
			SetSkill( SkillName.Wrestling, 100.0, 110.0 );

			Fame = 9000;
			Karma = -9000;

			VirtualArmor = 100;

			m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 2, 5 ) );

			PackItem( new Gold( 12000, 15000 ) );


			if ( Utility.Random( 1 ) < 1 ) PackItem( new IceCube() );

			AddItem( new GnarledStaff() );
			AddItem( new Server.Items.Skirt( Utility.RandomBlueHue() ) );
			AddItem( new Server.Items.Doublet( Utility.RandomBlueHue() ) );
			AddItem( new Server.Items.ThighBoots( Utility.RandomBlueHue() ) );
			AddItem( new TallStrawHat() );
			AddItem( new TwoPigTails( Utility.RandomBlueHue() ) );
		}

		public override bool AutoDispel{ get{ return true; } }
		public override bool BardImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override int Meat{ get{ return 1; } }

		public override int GetHurtSound()
		{
			return 0x14D;
		}

		public override int GetDeathSound()
		{
			return 0x150;
		}

		public override int GetAttackSound()
		{
			return 0x338;
		}

		private DateTime m_NextAbilityTime;

		public override void OnThink()
		{
			if ( DateTime.Now >= m_NextAbilityTime )
			{
				Mobile combatant = this.Combatant;

				if ( combatant != null && combatant.Map == this.Map && combatant.InRange( this, 12 ) && IsEnemy( combatant ) && !UnderEffect( combatant ) )
				{
					m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 20, 30 ) );

					// TODO: Forest summon ability

					this.Say( true, "I call apon an icy wind to sting you!" );

					m_Table[combatant] = Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), TimeSpan.FromSeconds( 7.0 ), new TimerStateCallback( DoEffect ), new object[]{ combatant, 0 } );
				}
			}

			base.OnThink();
		}

		private static Hashtable m_Table = new Hashtable();

		public static bool UnderEffect( Mobile m )
		{
			return m_Table.Contains( m );
		}

		public static void StopEffect( Mobile m, bool message )
		{
			Timer t = (Timer)m_Table[m];

			if ( t != null )
			{
				if ( message )
					m.PublicOverheadMessage( Network.MessageType.Emote, m.SpeechHue, true, "* The icy wind disperses! *" );

				t.Stop();
				m_Table.Remove( m );
			}
		}

		public void DoEffect( object state )
		{
			object[] states = (object[])state;

			Mobile m = (Mobile)states[0];
			int count = (int)states[1];

			if ( !m.Alive )
			{
				StopEffect( m, false );
			}
			else
			{
				if ( (count % 4) == 0 )
					{
						m.LocalOverheadMessage( Network.MessageType.Emote, m.SpeechHue, true, "* The icy wind whips your flesh! *" );
						m.NonlocalOverheadMessage( Network.MessageType.Emote, m.SpeechHue, true, String.Format( "* {0} is whiped by an icy wind! *", m.Name ) );
					}

					m.FixedParticles( 0x91C, 10, 180, 9539, 1, 1154, EffectLayer.Waist );
					m.PlaySound( 22 );
					m.PlaySound( 21 );

					AOS.Damage( m, this, Utility.RandomMinMax( 30, 40 ) - (Core.AOS ? 0 : 10), 100, 0, 0, 0, 0 );

					states[1] = count + 1;

					if ( !m.Alive )
						StopEffect( m, false );
			}

			if ( m is BaseCreature )
			{

				BaseCreature bc = (BaseCreature)m;

				if ( bc.Controlled && m.Hits == 0 )
				{
				StopEffect( m, true );
				Say("Haha One Slave is Defeated!");
				}
			}
			else
			{

				Torch torch = m.FindItemOnLayer( Layer.TwoHanded ) as Torch;

				if ( torch != null && torch.Burning )
				{
					StopEffect( m, true );
				}
			}
		}

		public override void CheckReflect( Mobile caster, ref bool reflect )
		{
			if ( caster.Body.IsMale )
				reflect = true; // Always reflect if caster isn't female
		}

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			if ( caster.Body.IsMale )
				scalar = 20; // Male bodies always reflect.. damage scaled 20x
		}

		public void DrainLife()
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 2 ) )
			{
				if ( m == this || !CanBeHarmful( m ) )
					continue;

				if ( m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != this.Team) )
					list.Add( m );
				else if ( m.Player )
					list.Add( m );
			}

			foreach ( Mobile m in list )
			{
				DoHarmful( m );

				m.FixedParticles( 0x374A, 10, 15, 5013, 0x496, 0, EffectLayer.Waist );
				m.PlaySound( 0x231 );

				m.SendMessage( "You feel weaker as the life drains out of you!" );

				int toDrain = Utility.RandomMinMax( 10, 40 );

				Hits += toDrain;
				m.Damage( toDrain, this );
			}
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			if ( 0.25 >= Utility.RandomDouble() )
				DrainLife();
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.25 >= Utility.RandomDouble() )
				DrainLife();
		}

		public IceWitch( Serial serial ) : base( serial )
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