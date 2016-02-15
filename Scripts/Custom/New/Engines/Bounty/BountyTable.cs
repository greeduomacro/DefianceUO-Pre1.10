using System;
using System.Collections;
using System.IO;
using Server.Accounting;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Misc
{
	public class BountyTable
	{
		public class BountyEntry
		{
			Mobile m_Owner;
			int m_Bounty;

			public Mobile Owner{ get{ return m_Owner; } }
			public int Bounty{ get{ return m_Bounty; } }

			public BountyEntry() // to allow serialization
			{
			}

			public BountyEntry( Mobile owner )
			{
				m_Owner = owner;
				m_Bounty = 0;
			}

			public void Add( int amount )
			{
				m_Bounty += amount;
			}

			public void Serialize( GenericWriter writer )
			{
				writer.Write( (int)0 );

				writer.Write( m_Owner );
				writer.Write( m_Bounty );
			}

			public void Deserialize( GenericReader reader )
			{
				int version = reader.ReadInt();

				switch ( version )
				{
					case 0:
					{
						m_Owner = reader.ReadMobile();
						m_Bounty = reader.ReadInt();
						break;
					}
				}
			}
		}

		private static Hashtable m_Table;

		public static Hashtable Table
		{
			get
			{
				if ( m_Table == null )
					m_Table = new Hashtable();
				return m_Table;
			}
		}

		private static ArrayList m_SortedBountyList;
		public static ArrayList SortedBountyList
		{
			get
			{
				if ( m_SortedBountyList == null )
				{
					m_SortedBountyList = new ArrayList();
					RefreshBountyBoard();
				}
				return m_SortedBountyList;
			}
		}

		private static ArrayList m_BountyMessages;
		public static ArrayList BountyMessages
		{
			get
			{
				ArrayList newlist = new ArrayList();

				if ( m_BountyMessages == null )
					m_BountyMessages = new ArrayList();

				foreach ( BountyEntry be in SortedBountyList )
				{
					BountyMessage existed = null;
					foreach ( BountyMessage bm in m_BountyMessages )
						if ( bm.BountyEntry == be )
						{
							existed = bm;
							break;
						}
					if ( existed != null )
						newlist.Add( existed );
					else
						newlist.Add( new BountyMessage( be ) );
				}

				m_BountyMessages = newlist;

				return m_BountyMessages;
				//return (BountyMessage[])m_BountyMessages.ToArray(typeof(BountyMessage));
			}
		}


		public static void Add( Mobile owner, int amount )
		{
			if ( owner == null )
				return;
			BountyEntry entry = Table[owner] as BountyEntry;
			if ( entry == null )
			{
				entry = new BountyEntry( owner );
				Table.Add( owner, entry );
			}
			entry.Add( amount );
		}

		public static void Remove( Mobile owner )
		{
			if ( owner == null )
				return;
			Table.Remove( owner );
		}

		public static void RefreshBountyBoard()
		{
			ArrayList bounties = new ArrayList();
			foreach ( BountyEntry be in Table.Values )
				bounties.Add( be );

			BountyComparer comp = new BountyComparer();

			bounties.Sort( comp );

			SortedBountyList.Clear();
			for ( int i = 0; i < 20 && i < bounties.Count; i++ )
				SortedBountyList.Add( (BountyEntry)bounties[i] );
		}

		public static int Bounty( Mobile owner )
		{
			if ( owner == null )
				return 0;
			BountyEntry entry = Table[owner] as BountyEntry;
			if ( entry != null )
				return entry.Bounty;
			else
				return 0;
		}

		public static void Initialize()
		{
			EventSink.WorldSave += new WorldSaveEventHandler( EventSink_WorldSave );
			Load();
		}

		private static void EventSink_WorldSave( WorldSaveEventArgs e )
		{
			Save();
		}

		public static void Load()
		{
			string idxPath = Path.Combine( "Saves/Custom", "Bounty.idx" );
			string binPath = Path.Combine( "Saves/Custom", "Bounty.bin" );

			if (File.Exists(idxPath) && File.Exists(binPath))
			{
				// Declare and initialize reader objects.
				FileStream idx = new FileStream(idxPath, FileMode.Open, FileAccess.Read, FileShare.Read);
				FileStream bin = new FileStream(binPath, FileMode.Open, FileAccess.Read, FileShare.Read);
				BinaryReader idxReader = new BinaryReader(idx);
				BinaryFileReader binReader = new BinaryFileReader(new BinaryReader(bin));

				// Start by reading the number of duels from an index file
				int orderCount = idxReader.ReadInt32();

				for (int i = 0; i < orderCount; ++i)
				{
					BountyEntry be = new BountyEntry();
					// Read start-position and length of current order from index file
					long startPos = idxReader.ReadInt64();
					int length = idxReader.ReadInt32();
					// Point the reading stream to the proper position
					binReader.Seek(startPos, SeekOrigin.Begin);

					try
					{
						be.Deserialize(binReader);

						if (binReader.Position != (startPos + length))
							throw new Exception(String.Format("***** Bad serialize on Bounty[{0}] *****", i));
					}
					catch
					{
						//handle
					}
					if ( be != null && be.Owner != null )
						Table.Add( be.Owner, be );
				}
				// Remember to close the streams
				idxReader.Close();
				binReader.Close();
			}

		}

		public static void Save()
		{
			if (!Directory.Exists("Saves/Custom/"))
				Directory.CreateDirectory("Saves/Custom/");

			string idxPath = Path.Combine( "Saves/Custom", "Bounty.idx" );
			string binPath = Path.Combine( "Saves/Custom", "Bounty.bin" );

			GenericWriter idx = new BinaryFileWriter(idxPath, false);
			GenericWriter bin = new BinaryFileWriter(binPath, true);

			idx.Write( (int)Table.Values.Count );
			foreach ( BountyEntry be in Table.Values )
			{
				long startPos = bin.Position;
				be.Serialize( bin );
				idx.Write( (long)startPos );
				idx.Write( (int)(bin.Position - startPos) );
			}
			idx.Close();
			bin.Close();
		}

		public class BountyComparer : IComparer
		{
			public int Compare( object a, object b )
			{
				if ( !( a is BountyEntry ) || !( b is BountyEntry ) )
					return 0;
				BountyEntry bea = (BountyEntry)a;
				BountyEntry beb = (BountyEntry)b;
				if ( bea.Bounty > beb.Bounty )
					return -1;
				else if ( bea.Bounty < beb.Bounty )
					return 1;
				else
					return 0;
			}
		}
	}
}