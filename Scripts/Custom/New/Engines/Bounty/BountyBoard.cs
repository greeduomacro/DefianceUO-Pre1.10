using System;
using System.Collections;
using System.IO;
using System.Text;
using Server;
using Server.Misc;
using Server.Network;

namespace Server.Items
{
	public class BountyMessage : Item
	{
		private BountyTable.BountyEntry m_BountyEntry;
		public BountyTable.BountyEntry BountyEntry{ get{ return m_BountyEntry; } }

		public BountyMessage( BountyTable.BountyEntry bountyentry ) : base( 0xEB0 )
		{
			m_BountyEntry = bountyentry;
		}

		public BountyMessage( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	[Flipable( 0x1E5E, 0x1E5F )]
	public class BountyBoard : BaseBulletinBoard
	{
		private static ArrayList m_Items;
		public static new ArrayList Items
		{
			get
			{
				if ( m_Items == null )
					m_Items = new ArrayList();
				return m_Items;
			}
		}
		[Constructable]
		public BountyBoard() : base( 0x1E5E )
		{
			BoardName = "bounty board";
			Name = "a bounty board";
		}

		public BountyBoard( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override void OnSingleClick( Mobile from )
		{
			LabelTo( from, String.Format( "a bounty board with {0} posted bounties", BountyTable.SortedBountyList.Count ) );
		}

		public override void OnDoubleClick( Mobile from )
		{
			BountyTable.RefreshBountyBoard();
			if ( CheckRange( from ) )
			{
				//Cleanup();
				//from.SendMessage( "moo...");
				Console.WriteLine( BountyTable.SortedBountyList.Count.ToString() );
				from.Send( new BBDisplayBoard( this ) );

				if ( from.NetState.IsPost6017 )
					from.Send( new BountyBoardContent6017( from, this ) );
				else
					from.Send( new BountyBoardContent( from, this ) );
			}
			else
			{
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
			}
		}
	}

	public class BountyMessageHeader : Packet
	{
		public BountyMessageHeader( BaseBulletinBoard board, BountyMessage msg ) : base( 0x71 )
		{
			BountyTable.BountyEntry be = msg.BountyEntry;

			string poster = "";//be.Owner.Name;
			string subject = String.Format( "{0}: {1} gold", be.Owner.Name, BountyTable.Bounty( be.Owner ) );
			string time = String.Format( "{0} kills", be.Owner.ShortTermMurders );

			EnsureCapacity( 22 + poster.Length + subject.Length + time.Length );

			m_Stream.Write( (byte) 0x01 ); // PacketID
			m_Stream.Write( (int) board.Serial ); // Bulletin board serial
			m_Stream.Write( (int) msg.Serial ); // Message serial

			m_Stream.Write( (int) 0 ); // Thread serial--root

			WriteString( "" );
			WriteString( subject );
			WriteString( time );
			/*WriteString( poster );
			WriteString( subject );
			WriteString( time );*/
		}

		public void WriteString( string v )
		{
			byte[] buffer = Utility.UTF8.GetBytes( v );
			int len = buffer.Length + 1;

			if ( len > 255 )
				len = 255;

			m_Stream.Write( (byte) len );
			m_Stream.Write( buffer, 0, len-1 );
			m_Stream.Write( (byte) 0 );
		}

		public string SafeString( string v )
		{
			if ( v == null )
				return String.Empty;

			return v;
		}
	}

	public class BountyMessageContent : Packet
	{
		public BountyMessageContent( BaseBulletinBoard board, BountyMessage msg ) : base( 0x71 )
		{
			BountyTable.BountyEntry be = msg.BountyEntry;


			string poster = "a royal guard";
			string subject = String.Format( "{0}: {1}gp [{2} kills]", be.Owner.Name, BountyTable.Bounty( be.Owner ), be.Owner.ShortTermMurders );
			string time = "";

			EnsureCapacity( 22 + poster.Length + subject.Length + time.Length );

			m_Stream.Write( (byte) 0x02 ); // PacketID
			m_Stream.Write( (int) board.Serial ); // Bulletin board serial
			m_Stream.Write( (int) msg.Serial ); // Message serial

			WriteString( poster );
			WriteString( subject );
			WriteString( time );

			m_Stream.Write( (short) 400 );
			m_Stream.Write( (short) 1015 );

			m_Stream.Write( (byte) 0 );



			/*int len = 1;
			m_Stream.Write( (byte) len );
			WriteString( String.Format( "The foul scum known as {0} cannot continue to kill! For {1} is responsible for {2} murder{3}. Lord British's bounty of {4} gold pieces will be paid out in exchange for the swine's head.", be.Owner.Name, be.Owner.Female ? "she" : "he", be.Owner.ShortTermMurders, be.Owner.ShortTermMurders > 1 ? "s" : "", BountyTable.Bounty( be.Owner ) ) );*/

            m_Stream.Write((byte) 3);//Length

            WriteString(String.Format("The foul scum {0} must hang!", be.Owner.Name));
            WriteString(String.Format("for they committed {0} murders.", be.Owner.Kills));
            WriteString(String.Format("claim {0} gp for their head.", BountyTable.Bounty(be.Owner)));

			/*
			BountyTable.BountyEntry be = msg.BountyEntry;

			string poster = "a royal guard";
			string subject = String.Format( "{0}: {1}gp [{2} kills]", be.Owner.Name, BountyTable.Bounty( be.Owner ), be.Owner.Kills );
			string time = "-";

			Console.WriteLine( subject );

			EnsureCapacity( 22 + poster.Length + subject.Length + time.Length );

			m_Stream.Write( (byte) 0x02 ); // PacketID
			m_Stream.Write( (int) board.Serial ); // Bulletin board serial
			m_Stream.Write( (int) msg.Serial ); // Message serial

			WriteString( poster ); // poster
			WriteString( subject ); // subject
			WriteString( time ); // time

			m_Stream.Write( (short) 1 );
			m_Stream.Write( (short) 1 );

			//int len = msg.PostedEquip.Length;

			//if ( len > 255 )
			//	len = 255;

			//m_Stream.Write( (byte) len );

			//for ( int i = 0; i < len; ++i )
			//{
			//	BulletinEquip eq = msg.PostedEquip[i];

			//	m_Stream.Write( (short) eq.itemID );
			//	m_Stream.Write( (short) eq.hue );
			//}

			len = 3;

			m_Stream.Write( (byte) len );

			WriteString( String.Format( "The fould scum known as {0} cannot continue to kill!", be.Owner.Name ) );
			WriteString( String.Format( "For he is responsible for {0} murders.", be.Owner.Kills ) );
			WriteString( String.Format( "Lord British's bounty of {0} gold pieces will be paid out in exchange for the swine's head.", BountyTable.Bounty( be.Owner ) ) );

			//for ( int i = 0; i < len; ++i )
			//	WriteString( msg.Lines[i] );*/
		}

		public void WriteString( string v )
		{
			byte[] buffer = Utility.UTF8.GetBytes( v );
			int len = buffer.Length + 1;

			if ( len > 255 )
				len = 255;

			m_Stream.Write( (byte) len );
			m_Stream.Write( buffer, 0, len-1 );
			m_Stream.Write( (byte) 0 );
		}

		public string SafeString( string v )
		{
			if ( v == null )
				return String.Empty;

			return v;
		}
	}

	public sealed class BountyBoardContent : Packet
	{
		public BountyBoardContent( Mobile beholder, BountyBoard board ) : base( 0x3C )
		{
			ArrayList items = BountyTable.BountyMessages;
			int count = items.Count;

			this.EnsureCapacity( 5 + (count * 19) );

			long pos = m_Stream.Position;

			int written = 0;

			m_Stream.Write( (ushort) 0 );

			for ( int i = 0; i < count; ++i )
			{
				BountyMessage child = (BountyMessage)items[i];

				//Point3D loc = child.Location;

				ushort cid = (ushort) 0xEB0;

				if ( cid > 0x3FFF )
					cid = 0x9D7;

				m_Stream.Write( (int) child.Serial );
				m_Stream.Write( (ushort) cid );
				m_Stream.Write( (byte) 0 ); // signed, itemID offset
				m_Stream.Write( (ushort) 1 );
				m_Stream.Write( (short) 0 );
				m_Stream.Write( (short) 0 );
				m_Stream.Write( (int) board.Serial );
				m_Stream.Write( (ushort) 0 );

				++written;
			}

			m_Stream.Seek( pos, SeekOrigin.Begin );
			m_Stream.Write( (ushort) written );
		}
	}

	public sealed class BountyBoardContent6017 : Packet
	{
		public BountyBoardContent6017( Mobile beholder, BountyBoard board ) : base( 0x3C )
		{
			ArrayList items = BountyTable.BountyMessages;
			int count = items.Count;

			this.EnsureCapacity( 5 + (count * 19) );

			long pos = m_Stream.Position;

			int written = 0;

			m_Stream.Write( (ushort) 0 );

			for ( int i = 0; i < count; ++i )
			{
				BountyMessage child = (BountyMessage)items[i];

				//Point3D loc = child.Location;

				ushort cid = (ushort) 0xEB0;

				if ( cid > 0x3FFF )
					cid = 0x9D7;

				m_Stream.Write( (int) child.Serial );
				m_Stream.Write( (ushort) cid );
				m_Stream.Write( (byte) 0 ); // signed, itemID offset
				m_Stream.Write( (ushort) 1 );
				m_Stream.Write( (short) 0 );
				m_Stream.Write( (short) 0 );
				m_Stream.Write( (byte) 0 ); //Grid Location
				m_Stream.Write( (int) board.Serial );
				m_Stream.Write( (ushort) 0 );

				++written;
			}

			m_Stream.Seek( pos, SeekOrigin.Begin );
			m_Stream.Write( (ushort) written );
		}
	}
}