using System;
using Server.Items;

namespace Server.Items
{
	public class MinaxRobe : BaseOuterTorso
	{

		[Constructable]
		public MinaxRobe() : base( 0x1f03 )
		{
			Hue = 38;
			Weight = 1;
			Name = "robe";
		}

		public MinaxRobe( Serial serial ) : base( serial )
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