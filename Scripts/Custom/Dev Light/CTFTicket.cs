using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;
using Server.Gumps;

namespace Server.Items
{
	public class CTFDonationTicket : Item
	{
		private Mobile m_Owner;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner
		{
			get{ return m_Owner; }
			set{ m_Owner = value; }
		}

		
		//public override int LabelNumber{ get{ return 1041492; } } 

		[Constructable]
		public CTFDonationTicket() : base( 0x14F0 )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
			Name = "a ctf ticket";
			Hue = 1140;
		}

		public CTFDonationTicket( Serial serial ) : base( serial )
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

			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else
			{
				from.SendGump( new InternalGump( from, this ) );
			}
		}

		private class InternalGump : Gump
		{

			public InternalGump( Mobile from, CTFDonationTicket ticket ) : base( 50, 50 )
			{
          		this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			AddPage(0);
			AddBackground(216, 101, 310, 254, 2620);
			AddLabel(240, 203, 900, "This deed entitles the player to page in");
			AddLabel(322, 164, 1280, "  CTF Ticket");
			AddImage(335, 113, 9804);
			AddLabel(240, 221, 900, "This allows a CTF if a Seer/GM is online");
			AddLabel(240, 277, 900, "This can only be used once.");
			AddLabel(240, 297, 900, "The GM will delete this ticket.");


			}



		}
	}
}