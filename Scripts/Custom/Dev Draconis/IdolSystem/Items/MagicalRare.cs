using Server;
using System;

namespace Server.Items
{
	public enum MagicalRareType
	{
		One,
		Two,
		Three,
		Four,
		Five,
		Six,
	}

	public class MagicalRare : Item
	{
		private MagicalRareType m_Rare;

		public MagicalRareType rares{ get{ return m_Rare; } set{ m_Rare = value; InvalidateProperties(); } }

		[Constructable]
		public MagicalRare( MagicalRareType rares ) : base( 7955 )
		{
            		Weight = 5.0;
			m_Rare = rares;
			switch ( rares )
			{
				case MagicalRareType.One: Name = "an odd looking rare"; Hue = 1010; break;
				case MagicalRareType.Two: Name = "a small rare item"; Hue = 1020; break;
				case MagicalRareType.Three: Name = "a dull rare"; Hue = 1030; break;
				case MagicalRareType.Four: Name = "a funny shaped rare"; Hue = 1040; break;
				case MagicalRareType.Five: Name = "a scratched rare"; Hue = 1050; break;
				case MagicalRareType.Six: Name = "an old rare"; Hue = 1055; break;
			}
		}

		public MagicalRare( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_Rare );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_Rare = (MagicalRareType)reader.ReadInt();;
					break;
				}
			}
		}
    	}
}