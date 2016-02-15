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
 * - Neither the name of Kai Sassmannshausen, Max Kellermann nor
 * the names of its contributors may be used to endorse or promote
 * products derived from this software without specific prior written
 * permission. THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND
 * CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,
 * BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
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
 */

using System;
using System.Collections;
using Server.Gumps;
using Server.Misc;

namespace Server.Items {
    public enum LotteryBasketMode {
        /** the whole lottery system is disabled */
        Disabled,
        /** open lottery. players can submit tickets and pay in their
            winning tickets */
        Open,
        /** temporarily closed lottery. the new numbers are already
            determined, but only staff members can see it. during this
            mode, the staff plays the drawing, and reopens the lottery
            after they published the new numbers */
        Closed,
    };

    /**
     * The LotteryBasket is the heart of the lottery system. It
     * implements all the rules, registers submitted tickets,
     * generates the new drawing etc. There can only be one basket.
     *
     * For security, the staff cannot modify the drawing. They can
     * only start the new drawing, but the numbers are generated
     * internally by this class. Manipulation is not possible, because
     * all players can see the official (unmodifiable) numbers after
     * the lottery is reopened.
     *
     * How to implement a lottery: XXX document this.
     */
    public class LotteryBasket : Item {
        private uint drawing;
        /** tickets which were submitted for the previous drawing, and
            may have won a prize */
        private Hashtable tickets = new Hashtable();
        /** incoming tickets which have been submitted for the next
            drawing */
        private Hashtable incoming = new Hashtable();
        /** jackpot [gp] in each of the winning classes */
        private ulong[] jackpot = new ulong[]{0, 0, 0, 0};
        /** award per ticket [gp] in each of the winning classes */
        private ulong[] awards = new ulong[]{0, 0, 0, 0};
        private ulong award = 0, given = 0;
        /** how much money has already been destroyed by the
            lottery? */
        private ulong destroyed = 0;
        /** the current lottery mode */
        private LotteryBasketMode mode = LotteryBasketMode.Open;
        /** result of the last drawing */
        private uint[] outcome = null;
        private int[] winners = new int[]{0, 0, 0, 0};
        private uint oldParticipants = 0;

        private uint[] drawingCount = new uint[]{0, 0, 0, 0};

        private bool itemPrize;
        private Item board;

        private int version = 1;
        private bool debug=false;

        public LotteryBasket(Serial serial) : base(serial) {
        }

        [Constructable]
        public LotteryBasket() : base(0x990) {
            Name = "a lottery basket";
            Weight = 700.0;
            Movable = false;
            Hue = 388;
            drawing = (uint)new Random().Next(1, 65536);
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Version {
            get {
                return version;
            }
        }

        public bool Debug {
	    get {
	        return debug;
	    }
	}

        public ulong[] Jackpot {
            get {
                return jackpot;
            }
        }

        public uint Drawing {
	    get {
	        return drawing;
	    }
	}

        public uint[] DrawingCount {
	    get {
	        return drawingCount;
	    }
	}

       public uint OldParticipants {
            get {
                return oldParticipants;
            }
        }

        [CommandProperty(AccessLevel.Counselor)]
        public uint Participants {
            get {
                return mode == LotteryBasketMode.Open
                    ? (uint)incoming.Count
                    : (uint)tickets.Count;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Item Board {
            get {
	      return board;
            }
	    set {
	      board = value;
	    }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ItemPrize {
            get {
	      return itemPrize;
            }
	    set {
	      itemPrize = value;
	    }
        }

        [CommandProperty(AccessLevel.Counselor)]
        public ulong Jackpot6 {
            get {
                return jackpot[0];
            }
        }

        [CommandProperty(AccessLevel.Counselor)]
        public ulong Jackpot5 {
            get {
                return jackpot[1];
            }
        }

        [CommandProperty(AccessLevel.Counselor)]
        public ulong Jackpot4 {
            get {
                return jackpot[2];
            }
        }

        [CommandProperty(AccessLevel.Counselor)]
        public ulong Jackpot3 {
            get {
                return jackpot[3];
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public ulong Destroyed {
            get {
                return destroyed;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public ulong Award {
            get {
                return award;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public ulong Given {
            get {
                return given;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public LotteryBasketMode Mode {
            get {
                return mode;
            }
        }

        public uint[] Outcome {
            get {
                return outcome;
            }
        }

      public ulong[] Awards {
	get {
	  return awards;
	}
      }

      public int[] Winners {
	get {
	  return winners;
	}
      }

      /*public bool setBoard (Target _target) {
  	  board.set;
	  return true;
	  }*/

      private bool publishNumbers (uint[] _numbers )
      {
	if ( board != null && !board.Deleted ) {
	  Array.Sort(_numbers);
	  string[] text = new string[6];
	  for (int z = 0; z < 6; z++)
	    text[z] = "" + _numbers[z];
	  board.Name = "Last drawn Numbers: " + string.Join(", ", text);
	  return true;
	}
	else
	{
	  board = null;
	  return false;
	}
      }

      private bool takeMoney(Mobile from, int amount) {
	if ( from == null || from.Deleted || amount <= 0 )
	  return false;
	Container cont;

	if (from.Backpack != null) {
	  cont = from.Backpack;
	  if (cont != null && cont.ConsumeTotal(typeof(Gold), amount)) {
	    from.PlaySound(0x32);
	    return true;
	  }
	}

	if (from.BankBox != null) {
	  cont = from.BankBox;
	  if (cont != null && cont.ConsumeTotal(typeof(Gold), amount)) {
	    from.PlaySound(0x32);
	    return true;
	  }
	}

	from.SendLocalizedMessage(500191); //Begging thy pardon, but thy bank  account lacks these funds.
	return false;
      }

      private bool giveMoney( Mobile to, int amount, bool mainwin )
      {
	if ( to == null || to.Deleted || amount <= 0 || to.Backpack == null )
	  return false;

	if ( itemPrize && mainwin ) {
	  Item WinningDeed = new Item( 5360 );
	  WinningDeed.Name="Congratulations! You won the Lotterys Main Prize. Page a GM or Seer and show him this Deed!";
	  WinningDeed.Hue=388;
	  WinningDeed.LootType=(LootType)2; // blessed
	  to.AddToBackpack( WinningDeed );
	  if ( debug )
	    Console.WriteLine("Outgiven: WinningDeed");
	}

	else
        {
	  Container playerPack = to.Backpack;
	  int itemCount = playerPack.GetAmount(typeof(Item), true);
	  int maxItems = playerPack.MaxItems;

	  if ( (amount / 1000000 + itemCount) < (maxItems -2) || amount <= 1000000 )
	  {
	      if ( debug )
		Console.WriteLine("Giving out: "+amount+" Gold");

	      while ( amount >= 1000000 )
	      {
	         to.AddToBackpack( new BankCheck( 1000000 ) );
		 amount -= 1000000;
	      }

	      if ( amount >= 5000 )
		to.AddToBackpack( new BankCheck( amount ) );
	      else
	      {
		if ( amount > 0 )
		  to.AddToBackpack( new Gold( amount) );
	      }
	  }
	  else {
	    if ( debug )
	      Console.WriteLine("Giving out GoldDeed. Value:"+amount);

	    Item GoldDeed = new Item ( 5360 );
	    GoldDeed.Name="Congratulations! You have won "+amount+" gold in the DFI Lottery. Page a GM or Seer and show him this Deed!";
	    GoldDeed.Hue=1448;
	    GoldDeed.LootType=(LootType)2; // blessed
	    to.AddToBackpack( GoldDeed );
	  }
	}

	return true;
      }


      public bool buyTicket(Mobile from) {
            if (from == null || from.Deleted || from.Backpack == null ||
                !from.Alive)
                return false; /* pedantic check */

            if (mode == LotteryBasketMode.Disabled) {
                from.SendMessage("The lottery is currently disabled, sorry.");
                return false;
            }

            if (!takeMoney(from, (int)LotteryConstants.TicketPrice))
                return false;

            LotteryTicket ticket = new LotteryTicket();
            if (!from.AddToBackpack(ticket)) {
                ticket.Delete();
                return false;
            }

            from.SendMessage("A new lottery ticket has been placed into your backpack.");
            return true;
        }

        public override void Serialize(GenericWriter writer) {
            base.Serialize(writer);

            writer.Write(version);

            writer.Write(drawing);

            foreach (uint v in drawingCount)
                writer.Write(v);

            foreach (ulong v in jackpot)
                writer.Write(v);

            foreach (ulong v in awards)
                writer.Write(v);

	    foreach (int v in winners)
	        writer.Write(v);

	    writer.Write(oldParticipants);

            writer.Write(award);
            writer.Write(given);
            writer.Write(destroyed);
            writer.Write((int)mode);

            writer.Write(outcome != null);
            if (outcome != null)
                foreach (int v in outcome)
                    writer.Write(v);

            writer.Write(tickets.Count);
            foreach (RegisteredTicket rt in tickets.Values) {
                writer.Write(rt.Ticket);
                foreach (int v in rt.Data)
                    writer.Write(v);
                writer.Write(rt.Class);
            }

            writer.Write(incoming.Count);
            foreach (RegisteredTicket rt in incoming.Values) {
                writer.Write(rt.Ticket);
                foreach (int v in rt.Data)
                    writer.Write(v);
            }
	    writer.Write( board );
	    writer.Write( itemPrize );
        }

        public override void Deserialize(GenericReader reader) {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch ( version ) {
            case 0:
                drawing = reader.ReadUInt();

                for (uint i = 0; i < jackpot.Length; i++)
                    jackpot[i] = reader.ReadULong();

                for (uint i = 0; i < awards.Length; i++)
                    awards[i] = reader.ReadULong();

                award = reader.ReadULong();
                given = reader.ReadULong();
                destroyed = reader.ReadULong();
                mode = (LotteryBasketMode)reader.ReadInt();

                if (reader.ReadBool()) {
                    outcome = new uint[LotteryConstants.Picks];
                    for (uint p = 0; p < LotteryConstants.Picks; p++)
                        outcome[p] = reader.ReadUInt();
                }

                uint count = reader.ReadUInt();
                for (uint i = 0; i < count; i++) {
                    int id = reader.ReadInt();
                    uint[] data = new uint[LotteryConstants.Picks];
                    for (uint p = 0; p < LotteryConstants.Picks; p++)
                        data[p] = reader.ReadUInt();
                    RegisteredTicket rt = new RegisteredTicket(id, data);
                    rt.Class = reader.ReadInt();
                    tickets[id] = rt;
                }

                count = reader.ReadUInt();
                for (uint i = 0; i < count; i++) {
                    int id = reader.ReadInt();
                    uint[] data = new uint[LotteryConstants.Picks];
                    for (uint p = 0; p < LotteryConstants.Picks; p++)
                        data[p] = reader.ReadUInt();
                    incoming[id] = new RegisteredTicket(id, data);
                }

		board = reader.ReadItem();
		itemPrize = reader.ReadBool();
                break;

	    case 1:
	        drawing = reader.ReadUInt();

                for (uint i = 0; i < drawingCount.Length; i++)
                    drawingCount[i] = reader.ReadUInt();

                for (uint i = 0; i < jackpot.Length; i++)
                    jackpot[i] = reader.ReadULong();

                for (uint i = 0; i < awards.Length; i++)
                    awards[i] = reader.ReadULong();

		for (uint i = 0; i < winners.Length; i++)
                    winners[i] = reader.ReadInt();

		oldParticipants = reader.ReadUInt();

		award = reader.ReadULong();
                given = reader.ReadULong();
                destroyed = reader.ReadULong();
                mode = (LotteryBasketMode)reader.ReadInt();

                if (reader.ReadBool()) {
                    outcome = new uint[LotteryConstants.Picks];
                    for (uint p = 0; p < LotteryConstants.Picks; p++)
                        outcome[p] = reader.ReadUInt();
                }

                count = reader.ReadUInt();
                for (uint i = 0; i < count; i++) {
                    int id = reader.ReadInt();
                    uint[] data = new uint[LotteryConstants.Picks];
                    for (uint p = 0; p < LotteryConstants.Picks; p++)
                        data[p] = reader.ReadUInt();
                    RegisteredTicket rt = new RegisteredTicket(id, data);
                    rt.Class = reader.ReadInt();
                    tickets[id] = rt;
                }

                count = reader.ReadUInt();
                for (uint i = 0; i < count; i++) {
                    int id = reader.ReadInt();
                    uint[] data = new uint[LotteryConstants.Picks];
                    for (uint p = 0; p < LotteryConstants.Picks; p++)
		        data[p] = reader.ReadUInt();
                    incoming[id] = new RegisteredTicket(id, data);
                }

		board = reader.ReadItem();
		itemPrize = reader.ReadBool();
	      break;
            }
        }

        public override void OnDoubleClick(Mobile from) {
            if (from == null || from.Deleted)
                return; /* pedantic check */

            from.SendGump(new LotteryBasketGump(from, this));
        }

        public bool closeLottery() {
            if (mode != LotteryBasketMode.Open)
                return false;

            mode = LotteryBasketMode.Closed;

	    if ( board != null && !board.Deleted )
	      board.Name="Drawing is in progress.";
	    else
	      board = null;

            return true;
        }

        public bool drawLottery() {
            if (mode != LotteryBasketMode.Closed)
                return false;

            bool[] f = new bool[LotteryConstants.Range];
            outcome = new uint[LotteryConstants.Picks];

            Random rnd = new Random();
            for (uint i = 0; i < LotteryConstants.Picks; i++) {
                uint v;
                do {
                    v = (uint)rnd.Next(1, (int)LotteryConstants.Range);
                } while (f[v - 1]);
                f[v - 1] = true;
                outcome[i] = v;
            }

            tickets = incoming;
            incoming = new Hashtable();
            drawing++;

	    if ( debug ) { outcome = new uint[]{1,2,3,4,5,6}; }

            return true;
        }

        public bool calcAwards() {

            if (mode != LotteryBasketMode.Closed)
                return false;

	    if ( debug ) {
	      Console.WriteLine("\n+-------------------------------------------+");
	      Console.WriteLine("| LotteryBasket.cs: Debug Mode Enabled      |");
	      Console.WriteLine("| Warning! All drawings will be 1,2,3,4,5,6 |");
	      Console.WriteLine("+-------------------------------------------+\n");
	    }

            ulong[] percent = { 18, 18, 12, 52 };

            /* prepare outcome for easy counting */
            bool[] f = new bool[LotteryConstants.Range];
            for (uint i = 0; i < LotteryConstants.Picks; i++)
                f[outcome[i] - 1] = true;

            /* sort tickets by their "correct" count */
            ArrayList[] classes = new ArrayList[4];
            for (uint i = 0; i < 4; i++)
                classes[i] = new ArrayList();

            foreach (RegisteredTicket rt in tickets.Values) {
	        if ( debug ) { Console.WriteLine("checking ticket "+rt.Ticket); }
	        uint[] data = rt.Data;
	        uint correct = 0;
                for (uint i = 0; i < data.Length; i++)
                    if (f[data[i] - 1])
                        correct++;

                if (correct >= 3 && correct <= 6) {
                    classes[6 - correct].Add(rt);
                    rt.Class = (int)(6 - correct);
                } else {
                    rt.Class = -1;
                }
            }

	    for (uint wincnt = 0; wincnt < 4; wincnt++)
	        winners[wincnt] = classes[wincnt].Count;

            /* determine how much money every class gets */
            awards = new ulong[4];
	    ulong[] classAward = {0, 0, 0, 0};

            for (uint i = 0; i < 4; i++) {
                classAward[i] = (award * percent[i]) / (ulong)100;

                if (classes[i].Count == 0) {
                    awards[i] = 0;
                    jackpot[i] += classAward[i];

                } else {
                    awards[i] = (jackpot[i] + classAward[i]) / (ulong)classes[i].Count;
                    jackpot[i] = 0;
		    if ( debug ) {
		      Console.WriteLine(" - class "+i+": aw="+awards[i]);
		    }
                }
            }

	    if ( debug ) {
	      ulong sum = 0;
	      Console.WriteLine("\nInitial Values:");
	      for ( int cnt=0; cnt < 4; cnt++ ) {
		Console.WriteLine("awards["+cnt+"]: "+awards[cnt]+"     \t( x"+classes[cnt].Count+" )");
		sum += (awards[cnt] * (ulong)classes[cnt].Count);
	      }
	      Console.WriteLine("Sum: "+sum+"\n\nRecalculation:\n");
	    }

	    uint outerCnt=0;
	    uint innerCnt=0;

	    ulong shareAward=0;
	    int sharer=0;

	    bool onemore=true;
	    do {

	      onemore=false;

	      if ( debug )

	      for (outerCnt = 3; outerCnt > 0; outerCnt-- )
	      {
		for ( innerCnt = 0; innerCnt < outerCnt; innerCnt++ )
		{
		  shareAward=0;
		  sharer=0;

		  if ( awards[outerCnt] > awards[(outerCnt-innerCnt-1)] && awards[(outerCnt-innerCnt-1)] > 0)
		  {
		    shareAward = (classAward[outerCnt] + classAward[(outerCnt-innerCnt-1)]);
		    sharer = (classes[outerCnt].Count + classes[(outerCnt-innerCnt-1)].Count);

		    if ( debug ) {
		      Console.WriteLine(outerCnt+" > "+(outerCnt-innerCnt-1));
		      Console.WriteLine(awards[outerCnt]+" > "+awards[outerCnt-innerCnt-1]);
		      Console.WriteLine("Sharer: "+sharer);
		      Console.WriteLine("ShareAward: "+shareAward);
		    }
		    awards[outerCnt] = shareAward / (ulong)sharer;
		    awards[outerCnt-innerCnt-1] = shareAward / (ulong)sharer;

		    classAward[outerCnt] = (ulong)classes[outerCnt].Count * awards[outerCnt];
		    classAward[(outerCnt-innerCnt-1)] = (ulong)classes[(outerCnt-innerCnt-1)].Count * awards[outerCnt-innerCnt-1];
		    onemore = true;

		    if ( debug ) {
		      ulong sum = 0;

		      Console.WriteLine("New classAward["+outerCnt+"]:"+classAward[outerCnt]);
		      Console.WriteLine("New classAward["+(outerCnt-innerCnt-1)+"]:"+classAward[outerCnt-innerCnt-1]);

		      for ( int cnt=0; cnt < 4; cnt++ ) {
			Console.WriteLine("awards["+cnt+"]: "+awards[cnt]+"     \t( x"+classes[cnt].Count+" )");
			sum += (awards[cnt] * (ulong)classes[cnt].Count);
		      }
		      Console.WriteLine("Sum: "+sum);
		      Console.WriteLine("--------------------------------");
		    }

		  }
		  else {
		    if ( debug ) {
		      Console.WriteLine("\n"+outerCnt+" > "+(outerCnt-innerCnt-1)+" not matched\n");
		      Console.WriteLine("--------------------------------");
		    }
		  }
		}
	      }
	    } while ( onemore );

	    if ( board != null )
	      publishNumbers(outcome);

	    oldParticipants = Participants;

            return true;
        }

        public bool openLottery() {
            if (mode != LotteryBasketMode.Closed)
                return false;

            award = given = 0;

            mode = LotteryBasketMode.Open;

            return true;
        }

        public bool submitTicket(LotteryTicket ticket) {
            if (ticket == null || ticket.Deleted ||
                mode != LotteryBasketMode.Open ||
                !ticket.Writable ||
                ticket.Data == null)
                return false;

            /* register the ticket */
            ticket.Drawing = drawing;
            RegisteredTicket rt = new RegisteredTicket(ticket);
            incoming[(int)ticket.Serial] = rt;

            /* half the value of a submitted ticket goes to the
               jackpot, the other half is destroyed */
            award += LotteryConstants.TicketPrice / 2;
            destroyed += LotteryConstants.TicketPrice / 2;

            return true;
        }

        public override bool OnDragDrop(Mobile from, Item dropped) {
            if (from == null || from.Deleted ||
                dropped == null || dropped.Deleted)
                return false; /* pedantic check */

            if (!(dropped is LotteryTicket)) {
                from.SendMessage("Only lottery tickets are accepted");
                return false;
            }

            LotteryTicket ticket = (LotteryTicket)dropped;

            if (mode != LotteryBasketMode.Open) {
                from.SendMessage("The lottery is currently closed.");
                return false;
            }

            if (ticket.Data == null) {
                from.SendMessage("Please fill out your ticket first.");
                return false;
            } else if (ticket.Writable) {
                /* submit ticket */
                if (submitTicket(ticket))
                    from.SendMessage("Your lottery ticket was submitted. Good luck!");
                return false;
            } else if (ticket.Drawing == drawing) {
                from.SendMessage("This lottery ticket is already submitted. Please wait for the drawing.");
                return false;
            } else if (ticket.Drawing == drawing - 1) {
                /* hand out the award */
                RegisteredTicket rt = (RegisteredTicket)tickets[(int)ticket.Serial];
		bool mainwinning=false;
		if ( rt.Class == 0 ) {
		       mainwinning=true;
		}
                ulong award = rt == null || rt.Class == -1
                    ? 0 : awards[rt.Class];
                if (award == 0) {
                    ticket.Delete();
                    from.SendMessage("Sorry, you havn't won. Better luck next time.");
                    return false;
                } else {
		    if ( debug )
		      Console.WriteLine("Checking winners ticket: Class "+rt.Class);

		    if (!giveMoney(from, (int)award, mainwinning))
		      return false;

                    ticket.Delete();

		    if ( mainwinning && itemPrize ) {
		      from.SendMessage("Congratulations! You won the main prize! A Deed has placed in your Bagpack.");
		      destroyed += award;
		    }
		    else {
		      from.SendMessage("Congratulations! You won " + award + " gold!");
		      given += award;
		    }
                    return false;
                }
            } else {
                ticket.Delete();
                from.SendMessage("Sorry, your lottery ticket has expired.");
                return false;
            }
        }

        /** refers to a ticket which was submitted to the basket */
        private class RegisteredTicket {
            /** the serial of the ticket */
            private int ticketId;
            /** the numbers on the ticket */
            private uint[] data;
            /** the winning class, or -1 */
            private int cls = -1;

            public RegisteredTicket(LotteryTicket ticket) {
                ticketId = (int)ticket.Serial;
                data = ticket.Data;
            }
            public RegisteredTicket(int _id, uint[] _data) {
                ticketId = _id;
                data = _data;
            }

            public int Ticket {
                get {
                    return ticketId;
                }
            }
            public uint[] Data {
                get {
                    return data;
                }
            }
            public int Class {
                get {
                    return cls;
                }
                set {
                    cls = value;
                }
            }
        }
    }
}