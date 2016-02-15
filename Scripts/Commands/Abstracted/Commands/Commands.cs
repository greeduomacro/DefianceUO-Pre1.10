using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Reflection;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Spells;
using Server.Mobiles;
using Server.Network;
using Server.Accounting;
using Server.Engines.Harvest;
using Server.Misc;

namespace Server.Scripts.Commands
{
	public class TargetCommands
	{
		public static void Initialize()
		{
			Register( new KillCommand( true ) );
			Register( new KillCommand( false ) );
			Register( new HideCommand( true ) );
			Register( new HideCommand( false ) );
			Register( new KickCommand( true ) );
			Register( new KickCommand( false ) );
			Register( new TeleCommand() );
			Register( new SetCommand() );
			Register( new AliasedSetCommand( AccessLevel.GameMaster, "Immortal", "blessed", "true", ObjectTypes.Mobiles ) );
			Register( new AliasedSetCommand( AccessLevel.GameMaster, "Invul", "blessed", "true", ObjectTypes.Mobiles ) );
			Register( new AliasedSetCommand( AccessLevel.GameMaster, "Mortal", "blessed", "false", ObjectTypes.Mobiles ) );
			Register( new AliasedSetCommand( AccessLevel.GameMaster, "NoInvul", "blessed", "false", ObjectTypes.Mobiles ) );
			Register( new AliasedSetCommand( AccessLevel.GameMaster, "Squelch", "squelched", "true", ObjectTypes.Mobiles ) );
			Register( new AliasedSetCommand( AccessLevel.GameMaster, "Unsquelch", "squelched", "false", ObjectTypes.Mobiles ) );
			Register( new GoBankCommand() );
			Register( new GoGreenAcresCommand() );
			Register( new BounceCommand() );
			Register( new GoCommand() );
			Register( new DisplayCommand() );
			Register( new GetCommand() );
			Register( new GetTypeCommand() );
			Register( new DeleteCommand() );
			Register( new RestockCommand() );
			Register( new DismountCommand() );
			Register( new AddCommand() );
			Register( new AddToPackCommand() );
			Register( new AddToBankCommand() );
			Register( new TellCommand() );
			Register( new PrivSoundCommand() );
			Register( new IncreaseCommand() );
			Register( new OpenBrowserCommand() );
			Register( new CountCommand() );
			Register( new InterfaceCommand() );
			Register( new Factions.FactionKickCommand( Factions.FactionKickType.Kick ) );
			Register( new Factions.FactionKickCommand( Factions.FactionKickType.Ban ) );
			Register( new Factions.FactionKickCommand( Factions.FactionKickType.Unban ) );
		}

		private static ArrayList m_AllCommands = new ArrayList();

		public static ArrayList AllCommands{ get{ return m_AllCommands; } }

		public static void Register( BaseCommand command )
		{
			m_AllCommands.Add( command );

			ArrayList impls = BaseCommandImplementor.Implementors;

			for ( int i = 0; i < impls.Count; ++i )
			{
				BaseCommandImplementor impl = (BaseCommandImplementor)impls[i];

				if ( (command.Supports & impl.SupportRequirement) != 0 )
					impl.Register( command );
			}
		}
	}

	public class CountCommand : BaseCommand
	{
		public CountCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.Complex;
			Commands = new string[]{ "Count" };
			ObjectTypes = ObjectTypes.All;
			Usage = "Count";
			Description = "Counts the number of objects that a command modifier would use. Generally used with condition arguments.";
			ListOptimized = true;
		}

		public override void ExecuteList( CommandEventArgs e, ArrayList list )
		{
			if ( list.Count == 1 )
				AddResponse( "There is one matching object." );
			else
				AddResponse( String.Format( "There are {0} matching objects.", list.Count ) );
		}
	}

	public class OpenBrowserCommand : BaseCommand
	{
		public OpenBrowserCommand()
		{
			AccessLevel = AccessLevel.Counselor;
			Supports = CommandSupport.AllMobiles;
			Commands = new string[]{ "OpenBrowser", "OB" };
			ObjectTypes = ObjectTypes.Mobiles;
			Usage = "OpenBrowser <url>";
			Description = "Opens the web browser of a targeted player to a specified url.";
		}

		public static void OpenBrowser_Callback( Mobile from, bool okay, object state )
		{
			object[] states = (object[])state;
			Mobile gm = (Mobile)states[0];
			string url = (string)states[1];

			if ( okay )
			{
				gm.SendMessage( "{0} : has opened their web browser to : {1}", from.Name, url );
				from.LaunchBrowser( url );
			}
			else
			{
				from.SendMessage( "You have chosen not to open your web browser." );
				gm.SendMessage( "{0} : has chosen not to open their web browser to : {1}", from.Name, url );
			}
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			if ( e.Length == 1 )
			{
				Mobile mob = (Mobile)obj;
				Mobile from = e.Mobile;

				if ( mob.Player )
				{
					NetState ns = mob.NetState;

					if ( ns == null )
					{
						LogFailure( "That player is not online." );
					}
					else
					{
						string url = e.GetString( 0 );

						CommandLogging.WriteLine( from, "{0} {1} requesting to open web browser of {2} to {3}", from.AccessLevel, CommandLogging.Format( from ), CommandLogging.Format( mob ), url );
						AddResponse( "Awaiting user confirmation..." );
						mob.SendGump( new WarningGump( 1060637, 30720, String.Format( "A game master is requesting to open your web browser to the following URL:<br>{0}", url ), 0xFFC000, 320, 240, new WarningGumpCallback( OpenBrowser_Callback ), new object[]{ from, url } ) );
					}
				}
				else
				{
					LogFailure( "That is not a player." );
				}
			}
			else
			{
				LogFailure( "Format: OpenBrowser <url>" );
			}
		}
	}

	public class IncreaseCommand : BaseCommand
	{
		public IncreaseCommand()
		{
			AccessLevel = AccessLevel.Counselor;
			Supports = CommandSupport.All;
			Commands = new string[]{ "Increase", "Inc" };
			ObjectTypes = ObjectTypes.Both;
			Usage = "Increase {<propertyName> <offset> ...}";
			Description = "Increases the value of a specified property by the specified offset.";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			if ( obj is BaseMulti )
				LogFailure( "This command does not work on multis." );
			else if ( e.Length >= 2 )
			{
				string result = Properties.IncreaseValue( e.Mobile, obj, e.Arguments );

				if ( result == "The property has been increased." || result == "The properties have been increased." || result == "The property has been decreased." || result == "The properties have been decreased." || result == "The properties have been changed." )
					AddResponse( result );
				else
					LogFailure( result );
			}
			else
				LogFailure( "Format: Increase {<propertyName> <offset> ...}" );
		}
	}

	public class GoBankCommand : BaseCommand
	{
		public GoBankCommand()
		{
			AccessLevel = AccessLevel.Counselor;
			Supports = CommandSupport.Area | CommandSupport.Online | CommandSupport.Multi | CommandSupport.Self | CommandSupport.Single;
			Commands = new string[]{ "GoBritainBank", "GBB" };
			ObjectTypes = ObjectTypes.Mobiles;
			Usage = "GoBank";
			Description = "Sends the affected mobiles to britain bank";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			Mobile from = e.Mobile;
			if (obj is Mobile)
			{
				Mobile mob = (Mobile)obj;
				if (mob.AccessLevel > from.AccessLevel)
                    LogFailure("Unable to send higher ranked mobiles");
				else if (from.AccessLevel < AccessLevel.GameMaster && mob != from)
                    LogFailure("Unable to send other mobiles as Counselor");
                else
				{
					mob.SetLocation(new Point3D(1438,1690,0), true);
					mob.Map = Map.Felucca;
					AddResponse("Successfully sent to Britain Bank");
				}
			}
		}
	}

	public class GoGreenAcresCommand : BaseCommand
	{
		public GoGreenAcresCommand()
		{
			AccessLevel = AccessLevel.Counselor;
			Supports = CommandSupport.Area | CommandSupport.Online | CommandSupport.Multi | CommandSupport.Self | CommandSupport.Single | CommandSupport.Global;
			Commands = new string[]{ "GoGreenAcres", "GGA" };
			ObjectTypes = ObjectTypes.Mobiles;
			Usage = "GoGreenAcres <1-3>";
			Description = "Sends the affected mobiles to Green Acres";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			Mobile from = e.Mobile;
			if (obj is Mobile)
			{
				Mobile mob = (Mobile)obj;
				if (mob.AccessLevel > from.AccessLevel)
					LogFailure("Unable to send higher ranked mobiles");
                else if (from.AccessLevel < AccessLevel.GameMaster && mob != from)
                    LogFailure("Unable to send other mobiles as Counselor");
                else
				{
					mob.Map = Map.Felucca;
					int gnum;
					if ( e.Arguments.Length < 1 )
						gnum = 1;
					else if ( Convert.ToInt32(e.Arguments[0]) < 1 || Convert.ToInt32(e.Arguments[0]) > 3 )
						gnum = 1;
					else
						gnum = Convert.ToInt32(e.Arguments[0]);
					switch (gnum)
					{
						case 1: mob.SetLocation(new Point3D(5454,1103,0), true); break;
						case 2: mob.SetLocation(new Point3D(5250,392,15), true); break;
						case 3: mob.SetLocation(new Point3D(5932,1945,0), true); break;
					}
					AddResponse(String.Format("Successfully sent to Green Acres {0}", gnum != 1 ? gnum.ToString() : null));
				}
			}
		}
	}

	public class GateCommand : BaseCommand
	{
		public GateCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.Online | CommandSupport.Multi | CommandSupport.Self | CommandSupport.Single;
			Commands = new string[]{ "Gate" };
			ObjectTypes = ObjectTypes.Mobiles;
			Usage = "Gate";
			Description = "A test command";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			Item item = new Moongate();
			item.ItemID = 8152;
			Effects.SendLocationEffect( e.Mobile.Location, e.Mobile.Map, 8139, 11 );
			Timer.DelayCall( TimeSpan.FromSeconds( 0.75 ), new TimerStateCallback( AddGate ), new object[]{ e.Mobile, item } );
		}

		public static void AddGate( object state )
		{
			object[] objects = (object[])state;
			Mobile mobile = objects[0] as Mobile;
			Item item = objects[1] as Item;
			item.MoveToWorld( mobile.Location, mobile.Map );
			Timer.DelayCall( TimeSpan.FromSeconds( 3.0 ), new TimerStateCallback( RemoveGate ), new object[]{ item } );
		}

		public static void RemoveGate( object state )
		{
			Item item = state as Item;
			item.Delete();
		}
	}

	public class BounceCommand : BaseCommand
	{
		public BounceCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.Simple | CommandSupport.Complex;
			Commands = new string[]{ "Bounce", "BNCE" };
			ObjectTypes = ObjectTypes.Items;
			Usage = "Bounce";
			Description = "Sends the affected items to the commander's backpack";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			Mobile from = e.Mobile;
			if (obj is Item)
			{
				Item i = (Item)obj;
				if (i.RootParent is Mobile)
				{
					Mobile m = (Mobile)i.RootParent;
					if (m.AccessLevel > from.AccessLevel)
						LogFailure("Unable to bounce item from higher ranked mobiles");
					else
					{
						from.PlaceInBackpack(i);
						AddResponse("Successfully bounced item(s) to your backpack");
					}
				}
				else
					from.PlaceInBackpack(i);
			}
		}
	}

	public class GoCommand : BaseCommand
	{
		public GoCommand()
		{
			AccessLevel = AccessLevel.Counselor;
			Supports = CommandSupport.Self | CommandSupport.Multi | CommandSupport.Single | CommandSupport.Region | CommandSupport.Online | CommandSupport.Global;
			Commands = new string[]{ "Go" };
			ObjectTypes = ObjectTypes.Mobiles;
			Usage = "Go [name | serial | (x y [z]) | (deg min (N | S) deg min (E | W))]";
			Description = "With no arguments, this command brings up the go menu. With one argument, (name), you are moved to that regions \"go location.\" Or, if a numerical value is specified for one argument, (serial), you are moved to that object. Two or three arguments, (x y [z]), will move your character to that location. When six arguments are specified, (deg min (N | S) deg min (E | W)), your character will go to an approximate of those sextant coordinates.";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			Mobile from = e.Mobile;

			if ( !(obj is Mobile) )
				return;

			Mobile to = obj as Mobile;

			if (from.AccessLevel == AccessLevel.Counselor && from != to)
				return;

			if ( e.Length == 0 )
			{
				GoGump.DisplayTo( from, to );
			}
			else if ( e.Length == 1 )
			{
				try
				{
					int ser = e.GetInt32( 0 );

					IEntity ent = World.FindEntity( ser );

					if ( ent is Item )
					{
						Item item = (Item)ent;

						Map map = item.Map;
						Point3D loc = item.GetWorldLocation();

						Mobile owner = item.RootParent as Mobile;

						if ( owner != null && (owner.Map != null && owner.Map != Map.Internal) && !to.CanSee( owner ) )
						{
							LogFailure( "It can not go to what you can not see." );
							return;
						}
						else if ( owner != null && (owner.Map == null || owner.Map == Map.Internal) && owner.Hidden && owner.AccessLevel >= to.AccessLevel )
						{
							LogFailure( "It can not go to what you can not see." );
							return;
						}
						else if ( !FixMap( ref map, ref loc, item ) )
						{
							LogFailure( "That is an internal item and you cannot go to it." );
							return;
						}

						to.MoveToWorld( loc, map );

						return;
					}
					else if ( ent is Mobile )
					{
						Mobile m = (Mobile)ent;

						Map map = m.Map;
						Point3D loc = m.Location;

						Mobile owner = m;

						if ( owner != null && (owner.Map != null && owner.Map != Map.Internal) && !to.CanSee( owner ) )
						{
							LogFailure( "It can not go to what you can not see." );
							return;
						}
						else if ( owner != null && (owner.Map == null || owner.Map == Map.Internal) && owner.Hidden && owner.AccessLevel >= to.AccessLevel )
						{
							LogFailure( "It can not go to what you can not see." );
							return;
						}
						else if ( !FixMap( ref map, ref loc, m ) )
						{
							LogFailure( "That is an internal mobile and you cannot go to it." );
							return;
						}

						to.MoveToWorld( loc, map );

						return;
					}
					else
					{
						string name = e.GetString( 0 );

						ArrayList list = from.Map.Regions;

						for ( int i = 0; i < list.Count; ++i )
						{
							Region r = (Region)list[i];

							if ( Insensitive.Equals( r.Name, name ) )
							{
								to.Location = new Point3D( r.GoLocation );
								return;
							}
						}

						if ( ser != 0 )
							LogFailure( "No object with that serial was found." );
						else
							LogFailure( "No region with that name was found." );

						return;
					}
				}
				catch
				{
				}

				from.SendMessage( "Region name not found" );
			}
			else if ( e.Length == 2 )
			{
				Map map = from.Map;

				if ( map != null )
				{
					int x = e.GetInt32( 0 ), y = e.GetInt32( 1 );
					int z = map.GetAverageZ( x, y );

					to.Location = new Point3D( x, y, z );
				}
			}
			else if ( e.Length == 3 )
			{
				to.Location = new Point3D( e.GetInt32( 0 ), e.GetInt32( 1 ), e.GetInt32( 2 ) );
			}
			else if ( e.Length == 6 )
			{
				Map map = from.Map;

				if ( map != null )
				{
					Point3D p = Sextant.ReverseLookup( map, e.GetInt32( 3 ), e.GetInt32( 0 ), e.GetInt32( 4 ), e.GetInt32( 1 ), Insensitive.Equals( e.GetString( 5 ), "E" ), Insensitive.Equals( e.GetString( 2 ), "S" ) );

					if ( p != Point3D.Zero )
						to.Location = p;
					else
						LogFailure( "Sextant reverse lookup failed." );
				}
			}
			else
			{
				AddResponse( "Format: Go [name | serial | (x y [z]) | (deg min (N | S) deg min (E | W)]" );
			}
		}

		private static bool FixMap( ref Map map, ref Point3D loc, Item item )
		{
			if ( map == null || map == Map.Internal )
			{
				Mobile m = item.RootParent as Mobile;

				return ( m != null && FixMap( ref map, ref loc, m ) );
			}

			return true;
		}

		private static bool FixMap( ref Map map, ref Point3D loc, Mobile m )
		{
			if ( map == null || map == Map.Internal )
			{
				map = m.LogoutMap;
				loc = m.LogoutLocation;
			}

			return ( map != null && map != Map.Internal );
		}
	}

	public class DisplayCommand : BaseCommand
	{
		public DisplayCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.Self | CommandSupport.Multi | CommandSupport.Single;
			Commands = new string[]{ "Display", "Dsply" };
			ObjectTypes = ObjectTypes.All;
			Usage = "Display [direction] <size>";
			Description = "Creates a display case of certain size and direction at the specified location";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			int max;
			bool west;
			IPoint3D p = obj as IPoint3D;
			if ( p == null )
				return;
			else if ( p is Item )
				p = ((Item)p).GetWorldTop();
			Item item;
			if (e.Arguments.Length == 1 && Convert.ToInt32(e.Arguments[0]) <= 1)
			{
				LogFailure("Invalid size, must be larger than 1");
				return;
			}
			else if (e.Arguments.Length == 1)
			{
				max = Convert.ToInt32(e.Arguments[0]);
				west = false;
			}
			else
			{
				max = Convert.ToInt32(e.Arguments[1]);
				west = e.Arguments[0].ToString().StartsWith( "w" );
			}
			if (max <= 1)
			{
				LogFailure("Invalid size, must be larger than 1");
				return;
			}
			int PointX = west ? p.X : p.X - (max % 2 == 0 ? ((max / 2) - 1) : (max / 2));
			int PointY = west ? p.Y - (max % 2 == 0 ? ((max / 2) - 1) : (max / 2)) : p.Y;
			for (int i=1;i<=max;i++)
			{
				if (i == 1)
				{
					item = new Static( west ? 2824 : 2818 );
					item.MoveToWorld( new Point3D(west ? PointX : (PointX + (i-1)), west ? (PointY + (i-1)) : PointY, p.Z), e.Mobile.Map);
					item = new Static( west ? 2821 : 2815 );
					item.MoveToWorld( new Point3D(west ? PointX : (PointX + (i-1)), west ? (PointY + (i-1)) : PointY, p.Z + 3), e.Mobile.Map);
				}
				else if (i == max)
				{
					item = new Static( west ? 2822 : 2816 );
					item.MoveToWorld( new Point3D(west ? PointX : (PointX + (i-1)), west ? (PointY + (i-1)) : PointY, p.Z), e.Mobile.Map);
					item = new Static( west ? 2819 : 2813 );
					item.MoveToWorld( new Point3D(west ? PointX : (PointX + (i-1)), west ? (PointY + (i-1)) : PointY, p.Z + 3), e.Mobile.Map);
				}
				else
				{
					item = new Static( west ? 2823 : 2817 );
					item.MoveToWorld( new Point3D(west ? PointX : (PointX + (i-1)), west ? (PointY + (i-1)) : PointY, p.Z), e.Mobile.Map);
					item = new Static( west ? 2820 : 2814 );
					item.MoveToWorld( new Point3D(west ? PointX : (PointX + (i-1)), west ? (PointY + (i-1)) : PointY, p.Z + 3), e.Mobile.Map);
				}
			}
			AddResponse("Finished creating display case");
		}
	}

	public class PrivSoundCommand : BaseCommand
	{
		public PrivSoundCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.AllMobiles;
			Commands = new string[]{ "PrivSound" };
			ObjectTypes = ObjectTypes.Mobiles;
			Usage = "PrivSound <index>";
			Description = "Plays a sound to a given target.";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			Mobile from = e.Mobile;

			if ( e.Length == 1 )
			{
				int index = e.GetInt32( 0 );
				Mobile mob = (Mobile)obj;

				CommandLogging.WriteLine( from, "{0} {1} playing sound {2} for {3}", from.AccessLevel, CommandLogging.Format( from ), index, CommandLogging.Format( mob ) );
				mob.Send( new PlaySound( index, mob.Location ) );
			}
			else
			{
				from.SendMessage( "Format: PrivSound <index>" );
			}
		}
	}

	public class TellCommand : BaseCommand
	{
		public TellCommand()
		{
			AccessLevel = AccessLevel.Counselor;
			Supports = CommandSupport.AllMobiles;
			Commands = new string[]{ "Tell" };
			ObjectTypes = ObjectTypes.Mobiles;
			Usage = "Tell \"text\"";
			Description = "Sends a system message to a targeted player.";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			Mobile mob = (Mobile)obj;
			Mobile from = e.Mobile;

			CommandLogging.WriteLine( from, "{0} {1} telling {2} \"{3}\"", from.AccessLevel, CommandLogging.Format( from ), CommandLogging.Format( mob ), e.ArgString );

			mob.SendMessage( e.ArgString );
		}
	}

	public class AddToPackCommand : BaseCommand
	{
		public AddToPackCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.All;
			Commands = new string[]{ "AddToPack", "AddToCont" };
			ObjectTypes = ObjectTypes.Both;
			ListOptimized = true;
			Usage = "AddToPack <name> [params] [set {<propertyName> <value> ...}]";
			Description = "Adds an item by name to the backpack of a targeted player or npc, or a targeted container. Optional constructor parameters. Optional set property list.";
		}

		public override void ExecuteList( CommandEventArgs e, ArrayList list )
		{
			ArrayList packs = new ArrayList( list.Count );

			for ( int i = 0; i < list.Count; ++i )
			{
				object obj = list[i];
				Container cont = null;

				if ( obj is Mobile )
					cont = ((Mobile)obj).Backpack;
				else if ( obj is Container )
					cont = (Container)obj;

				if ( cont != null )
					packs.Add( cont );
				else
					LogFailure( "That is not a container." );
			}

			Add.Invoke( e.Mobile, e.Mobile.Location, e.Mobile.Location, e.Arguments, packs );
		}
	}

	public class AddToBankCommand : BaseCommand
	{
		public AddToBankCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.All;
			Commands = new string[]{ "AddToBank" };
			ObjectTypes = ObjectTypes.Both;
			ListOptimized = true;
			Usage = "AddToBank <name> [params] [set {<propertyName> <value> ....}]";
			Description = "Adds an item by name to the bank of a targeted player or npc, or a targeted container. Optional constructor parameters. Optional set property list.";
		}
		public override void ExecuteList( CommandEventArgs e, ArrayList list )
		{
			ArrayList packs = new ArrayList( list.Count );

			for ( int i = 0; i < list.Count; ++i )
			{
				object obj = list[i];
				Container cont = null;

				if ( obj is Mobile )
				{
					cont = ((Mobile)obj).BankBox;
					packs.Add( cont );
				}
				else
					LogFailure( "That is not a mobile." );
			}

			Add.Invoke( e.Mobile, e.Mobile.Location, e.Mobile.Location, e.Arguments, packs );
		}
	}


	public class AddCommand : BaseCommand
	{
		public AddCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.Simple | CommandSupport.Self;
			Commands = new string[]{ "Add" };
			ObjectTypes = ObjectTypes.All;
			Usage = "Add [<name> [params] [set {<propertyName> <value> ...}]]";
			Description = "Adds an item or npc by name to a targeted location. Optional constructor parameters. Optional set property list. If no arguments are specified, this brings up a categorized add menu.";
		}

		public override bool ValidateArgs( BaseCommandImplementor impl, CommandEventArgs e )
		{
			if ( e.Length >= 1 )
			{
				Type t = ScriptCompiler.FindTypeByName( e.GetString( 0 ) );

				if ( t == null )
				{
					e.Mobile.SendMessage( "No type with that name was found." );

					string match = e.GetString( 0 ).Trim();

					if ( match.Length < 3 )
					{
						e.Mobile.SendMessage( "Invalid search string." );
						e.Mobile.SendGump( new Server.Gumps.AddGump( e.Mobile, match, 0, Type.EmptyTypes, false ) );
					}
					else
					{
						e.Mobile.SendGump( new Server.Gumps.AddGump( e.Mobile, match, 0, (Type[])Server.Gumps.AddGump.Match( match ).ToArray( typeof( Type ) ), true ) );
					}
				}
				else
				{
					return true;
				}
			}
			else
			{
				e.Mobile.SendGump( new Server.Gumps.CategorizedAddGump( e.Mobile ) );
			}

			return false;
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			IPoint3D p = obj as IPoint3D;

			if ( p == null )
				return;

			if ( p is Item )
				p = ((Item)p).GetWorldTop();
			else if ( p is Mobile )
				p = ((Mobile)p).Location;

			Add.Invoke( e.Mobile, new Point3D( p ), new Point3D( p ), e.Arguments );
		}
	}

	public class TeleCommand : BaseCommand
	{
		public TeleCommand()
		{
			AccessLevel = AccessLevel.Counselor;
			Supports = CommandSupport.Simple;
			Commands = new string[]{ "Teleport", "Tele" };
			ObjectTypes = ObjectTypes.All;
			Usage = "Teleport";
			Description = "Teleports your character to a targeted location.";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			IPoint3D p = obj as IPoint3D;

			if ( p == null )
				return;

			Mobile from = e.Mobile;

			SpellHelper.GetSurfaceTop( ref p );

			CommandLogging.WriteLine( from, "{0} {1} teleporting to {2}", from.AccessLevel, CommandLogging.Format( from ), new Point3D( p ) );

			Point3D fromLoc = from.Location;
			Point3D toLoc = new Point3D( p );

			from.Direction = from.GetDirectionTo( toLoc );

			from.Location = toLoc;
			from.ProcessDelta();

			if ( !from.Hidden )
			{
				Effects.SendLocationParticles( EffectItem.Create( fromLoc, from.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
				Effects.SendLocationParticles( EffectItem.Create(   toLoc, from.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 5023 );

				from.PlaySound( 0x1FE );
			}
		}
	}

	public class DismountCommand : BaseCommand
	{
		public DismountCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.AllMobiles;
			Commands = new string[]{ "Dismount" };
			ObjectTypes = ObjectTypes.Mobiles;
			Usage = "Dismount";
			Description = "Forcefully dismounts a given target.";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			Mobile from = e.Mobile;
			Mobile mob = (Mobile)obj;

			CommandLogging.WriteLine( from, "{0} {1} dismounting {2}", from.AccessLevel, CommandLogging.Format( from ), CommandLogging.Format( mob ) );

			bool takenAction = false;

			for ( int i = 0; i < mob.Items.Count; ++i )
			{
				Item item = (Item)mob.Items[i];

				if ( item is IMountItem )
				{
					IMount mount = ((IMountItem)item).Mount;

					if ( mount != null )
					{
						mount.Rider = null;
						takenAction = true;
					}

					if ( mob.Items.IndexOf( item ) == -1 )
						--i;
				}
			}

			for ( int i = 0; i < mob.Items.Count; ++i )
			{
				Item item = (Item)mob.Items[i];

				if ( item.Layer == Layer.Mount )
				{
					takenAction = true;
					item.Delete();
					--i;
				}
			}

			if ( takenAction )
				AddResponse( "They have been dismounted." );
			else
				LogFailure( "They were not mounted." );
		}
	}

	public class RestockCommand : BaseCommand
	{
		public RestockCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.AllNPCs;
			Commands = new string[]{ "Restock" };
			ObjectTypes = ObjectTypes.Mobiles;
			Usage = "Restock";
			Description = "Manually restocks a targeted vendor, refreshing the quantity of every item the vendor sells to the maximum. This also invokes the maximum quantity adjustment algorithms.";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			if ( obj is BaseVendor )
			{
				CommandLogging.WriteLine( e.Mobile, "{0} {1} restocking {2}", e.Mobile.AccessLevel, CommandLogging.Format( e.Mobile ), CommandLogging.Format( obj ) );

				((BaseVendor)obj).Restock();
				AddResponse( "The vendor has been restocked." );
			}
			else
			{
				AddResponse( "That is not a vendor." );
			}
		}
	}

	public class GetTypeCommand : BaseCommand
	{
		public GetTypeCommand()
		{
			AccessLevel = AccessLevel.Counselor;
			Supports = CommandSupport.All;
			Commands = new string[]{ "GetType" };
			ObjectTypes = ObjectTypes.All;
			Usage = "GetType";
			Description = "Gets the type name of a targeted object.";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			if ( obj == null )
			{
				AddResponse( "The object is null." );
			}
			else
			{
				Type type = obj.GetType();

				if ( type.DeclaringType == null )
					AddResponse( String.Format( "The type of that object is {0}.", type.Name ) );
				else
					AddResponse( String.Format( "The type of that object is {0}.", type.FullName ) );
			}
		}
	}

	public class GetCommand : BaseCommand
	{
		public GetCommand()
		{
			AccessLevel = AccessLevel.Counselor;
			Supports = CommandSupport.All;
			Commands = new string[]{ "Get" };
			ObjectTypes = ObjectTypes.All;
			Usage = "Get <propertyName>";
			Description = "Gets a property value by name of a targeted object.";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			if ( e.Length == 1 )
			{
				string result = Properties.GetValue( e.Mobile, obj, e.GetString( 0 ) );

				if ( result == "Property not found." || result == "Property is write only." || result.StartsWith( "Getting this property" ) )
					LogFailure( result );
				else
					AddResponse( result );
			}
			else
			{
				LogFailure( "Format: Get <propertyName>" );
			}
		}
	}

	public class AliasedSetCommand : BaseCommand
	{
		private string m_Name;
		private string m_Value;

		public AliasedSetCommand( AccessLevel level, string command, string name, string value, ObjectTypes objects )
		{
			m_Name = name;
			m_Value = value;

			AccessLevel = level;

			if ( objects == ObjectTypes.Items )
				Supports = CommandSupport.AllItems;
			else if ( objects == ObjectTypes.Mobiles )
				Supports = CommandSupport.AllMobiles;
			else
				Supports = CommandSupport.All;

			Commands = new string[]{ command };
			ObjectTypes = objects;
			Usage = command;
			Description = String.Format( "Sets the {0} property to {1}.", name, value );
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			if ( (obj is Mobile && (e.Mobile.AccessLevel >= ((Mobile)obj).AccessLevel)) || obj is Item || obj is BaseMulti )
			{
				string result = Properties.SetValue( e.Mobile, obj, m_Name, m_Value );

				if ( result == "Property has been set." )
					AddResponse( result );
				else
					LogFailure( result );
			}
		}
	}

	public class SetCommand : BaseCommand
	{
		public SetCommand()
		{
			AccessLevel = AccessLevel.Counselor;
			Supports = CommandSupport.All;
			Commands = new string[]{ "Set" };
			ObjectTypes = ObjectTypes.Both;
			Usage = "Set <propertyName> <value>";
			Description = "Sets a property value by name of a targeted object.";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			if ( (obj is Mobile && (e.Mobile.AccessLevel >= ((Mobile)obj).AccessLevel)) || obj is Item || obj is BaseMulti )
			{
				if ( e.Length >= 2 )
				{
					string result = Properties.SetValue( e.Mobile, obj, e.GetString( 0 ), e.GetString( 1 ) );

					if ( result == "Property has been set." )
						AddResponse( result );
					else
						LogFailure( result );
				}
				else
				{
					LogFailure( "Format: Set <propertyName> <value>" );
				}
			}
		}
	}

	public class DeleteCommand : BaseCommand
	{
		public DeleteCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.AllNPCs | CommandSupport.AllItems;
			Commands = new string[]{ "Delete", "Remove" };
			ObjectTypes = ObjectTypes.Both;
			Usage = "Delete";
			Description = "Deletes a targeted item or mobile. Does not delete players.";
		}

		private void OnConfirmCallback( Mobile from, bool okay, object state )
		{
			object[] states = (object[])state;
			CommandEventArgs e = (CommandEventArgs)states[0];
			ArrayList list = (ArrayList)states[1];

			bool flushToLog = false;

			if ( okay )
			{
				AddResponse( "Delete command confirmed." );

				if ( list.Count > 20 )
					CommandLogging.Enabled = false;

				base.ExecuteList( e, list );

				if ( list.Count > 20 )
				{
					flushToLog = true;
					CommandLogging.Enabled = true;
				}
			}
			else
			{
				AddResponse( "Delete command aborted." );
			}

			Flush( from, flushToLog );
		}

		public override void ExecuteList( CommandEventArgs e, ArrayList list )
		{
			if ( list.Count > 1 )
			{
				e.Mobile.SendGump( new WarningGump( 1060637, 30720, String.Format( "You are about to delete {0} objects. This cannot be undone without a full server revert.<br><br>Continue?", list.Count ), 0xFFC000, 420, 280, new WarningGumpCallback( OnConfirmCallback ), new object[]{ e, list } ) );
				AddResponse( "Awaiting confirmation..." );
			}
			else
			{
				base.ExecuteList( e, list );
			}
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			if ( obj is Item )
			{
				if ( ((Item)obj).RootParent is Mobile && ((Mobile)((Item)obj).RootParent).AccessLevel > e.Mobile.AccessLevel)
					LogFailure( "That cannot be deleted off of a higher rank mobile." );
				else
				{
					CommandLogging.WriteLine( e.Mobile, "{0} {1} deleting {2}", e.Mobile.AccessLevel, CommandLogging.Format( e.Mobile ), CommandLogging.Format( obj ) );
					((Item)obj).Delete();
					AddResponse( "The item has been deleted." );
				}
			}
			else if ( obj is Mobile && !((Mobile)obj).Player )
			{
				CommandLogging.WriteLine( e.Mobile, "{0} {1} deleting {2}", e.Mobile.AccessLevel, CommandLogging.Format( e.Mobile ), CommandLogging.Format( obj ) );
				((Mobile)obj).Delete();
				AddResponse( "The mobile has been deleted." );
			}
			else
			{
				LogFailure( "That cannot be deleted." );
			}
		}
	}

	public class KillCommand : BaseCommand
	{
		private bool m_Value;

		public KillCommand( bool value )
		{
			m_Value = value;

			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.AllMobiles;
			Commands = value ? new string[]{ "Kill" } : new string[]{ "Resurrect", "Res" };
			ObjectTypes = ObjectTypes.Mobiles;

			if ( value )
			{
				Usage = "Kill";
				Description = "Kills a targeted player or npc.";
			}
			else
			{
				Usage = "Resurrect";
				Description = "Resurrects a targeted ghost.";
			}
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			Mobile mob = (Mobile)obj;
			Mobile from = e.Mobile;

			if ( m_Value )
			{
				if ( !mob.Alive )
				{
					LogFailure( "They are already dead." );
				}
				else if ( !mob.CanBeDamaged() || mob.AccessLevel > from.AccessLevel )
				{
					LogFailure( "They cannot be harmed." );
				}
				else
				{
					CommandLogging.WriteLine( from, "{0} {1} killing {2}", from.AccessLevel, CommandLogging.Format( from ), CommandLogging.Format( mob ) );
					mob.Kill();

					AddResponse( "They have been killed." );
				}
			}
			else
			{
				if ( mob.IsDeadBondedPet )
				{
					BaseCreature bc = mob as BaseCreature;

					if ( bc != null )
					{
						CommandLogging.WriteLine( from, "{0} {1} resurrecting {2}", from.AccessLevel, CommandLogging.Format( from ), CommandLogging.Format( mob ) );

						bc.PlaySound( 0x214 );
						bc.FixedEffect( 0x376A, 10, 16 );

						bc.ResurrectPet();

						AddResponse( "It has been resurrected." );
					}
				}
				else if ( !mob.Alive )
				{
					CommandLogging.WriteLine( from, "{0} {1} resurrecting {2}", from.AccessLevel, CommandLogging.Format( from ), CommandLogging.Format( mob ) );

					//mob.PlaySound( 0x214 );
					//mob.FixedEffect( 0x376A, 10, 16 );

					mob.Resurrect();

					AddResponse( "They have been resurrected." );
				}
				else
				{
					LogFailure( "They are not dead." );
				}
			}
		}
	}

	public class HideCommand : BaseCommand
	{
		private bool m_Value;

		public HideCommand( bool value )
		{
			m_Value = value;

			AccessLevel = AccessLevel.Counselor;
			Supports = CommandSupport.AllMobiles;
			Commands = new string[]{ value ? "Hide" : "Unhide" };
			ObjectTypes = ObjectTypes.Mobiles;

			if ( value )
			{
				Usage = "Hide";
				Description = "Makes a targeted mobile disappear in a puff of smoke.";
			}
			else
			{
				Usage = "Unhide";
				Description = "Makes a targeted mobile appear in a puff of smoke.";
			}
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			Mobile m = (Mobile)obj;
			if (e.Mobile.AccessLevel >= m.AccessLevel)
			{
				CommandLogging.WriteLine( e.Mobile, "{0} {1} {2} {3}", e.Mobile.AccessLevel, CommandLogging.Format( e.Mobile ), m_Value ? "hiding" : "unhiding", CommandLogging.Format( m ) );

				Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y, m.Z + 4 ), m.Map, 0x3728, 13 );
				Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y, m.Z ), m.Map, 0x3728, 13 );
				Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y, m.Z - 4 ), m.Map, 0x3728, 13 );
				Effects.SendLocationEffect( new Point3D( m.X, m.Y + 1, m.Z + 4 ), m.Map, 0x3728, 13 );
				Effects.SendLocationEffect( new Point3D( m.X, m.Y + 1, m.Z ), m.Map, 0x3728, 13 );
				Effects.SendLocationEffect( new Point3D( m.X, m.Y + 1, m.Z - 4 ), m.Map, 0x3728, 13 );

				Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y + 1, m.Z + 11 ), m.Map, 0x3728, 13 );
				Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y + 1, m.Z + 7 ), m.Map, 0x3728, 13 );
				Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y + 1, m.Z + 3 ), m.Map, 0x3728, 13 );
				Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y + 1, m.Z - 1 ), m.Map, 0x3728, 13 );

				m.PlaySound( 0x228 );
				m.Hidden = m_Value;

				if ( m_Value )
					AddResponse( "They have been hidden." );
				else
					AddResponse( "They have been revealed." );
			}
		}
	}

	public class KickCommand : BaseCommand
	{
		private bool m_Ban;

		public KickCommand( bool ban )
		{
			m_Ban = ban;

			AccessLevel = ( ban ? AccessLevel.Administrator : AccessLevel.GameMaster );
			Supports = CommandSupport.AllMobiles;
			Commands = new string[]{ ban ? "Ban" : "Kick" };
			ObjectTypes = ObjectTypes.Mobiles;

			if ( ban )
			{
				Usage = "Ban";
				Description = "Bans the account of a targeted player.";
			}
			else
			{
				Usage = "Kick";
				Description = "Disconnects a targeted player.";
			}
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			Mobile from = e.Mobile;
			Mobile targ = (Mobile)obj;

			if ( from.AccessLevel > targ.AccessLevel )
			{
				NetState fromState = from.NetState, targState = targ.NetState;

				if ( fromState != null && targState != null )
				{
					Account fromAccount = fromState.Account as Account;
					Account targAccount = targState.Account as Account;

					if ( fromAccount != null && targAccount != null )
					{
						CommandLogging.WriteLine( from, "{0} {1} {2} {3}", from.AccessLevel, CommandLogging.Format( from ), m_Ban ? "banning" : "kicking", CommandLogging.Format( targ ) );

						targ.Say( "I've been {0}!", m_Ban ? "banned" : "kicked" );

						AddResponse( String.Format( "They have been {0}.", m_Ban ? "banned" : "kicked" ) );

						targState.Dispose();

						if ( m_Ban )
						{
							targAccount.Banned = true;
							targAccount.SetUnspecifiedBan( from );
							from.SendGump( new BanDurationGump( targAccount ) );
						}
					}
				}
				else if ( targState == null )
				{
					LogFailure( "They are not online." );
				}
			}
			else
			{
				LogFailure( "You do not have the required access level to do this." );
			}
		}
	}
}