using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;
using Server.Gumps;

namespace Server.Items
{
	public class LargeReward2 : Item
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
		public LargeReward2() : base( 0x14F0 )
		{
			Weight = 1.0;
			LootType = LootType.Regular;
			Name = "a bod ticket";
			Hue = 1150;
		}

		public LargeReward2( Serial serial ) : base( serial )
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
				from.SendGump( new InternalGump7( from, this ) );
			}
		}

		private class InternalGump7 : Gump
		{

			public InternalGump7( Mobile from, LargeReward2 ticket ) : base( 50, 50 )
			{
			Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=false;
			AddPage(0);
			AddBackground(6, 21, 385, 129, 9200);
			AddHtml( 28, 45, 340, 70, @"This is a Large BOD Reward Ticket, this will be replaced by a reward in the near future.",true,true);

			}



		}
	}
}