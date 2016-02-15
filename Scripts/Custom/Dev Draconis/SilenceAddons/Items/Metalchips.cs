namespace Server.Items
{
    public class MetalChips : BaseEarrings
    {
        [Constructable]
        public MetalChips() : base( 0x1087 )
        {
            Name = "Metalchips of Silence";
            Weight = 1.0;
            Hue = 1000;
            LootType = LootType.Blessed;
        }

        public MetalChips(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}