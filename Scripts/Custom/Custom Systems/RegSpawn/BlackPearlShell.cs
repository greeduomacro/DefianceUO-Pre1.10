using System;
using Server;

namespace Server.Items
{
	public class BlackPearlShell : Item
	{
		[Constructable]
		public BlackPearlShell() : base( Utility.RandomList( 0xFC7, 0xFC4, 0xFCB, 0xFCC ) )
		{
			Weight = 1.0;
			Name = "a sea shell";
			Movable = false;
		}

		public BlackPearlShell( Serial serial ) : base( serial )
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
				BlackPearl crop = new BlackPearl( pick );
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