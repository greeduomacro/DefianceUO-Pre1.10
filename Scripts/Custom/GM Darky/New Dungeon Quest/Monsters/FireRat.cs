using System;
using Server.Mobiles;
using Server.Items;
using Server.Engines.Quests.Collector;

namespace Server.Mobiles
{
	[CorpseName( "a fire rat corpse" )]
	public class FireRat : BaseCreature
	{
		[Constructable]
		public FireRat() : base( AIType.AI_Melee, FightMode.Weakest, 10, 1, 0.1, 0.3 )
		{
			Name = "a fire rat";
			Body = 0xD7;
			BaseSoundID = 0x188;
			Hue = 1161;
			Kills = 10;
			ShortTermMurders = 10;

			SetStr( 82, 114 );
			SetDex( 66, 85 );

			SetHits( 66, 83 );

			SetDamage( 12, 15 );

			SetSkill( SkillName.MagicResist, 95.1, 98.7 );
			SetSkill( SkillName.Tactics, 59.3, 64.0 );
			SetSkill( SkillName.Wrestling, 79.3, 84.0 );

			Fame = 8500;
			Karma = -8500;

			VirtualArmor = 18;

			switch( Utility.Random(250) )
			{
				case 0: PackItem( new EnchantedWood() ); break;
			}

			PackItem( new SulfurousAsh( Utility.RandomMinMax( 2, 15 ) ) );
			PackGold( 400, 500 );


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
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
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

			if ( combatant == null || combatant.Deleted || combatant.Map != Map || !InRange( combatant, 10 ) || !CanBeHarmful( combatant ) || !InLOS( combatant ) )
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

			m.FixedParticles( 0x373A, 10, 25, 9540, 1160, 0, EffectLayer.Waist );

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
				m_Mobile.PlaySound( 840 );
				AOS.Damage( m_Mobile, m_From, Utility.RandomMinMax( 1, 25 ), 0, 100, 0, 0, 0 );
			}
		}

		public override int Meat{ get{ return 4; } }
		public override int Hides{ get{ return 9; } }
		public override Poison PoisonImmune{ get{ return Poison.Greater; } }

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Controlled || bc.BardTarget == this )
					damage = 0; // Immune to pets and provoked creatures
			}
		}

		public override void CheckReflect( Mobile caster, ref bool reflect )
		{
			reflect = true; // Every spell is reflected back to the caster
		}

		public FireRat(Serial serial) : base(serial)
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