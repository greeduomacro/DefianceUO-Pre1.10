using System;
using Server;
using Server.Network;

namespace Server.Items
{
	public class AllBlocker : Item
	{
        public static void Initialize()
        {
            TileData.ItemTable[0x21A3].Flags = TileFlag.Wall | TileFlag.NoShoot | TileFlag.Impassable ;
            TileData.ItemTable[0x21A3].Height = 20;
        }

        [Constructable]
		public AllBlocker() : base( 0x21A3 )
		{
			Movable = false;
            Name = "Allblocker";
        }

		public AllBlocker( Serial serial ) : base( serial )
		{
		}

		public override void SendInfoTo( NetState state )
		{
			Mobile mob = state.Mobile;

            //Al: It seems that older clients display 0x21A3 as unused tile
            //      Since we modify the flags of 0x21A3 we have to use this itemid
            //      Hence we display this item as 0x21A4 (==normal blocker) to the
            //      UO client. This is extremely ugly :)

            if (mob != null && mob.AccessLevel >= AccessLevel.GameMaster)
                state.Send(new SpecialWorldItemPacket(this,0x241a));//Display as stone to GMs
            else
                state.Send(new SpecialWorldItemPacket(this, 0x21A4));//Display as blocker to Players

			if ( ObjectPropertyList.Enabled )
				state.Send( OPLPacket );
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

		public sealed class SpecialWorldItemPacket : Packet
		{
			public SpecialWorldItemPacket( Item item, int displayItemID) : base( 0x1A )
			{
				this.EnsureCapacity( 20 );

				// 14 base length
				// +2 - Amount
				// +2 - Hue
				// +1 - Flags

				uint serial = (uint)item.Serial.Value;
				int itemID = displayItemID; //Display as another item id
				int amount = item.Amount;
				Point3D loc = item.Location;
				int x = loc.X;
				int y = loc.Y;
				int hue = item.Hue;
				int flags = item.GetPacketFlags();
				int direction = (int)item.Direction;

				if ( amount != 0 )
					serial |= 0x80000000;
				else
					serial &= 0x7FFFFFFF;

				m_Stream.Write( (uint) serial );
				m_Stream.Write( (short) (itemID & 0x7FFF) );

				if ( amount != 0 )
					m_Stream.Write( (short) amount );

				x &= 0x7FFF;

				if ( direction != 0 )
					x |= 0x8000;

				m_Stream.Write( (short) x );

				y &= 0x3FFF;

				if ( hue != 0 )
					y |= 0x8000;

				if ( flags != 0 )
					y |= 0x4000;

				m_Stream.Write( (short) y );

				if ( direction != 0 )
					m_Stream.Write( (byte) direction );

				m_Stream.Write( (sbyte) loc.Z );

				if ( hue != 0 )
					m_Stream.Write( (ushort) hue );

				if ( flags != 0 )
					m_Stream.Write( (byte) flags );
			}
		}
    }
}