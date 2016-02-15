//Al@2006-06-20
using System;
using Server;
using Server.Regions;
using Server.Targeting;
using Server.Targets;
using Server.Items;

namespace Server.Scripts.Commands
{
    public class DebugCommands
    {
        public static void Initialize()
        {
            Server.Commands.Register("DumpAggression", AccessLevel.Seer, new CommandEventHandler(DumpAggression_OnCommand));
        }

        [Usage("DumpAgression")]
        [Description("Dumps a Mobile's Aggressor and Aggressed collections.")]
        public static void DumpAggression_OnCommand(CommandEventArgs e)
        {
            e.Mobile.Target = new DumpAggressionTarget();
        }

        private class DumpAggressionTarget : Target
        {
            public DumpAggressionTarget() : base(-1, false, TargetFlags.None) { }
            protected override void OnTarget(Mobile from, object o)
            {
                Mobile m = o as Mobile;
                if (m == null) return;
                from.SendMessage("Mobile: {0}", m.Name);
                from.SendMessage("Aggressed:");
                foreach (AggressorInfo ai in m.Aggressed)
                    if (ai.Defender != null) from.SendMessage(ai.Defender.Name+ (ai.CriminalAggression ? " [criminal]" : " [non criminal]"));

                from.SendMessage("Aggressors:");
                foreach (AggressorInfo ai in m.Aggressors)
                    if (ai.Attacker != null) from.SendMessage(ai.Attacker.Name + (ai.CriminalAggression ? " [criminal]" : " [non criminal]"));
            }
        }
    }
}