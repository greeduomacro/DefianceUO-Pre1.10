using System;
using System.IO;
using Server.Gumps;
using Server.Guilds;
using Server.Multis;
using Server.Network;
using System.Collections;

namespace Server.Items
{
	public class Guildstone : Item
	{
		private static ArrayList ms_DeleteCache; //Contains all stones to delete
		private DateTime m_StartDecay;
		private Guild m_Guild;

		[CommandProperty( AccessLevel.GameMaster, AccessLevel.Administrator )]
		public DateTime StartDecay
		{
			get { return m_StartDecay; }
			set { m_StartDecay = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Guild Guild
		{
			get
			{
				return m_Guild;
			}
		}

		public override int LabelNumber{ get{ return 1041429; } } // a guildstone

		public Guildstone( Guild g ) : base( 0xED4 )
		{
			m_Guild = g;

			Movable = false;
		}

		public Guildstone( Serial serial ) : base( serial )
		{
		}

		public static void Initialize()
		{
			//if (!Misc.AutoRestart.Enabled)
				EventSink.WorldSave += new WorldSaveEventHandler(EventSink_WorldSave);
		}

		private static void EventSink_WorldSave(WorldSaveEventArgs args)
		{
			if (ms_DeleteCache != null && ms_DeleteCache.Count > 0)
				for (int i = ms_DeleteCache.Count - 1; i >= 0; --i)
					((Guildstone)ms_DeleteCache[i]).Delete();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 2 ); // version

			if ( BaseHouse.FindHouseAt( this ) == null )
			{
				if ( m_StartDecay == DateTime.MinValue )
					m_StartDecay = DateTime.Now;
				else if (DateTime.Now - m_StartDecay > TimeSpan.FromDays(7.0))
				{
					//If the stone was moved out more than 7 days ago it
					//is added to the DeleteCache
					if (ms_DeleteCache == null) ms_DeleteCache = new ArrayList();
					ms_DeleteCache.Add(this);
				}
			}
			else
				m_StartDecay = DateTime.MinValue;


			writer.Write( m_StartDecay );

			writer.Write( m_Guild );



		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 2:
				{
					m_StartDecay = reader.ReadDateTime();
					goto case 1;
				}
				case 1:
				{
					m_Guild = reader.ReadGuild() as Guild;

					goto case 0;
				}
				case 0:
				{
					break;
				}
			}

			if ( m_Guild == null )
				this.Delete();
			if ( m_StartDecay != DateTime.MinValue && DateTime.Now - m_StartDecay > TimeSpan.FromDays( 7.0 ) )
				Delete();
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_Guild != null )
			{
				string name;

				if ( (name = m_Guild.Name) == null || (name = name.Trim()).Length <= 0 )
					name = "(unnamed)";

				list.Add( 1060802, Utility.FixHtml( name ) ); // Guild name: ~1_val~
			}
		}

		public override void OnSingleClick( Mobile from )
		{
			base.OnSingleClick( from );

			string name;

			if ( m_Guild == null )
				name = "(unfounded)";
			else if ( (name = m_Guild.Name) == null || (name = name.Trim()).Length <= 0 )
				name = "(unnamed)";

			this.LabelTo( from, name );
		}

		public override void OnAfterDelete()
		{
			if ( m_Guild != null && !m_Guild.Disbanded )
				m_Guild.Disband();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( m_Guild == null || m_Guild.Disbanded )
			{
				Delete();
			}
			else if ( !from.InRange( GetWorldLocation(), 2 ) )
			{
				from.SendLocalizedMessage( 500446 ); // That is too far away.
			}
			else if ( m_Guild.Accepted.Contains( from ) )
			{
				m_Guild.Accepted.Remove( from );
				m_Guild.AddMember( from );

				GuildGump.EnsureClosed( from );
				from.SendGump( new GuildGump( from, m_Guild ) );
			}
			else if ( from.AccessLevel < AccessLevel.GameMaster && !m_Guild.IsMember( from ) )
			{
				from.Send( new MessageLocalized( Serial, ItemID, MessageType.Regular, 0x3B2, 3, 501158, "", "" ) ); // You are not a member ...
			}
			else
			{
				GuildGump.EnsureClosed( from );
				from.SendGump( new GuildGump( from, m_Guild ) );
			}
		}
	}
}