using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Engines.Quests.Collector;

namespace Server.Mobiles
{
	[CorpseName( "an emerald ooze corpse" )]
	public class EmeraldOoze : BaseCreature
	{
		[Constructable]
		public EmeraldOoze() : base( AIType.AI_Mage, FightMode.Weakest, 10, 1, 0.06, 0.06 )
		{
			Name = "an emerald ooze";
			Body = 94;
			BaseSoundID = 456;
			Hue = 1272;
			Kills = 10;
			ShortTermMurders = 10;

			SetStr( 95, 105 );
			SetDex( 90, 100 );
			SetInt( 190, 200 );

			SetHits( 90, 110 );

			SetDamage( 5, 10 );

			SetSkill( SkillName.MagicResist, 105.1, 110.0 );
			SetSkill( SkillName.Magery, 94.3, 96.5 );
			SetSkill( SkillName.EvalInt, 39.3, 44.0 );
			SetSkill( SkillName.Meditation, 45.3, 55.0 );
			SetSkill( SkillName.Wrestling, 40.4, 45.7);
			SetSkill( SkillName.Tactics, 155.7, 160.4);

			Fame = 10500;
			Karma = -10500;

			VirtualArmor = 105;

			switch( Utility.Random(250) )
			{
				case 0: PackItem( new EnchantedWood() ); break;
			}

			PackGold( 300, 400 );
			PackWeapon( 0, 5 );
			PackSlayer();

				if ( 0.05 > Utility.RandomDouble() )
					PackItem( new Obsidian() );
		}


		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 250 ) <  1 )
				c.DropItem( new HangingBlood() );

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
				m_NextAttack = DateTime.Now + TimeSpan.FromSeconds( 10.0 + (5.0 * Utility.RandomDouble()) );
			}
		}

		public void SandAttack( Mobile m )
		{
			DoHarmful( m );

			m.FixedParticles( 0x37C4, 10, 25, 9540, 1271, 0, EffectLayer.Waist );

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
				AOS.Damage( m_Mobile, m_From, Utility.RandomMinMax( 1, 15 ), 100, 0, 0, 0, 0 );
			}
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Lethal; } }
		public override double HitPoisonChance{ get{ return 0.75; } }

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
				damage *= 10;
		}


		public EmeraldOoze( Serial serial ) : base( serial )
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