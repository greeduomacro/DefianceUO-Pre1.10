using Server.Network;
using Server.Mobiles;
using System;

namespace Server.Gumps
{
    public class PayedFullHealerGump : Gump
    {
        private int m_Price;
        private PayedFullHealer m_Healer;

        public PayedFullHealerGump(int gold, PayedFullHealer healer)
            : base(50, 50)
        {
            m_Price = gold;
            m_Healer = healer;

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;
            AddPage(0);

            AddBackground(0, 0, 400, 300, 3600);
            AddImage(-34, 16, 60970);
            AddImage(-34, 20, 60897, 1193);
            AddRadio(120, 180, 208, 209, true, 0);
            AddLabel(150, 180, 1149, String.Format("Full Resurrection ({0} gp)", gold));
            AddRadio(120, 210, 208, 209, false, 1);
            AddLabel(150, 210, 1149, "Regular Resurrection (free)");
            AddButton(270, 250, 247, 248, 1, GumpButtonType.Reply, 0);
            AddButton(70, 250, 243, 241, 0, GumpButtonType.Reply, 0);
            AddHtml(0, 28, 400, 25, "<basefont color=#ffffff><center>Resurrection Gump</center></basefont>", false, false);
            AddHtml(118, 68, 253, 100, "<basefont color=#ffffff>Psst... hey,<br>Come over here. I see you are in need of some assistance. I can offer you a resurrection and replenish your mana and health for a small price.</basefont>", false, false);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile m = sender.Mobile;

            if (info.ButtonID == 1)
            {
                if (m.InRange(m_Healer, 16))
                {
                    if (info.IsSwitched(0) && Banker.Withdraw(m, m_Price))
                    {
                        Timer.DelayCall(TimeSpan.FromSeconds(10.0), new TimerStateCallback(PayedRes), m);
                        m.Frozen = true;
                    }

                    else
                    {
                        m.Resurrect();
                        m.PlaySound(0x214);
                        m.FixedEffect(0x376A, 10, 16);
                    }
                }
                else
                    m.SendMessage("You have wandered to far off to gain any benefits from the healers practices.");
            }
        }

        private static void PayedRes(object obj)
        {
            Mobile m = (Mobile)obj;
            m.Resurrect();
            m.Hits = m.HitsMax;
            m.Mana = m.ManaMax;
            m.Frozen = false;
            m.PlaySound(0x214);
            m.FixedEffect(0x376A, 10, 16);
        }
    }
}