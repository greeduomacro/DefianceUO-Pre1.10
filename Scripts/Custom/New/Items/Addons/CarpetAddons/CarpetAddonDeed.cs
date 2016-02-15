using System;
using System.Collections;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Items
{
	public class CarpetAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return null; } }

		[Constructable]
		public CarpetAddonDeed()
		{
			Name = "carpet";
		}

		public CarpetAddonDeed( Serial serial ) : base( serial )
		{
		}
		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			else if ( from.HasGump( typeof(CarpetGump) ) )
				from.SendMessage( "You are already choosing a style of carpet." );
			else
				BoundingBoxPicker.Begin( from, new BoundingBoxCallback( BoundingBox_Callback ), null );
		}

		private void BoundingBox_Callback( Mobile from, Map map, Point3D start, Point3D end, object state )
		{
			IPoint3D p = start as IPoint3D;

			if ( p == null || map == null )
				return;

			int width = (end.X - start.X), height = (end.Y - start.Y);

			if ( width < 2 || height < 2 )
				from.SendMessage( "The bounding targets must be at least a 3x3 box." );
			else if ( IsChildOf( from.Backpack ) )
				from.SendGump( new CarpetGump( this, p, map, width, height ) );
			else
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
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

namespace Server.Gumps
{
	public class CarpetGump : Gump
	{
		private const int EntryCount = 3;

		private BaseAddonDeed m_Deed;
		private IPoint3D m_P3D;
		private Map m_Map;
		private int m_Width;
		private int m_Height;

		public CarpetGump( BaseAddonDeed deed, IPoint3D p, Map map, int width, int height ) : base( 30, 30 )
		{
			m_Deed = deed;
			m_P3D = p;
			m_Map = map;
			m_Width = width;
			m_Height = height;

			AddPage( 0 );

			AddBackground( 0, 0, 450, 180, 9200 );

			AddAlphaRegion( 12, 12, 381, 22 );
			AddHtml( 13, 13, 379, 20, "<BASEFONT COLOR=WHITE>Choose a carpet type</BASEFONT>", false, false );

			AddAlphaRegion( 398, 12, 40, 22 );
			AddAlphaRegion( 12, 39, 426, 129 );

			AddImage( 400, 16, 9766 );
			AddImage( 420, 16, 9762 );
			AddPage( 1 );

			int page = 1;

			for ( int i = 0, index = 0; i < CarpetInfo.Infos.Length; ++i, ++index )
			{
				if ( index >= EntryCount )
				{
					if ( (EntryCount * page) == EntryCount )
						AddImage( 400, 16, 0x2626 );

					AddButton( 420, 16, 0x15E1, 0x15E5, 0, GumpButtonType.Page, page + 1 );

					++page;
					index = 0;

					AddPage( page );

					AddButton( 400, 16, 0x15E3, 0x15E7, 0, GumpButtonType.Page, page - 1 );

					if ( (CarpetInfo.Infos.Length - (EntryCount * page)) < EntryCount )
						AddImage( 420, 16, 0x2622 );
				}

				CarpetInfo info = CarpetInfo.GetInfo( i );

				for ( int j = 0; j < info.Entries.Length; ++j )
					AddItem( 20 + (index * 140 ) + info.Entries[j].OffsetX, 46 + info.Entries[j].OffsetY, info.Entries[j].ItemID );

				AddButton( 20 + (index * 140 ), 46, 1209, 1210, i+1, GumpButtonType.Reply, 0);
			}
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from  = sender.Mobile;

			if ( info.ButtonID >= 1 )
			{
                if (!m_Deed.IsChildOf(from.Backpack))
                    return;

                BaseAddon addon = new CarpetAddon( info.ButtonID-1, m_Width, m_Height );

				Server.Spells.SpellHelper.GetSurfaceTop( ref m_P3D );

				ArrayList houses = null;

				AddonFitResult res = addon.CouldFit( m_P3D, m_Map, from, ref houses );

				if ( res == AddonFitResult.Valid )
					addon.MoveToWorld( new Point3D( m_P3D ), m_Map );
				else if ( res == AddonFitResult.Blocked )
					from.SendLocalizedMessage( 500269 ); // You cannot build that there.
				else if ( res == AddonFitResult.NotInHouse )
					from.SendLocalizedMessage( 500274 ); // You can only place this in a house that you own!
				else if ( res == AddonFitResult.DoorsNotClosed )
					from.SendMessage( "You must close all house doors before placing this." );

				if ( res == AddonFitResult.Valid )
				{
					m_Deed.Delete();

					if ( houses != null )
					{
						foreach ( Server.Multis.BaseHouse h in houses )
							h.Addons.Add( addon );
					}
				}
				else
				{
					addon.Delete();
				}
			}
		}
	}
}