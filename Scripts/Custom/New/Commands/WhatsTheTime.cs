//Al@2006-05-09
using System;

namespace Server.Scripts.Commands
{
    public class Time
    {
        public static void Initialize()
        {
            Server.Commands.Register("ServerTime", AccessLevel.Player, new CommandEventHandler(ServerTime_OnCommand));
        }

        [Usage("ServerTime")]
        [Description("Displays the current server time.")]
        public static void ServerTime_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendMessage("Current server time: " + DateTime.Now.ToString("T"));
        }
    }
}