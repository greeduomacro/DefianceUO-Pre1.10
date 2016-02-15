using System;
using System.Collections;
using Server.Items;
using Server.Spells.Seventh;
using Server.Spells.Fifth;
using Server.Spells;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;
using Server.Engines.Quests.Collector;

namespace Server.Mobiles
{
	public class Cyclonian : BaseCreature
	{
		[Constructable]
		public Cyclonian() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.15, 0.2 )
		{
			Name = "Cyclonian";
			Body = 400;
			SpeechHue= 1359;
			Hue = 1175;
			Title = "the summoner";
			Kills = 10;
			ShortTermMurders = 10;

			SetStr( 596, 700 );
			SetDex( 218, 225 );
			SetInt( 21, 25 );

			SetHits( 4500 );

			SetDamage( 32, 43 );

			SetSkill( SkillName.Macing, 95.0, 97.5 );
			SetSkill( SkillName.MagicResist, 175.0, 190.5 );
			SetSkill( SkillName.Tactics, 95.0, 98.5 );
			SetSkill( SkillName.Anatomy, 96.5, 97.3);

			Fame = 24500;
			Karma = -24500;

			VirtualArmor = 200;

			Item WarHammer = new WarHammer();
			WarHammer.Movable=false;
			WarHammer.Hue=2118;
		        EquipItem( WarHammer );

                        Item BoneHelm = new BoneHelm();
			BoneHelm.Movable=false;
			BoneHelm.Hue=1359;
			EquipItem( BoneHelm );

			Item BoneChest = new BoneChest();
			BoneChest.Movable=false;
			BoneChest.Hue=1359;
			EquipItem( BoneChest );

                        Item BoneGloves = new BoneGloves();
			BoneGloves.Movable=false;
			BoneGloves.Hue=1359;
			EquipItem( BoneGloves );

                        Item BoneLegs = new BoneLegs();
			BoneLegs.Movable=false;
			BoneLegs.Hue=1359;
			EquipItem( BoneLegs );

			Item BoneArms = new BoneArms();
			BoneArms.Movable=false;
			BoneArms.Hue=1359;
			EquipItem( BoneArms );

			switch( Utility.Random(5) )
			{
				case 0: PackItem( new EnchantedWood() ); break;
			}

			PackGold( 4400, 4900 );
			PackArmor( 1, 5 );
			PackWeapon( 1, 5 );
			PackArmor( 1, 5 );
			PackWeapon( 1, 5 );
			PackArmor( 1, 5 );
			PackWeapon( 1, 5 );
			PackArmor( 1, 5 );
			PackWeapon( 1, 5 );
			PackArmor( 1, 5 );
			PackWeapon( 1, 5 );
			PackItem( new Obsidian() );
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 250 ) <  1 )
				c.DropItem( new Rock() );

            		base.OnDeath( c );
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool Uncalmable{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 5; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override bool ClickTitle{ get{ return false; } }

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
				damage *= 4;
		}

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
			{
			if ( from is EnergyVortex || from is BladeSpirits )
				damage = 0; // Immune to Energy Vortex and Blade Spirits
			}
		}

		public override void CheckReflect( Mobile caster, ref bool reflect )
		{
			if ( caster.Body.IsMale )
				reflect = true; // Always reflect if caster isn't female
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			if ( 0.1 >= Utility.RandomDouble() ) // 10% chance to drop or throw an elemental summoning stone
				AddElementalStone( defender, 0.25 );
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.01 >= Utility.RandomDouble() ) // 1% chance to drop or throw an elemental summoning stone
				AddElementalStone( attacker, 0.25 );
		}

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			base.AlterDamageScalarFrom( caster, ref scalar );

			if ( 0.1 >= Utility.RandomDouble() ) // 10% chance to throw an elemental summoning stone
				AddElementalStone( caster, 1.0 );
		}

		public void AddElementalStone( Mobile target, double chanceToThrow )
		{
			if ( chanceToThrow >= Utility.RandomDouble() )
			{
				Direction = GetDirectionTo( target );
				MovingEffect( target, 0xF7E, 10, 1, true, false, 0x496, 0 );
				new DelayTimer( this, target ).Start();
			}
			else
			{
				new ElementalStone().MoveToWorld( Location, Map );
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
					new ElementalStone().MoveToWorld( m_Target.Location, m_Target.Map );
				}
			}
		}

		public Cyclonian( Serial serial ) : base( serial )
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