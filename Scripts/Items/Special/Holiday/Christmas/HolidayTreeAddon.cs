using System;
using System.Collections;
using Server.Network;
using Server.Items;

namespace Server.Items
{
/*
	public enum HolidayTreeType
	{
		Classic,
		Modern
	}
*/
	public class HolidayTreeAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new HolidayTreeAddonDeed(); } }

		public class OrnamentComponent : AddonComponent
		{
			public override int LabelNumber{ get{ return 1041118; } } // a tree ornament

			public OrnamentComponent( int itemID ) : base( itemID )
			{
			}

			public OrnamentComponent( Serial serial ) : base( serial )
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
		}

		public class TreeTrunkComponent : AddonComponent
		{
			public override int LabelNumber{ get{ return 1041117; } } // a tree for the holidays

			public TreeTrunkComponent( int itemID ) : base( itemID )
			{
			}

			public TreeTrunkComponent( Serial serial ) : base( serial )
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
		}

		public override int LabelNumber{ get{ return 1041117; } } // a tree for the holidays

		public HolidayTreeAddon( HolidayTreeType type ) : base()
		{
			switch ( type )
			{
				case HolidayTreeType.Classic:
				{
					AddComponent( new TreeTrunkComponent( 0xCD6 ), 0, 0, 0 );
					AddComponent( new TreeTrunkComponent( 0xCD7 ), 0, 0, 0 );

					AddOrnamentComponent( 0, 0,  2, 0xF22 );
					AddOrnamentComponent( 0, 0,  9, 0xF18 );
					AddOrnamentComponent( 0, 0, 15, 0xF20 );
					AddOrnamentComponent( 0, 0, 19, 0xF17 );
					AddOrnamentComponent( 0, 0, 20, 0xF24 );
					AddOrnamentComponent( 0, 0, 20, 0xF1F );
					AddOrnamentComponent( 0, 0, 20, 0xF19 );
					AddOrnamentComponent( 0, 0, 21, 0xF1B );
					AddOrnamentComponent( 0, 0, 28, 0xF2F );
					AddOrnamentComponent( 0, 0, 30, 0xF23 );
					AddOrnamentComponent( 0, 0, 32, 0xF2A );
					AddOrnamentComponent( 0, 0, 33, 0xF30 );
					AddOrnamentComponent( 0, 0, 34, 0xF29 );
					AddOrnamentComponent( 0, 1,  7, 0xF16 );
					AddOrnamentComponent( 0, 1,  7, 0xF1E );
					AddOrnamentComponent( 0, 1, 12, 0xF0F );
					AddOrnamentComponent( 0, 1, 13, 0xF13 );
					AddOrnamentComponent( 0, 1, 18, 0xF12 );
					AddOrnamentComponent( 0, 1, 19, 0xF15 );
					AddOrnamentComponent( 0, 1, 25, 0xF28 );
					AddOrnamentComponent( 0, 1, 29, 0xF1A );
					AddOrnamentComponent( 0, 1, 37, 0xF2B );
					AddOrnamentComponent( 1, 0, 13, 0xF10 );
					AddOrnamentComponent( 1, 0, 14, 0xF1C );
					AddOrnamentComponent( 1, 0, 16, 0xF14 );
					AddOrnamentComponent( 1, 0, 17, 0xF26 );
					AddOrnamentComponent( 1, 0, 22, 0xF27 );

					break;
				}
				case HolidayTreeType.Modern:
				{
					AddComponent( new TreeTrunkComponent( 0x1B7E ), 0, 0, 0 );

					AddOrnamentComponent( 0, 0,  2, 0xF2F );
					AddOrnamentComponent( 0, 0,  2, 0xF20 );
					AddOrnamentComponent( 0, 0,  2, 0xF22 );
					AddOrnamentComponent( 0, 0,  5, 0xF30 );
					AddOrnamentComponent( 0, 0,  5, 0xF15 );
					AddOrnamentComponent( 0, 0,  5, 0xF1F );
					AddOrnamentComponent( 0, 0,  5, 0xF2B );
					AddOrnamentComponent( 0, 0,  6, 0xF0F );
					AddOrnamentComponent( 0, 0,  7, 0xF1E );
					AddOrnamentComponent( 0, 0,  7, 0xF24 );
					AddOrnamentComponent( 0, 0,  8, 0xF29 );
					AddOrnamentComponent( 0, 0,  9, 0xF18 );
					AddOrnamentComponent( 0, 0, 14, 0xF1C );
					AddOrnamentComponent( 0, 0, 15, 0xF13 );
					AddOrnamentComponent( 0, 0, 15, 0xF20 );
					AddOrnamentComponent( 0, 0, 16, 0xF26 );
					AddOrnamentComponent( 0, 0, 17, 0xF12 );
					AddOrnamentComponent( 0, 0, 18, 0xF17 );
					AddOrnamentComponent( 0, 0, 20, 0xF1B );
					AddOrnamentComponent( 0, 0, 23, 0xF28 );
					AddOrnamentComponent( 0, 0, 25, 0xF18 );
					AddOrnamentComponent( 0, 0, 25, 0xF2A );
					AddOrnamentComponent( 0, 1,  7, 0xF16 );

					break;
				}
			}
		}

		private void AddOrnamentComponent( int x, int y, int z, int itemID )
		{
			AddComponent( new OrnamentComponent( itemID ), x + 1, y + 1, z + 11 );
		}

		public HolidayTreeAddon( Serial serial ) : base( serial )
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
	}
}