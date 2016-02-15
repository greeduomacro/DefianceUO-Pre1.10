using System;

namespace Server.Items
{
	public class RexxarRare : Item
	{
		[Constructable]
		public RexxarRare() : base( 0x116A )
		{
			Weight = 9.0;
			Name = "Murderer's tombstone";
		}

		public RexxarRare(Serial serial) : base(serial)
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
}