/*
 * Copyright (c) 2005, Kai Sassmannshausen <kai@sassie.org>
 * Copyright (c) 2005, Max Kellermann <max@duempel.org>
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 *
 * - Redistributions of source code must retain the above copyright
 * notice, this list of conditions and the following disclaimer.
 *
 * - Redistributions in binary form must reproduce the above copyright
 * notice, this list of conditions and the following disclaimer in the
 * documentation and/or other materials provided with the
 * distribution.
 *
 * - Neither the name of Max Kellermann, Kai Sassmannshausen  nor the
 * names of its contributors may be used to endorse or promote products
 * derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
 * FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE
 * COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
 * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
 * STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
 * OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * $Id: LotteryBasketGump.cs 4 2005-01-20 16:45:25Z max $
 */

using Server.Items;
using Server.Network;
using Server.Misc;

namespace Server.Gumps {
    /**
     * Gump for the lottery basket. It shows information about the
     * lottery (jackpot etc.) and lets the player buy a lottery
     * ticket.
     */

    public class LotteryBasketGump : Gump {
        private LotteryBasket basket;

        public LotteryBasketGump(Mobile from, LotteryBasket _basket) : base(200, 200) {
	    basket = _basket;

            AddBackground(0, 0, 400, 260, 2600);

            int y = 25;
            AddLabel(45, y, 902, "Defiance Lottery");
            y += 5;

            y += 30;
            ulong jp = basket.Award;
            foreach (ulong v in basket.Jackpot)
                jp += v;
            AddLabel(45, y, 902, "Current jackpot: " + jp + " gold");

            y += 30;
            uint[] outcome = basket.Outcome;
            string lastDrawing;
            if (outcome == null) {
                lastDrawing = "none yet";
            } else if (basket.Mode == LotteryBasketMode.Closed &&
                       from.AccessLevel <= AccessLevel.Counselor) {
                /* only GMs can see the new drawing until the
                 * lottery is open again */
                lastDrawing = "unknown yet";
            } else {
	        string[] text = new string[LotteryConstants.Picks];
                for (int z = 0; z < LotteryConstants.Picks; z++)
                    text[z] = "" + outcome[z];
                lastDrawing = string.Join(", ", text);
            }
            AddLabel(45, y, 902,
                     (basket.Mode == LotteryBasketMode.Closed ? "Next" : "Last") +
                     " drawing: " + lastDrawing);


            y += 40;
            AddButton(45, y, 2152, 2154, 1, GumpButtonType.Reply, 0);
            AddLabel(90, y, 902, "Buy a ticket for " + LotteryConstants.TicketPrice + " gold");

	    y += 30;
	    if (from.AccessLevel >= AccessLevel.Counselor) {
	      AddButton(45, y, 2152, 2154, 3, GumpButtonType.Reply, 0);
	      AddLabel(90, y, 133, "DFI Staff Functions");
	    }


            y += 30;
            AddButton(45, y, 2152, 2154, -1, GumpButtonType.Reply, 0);
            AddLabel(90, y, 902, "Cancel");
        }

        public override void OnResponse(NetState sender, RelayInfo info) {
            if (sender == null ||
                sender.Mobile == null || sender.Mobile.Deleted ||
                info == null ||
                basket.Deleted)
                return; /* pedantic check */

            if (info.ButtonID < 0 &&
                sender.Mobile.AccessLevel < AccessLevel.GameMaster)
                return;

            switch (info.ButtonID) {
            case 1:
                basket.buyTicket(sender.Mobile);
                break;
	    case 3:
		sender.Mobile.SendGump(new LotteryGMGump(sender.Mobile, basket));
		break;
            }
        }
    }
}