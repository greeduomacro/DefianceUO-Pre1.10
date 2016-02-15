using System;
using Server.Items;
using Server.Engines.Quests.Collector;

namespace Server.Mobiles
{
	[CorpseName( "a windcaller corpse" )]
	public class Windcaller : BaseCreature
	{
		public override bool ShowFameTitle{ get{ return false; } }

		[Constructable]
		public Windcaller() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.15, 0.2 )
		{
			Title = "the Windcaller";

			Hue = Utility.RandomSkinHue();
			Body = 0x190;
			Name = NameList.RandomName( "male" );
			BaseSoundID = 0;
			Kills = 10;
			ShortTermMurders = 10;

                        Item WizardsHat = new WizardsHat();
			WizardsHat.Movable=false;
			WizardsHat.Hue=1154;
			EquipItem( WizardsHat );

			Item Skirt = new Skirt();
			Skirt.Movable=false;
			Skirt.Hue=1154;
			EquipItem( Skirt );

                        Item Sandals = new Sandals();
			Sandals.Movable=false;
			Sandals.Hue=1154;
			EquipItem( Sandals );

			SetStr( 110, 120 );
			SetDex( 86, 95 );
			SetInt( 161, 170 );

			SetHits( 120, 130 );

			SetDamage( 5, 13 );

			SetSkill( SkillName.Wrestling, 70.3, 77.8 );
			SetSkill( SkillName.Tactics, 80.5, 87.0 );
			SetSkill( SkillName.MagicResist, 90.6, 92.8);
			SetSkill( SkillName.Magery, 94.7, 96.0 );
			SetSkill( SkillName.EvalInt, 40.1, 44.1 );
			SetSkill( SkillName.Meditation, 21.1, 30.1 );

			Fame = 10000;
			Karma = -10000;

			VirtualArmor = 20;

			switch( Utility.Random(175) )
	{
			case 0: PackItem( new EnchantedWood() ); break;
	}

			PackGold( 400, 700 );
			PackReg( Utility.RandomMinMax( 2, 16 ) );
			PackSlayer();
			PackScroll( 1, 7 );

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

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}

				if ( 0.05 > Utility.RandomDouble() )
					PackItem( new Obsidian() );
		}

		public override void OnDeath( Container c )
	  	{
			if ( Utility.Random( 75 ) <  1 )
				c.DropItem( new RareBloodCarpet( PieceType.SWCorner ) );

			base.OnDeath( c );
	  	}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lesser; } }

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			if ( 0.1 >= Utility.RandomDouble() ) // 10% chance to drop or throw a Windcaller Gate
				AddWindcallerGate( defender, 0.25 );
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.1 >= Utility.RandomDouble() ) // 10% chance to drop or throw a Windcaller Gate
				AddWindcallerGate( attacker, 0.25 );
		}

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			base.AlterDamageScalarFrom( caster, ref scalar );

			if ( 0.05 >= Utility.RandomDouble() ) // 5% chance to throw a Windcaller Gate
				AddWindcallerGate( caster, 1.0 );
		}

		public void AddWindcallerGate( Mobile target, double chanceToThrow )
		{
			if ( chanceToThrow >= Utility.RandomDouble() )
			{
				Direction = GetDirectionTo( target );
				MovingEffect( target, 0xF7E, 10, 1, true, false, 0x496, 0 );
				new DelayTimer( this, target ).Start();
			}
			else
			{
				new WindcallerGate().MoveToWorld( Location, Map );
			}
		}

		private class DelayTimer : Timer
		{
			private Mobile m_Mobile;
			private Mobile m_Target;

			public DelayTimer( Mobile m, Mobile target ) : base( TimeSpan.FromSeconds( 1.0 ) )
			{
				m_Mobile = m;
				m_Target = target;
			}

			protected override void OnTick()
			{
				if ( m_Mobile.CanBeHarmful( m_Target ) )
				{
					m_Mobile.DoHarmful( m_Target );
					AOS.Damage( m_Target, m_Mobile, Utility.RandomMinMax( 1, 20 ), 100, 0, 0, 0, 0 );
					new WindcallerGate().MoveToWorld( m_Target.Location, m_Target.Map );
				}
			}
		}

		public Windcaller( Serial serial ) : base( serial )
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