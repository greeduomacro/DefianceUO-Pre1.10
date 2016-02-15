using System;
using System.Collections;
using System.IO;

using Server;
using Server.Gumps;

namespace Arya.DialogEditor
{
	/// <summary>
	/// This gump is used to select the file used to configure a DialogNPC
	/// </summary>
	public class FileSelectionGump : Gump
	{
		private const int LabelHue = 0x480;
		private const int RedHue = 0x20;

		private string[] m_Files;
		private int m_Page;
		private DialogNPC m_NPC;

		public FileSelectionGump( DialogNPC npc, Mobile m ) : this( npc, m, DialogNPC.ConfigurationFiles, 0 )
		{
		}

		private FileSelectionGump( DialogNPC npc, Mobile m, string[] files, int page ) : base( 50, 50 )
		{
			m.CloseGump( typeof( FileSelectionGump ) );
			m_Files = files;
			m_Page = page;
			m_NPC = npc;

			if ( m_Files == null )
				m_Files = new string[ 0 ];

			MakeGump();
		}

		private void MakeGump()
		{
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			this.AddPage(0);
			this.AddBackground(0, 0, 255, 310, 9300);
			this.AddLabel(5, 0, LabelHue, @"Select a configuration file...");

			// Previous Button: 1
			if ( m_Page > 0 )
			{
				this.AddButton(210, 5, 5603, 5607, 1, GumpButtonType.Reply, 0);
			}

			// Next Button: 2
			if ( m_Page * 10 + 10 < m_Files.Length )
			{
				this.AddButton(230, 5, 5601, 5605, 2, GumpButtonType.Reply, 0);
			}

			if ( m_Files.Length == 0 )
			{
				this.AddLabel( 25, 30, LabelHue, "No files available" );
			}
			else
			{
				// Choice Buttons: 10..19
				for ( int i = 0; i < 10 && m_Page * 10 + i < m_Files.Length; i++ )
				{
					int index = m_Page * 10 + i;
					string name = Path.GetFileNameWithoutExtension( m_Files[ index ] );

					this.AddButton(5, 32 + i * 20, 5601, 5605, 10 + i, GumpButtonType.Reply, 0);
					this.AddLabelCropped( 25, 30 + i * 20, 220, 20, LabelHue, name );
				}
			}

			this.AddImageTiled(5, 240, 243, 1, 9274);
			this.AddLabel(5, 240, LabelHue, @"Current Configuration:");
			this.AddLabel(5, 255, RedHue, m_NPC.DialogName);
			this.AddImageTiled(5, 275, 243, 1, 9274);

			// Clear NPC : Button 3
			this.AddButton( 5, 282, 5601, 5605, 3, GumpButtonType.Reply, 0 );
			this.AddLabel( 25, 280, LabelHue, "Clear NPC" );


			this.AddButton(190, 282, 5601, 5605, 0, GumpButtonType.Reply, 0);
			this.AddLabel(210, 280, LabelHue, @"Close");
		}

		public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
		{
			if ( m_NPC == null || m_NPC.Deleted )
			{
				sender.Mobile.SendMessage( 0x40, "The NPC has been deleted" );
				return;
			}

			switch ( info.ButtonID )
			{
				case 0: // Cancel and do nothing

					break;

				case 1: // Prev Page

					sender.Mobile.SendGump( new FileSelectionGump( m_NPC, sender.Mobile, m_Files, --m_Page ) );
					break;

				case 2: // Next Page

					sender.Mobile.SendGump( new FileSelectionGump( m_NPC, sender.Mobile, m_Files, ++m_Page ) );
					break;

				case 3: // Clear NPC

					m_NPC.SetFileName( null, sender.Mobile );
					break;

				default:
					int index = m_Page * 10 + info.ButtonID - 10;
					if ( index > 0 && index <= m_Files.Length )
						m_NPC.SetFileName( m_Files[ index ], sender.Mobile );
					break;
			}
		}
	}
}