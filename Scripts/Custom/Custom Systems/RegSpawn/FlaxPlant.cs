using System;
using Server;

namespace Server.Items
{
	public class FlaxPlant : Item
	{
		[Constructable]
		public FlaxPlant() : base( Utility.RandomList( 0x1A99, 0x1A9A, 0x1A9B ) )
		{
			Weight = 1.0;
			Name = "Flax Plant";
			Movable = false;
		}

		public FlaxPlant( Serial serial ) : base( serial )
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
			Flax crop = new Flax( pick );
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