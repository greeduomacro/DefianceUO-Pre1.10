using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public class BloodCrystal : Item
	{
		[Constructable]
		public BloodCrystal() : this( 1 )
		{
		}

		[Constructable]
		public BloodCrystal( int amount ) : base( 0xF8E )
		{
			Weight = 11.0;
			Name = "Blood Crystal";
			Stackable = false;
			Hue = 38;
			Amount = amount;
		}

		public BloodCrystal( Serial serial ) : base( serial )
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