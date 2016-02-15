using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Guilds;
using Server.Multis;
using Server.Mobiles;
using Server.Engines.PartySystem;
using Server.Factions;
using Server.Ladder;
using Server.Regions;

namespace Server.Misc
{
	public class NotorietyHandlers
	{
		public static void Initialize()
		{
			Notoriety.Hues[Notoriety.Innocent]		= 0x59;
			Notoriety.Hues[Notoriety.Ally]			= 0x3F;
			Notoriety.Hues[Notoriety.CanBeAttacked]	= 0x3B2;
			Notoriety.Hues[Notoriety.Criminal]		= 0x3B2;
			Notoriety.Hues[Notoriety.Enemy]			= 0x90;
			Notoriety.Hues[Notoriety.Murderer]		= 0x22;
			Notoriety.Hues[Notoriety.Invulnerable]	= 0x35;

			Notoriety.Handler = new NotorietyHandler( MobileNotoriety );

			Mobile.AllowBeneficialHandler = new AllowBeneficialHandler( Mobile_AllowBeneficial );
			Mobile.AllowHarmfulHandler = new AllowHarmfulHandler( Mobile_AllowHarmful );
		}

		private enum GuildStatus{ None, Peaceful, Waring }

		private static GuildStatus GetGuildStatus( Mobile m )
		{
			if ( m.Guild == null )
				return GuildStatus.None;
			else if ( ((Guild)m.Guild).Enemies.Count == 0 && m.Guild.Type == GuildType.Regular )
				return GuildStatus.Peaceful;

			return GuildStatus.Waring;
		}

		private static bool CheckBeneficialStatus( GuildStatus from, GuildStatus target )
		{
			if ( from == GuildStatus.Waring || target == GuildStatus.Waring )
				return false;

			return true;
		}

		public static bool Mobile_AllowBeneficial( Mobile from, Mobile target )
		{
			if ( from == null || target == null )
				return true;

			// MODIFICATIONS FOR Capture the Flag / Double Dom games
			Server.Items.CTFTeam ft = Server.Items.CTFGame.FindTeamFor( from );
			if ( ft != null )
			{
				Server.Items.CTFTeam tt = Server.Items.CTFGame.FindTeamFor( target );
				if ( tt != null && ft == tt )
				{
					return true;
				}
			}

			#region Factions
			Faction targetFaction = Faction.Find( target, true );

			if ( targetFaction != null )
			{
				if ( Faction.Find( from, true ) != targetFaction )
					return false;
			}
			#endregion

			Map map = from.Map;

			if ( map != null && (map.Rules & MapRules.BeneficialRestrictions) == 0 )
				return true; // In felucca, anything goes

			if ( !from.Player )
				return true; // NPCs have no restrictions

			if ( target is BaseCreature && !((BaseCreature)target).Controlled )
				return false; // Players cannot heal uncontrolled mobiles

			if ( from is PlayerMobile && ((PlayerMobile)from).Young && ( !(target is PlayerMobile) || !((PlayerMobile)target).Young ) )
				return false; // Young players cannot perform beneficial actions towards older players

			Guild fromGuild = from.Guild as Guild;
			Guild targetGuild = target.Guild as Guild;

			if ( fromGuild != null && targetGuild != null && (targetGuild == fromGuild || fromGuild.IsAlly( targetGuild )) )
				return true; // Guild members can be beneficial

			return CheckBeneficialStatus( GetGuildStatus( from ), GetGuildStatus( target ) );
		}

		public static bool Mobile_AllowHarmful( Mobile from, Mobile target )
		{
			if ( from == null || target == null )
				return true;

			// MODIFICATIONS FOR Capture the Flag / Double Dom games
			Server.Items.CTFTeam ft = Server.Items.CTFGame.FindTeamFor( from );
			if ( ft != null )
			{
				Server.Items.CTFTeam tt = Server.Items.CTFGame.FindTeamFor( target );
				if ( tt != null && ft == tt )
				{
					return false;
				}
			}

			Map map = from.Map;

			if ( map != null && (map.Rules & MapRules.HarmfulRestrictions) == 0 )
				return true; // In felucca, anything goes

			if ( !from.Player && !(from is BaseCreature && (((BaseCreature)from).Controlled || ((BaseCreature)from).Summoned)) )
			{
				if ( !CheckAggressor( from.Aggressors, target ) && !CheckAggressed( from.Aggressed, target ) && target is PlayerMobile && ((PlayerMobile)target).CheckYoungProtection( from ) )
					return false;

				return true; // Uncontrolled NPCs are only restricted by the young system
			}

			Guild fromGuild = GetGuildFor( from.Guild as Guild, from );
			Guild targetGuild = GetGuildFor( target.Guild as Guild, target );

			if ( fromGuild != null && targetGuild != null && (fromGuild == targetGuild || fromGuild.IsAlly( targetGuild ) || fromGuild.IsEnemy( targetGuild )) )
				return true; // Guild allies or enemies can be harmful

			if ( target is BaseCreature && (((BaseCreature)target).Controlled || (((BaseCreature)target).Summoned && from != ((BaseCreature)target).SummonMaster)) )
				return false; // Cannot harm other controlled mobiles

			if ( target.Player )
				return false; // Cannot harm other players

			if ( !(target is BaseCreature && ((BaseCreature)target).InitialInnocent) )
			{
				if ( Notoriety.Compute( from, target ) == Notoriety.Innocent )
					return false; // Cannot harm innocent mobiles
			}

			return true;
		}

		public static Guild GetGuildFor( Guild def, Mobile m )
		{
			Guild g = def;

			BaseCreature c = m as BaseCreature;

			if ( c != null && c.Controlled && c.ControlMaster != null )
			{
				c.DisplayGuildTitle = false;

				if ( c.Map != Map.Internal && (c.ControlOrder == OrderType.Attack || c.ControlOrder == OrderType.Guard) )
					g = (Guild)(c.Guild = c.ControlMaster.Guild);
				else if ( c.Map == Map.Internal || c.ControlMaster.Guild == null )
					g = (Guild)(c.Guild = null);
			}

			return g;
		}

        // XLX added.
		public static int CheckCowardice( ArrayList list, Mobile target )

         {
         for ( int i = 0; i < list.Count; ++i )
         {
            CowardiceInfo info = (CowardiceInfo)list[i];

           if( list == null || target == null)
           return Notoriety.Invulnerable;

            if ( info.Coward == target )
               return info.Notoriety;
         }

         return Notoriety.Invulnerable; // They aren't necessarily invul, but it's a good return code because not static
         }
            //XLX End.

		public static int CorpseNotoriety( Mobile source, Corpse target )
		{
			if ( target.AccessLevel > AccessLevel.Player )
				return Notoriety.CanBeAttacked;

			Body body = (Body) target.Amount;

			BaseCreature cretOwner = target.Owner as BaseCreature;

			if ( cretOwner != null )
			{
				// Mod for ladder
				if (Ladder.Ladder.IsLootable(target))
					return Notoriety.Enemy;

				Guild sourceGuild = GetGuildFor( source.Guild as Guild, source );
				Guild targetGuild = GetGuildFor( target.Guild as Guild, target.Owner );

				if ( sourceGuild != null && targetGuild != null )
				{
					if ( sourceGuild == targetGuild || sourceGuild.IsAlly( targetGuild ) )
						return Notoriety.Ally;
					else if ( sourceGuild.IsEnemy( targetGuild ) )
						return Notoriety.Enemy;
				}

				Faction srcFaction = Faction.Find( source, true, true );
				Faction trgFaction = Faction.Find( target.Owner, true, true );
                bool srcRegionIsNoFactions = (source.Region is CustomRegion && ((CustomRegion)source.Region).NoFactionEffects);

                if ( srcFaction != null && trgFaction != null && srcFaction != trgFaction && Faction.IsFactionMap(source.Map) && !srcRegionIsNoFactions)
					return Notoriety.Enemy;

				if ( CheckHouseFlag( source, target.Owner, target.Location, target.Map ) )
					return Notoriety.CanBeAttacked;

				int actual = Notoriety.CanBeAttacked;

				if ( target.Kills >= 5 || (body.IsMonster && IsSummoned( target.Owner as BaseCreature )) || (target.Owner is BaseCreature && (((BaseCreature)target.Owner).AlwaysMurderer || ((BaseCreature)target.Owner).IsAnimatedDead)) )
					actual = Notoriety.Murderer;

				if ( DateTime.Now >= (target.TimeOfDeath + Corpse.MonsterLootRightSacrifice) )
					return actual;

				Party sourceParty = Party.Get( source );

				ArrayList list = target.Aggressors;

				for ( int i = 0; i < list.Count; ++i )
				{
					if ( list[i] == source || (sourceParty != null && Party.Get( (Mobile)list[i] ) == sourceParty) )
						return actual;
				}

				return Notoriety.Innocent;
			}
			else
			{
				// Mod for ladder
				if (Ladder.Ladder.IsLootable(target))
					return Notoriety.Enemy;

				if ( target.Kills >= 5 || (body.IsMonster && IsSummoned( target.Owner as BaseCreature )) || (target.Owner is BaseCreature && (((BaseCreature)target.Owner).AlwaysMurderer || ((BaseCreature)target.Owner).IsAnimatedDead)) )
					return Notoriety.Murderer;

				if ( target.Criminal )
					return Notoriety.Criminal;

				Guild sourceGuild = GetGuildFor( source.Guild as Guild, source );
				Guild targetGuild = GetGuildFor( target.Guild as Guild, target.Owner );

				if ( sourceGuild != null && targetGuild != null )
				{
					if ( sourceGuild == targetGuild || sourceGuild.IsAlly( targetGuild ) )
						return Notoriety.Ally;
					else if ( sourceGuild.IsEnemy( targetGuild ) )
						return Notoriety.Enemy;
				}

				Faction srcFaction = Faction.Find( source, true, true );
				Faction trgFaction = Faction.Find( target.Owner, true, true );
                bool srcRegionIsNoFactions = (source.Region is CustomRegion && ((CustomRegion)source.Region).NoFactionEffects);

				if ( srcFaction != null && trgFaction != null && srcFaction != trgFaction && Faction.IsFactionMap(source.Map) && !srcRegionIsNoFactions)
				{
					return Notoriety.Enemy;
					/*ArrayList secondList = target.Aggressors;

					for ( int i = 0; i < secondList.Count; ++i )
					{
						if ( secondList[i] == source || secondList[i] is BaseFactionGuard )
							return Notoriety.Enemy;
					}*/
				}

				if ( target.Owner != null && target.Owner is BaseCreature && ((BaseCreature)target.Owner).AlwaysAttackable )
					return Notoriety.CanBeAttacked;

				if ( CheckHouseFlag( source, target.Owner, target.Location, target.Map ) )
					return Notoriety.CanBeAttacked;

				if ( !(target.Owner is PlayerMobile) && !IsPet( target.Owner as BaseCreature ) )
					return Notoriety.CanBeAttacked;

				ArrayList list = target.Aggressors;

				for ( int i = 0; i < list.Count; ++i )
				{
					if ( list[i] == source )
						return Notoriety.CanBeAttacked;
				}

				return Notoriety.Innocent;
			}
		}

		public static int MobileNotoriety( Mobile source, Mobile target )
		{
            if ( Core.AOS && (target.Blessed || (target is BaseVendor && ((BaseVendor)target).IsInvulnerable) || target is PlayerVendor || target is TownCrier) )
				return Notoriety.Invulnerable;

			if ( target.AccessLevel > AccessLevel.Player )
				return Notoriety.CanBeAttacked;

            if ( ( target.Region is CustomRegion && ((CustomRegion)target.Region).AlwaysGrey ) || target.Region.Name == "Buccaneer's Den" )
                return Notoriety.CanBeAttacked;

			// Mod for Ladder
			if (Ladder.Ladder.IsDuelling(source, target))
				return Notoriety.Enemy;

			// MODIFICATIONS FOR Capture the Flag / Double Dom games
			Server.Items.CTFTeam ft = Server.Items.CTFGame.FindTeamFor( source );
			if ( ft != null )
			{
				Server.Items.CTFTeam tt = Server.Items.CTFGame.FindTeamFor( target );
				if ( tt != null && ft.Game == tt.Game )
					return ft == tt ? Notoriety.Ally : Notoriety.Enemy;
			}

			if ( source.Player && !target.Player && source is PlayerMobile && target is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)target;

				if ( !bc.Summoned && !bc.Controlled && ((PlayerMobile)source).EnemyOfOneType == target.GetType() )
					return Notoriety.Enemy;
			}

			// add by xlx to allow pks seeing criminal status.
			if ( target.Kills >= 5 || (target.Body.IsMonster && IsSummoned( target as BaseCreature ) && !(target is BaseFamiliar) && !(target is Golem)) || (target is BaseCreature && (((BaseCreature)target).AlwaysMurderer || ((BaseCreature)target).IsAnimatedDead)) )
			{
				if ( source == target && source.Player && source.Criminal )
					return Notoriety.Criminal;
				else if ( source == target && source is PlayerMobile && ((PlayerMobile)source).Attackable )
					return Notoriety.CanBeAttacked;
				else
					return Notoriety.Murderer;
			}

			if ( target.Criminal )
				return Notoriety.Criminal;
			else if ( target is PlayerMobile && ((PlayerMobile)target).Attackable )
				return Notoriety.CanBeAttacked;

			Guild sourceGuild = GetGuildFor( source.Guild as Guild, source );
			Guild targetGuild = GetGuildFor( target.Guild as Guild, target );

			if ( sourceGuild != null && targetGuild != null )
			{
				if ( sourceGuild == targetGuild || sourceGuild.IsAlly( targetGuild ) )
					return Notoriety.Ally;
				else if ( sourceGuild.IsEnemy( targetGuild ) )
					return Notoriety.Enemy;
			}

			Faction srcFaction = Faction.Find( source, true, true );
			Faction trgFaction = Faction.Find( target, true, true );
            bool srcRegionIsNoFactions = (source.Region is CustomRegion && ((CustomRegion)source.Region).NoFactionEffects);

            if (srcFaction != null && trgFaction != null && srcFaction != trgFaction && Faction.IsFactionMap(source.Map) && !srcRegionIsNoFactions)
				return Notoriety.Enemy;

			if ( SkillHandlers.Stealing.ClassicMode && target is PlayerMobile && ((PlayerMobile)target).PermaFlags.Contains( source ) )
				return Notoriety.CanBeAttacked;

			if ( target is BaseCreature && ((BaseCreature)target).AlwaysAttackable )
				return Notoriety.CanBeAttacked;

			if ( CheckHouseFlag( source, target, target.Location, target.Map ) )
				return Notoriety.CanBeAttacked;

			if ( !(target is BaseCreature && ((BaseCreature)target).InitialInnocent) ) //Untamed npcs that are not humans can be attacked
			{
				if ( !target.Body.IsHuman && !target.Body.IsGhost && !IsPet( target as BaseCreature ) && !Server.Spells.Necromancy.TransformationSpell.UnderTransformation( target ) )
					return Notoriety.CanBeAttacked;
			}

			if ( CheckAggressor( source.Aggressors, target ) )
				return Notoriety.CanBeAttacked;

			if ( CheckAggressed( source.Aggressed, target ) )
				return Notoriety.CanBeAttacked;

            // XLX added.
            if( source is PlayerMobile && target is PlayerMobile )
            {
                int noto = CheckCowardice(((PlayerMobile)source).Cowards, target);
                if (noto != Notoriety.Invulnerable)
                   return noto;
            }
            // XLX End.

			if ( target is BaseCreature ) //When in guard mode, the guarded person can attack the pet
			{
				BaseCreature bc = (BaseCreature)target;

				if ( bc.Controlled && bc.ControlOrder == OrderType.Guard && bc.ControlTarget == source )
					return Notoriety.CanBeAttacked;
			}

			if ( source is BaseCreature ) //Master's Aggressor can be attacked by the pet
			{
				BaseCreature bc = (BaseCreature)source;

				Mobile master = bc.GetMaster();
				if( master != null && CheckAggressor( master.Aggressors, target ))
					return Notoriety.CanBeAttacked;
            }

            //Al: Master's Aggressed can be attacked if the master attacked non criminal
            if (source is BaseCreature)
            {
                BaseCreature bc = (BaseCreature)source;

                Mobile master = bc.GetMaster();
                if (master != null && CheckAggressed(master.Aggressed, target))
                    return Notoriety.CanBeAttacked;
            }

            //Al: Master's cowards can be attacked by the pet
            if (source is BaseCreature)
            {
                BaseCreature bc = (BaseCreature)source;

                Mobile master = bc.GetMaster();

                if (master != null)
                {
                    int noto = CheckCowardice(((PlayerMobile)master).Cowards, target);
                    if (noto != Notoriety.Invulnerable)
                        return noto;
                }
            }

			return Notoriety.Innocent;
		}

		public static bool CheckHouseFlag( Mobile from, Mobile m, Point3D p, Map map )
		{
			BaseHouse house = BaseHouse.FindHouseAt( p, map, 16 );

			if ( house == null || house.Public || !house.IsFriend( from ) )
				return false;

			if ( m != null && house.IsFriend( m ) )
				return false;

			BaseCreature c = m as BaseCreature;

			if ( c != null && !c.Deleted && c.Controlled && c.ControlMaster != null )
				return !house.IsFriend( c.ControlMaster );

			return true;
		}

		public static bool IsPet( BaseCreature c )
		{
			return ( c != null && c.Controlled );
		}

		public static bool IsSummoned( BaseCreature c )
		{
			return ( c != null && /*c.Controlled &&*/ c.Summoned );
		}

		public static bool CheckAggressor( ArrayList list, Mobile target )
		{
			for ( int i = 0; i < list.Count; ++i )
				if ( ((AggressorInfo)list[i]).Attacker == target )
					return true;

			return false;
		}

		public static bool CheckAggressed( ArrayList list, Mobile target )
		{
			for ( int i = 0; i < list.Count; ++i )
			{
				AggressorInfo info = (AggressorInfo)list[i];

				if ( !info.CriminalAggression && info.Defender == target )
					return true;
			}

			return false;
		}
	}
}