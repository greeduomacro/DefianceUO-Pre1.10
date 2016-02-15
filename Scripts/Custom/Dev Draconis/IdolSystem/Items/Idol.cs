using System;
using Server;
using Server.Engines.IdolSystem;

namespace Server.Items
{
	public enum IdolType
	{
		Shame,
		Deceit,
		Destard,
		Hythloth,
		Despise,
		Covetous,
		FireLord,
		DragonKing,
		Undeath,
		Wrong,
	}

	public class Idol : Item
	{
		private IdolType m_Type;

		[CommandProperty( AccessLevel.GameMaster )]
		public IdolType Type{ get{ return m_Type; } set{ m_Type = value; InvalidateProperties(); } }

		[Constructable]
		public Idol( IdolType type ) : base( 0x1F18 )
		{
			m_Type = type;
			Name = "Idol of " + type;
			switch ( type )
			{
				case IdolType.Shame: Hue = 0x58F; break;
                case IdolType.Deceit: Hue = 0x4E2; break;
				case IdolType.Destard: Hue = 0x455;  break;
				case IdolType.Hythloth: Hue = 0x482;  break;
				case IdolType.Despise: Hue = 0x7DA; Movable = true; break;
				case IdolType.Covetous:  Hue = 0x4D3; break;
				case IdolType.FireLord: Hue = 0x4EA; Name = "Idol of the Fire Lord"; break;
				case IdolType.DragonKing: Hue = 0x47E; Name = "Idol of the Dragon King"; break;
				case IdolType.Undeath: Hue = 0x497; break;
				case IdolType.Wrong: Hue = 0x655; break;
			}

		}

		public Idol( Serial serial ) : base( serial )
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
					m_Type = (IdolType)reader.ReadInt();

					break;
				}
			}
		}
	}
}