using System;
using Server;
using Server.Engines.IdolSystem;

namespace Server.Items
{
	public class RareBlueCarpet : Item
	{
		private PieceType m_Type;

		[CommandProperty( AccessLevel.GameMaster )]
		public PieceType type{ get{ return m_Type; } set{ m_Type = value; InvalidateProperties(); } }

		[Constructable]
		public RareBlueCarpet( PieceType type ) : base()
		{
			m_Type = type;
			Name = "a dark blue carpet";
			switch ( type )
			{
				case PieceType.NWCorner: ItemID = 0xAD3; break;
				case PieceType.NECorner: ItemID = 0xAD5; break;
				case PieceType.SWCorner: ItemID = 0xAD4;  break;
				case PieceType.SECorner: ItemID = 0xAD2;  break;
				case PieceType.WestEdge: ItemID = 0xAD6; break;
				case PieceType.EastEdge:  ItemID = 0xAD8; break;
				case PieceType.SouthEdge: ItemID = 0xAD9; break;
				case PieceType.NorthEdge:  ItemID = 0xAD7; break;
				case PieceType.Centre: ItemID = 0xAD1; break;
			}
			Weight = 1.0;
			Hue = 0x4A1;
		}

		public RareBlueCarpet( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
			writer.Write( (int) m_Type );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			switch ( version )
			{
				case 0:
				{
					m_Type = (PieceType)reader.ReadInt();

					break;
				}
			}
		}
	}
}