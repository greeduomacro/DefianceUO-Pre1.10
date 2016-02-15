using System;
using Server.Items;

namespace Server.Items
{
	public class ComRobe : BaseOuterTorso
	{

		[Constructable]
		public ComRobe() : base( 0x1f03 )
		{
			Hue = 54;
			Weight = 1;
			Name = "robe";
		}

		public ComRobe( Serial serial ) : base( serial )
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