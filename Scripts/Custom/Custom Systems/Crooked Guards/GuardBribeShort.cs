using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;

namespace Server.Gumps
{
   	public class GuardBribeShort : Gump
   	{
      	private Mobile m_Owner;
        public Mobile Owner{ get{ return m_Owner; } set{ m_Owner = value; } }

        public int v_Counts;
     	public int v_Bribe;
      	public int v_LCounts;
      	public int v_TotalCost;
      	public int v_TotalLost;
      	public int v_BankedGoldAmount;

      	public string v_s;

      	public GuardBribeShort( Mobile owner, int v_counts, int v_bribe, int v_lCounts ) : base( 10, 10 )
      	{
			owner.CloseGump( typeof( GuardBribeShort ) );

         	v_Counts = v_counts;
         	v_Bribe = v_bribe;
			v_LCounts = v_lCounts;

         	int gumpX = 0; int gumpY = 0; bool initialState = false;

		    m_Owner = owner;

         	Closable = true;
         	Disposable = false;
         	Dragable = true;
         	Resizable = false;

         	v_Counts = owner.ShortTermMurders;
         	v_BankedGoldAmount = Banker.GetBalance( owner );

         	AddPage( 0 );

         	gumpX = 0; gumpY = 0;
         	AddBackground( gumpX, gumpY, 275, 154, 0x53 );

         	gumpX = 89; gumpY = 11;
         	AddLabel( gumpX, gumpY, 37, "Short Term Counts" );

         	gumpX = 66; gumpY = 38;
         	AddLabel( gumpX, gumpY, 40, "Short Terms:" );

         	gumpX = 128; gumpY = 40;
         	AddImageTiled( gumpX, gumpY, 81, 17, 0xC8 );

         	gumpX = 75; gumpY = 68;
         	AddLabel( gumpX, gumpY, 40, "Bribe:" );

        	gumpX = 128; gumpY = 69;
         	AddImageTiled( gumpX, gumpY, 81, 17, 0xC8 );

         	gumpX = 38; gumpY = 95;
         	AddLabel( gumpX, gumpY, 40, "Lost:" );

         	gumpX = 128; gumpY = 96;
         	AddImageTiled( gumpX, gumpY, 81, 17, 0xC8 );

         	gumpX = 63; gumpY = 65;
         	AddButton( gumpX, gumpY, 0x984, 0x983, 1, GumpButtonType.Reply, 0 );

         	gumpX = 210; gumpY = 72;
         	AddLabel( gumpX, gumpY, 40, "gp" );

         	gumpX = 63; gumpY = 79;
         	AddButton( gumpX, gumpY, 0x986, 0x985, 0, GumpButtonType.Reply, 0 );

         	gumpX = 169; gumpY = 123;
         	AddButton( gumpX, gumpY, 0x992, 0x993, 2, GumpButtonType.Reply, 0 );

         	gumpX = 213; gumpY = 123;
         	AddButton( gumpX, gumpY, 0x995, 0x996, 3, GumpButtonType.Reply, 0 );

         	gumpX = 129; gumpY = 39;
         	AddLabel( gumpX, gumpY, 0, ""+v_Counts+"" );

         	gumpX = 129; gumpY = 68;
         	AddLabel( gumpX, gumpY, 0, ""+v_Bribe+"" );

         	gumpX = 129; gumpY = 94;
         	AddLabel( gumpX, gumpY, 0, ""+v_LCounts+"" );
      	}

      	public override void OnResponse( NetState state, RelayInfo info )
      	{
         	Mobile from = state.Mobile;

         	switch( info.ButtonID )
         	{
            	case 1:
               	{
                  	v_Counts = from.ShortTermMurders;

				  	if ( v_BankedGoldAmount >= 30000 && v_Counts >= 1 && v_LCounts < from.ShortTermMurders )
                  	{
                     	from.SendMessage( "You have increased your bribe by 30,000gp!" );

                    	v_Bribe = v_Bribe + 30000;
                     	v_LCounts = v_LCounts + 1;

                     	v_TotalCost = v_Bribe;
                     	v_TotalLost = v_LCounts;

                     	if ( v_LCounts > 1 )
                     	{
                        	v_s = "s";
                     	}
                     	else if ( v_LCounts == 0 )
                     	{
                        	v_s = "s";
                     	}
                     	else
                     	{
                        	v_s = "";
                     	}

                     	from.SendMessage( "If you accept the bribe, you will lose "+v_LCounts+" count"+v_s+" (short term) and "+v_Bribe+" gp!" );
                     	from.SendGump( new GuardBribeShort( from, from.ShortTermMurders, v_TotalCost, v_TotalLost ) );
                  	}
					else if ( v_Counts == 0 )
					{
						from.SendMessage( "Why would you bribe the guards? You have 0 counts!" );
					}
					else if ( v_BankedGoldAmount < 30000 )
					{
						from.SendMessage( "You do not have enough gold in your bank to increase your bribe!" );
						from.SendGump( new GuardBribeShort( from, from.ShortTermMurders, v_TotalCost, v_TotalLost ) );
					}
					else if ( v_LCounts >= from.ShortTermMurders )
					{
						v_LCounts = from.ShortTermMurders;

						from.SendMessage( "You cannot bribe off more counts than you have!" );
						from.SendGump( new GuardBribeShort( from, from.ShortTermMurders, 0, 0 ) );
					}
					else
					{
						from.SendMessage( "ERROR Insufficient gold?" );
					}
               	}
               	break;

            	case 0:
               	{
                  	if ( v_Bribe >= 30000 )
                  	{
                     	from.SendMessage( "You have decreased your bribe by 30,000gp!" );

                     	v_Bribe = v_Bribe - 30000;
                     	v_LCounts = v_LCounts - 1;

                     	v_TotalCost = v_Bribe;
                     	v_TotalLost = v_LCounts;

                     	if ( v_LCounts > 1 )
                     	{
                        	v_s = "s";
                     	}
                     	else if ( v_LCounts == 0 )
                     	{
                        	v_s = "s";
                     	}
                     	else
                     	{
                        	v_s = "";
                     	}

                     	from.SendMessage( "If you accept the bribe, you will lose "+v_LCounts+" count"+v_s+" and "+v_Bribe+" gp!" );
                     	from.SendGump( new GuardBribeShort( from, from.ShortTermMurders, v_TotalCost, v_TotalLost ) );
                  	}
                  	else
                  	{
                     	from.SendMessage( "You cannot decrease your bribe any further!" );
                     	from.SendGump( new GuardBribeShort( from, from.ShortTermMurders, v_TotalCost, v_TotalLost ) );

                    	v_Bribe = 0;
                     	v_LCounts = 0;
                  	}
               	}
               	break;

            	case 2:
               	{
                  	if ( v_LCounts > 0 )
                  	{
                     	if ( v_LCounts > 1 )
                     	{
                        	v_s = "s";
                     	}
                     	else if ( v_LCounts == 0 )
                     	{
                        	v_s = "s";
                     	}
                     	else
                     	{
                        	v_s = "";
                     	}

                     	// Banker.Withdraw ( from, v_Bribe ); //Why doesn't this work?

                     	BankBox box = from.BankBox;

                     	if ( box == null || !box.ConsumeTotal( typeof( Gold ), v_Bribe ) )
                     	{
							from.SendMessage( "ERROR Insufficient gold?" );
                     	}
                     	else
                     	{
                        	from.ShortTermMurders = v_Counts - v_LCounts;
                        	from.SendMessage( "You have bribed the guards and lost "+v_LCounts+" kill"+v_s+" (short term) and "+v_Bribe+" gp!" );
                     	}
                  	}
                  	else
                  	{
                     	from.SendMessage( "You have not offered a bribe!" );
                  	}
               	}
               	break;

            	case 3:
               	{
                  	from.SendMessage( "You decide not to offer a bribe!" );
               	}
               	break;
         	}
      	}
   	}
}