using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Multis
{
	public class HouseDecayingCrate : Container
	{
		public static readonly int MaxItemsPerSubcontainer = 20;
		public static readonly int Rows = 3;
		public static readonly int Columns = 5;
		public static readonly int HorizontalSpacing = 25;
		public static readonly int VerticalSpacing = 25;

		public override int LabelNumber{ get{ return 1061690; } } // Packing Crate

		private Timer m_InternalizeTimer;

		public override int DefaultGumpID{ get{ return 0x44; } }
		public override int DefaultDropSound{ get{ return 0x42; } }

		public override Rectangle2D Bounds
		{
			get{ return new Rectangle2D( 20, 10, 150, 90 ); }
		}

		public override int DefaultMaxItems{ get{ return 0; } }
		public override int DefaultMaxWeight{ get{ return 0; } }

		public HouseDecayingCrate() : base( 0xE3D )
		{
			Hue = 0x8A5;
			Movable = false;
		}

		public HouseDecayingCrate( Serial serial ) : base( serial )
		{
		}

		public override void DropItem( Item dropped )
		{
			// 1. Try to stack the item
			foreach ( Item item in this.Items )
			{
				if ( item is PackingBox )
				{
					ArrayList subItems = item.Items;

					for ( int i = 0; i < subItems.Count; i++ )
					{
						Item subItem = (Item) subItems[i];

						if ( !(subItem is Container) && subItem.StackWith( null, dropped, false ) )
							return;
					}
				}
			}

			// 2. Try to drop the item into an existing container
			foreach ( Item item in this.Items )
			{
				if ( item is PackingBox )
				{
					Container box = (Container) item;
					ArrayList subItems = box.Items;

					if ( subItems.Count < MaxItemsPerSubcontainer )
					{
						box.DropItem( dropped );
						return;
					}
				}
			}

			// 3. Drop the item into a new container
			Container subContainer = new PackingBox();
			subContainer.DropItem( dropped );

			Point3D location = GetFreeLocation();
			if ( location != Point3D.Zero )
			{
				this.AddItem( subContainer );
				subContainer.Location = location;
			}
			else
			{
				base.DropItem( subContainer );
			}
		}

		private Point3D GetFreeLocation()
		{
			bool[,] positions = new bool[Rows, Columns];

			foreach ( Item item in this.Items )
			{
				if ( item is PackingBox )
				{
					int i = (item.Y - this.Bounds.Y) / VerticalSpacing;
					if ( i < 0 )
						i = 0;
					else if ( i >= Rows )
						i = Rows - 1;

					int j = (item.X - this.Bounds.X) / HorizontalSpacing;
					if ( j < 0 )
						j = 0;
					else if ( j >= Columns )
						j = Columns - 1;

					positions[i, j] = true;
				}
			}

			for ( int i = 0; i < Rows; i++ )
			{
				for ( int j = 0; j < Columns; j++ )
				{
					if ( !positions[i, j] )
					{
						int x = this.Bounds.X + j * HorizontalSpacing;
						int y = this.Bounds.Y + i * VerticalSpacing;

						return new Point3D( x, y, 0 );
					}
				}
			}

			return Point3D.Zero;
		}

		public override void SendCantStoreMessage( Mobile to, Item item )
		{
			to.SendLocalizedMessage( 1061145 ); // You cannot place items into a house moving crate.
		}

		public override void OnItemRemoved( Item item )
		{
			base.OnItemRemoved( item );

			if ( this.TotalItems == 0 )
				Delete();
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
}