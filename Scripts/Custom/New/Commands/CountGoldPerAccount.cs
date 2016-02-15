//Based on http://www.runuo.com/forums/showthread.php?t=72532&highlight=reagent+vendor&s=c147ca60d2bddc3539bd0a2e267d64f4&

using System;
using System.Collections;
using Server.Accounting;
using Server.Items;
using Server.Mobiles;
using Server.Multis;

namespace Server.Scripts
{
    public class CountGoldPerAccount
	{
        private const int TOP_COUNT = 10; //Display how many accounts?
        public static void Initialize()
		{
			Server.Commands.Register( "CountGoldPerAccount", AccessLevel.Administrator, new CommandEventHandler( CountGoldPerAccount_OnCommand ) );
		}

        [Usage("CountGoldPerAccount")]
		[Description( "Counts the gold owned by an account and writes to file. [Warning: Causes lag of about 2 seconds]" )]
		public static void CountGoldPerAccount_OnCommand( CommandEventArgs arg )
		{
			Mobile from = arg.Mobile;

			object parent;
			Mobile master = null;
			BaseHouse house = null;
			PlayerVendor vendor;
			int value = 0;
			IAccount account;
			uint goldHeldByNPCs = 0;
			uint goldOnFloor = 0;
			uint goldInContainers = 0;
			Hashtable goldOnAccountTable = new Hashtable();
			ArrayList itemList = new ArrayList( World.Items.Values );
			ArrayList mobileList = new ArrayList( World.Mobiles.Values );

			foreach ( Item item in itemList )
			{
				if ( item.Deleted )
					continue;

				if ( item is Gold )
					value = item.Amount;
				else if ( item is BankCheck)
					value = ((BankCheck)item).Worth;
				else
					continue;

				if ( value == 0 )
					continue;

				parent = item.RootParent;
				if ( parent is Mobile )
				{
					if ( parent is PlayerVendor )
						master = ((PlayerVendor)parent).Owner as PlayerMobile;
					else if ( parent is BaseCreature )
						master = ((BaseCreature)parent).ControlMaster as PlayerMobile;
					else
						master = parent as PlayerMobile;

					if ( master == null )
						goldHeldByNPCs += (uint)value;
				}
				else if ( parent != null ) // Containers
				{
					if ( ((Item)parent).IsSecure || ((Item)parent).IsLockedDown )
						house = BaseHouse.FindHouseAt( item );

					if ( house != null )
						master = house.Owner;

					if ( master == null )
						goldInContainers += (uint)value;
				}
				else
				{
					if ( item.IsSecure || item.IsLockedDown )
						house = BaseHouse.FindHouseAt( item );

					if ( house != null )
						master = house.Owner;

					if ( master == null )
						goldOnFloor += (uint)value;
				}

				if ( master != null )
				{
					account = master.Account;
					if (  account != null )
					{
						if ( goldOnAccountTable.Contains( account ) )
							goldOnAccountTable[account] = (uint)goldOnAccountTable[account] + (uint)value;
						else
							goldOnAccountTable[account] = (uint)value;
					}
					else
						from.SendMessage( master.ToString() + " has no account!" );
				}
				parent = null;
				master = null;
				house = null;
			}

			uint goldOnPlayervendors = 0;
			foreach ( Mobile m in mobileList )
			{
				if ( !(m is PlayerVendor) )
					continue;

				vendor = (PlayerVendor)m;
				master = vendor.Owner as PlayerMobile;
				if ( master != null )
				{
					account = master.Account;
					if (  account != null )
					{
						goldOnPlayervendors += (uint)(vendor.BankAccount + vendor.HoldGold);
						if ( goldOnAccountTable.Contains( account ) )
							goldOnAccountTable[account] = (uint)goldOnAccountTable[account] + (uint)(vendor.BankAccount + vendor.HoldGold);
						else
							goldOnAccountTable[account] = (uint)value;
					}
					else
						from.SendMessage( master.ToString() + " has no account!" );
				}
			}

			uint totalGold = goldHeldByNPCs + goldOnFloor + goldInContainers;

            from.SendMessage("Gold on NPCs: " + goldHeldByNPCs.ToString());
            from.SendMessage("Gold on playervendors: " + goldOnPlayervendors.ToString());
            from.SendMessage("Gold in containers: " + goldInContainers.ToString());
            from.SendMessage("Gold on the floor: " + goldOnFloor.ToString());

			IDictionaryEnumerator en = goldOnAccountTable.GetEnumerator();
            ArrayList gaList = new ArrayList();
			GoldAccountEntry gae;
			while (en.MoveNext())
			{
				gae = new GoldAccountEntry();
				gae.Account = (IAccount)(en.Key);
				gae.Value = (uint)(en.Value);
				gaList.Add( gae );
				totalGold += gae.Value;
			}

            from.SendMessage("Total gold: " + totalGold.ToString());
            from.SendMessage("-------------------------");
            from.SendMessage("Gold on Accounts by name:");

			gaList.Sort();
            for (int i = 0; i < Math.Min(gaList.Count,TOP_COUNT); ++i)
            {
                from.SendMessage(((GoldAccountEntry)gaList[i]).Account.ToString() + " : " + ((GoldAccountEntry)gaList[i]).Value.ToString());
            }
		}

		private class GoldAccountEntry : IComparable
		{
			public IAccount Account;
			public uint Value;

			public int CompareTo(object o)
			{
				GoldAccountEntry g = (GoldAccountEntry)o;

				int i = Value.CompareTo( g.Value );

				if ( i != 0 )
					return -i;

				return ((Account)Account).Username.CompareTo( ((Account)g.Account).Username );
			}
		}
	}
}