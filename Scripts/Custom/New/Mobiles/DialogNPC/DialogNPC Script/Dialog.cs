using System;
using System.Xml.Serialization;
using System.Collections;
using System.IO;

namespace Arya.DialogEditor
{
	[ Serializable, XmlInclude( typeof( DialogSpeech ) ), XmlInclude( typeof( DialogChoice ) ), XmlInclude( typeof( DialogInit ) ) ]
	/// <summary>
	/// Defines the dialog structure
	/// </summary>
	public class Dialog
	{
		private Hashtable m_Speech;
		private Hashtable m_Choice;
		private ArrayList m_Start;
		private ArrayList m_Init;
		private ArrayList m_SpeechList;
		private ArrayList m_ChoiceList;
		private bool m_AllowClose;
		private string m_Title = "New Dialog";
		private int m_Range = 2;
		private int m_SpeechRange = 6;
		private Outfit m_Outfit;
		private NPCProps m_Props;

		[ XmlIgnore ]
		/// <summary>
		/// Gets or sets the list of speech entries
		/// </summary>
		public Hashtable SpeechList
		{
			get { return m_Speech; }
			set { m_Speech = value; }
		}

		[ XmlIgnore ]
		/// <summary>
		/// Gets or sets the list of choices in the system
		/// </summary>
		public Hashtable ChoiceList
		{
			get { return m_Choice; }
			set { m_Choice = value; }
		}

		/// <summary>
		/// Gets or sets the root speech entries
		/// </summary>
		public ArrayList Start
		{
			get { return m_Start; }
			set { m_Start = value; }
		}

		/// <summary>
		/// Gets or sets the list of initial triggers
		/// </summary>
		public ArrayList Init
		{
			get { return m_Init; }
			set { m_Init = value; }
		}

		[ XmlAttribute ]
		/// <summary>
		/// Gets or sets the title of the whole dialog
		/// </summary>
		public string Title
		{
			get { return m_Title; }
			set { m_Title = value; }
		}

		[ XmlAttribute ]
		/// <summary>
		/// Specifies whether the End Conversation button will be available
		/// </summary>
		public bool AllowClose
		{
			get { return m_AllowClose; }
			set { m_AllowClose = value; }
		}

		/// <summary>
		/// Gets or sets the list of speech texts in this dialog. Used only for serialization purposes.
		/// </summary>
		public ArrayList Speech
		{
			get { return m_SpeechList; }
			set
			{
				m_SpeechList = value;
			}
		}

		/// <summary>
		/// Gets or sets the list of choices in this dialog. Used only for serialization purposes.
		/// </summary>
		public ArrayList Choice
		{
			get { return m_ChoiceList; }
			set
			{
				m_ChoiceList = value;
			}
		}

		[ XmlAttribute ]
		/// <summary>
		/// Gets or sets the reaction range
		/// </summary>
		public int Range
		{
			get { return m_Range; }
			set { m_Range = value; }
		}

		[ XmlAttribute ]
		/// <summary>
		/// Gets or sets the speech reaction range
		/// </summary>
		public int SpeechRange
		{
			get { return m_SpeechRange; }
			set { m_SpeechRange = value; }
		}

		/// <summary>
		/// Gets or sets the NPC's outfit
		/// </summary>
		public Outfit Outfit
		{
			get { return m_Outfit; }
			set { m_Outfit = value; }
		}

		/// <summary>
		/// Gets or sets the NPC's properties configuration
		/// </summary>
		public NPCProps Props
		{
			get { return m_Props; }
			set { m_Props = value; }
		}

		public Dialog()
		{
			m_Speech = new Hashtable();
			m_Choice = new Hashtable();
			m_Start = new ArrayList();
			m_Init = new ArrayList();
			m_SpeechList = new ArrayList();
			m_ChoiceList = new ArrayList();
			m_Outfit = new Outfit();
			m_Props = new NPCProps();
		}

		/// <summary>
		/// Rebuilds the speech and choice hashtabled
		/// </summary>
		public void BuildTables()
		{
			m_Speech.Clear();
			m_Choice.Clear();

			foreach( DialogSpeech ds in m_SpeechList )
			{
				m_Speech.Add( ds.ID, ds );
			}

			foreach( DialogChoice dc in m_ChoiceList )
			{
				m_Choice.Add( dc.ID, dc );
			}
		}

		/// <summary>
		/// Verifies all the types stored in choices and inits
		/// </summary>
		private void PerformValidation()
		{
			foreach( DialogChoice choice in m_ChoiceList )
			{
				choice.ValidateTypes();
			}

			foreach( DialogInit init in m_Init )
			{
				init.ValidateTypes();
			}
		}

		public static Dialog Load( string filename )
		{
			try
			{
				XmlSerializer serializer = new XmlSerializer( typeof( Dialog ) );
				FileStream stream = new FileStream( filename, FileMode.Open, FileAccess.Read, FileShare.Read );
				Dialog d = serializer.Deserialize( stream ) as Dialog;
				stream.Close();
				d.BuildTables();
				d.PerformValidation();
				return d;
			}
			catch ( Exception err )
			{
				Console.WriteLine( "Failed to load DialogNPC file {0}. Error:", filename );
				Console.WriteLine( err.ToString() );

				return null;
			}
		}

		/// <summary>
		/// Gets a specific dialog speech
		/// </summary>
		public DialogSpeech GetSpeech( Guid id )
		{
			return m_Speech[ id ] as DialogSpeech;
		}

		/// <summary>
		/// Gets a specific dialog choice
		/// </summary>
		public DialogChoice GetChoice( Guid id )
		{
			return m_Choice[ id ] as DialogChoice;
		}
	}
}