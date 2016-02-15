using System;
using Server;
using Server.Mobiles;
using System.Collections;
using Server.Items;
using Server.Network;
using System.Data;
using System.IO;
using Server.Engines.CannedEvil;

namespace Server.Scripts.Commands
{
	public class modifychampspawns
	{
		public static void Initialize()
		{
			Server.Commands.Register( "modifychampspawns", AccessLevel.Administrator, new CommandEventHandler( modifychampspawns_OnCommand ) );
		}

		[Usage( "modifychampspawns" )]
		[Description( "Correct all champs in world." )]
		public static void modifychampspawns_OnCommand( CommandEventArgs e )
		{

			ArrayList WipeList = new ArrayList();

			foreach( Item item in World.Items.Values )
			{
				if( item is ChampionSpawn )
				{
					WipeList.Add(item);
				}
			}

			e.Mobile.SendMessage("Fixed {0} champion spawns in world", WipeList.Count);
			for(int i=0; i<WipeList.Count;i++)
			{
				try
				{
					if ((ChampionSpawn)WipeList[i] != null)
					{
						ChampionSpawn css = (ChampionSpawn)WipeList[i];

						if ( css.Platform != null )
							css.Platform.Hue = 0x452;

						if (!css.Active )
						{
							if ( css.Altar != null )
								css.Altar.Hue = 0x452;
						}

						css.Idol = new ChampionIdol(css);
						css.Idol.Map = css.Altar.Map;
						css.Idol.Location = css.Altar.Location;
						css.Idol.Movable = false;
					}
				}
				catch
				{
				}
			}
		}
	}
}