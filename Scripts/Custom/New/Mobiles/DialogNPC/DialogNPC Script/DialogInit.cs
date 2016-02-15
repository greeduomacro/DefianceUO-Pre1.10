using System;
using System.Xml.Serialization;
using System.Reflection;

using Server;

namespace Arya.DialogEditor
{
	[ Serializable ]
	/// <summary>
	/// Defines a beginning scenario for this dialog
	/// </summary>
	public class DialogInit
	{
		private bool m_ReactToKeywords;
		private bool m_ReactToDoubleClick;
		private bool m_ReactToItemInBackpack;
		private bool m_ReactToItemGiven;
		private string[] m_Keywords;
		private string m_TypeBackpack;
		private int m_AmountBackpack = 1;
		private string m_TypeGiven;
		private int m_AmountGiven = 1;
		private Guid m_Speech = Guid.Empty;
		private Type m_ItemType;
		private Type m_ItemGivenType;
		private bool m_TriggerFunction = false;
		private string m_FunctionType = string.Empty;
		private string m_FunctionName = string.Empty;
		private MethodInfo m_Method;

		[ XmlAttribute ]
		/// <summary>
		/// States whether the NPC will react to keywords
		/// </summary>
		public bool ReactToKeywords
		{
			get { return m_ReactToKeywords; }
			set { m_ReactToKeywords = value; }
		}

		[ XmlAttribute ]
		/// <summary>
		/// States whether the NPC will react to double clicks
		/// </summary>
		public bool ReactToDoubleClick
		{
			get { return m_ReactToDoubleClick; }
			set { m_ReactToDoubleClick = value; }
		}

		[ XmlAttribute ]
		/// <summary>
		/// States whether the NPC will react to an item in a players backpack
		/// </summary>
		public bool ReactToItemInBackpack
		{
			get { return m_ReactToItemInBackpack; }
			set { m_ReactToItemInBackpack = value; }
		}

		[ XmlAttribute ]
		/// <summary>
		/// States whether the NPC will react to an item given to him
		/// </summary>
		public bool ReactToItemGiven
		{
			get { return m_ReactToItemGiven; }
			set { m_ReactToItemGiven = value; }
		}

		/// <summary>
		/// Gets or sets the list of keywords this NPC will react to
		/// </summary>
		public string[] Keywords
		{
			get { return m_Keywords; }
			set { m_Keywords = value; }
		}

		[ XmlAttribute ]
		/// <summary>
		/// Type of item in the player backpack the NPC will react to
		/// </summary>
		public string TypeBackpack
		{
			get { return m_TypeBackpack; }
			set { m_TypeBackpack = value; }
		}

		[ XmlAttribute ]
		/// <summary>
		/// Type of item NPC will react to if given
		/// </summary>
		public string TypeGiven
		{
			get { return m_TypeGiven; }
			set { m_TypeGiven = value; }
		}

		[ XmlAttribute ]
		/// <summary>
		/// Amount of given item the NPC reacts to
		/// </summary>
		public int AmountBackpack
		{
			get { return m_AmountBackpack; }
			set { m_AmountBackpack = value; }
		}

		[ XmlAttribute ]
		/// <summary>
		/// Amount of given item the NPC reacts to
		/// </summary>
		public int AmountGiven
		{
			get { return m_AmountGiven; }
			set { m_AmountGiven = value; }
		}

		[ XmlAttribute ]
		/// <summary>
		/// Gets or sets the speech ID that corresponds to this condition
		/// </summary>
		public Guid Speech
		{
			get { return m_Speech; }
			set { m_Speech = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// States whether the trigger will also check a custom defined function
			/// </summary>
		public bool TriggerFunction
		{
			get { return m_TriggerFunction; }
			set { m_TriggerFunction = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Gets or sets the name of the custom function that will be called by this trigger
			/// </summary>
		public string FunctionName
		{
			get { return m_FunctionName; }
			set { m_FunctionName = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Gets or sets the type that contains the function
			/// </summary>
		public string FunctionType
		{
			get { return m_FunctionType; }
			set { m_FunctionType = value; }
		}

		[ XmlIgnore ]
		/// <summary>
		/// Gets the type of the item required for the Item in Backpack reaction
		/// </summary>
		public Type ItemType
		{
			get { return m_ItemType; }
		}

		[ XmlIgnore ]
		/// <summary>
		/// Gets the type of the item required for the Item dropped reaction
		/// </summary>
		public Type ItemGivenType
		{
			get { return m_ItemGivenType; }
		}

		public DialogInit()
		{
		}

		/// <summary>
		/// Verifies if the types for the player backpack and
		/// </summary>
		public void ValidateTypes()
		{
			Type t = null;

			// Item in backpack
			if ( m_ReactToItemInBackpack && m_TypeBackpack != null )
			{
				t = ScriptCompiler.FindTypeByName( m_TypeBackpack, true );

				if ( t == null )
					t = ScriptCompiler.FindTypeByFullName( m_TypeBackpack, true );

				m_ItemType = t;

				if ( t == null ) // If the item doesn't exist, don't bother checking
				{
					m_ReactToItemInBackpack = false;
				}
			}

			// Item given
			if ( m_ReactToItemGiven && m_TypeGiven != null )
			{
				t = ScriptCompiler.FindTypeByName( m_TypeGiven, true );

				if ( t == null )
					t = ScriptCompiler.FindTypeByFullName( m_TypeGiven, true );

				m_ItemGivenType = t;

				if ( t == null )
				{
					m_ReactToItemGiven = false;
				}
			}

			// Function
			if ( m_TriggerFunction )
			{
				t = DialogNPC.FindType( m_FunctionType );
				if ( t == null )
				{
					m_TriggerFunction = false;
					return;
				}

				m_Method = t.GetMethod( m_FunctionName, new Type[] { typeof( DialogNPC ), typeof( Mobile ) } );

				if ( m_Method == null )
					m_TriggerFunction = false;
			}
		}

		/// <summary>
		/// Performs a custom trigger verification
		/// </summary>
		/// <param name="npc"></param>
		/// <param name="m"></param>
		/// <returns></returns>
		public bool DoFunctionTrigger( DialogNPC npc, Mobile m )
		{
			if ( ! m_TriggerFunction )
				return true;

			try
			{
				bool result = (bool) m_Method.Invoke( null, new object[] { m, npc } );
				return result;
			}
			catch {}

			return false;
		}
	}
}