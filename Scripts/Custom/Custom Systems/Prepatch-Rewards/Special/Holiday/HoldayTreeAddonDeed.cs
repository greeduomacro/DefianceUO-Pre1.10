using System;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Multis;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public class HolidayTreeAddonDeed : BaseAddonDeed
	{
		private HolidayTreeType m_TreeType;
		public HolidayTreeType TreeType{ get{ return m_TreeType; } set{ m_TreeType = value; } }

		public override BaseAddon Addon{ get{ return new HolidayTreeAddon( m_TreeType ); } }
		public override int LabelNumber{ get{ return 1041116; } } // a deed for a holiday tree

		[Constructable]
		public HolidayTreeAddonDeed() : base()
		{
			Hue = 0x488;
			Weight = 1.0;
		}

		public HolidayTreeAddonDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}

		public void BeginPlace( Mobile from, HolidayTreeType type )
		{
			m_TreeType = type;
			base.OnDoubleClick( from );
			//from.BeginTarget( -1, true, TargetFlags.None, new TargetStateCallback( Placement_OnTarget ), type );
		}

		public override void OnDoubleClick( Mobile from )
		{
			Map map = from.Map;

			if ( map == null || map == Map.Internal )
				return;

			if ( from.AccessLevel >= AccessLevel.GameMaster )
			{
				from.CloseGump( typeof( HolidayTreeAddonChoiceGump ) );
				from.SendGump( new HolidayTreeAddonChoiceGump( from, this ) );
			}
			else
			{

				if ( !IsChildOf( from.Backpack ) )
					from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
				else if ( !from.InRange( this.GetWorldLocation(), 1 ) )
					from.SendLocalizedMessage( 500446 ); // That is too far away.
				else if ( DateTime.Now.Month != 12 )
					from.SendLocalizedMessage( 1005700 ); // You will have to wait till next December to put your tree back up for display.
				else
				{
					from.CloseGump( typeof( HolidayTreeAddonChoiceGump ) );
					from.SendGump( new HolidayTreeAddonChoiceGump( from, this ) );
				}
			}
		}
	}

	public class HolidayTreeAddonChoiceGump : Gump
	{
		private Mobile m_From;
		private HolidayTreeAddonDeed m_Deed;

		public HolidayTreeAddonChoiceGump( Mobile from, HolidayTreeAddonDeed deed ) : base( 200, 200 )
		{
			m_From = from;
			m_Deed = deed;

			AddPage( 0 );

			AddBackground( 0, 0, 220, 120, 5054 );
			AddBackground( 10, 10, 200, 100, 3000 );

			AddButton( 20, 35, 4005, 4007, 1, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 55, 35, 145, 25, 1018322, false, false ); // Classic

			AddButton( 20, 65, 4005, 4007, 2, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 55, 65, 145, 25, 1018321, false, false ); // Modern
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if ( m_Deed.Deleted )
				return;

			switch ( info.ButtonID )
			{
				case 1:
				{
					m_Deed.BeginPlace( m_From, HolidayTreeType.Classic );
					break;
				}
				case 2:
				{
					m_Deed.BeginPlace( m_From, HolidayTreeType.Modern );
					break;
				}
			}
		}
	}
}