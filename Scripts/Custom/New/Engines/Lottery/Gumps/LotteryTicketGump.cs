/*
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
 * - Neither the name of Max Kellermann nor the names of its
 * contributors may be used to endorse or promote products derived
 * from this software without specific prior written permission.
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
 * $Id: LotteryTicketGump.cs 4 2005-01-20 16:45:25Z max $
 */

using Server.Items;
using Server.Network;
using Server.Misc;

namespace Server.Gumps {

    /**
     * Gump which displays the lottery ticket and lets the user pick
     * his numbers. Once the ticket is submitted, it is read only.
     */
    public class LotteryTicketGump : Gump {
        private const int borderLeft = 45;
        private const int borderTop = 25;
        private const int borderRight = 40;
        private const int borderBottom = 25;
        private const int columns = 7;
        private const int rows = ((int)LotteryConstants.Range + columns - 1) / columns;
        private const int buttonWidth = 55;
        private const int buttonHeight = 30;

        private LotteryTicket ticket;
        private bool[] data = new bool[LotteryConstants.Range];
        private uint numSelected = 0;

        public LotteryTicketGump(LotteryTicket _ticket) : base(250, 250) {
            ticket = _ticket;

            uint[] td = ticket.Data;
            if (td != null) {
                foreach (uint i in td) {
                    if (i >= 1 && i <= LotteryConstants.Range &&
                        !data[i - 1] && numSelected < LotteryConstants.Picks) {
                        data[i - 1] = true;
                        numSelected++;
                    }
                }
            }

            constructGump();
        }

        private LotteryTicketGump(LotteryTicket _ticket, bool[] _data) : base(250, 250) {
            ticket = _ticket;
            data = _data;

            foreach (bool f in data)
                if (f)
                    numSelected++;

            constructGump();
        }

        private void constructGump() {
            bool writable = ticket.Writable;

            AddBackground(0, 0,
                          borderLeft + columns * buttonWidth + borderRight,
                          borderTop + buttonHeight + rows * buttonHeight + 20 + buttonHeight + borderBottom,
                          2600);

            /* label */
            AddLabel(borderLeft, borderTop, 902,
                     writable
                     ? "Choose six numbers:"
                     : "You chose the following numbers:");

            /* the number buttons */
            for (int v = 1, y = 0; y < rows; y++) {
                for (int x = 0; x < columns && v <= LotteryConstants.Range; x++, v++) {
                    bool selected = data[v - 1];
                    if (writable && !selected && numSelected < LotteryConstants.Picks)
                        AddButton(borderLeft + x * buttonWidth,
                                  borderTop + buttonHeight + y * buttonHeight,
                                  2152, 2154, v, GumpButtonType.Reply, 0);
                    AddLabel(borderLeft + x * buttonWidth + 30,
                             borderTop + buttonHeight + y * buttonHeight,
                             selected ? 39 : 902, "" + v);
                }
            }

            /* command buttons */
            if (writable && numSelected == LotteryConstants.Picks) {
                /* allow "write down" only when all numbers are
                   selected */
                AddButton(borderLeft,
                          borderTop + buttonHeight + rows * buttonHeight + 20,
                          2152, 2154, -3, GumpButtonType.Reply, 0);
                AddLabel(borderLeft + 30,
                         borderTop + buttonHeight + rows * buttonHeight + 20,
                         902, "Write down");
            }

            if (writable && numSelected > 0) {
                AddButton(borderLeft + 120,
                          borderTop + buttonHeight + rows * buttonHeight + 20,
                          2152, 2154, -2, GumpButtonType.Reply, 0);
                AddLabel(borderLeft + 120 + 30,
                         borderTop + buttonHeight + rows * buttonHeight + 20,
                         902, "Reset");
            }

            AddButton(borderLeft + 300,
                      borderTop + buttonHeight + rows * buttonHeight + 20,
                      2152, 2154, -1, GumpButtonType.Reply, 0);
            AddLabel(borderLeft + 300 + 30,
                     borderTop + buttonHeight + rows * buttonHeight + 20,
                     902, writable ? "Cancel" : "Close");
        }

        public override void OnResponse(NetState sender, RelayInfo info) {
            if (sender == null ||
                sender.Mobile == null || sender.Mobile.Deleted ||
                sender.Mobile.Backpack == null ||
                info == null ||
                ticket.Deleted)
                return; /* pedantic check */

            if (!ticket.IsChildOf(sender.Mobile.Backpack)) {
                sender.Mobile.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            if (!ticket.Writable)
                return;

            if (info.ButtonID >= 1 && info.ButtonID <= LotteryConstants.Range) {
                /* user clicked on a number */
                if (numSelected < LotteryConstants.Picks)
                    data[info.ButtonID - 1] = true;

                sender.Mobile.SendGump(new LotteryTicketGump(ticket, data));
            } else if (info.ButtonID == -2) {
                /* reset button */
                sender.Mobile.SendGump(new LotteryTicketGump(ticket, new bool[LotteryConstants.Range]));
            } else if (info.ButtonID == -3) {
                /* write down button */
                if (numSelected == LotteryConstants.Picks) {
                    uint[] d = new uint[LotteryConstants.Picks];
                    uint i = 0, v = 1;
                    foreach (bool f in data) {
                        if (f)
                            d[i++] = v;
                        v++;
                    }
                    ticket.Data = d;

                    sender.Mobile.SendMessage("You have noted the numbers on your lottery ticket.");
                }
            }
        }
    }
}