using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;
using Server.Engines.Quests.Collector;

namespace Server.Mobiles
{
	public class CrazySmith : BaseCreature
	{
		[Constructable]
		public CrazySmith() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.15, 0.2 )
		{
			SpeechHue = Utility.RandomDyedHue();
			Title = "the crazy smith";
			Hue = Utility.RandomSkinHue();

			if ( this.Female = Utility.RandomBool() )
			{
				Body = 0x191;
				Name = NameList.RandomName( "female" );
				AddItem( new Skirt( Utility.RandomNeutralHue() ) );
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "male" );
				AddItem( new LongPants( Utility.RandomNeutralHue() ) );
			}

			Kills = 10;
			ShortTermMurders = 10;
			MagicDamageAbsorb = 5;

			SetStr( 96, 100 );
			SetDex( 91, 100 );
			SetInt( 21, 25 );

			SetHits( 160, 180 );
			SetDamage( 12, 23 );

			SetSkill( SkillName.Swords, 95.0, 99.5 );
			SetSkill( SkillName.MagicResist, 75.0, 87.5 );
			SetSkill( SkillName.Tactics, 85.0, 87.5 );
			SetSkill( SkillName.Wrestling, 65.0, 77.5 );

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 40;

			AddItem( new Boots( Utility.RandomNeutralHue() ) );
			AddItem( new FullApron( Utility.RandomNeutralHue() ) );
			AddItem( new Pickaxe());
			AddItem( new RingmailGloves());

			Item hair = new Item( Utility.RandomList( 0x203B, 0x2049, 0x2048, 0x204A ) );
			hair.Hue = Utility.RandomNondyedHue();
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem( hair );

			Item item = null;
			switch( Utility.Random(500) )
		{
			case 0: PackItem( item = new Item(0xFB1) ); break;
		}
			if (item != null)
			{
			item.Name = "A Magic Forge";
			item.Weight = 50;
			}

			switch ( Utility.Random( 20 ) )
			{
				case 0: PackItem( new IronOre( Utility.RandomMinMax( 1, 5 ) ) ); break;
				case 1: PackItem( new DullCopperOre( Utility.RandomMinMax( 1, 5 ) ) ); break;
				case 2: PackItem( new ShadowIronOre( Utility.RandomMinMax( 1, 5 ) ) ); break;
				case 3: PackItem( new CopperOre( Utility.RandomMinMax( 1, 5 ) ) ); break;
				case 4: PackItem( new BronzeOre( Utility.RandomMinMax( 1, 5 ) ) ); break;
				case 5: PackItem( new GoldOre( Utility.RandomMinMax( 1, 5 ) ) ); break;
				case 6: PackItem( new AgapiteOre( Utility.RandomMinMax( 1, 5 ) ) ); break;
				case 7: PackItem( new VeriteOre( Utility.RandomMinMax( 1, 5 ) ) ); break;
				case 8: PackItem( new ValoriteOre( Utility.RandomMinMax( 1, 5 ) ) ); break;
			}

			PackGold( 400, 600 );
			PackItem( new Bandage( Utility.RandomMinMax( 5, 10 ) ) );


			switch ( Utility.Random( 2 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 3 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 4 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}

			switch( Utility.Random(150) )
	{
			case 0: PackItem( new EnchantedWood() ); break;
	}

			switch ( Utility.Random( 15 ) )
			{
				case 0: PackItem( new IronIngot( Utility.RandomMinMax( 2, 5 ) ) ); break;
				case 1: PackItem( new DullCopperIngot( Utility.RandomMinMax( 2, 5 ) ) ); break;
				case 2: PackItem( new ShadowIronIngot( Utility.RandomMinMax( 2, 5 ) ) ); break;
				case 3: PackItem( new CopperIngot( Utility.RandomMinMax( 2, 5 ) ) ); break;
				case 4: PackItem( new BronzeIngot( Utility.RandomMinMax( 2, 5 ) ) ); break;
				case 5: PackItem( new GoldIngot( Utility.RandomMinMax( 2, 5 ) ) ); break;
				case 6: PackItem( new AgapiteIngot( Utility.RandomMinMax( 2, 5 ) ) ); break;
				case 7: PackItem( new VeriteIngot( Utility.RandomMinMax( 2, 5 ) ) ); break;
				case 8: PackItem( new ValoriteIngot( Utility.RandomMinMax( 2, 5 ) ) ); break;
			}

				if ( 0.05 > Utility.RandomDouble() )
					PackItem( new Obsidian() );

		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			defender.Stam -= Utility.Random( 1, 3 );
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lesser; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 3; } }
		public override bool AutoDispel{ get{ return true; } }
		public override bool ShowFameTitle{ get{ return false; } }

		private DateTime m_NextAttack;

		public override void OnActionCombat()
		{
			Mobile combatant = Combatant;

			if ( combatant == null || combatant.Deleted || combatant.Map != Map || !InRange( combatant, 2 ) || !CanBeHarmful( combatant ) || !InLOS( combatant ) )
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

			m.FixedParticles( 0x2AE8, 10, 25, 9540, 0, 0, EffectLayer.Waist );

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
				m_Mobile.PlaySound( 855 );
				AOS.Damage( m_Mobile, m_From, Utility.RandomMinMax( 15, 25 ), 0, 100, 0, 0, 0 );
			}
		}

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is EvolutionDragon || to is Dragon || to is WhiteWyrm || to is Nightmare )
				damage *= 20;
		}

		public CrazySmith( Serial serial ) : base( serial )
		{
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.95 >= Utility.RandomDouble() && attacker is BaseCreature )
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

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}