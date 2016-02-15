using System;
using Server;
using Server.Engines.IdolSystem;

namespace Server.Items
{
	public enum PieceType
	{
		NWCorner,
		NECorner,
		SWCorner,
		SECorner,
		WestEdge,
		EastEdge,
		SouthEdge,
		NorthEdge,
		Centre,
	}
	
	public class BasicPinkCarpet : Item
	{
		private PieceType m_Type;

		[CommandProperty( AccessLevel.GameMaster )]
		public PieceType type{ get{ return m_Type; } set{ m_Type = value; InvalidateProperties(); } }

		[Constructable]
		public BasicPinkCarpet( PieceType type ) : base()
		{
			m_Type = type;
			Name = "a pink carpet";
			switch ( type )
			{
				case PieceType.NWCorner: ItemID = 0xACA; break;
				case PieceType.NECorner: ItemID = 0xACC; break;
				case PieceType.SWCorner: ItemID = 0xACB;  break;
				case PieceType.SECorner: ItemID = 0xAC9;  break;
				case PieceType.WestEdge: ItemID = 0xACD; break;
				case PieceType.EastEdge:  ItemID = 0xACF; break;
				case PieceType.SouthEdge: ItemID = 0xAD0; break;
				case PieceType.NorthEdge:  ItemID = 0xACE; break;
				case PieceType.Centre: ItemID = Utility.RandomList( 0xAC6, 0xAC8, 0xAC7 ); break;
			}
			Weight = 1.0;
			Hue = 0x899;
		}

		public BasicPinkCarpet( Serial serial ) : base( serial )
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