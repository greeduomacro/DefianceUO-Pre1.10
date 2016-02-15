using System;
using Server;

namespace Server.Items
{
	public class TokenScroll : Item
	{
		private double m_Value;

		[CommandProperty( AccessLevel.GameMaster )]
		public double Value
		{
			get
			{
				return m_Value;
			}
			set
			{
				m_Value = value;
			}
		}

		[Constructable]
		public TokenScroll( double value ) : base( 0x14F0 )
		{
			Hue = 0x26;
			Weight = 1.0;

			LootType = LootType.Blessed;

			m_Value = value;
		}

		public TokenScroll( Serial serial ) : base( serial )
		{
		}

		public override void AddNameProperty(ObjectPropertyList list)
		{
			list.Add( "a token scroll of {0} points", m_Value );
		}

		public override void OnSingleClick( Mobile from )
		{
			base.LabelTo( from, "a token scroll of {0} points", m_Value );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (double) m_Value );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_Value = reader.ReadDouble();
					break;
				}
			}
		}
	}
}