using System;
using Server;

namespace Server.Items
{
	public class GarlicPlant : Item
	{
		[Constructable]
		public GarlicPlant() : base( Utility.RandomList( 0x18E1, 0x18E2 ) )
		{
			Weight = 1.0;
			Name = "a garlic plant";
			Movable = false;
		}

		public GarlicPlant( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
		public override void OnDoubleClick( Mobile from )
		{
		    if ( from.InRange( this.GetWorldLocation(), 2 ) )
		    {
		    int pick = Utility.Random( 2,4 );
			Garlic crop = new Garlic( pick );
			from.AddToBackpack( crop );
			this.Delete();
		    }
		    else
		    {
			from.SendMessage( "You are too far away to harvest anything." );
		    }
		}

	}
}