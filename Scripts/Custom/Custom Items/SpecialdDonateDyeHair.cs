using System;
using System.Text;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public class SpecialDonateDye : Item
	{
		[Constructable]
		public SpecialDonateDye() : base( 0xEFC )
		{
			Weight = 9.0;
			LootType = LootType.Regular;
			Name = "Special Donate Hair Dye";
			Hue = 1260;
		}

		public SpecialDonateDye( Serial serial ) : base( serial )
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
				from.CloseGump( typeof( SpecialDonateDyeGump ) );
				from.SendGump( new SpecialDonateDyeGump( this ) );
			}
			else
			{
				from.LocalOverheadMessage( MessageType.Regular, 906, 1019045 ); // I can't reach that.
			}

		}
	}

	public class SpecialDonateDyeGump : Gump
	{
		private SpecialDonateDye m_SpecialDonateDye;

		private class SpecialDonateDyeEntry
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

			public SpecialDonateDyeEntry( string name, int hueStart, int hueCount )
			{
				m_Name = name;
				m_HueStart = hueStart;
				m_HueCount = hueCount;
			}
		}

		private static SpecialDonateDyeEntry[] m_Entries = new SpecialDonateDyeEntry[]
			{
				new SpecialDonateDyeEntry( "*****", 1161, 1 ),
                                new SpecialDonateDyeEntry( "*****", 1281, 1 ),
				new SpecialDonateDyeEntry( "*****", 1258, 1 ),
				new SpecialDonateDyeEntry( "*****", 3, 2 ),
                                new SpecialDonateDyeEntry( "*****", 1282, 1 ),
                                new SpecialDonateDyeEntry( "*****", 1195, 1 ),
				new SpecialDonateDyeEntry( "*****", 1151, 1 ),
				new SpecialDonateDyeEntry( "*****", 1175, 1 ),
				new SpecialDonateDyeEntry( "*****", 1155, 1 ),
                                new SpecialDonateDyeEntry( "*****", 1286, 1 ),
				new SpecialDonateDyeEntry( "*****", 1166, 1 ),
				new SpecialDonateDyeEntry( "*****", 1194, 1 )
		};

		public SpecialDonateDyeGump( SpecialDonateDye dye ) : base( 0, 0 )
		{
			m_SpecialDonateDye = dye;

			AddPage( 0 );
			AddBackground( 150, 60, 350, 358, 2600 );
			AddBackground( 170, 104, 110, 270, 5100 );
			AddHtmlLocalized( 230, 75, 200, 20, 1011013, false, false );		// Hair Color Selection Menu
			AddHtmlLocalized( 235, 380, 300, 20, 1011014, false, false );		// Dye my hair this color!
			AddButton( 200, 380, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0 );        // DYE HAIR

			for ( int i = 0; i < m_Entries.Length; ++i )
			{
				AddLabel( 180, 109 + (i * 22), m_Entries[i].HueStart - 1, m_Entries[i].Name );
				AddButton( 257, 110 + (i * 22), 5224, 5224, 0, GumpButtonType.Page, i + 1 );
			}

			for ( int i = 0; i < m_Entries.Length; ++i )
			{
				SpecialDonateDyeEntry e = m_Entries[i];

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
			if ( m_SpecialDonateDye.Deleted )
				return;

			Mobile m = from.Mobile;
			int[] switches = info.Switches;

			if ( !m_SpecialDonateDye.IsChildOf( m.Backpack ) )
			{
				m.SendLocalizedMessage( 1042010 ); //You must have the objectin your backpack to use it.
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
						SpecialDonateDyeEntry e = m_Entries[entryIndex];

						if ( hueOffset >= 0 && hueOffset < e.HueCount )
						{
							m_SpecialDonateDye.Delete();

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