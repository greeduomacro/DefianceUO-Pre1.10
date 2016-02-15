using System;
using System.Collections;
using Server.Items.Staff;
using Server.Targeting;
using Server.Accounting;

namespace Server.Scripts.Commands
{
    class StaffAbuse
    {
        public static void Initialize()
        {
            Server.Commands.Register("StaffAbuse", AccessLevel.Seer, new CommandEventHandler(StaffAbuse_OnCommand));
        }

        public static void StaffAbuse_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile == null)
                return;

            if (e.Mobile.AccessLevel < AccessLevel.Administrator)
            {
                Account acc = e.Mobile.Account as Account;
                if (acc.Username != "infra" && acc.Username != "troystaff")
                {
                    e.Mobile.SendMessage("You are not allowed to use this command.");
                    return;
                }
            }

            e.Mobile.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(StaffAbuse_OnTarget));
            e.Mobile.SendMessage("Target the staffmember that abused his power. DO NOT USE THIS COMMAND FOR ANY FUN!");
        }

        public static void StaffAbuse_OnTarget(Mobile from, object obj)
        {
            if (from == null || from.Deleted || !(obj is Mobile))
                return;

            Mobile abuser = (Mobile)obj as Mobile;
            if (abuser != null)
            {
                if (abuser.AccessLevel == AccessLevel.Player)
                {
                    from.SendMessage("The targeted person has accesslevel player. Request canceled.");
                    return;
                }
                else if (abuser.AccessLevel == AccessLevel.Administrator)
                {
                    from.SendMessage("This command does not work on administrators. Request canceled.");
                    return;
                }

                if (abuser.Backpack != null)
                {
                    while (abuser.Backpack.ConsumeTotal(typeof(StaffOrb), 1, true)) { }
                }
                abuser.AccessLevel = AccessLevel.Player;
                abuser.Frozen = true;
                abuser.MoveToWorld(new Point3D(5275, 1172, 0), Map.Felucca); // move abuser to felucca jail
                Server.Scripts.Commands.CommandHandlers.BroadcastMessage(AccessLevel.Seer, 38, String.Format("Staffmessage: {0} was blamed by {1} for staffpower abusing.", abuser.Name, from.Name));
            }
        }
    }
}