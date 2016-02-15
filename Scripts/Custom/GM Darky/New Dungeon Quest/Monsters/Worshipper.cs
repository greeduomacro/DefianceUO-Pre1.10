using System;
using Server.Items;
using Server.Engines.Quests.Collector;

namespace Server.Mobiles
{
	[CorpseName( "a worshipper corpse" )]
	public class Worshipper : BaseCreature
	{
		public override bool ShowFameTitle{ get{ return false; } }

		[Constructable]
		public Worshipper() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.15, 0.2 )
		{
			Title = "the Worshipper";

			Hue = Utility.RandomSkinHue();
			Body = 0x190;
			Name = NameList.RandomName( "male" );
			BaseSoundID = 0;
			Kills = 10;
			ShortTermMurders = 10;

                        Item WizardsHat = new WizardsHat();
			WizardsHat.Movable=false;
			WizardsHat.Hue=1;
			EquipItem( WizardsHat );

			Item Skirt = new Skirt();
			Skirt.Movable=false;
			Skirt.Hue=1;
			EquipItem( Skirt );

			Item Shirt = new Shirt();
			Shirt.Movable=false;
			Shirt.Hue=1;
			EquipItem( Shirt );

                        Item Sandals = new Sandals();
			Sandals.Movable=false;
			Sandals.Hue=1;
			EquipItem( Sandals );

			SetStr( 110, 120 );
			SetDex( 86, 95 );
			SetInt( 461, 570 );

			SetHits( 320, 350 );

			SetDamage( 10, 13 );

			SetSkill( SkillName.Wrestling, 80.3, 87.8 );
			SetSkill( SkillName.Tactics, 80.5, 87.0 );
			SetSkill( SkillName.MagicResist, 108.6, 112.8);
			SetSkill( SkillName.Magery, 91.7, 100.0 );
			SetSkill( SkillName.EvalInt, 65.1, 80.1 );
			SetSkill( SkillName.Meditation, 31.1, 40.1 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 15;

			switch( Utility.Random(100) )
	{
			case 0: PackItem( new EnchantedWood() ); break;
	}

			PackGold( 900, 1300 );
			PackArmor( 0, 5 );
			PackWeapon( 0, 5 );

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackReg( 36 ); break;
				case 1: PackScroll( 7, 7 ); break;
				case 2: PackScroll( 8, 8 ); break;
				case 3: PackReg( 33 ); break;
				case 4: PackReg( 31 ); break;
			}

				if ( 0.05 > Utility.RandomDouble() )
					PackItem( new Obsidian() );
		}

		public override void OnDeath( Container c )
	  	{
			if ( Utility.Random( 75 ) <  1 )
				c.DropItem( new RareBloodCarpet( PieceType.SECorner ) );

			base.OnDeath( c );
	  	}


		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		public override int TreasureMapLevel{ get{ return 3; } }

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Controlled || bc.BardTarget == this )
					damage = 0; // Immune to pets and provoked creatures
			}
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			if ( 0.05 >= Utility.RandomDouble() ) // 5% chance to drop or throw a Daemon Gate
				AddDaemonGate( defender, 0.25 );
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.05 >= Utility.RandomDouble() ) // 5% chance to drop or throw a Daemon Gate
				AddDaemonGate( attacker, 0.25 );
		}

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			base.AlterDamageScalarFrom( caster, ref scalar );

			if ( 0.01 >= Utility.RandomDouble() ) // 1% chance to throw a Daemon Gate
				AddDaemonGate( caster, 1.0 );
		}

		public void AddDaemonGate( Mobile target, double chanceToThrow )
		{
			if ( chanceToThrow >= Utility.RandomDouble() )
			{
				Direction = GetDirectionTo( target );
				MovingEffect( target, 0xF7E, 10, 1, true, false, 0x496, 0 );
				new DelayTimer( this, target ).Start();
			}
			else
			{
				new DaemonGate().MoveToWorld( Location, Map );
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
					AOS.Damage( m_Target, m_Mobile, Utility.RandomMinMax( 10, 20 ), 100, 0, 0, 0, 0 );
					new DaemonGate().MoveToWorld( m_Target.Location, m_Target.Map );
				}
			}
		}

		public Worshipper( Serial serial ) : base( serial )
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