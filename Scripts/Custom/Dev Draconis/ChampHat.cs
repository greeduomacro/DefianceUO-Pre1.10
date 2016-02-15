using System;
using Server.Items;

namespace Server.Items
{
    	public class ChampionHat : WizardsHat, IOldLowerRegCost
	{
        	[Constructable]
        	public ChampionHat() : base(1157)
		{
            		Name = "Headdress of the Champion imbued with arcanum level 5";
			Weight = 0.0;
		}

		public ChampionHat(Serial serial) : base(serial) { }

        	public int OldLowerRegCost { get { return 5; } }

		public override void Serialize(GenericWriter writer)
		{
            		base.Serialize(writer);
            		writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
		    base.Deserialize(reader);
		    int version = reader.ReadInt();
            	    Name = "Headdress of the Champion imbued with arcanum level 5";
        	}
	 }
}