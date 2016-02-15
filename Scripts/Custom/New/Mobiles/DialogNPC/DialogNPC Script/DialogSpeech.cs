using System;
using System.Xml.Serialization;
using System.Collections;

namespace Arya.DialogEditor
{
	[ Serializable ]
	/// <summary>
	/// Defines a speech pronounced by the NPC
	/// </summary>
	public class DialogSpeech
	{
		private string m_Text;
		private Guid m_ID;
		private ArrayList m_Choices;
		private string m_Title = "Section Title";
		private Guid m_Parent = Guid.Empty;

		[ XmlAttribute ]
		/// <summary>
		/// Gets or sets this speech ID
		/// </summary>
		public Guid ID
		{
			get { return m_ID; }
			set { m_ID = value; }
		}

		[ XmlAttribute ]
		/// <summary>
		/// Gets or sets the parent choice of this speech
		/// </summary>
		public Guid Parent
		{
			get { return m_Parent; }
			set { m_Parent = value; }
		}

		[ XmlAttribute ]
		/// <summary>
		/// Gets or sets the lines
		/// </summary>
		public string Text
		{
			get { return m_Text; }
			set { m_Text = value; }
		}

		/// <summary>
		/// Gets or sets the list of choice GUIDs
		/// </summary>
		public ArrayList Choices
		{
			get { return m_Choices; }
			set { m_Choices = value; }
		}

		[ XmlAttribute ]
		/// <summary>
		/// Gets or sets a title for this speech
		/// </summary>
		public string Title
		{
			get { return m_Title; }
			set { m_Title = value; }
		}

		public DialogSpeech()
		{
			m_ID = Guid.NewGuid();
			m_Choices = new ArrayList();
		}
	}
}