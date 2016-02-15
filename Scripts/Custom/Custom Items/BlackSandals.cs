using System;
using Server;
using Server.Items;

namespace Server.Items
{

[FlipableAttribute( 0x170d, 0x170e )]
	public class BlackSandals : BaseShoes
	{
		[Constructable]
		public BlackSandals() : this( 0 )
		{
		}

		[Constructable]
		public BlackSandals( int hue ) : base( 0x170D, hue )
		{
			Weight = 1.0;
		        Hue = 1;
                        Name = "Quest Sandals";

                }

		public BlackSandals( Serial serial ) : base( serial )
		{
		}

		public override bool Dye( Mobile from, DyeTub sender )
		{
			from.SendLocalizedMessage( sender.FailMessage );
			return false;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}