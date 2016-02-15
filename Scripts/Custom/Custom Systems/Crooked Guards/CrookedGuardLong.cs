using System;
using Server;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Gumps;

namespace Server.Mobiles
{
	public class CrookedGuardLong : BaseCreature
	{
		[Constructable]
		public CrookedGuardLong() : base( AIType.AI_Melee, FightMode.Agressor, 20, 1, 1.5, 3.0 )
		{
			InitStats( 500, 200, 200 );
			Title = "the Crooked Guard";

			SpeechHue = Utility.RandomDyedHue();

			Hue = Utility.RandomSkinHue();

			if ( Female = Utility.RandomBool() )
			{
				Body = 0x191;
				Name = NameList.RandomName( "female" );

				AddItem( new FemalePlateChest() );
				AddItem( new PlateArms() );
				AddItem( new PlateLegs() );

				switch( Utility.Random( 2 ) )
				{
					case 0: AddItem( new Doublet( Utility.RandomNondyedHue() ) ); break;
					case 1: AddItem( new BodySash( Utility.RandomNondyedHue() ) ); break;
				}

				switch( Utility.Random( 2 ) )
				{
					case 0: AddItem( new Skirt( Utility.RandomNondyedHue() ) ); break;
					case 1: AddItem( new Kilt( Utility.RandomNondyedHue() ) ); break;
				}
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "male" );

				AddItem( new PlateChest() );
				AddItem( new PlateArms() );
				AddItem( new PlateLegs() );

				switch( Utility.Random( 3 ) )
				{
					case 0: AddItem( new Doublet( Utility.RandomNondyedHue() ) ); break;
					case 1: AddItem( new Tunic( Utility.RandomNondyedHue() ) ); break;
					case 2: AddItem( new BodySash( Utility.RandomNondyedHue() ) ); break;
				}
			}

			Item hair = new Item( Utility.RandomList( 0x203B, 0x203C, 0x203D, 0x2044, 0x2045, 0x2047, 0x2049, 0x204A ) );

			hair.Hue = Utility.RandomHairHue();
			hair.Layer = Layer.Hair;
			hair.Movable = false;

			AddItem( hair );

			if( Utility.RandomBool() && !this.Female )
			{
				Item beard = new Item( Utility.RandomList( 0x203E, 0x203F, 0x2040, 0x2041, 0x204B, 0x204C, 0x204D ) );

				beard.Hue = hair.Hue;
				beard.Layer = Layer.FacialHair;
				beard.Movable = false;

				AddItem( beard );
			}

			VikingSword weapon = new VikingSword();
			weapon.Movable = false;
			AddItem( weapon );

			HeaterShield shield = new HeaterShield();
			shield.Movable = false;
			AddItem( shield );

			PackGold( 250, 500 );

			Skills[SkillName.Anatomy].Base = 120.0;
			Skills[SkillName.Tactics].Base = 120.0;
			Skills[SkillName.Swords].Base = 120.0;
			Skills[SkillName.MagicResist].Base = 120.0;
			Skills[SkillName.DetectHidden].Base = 100.0;
		}

		public CrookedGuardLong( Serial serial ) : base( serial )
		{
		}

		public override bool HandlesOnSpeech( Mobile from )
		{
			return true;
		}

		public override void OnSpeech( SpeechEventArgs e )
		{
			if ( e.Mobile.InRange( this.Location, 2 ) )
			{
				if ( e.Speech.ToLower().IndexOf( "bribe" ) >= 0 )
				{
					Say( String.Format( "Very well "+e.Mobile.Name+"! Lets see what long-term's you need to lose, just don't tell the other guards!" ) );

					e.Mobile.CloseGump( typeof( GuardBribe ) );
					e.Mobile.SendGump( new GuardBribe( e.Mobile, e.Mobile.Kills, 0, 0 ) );
				}
			}
			base.OnSpeech( e );
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