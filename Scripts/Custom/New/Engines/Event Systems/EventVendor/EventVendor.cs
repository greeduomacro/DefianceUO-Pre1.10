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
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Prompts;
using Server.Targeting;
using Server.Misc;
using Server.Multis;
using Server.ContextMenus;

namespace Server.Mobiles
{

    public class EventVendor : PlayerVendor
    {
        private EventVendorPlaceholder m_Placeholder;

        private bool fromBag;
        private bool fromBank;

        public new EventVendorPlaceholder Placeholder
        {
	    get{ return m_Placeholder; }
	    set{ m_Placeholder = value; }
	}

        [CommandProperty( AccessLevel.GameMaster )]
	public bool GoldFromPack
        {
	    get{ return fromBag; }
	    set{ fromBag = value; }
	}

        [CommandProperty( AccessLevel.GameMaster )]
	public bool GoldFromBank
        {
	    get{ return fromBank; }
	    set{ fromBank = value; }
	}

        [Constructable]
	public EventVendor() : base(null, null)
        {
	    // Initial Parameters
	    fromBag = true;
	    fromBank = true;
	    BankAccount = 500000;
	    Blessed = true;
	}

        public EventVendor ( Serial serial ) : base( serial )
        {
	}

        public new static void TryToBuy( Item item, Mobile from )
        {
	    EventVendor vendor = item.RootParent as EventVendor;

	    if ( vendor == null || !vendor.CanInteractWith( from, false ) )
	        return;

	    if ( vendor.IsOwner( from ) )
	    {
	        vendor.SayTo( from, 503212 ); // You own this shop, just take what you want.
		return;
	    }

	    VendorItem vi = vendor.GetVendorItem( item );

	    if ( vi == null )
	    {
	        vendor.SayTo( from, 503216 ); // You can't buy that.
	    }
	    else if ( !vi.IsForSale )
	    {
	        vendor.SayTo( from, 503202 ); // This item is not for sale.
	    }
	    else if ( vi.Created + TimeSpan.FromMinutes( 1.0 ) > DateTime.Now )
	    {
	        from.SendMessage( "You cannot buy this item right now.  Please wait one minute and try again." );
	    }
	    else
	    {
	        from.CloseGump( typeof( EventVendorBuyGump ) );
	        from.SendGump( new EventVendorBuyGump( vendor, vi ) );
	    }
	}


        public override void OnSpeech( SpeechEventArgs e )
        {
	    Mobile from = e.Mobile;

	    if ( e.Handled || !from.Alive || from.GetDistanceToSqrt( this ) > 3 )
	        return;

	    if ( e.HasKeyword( 0x3C ) || (e.HasKeyword( 0x171 ) && WasNamed( e.Speech ))  ) // vendor buy, *buy*
	    {
		if ( IsOwner( from ) )
		{
		    SayTo( from, 503212 ); // You own this shop, just take what you want.
		}
		else if ( House == null || !House.IsBanned( from ) )
		{
		    from.SendLocalizedMessage( 503213 ); // Select the item you wish to buy.
		    from.Target = new EVBuyTarget();

		    e.Handled = true;
		}
	    }
	    else if ( e.HasKeyword( 0x3D ) || (e.HasKeyword( 0x172 ) && WasNamed( e.Speech )) ) // vendor browse, *browse
	    {
	        if ( House != null && House.IsBanned( from ) && !IsOwner( from ) )
		{
		    SayTo( from, 1062674 ); // You can't shop from this home as you have been banned from this establishment.
		}
		else
		{
		    OpenBackpack( from );
		    e.Handled = true;
		}
	    }
	    else if ( e.HasKeyword( 0x3E ) || (e.HasKeyword( 0x173 ) && WasNamed( e.Speech )) ) // vendor collect, *collect
	    {
	        if ( IsOwner( from ) )
		{
		    CollectGold( from );
		    e.Handled = true;
		}
	    }
	    else if ( e.HasKeyword( 0x3F ) || (e.HasKeyword( 0x174 ) && WasNamed( e.Speech )) ) // vendor status, *status
	    {
	        if ( IsOwner( from ) )
		{
		    SendOwnerGump( from );

		    e.Handled = true;
		}
		else
		{
		    SayTo( from, 503226 ); // What do you care? You don't run this shop.
		}
	    }
	    else if ( e.HasKeyword( 0x40 ) || (e.HasKeyword( 0x175 ) && WasNamed( e.Speech )) ) // vendor dismiss, *dismiss
	    {
	        if ( IsOwner( from ) )
		{
	            Dismiss( from );
		    e.Handled = true;
		}
	    }
	    else if ( e.HasKeyword( 0x41 ) || (e.HasKeyword( 0x176 ) && WasNamed( e.Speech )) ) // vendor cycle, *cycle
	    {
	        if ( IsOwner( from ) )
		{
		    this.Direction = this.GetDirectionTo( from );
		    e.Handled = true;
		}
	    }
	}

        public override void InitOutfit()
        {
	    Item v_skirt = new Skirt ( 1 );
	    v_skirt.LootType = LootType.Blessed;
	    AddItem ( v_skirt );

	    Item v_tunic = new Tunic ( 1 );
	    v_tunic.LootType = LootType.Blessed;
	    AddItem ( v_tunic );

	    Item v_halfapron = new HalfApron ( 1360 );
	    v_halfapron.LootType = LootType.Blessed;
	    AddItem ( v_halfapron );

	    Item v_sash = new BodySash ( 1360 );
	    v_sash.LootType = LootType.Blessed;
	    v_sash.Layer = Layer.Helm;
	    AddItem ( v_sash );

	    Item v_sandals = new Sandals ( 1360 );
	    v_sandals.LootType = LootType.Blessed;
	    AddItem ( v_sandals );

	    Item hair = new Item( Utility.RandomList( 0x203B, 0x2049, 0x2048, 0x204A ) );
	    hair.Hue = 1360;
	    hair.Layer = Layer.Hair;
	    hair.Movable = false;
	    AddItem( hair );

	    Container pack = new EventVendorBackpack();
	    pack.Movable = false;
	    AddItem( pack );
	}

        [PlayerVendorTarget]
	private class EVBuyTarget : Target
	{
	    public EVBuyTarget() : base( 3, false, TargetFlags.None )
	  {
	      AllowNonlocal = true;
	  }

	  protected override void OnTarget( Mobile from, object targeted )
	  {
	      if ( targeted is Item )
	      {
		  EventVendor.TryToBuy( (Item) targeted, from );
	      }
	  }
	}

        public new bool CheckTeleport( Mobile to )
        {
	    if ( Deleted || !IsOwner( to ) || House == null || this.Map == Map.Internal )
	        return false;

	    if ( House.IsInside( to ) || to.Map != House.Map || !House.InRange( to, 5 ) )
	        return false;

	    if ( Placeholder == null )
	    {
	        Placeholder = new EventVendorPlaceholder( this );
		Placeholder.MoveToWorld( this.Location, this.Map );

		this.MoveToWorld( to.Location, to.Map );

		to.SendLocalizedMessage( 1062431 );
		// This vendor has been moved out of the house to your current location temporarily.
		// The vendor will return home automatically after two minutes have passed once you are
		// done managing its inventory or customizing it.
	    }
	    else
	    {
	        Placeholder.RestartTimer();

		to.SendLocalizedMessage( 1062430 );
		// This vendor is currently temporarily in a location outside its house.
		// The vendor will return home automatically after two minutes have passed
		// once you are done managing its inventory or customizing it.
	    }

	    return true;
	}

        public override void Serialize( GenericWriter writer )
        {
	    base.Serialize( writer );

	    writer.Write( (int) 1 ); // version
	    writer.Write( fromBag );
	    writer.Write( fromBank );
	}

        public override void Deserialize( GenericReader reader )
        {
	    base.Deserialize( reader );

	    int version = reader.ReadInt();
	    switch (version)
	    {
	        case 1:
		{
		    fromBag = reader.ReadBool();
		    fromBank = reader.ReadBool();
		    break;
		}
	    }
	}
    }

    public class EventVendorBackpack : Backpack
    {
        public EventVendorBackpack()
        {
	    Layer = Layer.Backpack;
	    Weight = 1.0;
	}

        public override int DefaultMaxWeight{ get{ return 0; } }

        public override bool CanStore( Mobile m )
        {
	    EventVendor parent = this.Parent as EventVendor;

	    if ( parent != null )
	        return parent.IsOwner( m );

	    return base.CanStore( m );
	}

        public override bool CheckHold( Mobile m, Item item, bool message, bool checkItems, int plusItems, int plusWeight )
        {
	    if ( !base.CheckHold( m, item, message, checkItems, plusItems, plusWeight ) )
	        return false;

	    if ( !BaseHouse.NewVendorSystem && Parent is EventVendor )
	    {
	        BaseHouse house = ((EventVendor)Parent).House;

		if ( house != null && house.IsAosRules && !house.CheckAosStorage( 1 + item.TotalItems + plusItems ) )
		{
		    if ( message )
		        m.SendLocalizedMessage( 1061839 ); // This action would exceed the secure storage limit of the house.

		    return false;
		}
	    }

	    return true;
	}

        public override bool IsAccessibleTo( Mobile m )
        {
	    return true;
	}

        public override bool CheckItemUse( Mobile from, Item item )
        {
	    if ( !base.CheckItemUse( from, item ) )
	        return false;

	    if ( item is Container || item is Engines.BulkOrders.BulkOrderBook )
	        return true;

	    from.SendLocalizedMessage( 500447 ); // That is not accessible.
	    return false;
	}

        public override bool CheckTarget( Mobile from, Target targ, object targeted )
        {
	    if ( !base.CheckTarget( from, targ, targeted ) )
	        return false;

	    if ( from.AccessLevel >= AccessLevel.GameMaster )
	        return true;

	    return targ.GetType().IsDefined( typeof( PlayerVendorTargetAttribute ), false );
	}

        public override void GetChildContextMenuEntries( Mobile from, ArrayList list, Item item )
        {
	    base.GetChildContextMenuEntries( from, list, item );

	    EventVendor pv = RootParent as EventVendor;

	    if ( pv == null || pv.IsOwner( from ) )
	        return;

	    VendorItem vi = pv.GetVendorItem( item );

	    if ( vi != null )
	        list.Add( new BuyEntry( item ) );
	}

        private class BuyEntry : ContextMenuEntry
	{
	    private Item m_Item;

	    public BuyEntry( Item item ) : base( 6103 )
	    {
	       m_Item = item;
	    }

	    public override bool NonLocalUse{ get{ return true; } }

	    public override void OnClick()
	    {
	        if ( m_Item.Deleted )
		    return;

		EventVendor.TryToBuy( m_Item, Owner.From );
	    }
	}

        public override void GetChildNameProperties( ObjectPropertyList list, Item item )
        {
	    base.GetChildNameProperties( list, item );

	      EventVendor pv = RootParent as EventVendor;

	      if ( pv == null )
		  return;

	      VendorItem vi = pv.GetVendorItem( item );

	      if ( vi == null )
		  return;

	      if ( !vi.IsForSale )
		  list.Add( 1043307 ); // Price: Not for sale.
	      else if ( vi.IsForFree )
		  list.Add( 1043306 ); // Price: FREE!
	      else
		  list.Add( 1043304, vi.Price.ToString() ); // Price: ~1_COST~
	}

        public override void GetChildProperties( ObjectPropertyList list, Item item )
        {
	    base.GetChildProperties( list, item );

	    EventVendor pv = RootParent as EventVendor;

	    if ( pv == null )
	        return;

	    VendorItem vi = pv.GetVendorItem( item );

	    if ( vi != null && vi.Description != null && vi.Description.Length > 0 )
	        list.Add( 1043305, vi.Description ); // <br>Seller's Description:<br>"~1_DESC~"
	}

        public override void OnSingleClickContained( Mobile from, Item item )
        {
	    if ( RootParent is EventVendor )
	    {
	        EventVendor vendor = (EventVendor)RootParent;

		VendorItem vi = vendor.GetVendorItem( item );

		if ( vi != null )
		{
		    if ( !vi.IsForSale )
		        item.LabelTo( from, 1043307 ); // Price: Not for sale.
		    else if ( vi.IsForFree )
		        item.LabelTo( from, 1043306 ); // Price: FREE!
		    else
		        item.LabelTo( from, 1043304, vi.Price.ToString() ); // Price: ~1_COST~

		    if ( vi.Description != null && vi.Description != "" )
		    {
		        // The localized message (1043305) is no longer valid - <br>Seller's Description:<br>"~1_DESC~"
		        item.LabelTo( from, "Description: {0}", vi.Description );
		    }
		}
	    }

	    base.OnSingleClickContained( from, item );
	}

        public EventVendorBackpack( Serial serial ) : base( serial )
        {
	}

        public override void Serialize( GenericWriter writer )
        {
	  base.Serialize( writer );

	  writer.Write( (int) 0 ); // version
	}

        public override void Deserialize( GenericReader reader )
        {
	    base.Deserialize( reader );

	    int version = reader.ReadInt();
	}
    }

    public class EventVendorPlaceholder : Item
    {
        private EventVendor m_Vendor;
        private ExpireTimer m_Timer;

        [CommandProperty( AccessLevel.GameMaster )]
	public EventVendor Vendor{ get{ return m_Vendor; } }

        public EventVendorPlaceholder( EventVendor vendor ) : base( 0x1F28 )
        {
	    Hue = 0x672;
	    Movable = false;

	    m_Vendor = vendor;

	    m_Timer = new ExpireTimer( this );
	    m_Timer.Start();
	}

        public EventVendorPlaceholder( Serial serial ) : base( serial )
        {
	}

        public override void GetProperties( ObjectPropertyList list )
        {
	    base.GetProperties( list );

	    if ( m_Vendor != null )
	        list.Add( 1062498, m_Vendor.Name ); // reserved for vendor ~1_NAME~
	}

        public void RestartTimer()
        {
	    m_Timer.Stop();
	    m_Timer.Start();
	}

        private class ExpireTimer : Timer
	{
	    private EventVendorPlaceholder m_Placeholder;

	    public ExpireTimer( EventVendorPlaceholder placeholder ) : base( TimeSpan.FromMinutes( 2.0 ) )
	    {
	        m_Placeholder = placeholder;

		Priority = TimerPriority.FiveSeconds;
	    }

	    protected override void OnTick()
	    {
	        m_Placeholder.Delete();
	    }
	}

        public override void OnDelete()
        {
	    if ( m_Vendor != null && !m_Vendor.Deleted )
	    {
	        m_Vendor.MoveToWorld( this.Location, this.Map );
		m_Vendor.Placeholder = null;
	    }
	}

        public override void Serialize( GenericWriter writer )
        {
	    base.Serialize( writer );

	    writer.WriteEncodedInt( (int) 0 );

	    writer.Write( (Mobile) m_Vendor );
	}

        public override void Deserialize( GenericReader reader )
        {
	    base.Deserialize( reader );

	    int version = reader.ReadEncodedInt();

	    m_Vendor = (EventVendor) reader.ReadMobile();

	    Timer.DelayCall( TimeSpan.Zero, new TimerCallback( Delete ) );
	}
    }
}