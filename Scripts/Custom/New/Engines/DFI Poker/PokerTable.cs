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
using Server.Gumps;
using Server.Items;
using Server.Engines.Poker;

namespace Server.Engines.Poker
{
    public class PokerTableEast : BaseAddon
    {
        [Constructable]
        public PokerTableEast() : base()
        {
            ItemID = 4779;
            Visible = true;
            Name = "a deck of cards";
            Movable = false;
            AddonComponent

            component = new AddonComponent(2931);
            component.Hue = 1367;
            component.Name = "a poker table";
            AddComponent(component, -1, -4, -8);
            component = new AddonComponent(2930);
            component.Hue = 1367;
            component.Name = "a poker table";
            AddComponent(component, 1, -4, -8);
            component = new AddonComponent(2928);
            component.Hue = 1367;
            component.Name = "a poker table";
            AddComponent(component, -1, 0, -8);
            component = new AddonComponent(2929);
            component.Hue = 1367;
            component.Name = "a poker table";
            AddComponent(component, 1, 0, -8);
            component = new AddonComponent(2931);
            component.Hue = 1367;
            component.Name = "a poker table";
            AddComponent(component, -1, -3, -8);
            component = new AddonComponent(2931);
            component.Hue = 1367;
            component.Name = "a poker table";
            AddComponent(component, -1, -2, -8);
            component = new AddonComponent(2931);
            component.Hue = 1367;
            component.Name = "a poker table";
            AddComponent(component, -1, -1, -8);
            component = new AddonComponent(2931);
            component.Hue = 1367;
            component.Name = "a poker table";
            AddComponent(component, 0, -4, -8);
            component = new AddonComponent(2931);
            component.Hue = 1367;
            component.Name = "a poker table";
            AddComponent(component, 0, -3, -8);
            component = new AddonComponent(2931);
            component.Hue = 1367;
            component.Name = "a poker table";
            AddComponent(component, 0, -2, -8);
            component = new AddonComponent(2931);
            component.Hue = 1367;
            component.Name = "a poker table";
            AddComponent(component, 0, -1, -8);
            component = new AddonComponent(2932);
            component.Hue = 1367;
            component.Name = "a poker table";
            AddComponent(component, 0, 0, -8);
            component = new AddonComponent(2931);
            component.Hue = 1367;
            component.Name = "a poker table";
            AddComponent(component, 1, -3, -8);
            component = new AddonComponent(2931);
            component.Hue = 1367;
            component.Name = "a poker table";
            AddComponent(component, 1, -2, -8);
            component = new AddonComponent(2931);
            component.Hue = 1367;
            component.Name = "a poker table";
            AddComponent(component, 1, -1, -8);

            #region stools
            component = new AddonComponent(2602);
            component.Hue = 1872;
            component.Name = "a poker stool";
            AddComponent(component, -2, 0, -8);
            component = new AddonComponent(2602);
            component.Hue = 1872;
            component.Name = "a poker stool";
            AddComponent(component, -2, -1, -8);
            component = new AddonComponent(2602);
            component.Hue = 1872;
            component.Name = "a poker stool";
            AddComponent(component, -2, -2, -8);
            component = new AddonComponent(2602);
            component.Hue = 1872;
            component.Name = "a poker stool";
            AddComponent(component, -2, -3, -8);
            component = new AddonComponent(2602);
            component.Hue = 1872;
            component.Name = "a poker stool";
            AddComponent(component, -2, -4, -8);
            component = new AddonComponent(2602);
            component.Hue = 1872;
            component.Name = "a poker stool";
            AddComponent(component, 2, 0, -8);
            component = new AddonComponent(2602);
            component.Hue = 1872;
            component.Name = "a poker stool";
            AddComponent(component, 2, -1, -8);
            component = new AddonComponent(2602);
            component.Hue = 1872;
            component.Name = "a poker stool";
            AddComponent(component, 2, -2, -8);
            component = new AddonComponent(2602);
            component.Hue = 1872;
            component.Name = "a poker stool";
            AddComponent(component, 2, -3, -8);
            component = new AddonComponent(2602);
            component.Hue = 1872;
            component.Name = "a poker stool";
            AddComponent(component, 2, -4, -8);
            #endregion

            #region cards
            AddComponent(new AddonComponent(3608), -1, 0, -2);
            AddComponent(new AddonComponent(3608), -1, -1, -2);
            AddComponent(new AddonComponent(3608), -1, -2, -2);
            AddComponent(new AddonComponent(3608), -1, -3, -2);
            AddComponent(new AddonComponent(3608), -1, -4, -2);
            AddComponent(new AddonComponent(3609), 1, 0, -2);
            AddComponent(new AddonComponent(3609), 1, -1, -2);
            AddComponent(new AddonComponent(3609), 1, -2, -2);
            AddComponent(new AddonComponent(3609), 1, -3, -2);
            AddComponent(new AddonComponent(3609), 1, -4, -2);
            #endregion

            Timer.DelayCall(TimeSpan.Zero, new TimerCallback(MoveToSurface));
        }

        private void MoveToSurface()
        {
            Z += 8;
        }

        public PokerTableEast(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
    public class PokerTableSouth : BaseAddon
    {
        [Constructable]
        public PokerTableSouth() : base()
        {
            ItemID = 4780;
            Visible = true;
            Name = "a deck of cards";
            Movable = false;
            AddonComponent

            component = new AddonComponent(2931);
            component.Hue = 1367;
            component.Name = "a poker table";
            AddComponent(component, 0, -1, -8);

            component = new AddonComponent(2931);
            component.Hue = 1367;
            component.Name = "a poker table";
            AddComponent(component, 1, -1, -8);

            component = new AddonComponent(2931);
            component.Hue = 1367;
            component.Name = "a poker table";
            AddComponent(component, 2, -1, -8);

            component = new AddonComponent(2931);
            component.Hue = 1367;
            component.Name = "a poker table";
            AddComponent(component, 3, -1, -8);

            component = new AddonComponent(2930);
            component.Hue = 1367;
            component.Name = "a poker table";
            AddComponent(component, 4, -1, -8);

            component = new AddonComponent(2931);
            component.Hue = 1367;
            component.Name = "a poker table";
            AddComponent(component, 0, 0, -8);

            component = new AddonComponent(2931);
            component.Hue = 1367;
            component.Name = "a poker table";
            AddComponent(component, 1, 0, -8);

            component = new AddonComponent(2931);
            component.Hue = 1367;
            component.Name = "a poker table";
            AddComponent(component, 2, 0, -8);

            component = new AddonComponent(2931);
            component.Hue = 1367;
            component.Name = "a poker table";
            AddComponent(component, 3, 0, -8);

            component = new AddonComponent(2931);
            component.Hue = 1367;
            component.Name = "a poker table";
            AddComponent(component, 4, 0, -8);

            component = new AddonComponent(2928);
            component.Hue = 1367;
            component.Name = "a poker table";
            AddComponent(component, 0, 1, -8);

            component = new AddonComponent(2932);
            component.Hue = 1367;
            component.Name = "a poker table";
            AddComponent(component, 1, 1, -8);

            component = new AddonComponent(2932);
            component.Hue = 1367;
            component.Name = "a poker table";
            AddComponent(component, 2, 1, -8);

            component = new AddonComponent(2932);
            component.Hue = 1367;
            component.Name = "a poker table";
            AddComponent(component, 3, 1, -8);

            component = new AddonComponent(2929);
            component.Hue = 1367;
            component.Name = "a poker table";
            AddComponent(component, 4, 1, -8);

            #region stools
            component = new AddonComponent(2602);
            component.Hue = 1872;
            component.Name = "a poker stool";
            AddComponent(component, 0, -2, -8);
            component = new AddonComponent(2602);
            component.Hue = 1872;
            component.Name = "a poker stool";
            AddComponent(component, 1, -2, -8);
            component = new AddonComponent(2602);
            component.Hue = 1872;
            component.Name = "a poker stool";
            AddComponent(component, 2, -2, -8);
            component = new AddonComponent(2602);
            component.Hue = 1872;
            component.Name = "a poker stool";
            AddComponent(component, 3, -2, -8);
            component = new AddonComponent(2602);
            component.Hue = 1872;
            component.Name = "a poker stool";
            AddComponent(component, 4, -2, -8);
            component = new AddonComponent(2602);
            component.Hue = 1872;
            component.Name = "a poker stool";
            AddComponent(component, 0, 2, -8);
            component = new AddonComponent(2602);
            component.Hue = 1872;
            component.Name = "a poker stool";
            AddComponent(component, 1, 2, -8);
            component = new AddonComponent(2602);
            component.Hue = 1872;
            component.Name = "a poker stool";
            AddComponent(component, 2, 2, -8);
            component = new AddonComponent(2602);
            component.Hue = 1872;
            component.Name = "a poker stool";
            AddComponent(component, 3, 2, -8);
            component = new AddonComponent(2602);
            component.Hue = 1872;
            component.Name = "a poker stool";
            AddComponent(component, 4, 2, -8);
            #endregion

            #region cards
            AddComponent(new AddonComponent(3606), 0, -1, -5);
            AddComponent(new AddonComponent(3606), 1, -1, -5);
            AddComponent(new AddonComponent(3606), 2, -1, -5);
            AddComponent(new AddonComponent(3606), 3, -1, -5);
            AddComponent(new AddonComponent(3606), 4, -1, -5);
            AddComponent(new AddonComponent(3605), 0, 1, -2);
            AddComponent(new AddonComponent(3605), 1, 1, -2);
            AddComponent(new AddonComponent(3605), 2, 1, -2);
            AddComponent(new AddonComponent(3605), 3, 1, -2);
            AddComponent(new AddonComponent(3605), 4, 1, -2);
            #endregion

            Timer.DelayCall(TimeSpan.Zero, new TimerCallback(MoveToSurface));
        }

        private void MoveToSurface()
        {
            Z += 8;
        }

        public PokerTableSouth(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}