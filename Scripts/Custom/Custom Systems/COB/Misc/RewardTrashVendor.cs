using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Prompts;
using Server.Targeting;
using Server.Misc;
using Server.Multis;
using Server.ContextMenus;

namespace Server.Mobiles
{
	public class CopperRewardVendor : Mobile
	{
  	[Constructable]
		public CopperRewardVendor( ) : base( )
		{
			InitBody();
			InitOutfit();
		}

		public CopperRewardVendor( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}

		public virtual void InitOutfit()
		{
			Item item = new FancyShirt( Utility.RandomNeutralHue() );
			item.Layer = Layer.InnerTorso;
			AddItem( item );
			AddItem( new LongPants( Utility.RandomNeutralHue() ) );
			AddItem( new BodySash( Utility.RandomNeutralHue() ) );
			AddItem( new Boots( Utility.RandomNeutralHue() ) );
			AddItem( new Cloak( Utility.RandomNeutralHue() ) );

			Item hair = new Item( Utility.RandomList( 0x203B, 0x2049, 0x2048, 0x204A ) );
			hair.Hue = Utility.RandomNondyedHue();
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem( hair );

			Container pack = new VendorBackpack();
			pack.Movable = false;
			AddItem( pack );
		}

		public void InitBody()
		{
			Hue = Utility.RandomSkinHue();
			SpeechHue = 0x3B2;
            Title = "the Platinum Agent";
            InitStats(100, 100, 25);
            Blessed = true;

            CantWalk = true;

			if ( !Core.AOS )
				NameHue = 0x35;

			if ( this.Female = Utility.RandomBool() )
			{
				this.Body = 0x191;
				this.Name = NameList.RandomName( "female" );
			}
			else
			{
				this.Body = 0x190;
				this.Name = NameList.RandomName( "male" );
			}
		}

		public bool CanInteractWith( Mobile from )
		{
			if ( !from.CanSee( this ) || !Utility.InUpdateRange( from, this ) || !from.CheckAlive() )
				return false;
			return true;
		}

		public override void GetContextMenuEntries( Mobile from, ArrayList list )
		{
			if ( from.Alive && from.GetDistanceToSqrt( this ) <= 3)
			{
				list.Add( new BuyVendorEntry( this ) );
			}
			base.GetContextMenuEntries( from, list );
		}

		public override bool HandlesOnSpeech( Mobile from )
		{
			return ( from.Alive && from.GetDistanceToSqrt( this ) <= 3 );
		}

		public override void OnSpeech( SpeechEventArgs e )
		{
			Mobile from = e.Mobile;

			if ( e.Handled || !from.Alive || from.GetDistanceToSqrt( this ) > 3 )
				return;

			if ( e.HasKeyword( 0x3C ) || (e.HasKeyword( 0x171 ) )  ) // vendor buy, *buy*
			{
                from.CloseGump(typeof(TokenStoneGump));
                from.SendGump(new TokenStoneGump(from));
			}
		}
		private class BuyVendorEntry : ContextMenuEntry
		{
			public BuyVendorEntry( CopperRewardVendor vendor ) : base( 6103 )
			{
			}
			public override void OnClick()
			{
				Mobile from = Owner.From;
                from.CloseGump(typeof(TokenStoneGump));
                from.SendGump(new TokenStoneGump(from));
			}
		}
	}
}