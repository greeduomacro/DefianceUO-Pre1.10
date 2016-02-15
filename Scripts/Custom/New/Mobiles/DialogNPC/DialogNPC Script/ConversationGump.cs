using System;
using System.Collections;

using Server;
using Server.Gumps;

namespace Arya.DialogEditor
{
	/// <summary>
	/// This gump displays the converstiong for the DialogNPC
	/// </summary>
	public class ConversationGump : Gump
	{
		private const int LabelHue = 0x480;
		private const int GreenHue = 0x40;

		private Dialog m_Dialog;
		private ArrayList m_Choices;
		private DialogSpeech m_Speech;
		private DialogNPC m_NPC;

		public ConversationGump( Dialog dialog, DialogSpeech speech, ArrayList choices, DialogNPC npc, Mobile m ) : base( 100, 50 )
		{
			m_NPC = npc;
			m_Choices = choices;
			m_Dialog = dialog;
			m_Speech = speech;

			m.CloseGump( typeof( ConversationGump ) );

			MakeGump();
		}

		private void MakeGump()
		{
			this.Closable=m_Dialog.AllowClose;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			this.AddPage(0);
			this.AddBackground(0, 0, 340, 470, 9270);
			this.AddAlphaRegion(0, 0, 340, 470);

			// Dialog title
			this.AddLabel(15, 15, GreenHue, m_Dialog.Title != null ? m_Dialog.Title : "" );

			// Speech title
			this.AddLabel(15, 45, LabelHue, m_Speech.Text != null ? m_Speech.Title : "" );

			this.AddImageTiled( 14, 69, 312, 202, 9384 );
			this.AddImageTiled( 15, 70, 310, 200, 9274 );
			this.AddAlphaRegion( 15, 70, 310, 200 );

			// Speech text
			this.AddHtml( 15, 70, 310, 200, m_Speech.Text != null ? m_Speech.Text : "" , (bool)false, (bool)true);

			for ( int i = 0; i < 8 && i < m_Choices.Count; i++ )
			{
				DialogChoice choice = m_Choices[ i ] as DialogChoice;

				this.AddButton(15, 282 + i * 20, 9702, 9703, 10 + i, GumpButtonType.Reply, 0);
				this.AddLabelCropped( 35, 280 + i * 20, 310, 20, LabelHue, choice.Text );
			}

			if ( m_Dialog.AllowClose )
			{
				this.AddButton(190, 442, 9702, 9703, 0, GumpButtonType.Reply, 0);
				this.AddLabel(210, 440, LabelHue, @"End Conversation");
			}
		}

		public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
		{
			if ( info.ButtonID == 0 )
			{
				m_NPC.EndConversation( sender.Mobile );
			}
			else
			{
				if ( info.ButtonID - 10 >= 0 && info.ButtonID - 10 < m_Choices.Count )
				{
					DialogChoice choice = m_Choices[ info.ButtonID - 10 ] as DialogChoice;
					m_NPC.PerformChoice( sender.Mobile, choice );
				}
			}
		}
	}
}