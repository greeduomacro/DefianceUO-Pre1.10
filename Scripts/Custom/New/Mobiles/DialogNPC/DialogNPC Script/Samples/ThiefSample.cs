using System;

using Server;
using Server.Items;

namespace Arya.DialogEditor
{
	public class ThiefHelper
	{
		public static void GiveLittle( Mobile m, DialogNPC npc )
		{
			if ( m.Backpack != null )
			{
				Gold g = m.Backpack.FindItemByType( typeof( Gold ), true ) as Gold;

				if ( g != null )
				{
					if ( g.Amount == 1 )
						g.Delete();
					else
						g.Amount -= 1;

					npc.SayTo( m, "Thank ye, thank ye, M'lord" );
					npc.SayTo( m, "*bows*" );
				}
				else
				{
					npc.SayTo( m, "*spits on your empty pockets*" );
				}
			}
		}

		public static void GiveAll( Mobile m, DialogNPC npc )
		{
			if ( m.Backpack != null )
			{
				Gold g = m.Backpack.FindItemByType( typeof( Gold ), true ) as Gold;

				if ( g != null )
				{
					g.Delete();

					npc.SayTo( m, "Thank ye, thank ye, M'lord" );
					npc.SayTo( m, "*bows*" );
				}
				else
				{
					npc.SayTo( m, "*spits on your empty pockets*" );
				}
			}
		}

		public static void Steal( Mobile m, DialogNPC npc )
		{
			if ( m.Backpack != null )
			{
				Gold g = m.Backpack.FindItemByType( typeof( Gold ), true ) as Gold;

				if ( g != null )
				{
					if ( g.Amount > 200 )
						g.Amount -= 200;
					else
						g.Delete();

					npc.SayTo( m, "You notice {0} steal some gold from you and laugh...", npc.Name );
				}
				else
				{
					npc.SayTo( m, "*spits on your empty pockets*" );
				}
			}
		}
	}
}