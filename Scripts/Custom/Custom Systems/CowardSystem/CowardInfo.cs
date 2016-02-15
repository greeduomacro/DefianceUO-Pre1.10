using System;
using System.IO;
using System.Collections;

namespace Server.Mobiles
{
   public class CowardiceInfo
   {
      private PlayerMobile m_Coward;
      private DateTime m_LastCowardice;
      private int m_Notoriety;

      private bool m_Queued;

      private static Queue m_Pool = new Queue();

      public static CowardiceInfo Create( PlayerMobile coward, int notoriety )
      {
         CowardiceInfo info;

         if ( m_Pool.Count > 0 )
         {
            info = (CowardiceInfo)m_Pool.Dequeue();

            info.m_Coward = coward;

            info.m_Notoriety = notoriety;

            info.m_Queued = false;

            info.Refresh();
         }
         else
         {
            info = new CowardiceInfo( coward, notoriety );
         }

         return info;
      }

      public void Free()
      {
         if ( m_Queued )
            return;

         m_Queued = true;
         m_Pool.Enqueue( this );
      }

      private CowardiceInfo( PlayerMobile coward, int notoriety )
      {
         m_Coward = coward;

         m_Notoriety = notoriety;

         Refresh();
      }

      private static TimeSpan m_ExpireDelay = TimeSpan.FromMinutes( 2.0 );

      public static TimeSpan ExpireDelay
      {
         get{ return m_ExpireDelay; }
         set{ m_ExpireDelay = value; }
      }

      public static void DumpAccess()
      {
         using ( StreamWriter op = new StreamWriter( "warnings.log", true ) )
         {
            op.WriteLine( "Warning: Access to queued CowardiceInfo:" );
            op.WriteLine( new System.Diagnostics.StackTrace() );
            op.WriteLine();
            op.WriteLine();
         }
      }

      public bool Expired
      {
         get
         {
            if ( m_Queued )
               DumpAccess();

            return ( m_Coward.Deleted || DateTime.Now >= (m_LastCowardice + m_ExpireDelay) );
         }
      }

      public PlayerMobile Coward
      {
         get
         {
            if ( m_Queued )
               DumpAccess();

            return m_Coward;
         }
      }

      public DateTime LastCowardice
      {
         get
         {
            if ( m_Queued )
               DumpAccess();

            return m_LastCowardice;
         }
      }

      public int Notoriety
      {
         get
         {
            if ( m_Queued )
               DumpAccess();

            return m_Notoriety;
         }
         set
         {
            if ( m_Queued )
               DumpAccess();

            m_Notoriety = value;
         }
      }

      public void Refresh()
      {
         if ( m_Queued )
            DumpAccess();

         m_LastCowardice = DateTime.Now;
      }
   }
}