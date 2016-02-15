namespace Server.Items
{
 	public class PoemPartsThree : Item
 	{
		[Constructable]
  		public PoemPartsThree() : base( 0x14F0 )
  		{
  			switch ( Utility.Random( 17 ) )
				{
					case 0: Name = "As it sits and counts the days "; break;
					case 1: Name = "In one place it always stays"; break;
					case 2: Name = "While some things change in many ways"; break;
					case 3: Name = "One stands still while another plays"; break;
					case 4: Name = "The oak is covered in a stain"; break;
					case 5: Name = "Without this it would be plain"; break;
					case 6: Name = "And it must endure more pain"; break;
					case 7: Name = "Other parts could rust in the rain"; break;
					case 8: Name = "But the wood is strong and would remain"; break;
					case 9: Name = "This case protects all sorts of things"; break;
					case 10: Name = "Intricate parts like the springs"; break;
					case 11: Name = "And all the sounds they bring"; break;
					case 12: Name = "Make something plain seem to sing"; break;
					case 13: Name = "And when time breaks a gear"; break;
					case 14: Name = "From the stress of days and of years"; break;
					case 15: Name = "It depends more on the one that's near"; break;
					case 16: Name = "So that even at night the hours still cheer"; break;
				}
  			Weight = 2.0;
  			Hue = Utility.RandomGreenHue();
  		}

  		public PoemPartsThree(Serial serial) : base(serial)
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