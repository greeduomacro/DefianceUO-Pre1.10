using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.FSPvpPointSystem;

namespace Server.Scripts.Commands
{
	public class PvpStatsCommand
	{
		public static void Initialize()
		{
			TargetCommands.Register( new SetPvPCommand() );
			TargetCommands.Register( new IncreasePvPCommand() );
		}
	}

	public class SetPvPCommand : BaseCommand
	{
		public SetPvPCommand()
		{
			AccessLevel = AccessLevel.Seer;
			Supports = CommandSupport.All;
			Commands = new string[]{ "SetPvP" };
			ObjectTypes = ObjectTypes.Mobiles;
			Usage = "SetPvP <property> <amount>";
			Description = "Sets a pvp property on the effected players.";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			if ( e.Length < 2 )
				LogFailure( "Format: SetPvP <propertyName> <value>" );
			else if ( obj is PlayerMobile )
			{
				PlayerMobile from = (PlayerMobile)obj;
				FSPvpPointSystem.FSPvpSystem.PvpStats ps = FSPvpSystem.GetPvpStats( from );

				for ( int i = 0; ( i+1 ) < e.Length; i += 2 )
				{
					string result = Properties.SetValue( e.Mobile, ps, e.GetString( i ), e.GetString( i+1 ) );

					if ( result == "Property has been set." )
						AddResponse( result );
					else
						LogFailure( result );
				}
			}
		}
	}

	public class IncreasePvPCommand : BaseCommand
	{
		public IncreasePvPCommand()
		{
			AccessLevel = AccessLevel.Seer;
			Supports = CommandSupport.All;
			Commands = new string[]{ "IncreasePvP", "IncPvP" };
			ObjectTypes = ObjectTypes.Mobiles;
			Usage = "IncreasePvP {<propertyName> <offset> ...}";
			Description = "Increases the value of a pvp property by the specified offset.";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			if ( e.Length < 2 )
				LogFailure( "Format: IncreasePvP {<propertyName> <offset> ...}" );
			else if ( obj is PlayerMobile )
			{
				PlayerMobile from = (PlayerMobile)obj;
				FSPvpPointSystem.FSPvpSystem.PvpStats ps = FSPvpSystem.GetPvpStats( from );

				string result = Properties.IncreaseValue( e.Mobile, ps, e.Arguments );

				if ( result == "The property has been increased." || result == "The properties have been increased." || result == "The property has been decreased." || result == "The properties have been decreased." || result == "The properties have been changed." )
					AddResponse( result );
				else
					LogFailure( result );
			}
		}
	}
}