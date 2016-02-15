using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
	public class BrigandTamer : BaseCreature
	{
		public override bool IsScaryToPets{ get{ return true; } }

		[Constructable]
		public BrigandTamer() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.15, 0.2 )
		{
			SpeechHue = Utility.RandomDyedHue();
			Title = "the tamer";
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
			MagicDamageAbsorb = 10;

			SetStr( 106, 110 );
			SetDex( 91, 100 );
			SetInt( 21, 25 );

			SetHits( 150, 164 );
			SetDamage( 12, 23 );

			SetSkill( SkillName.Macing, 96.0, 97.5 );
			SetSkill( SkillName.MagicResist, 25.0, 27.5 );
			SetSkill( SkillName.Tactics, 88.0, 89.5 );
			SetSkill( SkillName.Wrestling, 105.0, 107.5 );

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 20;

			AddItem( new Shoes( Utility.RandomNeutralHue() ) );
			AddItem( new FancyShirt());
			AddItem( new FeatheredHat());
			AddItem( new ShepherdsCrook());

			Item hair = new Item( Utility.RandomList( 0x203B, 0x2049, 0x2048, 0x204A ) );
			hair.Hue = Utility.RandomNondyedHue();
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem( hair );

			switch( Utility.Random(150) )
			{
				case 0: PackItem( new DarkIronWire() ); break;
			}

			PackGold( 200, 450 );
			PackPotion();
			PackItem( new Bandage( Utility.RandomMinMax( 5, 10 ) ) );
			PackArmor( 0, 4 );
			PackWeapon( 0, 4 );

		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 250 ) <  1 )
				c.DropItem( new Hay() );

            		base.OnDeath( c );
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			defender.Stam -= Utility.Random( 1, 5 );
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lesser; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 2; } }
		public override bool ShowFameTitle{ get{ return false; } }

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
				damage *= 15;
		}

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Controlled || bc.BardTarget == this )
					damage = 0; // Immune to pets and provoked creatures
			}
		}

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
			{
			scalar = 0.0; // Immune to magic
			}

		public BrigandTamer( Serial serial ) : base( serial )
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