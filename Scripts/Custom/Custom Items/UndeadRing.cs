using System;
using Server;

namespace Server.Items
{
	public class UndeadRing : SilverRing
	{
		[Constructable]
		public UndeadRing()
		{
			Hue = 0x21;
			Name = "Undead Ring";
                        LootType = LootType.Blessed;

                }

		public UndeadRing( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}