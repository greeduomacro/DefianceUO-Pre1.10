using System;
using System.Collections;

using Server;

namespace Arya.DialogEditor
{
	public class TravelHelper
	{
		public static void GoBritain( Mobile m, DialogNPC npc )
		{

			m.Location = new Point3D( 1422, 1697, 0 );
		}

		public static void GoTrinsic( Mobile m, DialogNPC npc )
		{
			m.Location = new Point3D( 1820, 2822, 0 );
		}

		public static void TakeMoney( Mobile m, DialogNPC npc )
		{
			if ( !Server.Mobiles.Banker.Withdraw( m, 500 ) )
			{
				npc.SayTo( m, "Trying to cheat me of 500 gold huh? DIE!" );
				m.Kill();
			}
		}
	}
}