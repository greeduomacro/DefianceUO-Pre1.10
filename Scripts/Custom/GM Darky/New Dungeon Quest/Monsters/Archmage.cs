using System;
using Server;
using Server.Items;
using Server.Spells;
using Server.Spells.Seventh;
using Server.Spells.Fifth;
using Server.Engines.Quests.Collector;

namespace Server.Mobiles
{
	[CorpseName( "an archmage corpse" )]
	public class Archmage : BaseCreature
	{
		public override bool ShowFameTitle{ get{ return false; } }

		[Constructable]
		public Archmage() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.15, 0.2 )
		{
			Title = "the Archmage";

			Hue = Utility.RandomSkinHue();
			Body = 0x190;
			Name = NameList.RandomName( "male" );
			BaseSoundID = 0;
			Kills = 10;
			ShortTermMurders = 10;

                        Item WizardsHat = new WizardsHat();
			WizardsHat.Movable=false;
			WizardsHat.Hue=2118;
			EquipItem( WizardsHat );

			Item Robe = new Robe();
			Robe.Movable=false;
			Robe.Hue=2118;
			EquipItem( Robe );

                        Item Sandals = new Sandals();
			Sandals.Movable=false;
			Sandals.Hue=2118;
			EquipItem( Sandals );

			SetStr( 101, 110 );
			SetDex( 66, 75 );
			SetInt( 161, 170 );

			SetHits( 140, 150 );

			SetDamage( 7, 15 );

			SetSkill( SkillName.Wrestling, 80.3, 87.8 );
			SetSkill( SkillName.Tactics, 80.5, 97.0 );
			SetSkill( SkillName.MagicResist, 120.6, 136.8);
			SetSkill( SkillName.Magery, 97.7, 100.0 );
			SetSkill( SkillName.EvalInt, 75.1, 87.1 );
			SetSkill( SkillName.Meditation, 41.1, 50.1 );

			Fame = 12500;
			Karma = -12500;

			VirtualArmor = 10;

			switch( Utility.Random(100) )
	{
			case 0: PackItem( new EnchantedWood() ); break;
	}

			PackGold( 600, 800 );
			PackReg( Utility.RandomMinMax( 2, 20 ) );
			PackArmor( 0, 5 );
			PackWeapon( 0, 5 );
			PackSlayer();
			PackScroll( 8, 8 );

				if ( 0.05 > Utility.RandomDouble() )
					PackItem( new Obsidian() );
		}

		public override void OnDeath( Container c )
	  	{
			if ( Utility.Random( 100 ) <  1 )
				c.DropItem( new RareBloodCarpet( PieceType.EastEdge ) );

			base.OnDeath( c );
	  	}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override int Meat{ get{ return 1; } }
		public override int TreasureMapLevel{ get{ return 2; } }
		public override Poison PoisonImmune{ get{ return Poison.Lesser; } }

		public void Polymorph( Mobile m )
		{
			if ( !m.CanBeginAction( typeof( PolymorphSpell ) ) || !m.CanBeginAction( typeof( IncognitoSpell ) ) || m.IsBodyMod )
				return;

			IMount mount = m.Mount;

			if ( mount != null )
				mount.Rider = null;

			if ( m.Mounted )
				return;

			if ( m.BeginAction( typeof( PolymorphSpell ) ) )
			{
				Item disarm = m.FindItemOnLayer( Layer.OneHanded );

				if ( disarm != null && disarm.Movable )
					m.AddToBackpack( disarm );

				disarm = m.FindItemOnLayer( Layer.TwoHanded );

				if ( disarm != null && disarm.Movable )
					m.AddToBackpack( disarm );

				m.BodyMod = 0x51;
				m.HueMod = 51;

				new ExpirePolymorphTimer( m ).Start();
			}
		}

		private class ExpirePolymorphTimer : Timer
		{
			private Mobile m_Owner;

			public ExpirePolymorphTimer( Mobile owner ) : base( TimeSpan.FromMinutes( 10.0 ) )
			{
				m_Owner = owner;

				Priority = TimerPriority.OneSecond;
			}

			protected override void OnTick()
			{
				if ( !m_Owner.CanBeginAction( typeof( PolymorphSpell ) ) )
				{
					m_Owner.BodyMod = 0;
					m_Owner.HueMod = -1;
					m_Owner.EndAction( typeof( PolymorphSpell ) );
				}
			}
		}

		public void DoSpecialAbility( Mobile target )
		{
			if ( 0.05 >= Utility.RandomDouble() ) // 5% chance to polymorph attacker into a bullfrog
				Polymorph( target );
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			DoSpecialAbility( attacker );
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			DoSpecialAbility( defender );
		}

		public Archmage( Serial serial ) : base( serial )
		{
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