using System;
using System.Reflection;
using Server.Items;
using Server.Targeting;
using Server.Misc;
namespace Server.Scripts.Commands
{
	public class AddBountyCommand
	{
		public static void Initialize()
		{
			Server.Commands.Register( "AddBounty", AccessLevel.Administrator, new CommandEventHandler( AddBountyCommand_OnCommand ) );
		}

		[Usage( "AddBounty [amount]" )]
		[Description( "Add targetted players bounty with the amount" )]
		private static void AddBountyCommand_OnCommand( CommandEventArgs e )
		{
			int amount = 1;
			if ( e.Length >= 1 )
				amount = e.GetInt32( 0 );
			e.Mobile.Target = new InternalTarget( amount > 0 ? amount : 1 );
			e.Mobile.SendMessage( "Who should have a bounty on his head?" );
		}

		private class InternalTarget : Target
		{
			private int m_Amount;

			public InternalTarget( int amount ) : base( 15, false, TargetFlags.None )
			{
				m_Amount = amount;
			}

			protected override void OnTarget( Mobile from, object targ )
			{
				BountyTable.Add((Mobile)targ,m_Amount);
			}
		}
	}
}