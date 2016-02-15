using System;
using System.Xml.Serialization;
using System.Collections;

namespace Arya.DialogEditor
{
	[ Serializable ]
	/// <summary>
	/// Holds various properties set on the NPC
	/// </summary>
	public class NPCProps
	{
		private string m_AI = "Thief";
		private string m_FightMode = "None";
		private int m_Fame = 0;
		private int m_Karma = 0;
		private string m_Damage = string.Empty;
		private string[] m_Resistances;
		private string[] m_Damages;
		private string m_Hits = string.Empty;
		private string m_Stam = string.Empty;
		private string m_Mana = string.Empty;
		private string m_Mount = string.Empty;
		private int m_MountHue = 0;
		private ArrayList m_Properties;
		private ArrayList m_Values;

		[ XmlAttribute ]
			/// <summary>
			/// Gets or sets the NPC's AI
			/// </summary>
		public string AI
		{
			get { return m_AI; }
			set { m_AI = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Gets or sets the NPC's FightMode
			/// </summary>
		public string FightMode
		{
			get { return m_FightMode; }
			set { m_FightMode = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Gets or sets the NPC's Fame
			/// </summary>
		public int Fame
		{
			get { return m_Fame; }
			set { m_Fame = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Gets or sets the NPC's Karma
			/// </summary>
		public int Karma
		{
			get { return m_Karma; }
			set { m_Karma = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Gets or sets the NPC's Damage
			/// </summary>
		public string Damage
		{
			get { return m_Damage; }
			set { m_Damage = value; }
		}

		/// <summary>
		/// Gets or sets the NPC's Resistances
		/// </summary>
		public string[] Resistances
		{
			get { return m_Resistances; }
			set { m_Resistances = value; }
		}

		/// <summary>
		/// Gets or sets the NPC's Damages
		/// </summary>
		public string[] Damages
		{
			get { return m_Damages; }
			set { m_Damages = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Gets or sets the NPC's Hits
			/// </summary>
		public string Hits
		{
			get { return m_Hits; }
			set { m_Hits = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Gets or sets the NPC's Stam
			/// </summary>
		public string Stam
		{
			get { return m_Stam; }
			set { m_Stam = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Gets or sets the NPC's Mana
			/// </summary>
		public string Mana
		{
			get { return m_Mana; }
			set { m_Mana = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Gets or sets the NPC's AI
			/// </summary>
		public string Mount
		{
			get { return m_Mount; }
			set { m_Mount = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Gets or sets the NPC's MountHue
			/// </summary>
		public int MountHue
		{
			get { return m_MountHue; }
			set { m_MountHue = value; }
		}

		/// <summary>
		/// Gets or sets the NPC's Properties
		/// </summary>
		public ArrayList Properties
		{
			get { return m_Properties; }
			set { m_Properties = value; }
		}

		/// <summary>
		/// Gets or sets the values corresponding to the properties list
		/// </summary>
		public ArrayList Values
		{
			get { return m_Values; }
			set { m_Values = value; }
		}

		public NPCProps()
		{
			m_Damages = new string [ 5 ];
			m_Resistances = new string [ 5 ];

			for ( int i = 0; i < 5; i++ )
			{
				m_Damages[ i ] = string.Empty;
				m_Resistances[ i ] = string.Empty;
			}

			m_Properties = new ArrayList();
			m_Values = new ArrayList();
		}
	}
}