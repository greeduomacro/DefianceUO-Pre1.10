using System;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.FSPvpPointSystem;

namespace Server.Gumps
{
	public class PvpRewardGump13 : Gump
	{
		private string m_GumpTitle = "Menu Name Here";
		private string m_Item1 = "Empty";
		private string m_Item2 = "Empty";
		private string m_Item3 = "Empty";
		private string m_Item4 = "Empty";
		private string m_Item5 = "Empty";
		private int m_Cost1 = 1;
		private int m_Cost2 = 1;
		private int m_Cost3 = 1;
		private int m_Cost4 = 1;
		private int m_Cost5 = 1;

		public PvpRewardGump13() : base( 25, 25 )
		{
			Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=false;
			AddPage(0);
			AddBackground(14, 13, 298, 227, 9250);
			AddImageTiled(25, 24, 275, 25, 5104);
			AddImageTiled(25, 56, 275, 172, 5104);
			AddAlphaRegion(25, 24, 275, 25);
			AddAlphaRegion(25, 56, 275, 171);
			AddLabel(28, 26, 1149, m_GumpTitle.ToString() );
			AddLabel(77, 57, 1149, @"Buy Items");
			AddButton(30, 80, 4005, 4006, 1, GumpButtonType.Reply, 0);
			AddButton(30, 110, 4005, 4006, 2, GumpButtonType.Reply, 0);
			AddButton(30, 140, 4005, 4006, 3, GumpButtonType.Reply, 0);
			AddButton(30, 170, 4005, 4006, 4, GumpButtonType.Reply, 0);
			AddButton(30, 200, 4005, 4006, 5, GumpButtonType.Reply, 0);
			AddLabel(65, 80, 1160, m_Item1.ToString() + " (" + m_Cost1.ToString() + " Gold)" );
			AddLabel(65, 110, 1160, m_Item2.ToString() + " (" + m_Cost2.ToString() + " Gold)" );
			AddLabel(65, 140, 1160, m_Item3.ToString() + " (" + m_Cost3.ToString() + " Gold)" );
			AddLabel(65, 170, 1160, m_Item4.ToString() + " (" + m_Cost4.ToString() + " Gold)" );
			AddLabel(65, 200, 1160, m_Item5.ToString() + " (" + m_Cost5.ToString() + " Gold)" );

		}

      		public override void OnResponse( NetState state, RelayInfo info )
      		{
			Mobile from = state.Mobile;
			string msg = "You lack the gold to buy this item.";
			string msg2 = "The item has been added to your backpack.";

			if ( from == null )
				return;

        		if ( info.ButtonID == 0 )
         		{
				from.CloseGump( typeof( PvpRewardGump ) );
				from.SendGump( new PvpRewardGump() );
			}

        		if ( info.ButtonID == 1 )
         		{
		   		if ( from.Backpack.ConsumeTotal( typeof( Gold ), m_Cost1 ) )
		   		{
					//from.AddToBackpack( new YOURITEMHERE() );
					from.SendMessage( msg2 );
				}
		   		else if ( from.BankBox.ConsumeTotal( typeof( Gold ), m_Cost1 ) )
		   		{
					//from.AddToBackpack( new YOURITEMHERE() );
					from.SendMessage( msg2 );
				}
				else
				{
					from.SendMessage( msg );
				}
			}

        		if ( info.ButtonID == 2 )
         		{
		   		if ( from.Backpack.ConsumeTotal( typeof( Gold ), m_Cost2 ) )
		   		{
					//from.AddToBackpack( new YOURITEMHERE() );
					from.SendMessage( msg2 );
				}
		   		else if ( from.BankBox.ConsumeTotal( typeof( Gold ), m_Cost2 ) )
		   		{
					//from.AddToBackpack( new YOURITEMHERE() );
					from.SendMessage( msg2 );
				}
				else
				{
					from.SendMessage( msg );
				}
			}

        		if ( info.ButtonID == 3 )
         		{
		   		if ( from.Backpack.ConsumeTotal( typeof( Gold ), m_Cost3 ) )
		   		{
					//from.AddToBackpack( new YOURITEMHERE() );
					from.SendMessage( msg2 );
				}
		   		else if ( from.BankBox.ConsumeTotal( typeof( Gold ), m_Cost3 ) )
		   		{
					//from.AddToBackpack( new YOURITEMHERE() );
					from.SendMessage( msg2 );
				}
				else
				{
					from.SendMessage( msg );
				}
			}

        		if ( info.ButtonID == 4 )
         		{
		   		if ( from.Backpack.ConsumeTotal( typeof( Gold ), m_Cost4 ) )
		   		{
					//from.AddToBackpack( new YOURITEMHERE() );
					from.SendMessage( msg2 );
				}
		   		else if ( from.BankBox.ConsumeTotal( typeof( Gold ), m_Cost4 ) )
		   		{
					//from.AddToBackpack( new YOURITEMHERE() );
					from.SendMessage( msg2 );
				}
				else
				{
					from.SendMessage( msg );
				}
			}

        		if ( info.ButtonID == 5 )
         		{
		   		if ( from.Backpack.ConsumeTotal( typeof( Gold ), m_Cost5 ) )
		   		{
					//from.AddToBackpack( new YOURITEMHERE() );
					from.SendMessage( msg2 );
				}
		   		else if ( from.BankBox.ConsumeTotal( typeof( Gold ), m_Cost5 ) )
		   		{
					//from.AddToBackpack( new YOURITEMHERE() );
					from.SendMessage( msg2 );
				}
				else
				{
					from.SendMessage( msg );
				}
			}
		}
	}
}