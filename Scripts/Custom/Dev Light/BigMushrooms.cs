using System;
using Server.Network;

namespace Server.Items
{
	public class BigMushroom1 : Item
	{
		[Constructable]
		public BigMushroom1() : base( 0x222E )
		{
			Movable = true;
		}

		public BigMushroom1( Serial serial ) : base( serial )
		{
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

	public class BigMushroom2 : Item
	{
		[Constructable]
		public BigMushroom2() : base( 0x222F )
		{
			Movable = true;
		}

		public BigMushroom2( Serial serial ) : base( serial )
		{
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

	public class BigMushroom3 : Item
	{
		[Constructable]
		public BigMushroom3() : base( 0x2230 )
		{
			Movable = true;
		}

		public BigMushroom3( Serial serial ) : base( serial )
		{
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

	public class BigMushroom4 : Item
	{
		[Constructable]
		public BigMushroom4() : base( 0x2231 )
		{
			Movable = true;
		}

		public BigMushroom4( Serial serial ) : base( serial )
		{
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