using Server;
using Server.Items;

public class FactionParchament : Item
{
	[Constructable]
	public FactionParchament() : base(8792)
	{
		Weight = 1.0;
		Name = "a Parchament, I joined Britannia at War.";
	}

	public FactionParchament(Serial serial) : base(serial)
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