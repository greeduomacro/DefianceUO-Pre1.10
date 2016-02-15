using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
	public class RookieBrigand : BaseCreature
	{
		public override bool ClickTitle{ get{ return false; } }

		[Constructable]
		public RookieBrigand() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			SpeechHue = Utility.RandomDyedHue();
			Title = "the rookie brigand";
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
				AddItem( new ShortPants( Utility.RandomNeutralHue() ) );
			}

			SetStr( 86, 100 );
			SetDex( 81, 95 );
			SetInt( 61, 75 );

			SetHits( 70, 110 );

			SetDamage( 10, 20 );

			SetSkill( SkillName.Fencing, 66.0, 97.5 );
			SetSkill( SkillName.Macing, 65.0, 87.5 );
			SetSkill( SkillName.MagicResist, 25.0, 47.5 );
			SetSkill( SkillName.Swords, 65.0, 87.5 );
			SetSkill( SkillName.Tactics, 65.0, 87.5 );
			SetSkill( SkillName.Wrestling, 15.0, 37.5 );

			Fame = 1000;
			Karma = -1000;

			AddItem( new Boots( Utility.RandomNeutralHue() ) );
			AddItem( new FancyShirt());
			AddItem( new Bandana());

			int hairHue = Utility.RandomHairHue();

			if ( Female )
			{
				switch ( Utility.Random( 9 ) )
				{
					case 0: AddItem( new Afro( hairHue ) ); break;
					case 1: AddItem( new KrisnaHair( hairHue ) ); break;
					case 2: AddItem( new PageboyHair( hairHue ) ); break;
					case 3: AddItem( new PonyTail( hairHue ) ); break;
					case 4: AddItem( new ReceedingHair( hairHue ) ); break;
					case 5: AddItem( new TwoPigTails( hairHue ) ); break;
					case 6: AddItem( new ShortHair( hairHue ) ); break;
					case 7: AddItem( new LongHair( hairHue ) ); break;
					case 8: AddItem( new BunsHair( hairHue ) ); break;
				}
			}
			else
			{
				switch ( Utility.Random( 8 ) )
				{
					case 0: AddItem( new Afro( hairHue ) ); break;
					case 1: AddItem( new KrisnaHair( hairHue ) ); break;
					case 2: AddItem( new PageboyHair( hairHue ) ); break;
					case 3: AddItem( new PonyTail( hairHue ) ); break;
					case 4: AddItem( new ReceedingHair( hairHue ) ); break;
					case 5: AddItem( new TwoPigTails( hairHue ) ); break;
					case 6: AddItem( new ShortHair( hairHue ) ); break;
					case 7: AddItem( new LongHair( hairHue ) ); break;
				}

				switch ( Utility.Random( 5 ) )
				{
					case 0: AddItem( new LongBeard( hairHue ) ); break;
					case 1: AddItem( new MediumLongBeard( hairHue ) ); break;
					case 2: AddItem( new Vandyke( hairHue ) ); break;
					case 3: AddItem( new Mustache( hairHue ) ); break;
					case 4: AddItem( new Goatee( hairHue ) ); break;
				}
			}
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 15 ) < 1 )
			c.AddItem( new GreenBall() );

			base.OnDeath( c );
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 3; } }

		public RookieBrigand( Serial serial ) : base( serial )
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