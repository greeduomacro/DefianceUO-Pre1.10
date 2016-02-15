using System;
using Server;

namespace Server.Items
{
	public class GinsengPlant : Item
	{
		[Constructable]
		public GinsengPlant() : base( Utility.RandomList( 0x18EA, 0x18E9 ) )
		{
			Weight = 1.0;
			Name = "Ginseng Plant";
			Movable = false;
		}

		public GinsengPlant( Serial serial ) : base( serial )
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
			Ginseng crop = new Ginseng( pick );
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