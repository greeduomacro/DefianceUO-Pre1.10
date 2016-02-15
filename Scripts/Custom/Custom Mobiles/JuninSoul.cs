using System;
using Server;

namespace Server.Items
{
	public class JuninSoul : Item
	{
		[Constructable]
		public JuninSoul() : this( 1 )
		{
		}

		[Constructable]
		public JuninSoul( int amount ) : base( 0xF21 )
		{
			Stackable = false;
			Weight = 30;
			Name = "Junin's Soulstone";
                        Hue = 1249;
                        Amount = amount;
		}

		public JuninSoul( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new JuninSoul( amount ), amount );
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