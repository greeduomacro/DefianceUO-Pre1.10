using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Scripts.Commands
{
	public class ClearVendorCommand
	{
		public static void Initialize()
		{
			Server.Commands.Register( "ClearVendor", AccessLevel.Administrator, new CommandEventHandler( ClearContainer_OnCommand ) );
		}

		[Usage( "ClearVendor" )]
		[Description( "Clears bugged vendors." )]
		public static void ClearContainer_OnCommand( CommandEventArgs e )
		{
			foreach ( Mobile m in World.Mobiles.Values )
			{
				if ( m is BaseVendor )
				{
					BaseVendor vendor = m as BaseVendor;
					Container buypack = vendor.BuyPack;
					if ( buypack != null && buypack.Items != Item.EmptyItems )
					{
						ArrayList newitemslist = new ArrayList( buypack.Items );
						buypack.Items.Clear();

						for ( int i = newitemslist.Count - 1;i >= 0; i-- )
						{
							Item item = newitemslist[i] as Item;
							if ( item != null )
								item.Delete();
						}

						buypack.UpdateTotals();
					}
				}
			}
		}
   }
}