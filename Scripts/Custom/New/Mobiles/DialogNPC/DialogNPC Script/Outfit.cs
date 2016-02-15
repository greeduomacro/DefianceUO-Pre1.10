using System;
using System.Collections;
using System.Xml.Serialization;

namespace Arya.DialogEditor
{
	[ Serializable, XmlInclude( typeof( ItemEntry ) ) ]
	/// <summary>
	/// Describes the NPCs outfit
	/// </summary>
	public class Outfit
	{
		private bool m_CustomOutfit = false;
		private bool m_Creature;
		private int m_Hue;
		private int m_Body;
		private bool m_Blessed;
		private string m_Name = string.Empty;
		private string m_Title = string.Empty;
		private bool m_Female;
		private string m_Hair = "None";
		private int m_HairHue;
		private string m_Beard = "None";
		private int m_BeardHue;
		private bool m_Mustache;
		private int m_MustacheHue;
		private ArrayList m_Items;
		private int m_Str = 1;
		private int m_Dex = 1;
		private int m_Int = 1;
		private int[] m_Skills;
		private bool m_CustomFunction = false;
		private string m_FunctionType = string.Empty;
		private string m_Function = string.Empty;

		[ XmlAttribute ]
			/// <summary>
			/// Specifies whether to apply the custom outfit to the NPC
			/// </summary>
		public bool CustomOutfit
		{
			get { return m_CustomOutfit; }
			set { m_CustomOutfit = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Specifies whether the NPC should be blessed
			/// </summary>
		public bool Blessed
		{
			get { return m_Blessed; }
			set { m_Blessed = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Gets or sets the NPC's hue
			/// </summary>
		public int Hue
		{
			get { return m_Hue; }
			set { m_Hue = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Specifies the NPC's name
			/// </summary>
		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Specifies the NPC's title
			/// </summary>
		public string Title
		{
			get { return m_Title; }
			set { m_Title = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Specifies whether this NPC should have a non-human body
			/// </summary>
		public bool Creature
		{
			get { return m_Creature; }
			set { m_Creature = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Specifies the non-human body for this creature
			/// </summary>
		public int Body
		{
			get { return m_Body; }
			set { m_Body = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Specifies whether the NPC is female
			/// </summary>
		public bool Female
		{
			get { return m_Female; }
			set { m_Female = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Specifies the hair type
			/// </summary>
		public string Hair
		{
			get { return m_Hair; }
			set { m_Hair = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Specifies the hair hue
			/// </summary>
		public int HairHue
		{
			get { return m_HairHue; }
			set { m_HairHue = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Specifies the beard's type
			/// </summary>
		public string Beard
		{
			get { return m_Beard; }
			set { m_Beard = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Specifies the beard's hue
			/// </summary>
		public int BeardHue
		{
			get { return m_BeardHue; }
			set { m_BeardHue = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Specifies if the NPC should have mustache
			/// </summary>
		public bool Mustache
		{
			get { return m_Mustache; }
			set { m_Mustache = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Specifies the mustache's hue
			/// </summary>
		public int MustacheHue
		{
			get { return m_MustacheHue; }
			set { m_MustacheHue = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Gets or sets the NPC's STR
			/// </summary>
		public int Str
		{
			get { return m_Str; }
			set { m_Str = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Gets or sets the NPC's DEX
			/// </summary>
		public int Dex
		{
			get { return m_Dex; }
			set { m_Dex = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Gets or sets the NPC's INT
			/// </summary>
		public int Int
		{
			get { return m_Int; }
			set { m_Int = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Specifies whether the outfit should call a custom function
			/// </summary>
		public bool CustomFunction
		{
			get { return m_CustomFunction; }
			set { m_CustomFunction = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// The type that holds the custom outfit function
			/// </summary>
		public string FunctionType
		{
			get { return m_FunctionType; }
			set { m_FunctionType = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// The custom function name
			/// </summary>
		public string Function
		{
			get { return m_Function; }
			set { m_Function = value; }
		}

		/// <summary>
		/// Lists the NPC's skills
		/// </summary>
		public int[] Skills
		{
			get { return m_Skills; }
			set { m_Skills = value; }
		}

		/// <summary>
		/// Gets or sets the list of items carried by the NPC
		/// </summary>
		public ArrayList Items
		{
			get { return m_Items; }
			set { m_Items = value; }
		}

		public Outfit()
		{
			m_Items = new ArrayList();
			m_Skills = new int[ 52 ];
			m_Skills.Initialize();
		}

		public void AddItem( string type, int hue, int amount, string loot )
		{
			ItemEntry item = new ItemEntry( type, hue, amount, loot );
			m_Items.Add( item );
		}
	}
}