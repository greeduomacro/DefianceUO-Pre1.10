using Server;
using Server.Items;

public class AncientSigil : Item
{
	[Constructable]
	public AncientSigil() : base(3685)
	{
		Weight = 2.0;
		Name = "an ancient sigil";
	}

	public AncientSigil(Serial serial) : base(serial)
	{
	}

	public override void Serialize(GenericWriter writer)
	{
		base.Serialize(writer);

		writer.Write((int) 0);
	}

	public override void Deserialize(GenericReader reader)
	{
		base.Deserialize(reader);

		int version = reader.ReadInt();
	}
}