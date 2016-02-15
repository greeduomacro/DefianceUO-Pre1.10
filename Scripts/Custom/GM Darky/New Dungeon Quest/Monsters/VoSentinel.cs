using System;
using Server.Mobiles;
using Server.Items;
using Server.Engines.Quests.Collector;

namespace Server.Mobiles
{
	[CorpseName( "a vothss sentinel corpse" )]
	public class VoSentinel : BaseCreature
	{
		[Constructable]
		public VoSentinel() : base( AIType.AI_Melee, FightMode.Strongest, 10, 1, 0.4, 0.8 )
		{
			Name = "Vothss Sentinel";
			Body = 0x24;
			BaseSoundID = 0x1A1;
			Hue = Utility.RandomList( 1410, 1411, 1414, 1416, 1418, 1420, 1422, 1424, 1428, 1432, 1434, 1435, 1436, 1437, 1439, 1440 );
			Kills = 5;
			ShortTermMurders = 5;

			SetStr( 1572, 1604 );
			SetDex( 276, 295 );

			SetHits( 2600, 2700 );

			SetDamage( 38, 42 );

			SetSkill( SkillName.MagicResist, 135.1, 140.0 );
			SetSkill( SkillName.Tactics, 139.3, 144.0 );
			SetSkill( SkillName.Wrestling, 139.3, 144.0 );
			SetSkill( SkillName.Anatomy, 99.3, 108.0 );

			Fame = 30500;
			Karma = -30500;

			VirtualArmor = 150;

			int gems = Utility.RandomMinMax( 5, 15 );

			for ( int i = 0; i < gems; ++i )
				PackGem();

			PackGold( 3050, 4375 );
			PackItem( new Bandage( Utility.RandomMinMax( 25, 50 ) ) );

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackItem( new GreaterCurePotion() ); break;
				case 1: PackItem( new GreaterPoisonPotion() ); break;
				case 2: PackItem( new GreaterHealPotion() ); break;
				case 3: PackItem( new GreaterStrengthPotion() ); break;
				case 4: PackItem( new GreaterAgilityPotion() ); break;
			}

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackItem( new GreaterCurePotion() ); break;
				case 1: PackItem( new GreaterPoisonPotion() ); break;
				case 2: PackItem( new GreaterHealPotion() ); break;
				case 3: PackItem( new GreaterStrengthPotion() ); break;
				case 4: PackItem( new GreaterAgilityPotion() ); break;
			}

			switch ( Utility.Random( 2 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}

			switch ( Utility.Random( 2 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}

			switch ( Utility.Random( 4 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}

			switch ( Utility.Random( 4 ) )
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

				if ( 0.04 > Utility.RandomDouble() )
					PackItem( new Whip() );

		}

		private DateTime m_NextAttack;

		public override void OnActionCombat()
		{
			Mobile combatant = Combatant;

			if ( combatant == null || combatant.Deleted || combatant.Map != Map || !InRange( combatant, 5 ) || !CanBeHarmful( combatant ) || !InLOS( combatant ) )
				return;

			if ( DateTime.Now >= m_NextAttack )
			{
				SandAttack( combatant );
				m_NextAttack = DateTime.Now + TimeSpan.FromSeconds( 25.0 + (25.0 * Utility.RandomDouble()) );
			}
		}

		public void SandAttack( Mobile m )
		{
			DoHarmful( m );

			m.FixedParticles( 0x3709, 10, 30, 5052, 1271, 0, EffectLayer.LeftFoot );

			new InternalTimer( m, this ).Start();
		}

		private class InternalTimer : Timer
		{
			private Mobile m_Mobile, m_From;

			public InternalTimer( Mobile m, Mobile from ) : base( TimeSpan.FromSeconds( 0 ) )
			{
				m_Mobile = m;
				m_From = from;
				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				m_Mobile.PlaySound( 0x22F );
				AOS.Damage( m_Mobile, m_From, Utility.RandomMinMax( 56, 70 ), 0, 0, 0, 0, 100 );
			}
		}

		public override int Meat{ get{ return 9; } }
		public override int Hides{ get{ return 30; } }
		public override HideType HideType{ get{ return HideType.Spined; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Lesser; } }

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
			{
			if ( from is EnergyVortex || from is BladeSpirits )
				damage = 0; // Immune to Energy Vortex and Blade Spirits
			}
		}

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
				damage *= 2;
		}

		public VoSentinel(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
		public override void OnDeath( Container c )
		{
			Item item = null;
			switch( Utility.Random(500) )
				{
			case 0: c.DropItem( item = new MinotaurSmallAxe() ); break;
			case 1: c.DropItem( item = new MinotaurSmallAxe() ); break;
			        }
			base.OnDeath( c );
		}
	}
}