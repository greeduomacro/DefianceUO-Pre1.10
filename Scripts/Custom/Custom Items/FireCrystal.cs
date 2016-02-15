using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public class FireCrystal : Item
	{
		[Constructable]
		public FireCrystal() : this( 1 )
		{
		}

		[Constructable]
		public FireCrystal( int amount ) : base( 0xF8E )
		{
			Weight = 11.0;
			Name = "Fire Crystal";
			Stackable = false;
			Hue = 1161;
			Amount = amount;
		}

		public FireCrystal( Serial serial ) : base( serial )
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