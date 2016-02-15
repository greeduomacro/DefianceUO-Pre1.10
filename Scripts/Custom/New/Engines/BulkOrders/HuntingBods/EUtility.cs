using System;
using Server;
using Server.Scripts.Commands;
using Server.Engines.BulkOrders;
using Server.Mobiles;
using Server.Targeting;

namespace Server
{
    public class EUtility
    {
        public static void Initialize()
        {
            Server.Commands.Register("ECompleteBod", AccessLevel.Administrator, new CommandEventHandler(ECompleteBod_OnCommand));
        }

        // Used for non-localized HTML.
        public static string HtmlGreen = "006600";
        public static string HtmlYellow = "EFEF5A";
        public static string HtmlBlue = "0000CC";
        public static string HtmlRed = "F70839";
        public static string HtmlWhite = "FFFFFF";
        public static string HtmlBodSelected = "8080ff";
        public static string HtmlPartyGumpGreen = "B5CE6B";

        public static string Color(string text, string color)
        {
            return String.Format("<BASEFONT COLOR=#{0}>{1}</BASEFONT>", color, text);
        }

        [Usage("")]
        [Description("")]
        private static void ECompleteBod_OnCommand(CommandEventArgs e)
        {
            e.Mobile.Target = new ECompleteBodTarget();
        }

        private class ECompleteBodTarget : Target
        {
            public ECompleteBodTarget()
                : base(15, false, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object targ)
            {
                if (targ is SmallBOD)
                {
                    ((SmallBOD)targ).AmountCur = ((SmallBOD)targ).AmountMax;
                }
                else if (targ is LargeBOD)
                {
                    for (int i = 0; i < ((LargeBOD)targ).Entries.Length; ++i)
                    {
                        if (((LargeBOD)targ).Entries[i] != null)
                            ((LargeBOD)targ).Entries[i].Amount = ((LargeBOD)targ).AmountMax;
                    }
                }
            }
        }
    }
}