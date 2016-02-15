using System;
using Server.Network;

namespace Server.Items
{
	public class LillyPad1 : Item
	{
		[Constructable]
		public LillyPad1() : base( 0xDBC )
		{
			Movable = true;
		}

		public LillyPad1( Serial serial ) : base( serial )
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

	public class LillyPad2 : Item
	{
		[Constructable]
		public LillyPad2() : base( 0xDBD )
		{
			Movable = true;
		}

		public LillyPad2( Serial serial ) : base( serial )
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

	public class LillyPad3 : Item
	{
		[Constructable]
		public LillyPad3() : base( 0xDBE )
		{
			Movable = true;
		}

		public LillyPad3( Serial serial ) : base( serial )
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