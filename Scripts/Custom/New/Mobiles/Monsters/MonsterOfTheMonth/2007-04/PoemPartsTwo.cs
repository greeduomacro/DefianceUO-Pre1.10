namespace Server.Items
{
 	public class PoemPartsTwo : Item
 	{
		[Constructable]
  		public PoemPartsTwo() : base( 0x14F0 )
  		{
  			switch ( Utility.Random( 11 ) )
				{
					case 0: Name = "It can be smooth"; break;
					case 1: Name = "Or riddled with grooves"; break;
					case 2: Name = "And is sometimes the most efficient to use"; break;
					case 3: Name = "When you have nothing else to choose"; break;
					case 4: Name = "It can take you to the mountains up high"; break;
					case 5: Name = "But not close enough to touch the sky"; break;
					case 6: Name = "Even though you feel like you could fly"; break;
					case 7: Name = "But this feeling is false"; break;
					case 8: Name = "As the road is filled with flaws"; break;
					case 9: Name = "And gives us just cause"; break;
					case 10: Name = "To be more aware and have laws"; break;
				}
  			Weight = 2.0;
  			Hue = Utility.RandomBlueHue();
  		}

  		public PoemPartsTwo(Serial serial) : base(serial)
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