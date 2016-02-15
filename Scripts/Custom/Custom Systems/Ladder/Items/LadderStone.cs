/**
*	Ladder system by Morten Legarth (c)taunted.dk ( legarth@taunted.dk )
*	Version: v0.10 -  26-02-2005
*
*	This system has been written for use at the Blitzkrieg frees-shard
*	http://blitzkrieg.dorijan.de . Unauthorized reproduction or redistribution
*	is prohibited.
*
*							LadderStone.cs
*						-------------------------
*
*	File Description:	Ladder stone for the ladder system.
*						This stone works as the interface for
*						the ladder system
*/


using System;
using Server.Accounting;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Targeting;
using System.Collections;
using Server.Scripts;
using Server.Prompts;

namespace Server.Ladder
{

    public class LadderStone : Item
    {
        private bool m_Active;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Active
        {
            get { return m_Active; }
            set { m_Active = value; InvalidateProperties(); }
        }

        [Constructable]
        public LadderStone() : base( 0xED4 )
        {
            Movable = false;
            Name = "a ladder stone";
            m_Active = false;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from != null && from is PlayerMobile)
            {
                if (!from.InRange(GetWorldLocation(), 3))
                    from.SendLocalizedMessage(500446); // That is too far away.
                else if (Ladder.Duellers.Contains(from))
                    from.SendMessage("You are already involved in a duel");
                else if (!m_Active)
                    from.SendMessage("This stone is deactivated, contact a GameMaster to play.");
                else
                {
					if (!Ladder.Players.Contains(from))
					{
						Ladder.Players.Add(from);
						Ladder.Players.Sort();
					}

					from.SendGump(new ConfigGump(50, 50, 0));
					//from.SendMessage("Select a player to challenge");
                    //from.Target = new ChallengeTarget();
                }
            }
        }

        public LadderStone(Serial serial) : base(serial)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);// version

            writer.Write(m_Active);

        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        m_Active = reader.ReadBool();

                        break;
                    }
            }
        }
    }
}