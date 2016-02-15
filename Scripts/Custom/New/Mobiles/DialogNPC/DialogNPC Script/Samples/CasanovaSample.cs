using System;
using System.Collections;

using Server;

namespace Arya.DialogEditor
{
	/// <summary>
	/// Holds functions for the Casanova DialogNPC sample.
	/// Delete this file if you don't want to use the Casanova NPC
	/// </summary>
	public class CasanovaSample
	{
		private static ArrayList m_Received = new ArrayList();

		public static bool CanReceiveRose( Mobile m, DialogNPC npc )
		{
			if ( ! m.Female )
			{
				npc.SayTo( m, "What do you want from? I don't like your kind!" );
				return false;
			}

			return ! m_Received.Contains( m );
		}

		public static void GiveRoseTo( Mobile m, DialogNPC npc )
		{
			Item rose = new Server.Items.PottedPlant1();
			rose.Hue = 37;
			rose.Name = "Casanova's Rose";

			if ( m.AddToBackpack( rose ) )
			{
				npc.SayTo( m, "Thank you my dear... take this rose as a sign of my love for you..." );
				m_Received.Add( m );
			}
			else
			{
				npc.SayTo( m, "I wish I could grant you with a rose, but I don't think you could carry its weight..." );
			}
		}
	}
}