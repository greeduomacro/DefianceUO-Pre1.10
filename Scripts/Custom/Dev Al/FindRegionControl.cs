//Al@2006-06-20
using System;
using Server.Regions;
using Server.Items;

namespace Server.Scripts.Commands
{
    public class FindRegionControl
    {
        public static void Initialize()
        {
            Server.Commands.Register("FindRegionControl", AccessLevel.GameMaster, new CommandEventHandler(FindRegionControl_OnCommand));
            Server.Commands.Register("FindRC", AccessLevel.GameMaster, new CommandEventHandler(FindRegionControl_OnCommand));
            Server.Commands.Register("PullRegionControl", AccessLevel.Seer, new CommandEventHandler(PullRegionControl_OnCommand));
        }

        [Usage("FindRegionControl")]
        [Aliases("FindRC")]
        [Description("Finds the region controller for the current region.")]
        public static void FindRegionControl_OnCommand(CommandEventArgs e)
        {
            if (
                e.Mobile.Region is CustomRegion &&
                ((CustomRegion)e.Mobile.Region).Controller != null &&
                !((CustomRegion)e.Mobile.Region).Controller.Deleted
            )
            {
                e.Mobile.SetLocation(((CustomRegion)e.Mobile.Region).Controller.Location, true);
            }
            else
            {
                e.Mobile.SendMessage("No region controller found.");
            }
        }
        [Usage("PullRegionControl")]
        [Description("Pulls the region controller for the current region.")]
        public static void PullRegionControl_OnCommand(CommandEventArgs e)
        {
            if (
                e.Mobile.Region is CustomRegion &&
                ((CustomRegion)e.Mobile.Region).Controller != null &&
                !((CustomRegion)e.Mobile.Region).Controller.Deleted
            )
            {
                ((CustomRegion)e.Mobile.Region).Controller.Location = e.Mobile.Location;
                ((CustomRegion)e.Mobile.Region).Controller.Map = e.Mobile.Map;
                e.Mobile.SendMessage("The region controller has been pulled.");
            }
            else
            {
                e.Mobile.SendMessage("No region controller found.");
            }
        }
    }
}