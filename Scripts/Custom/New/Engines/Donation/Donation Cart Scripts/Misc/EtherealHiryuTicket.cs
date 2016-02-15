using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Items
{
	public class HiryuTicket : Item
	{
		private Mobile m_Owner;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner
		{
			get{ return m_Owner; }
			set{ m_Owner = value; }
		}

		// What a disgustingly long name
		//public override int LabelNumber{ get{ return 1041492; } } // This is half a prize ticket! Double-click this ticket and target any other ticket marked NEW PLAYER and get a prize! This ticket will only work for YOU, so don't give it away!

		[Constructable]
		public HiryuTicket() : base( 0x14F0 )
		{
			Weight = 6.0;
			LootType = LootType.Blessed;
			Name = "an ethereal hiyru ticket";
			Hue = 1158;
		}

		public HiryuTicket( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (Mobile) m_Owner );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_Owner = reader.ReadMobile();
					break;
				}
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			//if ( from != m_Owner )
			//{
			//	from.SendLocalizedMessage( 501926 ); // This isn't your ticket! Shame on you! You have to use YOUR ticket.
			//}
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else
			{
				from.SendGump( new InternalGump( from, this ) );
			}
		}

		private class InternalTarget : Target
		{
			private HiryuTicket m_Ticket;

			public InternalTarget( HiryuTicket ticket ) : base( 2, false, TargetFlags.None )
			{
				m_Ticket = ticket;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted == m_Ticket )
				{
					from.SendLocalizedMessage( 501928 ); // You can't target the same ticket!
				}
				else if ( targeted is HiryuTicket )
				{
					HiryuTicket theirTicket = targeted as HiryuTicket;
					Mobile them = theirTicket.m_Owner;

					if ( them == null || them.Deleted )
					{
						from.SendLocalizedMessage( 501930 ); // That is not a valid ticket.
					}
					else
					{
						from.SendGump( new InternalGump( from, m_Ticket ) );
						them.SendGump( new InternalGump( them, theirTicket ) );
					}
				}
				else if ( targeted is Item && ((Item)targeted).ItemID == 0x14F0 )
				{
					from.SendLocalizedMessage( 501931 ); // You need to find another ticket marked NEW PLAYER.
				}
				else
				{
					from.SendLocalizedMessage( 501929 ); // You will need to select a ticket.
				}
			}
		}

		private class InternalGump : Gump
		{
			private Mobile m_From;
			private HiryuTicket m_Ticket;

			public InternalGump( Mobile from, HiryuTicket ticket ) : base( 50, 50 )
			{
				m_From = from;
				m_Ticket = ticket;

				AddBackground( 0, 0, 400, 385, 0xA28 );

				AddHtml(30, 45, 340, 70,"Defiance Network and it's users humbly bows in appreciation of your financial contribution, please select a reward!",true,true);

				AddButton( 46, 128, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0 );
				AddHtml(80,129,240,24,"Ethereal Hiryu",true,false);
				//AddLabel( 80, 128, 0x489, "Agapite Colored Bundle." );  //# 1test

				//AddButton( 46, 163, 0xFA5, 0xFA7, 2, GumpButtonType.Reply, 0 );
				//AddHtml(80,164,240,24,"Verite Colored Bundle.",true,false);
				//AddLabel( 80, 163, 0x489, "Verite Colored Bundle." );  //# 1test

				//AddButton( 46, 198, 0xFA5, 0xFA7, 3, GumpButtonType.Reply, 0 );
				//AddHtml(80,199,240,24,"Valorite Colored Bundle.",true,false);
				//AddLabel( 80, 198, 0x489, "Valorite Colored Bundle." );  //# 1test

				//AddButton( 46, 233, 0xFA5, 0xFA7, 4, GumpButtonType.Reply, 0 );
				//AddHtml(80,234,240,24,"Silver Colored Bundle.",true,false);
				//AddLabel( 80, 233, 0x489, "Silver Colored Bundle." );  //# 1test

				//AddButton( 46, 268, 0xFA5, 0xFA7, 5, GumpButtonType.Reply, 0 );
				//AddHtml(80,269,240,24,"Golden Colored Bundle.",true,false);
				//AddLabel( 80, 268, 0x489, "Golden Colored Bundle." );  //# 1test

				//AddButton( 46, 303, 0xFA5, 0xFA7, 6, GumpButtonType.Reply, 0 );
				//AddLabel( 50, 320, 0x489, "a wreath deed" );  //# 1test

				AddButton( 120, 340, 0xFA5, 0xFA7, 0, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 154, 342, 100, 35, 1011012, false, false ); // CANCEL
			}

			public override void OnResponse( NetState sender, RelayInfo info )
			{
				if ( m_Ticket == null || m_Ticket.Deleted || !m_Ticket.IsChildOf( sender.Mobile.Backpack ) )
					return;

				//int number = 0;

				Item item = null;
				Item item2 = null;

				switch ( info.ButtonID )
				{
					case 1: item = new EtherealHiryu(); /*number = 1049368;*/ break; // You have been rewarded for your dedication to Justice!.
					//case 2: item = new VeriteDonationBox(); /*number = 1049368;*/ break; // You have been rewarded for your dedication to Justice!.
					//case 3: item = new ValoriteDonationBox(); /*number = 1049368;*/ break; // You have been rewarded for your dedication to Justice!.
					//case 4: item = new SilverDonationBox(); /*number = 1049368;*/ break; // You have been rewarded for your dedication to Justice!.
					//case 5: item = new GoldenDonationBox(); /*number = 1049368;*/ break; // You have been rewarded for your dedication to Justice!.
					//case 6: item = new DyeTub(); item2 = new Dyes(); /*number = 1049368;*/ break; // You have been rewarded for your dedication to Justice!.
				}

				if ( item != null )
				{
					m_Ticket.Delete();

	                                m_From.SendMessage("The item was placed in your bankbox. Thank you for your support! ");
					m_From.BankBox.DropItem( item );

                                        if ( item2 != null)
                                        m_From.BankBox.DropItem( item2 );
				}
			}
		}
	}
}