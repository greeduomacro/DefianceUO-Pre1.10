namespace Server.Items
{
 	public class DecemberDeed : Item
 	{
		[Constructable]
  		public DecemberDeed() : base( 0x2258 )
  		{
  			Name = "a December prize deed";
  			Weight = 2.0;
  			Hue = 1150;
			Movable = false;
  		}

  		public DecemberDeed(Serial serial) : base(serial)
  		{
  		}

  		public override void Serialize(GenericWriter writer)
  		{
  			base.Serialize(writer);
 			writer.Write((int)0);
  		}

  		public override void Deserialize(GenericReader reader)
  		{
  			base.Deserialize(reader);
  			int version = reader.ReadInt();
  		}
  	}
}