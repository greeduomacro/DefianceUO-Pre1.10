using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
	public class Executioner : BaseCreature
	{
		[Constructable]
		public Executioner() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			SpeechHue = Utility.RandomDyedHue();
			Title = "the executioner";
			Hue = Utility.RandomSkinHue();

			if ( this.Female = Utility.RandomBool() )
			{
				this.Body = 0x191;
				this.Name = NameList.RandomName( "female" );
				AddItem( new Skirt( Utility.RandomRedHue() ) );
			}
			else
			{
				this.Body = 0x190;
				this.Name = NameList.RandomName( "male" );
				AddItem( new ShortPants( Utility.RandomRedHue() ) );
			}

			MeleeDamageAbsorb = 500;

			SetStr( 386, 400 );
			SetDex( 151, 165 );
			SetInt( 161, 175 );

			SetDamage( 10, 14 );

			SetSkill( SkillName.Anatomy, 125.0 );
			SetSkill( SkillName.MagicResist, 87.5, 88.5 );
			SetSkill( SkillName.Swords, 125.0 );
			SetSkill( SkillName.Tactics, 125.0 );
			SetSkill( SkillName.Lumberjacking, 125.0 );

			Fame = 8000;
			Karma = -8000;

			VirtualArmor = 40;

			AddItem( new ThighBoots( Utility.RandomRedHue() ) );
			AddItem( new Surcoat( Utility.RandomRedHue() ) );
			AddItem( new ExecutionersAxe());

			Item hair = new Item( Utility.RandomList( 0x203B, 0x2049, 0x2048, 0x204A ) );
			hair.Hue = Utility.RandomNondyedHue();
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem( hair );

                        ExecutionersAxe weapon = new ExecutionersAxe();

			weapon.DamageLevel = (WeaponDamageLevel)Utility.Random( 0, 5 );
			weapon.DurabilityLevel = (WeaponDurabilityLevel)Utility.Random( 0, 5 );
			weapon.AccuracyLevel = (WeaponAccuracyLevel)Utility.Random( 0, 5 );

			PackItem( weapon );

			PackGold( 750, 800 );
			PackArmor( 0, 5 );
			PackWeapon( 0, 5 );
		}

		public override void OnDeath( Container c )
	  	{
			if ( Utility.Random( 100 ) <  1 )
				c.DropItem( new RareBloodCarpet( PieceType.NECorner ) );

			base.OnDeath( c );
	  	}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			defender.Stam -= Utility.Random( 10, 20 );
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool Uncalmable{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }

		public Executioner( Serial serial ) : base( serial )
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