using System;
using Server;
using Server.Mobiles;
using Server.Engines.IdolSystem;

namespace Server.Items
{
	public class DarkMasterGate : Moongate
	{
		public DarkMasterGate( Point3D loc, Map map, Point3D targLoc, Map targMap ) : base( targLoc, targMap )
		{
			Dispellable = false;
			ItemID = 0x1FD4;
			Hue = 0x497;
			Light = LightType.Circle300;
			Name = "To the Dark Master's Lair";

			MoveToWorld( loc, map );

			new InternalTimer( this ).Start();
		}

		private class InternalTimer : Timer
        	{
            		private DarkMasterGate m_Gate;

            		public InternalTimer( DarkMasterGate gate ) : base( TimeSpan.FromMinutes(60.0) )
            		{
               			Priority = TimerPriority.OneMinute;
               			m_Gate = gate;
            		}

            		protected override void OnTick()
            		{
               			if ( m_Gate == null||m_Gate.Deleted )
                		{
                    			Stop();
                		}

                		else
                		{
                   			m_Gate.Delete();
                		}
            		}
        	}

		public DarkMasterGate( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}