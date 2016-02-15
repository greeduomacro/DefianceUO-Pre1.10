using System;
using Server;

namespace Server.Items
{
	public class BloodMossPlant : Item
	{
		[Constructable]
		public BloodMossPlant() : base( Utility.RandomList( 0x1F0D, 0x1F11 ) )
		{
			Weight = 1.0;
			Name = "bloodmoss";
			Movable = false;
			Hue = 38;
		}

		public BloodMossPlant( Serial serial ) : base( serial )
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
			Bloodmoss crop = new Bloodmoss( pick );
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