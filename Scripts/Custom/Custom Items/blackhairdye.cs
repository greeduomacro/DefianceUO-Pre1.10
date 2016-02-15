using System;
using System.Text;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public class BlackHairDye : Item
	{
		[Constructable]
		public BlackHairDye() : base( 0xEFC )
		{
			Weight = 1.0;
			Name = "a black hair dye";
			Hue = 1175;
		}

		public BlackHairDye( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.InRange( this.GetWorldLocation(), 1 ) )
			{
				from.CloseGump( typeof( BlackHairDyeGump ) );
				from.SendGump( new BlackHairDyeGump( this ) );
			}
			else
			{
				from.LocalOverheadMessage( MessageType.Regular, 906, 1019045 ); // I can't reach that.
			}

		}
	}

	public class BlackHairDyeGump : Gump
	{
		private BlackHairDye m_BlackHairDye;

		private class BlackHairDyeEntry
		{
			private string m_Name;
			private int m_HueStart;
			private int m_HueCount;

			public string Name
			{
				get
				{
					return m_Name;
				}
			}

			public int HueStart
			{
				get
				{
					return m_HueStart;
				}
			}

			public int HueCount
			{
				get
				{
					return m_HueCount;
				}
			}

			public BlackHairDyeEntry( string name, int hueStart, int hueCount )
			{
				m_Name = name;
				m_HueStart = hueStart;
				m_HueCount = hueCount;
			}
		}

		private static BlackHairDyeEntry[] m_Entries = new BlackHairDyeEntry[]
			{
				new BlackHairDyeEntry( "*****", 1, 1 ),
				//new BlackHairDyeEntry( "*****", 1258, 1 ),
				//new BlackHairDyeEntry( "*****", 3, 2 ),
				//new BlackHairDyeEntry( "*****", 1151, 1 ),
				//new BlackHairDyeEntry( "*****", 1175, 1 ),
				//new BlackHairDyeEntry( "*****", 1155, 1 ),
				//new BlackHairDyeEntry( "*****", 1166, 1 ),
				//new BlackHairDyeEntry( "*****", 1153, 2 )
		};

		public BlackHairDyeGump( BlackHairDye dye ) : base( 0, 0 )
		{
			m_BlackHairDye = dye;

			AddPage( 0 );
			AddBackground( 150, 60, 350, 138, 2600 );
			AddBackground( 170, 104, 110, 40, 5100 );
			AddHtmlLocalized( 230, 75, 200, 20, 1011013, false, false );		// Hair Color Selection Menu
			AddHtmlLocalized( 235, 150, 300, 20, 1011014, false, false );		// Dye my hair this color!
			AddButton( 200, 150, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0 );        // DYE HAIR

			for ( int i = 0; i < m_Entries.Length; ++i )
			{
				AddLabel( 180, 109 + (i * 22), m_Entries[i].HueStart - 1, m_Entries[i].Name );
				AddButton( 257, 110 + (i * 22), 5224, 5224, 0, GumpButtonType.Page, i + 1 );
			}

			for ( int i = 0; i < m_Entries.Length; ++i )
			{
				BlackHairDyeEntry e = m_Entries[i];

				AddPage( i + 1 );

				for ( int j = 0; j < e.HueCount; ++j )
				{
					AddLabel( 335 + ((j / 16) * 80), 102 + ((j % 16) * 17), e.HueStart + j - 1, "*****" );
					AddRadio( 310 + ((j / 16) * 80), 102 + ((j % 16) * 17), 210, 211, false, (i * 100) + j );
				}
			}
		}

		public override void OnResponse( NetState from, RelayInfo info )
		{
			if ( m_BlackHairDye.Deleted )
				return;

			Mobile m = from.Mobile;
			int[] switches = info.Switches;

			if ( !m_BlackHairDye.IsChildOf( m.Backpack ) )
			{
				m.SendLocalizedMessage( 1042010 ); //You must have the object in your backpack to use it.
				return;
			}

			if ( info.ButtonID != 0 && switches.Length > 0 )
			{
				Item hair = m.Hair;

				if ( hair == null )
				{
					m.SendLocalizedMessage( 502623 );	// You have no hair to dye and cannot use this
				}
				else
				{
					// To prevent this from being exploited, the hue is abstracted into an internal list

					int entryIndex = switches[0] / 100;
					int hueOffset = switches[0] % 100;

					if ( entryIndex >= 0 && entryIndex < m_Entries.Length )
					{
						BlackHairDyeEntry e = m_Entries[entryIndex];

						if ( hueOffset >= 0 && hueOffset < e.HueCount )
						{
							m_BlackHairDye.Delete();

							int hue = e.HueStart + hueOffset;

							hair.Hue = hue;

							m.SendLocalizedMessage( 501199 );  // You dye your hair
							m.PlaySound( 0x4E );
						}
					}
				}
			}
			else
			{
				m.SendLocalizedMessage( 501200 ); // You decide not to dye your hair
			}
		}
	}
}