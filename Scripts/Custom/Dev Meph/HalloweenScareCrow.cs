namespace Server.Items
{
	public class HalloweenScareCrow : Item
	{
		[Constructable]
		public HalloweenScareCrow() : this( 0x1E34 )
		{
		}

		[Constructable]
		public HalloweenScareCrow( int itemID ) : base( itemID )
		{
			Movable = true;
			Weight = 25;
			Name = "a halloween scarecrow";

		}

		public HalloweenScareCrow( Serial serial ) : base( serial )
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