    //////////////////////////////////
   //			           //
  //      Scripted by Raelis      //
 //		             	 //
//////////////////////////////////
using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;


namespace Server.Mobiles
{
	public class KPSystem
	{

		public static void Initialize()
		{
			Server.Commands.Register( "KP", AccessLevel.Player, new CommandEventHandler( KP_OnCommand ) );
		}

		public static void KP_OnCommand( CommandEventArgs args )
		{
			Mobile m = args.Mobile;
			PlayerMobile from = m as PlayerMobile;

			if( from != null )
			{
				from.SendMessage ( "Target a dragon that you own to get their kill point amount." );
				m.Target = new InternalTarget();
			}
		}

		private class InternalTarget : Target
		{
			public InternalTarget() : base( 8, false, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object obj )
			{
				if ( !from.Alive )
				{
					from.SendMessage( "You may not do that while dead." );
				}
                           	else if ( obj is EvolutionDragon && obj is BaseCreature )
                           	{
					BaseCreature bc = (BaseCreature)obj;
					EvolutionDragon ed = (EvolutionDragon)obj;

					if ( ed.Controlled == true && ed.ControlMaster == from )
					{
						ed.PublicOverheadMessage( MessageType.Regular, ed.SpeechHue, true, ed.Name +" has "+ ed.KP +" kill points.", false );
					}
					else
					{
						from.SendMessage( "You do not control this dragon!" );
					}
                           	}
                           	else
                           	{
                              		from.SendMessage( "That is not a dragon!" );
			   	}
			}
		}
	}
}