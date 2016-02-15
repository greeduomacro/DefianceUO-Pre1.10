/* This script was made by Massimiliano.
 *
 * Release Date: 13 November 2007.
 *
 * Version: 5.0.1
 *
 *
 * Scripts\Items\Skill Items\Magical\Misc\Moongate.cs
 *
 * add at top of script: using Server.Engines.Poker;
 *
 * and in methode "UseGate( Mobile m )" add:
 *
 * else if (PokerDealer.IsPokerPlayer(m) >= 0)
    {
        m.SendMessage("You cannot travel while playing poker.");
    }
 * This will prevent players from using a gate while they're seated at a poker table.
 *
 *
 * Scripts\Spells\Fourth\Recall.cs & Scripts\Spells\Third\Teleport.cs
 *
 * add at top of script: using Server.Engines.Poker;
 *
 * and in methode "CheckCast()" add:
 *
 *  else if (PokerDealer.IsPokerPlayer(Caster) >= 0)
    {
        Caster.SendMessage("You cannot travel while playing poker.");
        return false;
    }
 * This will prevent players from casting recall or teleport while they're seated at a poker table.
 *
 * Includes the scripts:
 * - Card.cs
 * - CardDeck.cs
 * - CardHand.cs
 * - HandResult.cs
 * - JackpotReward.cs
 * - PokerDealer.cs
 * - PokerGumps.cs
 * - PokerJackpot.cs
 * - PokerPlayer.cs
 * - PokerTablePotGold.cs
 * - PokerTable.cs
 * - PokerTournyTicket.cs
 */

using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Engines.Poker;

namespace Server.Engines.Poker
{
    public class PokerJackpot : Item
    {
        private bool m_Enabled;
        private string m_HandName;
        private int m_Jackpot;
        private int m_HandID;
        private bool m_IsHighRoller;
        private ArrayList m_JackpotWinner = new ArrayList();

        [CommandProperty(AccessLevel.Seer)]
        public bool Running { get { return m_Enabled; } set { m_Enabled = value; if (m_Enabled) StartTimer(); } }
        public string HandName { get { return m_HandName; } set { m_HandName = value; } }
        public int Jackpot { get { return m_Jackpot; } set { m_Jackpot = value; } }
        public int HandID { get { return m_HandID; } set { m_HandID = value; } }
        public bool IsHighRoller { get { return m_IsHighRoller; } set { m_IsHighRoller = value; } }
        public ArrayList JackpotWinner { get { return m_JackpotWinner; } set { m_JackpotWinner = value; } }
        public TimeSpan TimeLeft { get { if (m_Enabled) return m_End - DateTime.Now; else return TimeSpan.FromSeconds(0); } }

        [Constructable]
        public PokerJackpot() : base(3820)
        {
            Movable = false;
            m_Jackpot = 0;
            m_IsHighRoller = false;
            m_HandID = 0;
        }

        public PokerJackpot(Serial serial) : base(serial)
        {
        }

        public override void OnAfterDelete()
        {
            if (m_Timer != null)
                m_Timer.Stop();
            m_Timer = null;
            base.OnAfterDelete();
        }

        public override void OnDelete()
        {
            if (m_Timer != null)
                m_Timer.Stop();
            base.OnDelete();
        }

        public void StartTimer()
        {
            if (!m_Enabled)
                return;
            if (m_End < DateTime.Now)
            {
                TimeSpan delay = TimeSpan.FromDays(7.0);
                StartTimer(delay);
            }
        }

        public void StartTimer(TimeSpan delay)
        {
            if (!m_Enabled)
                return;
            m_End = DateTime.Now + delay;
            if (m_Timer != null)
                m_Timer.Stop();
            m_Timer = new InternalTimer(this, delay);
            m_Timer.Start();
        }

        public override void OnSingleClick(Mobile from)
        {
            if (from.AccessLevel >= AccessLevel.GameMaster)
            {
                if (m_Enabled)
                    LabelTo(from, "Time left: {0:00}:{1:00}:{2:00}", (int)(TimeLeft.TotalSeconds / 60 / 60), (int)(TimeLeft.TotalSeconds / 60) % 60, (int)(TimeLeft.TotalSeconds) % 60);
                else
                    LabelTo(from, "Timer disabled");
                LabelTo(from, "MasterPokerJackpot");
                LabelTo(from, "[Do not delete!!]");
            }
            else
                if (m_Jackpot == 0)
                    LabelTo(from, "Jackpot amount: 0gp");
                else
                    LabelTo(from, "Jackpot amount: {0:0,0}gp", (double)m_Jackpot);
        }

        public void CompareHand(Mobile mobile, int hndid, bool highroller, string combname)
        {
            if (hndid == m_HandID)
            {
                if (((IsHighRoller && highroller) || (!IsHighRoller && !highroller)) && AddToList(mobile))
                    JackpotWinner.Add(mobile);
                if (!IsHighRoller && highroller)
                {
                    JackpotWinner.Clear();
                    JackpotWinner.Add(mobile);
                    IsHighRoller = highroller;
                    HandName = combname;
                }
            }
            if (hndid > m_HandID)
            {
                JackpotWinner.Clear();
                JackpotWinner.Add(mobile);
                IsHighRoller = highroller;
                HandID = hndid;
                HandName = combname;
            }
        }

        public bool AddToList(Mobile from)
        {
            foreach (Mobile mobile in JackpotWinner)
                if (from == mobile)
                    return false;
            return true;
        }

        private InternalTimer m_Timer;
        private DateTime m_End;

        private class InternalTimer : Timer
        {
            private PokerJackpot m_Stone;

            public InternalTimer(PokerJackpot stone, TimeSpan delay) : base(delay)
            {
                m_Stone = stone;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                Stop();
                if (m_Stone.JackpotWinner.Count == 0)
                {
                    bcmsg(String.Format("Jackpot announcement."));
                    bcmsg(String.Format("No jackpot winner, jackpot starts at {0:0,0}gp.", (double)m_Stone.Jackpot));
                }
                else
                {
                    bcmsg(String.Format("Jackpot winner!"));
                    int todis = 0;

                    if (m_Stone.IsHighRoller)
                    {
                        todis = m_Stone.Jackpot;
                        m_Stone.Jackpot = 0;
                    }
                    else
                    {
                        m_Stone.Jackpot /= 2;
                        todis = m_Stone.Jackpot;
                    }

                    for (int j = 0; j < m_Stone.JackpotWinner.Count; j++)
                    {
                        todis /= m_Stone.JackpotWinner.Count;
                        Mobile player = (Mobile)m_Stone.JackpotWinner[j];
                        BankBox bank = player.BankBox;
                        bcmsg(String.Format("{0} won {1:0,0}gp in the poker jackpot with a {2}!", player.Name, (double)todis, m_Stone.HandName));
                        bank.DropItem(new JackpotReward(todis, player.Name));
                        m_Stone.JackpotWinner.Remove(player);
                        j--;
                    }

                    if (m_Stone.Jackpot == 0)
                        bcmsg(String.Format("The jackpot for next week starts at 0gp."));
                    else
                        bcmsg(String.Format("The jackpot for next week starts at {0:0,0}gp.", (double)m_Stone.Jackpot));
                }
                m_Stone.IsHighRoller = false;
                m_Stone.HandName = null;
                m_Stone.HandID = 0;
                m_Stone.StartTimer();
            }

            public static void bcmsg(string message)
            {
                foreach (NetState state in NetState.Instances)
                {
                    Mobile m = state.Mobile;
                    if (m != null)
                        m.SendMessage(0x482, message);
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);

            writer.Write(m_HandName);
            writer.Write(m_HandID);
            writer.Write(m_Jackpot);
            writer.Write(m_IsHighRoller);
            writer.WriteMobileList(m_JackpotWinner);
            writer.Write(m_Enabled);
            if (m_Enabled)
                writer.WriteDeltaTime(m_End);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        m_HandName = reader.ReadString();
                        m_HandID = reader.ReadInt();
                        m_Jackpot = reader.ReadInt();
                        m_IsHighRoller = reader.ReadBool();
                        m_JackpotWinner = reader.ReadMobileList();
                        m_Enabled = reader.ReadBool();
                        TimeSpan ts = TimeSpan.Zero;
                        if (m_Enabled)
                        {
                            ts = reader.ReadDeltaTime() - DateTime.Now;
                            StartTimer(ts);
                        }
                        break;
                    }
            }
        }
    }
}