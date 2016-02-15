using System;
using System.Xml.Serialization;
using System.Reflection;

using Server;

namespace Arya.DialogEditor
{
	[ Serializable ]
	/// <summary>
	/// Defines a choice in the dialog
	/// </summary>
	public class DialogChoice
	{
		private Guid m_ID;
		private string m_Text = "Enter choice text";
		private Guid m_ChoiceID;
		private bool m_EndDialog = false;
		private bool m_Invoke = false;
		private string m_InvokeType;
		private string m_InvokeFunction;
		private Type m_Type;
		private MethodInfo m_Method;

		[ XmlAttribute ]
		/// <summary>
		/// Gets or sets the ID for this choice
		/// </summary>
		public Guid ID
		{
			get { return m_ID; }
			set { m_ID = value; }
		}

		[ XmlAttribute ]
		/// <summary>
		/// Gets or sets the choice Text
		/// </summary>
		public string Text
		{
			get { return m_Text; }
			set { m_Text = value; }
		}

		[ XmlAttribute ]
		/// <summary>
		/// Gets or sets the ID of the speech used when this dialog choice is made
		/// </summary>
		public Guid ChoiceID
		{
			get { return m_ChoiceID; }
			set { m_ChoiceID = value; }
		}

		[ XmlAttribute ]
		/// <summary>
		/// States whether this choice will terminate the dialog
		/// </summary>
		public bool EndDialog
		{
			get { return m_EndDialog; }
			set { m_EndDialog = value; }
		}


		[ XmlAttribute ]
		/// <summary>
		/// States whether this choice should invoke a function
		/// </summary>
		public bool Invoke
		{
			get { return m_Invoke; }
			set { m_Invoke = value; }
		}

		[ XmlAttribute ]
		/// <summary>
		/// Gets or sets the type that contains the function that should be invoked
		/// </summary>
		public string InvokeType
		{
			get { return m_InvokeType; }
			set { m_InvokeType = value; }
		}

		[ XmlAttribute ]
		/// <summary>
		/// Gets or sets the name of the function that should be invoked on this choice
		/// </summary>
		public string InvokeFunction
		{
			get { return m_InvokeFunction; }
			set { m_InvokeFunction = value; }
		}

		public DialogChoice()
		{
			m_ID = Guid.NewGuid();
		}

		/// <summary>
		/// Validates the types using for invoking functions
		/// </summary>
		public void ValidateTypes()
		{
			if ( m_InvokeType == null || m_InvokeFunction == null )
			{
				m_Invoke = false;
				return;
			}

			m_Type = DialogNPC.FindType( m_InvokeType );
			if ( m_Type == null )
			{
				m_Invoke = false;
				return;
			}

			m_Method = m_Type.GetMethod( m_InvokeFunction );

			if ( m_Method == null || !m_Method.IsStatic )
			{
				m_Type = null;
				m_Method = null;

				m_Invoke = false;
			}
		}

		/// <summary>
		/// Performs the invoke specified by this method
		/// </summary>
		/// <param name="m">The mobile using the NPC</param>
		/// <param name="npc">The NPC object</param>
		public void PerformInvoke( Mobile m, DialogNPC npc )
		{
			try
			{
				m_Method.Invoke( null, new object[] { m, npc } );
			}
			catch {}
		}
	}
}