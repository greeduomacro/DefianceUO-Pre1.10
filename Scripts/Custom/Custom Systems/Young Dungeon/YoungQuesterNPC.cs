using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Targeting;
using Server.ContextMenus;
using Server.Gumps;
using Server.Misc;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
	[CorpseName( "a young corpse" )]
	public class YoungQuestNPC : Mobile
		{
			public virtual bool IsInvulnerable{ get{ return true; } }
			[Constructable]
			public YoungQuestNPC()
				{
					InitStats( 31, 41, 51 );

					Name = "Cameron" ;

					Title = "The young";

					Body = 0x190;

					Hue = Utility.RandomSkinHue();

					int hairHue = Utility.RandomHairHue();

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

					AddItem( new Server.Items.Shirt( Utility.RandomBlueHue() ) );
					AddItem( new Server.Items.LongPants( Utility.RandomBlueHue() ) );
					AddItem( new Server.Items.Sandals( Utility.RandomBlueHue() ) );

					Blessed = true;
					CantWalk = true;

					Container pack = new Backpack();
					pack.DropItem( new Gold( 250, 300 ) );
					pack.Movable = false;
					AddItem( pack );
				}

				public YoungQuestNPC( Serial serial ) : base( serial )
				{
				}

				public override void GetContextMenuEntries( Mobile from, ArrayList list )
				{
				base.GetContextMenuEntries( from, list );
				list.Add( new YoungQuestNPCEntry( from, this ) );
				}

				public override void Serialize( GenericWriter writer )
				{
				base.Serialize( writer );
				writer.Write( (int) 0 );
				}

				public override void Deserialize( GenericReader reader )
				{
				base.Deserialize( reader );
				int version = reader.ReadInt();
				}

				public class YoungQuestNPCEntry : ContextMenuEntry
				{
				private Mobile m_Mobile;
				private Mobile m_Giver;

				public YoungQuestNPCEntry( Mobile from, Mobile giver ) : base( 6146, 3 )
					{
					m_Mobile = from;
					m_Giver = giver;
					}

				public override void OnClick()
					{
					if( !( m_Mobile is PlayerMobile ) )
					return;
					PlayerMobile mobile = (PlayerMobile) m_Mobile;

						{
						if ( ! mobile.HasGump( typeof( YoungQuestGump ) ) )

							{
							mobile.SendGump( new YoungQuestGump( mobile ));
							}
						}
					}
				}

			public override bool OnDragDrop( Mobile from, Item dropped )
			{
               	Mobile m = from;PlayerMobile mobile = m as PlayerMobile;
			if ( mobile != null)
				{
				if( dropped is BlueBall )
					{
					if(dropped.Amount!=5)
						{
						this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "There's not the right amount here!", mobile.NetState );
						return false;
						}
						dropped.Delete();

						mobile.AddToBackpack( new Gold( 1000 ) );
						mobile.AddToBackpack( new YoungTollKey1( ) );

						mobile.SendGump( new YoungQuestGump2( mobile ));

						this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "Thank you young adventurer!", mobile.NetState );

						return true;
					}

				if( dropped is RedBall )
					{
					if(dropped.Amount!=5)
						{
						this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "There's not the right amount here!", mobile.NetState );
						return false;
						}
						dropped.Delete();

						mobile.AddToBackpack( new Gold( 1000 ) );
						mobile.AddToBackpack( new YoungTollKey2( ) );

						mobile.SendGump( new YoungQuestGump3( mobile ));

						this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "Thank you young adventurer!", mobile.NetState );

						return true;
					}

				if( dropped is GreenBall )
					{
					if(dropped.Amount!=5)
						{
						this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "There's not the right amount here!", mobile.NetState );
						return false;
						}
						dropped.Delete();

						mobile.AddToBackpack( new Gold( 1000 ) );
						mobile.AddToBackpack( new YoungTollKey3( ) );

						mobile.SendGump( new YoungQuestGump4( mobile ));

						this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "Thank you young adventurer!", mobile.NetState );

						return true;

					}

					if( dropped is PurpleBall )
					{
					if(dropped.Amount!=5)
						{
						this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "There's not the right amount here!", mobile.NetState );
						return false;
						}
						dropped.Delete();

						mobile.AddToBackpack( new Gold( 1000 ) );
						mobile.AddToBackpack( new YoungParchment( ) );

						mobile.SendGump( new YoungQuestGump5( mobile ));

						this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "Thank you young adventurer!", mobile.NetState );

						return true;

					}


				if ( dropped is Whip)
					{
					this.PrivateOverheadMessage( MessageType.Regular, 1153, 1054071, mobile.NetState );
					return false;

					}
				else{this.PrivateOverheadMessage( MessageType.Regular, 1153, false,"I have no need for this...", mobile.NetState );
			}
		}
return false;
}
}
}