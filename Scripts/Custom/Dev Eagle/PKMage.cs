using System;
using System.Collections;
using Server.Items;
using Server.Mobiles;
using Server.Network;


namespace Server.Mobiles
{
	public class PKMage : BaseCreature
	{
		public override bool ClickTitle{ get{ return false; } }
		private DateTime m_NextAbility;
		[Constructable]
		public PKMage() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.075, 0.4 )
		{
			SpeechHue = Utility.RandomDyedHue();
			Title = "the murderer";
			Body = 0x190;
			Hue = Utility.RandomSkinHue();

			switch (Utility.Random( 51 ))
			{
			case 0: Name = "Trammie Killa"; break;
			case 1:	Name = "L33t D00d"; break;
			case 2:	Name = "Pwnz0r"; break;
			case 3: Name = "N3wB1E";break;
			case 4: Name = "Stone Cold Killa";break;
			case 5: Name = "Sir Laggalot";break;
			case 6: Name = "Gimpy";break;
			case 7: Name = "Dark Knight";break;
			case 8: Name = "Devourer";break;
			case 9: Name = "Mr.Killer";break;
			case 10: Name = "UrAboutToDie";break;
			case 11: Name = "DieNowPlz";break;
			case 12: Name = "Im with stupid";break;
			case 13: Name = "IRoxxJoo";break;
			case 14: Name = "Drone";break;
			case 15: Name = "OwnYourFace";break;
			case 16: Name = "Dirt Napper";break;
			case 17: Name = "Freddy";break;
			case 18: Name = "Jason";break;
			case 19: Name = "RunNowNoob";break;
			case 20: Name = "Death";break;
			case 21: Name = "Cpt. Killa";break;
			case 22: Name = "Rag3L0gger";break;
			case 23: Name = "3p1x";break;
			case 24: Name = "Cron";break;
			case 25: Name = "N3wbl37";break;
			case 26: Name = "Dax";break;
			case 27: Name = "Last Target";break;
			case 28: Name = "an earth elemental";break;
			case 29: Name = "an energy vortex";break;
			case 30: Name = "llama killer";break;
			case 31: Name = "Trammy Slay3r";break;
			case 32: Name = "C4r3834r K1ll3r";break;
			case 33: Name = "Arsenic";break;
			case 34: Name = "Darkness";break;
			case 35: Name = "Dread Pirate";break;
			case 36: Name = "Dr.Doom";break;
			case 37: Name = "K1ll3r B33";break;
			case 38: Name = "sn00p d4wg";break;
			case 39: Name = "killing teletubby";break;
			case 40: Name = "Johnny b Dead";break;
			case 41: Name = "Jackal";break;
			case 42: Name = "6 feet under";break;
			case 43: Name = "ImaNewbie";break;
			case 44: Name = "The Poker";break;
			case 45: Name = "I R U";break;
			case 46: Name = "The Grudge";break;
			case 47: Name = "HoRkuzT";break;
			case 48: Name = "ThAi FightR";break;
			case 49: Name = "KmkzI SoLdR";break;
			case 50: Name = "I win life";break;
			}

			SetStr( 100 );
			SetDex( 25 );
			SetInt( 100 );

			SetHits( 62 );
			SetMana( 10000 );

			SetSkill( SkillName.MagicResist, 100.0 );
			SetSkill( SkillName.Magery, 90.0 );
			SetSkill( SkillName.EvalInt, 90.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 100.0 );
			SetSkill( SkillName.Anatomy, 100.0 );

			Fame = 10000;
			Karma = -10000;

			switch ( Utility.Random( 3 ))
			{
				case 0: AddItem( new ShortPants( Utility.RandomList( 4, 33, 367, 44, 53, 13) ) ); break;
				case 1: AddItem( new Skirt( Utility.RandomList( 4, 33, 367, 44, 53, 13) ) ); break;
				case 2: AddItem( new Kilt( Utility.RandomList( 4, 33, 367, 44, 53, 13) ) ); break;

			}

			switch ( Utility.Random( 3 ))
			{
				case 0: AddItem( new BodySash( Utility.RandomList( 4, 33, 367, 44, 53, 13) ) ); break;
				case 1: AddItem( new Tunic( Utility.RandomList( 4, 33, 367, 44, 53, 13) ) ); break;
				case 2: AddItem( new GoldRing() ); break;

			}

			switch ( Utility.Random( 3 ))
			{
				case 0: AddItem( new WizardsHat( Utility.RandomList( 4, 33, 367, 44, 53, 13) ) ); break;
				case 1: AddItem( new Bandana( Utility.RandomList( 4, 33, 367, 44, 53, 13) ) ); break;
				case 2: AddItem( new GoldRing() ); break;

			}

			PackReg( 30 );

			new Horse().Rider = this;
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 100 ) < 1 )
			c.AddItem( new PKHammer() );

			base.OnDeath( c );
		}

		//constantly regenerating to full health requiring a spell sync to kill
		public override void OnThink()
		{
			this.Hits = this.HitsMax;
		}


		//Melee damage to controlled mobiles is multiplied by 20
		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)to;

				if ( bc.Controlled )
				damage *= 40;
			}
		}

		//damage over 25 from pets is negated
		public override void Damage( int amount, Mobile from )
		{
			if(from is BaseCreature)
			{
               	BaseCreature bc = (BaseCreature)from;

				if ( bc.Controlled && amount >= 25)
				{
					amount = (int)(0);
				}
			}
			base.Damage( amount, from );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 2 );
		}

		public override bool ShowFameTitle{ get{ return false; } }
		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 5; } }
		public override bool BardImmune{ get{ return true; } }

		public PKMage( Serial serial ) : base( serial )
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