using System;
using Server;

namespace Server.Items
{
	public class LargeGoldCarpetAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new LargeGoldCarpetDeed(); } }

		[Constructable]
		public LargeGoldCarpetAddon()
		{
			// 1
			AddComponent( new AddonComponent( 0xADC ), -2, -2, 0 );
			AddComponent( new AddonComponent( 0xAE0 ), -1, -2, 0 );
			AddComponent( new AddonComponent( 0xAE0 ), 0, -2, 0 );
			AddComponent( new AddonComponent( 0xAE0 ), 1, -2, 0 );
			AddComponent( new AddonComponent( 0xADE ), 2, -2, 0 );

			// 2
			AddComponent( new AddonComponent( 0xADF ), -2, -1, 0 );
			AddComponent( new AddonComponent( 0xADA ), -1, -1, 0 );
			AddComponent( new AddonComponent( 0xADA ), 0, -1, 0 );
			AddComponent( new AddonComponent( 0xADA ), 1, -1, 0 );
			AddComponent( new AddonComponent( 0xAE1 ), 2, -1, 0 );

			// 3
			AddComponent( new AddonComponent( 0xADF ), -2, 0, 0 );
			AddComponent( new AddonComponent( 0xADA ), -1, 0, 0 );
			AddComponent( new AddonComponent( 0xADA ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0xADA ), 1, 0, 0 );
			AddComponent( new AddonComponent( 0xAE1 ), 2, 0, 0 );

			// 4
			AddComponent( new AddonComponent( 0xADF ), -2, 1, 0 );
			AddComponent( new AddonComponent( 0xADA ), -1, 1, 0 );
			AddComponent( new AddonComponent( 0xADA ), 0, 1, 0 );
			AddComponent( new AddonComponent( 0xADA ), 1, 1, 0 );
			AddComponent( new AddonComponent( 0xAE1 ), 2, 1, 0 );

			// 5
			AddComponent( new AddonComponent( 0xADD ), -2, 2, 0 );
			AddComponent( new AddonComponent( 0xAE2 ), -1, 2, 0 );
			AddComponent( new AddonComponent( 0xAE2 ), 0, 2, 0 );
			AddComponent( new AddonComponent( 0xAE2 ), 1, 2, 0 );
			AddComponent( new AddonComponent( 0xADB ), 2, 2, 0 );
		}

		public LargeGoldCarpetAddon( Serial serial ) : base( serial )
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

	public class LargeGoldCarpetDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new LargeGoldCarpetAddon(); } }

		[Constructable]
		public LargeGoldCarpetDeed()
		{
			Name = "a large gold carpet deed";
			LootType = LootType.Blessed;
		}

		public LargeGoldCarpetDeed( Serial serial ) : base( serial )
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

	public class LargeBlueCarpetAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new LargeBlueCarpetDeed(); } }

		[Constructable]
		public LargeBlueCarpetAddon()
		{
			// 1
			AddComponent( new AddonComponent( 0xAC3 ), -2, -2, 0 );
			AddComponent( new AddonComponent( 0xAF7 ), -1, -2, 0 );
			AddComponent( new AddonComponent( 0xAF7 ), 0, -2, 0 );
			AddComponent( new AddonComponent( 0xAF7 ), 1, -2, 0 );
			AddComponent( new AddonComponent( 0xAC5 ), 2, -2, 0 );

			// 2
			AddComponent( new AddonComponent( 0xAF6 ), -2, -1, 0 );
			AddComponent( new AddonComponent( 0xAC0 ), -1, -1, 0 );
			AddComponent( new AddonComponent( 0xAC0 ), 0, -1, 0 );
			AddComponent( new AddonComponent( 0xAC0 ), 1, -1, 0 );
			AddComponent( new AddonComponent( 0xAF8 ), 2, -1, 0 );

			// 3
			AddComponent( new AddonComponent( 0xAF6 ), -2, 0, 0 );
			AddComponent( new AddonComponent( 0xAC0 ), -1, 0, 0 );
			AddComponent( new AddonComponent( 0xAC0 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0xAC0 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 0xAF8 ), 2, 0, 0 );

			// 4
			AddComponent( new AddonComponent( 0xAF6 ), -2, 1, 0 );
			AddComponent( new AddonComponent( 0xAC0 ), -1, 1, 0 );
			AddComponent( new AddonComponent( 0xAC0 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 0xAC0 ), 1, 1, 0 );
			AddComponent( new AddonComponent( 0xAF8 ), 2, 1, 0 );

			// 5
			AddComponent( new AddonComponent( 0xAC4 ), -2, 2, 0 );
			AddComponent( new AddonComponent( 0xAF9 ), -1, 2, 0 );
			AddComponent( new AddonComponent( 0xAF9 ), 0, 2, 0 );
			AddComponent( new AddonComponent( 0xAF9 ), 1, 2, 0 );
			AddComponent( new AddonComponent( 0xAC2 ), 2, 2, 0 );
		}

		public LargeBlueCarpetAddon( Serial serial ) : base( serial )
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

	public class LargeBlueCarpetDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new LargeBlueCarpetAddon(); } }

		[Constructable]
		public LargeBlueCarpetDeed()
		{
			Name = "a large blue carpet deed";
			LootType = LootType.Blessed;
		}

		public LargeBlueCarpetDeed( Serial serial ) : base( serial )
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

	public class LargeRedCarpetAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new LargeRedCarpetDeed(); } }

		[Constructable]
		public LargeRedCarpetAddon()
		{
			// 1
			AddComponent( new AddonComponent( 0xACA ), -2, -2, 0 );
			AddComponent( new AddonComponent( 0xACE ), -1, -2, 0 );
			AddComponent( new AddonComponent( 0xACE ), 0, -2, 0 );
			AddComponent( new AddonComponent( 0xACE ), 1, -2, 0 );
			AddComponent( new AddonComponent( 0xACC ), 2, -2, 0 );

			// 2
			AddComponent( new AddonComponent( 0xACD ), -2, -1, 0 );
			AddComponent( new AddonComponent( 0xAC7 ), -1, -1, 0 );
			AddComponent( new AddonComponent( 0xAC7 ), 0, -1, 0 );
			AddComponent( new AddonComponent( 0xAC7 ), 1, -1, 0 );
			AddComponent( new AddonComponent( 0xACF ), 2, -1, 0 );

			// 3
			AddComponent( new AddonComponent( 0xACD ), -2, 0, 0 );
			AddComponent( new AddonComponent( 0xAC7 ), -1, 0, 0 );
			AddComponent( new AddonComponent( 0xAC7 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0xAC7 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 0xACF ), 2, 0, 0 );

			// 4
			AddComponent( new AddonComponent( 0xACD ), -2, 1, 0 );
			AddComponent( new AddonComponent( 0xAC7 ), -1, 1, 0 );
			AddComponent( new AddonComponent( 0xAC7 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 0xAC7 ), 1, 1, 0 );
			AddComponent( new AddonComponent( 0xACF ), 2, 1, 0 );

			// 5
			AddComponent( new AddonComponent( 0xACB ), -2, 2, 0 );
			AddComponent( new AddonComponent( 0xAD0 ), -1, 2, 0 );
			AddComponent( new AddonComponent( 0xAD0 ), 0, 2, 0 );
			AddComponent( new AddonComponent( 0xAD0 ), 1, 2, 0 );
			AddComponent( new AddonComponent( 0xAC9 ), 2, 2, 0 );
		}

		public LargeRedCarpetAddon( Serial serial ) : base( serial )
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

	public class LargeRedCarpetDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new LargeRedCarpetAddon(); } }

		[Constructable]
		public LargeRedCarpetDeed()
		{
			Name = "a large red carpet deed";
			LootType = LootType.Blessed;
		}

		public LargeRedCarpetDeed( Serial serial ) : base( serial )
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

	public class LargeBrownCarpetAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new LargeBrownCarpetDeed(); } }

		[Constructable]
		public LargeBrownCarpetAddon()
		{
			// 1
			AddComponent( new AddonComponent( 0xAE4 ), -2, -2, 0 );
			AddComponent( new AddonComponent( 0xAE8 ), -1, -2, 0 );
			AddComponent( new AddonComponent( 0xAE8 ), 0, -2, 0 );
			AddComponent( new AddonComponent( 0xAE8 ), 1, -2, 0 );
			AddComponent( new AddonComponent( 0xAE6 ), 2, -2, 0 );

			// 2
			AddComponent( new AddonComponent( 0xAE7 ), -2, -1, 0 );
			AddComponent( new AddonComponent( 0xAEB ), -1, -1, 0 );
			AddComponent( new AddonComponent( 0xAEB ), 0, -1, 0 );
			AddComponent( new AddonComponent( 0xAEB ), 1, -1, 0 );
			AddComponent( new AddonComponent( 0xAE9 ), 2, -1, 0 );

			// 3
			AddComponent( new AddonComponent( 0xAE7 ), -2, 0, 0 );
			AddComponent( new AddonComponent( 0xAEB ), -1, 0, 0 );
			AddComponent( new AddonComponent( 0xAEB ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0xAEB ), 1, 0, 0 );
			AddComponent( new AddonComponent( 0xAE9 ), 2, 0, 0 );

			// 4
			AddComponent( new AddonComponent( 0xAE7 ), -2, 1, 0 );
			AddComponent( new AddonComponent( 0xAEB ), -1, 1, 0 );
			AddComponent( new AddonComponent( 0xAEB ), 0, 1, 0 );
			AddComponent( new AddonComponent( 0xAEB ), 1, 1, 0 );
			AddComponent( new AddonComponent( 0xAE9 ), 2, 1, 0 );

			// 5
			AddComponent( new AddonComponent( 0xAE5 ), -2, 2, 0 );
			AddComponent( new AddonComponent( 0xAEA ), -1, 2, 0 );
			AddComponent( new AddonComponent( 0xAEA ), 0, 2, 0 );
			AddComponent( new AddonComponent( 0xAEA ), 1, 2, 0 );
			AddComponent( new AddonComponent( 0xAE3 ), 2, 2, 0 );
		}

		public LargeBrownCarpetAddon( Serial serial ) : base( serial )
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

	public class LargeBrownCarpetDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new LargeBrownCarpetAddon(); } }

		[Constructable]
		public LargeBrownCarpetDeed()
		{
			Name = "a large brown carpet deed";
			LootType = LootType.Blessed;
		}

		public LargeBrownCarpetDeed( Serial serial ) : base( serial )
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