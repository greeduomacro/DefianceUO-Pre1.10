using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;

namespace Server.Misc
{
  public class BondInfoCommand
	{
		public static void Initialize()
		{
	      Commands.Register( "BondInfo", AccessLevel.Player, new CommandEventHandler( BondInfo_OnCommand ) );
		}

		[Usage( "Bondinfo" )]
		[Description( "Reveals the targeted pet's remaining bondingtime." )]
		public static void BondInfo_OnCommand( CommandEventArgs e )
		{
			Mobile from = e.Mobile;

			e.Mobile.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( BondInfo_OnTarget ) );
			e.Mobile.SendMessage("Target the pet you wish to know the bonding timer of");
		}


		public static void BondInfo_OnTarget( Mobile from, object targeted )
		{
			if ( targeted is BaseCreature )
			 {
				 BaseCreature targ = (BaseCreature)targeted;
						if ( targ.ControlMaster == from )
						{
                            //Al: Fix for displaying "hasnt started to bond yet" on bonded pets.
                            if (targ.IsBonded)
                            {
                                from.SendMessage("Your pet is already bonded.");
                            }
                            else if (targ.BondingBegin == DateTime.MinValue)
                            {
                                from.SendMessage("Your pet hasn't started to bond yet, please feed it and try again.");
                                if (targ.MinTameSkill > 29.1 && targ.MinTameSkill > from.Skills.AnimalTaming.Base)
                                    from.SendMessage(0x2b, "This animal requires {0} animal taming to bond it, your skill is at {1}", targ.MinTameSkill.ToString(), from.Skills.AnimalTaming.Base.ToString());
                            }
                            else
                            {
                                string bondinfo = string.Format("The pet started bonding with you on {0}. It takes 7 days to bond.", targ.BondingBegin );
                                from.SendMessage(bondinfo);
                            }
			            }
			            else
				        {
                              from.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( BondInfo_OnTarget ) );
                              from.SendMessage( "That is not your pet!" );
                        }

		}
 		else
			{
				from.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( BondInfo_OnTarget ) );
			 	from.SendMessage("That is not a pet!" );
		 }
	}
 }
}