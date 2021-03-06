using System;
using System.Net;
using System.Net.Sockets;
using Server;
using Server.Network;

namespace Server.Misc
{
	public class ServerList
	{
        public const string Address = StaticConfiguration.ServerAddress;

        public const string ServerName = StaticConfiguration.ServerName;

		public static void Initialize()
		{
            Listener.Port = StaticConfiguration.ServerPort;

			EventSink.ServerList += new ServerListEventHandler( EventSink_ServerList );
		}

		public static void EventSink_ServerList( ServerListEventArgs e )
		{
			try
			{
				IPAddress ipAddr;

				if ( Resolve( Address != null && !IsLocalMachine( e.State ) ? Address : Dns.GetHostName(), out ipAddr ) )
					e.AddServer( ServerName, new IPEndPoint( ipAddr, Listener.Port ) );
				else
					e.Rejected = true;
			}
			catch
			{
				e.Rejected = true;
			}
		}

		public static bool Resolve( string addr, out IPAddress outValue )
		{
			try
			{
				outValue = IPAddress.Parse( addr );
				return true;
			}
			catch
			{
				try
				{
					IPHostEntry iphe = Dns.Resolve( addr );

					if ( iphe.AddressList.Length > 0 )
					{
						outValue = iphe.AddressList[iphe.AddressList.Length - 1];
						return true;
					}
				}
				catch
				{
				}
			}

			outValue = IPAddress.None;
			return false;
		}

		private static bool IsLocalMachine( NetState state )
		{
			Socket sock = state.Socket;

			IPAddress theirAddress = ((IPEndPoint)sock.RemoteEndPoint).Address;

			if ( IPAddress.IsLoopback( theirAddress ) )
				return true;

			bool contains = false;
			IPHostEntry iphe = Dns.Resolve( Dns.GetHostName() );

			for ( int i = 0; !contains && i < iphe.AddressList.Length; ++i )
				contains = theirAddress.Equals( iphe.AddressList[i] );

			return contains;
		}
	}
}