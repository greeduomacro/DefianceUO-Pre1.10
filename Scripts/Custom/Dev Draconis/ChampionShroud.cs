using System;
using Server.Items;

namespace Server.Items
{
    public class ChampionShroud : HoodedShroudOfShadows, IOldLowerRegCost
    {
	[Constructable]
        public ChampionShroud() : base(1157)
        {
	    Name = "Shroud of the Champion imbued with arcanum level 10";
            Weight = 1.0;
        }

	public int OldLowerRegCost{ get{ return 10; } }

        public ChampionShroud(Serial serial) : base(serial) { }

	public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
	    Name = "Shroud of the Champion imbued with arcanum level 10";
	}
    }
}