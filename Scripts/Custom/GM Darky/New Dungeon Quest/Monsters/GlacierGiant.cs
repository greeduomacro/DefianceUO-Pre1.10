using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a glacier giant corpse" )]
	public class GlacierGiant : BaseCreature
	{
		[Constructable]
		public GlacierGiant() : base( AIType.AI_Melee, FightMode.Weakest, 10, 1, 0.4, 0.7 )
		{
			Name = "a glacier giant";
			Body = 189;
			Hue = 1154;
			BaseSoundID = 604;
			ShortTermMurders = 10;
			Kills = 10;

			SetStr( 836, 985 );
			SetDex( 56, 85 );

			SetHits( 772, 851 );

			SetDamage( 27, 31 );

			SetSkill( SkillName.MagicResist, 100.3, 135.0 );
			SetSkill( SkillName.Tactics, 130.1, 150.0 );
			SetSkill( SkillName.Anatomy, 30.1, 50.0 );
			SetSkill( SkillName.Wrestling, 130.1, 150.0 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 115;
			
			PackGold( 900, 2000 );
			PackSlayer();

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

			switch ( Utility.Random( 25 ) )
			{
				case 0: PackWeapon( 3, 5 ); break;
				case 1: PackArmor( 3, 5 ); break;
			}

				if ( 0.04 > Utility.RandomDouble() )
					PackItem( new EnchantedSextant() );

				if ( 0.02 > Utility.RandomDouble() )
					PackItem( new FullJars4() );
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 250 ) <  1 )
				c.DropItem( new FrozenBook() );

            		base.OnDeath( c );
		}


		private DateTime m_NextAttack;

		public override void OnActionCombat()
		{
			Mobile combatant = Combatant;

			if ( combatant == null || combatant.Deleted || combatant.Map != Map || !InRange( combatant, 12 ) || !CanBeHarmful( combatant ) || !InLOS( combatant ) )
				return;

			if ( DateTime.Now >= m_NextAttack )
			{
				SandAttack( combatant );
				m_NextAttack = DateTime.Now + TimeSpan.FromSeconds( 10.0 + (10.0 * Utility.RandomDouble()) );
			}
		}

		public void SandAttack( Mobile m )
		{
			DoHarmful( m );

			m.FixedParticles( 0x36B0, 10, 25, 9540, 1153, 0, EffectLayer.Waist );

			new InternalTimer( m, this ).Start();
		}

		private class InternalTimer : Timer
		{
			private Mobile m_Mobile, m_From;

			public InternalTimer( Mobile m, Mobile from ) : base( TimeSpan.FromSeconds( 1.0 ) )
			{
				m_Mobile = m;
				m_From = from;
				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				m_Mobile.PlaySound( 889 );
				AOS.Damage( m_Mobile, m_From, Utility.RandomMinMax( 15, 25 ), 100, 0, 0, 0, 0 );
			}
		}

		public override int Meat{ get{ return 24; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool BardImmune{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 5; } }

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
				damage *= 2;
		}

		public GlacierGiant( Serial serial ) : base( serial )
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