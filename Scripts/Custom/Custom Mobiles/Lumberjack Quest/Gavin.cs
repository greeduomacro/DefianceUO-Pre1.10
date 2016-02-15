using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.ContextMenus;
using Server.Gumps;
using Server.Misc;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
	[CorpseName( "Gavin's Corpse" )]
	public class Gavin : Mobile
	{
		public virtual bool IsInvulnerable{ get{ return true; } }
		[Constructable]
		public Gavin()
		{
			Name = "Gavin";
                        Title = "the Forester";
			Body = 0x190;
			CantWalk = true;
			Hue = 0x83F8;
			AddItem( new Server.Items.Boots() );
			AddItem( new Server.Items.Shirt( 1436 ) );
			AddItem( new Server.Items.ShortPants( 1436 ) );
			AddItem( new Server.Items.Hatchet() );

            int hairHue = 1741;

			AddItem( new ShortHair( hairHue ) );

			Blessed = true;
		}



		public Gavin( Serial serial ) : base( serial )
		{
		}

		public override void GetContextMenuEntries( Mobile from, ArrayList list )
		{
				base.GetContextMenuEntries( from, list );
				list.Add( new GavinEntry( from, this ) );
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

		public class GavinEntry : ContextMenuEntry
		{
			private Mobile m_Mobile;
			private Mobile m_Giver;

			public GavinEntry( Mobile from, Mobile giver ) : base( 6146, 3 )
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
					if ( ! mobile.HasGump( typeof( GavinGump ) ) )
					{
						mobile.SendGump( new GavinGump( mobile ));

					}
				}
			}
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
         	        Mobile m = from;
			PlayerMobile mobile = m as PlayerMobile;

			if ( mobile != null)
			{
				if( dropped is Board )
         		{
         			if(dropped.Amount!=500)
         			{
					this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "Ack, nay. There's nay enough here!", mobile.NetState );
         				return false;
         			}

					dropped.Delete();
					mobile.AddToBackpack( new Gold( 2500 ) );
					mobile.SendGump( new GavinFinishGump());



					return true;
         		}
				else if ( dropped is Board)
				{
				this.PrivateOverheadMessage( MessageType.Regular, 1153, 1054071, mobile.NetState );
         			return false;
				}
         		else
         		{
					this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "I have no need of this!", mobile.NetState );
     			}
			}
			return false;
		}
	}
}