using Server.Gumps;
using Server;
using System;
using System.Collections;
using Server.Mobiles;
using Server.Factions;
using Server.Network;
using Server.Items;
using Server.Regions;

namespace Server.Items
{
	public class FactionSilverConverter : Barrel
	{
		[Constructable]
		public FactionSilverConverter() : base()
		{
			Hue = 2207;
			Movable = false;
			Name = "Faction Silver Converter (Drop old silver here and new silver will appear in your backpack)";
		}

		public FactionSilverConverter( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					break;
				}
			}
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			list.Add( Utility.FixHtml( "Faction Silver Converter (Drop old silver here and new silver will appear in your backpack)" ) );
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			if ( !( dropped is FactionSilver ) )
			{
				from.SendMessage( "You can only drop old faction silver here." );
				return false;
			}
			else
			{
				if ( from.Backpack != null )
				{
					Silver silver = new Silver( ((FactionSilver)dropped).Amount );
					from.Backpack.DropItem( silver );
					dropped.Delete();
					from.SendMessage( "You successfully converted the old silver to new silver!" );
				}
				else
					from.SendMessage( "Where is your backpack?!" );
				return true;
			}
		}

		public override bool OnDragDropInto( Mobile from, Item item, Point3D p )
		{
			return OnDragDrop( from, item );
		}
	}
}