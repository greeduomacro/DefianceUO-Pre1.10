using System;
using Server.Mobiles;
using Server.Items;
using Server.Engines.Quests.Collector;

namespace Server.Mobiles
{
	[CorpseName( "a vothss mystic corpse" )]
	public class VoMystic : BaseCreature
	{
		[Constructable]
		public VoMystic() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Vothss Mystic";
			Body = 0x24;
			BaseSoundID = 0x1A1;
			Hue = Utility.RandomList( 2201, 2202, 2203, 2204, 2205, 2206 );
			Kills = 5;
			ShortTermMurders = 5;

			SetStr( 72, 104 );
			SetDex( 76, 95 );
			SetInt( 1076, 1095 );

			SetHits( 150, 170 );
			SetMana( 1600, 1700 );

			SetDamage( 8, 12 );

			SetSkill( SkillName.EvalInt, 155.1, 160.0 );
			SetSkill( SkillName.Magery, 135.1, 147.0 );
			SetSkill( SkillName.MagicResist, 56.2, 60.0 );
			SetSkill( SkillName.Tactics, 29.3, 34.0 );
			SetSkill( SkillName.Wrestling, 19.3, 24.0 );

			Fame = 12500;
			Karma = -12500;

			VirtualArmor = 8;

			PackGold( 550, 675 );
			PackReg( Utility.RandomMinMax( 5, 50 ) );
			PackSlayer();
			PackScroll( 1, 8 );
			PackScroll( 1, 8 );
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

			switch ( Utility.Random( 10 ) )
			{
				case 0: PackWeapon( 2, 5 ); break;
				case 1: PackArmor( 3, 5 ); break;
			}

				if ( 0.01 > Utility.RandomDouble() )
					PackItem( new Whip() );

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
			switch ( Utility.Random( 22 ) )
			{
				case 0: m.Location = new Point3D(1275,738,-80); break;
				case 1: m.Hits += ( Utility.Random( 10, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
				case 2: m.Location = new Point3D(1300,755,-80); break;
				case 3: m.Hits += ( Utility.Random( 10, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
				case 4: m.Location = new Point3D(1291,757,-80); break;
				case 5: m.Hits += ( Utility.Random( 10, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
				case 6: m.Location = new Point3D(1264,761,-80); break;
				case 7: m.Hits += ( Utility.Random( 10, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
				case 8: m.Location = new Point3D(1228,771,-80); break;
				case 9: m.Hits += ( Utility.Random( 10, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
				case 10: m.Location = new Point3D(1201,749,-80); break;
				case 11: m.Hits += ( Utility.Random( 10, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
				case 12: m.Location = new Point3D(1223,713,-80); break;
				case 13: m.Hits += ( Utility.Random( 10, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
				case 14: m.Location = new Point3D(1214,687,-80); break;
				case 15: m.Hits += ( Utility.Random( 10, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
				case 16: m.Location = new Point3D(1243,696,-80); break;
				case 17: m.Hits += ( Utility.Random( 10, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
				case 18: m.Location = new Point3D(1239,718,-40); break;
				case 19: m.Hits += ( Utility.Random( 10, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
				case 20: m.Location = new Point3D(1261,721,-80); break;
				case 21: m.Hits += ( Utility.Random( 10, 50 ) );
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
					break;
			}
		}

		private DateTime m_NextAttack;

		public override void OnActionCombat()
		{
			Mobile combatant = Combatant;

			if ( combatant == null || combatant.Deleted || combatant.Map != Map || !InRange( combatant, 18 ) || !CanBeHarmful( combatant ) || !InLOS( combatant ) )
				return;

			if ( DateTime.Now >= m_NextAttack )
			{
				SandAttack( combatant );
				m_NextAttack = DateTime.Now + TimeSpan.FromSeconds( 3.0 + (10.0 * Utility.RandomDouble()) );
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
		public override HideType HideType{ get{ return HideType.Barbed; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Lesser; } }

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			defender.Mana -= Utility.Random( 5, 15 );
		}

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
				damage *= 0;
		}

		public VoMystic(Serial serial) : base(serial)
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