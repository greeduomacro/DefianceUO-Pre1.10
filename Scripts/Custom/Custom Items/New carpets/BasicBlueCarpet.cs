using System;
using Server;
using Server.Engines.IdolSystem;

namespace Server.Items
{
	public class BasicBlueCarpet : Item
	{
		private PieceType m_Type;

		[CommandProperty( AccessLevel.GameMaster )]
		public PieceType type{ get{ return m_Type; } set{ m_Type = value; InvalidateProperties(); } }

		[Constructable]
		public BasicBlueCarpet( PieceType type ) : base()
		{
			m_Type = type;
			Name = "a blue carpet";
			switch ( type )
			{
				case PieceType.NWCorner: ItemID = 0xAC3; break;
				case PieceType.NECorner: ItemID = 0xAC5; break;
				case PieceType.SWCorner: ItemID = 0xAC4;  break;
				case PieceType.SECorner: ItemID = 0xAC2;  break;
				case PieceType.WestEdge: ItemID = 0xAF6; break;
				case PieceType.EastEdge:  ItemID = 0xAF8; break;
				case PieceType.SouthEdge: ItemID = 0xAF9; break;
				case PieceType.NorthEdge:  ItemID = 0xAF7; break;
				case PieceType.Centre: ItemID = Utility.RandomList( 0xABF, 0xABE, 0xABD ); break;
			}
			Weight = 1.0;
			Hue = 0x8AB;
		}

		public BasicBlueCarpet( Serial serial ) : base( serial )
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