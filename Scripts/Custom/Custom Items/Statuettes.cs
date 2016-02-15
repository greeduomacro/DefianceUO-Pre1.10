// Dont know who did it!
using System;
using System.Collections;
using Server;
using Server.Items;
using Server.ContextMenus;

namespace Server.Items
{
   public class VetStatuette : Item, IDyableS
   {
      private bool m_Squelch;
      private StatutteType m_Type;

      [CommandProperty( AccessLevel.GameMaster )]
      public bool SoundSquelch
      {
         get { return m_Squelch; }
         set { m_Squelch = value; InvalidateProperties();}
      }

      [CommandProperty( AccessLevel.GameMaster )]
      public StatutteType Type
      {
         get{ return m_Type; }
         set{ m_Type = value; InvalidateProperties(); Rebuild();}
      }

      [Constructable]
      public VetStatuette() : this( StatutteType.DragonStat )
      {
      }

      [Constructable]
      public VetStatuette( StatutteType type )
      {
         LootType = LootType.Blessed;

         m_Type = type;
         Rebuild();
      }

      public VetStatuette( Serial serial ) : base( serial )
      {
      }

      public void Rebuild()
      {
         this.ItemID = StatuetteInfo.GetInfo( m_Type ).ModelID;
      }

      public override int LabelNumber{ get{ return StatuetteInfo.GetInfo( m_Type ).LabelNumber; } } //label # base on info

      public override void GetProperties( ObjectPropertyList list )
      {
         base.GetProperties( list );

         if ( m_Squelch )
            list.Add( 3005118 );
         else
            list.Add( 3005117 );
      }

      public override bool HandlesOnMovement{ get{ return true; } }

      public override void GetContextMenuEntries( Mobile from, ArrayList list )
      {
         base.GetContextMenuEntries( from, list );

         if ( from.Alive )
         {
            if ( m_Squelch )
               list.Add( new SoundSwitchOff( from, this ) );
            else
               list.Add( new SoundSwitchOn( from, this ) );
         }
      }

      public override void OnDoubleClick( Mobile from )
      {
         if ( from.InRange( this, 2 ) )
            SoundOnOff( from );
      }

      public override void OnMovement( Mobile m, Point3D oldLocation )
      {
         if ( this.SoundSquelch )
            return;

         //Don't trigger statuettes by hidden staff members
         if (m.AccessLevel >= AccessLevel.Counselor && m.Hidden) return;

         if ( m.InRange( this, 2 ) )
         {
            StatuetteInfo info = StatuetteInfo.GetInfo( m_Type );
            int random = Utility.Random( info.SoundID.Length );

            Effects.PlaySound( this, this.Map, info.SoundID[random] );
         }
      }

      public void SoundOnOff( Mobile m )
      {
         if ( SoundSquelch )
         {
            SoundSquelch = false;
            m.SendMessage( "Sound ON" );
         }
         else
         {
            SoundSquelch = true;
            m.SendMessage( "Sound OFF" );
         }
      }

      public override void Serialize( GenericWriter writer )
      {
         base.Serialize( writer );
         writer.Write( (int) 0 ); // version

         writer.Write( (bool)m_Squelch );
         writer.Write( (int) m_Type );
      }

      public override void Deserialize( GenericReader reader )
      {
         base.Deserialize( reader );
         int version = reader.ReadInt();

         m_Squelch = reader.ReadBool();
         m_Type = (StatutteType)reader.ReadInt();
      }
      // Added for Dye Tub
   	  public virtual bool DyeS( Mobile from, StatDyeTub sender )
      {
		 if ( Deleted )
			return false;
		 else if ( RootParent is Mobile && from != RootParent )
		 	return false;

		 Hue = sender.DyedHue;

		 return true;
       }
   }


   public class SoundSwitchOn : ContextMenuEntry
   {
      private Mobile m_From;
      private VetStatuette m_Item;

      public SoundSwitchOn( Mobile m, VetStatuette item ) : base( 410, 3 )
      {
         m_From = m;
         m_Item = item;
      }

      public override void OnClick()
      {
         m_Item.SoundOnOff( m_From );
      }
   }

   public class SoundSwitchOff : ContextMenuEntry
   {
      private Mobile m_From;
      private VetStatuette m_Item;

      public SoundSwitchOff( Mobile m, VetStatuette item ) : base( 411, 3 )
      {
         m_From = m;
         m_Item = item;
      }

      public override void OnClick()
      {
         m_Item.SoundOnOff( m_From );
      }
   }

   public enum StatutteType
   {
      CrocodileStat,
      DaemonStat,
      DragonStat,
      EarthElementalStat,
      EttinStat,
      GargoyleStat,
      GorillaStat,
      LichStat,
      LizardmanStat,
      OgreStat,
      OrcStat,
      RatmanStat,
      SkeletonStat,
      TrollStat,
      CowStat,
      ZombieStat,
      LlamaStat,
      OphidianStat,
      ReaperStat,
      MongbatStat,
      GazerStat,
      FireElementalStat,
      WolfStat
   }

   public class StatuetteInfo
   {
      private int m_ModelID;
      private int m_LabelNumber;
      private int[] m_SoundID;

      public int ModelID{ get{ return m_ModelID; } }
      public int LabelNumber{ get{ return m_LabelNumber; } }
      public int[] SoundID{ get{ return m_SoundID; } }

      public StatuetteInfo( int id, int labelNumber, int sound, int count )
      {
         m_ModelID = id;
         m_LabelNumber = labelNumber;

         m_SoundID = new int[count];
         for ( int i = 0; i < count; ++i )
            m_SoundID[i] = sound + i;
      }

      public StatuetteInfo( int id, int labelNumber, int sound, int count, int sound2, int count2 )
      {
         int total = count + count2;

         m_ModelID = id;
         m_LabelNumber = labelNumber;

         m_SoundID = new int[total];
         for ( int i = 0; i < count; ++i )
            m_SoundID[i] = sound + i;
         for ( int j = count; j < total; ++j )
            m_SoundID[j] = sound2 + (j - count);
      }

//      public StatuetteInfo( int id, int labelNumber, params int[] sounds )
//      {
//         m_ModelID = id;
//         m_LabelNumber = labelNumber;
//         m_SoundID = sounds;
//      }

      private static StatuetteInfo[] m_Info = new StatuetteInfo[]
         {
            /* Crocodile Statuette                     */ new StatuetteInfo( 0x20DA, 1006024, 0x5A, 5, 0x294, 5 ),
            /* Daemon Statuette                        */ new StatuetteInfo( 0x20D3, 1006025, 0x165, 5, 0x2B8, 5 ),
            /* Dragon Statuette                        */ new StatuetteInfo( 0x20D6, 1006026, 0x16A, 5, 0x2C0, 22 ),
            /* Earth Elemental Statuette               */ new StatuetteInfo( 0x20D7, 1006027, 0x10E, 5 ),
            /* Ettin Statuette                         */ new StatuetteInfo( 0x20D8, 1006028, 0x16F, 5,0x2FB, 5 ),
            /* Gargoyle Statuette                      */ new StatuetteInfo( 0x20D9, 1006029, 0x174, 5 ),
            /* Gorilla Statuette                       */ new StatuetteInfo( 0x20F5, 1006030, 0x9E, 5 ),
            /* Lich Statuette                          */ new StatuetteInfo( 0x20F8, 1006031, 0x19C, 5, 0x3E9, 5 ),
            /* Lizardman Statuette                     */ new StatuetteInfo( 0x20DE, 1006032, 0x1A1, 5 ),
            /* Ogre Statuette                          */ new StatuetteInfo( 0x20DF, 1006033, 0x1AB, 5 ),
            /* Orc Statuette                           */ new StatuetteInfo( 0x20E0, 1006034, 0x1B0, 5, 0x458, 10 ),
            /* Ratman Statuette                        */ new StatuetteInfo( 0x20E3, 1006035, 0x1B5, 5 ),
            /* Skeleton Statuette                      */ new StatuetteInfo( 0x20E7, 1006036, 0x1C3, 5, 0x48D, 5 ),
            /* Troll Statuette                         */ new StatuetteInfo( 0x20E9, 1006037, 0x1CD, 5 ),
            /* Cow Statuette                           */ new StatuetteInfo( 0x2103, 1006038, 0x78, 5 ),
            /* Zombie Statuette                        */ new StatuetteInfo( 0x20EC, 1006039, 0x1D7, 5 ),
            /* Llama Statuette                         */ new StatuetteInfo( 0x20F6, 1006040, 0xB7, 3, 0x3F3, 5 ),
            /* Ophidian Statuette                      */ new StatuetteInfo( 0x2133, 1049742, 0x27A, 5 ),
            /* Reaper Statuette                        */ new StatuetteInfo( 0x20FA, 1049743, 0x1BA, 5 ),
            /* Mongbat Statuette                       */ new StatuetteInfo( 0x20F9, 1049744, 0x1A6, 5 ),
            /* Gazer Statuette                         */ new StatuetteInfo( 0x20F4, 1049768, 0x179, 5, 0x36B, 5 ),
            /* Fire Elemental Statuette                */ new StatuetteInfo( 0x20F3, 1049769, 0x111, 5 ),
            /* Wolf Statuette                          */ new StatuetteInfo( 0x20EA, 1049770, 0xE5, 5 )
         };

      public static StatuetteInfo GetInfo( StatutteType type )
      {
         int v = (int)type;

         if ( v < 0 || v >= m_Info.Length )
            v = 0;

         return m_Info[v];
      }
   }
}