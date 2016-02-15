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

using Server.Items;
using Server.Network;
using Server.Misc;
using Server.Targeting;

namespace Server.Gumps {

  public class LotteryGMGump : Gump {
    private LotteryBasket basket;

    public LotteryGMGump(Mobile from, LotteryBasket _basket) : base (100,100) {

      basket = _basket;
      if ( !from.Deleted && from != null && !basket.Deleted && basket != null ) {

	if (from.AccessLevel == AccessLevel.Counselor)
	  AddBackground(0, 0, 420, 200, 2600);
	else
	  AddBackground(0, 0, 420, 400, 2600);

	int y = 25;
	AddLabel(45, y, 133, "Defiance Lottery");
	y += 15;
	AddLabel(45, y, 133, "GM Menu");
	y += 20;
	if ( basket.Debug )
	    AddLabel(150, y, 133, "!!! DEBUG MODE ENABLED !!!");
	y += 25;

	AddLabel(45, y, 2213, "Statistics");
	y += 30;
	AddButton(45, y, 2152, 2154, 5, GumpButtonType.Reply, 0);
	AddLabel(90, y, 902, "Open statistics gump");

	if (from.AccessLevel >= AccessLevel.GameMaster) {
	  y += 50;

	  AddLabel(45, y, 2213, "Settings");
	  y += 30;
	  AddButton(45, y, 2152, 2154, 7, GumpButtonType.Reply, 0);
	  AddLabel(90, y, 902, "Item prize true/false");
	  AddLabel(230, y, 902, "Currently: "+basket.ItemPrize);
	  y += 30;
	  AddLabel(90, y, 902, "Current publishing Item: ");
	  y += 15;
	  if ( basket.Board != null )
	      AddLabel(90, y, 902, ""+basket.Board);
	  else
	      AddLabel(90, y, 902, "none");
	  y += 15;
	  AddLabel(90, y, 902, "(change this via [props)");
	  y += 50;
	  AddLabel(45, y, 2213, "Preform a drawing");
	  y += 30;
	  if (basket.Mode == LotteryBasketMode.Open) {
	    AddButton(45, y, 2152, 2154, -1, GumpButtonType.Reply, 0);
	    AddLabel(90, y, 902, "Close lottery and draw new numbers (GM)");
	  } else if (basket.Mode == LotteryBasketMode.Closed) {
	    AddButton(45, y, 2152, 2154, -2, GumpButtonType.Reply, 0);
	    AddLabel(90, y, 902, "Publish new drawing and re-open lottery");
	  }
	  y += 20;
	  AddLabel(90, y, 902, "Attention: This can not be undone!");
	}
      }
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
      case -1:
	basket.closeLottery();
	basket.drawLottery();
	break;
      case -2:
	basket.calcAwards();
	basket.openLottery();
	break;
      case 5:
	sender.Mobile.SendGump(new LotteryStatisticsGump(sender.Mobile, basket));
	break;
      case 7:
	basket.ItemPrize = !basket.ItemPrize;
	break;
      }
    }
  }
}