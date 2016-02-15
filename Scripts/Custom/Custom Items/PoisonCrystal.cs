using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public class PoisonCrystal : Item
	{
		[Constructable]
		public PoisonCrystal() : this( 1 )
		{
		}

		[Constructable]
		public PoisonCrystal( int amount ) : base( 0xF8E )
		{
			Weight = 11.0;
			Name = "Poison Crystal";
			Stackable = false;
			Hue = 64;
			Amount = amount;
		}

		public PoisonCrystal( Serial serial ) : base( serial )
		{
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