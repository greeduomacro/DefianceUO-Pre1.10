using Server;
using Server.Items;

namespace Server.Scripts.Commands
{
	public class CountShardGold
	{
		public static void Initialize()
		{
			Server.Commands.Register( "CountShardGold", AccessLevel.Administrator, new CommandEventHandler( CountShardGold_OnCommand ) );
		}

		[Usage( "CountShardGold" )]
		[Description( "Counts money on shard." )]
		private static void CountShardGold_OnCommand( CommandEventArgs e )
		{
			Mobile from = e.Mobile;
			if ( from == null  )
				return;

			long gold = 0;
			foreach( Item item in World.Items.Values )
			{
				if ( item is Gold )
					gold += item.Amount;
				else if ( item is BankCheck )
					gold += ((BankCheck)item).Worth;
			}

			from.SendMessage( "Gold on shard: {0}gp.", gold );
		}
	}
}