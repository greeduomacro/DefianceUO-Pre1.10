using System;
using Server.Items;

namespace Server.Items
{
	public class CTFBlocker : Item
	{
		private CTFGame m_Game;

		[Constructable]
		public CTFBlocker() : base( 197 )
		{
			Movable = false;
		}

		public CTFBlocker( Serial serial ) : base( serial )
		{
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public CTFGame Game{ get{ return m_Game; } set{ m_Game = value; if ( m_Game != null ) m_Game.AddBlocker( this ); } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version
			writer.Write( m_Game );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			switch ( version )
			{
				case 0:
				{
					m_Game = reader.ReadItem() as CTFGame;
					break;
				}
			}
		}
	}
}