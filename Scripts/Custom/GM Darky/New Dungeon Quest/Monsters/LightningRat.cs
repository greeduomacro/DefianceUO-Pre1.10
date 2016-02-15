using System;
using Server.Mobiles;
using Server.Items;
using Server.Engines.Quests.Collector;

namespace Server.Mobiles
{
	[CorpseName( "a lightning rat corpse" )]
	public class LightningRat : BaseCreature
	{
		[Constructable]
		public LightningRat() : base( AIType.AI_Melee, FightMode.Weakest, 10, 1, 0.2, 0.4 )
		{
			Name = "a lightning rat";
			Body = 0xD7;
			BaseSoundID = 0x188;
			Hue = 2123;
			Kills = 10;
			ShortTermMurders = 10;

			SetStr( 72, 104 );
			SetDex( 76, 95 );

			SetHits( 76, 93 );

			SetDamage( 8, 12 );

			SetSkill( SkillName.MagicResist, 75.1, 90.0 );
			SetSkill( SkillName.Tactics, 79.3, 84.0 );
			SetSkill( SkillName.Wrestling, 89.3, 94.0 );

			Fame = 5500;
			Karma = -5500;

			VirtualArmor = 18;

			switch( Utility.Random(250) )
			{
				case 0: PackItem( new EnchantedWood() ); break;
			}

			PackItem( new BlackPearl( 5 ) );
			PackGold( 50, 175 );

			switch ( Utility.Random( 4 ) )
			{
				case 0: PackWeapon( 0, 3 ); break;
				case 1: PackArmor( 0, 3 ); break;
			}

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackWeapon( 0, 3 ); break;
				case 1: PackArmor( 0, 3 ); break;
			}

			switch ( Utility.Random( 25 ) )
			{
				case 0: PackWeapon( 3, 5 ); break;
				case 1: PackArmor( 3, 5 ); break;
			}

				if ( 0.05 > Utility.RandomDouble() )
					PackItem( new Obsidian() );
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 250 ) <  1 )
				c.DropItem( new SpiderWeb() );

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

			m.FixedParticles( 0x37CC, 10, 25, 9540, 1155, 0, EffectLayer.Waist );

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
				m_Mobile.PlaySound( 41 );
				AOS.Damage( m_Mobile, m_From, Utility.RandomMinMax( 1, 20 ), 0, 0, 0, 0, 100 );
			}
		}

		public override int Meat{ get{ return 3; } }
		public override int Hides{ get{ return 8; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }

		public LightningRat(Serial serial) : base(serial)
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
	}
}