/*
 * Copyright (c) 2005, Kai Sassmannshausen <kai@sassie.org>
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
 * - Neither the name of Kai Sassmannshausen  nor the
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
 */

using System;
using System.Collections;
using Server.Items;
using Server.Network;
using Server.Misc;

namespace Server.Gumps {

  public class LotteryStatisticsGump : Gump {
    private LotteryBasket basket;

    public LotteryStatisticsGump(Mobile from, LotteryBasket _basket) : base (100,100) {
      basket = _basket;

      if ( !from.Deleted && from != null && !basket.Deleted && basket != null ) {

	int height = 550;
	int lenght = 550;

	uint[] numbers = basket.Outcome;

	if ( numbers == null ) {
	    height = 385;
	    lenght = 450;
	}

	AddBackground(0, 0, lenght, height, 2600);

	int y = 25;
	AddLabel(45, y, 133, "Defiance Lottery");
	y += 15;
	AddLabel(45, y, 133, "GM Menu - Statistics");
	y += 40;

	AddLabel(45, y, 2213, "Global statistics");
	y += 20;
	AddLabel(60, y, 2215, "Gold destroyed: "+basket.Destroyed);
	y += 20;
	AddLabel(60, y, 2215, "Gold given out: "+basket.Given);

	y += 30;
	AddLabel(45, y, 2213, "Drawing statistics");
	y += 25;
	AddLabel(60, y, 2213, "Surrent drawing");
	y += 20;
	AddLabel(60, y, 2215, "Drawing ID: "+basket.Drawing);

	ulong jp = basket.Award;
	foreach (ulong v in basket.Jackpot)
	  jp += v;
	y += 20;
	AddLabel(60, y, 2215, "Jackpot: "+jp);
	y += 20;
	AddLabel(75, y, 2215, "Ticket sales revenue: "+basket.Award);


	ulong[] jps = new ulong[4]{basket.Jackpot6, basket.Jackpot5, basket.Jackpot4, basket.Jackpot3};
	for ( uint cnt = 0; cnt < 4; cnt++ ) {
	    y += 20;
	    AddLabel(75, y, 2215, "Jackpot "+(6-cnt)+"-Rights: "+jps[cnt]);
	}

	y += 20;
	AddLabel(60, y, 2215, "Submitted tickets until now: "+basket.Participants);

	if ( numbers != null ) {
	  y += 30;
	  AddLabel(60, y, 2213, "Past drawing");
	  y += 20;

	  AddLabel(60, y, 2215, "Drawing ID: "+(basket.Drawing-1));
	  y += 20;

	  string outcome = "";
	  Array.Sort(numbers);
	  string[] text = new string[6];
	  for (int z = 0; z < 6; z++)
	    text[z] = "" + numbers[z];
	  outcome = string.Join(", ", text);
	  AddLabel(60, y, 2215, "Drawn numbers: "+outcome);

	  for ( uint cnt = 0; cnt < 4; cnt++ ) {
	      y += 20;

	      if ( basket.Awards[cnt] == 0 )
		  text[cnt] = "n/a";
	      else
		  text[cnt] = ""+basket.Awards[cnt];

	      AddLabel(60, y, 2215, "Win with "+(6-cnt)+" rights: "+text[cnt]);

	      if ( basket.Winners[cnt] > 0 )
	        AddLabel(400, y, 2215, "Winners: "+basket.Winners[cnt]);
	  }

	  y += 20;
	  AddLabel(60, y, 2215, "Participants: "+basket.OldParticipants);
	}
      }
    }
  }
}