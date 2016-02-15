using System;
using Server.Items;

namespace Server.Items
{
	public class RegStone : Item
	{
		[Constructable]
		public RegStone() : base( 0xED4 )
		{
			Movable = false;
			Hue = 0x2D1;
			Name = "a reagent stone";
		}

		public override void OnDoubleClick( Mobile from )
		{
            //Al:Added range and pedantic visibility check.
            if ( !from.InRange( GetWorldLocation(), 4 ) )
			{
				from.SendLocalizedMessage( 500446 ); // That is too far away.
			}
            else if ( (!this.Visible) || !from.InLOS(this.GetWorldLocation()) )
            {
                from.SendLocalizedMessage(502800); // You can't see that.
            }
            else
            {
                BagOfReagents regBag = new BagOfReagents( 50 );
			    if ( !from.AddToBackpack( regBag ) )
				    regBag.Delete();
            }
		}

		public RegStone( Serial serial ) : base( serial )
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