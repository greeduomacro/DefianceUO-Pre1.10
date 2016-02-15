using System;
using Server.Network;

namespace Server.Items
{
	public class DonationDecorArmor1 : Item
	{
		[Constructable]
		public DonationDecorArmor1() : base( 0x1508 )
		{
			Movable = true;
		}

		public DonationDecorArmor1( Serial serial ) : base( serial )
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

	public class DonationDecorArmor2 : Item
	{
		[Constructable]
		public DonationDecorArmor2() : base( 0x151C )
		{
			Movable = true;
		}

		public DonationDecorArmor2( Serial serial ) : base( serial )
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

	public class DonationDecorArmor3 : Item
	{
		[Constructable]
		public DonationDecorArmor3() : base( 0x151A )
		{
			Movable = true;
		}

		public DonationDecorArmor3( Serial serial ) : base( serial )
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

	public class DonationDecorArmor4 : Item
	{
		[Constructable]
		public DonationDecorArmor4() : base( 0x1512 )
		{
			Movable = true;
		}

		public DonationDecorArmor4( Serial serial ) : base( serial )
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