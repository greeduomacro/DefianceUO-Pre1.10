namespace Server.Items
{
 	public class PoemParts : Item
 	{
		[Constructable]
  		public PoemParts() : base( 0x14F0 )
  		{
  			switch ( Utility.Random( 18 ) )
				{
					case 0: Name = "Through a marriage of the sun and of the sky"; break;
					case 1: Name = "It forms and starts to fly"; break;
					case 2: Name = "From humble beginnings way up high"; break;
					case 3: Name = "No one knows how this will end"; break;
					case 4: Name = "Or even how that it begins"; break;
					case 5: Name = "Eventhough most of it is spent with friends"; break;
					case 6: Name = "Its not the end of the world but that is how it seems"; break;
					case 7: Name = "When you hit the ground runoff and meet a stream"; break;
					case 8: Name = "But now there is time to think and dream"; break;
					case 9: Name = "Just when you think tranquility is the only way"; break;
					case 10: Name = "An eagle takes a fish and flies away"; break;
					case 11: Name = "One must follow on this day"; break;
					case 12: Name = "Only one of many on its girth"; break;
					case 13: Name = "The fish struggles for all it is worth"; break;
					case 14: Name = "Then one tumbles to the earth"; break;
					case 15: Name = "But no one is there for you to meet"; break;
					case 16: Name = "You just whither away from the heat"; break;
					case 17: Name = "To start a new and repeat"; break;
				}
  			Weight = 2.0;
  			Hue = Utility.RandomRedHue();
  		}

  		public PoemParts(Serial serial) : base(serial)
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