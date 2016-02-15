using System;
using Server;

namespace Server.Items
{
	public class SulfurousAshRock : Item
	{
		[Constructable]
		public SulfurousAshRock() : base( Utility.RandomList( 0x1773, 0x1774, 0x1777 ) )
		{
			Weight = 1.0;
			Name = "sulfur";
			Hue =  353;
			Movable = false;
		}

		public SulfurousAshRock( Serial serial ) : base( serial )
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
			SulfurousAsh crop = new SulfurousAsh( pick );
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