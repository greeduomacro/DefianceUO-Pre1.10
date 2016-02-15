using System;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x141B, 0x141C )]
	public class OrcMask : BaseHat
	{
		[Constructable]
		public OrcMask() : base( 0x141B )
		{
		}

		public OrcMask( Serial serial ) : base ( serial )
		{
		}

		public override bool Dye( Mobile from, DyeTub sender )
		{
			from.SendMessage( "You can't dye that." );
			return false;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}