using System;
using Server;

namespace Server.Items
{
	public class SpidersSilkWeb : Item
	{
		[Constructable]
		public SpidersSilkWeb() : base( Utility.RandomList( 0x10D2, 0x10D3, 0x10D4, 0x10D5, 0x10D6, 0x10D7 ) )
		{
			Weight = 1.0;
			Name = "a spider web";
			Movable = false;
		}

		public SpidersSilkWeb( Serial serial ) : base( serial )
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
		    int pick = Utility.Random( 1,3 );
			SpidersSilk crop = new SpidersSilk( pick );
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