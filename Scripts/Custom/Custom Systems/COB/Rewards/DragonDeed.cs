//==============================================//
// Created by Dupre					//
//==============================================//
using System;
using Server;
using Server.Multis;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{

	[Flipable( 0x14F0, 0x14EF )]
	public class DragonTrophyDeed : Item
	{
		private int m_WestID;
		private int m_NorthID;

		[Constructable]
		public DragonTrophyDeed() : base( 0x14F0 )
		{
			m_WestID = 8757;
			m_NorthID = 8756;
			this.Name = "A Dragon's Head Deed";
			this.LootType = LootType.Blessed;
		}

		public DragonTrophyDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_WestID );
			writer.Write( (int) m_NorthID );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_WestID = reader.ReadInt();
					m_NorthID = reader.ReadInt();
					break;
				}
			}
		}

		public static bool IsWall( int x, int y, int z, Map map )
		{
			if ( map == null )
				return false;

			Tile[] tiles = map.Tiles.GetStaticTiles( x, y, true );

			for ( int i = 0; i < tiles.Length; ++i )
			{
				Tile t = tiles[i];
				ItemData id = TileData.ItemTable[t.ID & 0x3FFF];

				if ( (id.Flags & TileFlag.Wall) != 0 && (z + 16) > t.Z && (t.Z + t.Height) > z )
					return true;
			}

			return false;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsChildOf( from.Backpack ) )
			{
				BaseHouse house = BaseHouse.FindHouseAt( from );

				if ( house != null && house.IsCoOwner( from ) )
				{
					bool northWall = IsWall( from.X, from.Y - 1, from.Z, from.Map );
					bool westWall = IsWall( from.X - 1, from.Y, from.Z, from.Map );

					if ( northWall && westWall )
					{
						switch ( from.Direction & Direction.Mask )
						{
							case Direction.North:
							case Direction.South: northWall = true; westWall = false; break;

							case Direction.East:
							case Direction.West:  northWall = false; westWall = true; break;

							default: from.SendMessage( "Turn to face the wall on which to hang this trophy." ); return;
						}
					}

					int itemID = 0;

					if ( northWall )
						itemID = m_NorthID;
					else if ( westWall )
						itemID = m_WestID;
					else
						from.SendLocalizedMessage( 1042626 ); // The trophy must be placed next to a wall.

					if ( itemID > 0 )
					{
					house.Addons.Add( new DragonTrophyAddon( from, itemID) );
						Delete();
					}
				}
				else
				{
					from.SendLocalizedMessage( 502092 ); // You must be in your house to do this.
				}
			}
			else
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
		}
	}

	public class DragonTrophyAddon : Item, IDyable
	{
		private int m_WestID;
		private int m_NorthID;

		[Constructable]
		public DragonTrophyAddon( Mobile from, int itemID) : base( itemID )
		{
			m_WestID = 8757;
			m_NorthID = 8756;
			this.Name = "A Dragon's Head";
			Movable = false;
		MoveToWorld( (new Point3D( from.Location.X, from.Location.Y, from.Location.Z + 12)),from.Map );
		}

		public bool Dye( Mobile from, DyeTub sender )
		{
			if ( Deleted )
				return false;

			Hue = sender.DyedHue;

			return true;
		}

		public DragonTrophyAddon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_WestID );
			writer.Write( (int) m_NorthID );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_WestID = reader.ReadInt();
					m_NorthID = reader.ReadInt();
					break;
				}
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			BaseHouse house = BaseHouse.FindHouseAt( this );

			if ( house != null && house.IsCoOwner( from ) )
			{
				if ( from.InRange( GetWorldLocation(), 1 ) )
				{
					from.AddToBackpack( new DragonTrophyDeed() );
					Delete();
				}
				else
				{
					from.SendLocalizedMessage( 500295 ); // You are too far away to do that.
				}
			}
		}
	}
}