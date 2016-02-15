using System;
using Server;
using System.Collections;
using Server.Scripts.Commands;
using System.Text;
using System.Reflection;
using Server.Gumps;
using Server.Network;

//NOTE TO SELF, replace the < & >  with ( and  )

namespace Server.Scripts.Commands
{
	public class HelpInfo
	{
		private static Hashtable m_HelpInfos = new Hashtable();
		private static ArrayList m_SortedHelpInfo = new ArrayList();

		public static Hashtable HelpInfos{ get{ return m_HelpInfos; } }
		public static ArrayList SortedHelpInfos{ get{ return m_SortedHelpInfo; } }

		[CallPriority( 100 )]
		public static void Initialize()
		{
			Server.Commands.Register( "HelpInfo", AccessLevel.Player, new CommandEventHandler( HelpInfo_OnCommand ) );

			FillHashTable();
		}

		[Usage( "HelpInfo [<command>]" )]
		[Description( "Gives information on a specified command, or when no argument specified, displays a gump containing all commands" )]
		private static void HelpInfo_OnCommand( CommandEventArgs e )
		{
			if( e.Length > 0 )
			{
				string arg = e.GetString( 0 ).ToLower();

				if( m_HelpInfos[arg] != null )
				{
					CommandHelpInfo c = (CommandHelpInfo)m_HelpInfos[arg];
					Mobile m = e.Mobile;

					if( m.AccessLevel >= c.AccessLevel )
						m.SendGump( new CommandInfo( c ) );
					else
						m.SendMessage( "You cannot access that command." );

					return;
				}
				else
					e.Mobile.SendMessage( String.Format( "Command '{0}' not found!", arg ) );
			}

			e.Mobile.SendGump( new CommandListGump( 0, e.Mobile, null ) );

		}

		public HelpInfo()
		{
		}

		public static void FillHashTable()
		{
			ArrayList commands = new ArrayList( Server.Commands.Entries.Values );
			ArrayList list = new ArrayList();

			commands.Sort();
			commands.Reverse();
			Clean( commands );

			for ( int i = 0; i < commands.Count; ++i )
			{
				CommandEntry e = (CommandEntry)commands[i];

				MethodInfo mi = e.Handler.Method;

				object[] attrs = mi.GetCustomAttributes( typeof( UsageAttribute ), false );

				if ( attrs.Length == 0 )
					continue;

				UsageAttribute usage = attrs[0] as UsageAttribute;

				attrs = mi.GetCustomAttributes( typeof( DescriptionAttribute ), false );

				if ( attrs.Length == 0 )
					continue;

				DescriptionAttribute desc = attrs[0] as DescriptionAttribute;

				if ( usage == null || desc == null )
					continue;

				attrs = mi.GetCustomAttributes( typeof( AliasesAttribute ), false );

				AliasesAttribute aliases = ( attrs.Length == 0 ? null : attrs[0] as AliasesAttribute );

				string descString = desc.Description.Replace( "<", "(" ).Replace( ">", ")" );

				if ( aliases == null )
					list.Add( new CommandHelpInfo( e.AccessLevel, e.Command, null, usage.Usage, descString ) );
				else
				{
					list.Add( new CommandHelpInfo( e.AccessLevel, e.Command, aliases.Aliases, usage.Usage, descString ) );

					for( int j = 0; j < aliases.Aliases.Length; j++ )
					{
						string[] newAliases = new string[aliases.Aliases.Length];

						aliases.Aliases.CopyTo( newAliases, 0 );

						newAliases[j] = e.Command;

						list.Add( new CommandHelpInfo( e.AccessLevel, aliases.Aliases[j], newAliases, usage.Usage, descString ) );
					}
				}
			}

			commands = TargetCommands.AllCommands;

			for ( int i = 0; i < commands.Count; ++i )
			{
				BaseCommand command = (BaseCommand)commands[i];

				string usage = command.Usage;
				string desc = command.Description;

				if ( usage == null || desc == null )
					continue;

				string[] cmds = command.Commands;
				string cmd = cmds[0];
				string[] aliases = new string[cmds.Length - 1];

				for ( int j = 0; j < aliases.Length; ++j )
					aliases[j] = cmds[j + 1];

				desc = desc.Replace( "<", "(" ).Replace( ">", ")" );

				if ( command.Supports != CommandSupport.Single )
				{
					StringBuilder sb = new StringBuilder( 50 + desc.Length );

					sb.Append( "Modifiers: " );

					if ( (command.Supports & CommandSupport.Global) != 0 )
						sb.Append( "<i><Global</i>, " );

					if ( (command.Supports & CommandSupport.Online) != 0 )
						sb.Append( "<i>Online</i>, " );

					if ( (command.Supports & CommandSupport.Region) != 0 )
						sb.Append( "<i>Region</i>, " );

					if ( (command.Supports & CommandSupport.Contained) != 0 )
						sb.Append( "<i>Contained</i>, " );

					if ( (command.Supports & CommandSupport.Multi) != 0 )
						sb.Append( "<i>Multi</i>, " );

					if ( (command.Supports & CommandSupport.Area) != 0 )
						sb.Append( "<i>Area</i>, " );

					if ( (command.Supports & CommandSupport.Self) != 0 )
						sb.Append( "<i>Self</i>, " );

					sb.Remove( sb.Length - 2, 2 );
					sb.Append( "<br>" );
					sb.Append( desc );

					desc = sb.ToString();
				}

				list.Add( new CommandHelpInfo( command.AccessLevel, cmd, aliases, usage, desc ) );

				for( int j = 0; j < aliases.Length; j++ )
				{
					string[] newAliases = new string[aliases.Length];

					aliases.CopyTo( newAliases, 0 );

					newAliases[j] = cmd;

					list.Add( new CommandHelpInfo( command.AccessLevel, aliases[j], newAliases, usage, desc ) );
				}
			}

			commands = BaseCommandImplementor.Implementors;

			for ( int i = 0; i < commands.Count; ++i )
			{
				BaseCommandImplementor command = (BaseCommandImplementor)commands[i];

				string usage = command.Usage;
				string desc = command.Description;

				if ( usage == null || desc == null )
					continue;

				string[] cmds = command.Accessors;
				string cmd = cmds[0];
				string[] aliases = new string[cmds.Length - 1];

				for ( int j = 0; j < aliases.Length; ++j )
					aliases[j] = cmds[j + 1];

				desc = desc.Replace( "<", ")" ).Replace( ">", ")" );

				list.Add( new CommandHelpInfo( command.AccessLevel, cmd, aliases, usage, desc ) );

				for( int j = 0; j < aliases.Length; j++ )
				{
					string[] newAliases = new string[aliases.Length];

					aliases.CopyTo( newAliases, 0 );

					newAliases[j] = cmd;

					list.Add( new CommandHelpInfo( command.AccessLevel, aliases[j], newAliases, usage, desc ) );
				}
			}

			list.Sort( new CommandEntrySorter() );

			m_SortedHelpInfo = list;

			foreach( CommandHelpInfo c in m_SortedHelpInfo )
			{
				if( !m_HelpInfos.ContainsKey( c.Name.ToLower() ) )
					m_HelpInfos.Add( c.Name.ToLower(), c );
			}
		}


		/// <summary>
		/// Same as Doc.docCommandEntry, but doccommandentry is private :(
		/// </summary>
		public class CommandHelpInfo
		{
			private AccessLevel m_AccessLevel;
			private string m_Name;
			private string[] m_Aliases;
			private string m_Usage;
			private string m_Description;

			public AccessLevel AccessLevel{ get{ return m_AccessLevel; } }
			public string Name{ get{ return m_Name; } }
			public string[] Aliases{ get{ return m_Aliases; } }
			public string Usage{ get{ return m_Usage; } }
			public string Description{ get{ return m_Description; } }

			public CommandHelpInfo( AccessLevel accessLevel, string name, string[] aliases, string usage, string description )
			{
				m_AccessLevel = accessLevel;
				m_Name = name;
				m_Aliases = aliases;
				m_Usage = usage;
				m_Description = description;
			}

			public override string ToString()
			{
				return String.Format( "{0}: Usage -> {1} accesslevel -> {2}", m_Name, Usage , AccessLevel);
				//The above is jstu for easy testing of stuffs
			}
		}

		/// <summary>
		/// Like the one in docs but docs on is private.. etc
		/// </summary>
		private class CommandEntrySorter : IComparer
		{
			public int Compare( object x, object y )
			{
				CommandHelpInfo a = (CommandHelpInfo)x;
				CommandHelpInfo b = (CommandHelpInfo)y;

				int v = b.AccessLevel.CompareTo( a.AccessLevel );

				if ( v == 0 )
					v = a.Name.CompareTo( b.Name );

				return v;
			}
		}

		/// <summary>
		/// Copy of the one in Docs, but the one in Docs is private
		/// </summary>
		/// <param name="list"></param>
		public static void Clean( ArrayList list )
		{
			for ( int i = 0; i < list.Count; ++i )
			{
				CommandEntry e = (CommandEntry)list[i];

				for ( int j = i + 1; j < list.Count; ++j )
				{
					CommandEntry c = (CommandEntry)list[j];

					if ( e.Handler.Method == c.Handler.Method )
					{
						list.RemoveAt( j );
						--j;
					}
				}
			}
		}

	}
}

namespace Server.Gumps
{
	public class CommandListGump : BaseGridGump
	{
		private const int EntriesPerPage = 15;

		int m_Page;
		ArrayList m_List;

		public CommandListGump( int page, Mobile from, ArrayList list ) : base( 30, 30 )
		{
			m_Page = page;

			if( list == null )
			{
				m_List = new ArrayList();

				foreach( HelpInfo.CommandHelpInfo c in HelpInfo.SortedHelpInfos )
					if( from.AccessLevel >= c.AccessLevel )
						m_List.Add( c );
			}
			else
				m_List = list;


			AddNewPage();

			if ( m_Page > 0 )
				AddEntryButton( 20, ArrowLeftID1, ArrowLeftID2, 1, ArrowLeftWidth, ArrowLeftHeight );
			else
				AddEntryHeader( 20 );

			AddEntryHtml( 160, Center( String.Format( "Page {0} of {1}", m_Page+1, (m_List.Count + EntriesPerPage - 1) / EntriesPerPage ) ) );

			if ( (m_Page + 1) * EntriesPerPage < m_List.Count )
				AddEntryButton( 20, ArrowRightID1, ArrowRightID2, 2, ArrowRightWidth, ArrowRightHeight );
			else
				AddEntryHeader( 20 );

			int last = (int)AccessLevel.Player - 1;

			for ( int i = m_Page * EntriesPerPage, line = 0; line < EntriesPerPage && i < m_List.Count; ++i, ++line )
			{
				HelpInfo.CommandHelpInfo c = (HelpInfo.CommandHelpInfo)m_List[i];
				if( from.AccessLevel >= c.AccessLevel )
				{
					if ( (int)c.AccessLevel != last )
					{
						AddNewLine();

						AddEntryHtml( 20 + OffsetSize + 160, Color( c.AccessLevel.ToString(), 0xFF0000 ) );
						AddEntryHeader( 20 );
						line++;
					}

					last = (int)c.AccessLevel;

					AddNewLine();

					AddEntryHtml( 20 + OffsetSize + 160, c.Name );

					AddEntryButton( 20, ArrowRightID1, ArrowRightID2, 3 + i, ArrowRightWidth, ArrowRightHeight );
				}
			}

			FinishPage();
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile m = sender.Mobile;
			switch ( info.ButtonID )
			{
				case 0:
				{
					m.CloseGump( typeof( CommandInfo ) );
					break;
				}
				case 1:
				{
					if ( m_Page > 0 )
						m.SendGump( new CommandListGump( m_Page - 1, m, m_List ) );

					break;
				}
				case 2:
				{
					if ( (m_Page + 1) * EntriesPerPage < HelpInfo.SortedHelpInfos.Count )
						m.SendGump( new CommandListGump( m_Page + 1, m, m_List ) );

					break;
				}
				default:
				{

					int v = info.ButtonID - 3;

					if ( v >= 0 && v < m_List.Count )
					{
						HelpInfo.CommandHelpInfo c = (HelpInfo.CommandHelpInfo)m_List[v];

						if( m.AccessLevel >= c.AccessLevel )
						{
							m.SendGump( new CommandInfo( c ) );
							m.SendGump( new CommandListGump( m_Page, m, m_List ) );
						}
						else
						{
							m.SendMessage( "You can no longer access that command." );
							m.SendGump( new CommandListGump( m_Page, m, null ) );
						}
					}
					break;
				}
			}
		}
	}


	public class CommandInfo : Gump
	{

		public string Color( string text, int color )
		{
			return String.Format( "<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", color, text );
		}

		public string Center( string text )
		{
			return String.Format( "<CENTER>{0}</CENTER>", text );
		}

		public CommandInfo( HelpInfo.CommandHelpInfo info ) : this( info, 320, 200 )
		{
		}

		public CommandInfo( HelpInfo.CommandHelpInfo info, int width, int height ) : base( 300, 50 )
		{
			AddPage( 0 );

			AddBackground( 0, 0, width, height, 5054 );

			//AddImageTiled( 10, 10, width - 20, 20, 2624 );
			//AddAlphaRegion( 10, 10, width - 20, 20 );
			//AddHtmlLocalized( 10, 10, width - 20, 20, header, headerColor, false, false );
			AddHtml( 10,10,width - 20, 20, Color( Center( info.Name ), 0xFF0000 ), false, false );

			//AddImageTiled( 10, 40, width - 20, height - 80, 2624 );
			//AddAlphaRegion( 10, 40, width - 20, height - 80 );

			StringBuilder sb = new StringBuilder();

			sb.Append( "Usage: " );
			sb.Append( info.Usage.Replace( "<", "(" ).Replace( ">", ")" ) );
			sb.Append( "<BR>" );

			string[] aliases = info.Aliases;

			if ( aliases != null && aliases.Length != 0 )
			{
				sb.Append( String.Format( "Alias{0}: ", aliases.Length == 1 ? "" : "es" ) );

				for ( int i = 0; i < aliases.Length; ++i )
				{
					if ( i != 0 )
						sb.Append( ", " );

					sb.Append( aliases[i] );
				}

				sb.Append( "<BR>" );
			}

			sb.Append( "AccessLevel: " );
			sb.Append( info.AccessLevel.ToString() );
			sb.Append( "<BR>" );
			sb.Append( "<BR>" );

			sb.Append( info.Description );

			AddHtml( 10, 40, width - 20, height - 80, sb.ToString(), false, true );

			//AddImageTiled( 10, height - 30, width - 20, 20, 2624 );
			//AddAlphaRegion( 10, height - 30, width - 20, 20 );

		}
	}
}