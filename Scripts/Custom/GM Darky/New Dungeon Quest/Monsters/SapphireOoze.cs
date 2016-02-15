using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Engines.Quests.Collector;

namespace Server.Mobiles
{
	[CorpseName( "a sapphire ooze corpse" )]
	public class SapphireOoze : BaseCreature
	{
		[Constructable]
		public SapphireOoze() : base( AIType.AI_Melee, FightMode.Closest, 15, 1, 0.03, 0.03 )
		{
			Name = "a sapphire ooze";
			Body = 94;
			BaseSoundID = 456;
			Hue = 1176;
			Kills = 10;
			ShortTermMurders = 10;

			SetStr( 95, 105 );
			SetDex( 90, 100 );

			SetHits( 130, 170 );

			SetDamage( 5, 5 );

			SetSkill( SkillName.MagicResist, 105.1, 110.0 );
			SetSkill( SkillName.Tactics, 189.3, 194.0 );
			SetSkill( SkillName.Anatomy, 29.3, 34.0 );
			SetSkill( SkillName.Wrestling, 85.3, 90.6 );

			Fame = 10500;
			Karma = -10500;

			VirtualArmor = 0;

			switch( Utility.Random(350) )
			{
				case 0: PackItem( new EnchantedWood() ); break;
			}

			PackGold( 200, 400 );

			switch ( Utility.Random( 2 ) )
			{
				case 0: PackWeapon( 0, 3 ); break;
				case 1: PackArmor( 0, 3 ); break;
			}

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackWeapon( 0, 3 ); break;
				case 1: PackArmor( 0, 3 ); break;
			}

			switch ( Utility.Random( 6 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

				if ( 0.05 > Utility.RandomDouble() )
					PackItem( new Obsidian() );

		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 250 ) <  1 )
				c.DropItem( new FloorBlood() );

            		base.OnDeath( c );
		}

		private DateTime m_NextAttack;

		public override void OnActionCombat()
		{
			Mobile combatant = Combatant;

			if ( combatant == null || combatant.Deleted || combatant.Map != Map || !InRange( combatant, 15 ) || !CanBeHarmful( combatant ) || !InLOS( combatant ) )
				return;

			if ( DateTime.Now >= m_NextAttack )
			{
				SandAttack( combatant );
				m_NextAttack = DateTime.Now + TimeSpan.FromSeconds( 10.0 + (3.0 * Utility.RandomDouble()) );
			}
		}

		public void SandAttack( Mobile m )
		{
			DoHarmful( m );

			m.FixedParticles( 0x37C4, 10, 25, 9540, 1265, 0, EffectLayer.Waist );

			new InternalTimer( m, this ).Start();
		}

		private class InternalTimer : Timer
		{
			private Mobile m_Mobile, m_From;

			public InternalTimer( Mobile m, Mobile from ) : base( TimeSpan.FromSeconds( 0 ) )
			{
				m_Mobile = m;
				m_From = from;
			}

			protected override void OnTick()
			{
				m_Mobile.PlaySound( 288 );
				AOS.Damage( m_Mobile, m_From, Utility.RandomMinMax( 1, 10 ), 100, 0, 0, 0, 0 );
			}
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Greater; } }
		public override double HitPoisonChance{ get{ return 0.75; } }

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
				damage *= 20;
		}


		public SapphireOoze( Serial serial ) : base( serial )
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