using System;
using Server;
using Server.Engines.IdolSystem;

namespace Server.Items
{
	public class FragmentCrystal : Item
	{
		private QuestType m_Type;

		[CommandProperty( AccessLevel.GameMaster )]
		public QuestType type{ get{ return m_Type; } set{ m_Type = value; InvalidateProperties(); } }


		[Constructable]
		public FragmentCrystal() : base( 1 )
		{
		}

		[Constructable]
		public FragmentCrystal( QuestType type, int amount ) : base( 0xF8E )
		{
			m_Type = type;
			Name = "a fragment crystal of " + type;
			switch ( type )
			{
				case QuestType.Shame: Hue = 0x58F; break;
				case QuestType.Deceit: Hue = 0x7DA; break;
				case QuestType.Destard: Hue = 0x455;  break;
				case QuestType.Hythloth: Hue = 0x482;  break;
				case QuestType.Despise: Hue = 0x4E2; break;
				case QuestType.Covetous:  Hue = 0x4D3; break;
				case QuestType.Wrong: Hue = 0x655; break;
			}
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
		}

		public FragmentCrystal( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new FragmentCrystal( m_Type, amount ), amount );
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
					m_Type = (QuestType)reader.ReadInt();

					break;
				}
			}
		}
	}
}