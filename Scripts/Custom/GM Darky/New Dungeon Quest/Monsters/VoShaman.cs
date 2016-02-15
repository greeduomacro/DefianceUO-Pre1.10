using System;
using Server.Mobiles;
using Server.Items;
using Server.Engines.Quests.Collector;

namespace Server.Mobiles
{
	[CorpseName( "a vothss shaman corpse" )]
	public class VoShaman : BaseCreature
	{
		[Constructable]
		public VoShaman() : base( AIType.AI_Healer, FightMode.Weakest, 10, 1, 0.1, 0.15 )
		{
			Name = "Vothss Shaman";
			Body = 0x24;
			BaseSoundID = 0x1A1;
			Hue = Utility.RandomList( 1701, 1702, 1703, 1704, 1705, 1706, 1707, 1708, 1709 );
			Kills = 5;
			ShortTermMurders = 5;

			SetStr( 72, 104 );
			SetDex( 76, 95 );
			SetInt( 1076, 1095 );

			SetHits( 320, 350 );
			SetMana( 350, 400 );

			SetDamage( 8, 12 );

			SetSkill( SkillName.EvalInt, 155.1, 160.0 );
			SetSkill( SkillName.Magery, 135.1, 147.0 );
			SetSkill( SkillName.MagicResist, 86.2, 94.0 );
			SetSkill( SkillName.Tactics, 59.3, 64.0 );
			SetSkill( SkillName.Wrestling, 79.3, 84.0 );

			Fame = 15500;
			Karma = -15500;

			VirtualArmor = 48;

			PackGold( 750, 975 );
			PackReg( Utility.RandomMinMax( 5, 20 ) );
			PackItem( new Bandage( Utility.RandomMinMax( 5, 20 ) ) );
			PackSlayer();
			PackScroll( 1, 8 );


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
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
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

				if ( 0.02 > Utility.RandomDouble() )
					PackItem( new Whip() );

		}

		public override void OnDamagedBySpell( Mobile from )
		{
			this.Combatant = from;

			if (from.Combatant == null)
				return;

			Mobile m = from.Combatant;

			if (m.Combatant == null)
				return;

			if ( Alive )
			switch ( Utility.Random( 50 ) )
			{
				case 0: m.Hits += ( Utility.Random( 25, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
				case 1: m.Hits += ( Utility.Random( 25, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
				case 2: m.Hits += ( Utility.Random( 25, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
				case 3: m.Hits += ( Utility.Random( 25, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
				case 4: m.Hits += ( Utility.Random( 25, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
				case 5: m.Hits += ( Utility.Random( 25, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
				case 6: m.Hits += ( Utility.Random( 25, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
				case 7: m.Hits += ( Utility.Random( 25, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
				case 8: m.Hits += ( Utility.Random( 25, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
				case 9: m.Hits += ( Utility.Random( 25, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
				case 10: m.Hits += ( Utility.Random( 25, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
			}
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			this.Combatant = attacker;

			if (attacker.Combatant == null)
				return;

			Mobile m = attacker.Combatant;

			if (m.Combatant == null)
				return;

			if ( Alive )
			switch ( Utility.Random( 50 ) )
			{
				case 0: m.Hits += ( Utility.Random( 25, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
				case 1: m.Hits += ( Utility.Random( 25, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
				case 2: m.Hits += ( Utility.Random( 25, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
				case 3: m.Hits += ( Utility.Random( 25, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
				case 4: m.Hits += ( Utility.Random( 25, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
				case 5: m.Hits += ( Utility.Random( 25, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
				case 6: m.Hits += ( Utility.Random( 25, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
				case 7: m.Hits += ( Utility.Random( 25, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
				case 8: m.Hits += ( Utility.Random( 25, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
				case 9: m.Hits += ( Utility.Random( 25, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
				case 10: m.Hits += ( Utility.Random( 25, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
			}
		}

		private DateTime m_NextAttack;

		public override void OnActionCombat()
		{
			Mobile combatant = Combatant;

			if ( combatant == null || combatant.Deleted || combatant.Map != Map || !InRange( combatant, 10 ) || !CanBeHarmful( combatant ) || !InLOS( combatant ) )
				return;

			if ( DateTime.Now >= m_NextAttack )
			{
				SandAttack( combatant );
				m_NextAttack = DateTime.Now + TimeSpan.FromSeconds( 13.0 + (20.0 * Utility.RandomDouble()) );
			}
		}

		public void SandAttack( Mobile m )
		{
			DoHarmful( m );

			m.FixedParticles( 0x3709, 10, 30, 5052, 2126, 0, EffectLayer.LeftFoot );

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
				AOS.Damage( m_Mobile, m_From, Utility.RandomMinMax( 36, 40 ), 0, 0, 0, 0, 100 );
			}
		}

		public override int Meat{ get{ return 9; } }
		public override int Hides{ get{ return 20; } }
		public override HideType HideType{ get{ return HideType.Horned; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Lesser; } }

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
				damage *= 0;
		}

		public VoShaman(Serial serial) : base(serial)
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
			switch( Utility.Random(1000) )
				{
			case 0: c.DropItem( item = new MinotaurSmallAxe() ); break;
			case 1: c.DropItem( item = new MinotaurSmallAxe() ); break;
			        }
			base.OnDeath( c );
		}
	}
}