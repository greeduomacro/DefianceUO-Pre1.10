using System;
using Server.Items;

namespace Server.Items
{
	public class PayRegStone : Item
	{
		[Constructable]
		public PayRegStone() : base( 0xED4 )
		{
			Movable = false;
			Hue = 1109;
			Name = "a reagent stone cost 2400gp per 50 each reg";
		}

		public override void OnDoubleClick( Mobile from )
		{
            //al: fix for missing range check
			if ( !from.InRange( GetWorldLocation(), 2 ) )
			{
				from.SendLocalizedMessage( 500446 ); // That is too far away.
			}
			else if ( from.BankBox.ConsumeTotal( typeof( Gold ), 2400 ) )
			{
                //Al: Moved Item creation here so that no internal waste items are created
                BagOfReagents regBag = new BagOfReagents(50);
                if (!from.AddToBackpack(regBag))
					regBag.Delete();
			}
			else
		  	{
			  	from.SendMessage( "Your do not have enough gold." );
		  	}
		  	return;
		}

		public PayRegStone( Serial serial ) : base( serial )
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
}