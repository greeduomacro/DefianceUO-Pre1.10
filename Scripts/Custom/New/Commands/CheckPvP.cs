using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Scripts.Commands
{
    public class CheckPvP
    {
        public static void Initialize()
        {
            Server.Commands.Register("CheckPvP", AccessLevel.GameMaster, new CommandEventHandler(CheckPvP_OnCommand));
        }

        //[Usage("CheckPvP")]
        //[Description("Displays information about currently ongoing PvP.")]
        public static void CheckPvP_OnCommand(CommandEventArgs e)
        {
            DateTime startTime = DateTime.Now;
            int pvpers = 0;
            int maxAggressors = 0;
            foreach (NetState ns in Server.Network.NetState.Instances)
            {
                PlayerMobile pm = ns.Mobile as PlayerMobile;
                int playersAggressed = 0;
                if (pm != null && pm.Aggressors != null)
                    foreach (Mobile m in pm.Aggressors)
                        if (m is PlayerMobile)
                            playersAggressed++;

                if (playersAggressed > 0) pvpers++;
                maxAggressors = Math.Max(maxAggressors, playersAggressed);
            }

            e.Mobile.SendMessage("Check took {0:F1} seconds.", (DateTime.Now - startTime).TotalSeconds);
            e.Mobile.SendMessage("Current PvP statistics\nPvPers: {0}\nMax. aggressors: {1}", pvpers, maxAggressors);
        }
    }
}