using System;
using System.IO;
using System.Collections;
using System.Reflection;
using Server;
using Server.Targeting;
using Server.Targets;
using Server.Mobiles;
using Server.Items;
using Server.Misc;

namespace Server.Scripts.Commands
{
   public class CustomCmdHandlers
   {
      public static void Initialize()
      {
         Register( "SayThis", AccessLevel.GameMaster, new CommandEventHandler( SayThis_OnCommand ) );

         Register( "GM-me", AccessLevel.GameMaster, new CommandEventHandler( GM_OnCommand ) );
      }

      public static void Register( string command, AccessLevel access, CommandEventHandler handler )
      {
         Server.Commands.Register( command, access, handler );
      }

      [Usage( "GM-me" )]
      [Description( "Helps senior staff members set their body to GM style." )]
      public static void GM_OnCommand( CommandEventArgs e )
      {
         e.Mobile.Target = new GMmeTarget();
      }

      private class GMmeTarget : Target
      {
         public GMmeTarget() : base( -1, false, TargetFlags.None )
         {
         }

         protected override void OnTarget( Mobile from, object targeted )
         {
            if ( targeted is Mobile )
            {
               Mobile targ = (Mobile)targeted;
               if ( from != targ )
                  from.SendMessage( "You may only set your own body to GM style." );

               else
               {
                  CommandLogging.WriteLine( from, "{0} {1} is assuming a GM body", from.AccessLevel, CommandLogging.Format( from ) );

                  DisRobe( from, Layer.Shoes );
                  DisRobe( from, Layer.Pants );
                  DisRobe( from, Layer.Shirt );
                  DisRobe( from, Layer.Helm );
                  DisRobe( from, Layer.Gloves );
                  DisRobe( from, Layer.Neck );
                  DisRobe( from, Layer.Hair );
                  DisRobe( from, Layer.Waist );
                  DisRobe( from, Layer.InnerTorso );
                  //DisRobe( from, Layer.FacialHair );
                  DisRobe( from, Layer.MiddleTorso );
                  DisRobe( from, Layer.Arms );
                  DisRobe( from, Layer.Cloak );
                  DisRobe( from, Layer.OuterTorso );
                  DisRobe( from, Layer.OuterLegs );

                  from.Body = 0x3DB;

                  for ( int i = 0; i < targ.Skills.Length; ++i )
                     targ.Skills[i].Base = 120;
               }
            }
         }

         private static void DisRobe( Mobile m_from, Layer layer )
         {
            if ( m_from.FindItemOnLayer( layer ) != null )
            {
               Item item = m_from.FindItemOnLayer( layer );
               m_from.PlaceInBackpack( item ); // Place in a bag first?
            }
         }
      }

      [Usage( "SayThis <text>" )]
      [Description( "Forces Target to Say <text>." )]
      public static void SayThis_OnCommand( CommandEventArgs e )
      {
         string toSay = e.ArgString.Trim();

         if ( toSay.Length > 0 )
            e.Mobile.Target = new SayThisTarget( toSay );
         else
            e.Mobile.SendMessage( "Format: SayThis \"<text>\"" );
      }

      private class SayThisTarget : Target
      {
         private string m_toSay;

         public SayThisTarget( string s ) : base( -1, false, TargetFlags.None )
         {
            m_toSay = s;
         }

         protected override void OnTarget( Mobile from, object targeted )
         {
            if ( targeted is Mobile )
            {
               Mobile targ = (Mobile)targeted;

               // if ( !targ.Player ) // use to limit to Non-Players
               if ( from != targ && from.AccessLevel > targ.AccessLevel )
               {
                  CommandLogging.WriteLine( from, "{0} {1} forcing speech on {2}", from.AccessLevel, CommandLogging.Format( from ), CommandLogging.Format( targ ) );
                  targ.Say( m_toSay );
               }
            }
         }
      }

      [Usage( "Clone" )]
      [Description( "Assume the form of another Player or Creature." )]
      public static void Clone_OnCommand( CommandEventArgs e )
      {
         e.Mobile.Target = new CloneTarget();
      }

      private class CloneTarget : Target
      {
         public CloneTarget() : base( -1, false, TargetFlags.None )
         {
         }

         protected override void OnTarget( Mobile from, object targeted )
         {
            if ( targeted is Mobile )
            {
               Mobile targ = (Mobile)targeted;

               // if ( !targ.Player ) // use to limit to Non-Players
               if ( from != targ && from.AccessLevel > targ.AccessLevel )
               {
                  CommandLogging.WriteLine( from, "{0} {1} is cloning {2}", from.AccessLevel, CommandLogging.Format( from ), CommandLogging.Format( targ ) );

                  from.Hidden = true;

                  from.Dex = targ.Dex;
                  from.Int = targ.Int;
                  from.Str = targ.Str;
                  from.Fame = targ.Fame;
                  from.Karma = targ.Karma;
                  from.NameHue = targ.NameHue;
                  from.SpeechHue = targ.SpeechHue;
                  //from.Criminal = !targ.Criminal;

                  from.Name = targ.Name;
                  from.Title = targ.Title;
                  from.Female = targ.Female;
                  from.Body = targ.Body;
                  from.Hue = targ.Hue;

                  from.Hits = from.HitsMax;
                  from.Mana = from.ManaMax;
                  from.Stam = from.StamMax;

                  from.Location = targ.Location;
                  from.Direction = targ.Direction;

                  if( !targ.Player )
                     from.BodyMod = targ.Body;
                  else
                     from.BodyMod = 0;

                  for (int i=0; i<from.Skills.Length; i++)
                     from.Skills[i].Base = targ.Skills[i].Base;

                  // This code block deletes everything GM has equiped.
                  ArrayList m_items = new ArrayList( from.Items );
                  for (int i=0; i<m_items.Count; i++)
                  {
                     Item item = (Item)m_items[i];
                     if((( item.Parent == from ) && ( item != from.Backpack )))
                        item.Delete();
                        // Alternate Option: from.PlaceInBackpack( item );
                  }

                  // This code block deletes everything in GM's pack if
                  // subject is a Player. (This block is optional.)
                  if( targ.Player )
                  {
                     Container m_pack = from.Backpack;
                     if( m_pack != null)
                     {
                        ArrayList p_items = new ArrayList( m_pack.Items );
                        for (int i=0; i<p_items.Count; i++)
                        {
                           ((Item)p_items[i]).Delete();
                        }
                     }
                  }

                  // This code block duplicates all equiped items
                  ArrayList items = new ArrayList( targ.Items );
                  for (int i=0; i<items.Count; i++)
                  {
                     Item item = (Item)items[i]; //my favorite line of code, ever.
                     if((( item != null ) && ( item.Parent == targ ) && ( item != targ.Backpack )))
                     {
                        Type type = item.GetType();
                        Item newitem = Loot.Construct( type );
                        CopyProperties( newitem, item );
                        from.AddItem( newitem );
                     }
                  }

                  // This code block duplicates all pack items if subject is a Player
                  if( targ.Player )
                  {
                     Container pack = targ.Backpack;
                     if( pack != null )
                     {
                        ArrayList t_items = new ArrayList( pack.Items );
                        for (int i=0; i<t_items.Count; i++)
                        {
                           Item item = (Item)t_items[i];
                           if(( item != null ) && ( item.IsChildOf( pack )))
                           {
                              Type type = item.GetType();
                              Item newitem = Loot.Construct( type );
                              CopyProperties( newitem, item );
                              from.PlaceInBackpack( newitem );
                           }
                        }
                     }
                  }
               }
            }
         }

         //Duplicates props between two items of same type
         private static void CopyProperties ( Item dest, Item src )
         {
            PropertyInfo[] props = src.GetType().GetProperties();

            for ( int i = 0; i < props.Length; i++ )
            {
               try
               {
                  if ( props[i].CanRead && props[i].CanWrite )
                  {
                     props[i].SetValue( dest, props[i].GetValue( src, null ), null );
                  }
               }
               catch
               {
               }
            }
         }
      }
   }
}