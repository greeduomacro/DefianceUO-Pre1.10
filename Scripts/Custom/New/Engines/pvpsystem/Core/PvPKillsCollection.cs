using System;
using System.Collections;

namespace Server.FSPvpPointSystem
{
	public class PvPKillsCollection : CollectionBase
	{
		public PvPKillsCollection()
		{
		}

		public PvPKillEntry this[int index]
		{
			get
			{
				return (PvPKillEntry)List[index];
			}
			set
			{
				List[index] = value;
			}
		}

		public int Add( PvPKillEntry value )
		{
			return List.Add( value );
		}

		public bool Contains( PvPKillEntry value )
		{
			return List.Contains( value );
		}

		public int IndexOf( PvPKillEntry value )
		{
			return List.IndexOf( value );
		}

		public void Remove( PvPKillEntry value )
		{
			List.Remove( value );
		}

		public new PvPKillsCollectionEnumerator GetEnumerator()
		{
			return new PvPKillsCollectionEnumerator( this );
		}

		public void Insert( int index, PvPKillEntry value )
		{
			List.Insert( index, value );
		}

		public class PvPKillsCollectionEnumerator : IEnumerator
		{

			private int m_Index;
			private PvPKillEntry m_CurrentElement;
			private PvPKillsCollection m_Collection;

			internal PvPKillsCollectionEnumerator( PvPKillsCollection collection )
			{
				m_Index = -1;
				m_Collection = collection;
			}

			public PvPKillEntry Current
			{
				get
				{
					if ( ( ( m_Index == -1 ) || ( m_Index >= m_Collection.Count ) ) )
						throw new System.IndexOutOfRangeException("Enumerator not started.");
					else
						return m_CurrentElement;
				}
			}

			object IEnumerator.Current
			{
				get
				{
					if ( ( ( m_Index == -1 ) || ( m_Index >= m_Collection.Count ) ) )
						throw new System.IndexOutOfRangeException("Enumerator not started.");
					else
						return m_CurrentElement;
				}
			}

			public void Reset()
			{
				m_Index = -1;
				m_CurrentElement = null;
			}

			public bool MoveNext()
			{
				if ( ( m_Index < ( m_Collection.Count - 1 ) ) )
				{
					m_Index++;
					m_CurrentElement = m_Collection[m_Index];
					return true;
				}

				m_Index = m_Collection.Count;
				return false;
			}
		}
	}
}