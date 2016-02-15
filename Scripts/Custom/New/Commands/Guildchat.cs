using System;
using System.Text;
using System.Collections;
using Server;
using Server.Network;
using Server.Guilds;
using Server.Mobiles;

namespace Server.Scripts.Commands
{
   public class G
      {
      private static int usercount;

      public static void Initialize()
      {
         Server.Commands.Register( "G", AccessLevel.Player, new CommandEventHandler( G_OnCommand ) );
            }

      [Usage( "G [<text>|list]" )]
      [Description( "Broadcasts a message to all online members of your guild." )]
      private static void G_OnCommand( CommandEventArgs e )
      {
         switch( e.ArgString.ToLower() )
         {
            case "list":
               List ( e.Mobile );
               break;
            default:
               Msg ( e );
               break;
         }
      }

      private static void List( Mobile g )
      {
         usercount = 0;
         Guild GuildC = g.Guild as Guild;
         if ( GuildC == null )
         {
            g.SendMessage( "You are not in a guild!" );
         }
         else
         {
            foreach ( NetState state in NetState.Instances )
            {
               Mobile m = state.Mobile;
               if ( m != null && GuildC.IsMember( m ) )
               {
                  usercount++;
               }
            }
            if (usercount == 1)
            {
               g.SendMessage( "There is 1 member of your guild online." );
            }
            else
            {
               g.SendMessage( "There are {0} members of your guild online.", usercount );
            }
            g.SendMessage ("Online list:" );
            foreach ( NetState state in NetState.Instances )
            {
               Mobile m = state.Mobile;
               if ( m != null && GuildC.IsMember( m ) )
               {
                  string region = m.Region.ToString();
                  if (region == "")
                  {
                     region = "Britannia";
                  }
                  g.SendMessage( "{0} ({1})", m.Name, region );
               }
            }

         }
      }

      private static void Msg( CommandEventArgs e  )
      {
         Mobile from = e.Mobile;

         Guild GuildC = from.Guild as Guild;
         if ( GuildC == null )
         {
            from.SendMessage( "You are not in a guild!" );
         }
         else
         {
            foreach ( NetState state in NetState.Instances )
            {
               Mobile m = state.Mobile;
               if ( m != null && GuildC.IsMember( m ) )
               {
                  m.SendMessage( 0x2C, String.Format( "Guild[{0}]: {1}", from.Name, e.ArgString ) );
               }
            }
			Packet p = null;
			foreach (NetState ns in from.GetClientsInRange(8))
			{
				Mobile mob = ns.Mobile;
				if (mob != null && mob.AccessLevel >= AccessLevel.GameMaster && mob.AccessLevel > from.AccessLevel)
				{
					if (p == null)
						p = new UnicodeMessage(from.Serial, from.Body, MessageType.Regular, from.SpeechHue, 3, from.Language, from.Name, String.Format("[Guild]: {0}", e.ArgString));
					ns.Send(p);
				}
			}
         }
      }
   }
}