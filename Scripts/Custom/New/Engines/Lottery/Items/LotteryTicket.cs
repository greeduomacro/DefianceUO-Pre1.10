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
 * $Id: LotteryTicket.cs 5 2005-01-20 16:59:00Z max $
 */

using Server.Misc;
using Server.Gumps;

namespace Server.Items {

    public class LotteryTicket : Item {
        /** index of the drawing this ticket was submitted to; if
            0, it was not submitted yet, and is still writable */
        private uint drawing = 0;
        /** numbers the player chose; null if none yet */
        private uint[] data = null;

        public LotteryTicket(Serial serial) : base(serial) {
        }

        [Constructable]
        public LotteryTicket() : base(0x227a) {
            Name = "an empty lottery ticket";
	    LootType = (LootType)2; // blessed
            Weight = 0.1;
            Hue = 250;
        }

        /** true if the user may still modify the ticket. false if it
            was already submitted */
        [CommandProperty(AccessLevel.Counselor)]
        public bool Writable {
            get {
                return drawing == 0;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public uint Drawing {
            get {
                return drawing;
            }
            /** called by the basket, when the ticket is submitted */
            set {
                if (drawing != 0 || value == 0 || data == null)
                    return;

                drawing = value;

                string[] text = new string[LotteryConstants.Picks];
                for (int z = 0; z < LotteryConstants.Picks; z++)
                    text[z] = "" + data[z];
		Name = "a submitted lottery ticket: " + string.Join(", ", text);
            }
        }

        public uint[] Data {
            get {
                return data;
            }
            /** called by the gump, when the user choses numbers */
            set {
                if (drawing != 0 || value == null ||
                    value.Length != LotteryConstants.Picks)
                    return;

                data = value;

                string[] text = new string[LotteryConstants.Picks];
                for (int z = 0; z < LotteryConstants.Picks; z++)
                    text[z] = "" + data[z];
                Name = "a lottery ticket: " + string.Join(", ", text);
            }
        }

        public override void Serialize(GenericWriter writer) {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write(data != null);
            if (data != null) {
                foreach (int v in data)
                    writer.Write(v);
                writer.Write(drawing);
            }
        }

        public override void Deserialize(GenericReader reader) {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version) {
            case 1:
                if (reader.ReadBool()) {
                    data = new uint[LotteryConstants.Picks];
                    for (uint i = 0; i < LotteryConstants.Picks; i++)
                        data[i] = reader.ReadUInt();
                    drawing = reader.ReadUInt();
                }
                break;
            }
        }

        public override void OnDoubleClick(Mobile from) {
            if (from == null || from.Deleted || from.Backpack == null)
                return; /* pedantic check */

            if (!IsChildOf(from.Backpack)) {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            from.SendGump(new LotteryTicketGump(this));
        }
    }
}