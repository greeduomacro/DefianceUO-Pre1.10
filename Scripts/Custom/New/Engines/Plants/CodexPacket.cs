using System;
using Server;

namespace Server.Network
{
	public class CodexOfWisdom : Packet
	{

		public CodexOfWisdom(int CodexID) : this (CodexID, true )
		{
		}

		public CodexOfWisdom(int CodexID, bool Display) : base (0xBF)
		{
		EnsureCapacity( 11 );
		m_Stream.Write( (short) 0x17 );
		m_Stream.Write( (byte) 1 );
		m_Stream.Write( (int) CodexID );
		m_Stream.Write( (bool) Display );
		}
	}
}