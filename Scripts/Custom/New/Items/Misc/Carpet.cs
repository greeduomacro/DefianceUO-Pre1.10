using System;

namespace Server.Items
{
	public class CarpetPlain : Item
	{
		[Constructable]
		public CarpetPlain() : base( 0xaa9 )
		{
			Weight = 0.1;
			Movable = true;
		}

		public CarpetPlain( Serial serial ) : base( serial )
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
	public class CarpetPattern : Item
	{
		[Constructable]
		public CarpetPattern() : base( 0xabd )
		{
			Weight = 0.1;
			Movable = true;
		}

		public CarpetPattern( Serial serial ) : base( serial )
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
	public class CarpetCenter : Item
	{
		[Constructable]
		public CarpetCenter() : base( 0xad1 )
		{
			Weight = 0.1;
			Movable = true;
		}

		public CarpetCenter( Serial serial ) : base( serial )
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
	public class CarpetCorner1 : Item
	{
		[Constructable]
		public CarpetCorner1() : base( 0xad2 )
		{
			Weight = 0.1;
			Movable = true;
		}

		public CarpetCorner1( Serial serial ) : base( serial )
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
	public class CarpetCorner2 : Item
	{
		[Constructable]
		public CarpetCorner2() : base( 0xad3 )
		{
			Weight = 0.1;
			Movable = true;
		}

		public CarpetCorner2( Serial serial ) : base( serial )
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
	public class CarpetCorner3 : Item
	{
		[Constructable]
		public CarpetCorner3() : base( 0xad4 )
		{
			Weight = 0.1;
			Movable = true;
		}

		public CarpetCorner3( Serial serial ) : base( serial )
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
	public class CarpetCorner4 : Item
	{
		[Constructable]
		public CarpetCorner4() : base( 0xad5 )
		{
			Weight = 0.1;
			Movable = true;
		}

		public CarpetCorner4( Serial serial ) : base( serial )
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
	public class CarpetSide1 : Item
	{
		[Constructable]
		public CarpetSide1() : base( 0xad6 )
		{
			Weight = 0.1;
			Movable = true;
		}

		public CarpetSide1( Serial serial ) : base( serial )
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
	public class CarpetSide2 : Item
	{
		[Constructable]
		public CarpetSide2() : base( 0xad7 )
		{
			Weight = 0.1;
			Movable = true;
		}

		public CarpetSide2( Serial serial ) : base( serial )
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
	public class CarpetSide3 : Item
	{
		[Constructable]
		public CarpetSide3() : base( 0xad8 )
		{
			Weight = 0.1;
			Movable = true;
		}

		public CarpetSide3( Serial serial ) : base( serial )
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
	public class CarpetSide4 : Item
	{
		[Constructable]
		public CarpetSide4() : base( 0xad9 )
		{
			Weight = 0.1;
			Movable = true;
		}

		public CarpetSide4( Serial serial ) : base( serial )
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