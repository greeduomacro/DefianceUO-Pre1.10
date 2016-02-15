using System;
using System.Xml.Serialization;
using System.Reflection;

using Server;

namespace Arya.DialogEditor
{
	[ Serializable ]
	/// <summary>
	/// Describes an item carried by the NPC
	/// </summary>
	public class ItemEntry
	{
		private string m_Type;
		private int m_Hue;
		private int m_Amount = 1;
		private string m_LootType;

		[ XmlAttribute ]
			/// <summary>
			/// Gets or sets the item's type
			/// </summary>
		public string Type
		{
			get { return m_Type; }
			set { m_Type = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Gets or sets the item's hue
			/// </summary>
		public int Hue
		{
			get { return m_Hue; }
			set { m_Hue = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Gets or sets the item's amount
			/// </summary>
		public int Amount
		{
			get { return m_Amount; }
			set { m_Amount = value; }
		}

		[ XmlAttribute ]
			/// <summary>
			/// Gets or sets the item's loot type
			/// </summary>
		public string LootType
		{
			get { return m_LootType; }
			set { m_LootType = value; }
		}

		/// <summary>
		/// Gets the item described by this entry
		/// </summary>
		public Item Item
		{
			get
			{
				Type type = ScriptCompiler.FindTypeByName( m_Type, true );

				if ( type == null )
					type = ScriptCompiler.FindTypeByFullName( m_Type, true );

				if ( type == null )
					return null;

				Item item = null;

				try
				{
					item = Activator.CreateInstance( type ) as Item;
				}
				catch {}

				if ( item == null )
					return null;

				item.Hue = m_Hue;
				item.Amount = m_Amount;

				switch ( m_LootType )
				{
					case "Regular" : item.LootType = Server.LootType.Regular;
						break;

					case "Newbied" : item.LootType = Server.LootType.Newbied;
						break;

					case "Blessed" : item.LootType = Server.LootType.Blessed;
						break;

					case "Cursed" : item.LootType = Server.LootType.Cursed;
						break;
				}

				return item;
			}
		}

		public ItemEntry( string type, int hue, int amount, string loot )
		{
			m_Type = type;
			m_Hue = hue;
			m_Amount = amount;
			m_LootType = loot;
		}

		public ItemEntry()
		{
		}

		public override string ToString()
		{
			return string.Format( "{0} {1} ({2}) {3}", m_Amount, m_Type, m_Hue, m_LootType );
		}
	}
}