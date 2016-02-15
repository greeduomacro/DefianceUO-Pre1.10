using System;
using Server;
using Server.Engines.IdolSystem;

namespace Server.Items
{
	public class RareBloodCarpet : Item
	{
		private PieceType m_Type;

		[CommandProperty( AccessLevel.GameMaster )]
		public PieceType type{ get{ return m_Type; } set{ m_Type = value; InvalidateProperties(); } }

		[Constructable]
		public RareBloodCarpet( PieceType type ) : base()
		{
			m_Type = type;
			Name = "a red carpet";
			switch ( type )
			{
				case PieceType.NWCorner: ItemID = 0xAE4; break;
				case PieceType.NECorner: ItemID = 0xAE6; break;
				case PieceType.SWCorner: ItemID = 0xAE5;  break;
				case PieceType.SECorner: ItemID = 0xAE3;  break;
				case PieceType.WestEdge: ItemID = 0xAE7; break;
				case PieceType.EastEdge:  ItemID = 0xAE9; break;
				case PieceType.SouthEdge: ItemID = 0xAEA; break;
				case PieceType.NorthEdge:  ItemID = 0xAE8; break;
				case PieceType.Centre: ItemID = 0xAEB; break;
			}
			Weight = 1.0;
			Hue = 0x4AA;
		}

		public RareBloodCarpet( Serial serial ) : base( serial )
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