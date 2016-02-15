using System;
using System.Text;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public class BlackBeardHairDye : Item
	{
		public override int LabelNumber{ get{ return 1041087; } } // Special Beard Dye

		[Constructable]
		public BlackBeardHairDye() : base( 0xE26 )
		{
			Weight = 1.0;
			LootType = LootType.Regular;
			Hue = 1175;
			Name = "Special Black Beard Dye";
		}

		public BlackBeardHairDye( Serial serial ) : base( serial )
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
				from.CloseGump( typeof( BlackBeardHairDyeGump ) );
				from.SendGump( new BlackBeardHairDyeGump( this ) );
			}
			else
			{
				from.LocalOverheadMessage( MessageType.Regular, 906, 1019045 ); // I can't reach that.
			}
		}
	}

	public class BlackBeardHairDyeGump : Gump
	{
		private BlackBeardHairDye m_BlackBeardHairDye;

		private class BlackBeardHairDyeEntry
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

			public BlackBeardHairDyeEntry( string name, int hueStart, int hueCount )
			{
				m_Name = name;
				m_HueStart = hueStart;
				m_HueCount = hueCount;
			}
		}

		private static BlackBeardHairDyeEntry[] m_Entries = new BlackBeardHairDyeEntry[]
			{
				new BlackBeardHairDyeEntry( "*****", 1, 1 ),
				//new BlackBeardHairDyeEntry( "*****", 32, 5 ),
				//new BlackBeardHairDyeEntry( "*****", 38, 8 ),
				//new BlackBeardHairDyeEntry( "*****", 54, 3 ),
				//new BlackBeardHairDyeEntry( "*****", 62, 10 ),
				//new BlackBeardHairDyeEntry( "*****", 81, 2 ),
				//new BlackBeardHairDyeEntry( "*****", 89, 2 ),
				//new BlackBeardHairDyeEntry( "*****", 1153, 2 )
		};

		public BlackBeardHairDyeGump( BlackBeardHairDye dye ) : base( 0, 0 )
		{
			m_BlackBeardHairDye = dye;

			AddPage( 0 );
			AddBackground( 150, 60, 350, 358, 2600 );
			AddBackground( 170, 104, 110, 270, 5100 );
			AddHtmlLocalized( 230, 75, 200, 20, 1011013, false, false );		// Hair Color Selection Menu
			AddHtmlLocalized( 235, 380, 300, 20, 1013007, false, false );		// Dye my beard this color!
			AddButton( 200, 380, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0 );        // DYE HAIR

			for ( int i = 0; i < m_Entries.Length; ++i )
			{
				AddLabel( 180, 109 + (i * 22), m_Entries[i].HueStart - 1, m_Entries[i].Name );
				AddButton( 257, 110 + (i * 22), 5224, 5224, 0, GumpButtonType.Page, i + 1 );
			}

			for ( int i = 0; i < m_Entries.Length; ++i )
			{
				BlackBeardHairDyeEntry e = m_Entries[i];

				AddPage( i + 1 );

				for ( int j = 0; j < e.HueCount; ++j )
				{
					AddLabel( 328 + ((j / 16) * 80), 102 + ((j % 16) * 17), e.HueStart + j - 1, "*****" );
					AddRadio( 310 + ((j / 16) * 80), 102 + ((j % 16) * 17), 210, 211, false, (i * 100) + j );
				}
			}
		}

		public override void OnResponse( NetState from, RelayInfo info )
		{
			if ( m_BlackBeardHairDye.Deleted )
				return;

			Mobile m = from.Mobile;
			int[] switches = info.Switches;

			if ( !m_BlackBeardHairDye.IsChildOf( m.Backpack ) )
			{
				m.SendLocalizedMessage( 1042010 ); //You must have the objectin your backpack to use it.
				return;
			}

			if ( info.ButtonID != 0 && switches.Length > 0 )
			{
				Item beard = m.Beard;

				if ( beard == null )
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
						BlackBeardHairDyeEntry e = m_Entries[entryIndex];

						if ( hueOffset >= 0 && hueOffset < e.HueCount )
						{
							int hue = e.HueStart + hueOffset;

							if ( beard != null )
								beard.Hue = hue;

							m.SendLocalizedMessage( 501199 );  // You dye your hair
							m_BlackBeardHairDye.Delete();
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