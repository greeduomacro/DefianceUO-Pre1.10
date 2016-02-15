using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Guilds;
using Server.Mobiles;
// jakob, added this
using Server.Network;
// end
using Server.Prompts;
using Server.Targeting;
using Server.Accounting;
using Server.Scripts.Commands;
using Server.Regions;

namespace Server.Factions
{
	[CustomEnum( new string[]{ "Minax", "Council of Mages", "True Britannians", "Shadowlords" } )]
	public abstract class Faction : IComparable
	{
		private FactionDefinition m_Definition;
		private FactionState m_State;
		private StrongholdRegion m_StrongholdRegion;

		public StrongholdRegion StrongholdRegion
		{
			get{ return m_StrongholdRegion; }
			set{ m_StrongholdRegion = value; }
		}

		public FactionDefinition Definition
		{
			get{ return m_Definition; }
			set
			{
				m_Definition = value;
				Region.AddRegion( m_StrongholdRegion = new StrongholdRegion( this ) );
			}
		}

		public FactionState State
		{
			get{ return m_State; }
			set{ m_State = value; }
		}

		public Election Election
		{
			get{ return m_State.Election; }
			set{ m_State.Election = value; }
		}

		public Mobile Commander
		{
			get{ return m_State.Commander; }
			set{ m_State.Commander = value; }
		}

		// jakob, added this
		public Mobile DeputyCommander
		{
			get{ return m_State.DeputyCommander; }
			set{ m_State.DeputyCommander = value; }
		}
		// end

		public int Tithe
		{
			get{ return m_State.Tithe; }
			set{ m_State.Tithe = value; }
		}

		public int Silver
		{
			get{ return m_State.Silver; }
			set{ m_State.Silver = value; }
		}

		public PlayerStateCollection Members
		{
			get{ return m_State.Members; }
			set{ m_State.Members = value; }
		}

		// jakob, added this to get a quick count of how many towns a faction own
		public int OwnedTowns
		{
			get
			{
				int ownedTowns = 0;
				foreach ( Town town in Town.Towns )
					if ( town.Owner == this )
						ownedTowns++;
				return ownedTowns;
			}
		}
		// end

		public static readonly TimeSpan LeavePeriod = TimeSpan.FromDays( 7.0 );

		public bool FactionMessageReady
		{
			get{ return m_State.FactionMessageReady; }
		}

		public void Broadcast( string text )
		{
			Broadcast( 0x3B2, text );
		}

		public void Broadcast( int hue, string text )
		{
			PlayerStateCollection members = Members;

			for ( int i = 0; i < members.Count; ++i )
				members[i].Mobile.SendMessage( hue, text );
		}

		public void Broadcast( int number )
		{
			PlayerStateCollection members = Members;

			for ( int i = 0; i < members.Count; ++i )
				members[i].Mobile.SendLocalizedMessage( number );
		}

		public void Broadcast( string format, params object[] args )
		{
			Broadcast( String.Format( format, args ) );
		}

		public void Broadcast( int hue, string format, params object[] args )
		{
			Broadcast( hue, String.Format( format, args ) );
		}

		public void BeginBroadcast( Mobile from )
		{
			from.SendLocalizedMessage( 1010265 ); // Enter Faction Message
			from.Prompt = new BroadcastPrompt( this );
		}

                public void EndBroadcast( Mobile from, string text )
		{
			if ( from.AccessLevel == AccessLevel.Player )
				m_State.RegisterBroadcast();

		// jakob, modified this to broadcast to all faction members, and other characters on the account too
		//Broadcast( Definition.HueBroadcast, "{0} [Commander] {1} : {2}", from.Name, Definition.FriendlyName, text );

		string broadcastString = String.Format( "{0} [Commander] {1} : {2}", from.Name, Definition.FriendlyName, text );

			// loop through all online states
			foreach (NetState netState in NetState.Instances)
			{
				Account acc = (Account)netState.Account;
				if( acc != null ) // Added by E
				{
					// check if the account has a player that is a member of this faction
					for (int i = 0; i < acc.Length; i++)
					{
						PlayerState ps = PlayerState.Find( acc[i] );
						if ( ps != null && ps.Faction == this)
						{
							// if so, send broadcast message to the online mobile
							if( netState.Mobile != null ) // Added by E
							netState.Mobile.SendMessage( Definition.HueBroadcast, broadcastString );
							break;
						}
					}
				}
			}
			// end
		}

		private class BroadcastPrompt : Prompt
		{
			private Faction m_Faction;

			public BroadcastPrompt( Faction faction )
			{
				m_Faction = faction;
			}

			public override void OnResponse( Mobile from, string text )
			{
				m_Faction.EndBroadcast( from, text );
			}
		}

		// jakob, added these for the ability to appoint a deputy commander
		public void BeginAppointDeputyCommander( Mobile from )
		{
			if ( IsCommander( from ) && !IsDeputyCommander( from ) && OwnedTowns >= 1 )
			{
				from.SendMessage( "Select the player whom you wish to be the deputy commander." );
				from.BeginTarget( 12, false, TargetFlags.None, new TargetCallback( AppointDeputyCommander_OnTarget ) );
			}
		}

		public void AppointDeputyCommander_OnTarget( Mobile from, object obj )
		{
			if ( obj is Mobile )
			{
				Mobile recv = (Mobile) obj;

				PlayerState giveState = PlayerState.Find( from );
				PlayerState recvState = PlayerState.Find( recv );

				if ( giveState == null || !IsCommander( from ) )
					return;

				if ( recvState == null || recvState.Faction != giveState.Faction )
				{
					from.SendMessage( "You can only appoint faction mates." );
				}
				else if ( recvState.Sheriff != null || recvState.Finance != null || IsCommander( recv ) )
				{
					from.SendMessage( "That player seems to have important tasks already." );
				}
				else
				{
					from.SendMessage( "They are now the deputy commander." );
					DeputyCommander = recv;
				}
			}
			else
			{
				from.SendMessage( "You may only appoint another player." );
			}
		}
		// end

		public void BeginHonorLeadership( Mobile from )
		{
			from.SendLocalizedMessage( 502090 ); // Click on the player whom you wish to honor.
			from.BeginTarget( 12, false, TargetFlags.None, new TargetCallback( HonorLeadership_OnTarget ) );
		}

		public void HonorLeadership_OnTarget( Mobile from, object obj )
		{
			if ( obj is Mobile )
			{
				Mobile recv = (Mobile) obj;

				PlayerState giveState = PlayerState.Find( from );
				PlayerState recvState = PlayerState.Find( recv );

				if ( giveState == null )
					return;

				if ( recvState == null || recvState.Faction != giveState.Faction )
				{
					from.SendLocalizedMessage( 1042497 ); // Only faction mates can be honored this way.
				}
				else if ( giveState.KillPoints < 5 ) // TODO: Verify 5 or 10
				{
					from.SendLocalizedMessage( 1042499 ); // You must have at least five kill points to honor them.
				}
				else
				{
					giveState.KillPoints -= 5;
					recvState.KillPoints += 4;

					// TODO: Confirm no message sent to giver
					recv.SendLocalizedMessage( 1042500 ); // You have been honored with four kill points.
				}
			}
			else
			{
				from.SendLocalizedMessage( 1042496 ); // You may only honor another player.
			}
		}

		public void AddMember( Mobile mob )
		{
			Members.Add( new PlayerState( mob, this, Members ) );

			mob.AddToBackpack( FactionItem.Imbue( new Robe(), this, false, Definition.HuePrimary ) );
                        mob.AddToBackpack( new FactionParchament() );
			mob.SendLocalizedMessage( 1010374 ); // You have been granted a robe which signifies your faction

			mob.InvalidateProperties();
			mob.Delta( MobileDelta.Noto );
		}

		public static bool IsNearType( Mobile mob, Type type, int range )
		{
			bool mobs = type.IsSubclassOf( typeof( Mobile ) );
			bool items = type.IsSubclassOf( typeof( Item ) );

			IPooledEnumerable eable;

			if ( mobs )
				eable = mob.GetMobilesInRange( range );
			else if ( items )
				eable = mob.GetItemsInRange( range );
			else
				return false;

			foreach ( object obj in eable )
			{
				if ( type.IsAssignableFrom( obj.GetType() ) )
				{
					eable.Free();
					return true;
				}
			}

			eable.Free();
			return false;
		}

		public void RemovePlayerState( PlayerState pl )
		{
			if ( pl == null || !Members.Contains( pl ) )
				return;

			Members.Remove( pl );

			PlayerMobile pm = (PlayerMobile) pl.Mobile;
			if( pm == null )
				return;

			Mobile mob = pl.Mobile;
			if ( pm.FactionPlayerState == pl )
			{
				pm.FactionPlayerState = null;

				mob.InvalidateProperties();
				mob.Delta( MobileDelta.Noto );

				if ( Election.IsCandidate( mob ) )
					Election.RemoveCandidate( mob );

				if ( pl.Finance != null )
					pl.Finance.Finance = null;

				if ( pl.Sheriff != null )
					pl.Sheriff.Sheriff = null;

				Election.RemoveVoter( mob );

				if ( Commander == mob )
					Commander = null;

				if ( DeputyCommander == mob )
					DeputyCommander = null;

				pm.ValidateEquipment();
			}
		}

		public void RemoveMember( Mobile mob )
		{
			PlayerState pl = PlayerState.Find( mob );

			if ( pl == null || !Members.Contains( pl ) )
				return;

			Members.Remove( pl );

			if ( mob is PlayerMobile )
				((PlayerMobile)mob).FactionPlayerState = null;

			mob.InvalidateProperties();
			mob.Delta( MobileDelta.Noto );

			if ( Election.IsCandidate( mob ) )
				Election.RemoveCandidate( mob );

			Election.RemoveVoter( mob );

			if ( pl.Finance != null )
				pl.Finance.Finance = null;

			if ( pl.Sheriff != null )
				pl.Sheriff.Sheriff = null;

			if ( Commander == mob )
				Commander = null;

			if ( mob is PlayerMobile )
				((PlayerMobile)mob).ValidateEquipment();
		}

		public void JoinGuilded( PlayerMobile mob, Guild guild )
		{
			if ( mob.Young )
			{
				guild.RemoveMember( mob );
				mob.SendLocalizedMessage( 1042283 ); // You have been kicked out of your guild!  Young players may not remain in a guild which is allied with a faction.
			}
			else if ( AlreadyHasCharInFaction( mob ) )
			{
				guild.RemoveMember( mob );
				mob.SendLocalizedMessage( 1005281 ); // You have been kicked out of your guild due to factional overlap
			}
			else if ( IsFactionBanned( mob ) )
			{
				guild.RemoveMember( mob );
				mob.SendLocalizedMessage( 1005052 ); // You are currently banned from the faction system
			}
			else
			{
				AddMember( mob );
				mob.SendLocalizedMessage( 1042756, true, " " + m_Definition.FriendlyName ); // You are now joining a faction:
			}
		}

		public void JoinAlone( Mobile mob )
		{
			AddMember( mob );
			mob.SendLocalizedMessage( 1005058 ); // You have joined the faction
		}

		private bool AlreadyHasCharInFaction( Mobile mob )
		{
			Account acct = mob.Account as Account;

			if ( acct != null )
			{
				for ( int i = 0; i < acct.Length; ++i )
				{
					Mobile c = acct[i];

					if ( Find( c ) != null )
						return true;
				}
			}

			return false;
		}

		public static bool IsFactionBanned( Mobile mob )
		{
			Account acct = mob.Account as Account;

			if ( acct == null )
				return false;

			return ( acct.GetTag( "FactionBanned" ) != null );
		}

		// jakob, added this to check skills and stats
		public bool HasEnoughSkills( Mobile mob )
		{
			int gms = 0, totalStats = mob.Str + mob.Dex + mob.Int;
			for (int i = 0; i < mob.Skills.Length; i++)
				if (mob.Skills[i].Base > 99.9) // is gm
					gms++;

			// requires 700 or more skills, at least 5 GMs and at least 225 stats
			return mob.SkillsTotal >= 700 && gms >= 5 && totalStats >= 225;
		}
		// end

		public void OnJoinAccepted( Mobile mob )
		{
			PlayerMobile pm = mob as PlayerMobile;

			if ( pm == null )
				return; // sanity

			PlayerState pl = PlayerState.Find( pm );

			if ( pm.Young )
				pm.SendLocalizedMessage( 1010104 ); // You cannot join a faction as a young player
			else if ( pl != null && pl.IsLeaving )
				pm.SendLocalizedMessage( 1005051 ); // You cannot use the faction stone until you have finished quitting your current faction
			else if ( AlreadyHasCharInFaction( pm ) )
				pm.SendLocalizedMessage( 1005059 ); // You cannot join a faction because you already declared your allegiance with another character
			else if ( IsFactionBanned( mob ) )
				pm.SendLocalizedMessage( 1005052 ); // You are currently banned from the faction system
			else if ( pm.Guild != null )
			{
				Guild guild = pm.Guild as Guild;

				if ( guild.Leader != pm )
					pm.SendLocalizedMessage( 1005057 ); // You cannot join a faction because you are in a guild and not the guildmaster
				else if ( guild.Type != GuildType.Regular && !( ( guild.Type == GuildType.Chaos ) && ( this == Minax.Instance || this == Shadowlords.Instance ) )
				|| !( ( guild.Type == GuildType.Order ) && ( this == CouncilOfMages.Instance || this == TrueBritannians.Instance ) ) )
					pm.SendLocalizedMessage( 1042161 ); // You cannot join a faction because your guild is an Order or Chaos type.
				else if ( guild.Enemies != null && guild.Enemies.Count > 0 )
					pm.SendLocalizedMessage( 1005056 ); // You cannot join a faction with active Wars
				else
				{
					ArrayList members = new ArrayList( guild.Members );

					bool failedCheck = false;

					for ( int i = 0;!failedCheck && i < members.Count; i++ )
					{
						PlayerMobile member = members[i] as PlayerMobile;

						if ( !HasEnoughSkills( member ) )
							failedCheck = true;
					}

					if ( failedCheck )
						pm.SendMessage( "One or more of your guild's member does not have enough skills and stats to join this faction." );
					else
					{
						for ( int i = 0; i < members.Count; ++i )
						{
							PlayerMobile member = members[i] as PlayerMobile;

							if ( member != null )
								JoinGuilded( member, guild );
						}
					}
				}
			}
			//else if ( !CanHandleInflux( 1 ) )
				//pm.SendLocalizedMessage( 1018031 ); // In the interest of faction stability, this faction declines to accept new members for now.
			else
			{
				if ( HasEnoughSkills( mob ) )
					JoinAlone( mob );
				else
					pm.SendMessage( "You do not have enough skills or stats to join this faction." );
			}
		}

		public bool IsCommander( Mobile mob )
		{
			if ( mob == null )
				return false;

			// jakob, DeputyCommander now qualifies as a commander
			return ( mob.AccessLevel >= AccessLevel.GameMaster || mob == Commander || IsDeputyCommander( mob ) );
			// end
		}

		// jakob, check for deputy commander
		public bool IsDeputyCommander( Mobile mob )
		{
			if ( mob == null )
				return false;

			return ( mob == DeputyCommander );
		}
		// end

		public Faction()
		{
			m_State = new FactionState( this );
		}

		public override string ToString()
		{
			return m_Definition.FriendlyName;
		}

		public int CompareTo( object obj )
		{
			return m_Definition.Sort - ((Faction)obj).m_Definition.Sort;
		}

		public static bool CheckLeaveTimer( Mobile mob )
		{
			PlayerState pl = PlayerState.Find( mob );

			if ( pl == null || !pl.IsLeaving )
				return false;

			if ( (pl.Leaving + LeavePeriod) >= DateTime.Now )
				return false;

			mob.SendLocalizedMessage( 1005163 ); // You have now quit your faction

			pl.Faction.RemoveMember( mob );

			return true;
		}

		public void UpdateRanks()
		{
			PlayerStateCollection members = Members;

			ArrayList list = new ArrayList( members );

			list.Sort();

			RankDefinition[] ranks = m_Definition.Ranks;

			for ( int i = 0; i < list.Count; ++i )
			{
				PlayerState pl = (PlayerState)list[i];

				int percent;

				if ( list.Count == 1 )
					percent = 1000;
				else
					percent = (i * 1000) / (list.Count - 1);

				RankDefinition rank = null;

				for ( int j = 0; j < ranks.Length; ++j )
				{
					RankDefinition check = ranks[j];

					if ( percent >= check.Required )
					{
						rank = check;
						break;
					}
				}

				//RankDefinition rank = ranks[Math.Min(9, Math.Max(pl.KillPoints, 0))];

				pl.Rank = rank;
			}
		}

		public static void Initialize()
		{
			// jakob, added this to remove inactives on server start
			CleanUp();
			// end

			EventSink.Login += new LoginEventHandler( EventSink_Login );
			EventSink.Logout += new LogoutEventHandler( EventSink_Logout );

			Timer.DelayCall( TimeSpan.FromSeconds( 30.0 ), TimeSpan.FromSeconds( 30.0 ), new TimerCallback( ProcessTick ) );

			Commands.Register( "FactionElection", AccessLevel.GameMaster, new CommandEventHandler( FactionElection_OnCommand ) );
			Commands.Register( "FactionCommander", AccessLevel.Administrator, new CommandEventHandler( FactionCommander_OnCommand ) );
			Commands.Register( "FactionItemReset", AccessLevel.Administrator, new CommandEventHandler( FactionItemReset_OnCommand ) );
			Commands.Register( "FactionReset", AccessLevel.Administrator, new CommandEventHandler( FactionReset_OnCommand ) );
			Commands.Register( "FactionTownReset", AccessLevel.Administrator, new CommandEventHandler( FactionTownReset_OnCommand ) );

            Commands.Register("FactionUpdateRanks", AccessLevel.Administrator, new CommandEventHandler(FactionUpdateRanks_OnCommand));
		}

		private static readonly TimeSpan MustKillThisOften = TimeSpan.FromDays( 30.0 );

		// jakob, added this to remove inactives on server start
		public static void CleanUp()
		{
			Console.Write( "Cleaning up factions..." );
			int count = 0;
			// loop through every mobile that's a faction member
			FactionCollection factions = Faction.Factions;
			for ( int i = 0; i < factions.Count; i++ )
			{
				Faction f = factions[i];

				ArrayList list = new ArrayList( f.Members );

				for ( int j = 0; j < list.Count; j++ )
				{
					PlayerState ps = (PlayerState)list[j];
					Mobile m = ps.Mobile;

					// kick them if they haven't killed anybody in MustKillThisOften days
					//Edited my Kamron (This is the proper format & fixed bug with crafters)
                                        if ( DateTime.Now - ps.LastKill > MustKillThisOften && m.Skills[SkillName.Blacksmith].Base < 100.0 && m.Skills[SkillName.Tailoring].Base < 100.0 )
					{
						f.RemoveMember( m );
						count ++;
					}
				}
			}
			Console.WriteLine( "removed {0} inactives.", count );
		}
		// end

		public static void FactionTownReset_OnCommand( CommandEventArgs e )
		{
			MonolithCollection monoliths = BaseMonolith.Monoliths;

			for ( int i = 0; i < monoliths.Count; ++i )
				monoliths[i].Sigil = null;

			TownCollection towns = Town.Towns;

			for ( int i = 0; i < towns.Count; ++i )
			{
				towns[i].Silver = 0;
				towns[i].Sheriff = null;
				towns[i].Finance = null;
				towns[i].Tax = 0;
				towns[i].Owner = null;
			}

			SigilCollection sigils = Sigil.Sigils;

			for ( int i = 0; i < sigils.Count; ++i )
			{
				sigils[i].Corrupted = null;
				sigils[i].Corrupting = null;
				sigils[i].LastStolen = DateTime.MinValue;
				sigils[i].GraceStart = DateTime.MinValue;
				sigils[i].CorruptionStart = DateTime.MinValue;
				sigils[i].PurificationStart = DateTime.MinValue;
				sigils[i].LastMonolith = null;
				sigils[i].ReturnHome();
			}

			FactionCollection factions = Faction.Factions;

			for ( int i = 0; i < factions.Count; ++i )
			{
				Faction f = factions[i];

				ArrayList list = new ArrayList( f.State.FactionItems );

				for ( int j = 0; j < list.Count; ++j )
				{
					FactionItem fi = (FactionItem)list[j];

					if ( fi.Expiration == DateTime.MinValue )
						fi.Item.Delete();
					else
						fi.Detach();
				}
			}
		}

		public static void FactionReset_OnCommand( CommandEventArgs e )
		{
			MonolithCollection monoliths = BaseMonolith.Monoliths;

			for ( int i = 0; i < monoliths.Count; ++i )
				monoliths[i].Sigil = null;

			TownCollection towns = Town.Towns;

			for ( int i = 0; i < towns.Count; ++i )
			{
				towns[i].Silver = 0;
				towns[i].Sheriff = null;
				towns[i].Finance = null;
				towns[i].Tax = 0;
				towns[i].Owner = null;
			}

			SigilCollection sigils = Sigil.Sigils;

			for ( int i = 0; i < sigils.Count; ++i )
			{
				sigils[i].Corrupted = null;
				sigils[i].Corrupting = null;
				sigils[i].LastStolen = DateTime.MinValue;
				sigils[i].GraceStart = DateTime.MinValue;
				sigils[i].CorruptionStart = DateTime.MinValue;
				sigils[i].PurificationStart = DateTime.MinValue;
				sigils[i].LastMonolith = null;
				sigils[i].ReturnHome();
			}

			FactionCollection factions = Faction.Factions;

			for ( int i = 0; i < factions.Count; ++i )
			{
				Faction f = factions[i];

				ArrayList list = new ArrayList( f.Members );

				for ( int j = 0; j < list.Count; ++j )
					f.RemoveMember( ((PlayerState)list[j]).Mobile );

				list = new ArrayList( f.State.FactionItems );

				for ( int j = 0; j < list.Count; ++j )
				{
					FactionItem fi = (FactionItem)list[j];

					if ( fi.Expiration == DateTime.MinValue )
						fi.Item.Delete();
					else
						fi.Detach();
				}

				list = new ArrayList( f.Traps );

				for ( int j = 0; j < list.Count; ++j )
					((BaseFactionTrap)list[j]).Delete();
			}
		}

		public static void FactionItemReset_OnCommand( CommandEventArgs e )
		{
			ArrayList toreset = new ArrayList();

			foreach ( Item item in World.Items.Values )
			{
				if ( item is IFactionItem && !(item is HoodedShroudOfShadows) )
					toreset.Add( item );
			}
/*
			int[] hues = new int[Factions.Count * 2];

			for ( int i = 0; i < Factions.Count; ++i )
			{
				hues[0+(i*2)] = Factions[i].Definition.HuePrimary;
				hues[1+(i*2)] = Factions[i].Definition.HueSecondary;
			}
*/
			int count = 0;

			for ( int i = 0; i < toreset.Count; ++i )
			{
				Item item = (Item)toreset[i];
				IFactionItem fci = (IFactionItem)item;

				if ( fci.FactionItemState != null )
				{
					fci.FactionItemState = null;
					++count;
				}
			}

			e.Mobile.SendMessage( "{0} items reset", count );
		}

        public static void FactionUpdateRanks_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendMessage("Updating faction ranks...");
            DateTime startTime=DateTime.Now;

            e.Mobile.SendMessage("-Minax...");
            Minax.Instance.UpdateRanks();

            e.Mobile.SendMessage("-True Britannias...");
            TrueBritannians.Instance.UpdateRanks();

            e.Mobile.SendMessage("-Shadowlords...");
            Shadowlords.Instance.UpdateRanks();

            e.Mobile.SendMessage("-Council Of Mages...");
            CouncilOfMages.Instance.UpdateRanks();

            e.Mobile.SendMessage("Done. Update took {0:F1} seconds.", (DateTime.Now - startTime).TotalSeconds);
        }

        public static void FactionCommander_OnCommand(CommandEventArgs e)
		{
			e.Mobile.SendMessage( "Target a player to make them the faction commander." );
			e.Mobile.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( FactionCommander_OnTarget ) );
		}

		public static void FactionCommander_OnTarget( Mobile from, object obj )
		{
			if ( obj is PlayerMobile )
			{
				Mobile targ = (Mobile)obj;
				PlayerState pl = PlayerState.Find( targ );

				if ( pl != null )
				{
					pl.Faction.Commander = targ;
					from.SendMessage( "You have appointed them as the faction commander." );
				}
				else
				{
					from.SendMessage( "They are not in a faction." );
				}
			}
			else
			{
				from.SendMessage( "That is not a player." );
			}
		}

		public static void FactionElection_OnCommand( CommandEventArgs e )
		{
			e.Mobile.SendMessage( "Target a faction stone to open its election properties." );
			e.Mobile.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( FactionElection_OnTarget ) );
		}

		public static void FactionElection_OnTarget( Mobile from, object obj )
		{
			if ( obj is FactionStone )
			{
				Faction faction = ((FactionStone)obj).Faction;

				if ( faction != null )
					from.SendGump( new ElectionManagementGump( faction.Election ) );
					//from.SendGump( new Gumps.PropertiesGump( from, faction.Election ) );
				else
					from.SendMessage( "That stone has no faction assigned." );
			}
			else
			{
				from.SendMessage( "That is not a faction stone." );
			}
		}

		public static void FactionKick_OnCommand( CommandEventArgs e )
		{
			e.Mobile.SendMessage( "Target a player to remove them from their faction." );
			e.Mobile.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( FactionKick_OnTarget ) );
		}

		public static void FactionKick_OnTarget( Mobile from, object obj )
		{
			if ( obj is Mobile )
			{
				Mobile mob = (Mobile) obj;
				PlayerState pl = PlayerState.Find( (Mobile) mob );

				if ( pl != null )
				{
					pl.Faction.RemoveMember( mob );

					mob.SendMessage( "You have been kicked from your faction." );
					from.SendMessage( "They have been kicked from their faction." );
				}
				else
				{
					from.SendMessage( "They are not in a faction." );
				}
			}
			else
			{
				from.SendMessage( "That is not a player." );
			}
		}

		public static void ProcessTick()
		{
			SigilCollection sigils = Sigil.Sigils;

			for ( int i = 0; i < sigils.Count; ++i )
			{
				Sigil sigil = sigils[i];

				if ( !sigil.IsBeingCorrupted && sigil.GraceStart != DateTime.MinValue && (sigil.GraceStart + Sigil.CorruptionGrace) < DateTime.Now )
				{
					if ( sigil.LastMonolith is StrongholdMonolith )
					{
						sigil.Corrupting = sigil.LastMonolith.Faction;
						sigil.CorruptionStart = DateTime.Now;
					}
					else
					{
						sigil.Corrupting = null;
						sigil.CorruptionStart = DateTime.MinValue;
					}

					sigil.GraceStart = DateTime.MinValue;
				}

				if ( sigil.LastMonolith == null || sigil.LastMonolith.Sigil == null )
				{
					if ( (sigil.LastStolen + Sigil.ReturnPeriod) < DateTime.Now )
						sigil.ReturnHome();
				}
				else
				{
					if ( sigil.IsBeingCorrupted && (sigil.CorruptionStart + Sigil.CorruptionPeriod) < DateTime.Now )
					{
						sigil.Corrupted = sigil.Corrupting;
						sigil.Corrupting = null;
						sigil.CorruptionStart = DateTime.MinValue;
						sigil.GraceStart = DateTime.MinValue;
					}
					else if ( sigil.IsPurifying && (sigil.PurificationStart + Sigil.PurificationPeriod) < DateTime.Now )
					{
						sigil.PurificationStart = DateTime.MinValue;
						sigil.Corrupted = null;
						sigil.Corrupting = null;
						sigil.CorruptionStart = DateTime.MinValue;
						sigil.GraceStart = DateTime.MinValue;
					}
				}
			}
		}

		public static void HandleDeath( Mobile mob )
		{
			HandleDeath( mob, null );
		}

		#region Skill Loss
		public const double SkillLossFactor = 1.0 / 3;
		public static readonly TimeSpan SkillLossPeriod = TimeSpan.FromMinutes( 5.0 );

		private static Hashtable m_SkillLoss = new Hashtable();

		private class SkillLossContext
		{
			public Timer m_Timer;
			public ArrayList m_Mods;
		}

		public static void ApplySkillLoss( Mobile mob )
		{
			SkillLossContext context = (SkillLossContext)m_SkillLoss[mob];

			if ( context != null )
				return;

			context = new SkillLossContext();
			m_SkillLoss[mob] = context;

			ArrayList mods = context.m_Mods = new ArrayList();

			for ( int i = 0; i < mob.Skills.Length; ++i )
			{
				Skill sk = mob.Skills[i];
				double baseValue = sk.Base;

				if ( baseValue > 0 )
				{
					SkillMod mod = new DefaultSkillMod( sk.SkillName, true, -(baseValue * SkillLossFactor) );

					mods.Add( mod );
					mob.AddSkillMod( mod );
				}
			}


			// jakob, added this to shorten skillloss timer by 1 minute per owned town
			int ownedTowns = 0;
			PlayerState ps = PlayerState.Find( mob );
			if ( ps != null )
				foreach ( Town town in Town.Towns )
				{
					if ( town.Owner == ps.Faction )
						ownedTowns++;
				}

			context.m_Timer = Timer.DelayCall( SkillLossPeriod - TimeSpan.FromMinutes( (double)ownedTowns ), new TimerStateCallback( ClearSkillLoss_Callback ), mob );
			// end
		}

		private static void ClearSkillLoss_Callback( object state )
		{
			ClearSkillLoss( (Mobile) state );
		}

		public static void ClearSkillLoss( Mobile mob )
		{
			SkillLossContext context = (SkillLossContext)m_SkillLoss[mob];

			if ( context == null )
				return;

			m_SkillLoss.Remove( mob );

			ArrayList mods = context.m_Mods;

			for ( int i = 0; i < mods.Count; ++i )
				mob.RemoveSkillMod( (SkillMod) mods[i] );

			context.m_Timer.Stop();
		}

		public static bool HasSkillLoss( Mobile mob )
		{
			SkillLossContext context = (SkillLossContext)m_SkillLoss[mob];
			return (context != null);
		}
		#endregion

		public int AwardSilver( Mobile mob, int silver )
		{
			if ( silver <= 0 )
				return 0;

			int tithed = ( silver * Tithe ) / 100;

			Silver += tithed;

			silver = silver - tithed;

			if ( silver > 0 )
				mob.AddToBackpack( new Silver( silver ) );

			return silver;
		}

		public virtual int MaximumTraps{ get{ return 15; } }

		public FactionTrapCollection Traps
		{
			get{ return m_State.Traps; }
			set{ m_State.Traps = value; }
		}

		// jakob, changed this for stability
		public const int StabilityFactor = 120; // 20% greater than smallest faction
		public const int StabilityActivation = 1000; // Stablity code goes into effect when largest faction has > 50 people
		// end

		public static Faction FindSmallestFaction()
		{
			FactionCollection factions = Factions;
			Faction smallest = null;

			for ( int i = 0; i < factions.Count; ++i )
			{
				Faction faction = factions[i];

				if ( smallest == null || faction.Members.Count < smallest.Members.Count )
					smallest = faction;
			}

			return smallest;
		}

		public static bool StabilityActive()
		{
			FactionCollection factions = Factions;

			for ( int i = 0; i < factions.Count; ++i )
			{
				Faction faction = factions[i];

				if ( faction.Members.Count > StabilityActivation )
					return true;
			}

			return false;
		}

		//public bool CanHandleInflux( int influx )
		//{
			//if( !StabilityActive())
			//	return true;
			//
			//Faction smallest = FindSmallestFaction();
			//
			//if ( smallest == null )
			//	return true; // sanity
			//
			//if ( StabilityFactor > 0 && (((this.Members.Count + influx) * 100) / StabilityFactor) > smallest.Members.Count )
			//return false;
			//
			//return true;
			//}

		// jakob, added this for a change of getting a rare on death
		private const double RareChancePerOwnedTown = 0.00625;
		// end

		public static void HandleDeath( Mobile victim, Mobile killer )
		{
            if ((victim != null && victim.Region is CustomRegion && ((CustomRegion)victim.Region).NoFactionEffects) ||
                (killer != null && killer.Region is CustomRegion && ((CustomRegion)killer.Region).NoFactionEffects))
                    return;

            if ( killer == null )
				killer = victim.FindMostRecentDamager( true );


			PlayerState victimState = PlayerState.Find( victim );

			//to give statloss from guards
			if ( victimState != null && killer is BaseFactionGuard && ((BaseFactionGuard)killer).Faction != victimState.Faction )
				ApplySkillLoss( victim );

			PlayerState killerState = PlayerState.Find( killer );

			Container pack = victim.Backpack;

			if ( pack != null )
			{
				Container killerPack = ( killer == null ? null : killer.Backpack );
				Item[] sigils = pack.FindItemsByType( typeof( Sigil ) );

				for ( int i = 0; i < sigils.Length; ++i )
				{
					Sigil sigil = (Sigil)sigils[i];

					if ( killerState != null && killerPack != null )
					{
						if ( Sigil.ExistsOn( killer ) )
						{
							sigil.ReturnHome();
							killer.SendLocalizedMessage( 1010258 ); // The sigil has gone back to its home location because you already have a sigil.
						}
						else if ( !killerPack.TryDropItem( killer, sigil, false ) )
						{
							sigil.ReturnHome();
							killer.SendLocalizedMessage( 1010259 ); // The sigil has gone home because your backpack is full.
						}
					}
					else
					{
						sigil.ReturnHome();
					}
				}
			}

			if ( killerState == null )
				return;

			if ( victim is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)victim;
				Faction victimFaction = bc.FactionAllegiance;

                bool bcRegionIsNoFactions = (bc.Region is CustomRegion && ((CustomRegion)bc.Region).NoFactionEffects);
                if ( Faction.IsFactionMap(bc.Map) && victimFaction != null && killerState.Faction != victimFaction && !bcRegionIsNoFactions)
				{
					int silver = killerState.Faction.AwardSilver( killer, bc.FactionSilverWorth );

					if ( silver > 0 )
						killer.SendLocalizedMessage( 1042748, silver.ToString( "N0" ) ); // Thou hast earned ~1_AMOUNT~ silver for vanquishing the vile creature.
				}

				return;
			}

			//PlayerState victimState = PlayerState.Find( victim );

			if ( victimState == null )
				return;

			// MODIFICATIONS FOR Capture the Flag / Double Dom games
			Server.Items.CTFTeam ft = Server.Items.CTFGame.FindTeamFor( killer );
			if ( ft != null )
			{
				Server.Items.CTFTeam tt = Server.Items.CTFGame.FindTeamFor( victim );
				if ( tt != null && ft.Game == tt.Game )
					return;
			}
			// END

			// MODIFICATIONS TO award correct player for faction kill ( last damager )
			TimeSpan lastTime = TimeSpan.MaxValue;

			for ( int i = 0; i < victim.Aggressors.Count; ++i )
			{
				AggressorInfo info = (AggressorInfo)victim.Aggressors[i];
				if ( info.Attacker != null && info.Attacker is PlayerMobile && info.Attacker.Alive && (DateTime.Now - info.LastCombatTime) < lastTime && (DateTime.Now - info.LastCombatTime) < TimeSpan.FromMinutes( 2.0 ) )
				{
					PlayerMobile tempkiller = info.Attacker as PlayerMobile;
					PlayerState tempkillerState = PlayerState.Find( tempkiller );
					if ( tempkillerState != null && tempkillerState.Faction != victimState.Faction )
					{
						killer = tempkiller;
						killerState = tempkillerState;
						lastTime = (DateTime.Now - info.LastCombatTime);
					}
				}
			}

			for ( int i = 0; i < victim.Aggressed.Count; ++i )
			{
				AggressorInfo info = (AggressorInfo)victim.Aggressed[i];
				if ( info.Defender != null && info.Defender is PlayerMobile && info.Defender.Alive && (DateTime.Now - info.LastCombatTime) < lastTime && (DateTime.Now - info.LastCombatTime) < TimeSpan.FromMinutes( 2.0 ) )
				{
					PlayerMobile tempkiller = info.Defender as PlayerMobile;
					PlayerState tempkillerState = PlayerState.Find( tempkiller );
					if ( tempkillerState != null && tempkillerState.Faction != victimState.Faction )
					{
						killer = tempkiller;
						killerState = tempkillerState;
						lastTime = (DateTime.Now - info.LastCombatTime);
					}
				}
			}
			// END



			if ( victim == null || killer == null || victimState == null || killerState == null )
				return;

			if ( killer == victim || killerState.Faction != victimState.Faction )
				ApplySkillLoss( victim );

			if ( killerState.Faction != victimState.Faction )
			{
				if ( victimState.KillPoints <= -6 )
				{
					killer.SendLocalizedMessage( 501693 ); // This victim is not worth enough to get kill points from.
				}
				else
				{
					int award = Math.Max( victimState.KillPoints / 10, 1 );

					if ( award > 40 )
						award = 40;

					if ( victimState.CanGiveSilverTo( killer ) )
					{
						if ( victimState.KillPoints > 0 )
						{
							int silver = 0;

							silver = killerState.Faction.AwardSilver( killer, award * 40 );

							if ( silver > 0 )
								killer.SendLocalizedMessage( 1042736, String.Format( "{0:N0} silver\t{1}", silver, victim.Name ) ); // You have earned ~1_SILVER_AMOUNT~ pieces for vanquishing ~2_PLAYER_NAME~!
						}

						victimState.KillPoints -= award;
						killerState.KillPoints += award;

						int offset = ( award != 1 ? 0 : 2 ); // for pluralization

						string args = String.Format( "{0}\t{1}\t{2}", award, victim.Name, killer.Name );

						killer.SendLocalizedMessage( 1042737 + offset, args ); // Thou hast been honored with ~1_KILL_POINTS~ kill point(s) for vanquishing ~2_DEAD_PLAYER~!
						victim.SendLocalizedMessage( 1042738 + offset, args ); // Thou has lost ~1_KILL_POINTS~ kill point(s) to ~3_ATTACKER_NAME~ for being vanquished!

						victimState.OnGivenSilverTo( killer );

						killerState.LastKill = DateTime.Now;

						// jakob, added this to hand out rares
						double bonusChance = killerState.Faction.OwnedTowns * RareChancePerOwnedTown;
						if ( bonusChance > Utility.RandomDouble() )
						{
							Type rareType = FactionRares.Rares[Utility.Random(FactionRares.Rares.Length)];
							Item toGive = null;

							if ( rareType == typeof(RunicHammer) ) // special case for runic hammer
							{
								CraftResource resource = (CraftResource)(Utility.Random(9) + 1);
								int uses = Utility.Random( 1, 5 );

								toGive = new RunicHammer( resource, uses );
							}
							else
							{
								try
								{
									toGive = (Item)Activator.CreateInstance( rareType );
								}
								catch(Exception e)
								{
									Console.WriteLine( "Error when rewarding faction member: {0}", e.ToString() );
								}
							}

							if ( toGive != null )
							{
								killer.AddToBackpack( toGive );
								killer.SendMessage( "You have been given something special." );
							}
						}
						// end
					}
					else
					{
						killer.SendLocalizedMessage( 1042231 ); // You have recently defeated this enemy and thus their death brings you no honor.
					}
				}
			}
		}

		private static void EventSink_Logout( LogoutEventArgs e )
		{
			Mobile mob = e.Mobile;

			Container pack = mob.Backpack;

			if ( pack == null )
				return;

			Item[] sigils = pack.FindItemsByType( typeof( Sigil ) );

			for ( int i = 0; i < sigils.Length; ++i )
				((Sigil)sigils[i]).ReturnHome();
		}

		private static void EventSink_Login( LoginEventArgs e )
		{
			Mobile mob = e.Mobile;

			CheckLeaveTimer( mob );
		}

		public static readonly Map Facet = Map.Felucca;

        //Al: Added this to enable multiple maps for factions.
        public static bool IsFactionMap(Map map)
        {
            return true; //All maps are faction maps.
        }

		public static void WriteReference( GenericWriter writer, Faction fact )
		{
			int idx = Factions.IndexOf( fact );

			writer.WriteEncodedInt( (int) (idx + 1) );
		}

		public static FactionCollection Factions{ get{ return Reflector.Factions; } }

		public static Faction ReadReference( GenericReader reader )
		{
			int idx = reader.ReadEncodedInt() - 1;

			if ( idx >= 0 && idx < Factions.Count )
				return Factions[idx];

			return null;
		}

		public static Faction Find( Mobile mob )
		{
			return Find( mob, false, false );
		}

		public static Faction Find( Mobile mob, bool inherit )
		{
			return Find( mob, inherit, false );
		}

		public static Faction Find( Mobile mob, bool inherit, bool creatureAllegiances )
		{
			PlayerState pl = PlayerState.Find( mob );

			if ( pl != null )
				return pl.Faction;

			if ( inherit && mob is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)mob;

				if ( bc.Controlled )
					return Find( bc.ControlMaster, false );
				else if ( bc.Summoned )
					return Find( bc.SummonMaster, false );
				else if ( creatureAllegiances && mob is BaseFactionGuard )
					return ((BaseFactionGuard)mob).Faction;
				else if ( creatureAllegiances )
					return bc.FactionAllegiance;
			}

			return null;
		}

		public static Faction Parse( string name )
		{
			FactionCollection factions = Factions;

			for ( int i = 0; i < factions.Count; ++i )
			{
				Faction faction = factions[i];

				if ( Insensitive.Equals( faction.Definition.FriendlyName, name ) )
					return faction;
			}

			return null;
		}
	}

	public enum FactionKickType
	{
		Kick,
		Ban,
		Unban
	}

	public class FactionKickCommand : BaseCommand
	{
		private FactionKickType m_KickType;

		public FactionKickCommand( FactionKickType kickType )
		{
			m_KickType = kickType;

			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.AllMobiles;
			ObjectTypes = ObjectTypes.Mobiles;

			switch ( m_KickType )
			{
				case FactionKickType.Kick:
				{
					Commands = new string[]{ "FactionKick" };
					Usage = "FactionKick";
					Description = "Kicks the targeted player out of his current faction. This does not prevent them from rejoining.";
					break;
				}
				case FactionKickType.Ban:
				{
					Commands = new string[]{ "FactionBan" };
					Usage = "FactionBan";
					Description = "Bans the account of a targeted player from joining factions. All players on the account are removed from their current faction, if any.";
					break;
				}
				case FactionKickType.Unban:
				{
					Commands = new string[]{ "FactionUnban" };
					Usage = "FactionUnban";
					Description = "Unbans the account of a targeted player from joining factions.";
					break;
				}
			}
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			Mobile mob = (Mobile)obj;

			switch ( m_KickType )
			{
				case FactionKickType.Kick:
				{
					PlayerState pl = PlayerState.Find( mob );

					if ( pl != null )
					{
						pl.Faction.RemoveMember( mob );
						mob.SendMessage( "You have been kicked from your faction." );
						AddResponse( "They have been kicked from their faction." );
					}
					else
					{
						LogFailure( "They are not in a faction." );
					}

					break;
				}
				case FactionKickType.Ban:
				{
					Account acct = mob.Account as Account;

					if ( acct != null )
					{
						if ( acct.GetTag( "FactionBanned" ) == null )
						{
							acct.SetTag( "FactionBanned", "true" );
							AddResponse( "The account has been banned from joining factions." );
						}
						else
						{
							AddResponse( "The account is already banned from joining factions." );
						}

						for ( int i = 0; i < acct.Length; ++i )
						{
							mob = acct[i];

							if ( mob != null )
							{
								PlayerState pl = PlayerState.Find( mob );

								if ( pl != null )
								{
									pl.Faction.RemoveMember( mob );
									mob.SendMessage( "You have been kicked from your faction." );
									AddResponse( "They have been kicked from their faction." );
								}
							}
						}
					}
					else
					{
						LogFailure( "They have no assigned account." );
					}

					break;
				}
				case FactionKickType.Unban:
				{
					Account acct = mob.Account as Account;

					if ( acct != null )
					{
						if ( acct.GetTag( "FactionBanned" ) == null )
						{
							AddResponse( "The account is not already banned from joining factions." );
						}
						else
						{
							acct.RemoveTag( "FactionBanned" );
							AddResponse( "The account may now freely join factions." );
						}
					}
					else
					{
						LogFailure( "They have no assigned account." );
					}

					break;
				}
			}
		}
	}
}