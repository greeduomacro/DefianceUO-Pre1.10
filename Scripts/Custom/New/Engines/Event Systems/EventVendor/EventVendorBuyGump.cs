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
 * - Neither the name of Kai Sassmannshausen, nor the names of its
 * contributors may be used to endorse or promote products derived from
 * this software without specific prior written permission.
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND
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
 * Version 1.1
 * This script is based on PlayerVendor.cs
 * Taken from RunUO 1.0
 */

using System;
using System.Collections;
using System.Reflection;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.HuePickers;
using Server.Multis;

namespace Server.Gumps
{
	public class EventVendorBuyGump : Gump
	{
		private EventVendor m_Vendor;
		private VendorItem m_VI;

		public EventVendorBuyGump( EventVendor vendor, VendorItem vi ) : base( 100, 200 )
		{
			m_Vendor = vendor;
			m_VI = vi;

			AddBackground( 100, 10, 300, 150, 5054 );

			AddHtmlLocalized( 125, 20, 250, 24, 1019070, false, false ); // You have agreed to purchase:

			if ( vi.Description != null && vi.Description != "" )
				AddLabel( 125, 45, 0, vi.Description );
			else
				AddHtmlLocalized( 125, 45, 250, 24, 1019072, false, false ); // an item without a description

			AddHtmlLocalized( 125, 70, 250, 24, 1019071, false, false ); // for the amount of:
			AddLabel( 125, 95, 0, vi.Price.ToString() );

			AddButton( 250, 130, 4005, 4007, 0, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 282, 130, 100, 24, 1011012, false, false ); // CANCEL

			AddButton( 120, 130, 4005, 4007, 1, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 152, 130, 100, 24, 1011036, false, false ); // OKAY
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;

			if ( !m_Vendor.CanInteractWith( from, false ) )
				return;

			if ( m_Vendor.IsOwner( from ) )
			{
				m_Vendor.SayTo( from, 503212 ); // You own this shop, just take what you want.
				return;
			}

			if ( info.ButtonID == 1 )
			{
				if ( !m_VI.Valid )
				{
					m_Vendor.SayTo( from, 503216 ); // You can't buy that.
					return;
				}

				int goldInBank = 0;
				int goldInBag = 0;
				bool nodeal = false;

				if ( from.BankBox != null )
				    goldInBank = from.BankBox.GetAmount( typeof( Gold ) );

				if ( from.Backpack != null )
				goldInBag = from.Backpack.GetAmount( typeof( Gold ) );

				if ( !m_Vendor.GoldFromBank && !m_Vendor.GoldFromPack ) {
				     m_Vendor.SayTo( from,"My shop is closed at the moment. I am sorry." );
				     nodeal = true;
				}
				else if ( (goldInBank + goldInBag < m_VI.Price && m_Vendor.GoldFromBank && m_Vendor.GoldFromPack) ||
				     (goldInBag < m_VI.Price && m_Vendor.GoldFromPack && !m_Vendor.GoldFromBank) ||
				     (goldInBank < m_VI.Price && m_Vendor.GoldFromBank && !m_Vendor.GoldFromPack) )
				{
				    m_Vendor.SayTo( from, 503205 ); // You cannot afford this item.
				    nodeal = true;
				}
				else if ( !from.PlaceInBackpack( m_VI.Item ) )
				{
				    m_Vendor.SayTo( from, 503204 ); // You do not have room in your backpack for this.
				}
				else {
				    // Payment from bag, rest from bank
				    if ( m_Vendor.GoldFromPack && m_Vendor.GoldFromBank )
				    {
				        int leftPrice = m_VI.Price;

					if ( from.Backpack != null )
					    leftPrice -= from.Backpack.ConsumeUpTo( typeof( Gold ), leftPrice );

					if ( leftPrice > 0 && from.BankBox != null )
					    from.BankBox.ConsumeUpTo( typeof( Gold ), leftPrice );
				    }
				    // Payment from bag
				    else if ( m_Vendor.GoldFromPack && !m_Vendor.GoldFromBank ) {
				        from.Backpack.ConsumeUpTo( typeof( Gold ), m_VI.Price );

				    }
				    // Payment from bank
				    else if ( m_Vendor.GoldFromBank && !m_Vendor.GoldFromPack ) {
				        from.BankBox.ConsumeUpTo( typeof( Gold ), m_VI.Price );
				    }

				    // If the player is fine, and can afford the item, give out the item
				    if ( !nodeal && !from.Deleted && from.Account != null ) {
				        m_Vendor.HoldGold += m_VI.Price;
					from.SendLocalizedMessage( 503201 ); // You take the item.
				    }
				}
			}
			else
			{
				from.SendLocalizedMessage( 503207 ); // Cancelled purchase.
			}
		}
	}

}