using Server;
using Server.Items;
using Server.Multis;
using Server.Network;
using System;

namespace Server.Items
{
	[FlipableAttribute( 0xe43, 0xe42 )]
	public class WoodenTreasureChest : BaseTreasureChest
	{
		public override int DefaultGumpID{ get{ return 0x49; } }
		public override int DefaultDropSound{ get{ return 0x42; } }

		public override Rectangle2D Bounds
		{
			get{ return new Rectangle2D( 20, 105, 150, 180 ); }
		}

		protected override void SetLockedName()
		{
			Name = "a locked wooden treasure chest";
		}

		protected override void SetUnlockedName()
		{
			Name = "a wooden treasure chest";
		}

		[Constructable]
		public WoodenTreasureChest() : base( 0xE43 )
		{
		}

		public WoodenTreasureChest( Serial serial ) : base( serial )
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

	[FlipableAttribute( 0xe41, 0xe40 )]
	public class MetalGoldenTreasureChest : BaseTreasureChest
	{
		public override int DefaultGumpID{ get{ return 0x42; } }
		public override int DefaultDropSound{ get{ return 0x42; } }

		public override Rectangle2D Bounds
		{
			get{ return new Rectangle2D( 20, 105, 150, 180 ); }
		}

		protected override void SetLockedName()
		{
			Name = "a locked metal treasure chest";
		}

		protected override void SetUnlockedName()
		{
			Name = "a metal treasure chest";
		}

		[Constructable]
		public MetalGoldenTreasureChest() : base( 0xE41 )
		{
		}

		public MetalGoldenTreasureChest( Serial serial ) : base( serial )
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

	[FlipableAttribute( 0x9ab, 0xe7c )]
	public class MetalTreasureChest : BaseTreasureChest
	{
		public override int DefaultGumpID{ get{ return 0x4A; } }
		public override int DefaultDropSound{ get{ return 0x42; } }

		public override Rectangle2D Bounds
		{
			get{ return new Rectangle2D( 20, 105, 150, 180 ); }
		}

		protected override void SetLockedName()
		{
			Name = "a locked metal treasure chest";
		}

		protected override void SetUnlockedName()
		{
			Name = "a metal treasure chest";
		}

		[Constructable]
		public MetalTreasureChest() : base( 0x9AB )
		{
		}

		public MetalTreasureChest( Serial serial ) : base( serial )
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

	[Flipable( 0xA97, 0xA99, 0xA98, 0xA9A, 0xA9B, 0xA9C )]
	public class TreasureBookcase : BaseTreasureChest
	{
		protected override void SetLockedName()
		{
			Name = "a locked bookcase";
		}

		protected override void SetUnlockedName()
		{
			Name = "a bookcase";
		}

		[Constructable]
		public TreasureBookcase() : base( 0xA97 )
		{
		}

		public TreasureBookcase( Serial serial ) : base( serial )
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

	[Flipable( 0xE3D, 0xE3C )]
	public class LargeTreasureCrate : BaseTreasureChest
	{
		protected override void SetLockedName()
		{
			Name = "a locked crate";
		}

		protected override void SetUnlockedName()
		{
			Name = "a crate";
		}

		[Constructable]
		public LargeTreasureCrate() : base( 0xE3D )
		{
		}

		public LargeTreasureCrate( Serial serial ) : base( serial )
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

	[Flipable( 0x9A9, 0xE7E )]
	public class SmallTreasureCrate : BaseTreasureChest
	{
		protected override void SetLockedName()
		{
			Name = "a locked crate";
		}

		protected override void SetUnlockedName()
		{
			Name = "a crate";
		}

		[Constructable]
		public SmallTreasureCrate() : base( 0x9A9 )
		{
		}

		public SmallTreasureCrate( Serial serial ) : base( serial )
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

	[Flipable( 0x9AA, 0xE7D )]
	public class WoodenTreasureBox : BaseTreasureChest
	{
		protected override void SetLockedName()
		{
			Name = "a locked wooden treasure box";
		}

		protected override void SetUnlockedName()
		{
			Name = "a locked wooden treasure box";
		}

		[Constructable]
		public WoodenTreasureBox() : base( 0x9AA )
		{
		}

		public WoodenTreasureBox( Serial serial ) : base( serial )
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

	[Flipable( 0x9A8, 0xE80 )]
	public class MetalTreasureBox : BaseTreasureChest
	{
		protected override void SetLockedName()
		{
			Name = "a locked metal treasure box";
		}

		protected override void SetUnlockedName()
		{
			Name = "a locked metal treasure box";
		}

		[Constructable]
		public MetalTreasureBox() : base( 0x9A8 )
		{
		}

		public MetalTreasureBox( Serial serial ) : base( serial )
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

	public class TreasureBarrel : BaseTreasureChest
	{
		protected override void SetLockedName()
		{
			Name = "a sealed barrel";
		}

		protected override void SetUnlockedName()
		{
			Name = "a barrel";
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public override bool Locked
		{
			get { return base.Locked; }
			set
			{
				base.Locked = value;
				if ( value )
					ItemID = 0xFAE;
				else
					ItemID = 0x0E77;
			}
		}

		[Constructable]
		public TreasureBarrel() : base( 0xFAE )
		{
		}

		public TreasureBarrel( Serial serial ) : base( serial )
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