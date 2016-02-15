using System;
using System.Net;
using Server;
using Server.Accounting;
using Server.Network;
using Server.Targets;
using Server.Scripts.Gumps;

namespace Server.Gumps
{
   public class OnlineClientGump : Gump
   {
      private NetState m_State;

      private void Resend( Mobile to, RelayInfo info )
      {
         TextRelay te = info.GetTextEntry( 0 );

         to.SendGump( new OnlineClientGump( to, m_State, te == null ? "" : te.Text ) );
      }

      public override void OnResponse( NetState state, RelayInfo info )
      {
         if ( m_State == null )
            return;

         Mobile focus = m_State.Mobile;
         Mobile from = state.Mobile;

         if ( focus == null )
         {
            from.SendMessage( "That character is no longer online." );
            return;
         }
         else if ( focus.Deleted )
         {
            from.SendMessage( "That character no longer exists." );
            return;
         }
         else if ( from != focus && focus.Hidden && from.AccessLevel < focus.AccessLevel )
         {
            from.SendMessage( "That character is no longer visible." );
            return;
         }

         switch ( info.ButtonID )
         {
            case 1: // Tell
            {
               TextRelay text = info.GetTextEntry( 0 );

               if ( text != null )
               {
                  focus.SendMessage( 0x482, "{0} tells you:", from.Name );
                  focus.SendMessage( 0x482, text.Text );
               }

               from.SendGump( new OnlineClientGump( from, m_State ) );

               break;
            }
         }
      }

      public OnlineClientGump( Mobile from, NetState state ) : this( from, state, "" )
      {
      }

      public OnlineClientGump( Mobile from, NetState state, string initialText ) : base( 30, 20 )
      {
         if ( state == null )
            return;

         m_State = state;

         AddPage( 0 );

         AddBackground( 0, 0, 300, 100, 5054 );
         AddBackground( 8, 8, 284, 84, 3000 );

         int line = 0;

         Account a = state.Account as Account;
         Mobile m = state.Mobile;

         if ( m != null )
         {
            AddButton( 12, 38 + (line * 22), 0xFAB, 0xFAD, 1, GumpButtonType.Reply, 0 );
            AddImageTiled( 47, 38 + (line * 22), 234, 22, 0x52 );
            AddImageTiled( 48, 38 + (line * 22) + 1, 232, 20, 0xBBC );
            AddTextEntry( 48, 38 + (line++ * 22) + 1, 232, 20, 0, 0, initialText );
         }

      }
   }
}