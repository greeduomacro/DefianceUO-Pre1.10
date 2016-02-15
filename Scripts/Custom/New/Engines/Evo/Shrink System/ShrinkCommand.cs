#region AuthorHeader
//
//	EvoSystem version 1.6, by Xanthos
//
//
#endregion AuthorHeader
using System;
using Server;
using Server.Mobiles;

namespace Xanthos.Evo
{
	public class ShrinkCommands
	{
		private static bool m_LockDown; // TODO: need to persist this.

		public static void Initialize()
		{
			Server.Commands.Register( "Shrink", AccessLevel.GameMaster, new CommandEventHandler( Shrink_OnCommand ) );
			Server.Commands.Register( "ShrinkLockDown", AccessLevel.Administrator, new CommandEventHandler( ShrinkLockDown_OnCommand ) );
			Server.Commands.Register( "ShrinkRelease", AccessLevel.Administrator, new CommandEventHandler( ShrinkRelease_OnCommand ) );
		}

		public static bool LockDown
		{
			get { return m_LockDown; }
		}

		[Usage( "Shrink" )]
		[Description( "Shrinks a creature." )]
		private static void Shrink_OnCommand( CommandEventArgs e )
		{
			PlayerMobile from = e.Mobile as PlayerMobile;

			if ( null != from )
				from.Target = new Xanthos.Evo.ShrinkTarget( from, null );
		}

		[Usage( "ShrinkLockDown" )]
		[Description( "Disables all shrinkitems in the world." )]
		private static void ShrinkLockDown_OnCommand( CommandEventArgs e )
		{
			PlayerMobile from = e.Mobile as PlayerMobile;

			if ( null != from )
				SetLockDown( from, true );
		}

		[Usage( "ShrinkRelease" )]
		[Description( "Re-enables all disabled shrink items in the world." )]
		private static void ShrinkRelease_OnCommand( CommandEventArgs e )
		{
			PlayerMobile from = e.Mobile as PlayerMobile;

			if ( null != from )
				SetLockDown( from, false );
		}

		static private void SetLockDown( Mobile from, bool lockDown )
		{
			if ( m_LockDown = lockDown )
			{
				World.Broadcast( 0x35, true, "A server wide shrinkitem lockout has initiated." );
				World.Broadcast( 0x35, true, "All shrunken pets have will remain shruken until further notice." );
			}
			else
			{
				World.Broadcast( 0x35, true, "The server wide shrinkitem lockout has been lifted." );
				World.Broadcast( 0x35, true, "You may once again unshrink shrunken pets." );
			}
		}
	}
}