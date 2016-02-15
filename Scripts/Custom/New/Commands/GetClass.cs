using System;
using System.Reflection;
using System.Collections;
using Server;
using Server.Targeting;
using Server.Items;
using Server.Gumps;
using CPA = Server.CommandPropertyAttribute;

namespace Server.Scripts.Commands
{
	public class GetClass
	{
		public static void Initialize()
		{
			Server.Commands.Register( "GetClass", AccessLevel.Counselor, new CommandEventHandler( GetClass_OnCommand ) );
		}

        private class GetClassTarget : Target
		{
			public GetClassTarget( ) : base(-1, true, TargetFlags.None) { }
			protected override void OnTarget( Mobile from, object o )
			{
				if (o is Item || o is Mobile)
                    from.SendMessage( "Item/Mobile Class: {0}", o.GetType().ToString() );
            }
		}

		[Usage( "GetClass" )]
		[Description( "Displays the class name of the targeted item or mobile." )]
        private static void GetClass_OnCommand(CommandEventArgs e)
		{
				e.Mobile.Target = new GetClassTarget();
		}
	}
}