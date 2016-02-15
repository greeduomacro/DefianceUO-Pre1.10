using System;
using System.Collections;
using System.IO;
using Server.Accounting;
using Server.Gumps;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Misc
{
	public class FriendSystem
	{
		private static Hashtable m_FriendLists;
		private static Hashtable FriendLists
		{
			get
			{
				if ( m_FriendLists == null )
					m_FriendLists = new Hashtable();
				return m_FriendLists;
			}
		}

		public class FriendList
		{
			public Mobile m_Mobile;
			private ArrayList m_Friends;

			public ArrayList Friends
			{
				get
				{
					if ( m_Friends == null )
						m_Friends = new ArrayList();
					for ( int i = 0; i < m_Friends.Count; i++ )
					{
						if ( m_Friends[i] == null || !(m_Friends[i] is Mobile) )
						{
							m_Friends.Remove( m_Friends[i] );
							i--;
						}
					}
					return m_Friends;
				}
			}

			public ArrayList MutualFriends
			{
				get
				{
					ArrayList mf = new ArrayList();
					foreach ( Mobile m in Friends )
					{
						if ( FriendSystem.GetFriendList( m ).Friends.Contains( m_Mobile ) )
							mf.Add( m );
					}
					/*for ( int i = 0; i < 30; i++ )
						mf.Add( m_Mobile );*/
					return mf;
				}
			}

			public FriendList()
			{
				m_Mobile = null;
			}

			public FriendList( Mobile mob )
			{
				m_Mobile = mob;
			}

			public void Serialize( GenericWriter writer )
			{
				writer.Write( (int)0 );

				writer.Write( m_Mobile );
				writer.Write( Friends.Count );
				for ( int i = 0; i < Friends.Count; i++ )
					writer.Write( (Mobile)Friends[i] );
			}

			public void Deserialize( GenericReader reader )
			{
				int version = reader.ReadInt();

				switch ( version )
				{
					case 0:
					{
						m_Mobile = reader.ReadMobile();
						int friends = reader.ReadInt();
						for ( int i = 0; i < friends; i++ )
							Friends.Add( reader.ReadMobile() );
						break;
					}
				}
			}

			public bool AddFriend( Mobile target )
			{
				if ( !Friends.Contains( target ) && target != m_Mobile )
				{
					Friends.Add( target );
					return true;
				}
				else
					return false;
			}

			public void RemoveFriend( Mobile target )
			{
				if ( Friends.Contains( target ) )
				{
					Friends.Remove( target );
					FriendSystem.RemoveFriend( target, m_Mobile );
				}

			}
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
			string idxPath = Path.Combine( "Saves/FriendLists", "FriendLists.idx" );
			string binPath = Path.Combine( "Saves/FriendLists", "FriendLists.bin" );

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

					FriendList fl = new FriendList();
					// Read start-position and length of current order from index file
					long startPos = idxReader.ReadInt64();
					int length = idxReader.ReadInt32();
					// Point the reading stream to the proper position
					binReader.Seek(startPos, SeekOrigin.Begin);

					try
					{
						fl.Deserialize(binReader);

						if (binReader.Position != (startPos + length))
							throw new Exception(String.Format("***** Bad serialize on FriendList[{0}] *****", i));
					}
					catch
					{
						//handle
					}
					if ( fl != null && fl.m_Mobile != null )
						FriendLists.Add( fl.m_Mobile, fl );
				}
				// Remember to close the streams
				idxReader.Close();
				binReader.Close();
			}

		}


		public static void Save()
		{
			if (!Directory.Exists("Saves/FriendLists/"))
				Directory.CreateDirectory("Saves/FriendLists/");

			string idxPath = Path.Combine( "Saves/FriendLists", "FriendLists.idx" );
			string binPath = Path.Combine( "Saves/FriendLists", "FriendLists.bin" );


			GenericWriter idx = new BinaryFileWriter(idxPath, false);
			GenericWriter bin = new BinaryFileWriter(binPath, true);

			idx.Write( (int)FriendLists.Values.Count );
			foreach ( FriendList fl in FriendLists.Values )
			{
				long startPos = bin.Position;
				fl.Serialize( bin );
				idx.Write( (long)startPos );
				idx.Write( (int)(bin.Position - startPos) );
			}
			idx.Close();
			bin.Close();
		}

		public static FriendList GetFriendList( Mobile m )
		{
			if ( m == null )
				return null;
			FriendList fl = (FriendList)FriendLists[m];
			if ( fl == null )
			{
				fl = new FriendList( m );
				FriendLists.Add( m, fl );
			}
			return fl;
		}

		public static bool AddFriend( Mobile from, Mobile target )
		{
			if ( from != null && target != null )
			{
				FriendList fl = GetFriendList( from );
				return fl.AddFriend( target );
			}
			return false;
		}

		public static void RemoveFriend( Mobile from, Mobile target )
		{
			if ( from != null && target != null )
			{
				FriendList fl = GetFriendList( from );
				fl.RemoveFriend( target );
			}
		}

		public static void Tell( Mobile from, Mobile to, string text )
		{
			to.SendMessage( 133, "[{0}]: {1}", from.Name, text );
			from.SendMessage( 133, "You told {0}: {1}", to.Name, text );
		}
	}
}