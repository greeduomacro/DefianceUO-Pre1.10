using Server;
using System;
using Server.Items;
using Server.Spells;
using Server.Mobiles;
using Server.Ladder;
using System.Text;

namespace Server.Regions
{
	public class CustomRegion : GuardedRegion
	{
		RegionControl m_Controller;

		public CustomRegion( RegionControl m, Map map ) : base( "", "Custom Region", map, typeof( WarriorGuard ) )
		{
			LoadFromXml = false;

			m_Controller = m;
		}

		public RegionControl Controller{ get{ return m_Controller; } set{ m_Controller = value; } }

		public override bool IsDisabled()
		{
			if( base.Disabled != !m_Controller.GetFlag( RegionFlag.IsGuarded ) )
			{
				m_Controller.IsGuarded = !Disabled;
			}

			return Disabled;
		}

		public override bool AllowBenificial( Mobile from, Mobile target )
		{
			if (IsGameRegion)
			{
				CTFTeam ft = CTFGame.FindTeamFor(from);
				if (ft == null)
					return false;
				CTFTeam tt = CTFGame.FindTeamFor(target);
				if (tt == null)
					return false;
				return ft == tt && ft.Game.Running;
			}

			if( ( !m_Controller.AllowBenifitPlayer && target is PlayerMobile ) || ( !m_Controller.AllowBenifitNPC && target is BaseCreature ))
			{
				from.SendMessage( "You cannot perform benificial acts on your target." );
				return false;
			}

			// ADDED FOR LADDER Remember to look in Allow harmful method for more laddder

			// We cannot do anything to people outside the arena.
			if (m_Controller is ArenaControl && target.Region != null && target.Region != this)
			{
				//from.SendMessage("You cannot perform benificial acts on your people outside the arena.");
				return false;
			}
			// We cannot do anything to people outside the arena.
			if (m_Controller is LadderAreaControl && target.Region != null && target.Region is CustomRegion && ((CustomRegion)target.Region).Controller != null && ((CustomRegion)target.Region).Controller is ArenaControl)
			{
				//from.SendMessage("You cannot perform benificial acts on people in the arena");
				return false;
			}


			// END ADDED FOR LADDER

			return base.AllowBenificial( from, target );
		}

		public override bool AllowHarmful( Mobile from, Mobile target )
		{
			if (IsGameRegion)
			{
				CTFTeam ft = CTFGame.FindTeamFor(from);
				if (ft == null)
					return false;
				CTFTeam tt = CTFGame.FindTeamFor(target);
				if (tt == null)
					return false;
				return ft != tt && ft.Game == tt.Game && ft.Game.Running;
			}

			if( ( !m_Controller.AllowHarmPlayer && target is PlayerMobile ) || ( !m_Controller.AllowHarmNPC && target is BaseCreature ))
			{
				from.SendMessage( "You cannot perform harmful acts on your target." );
				return false;
			}

			// ADDED FOR LADDER

			// We cannot do anything to people outside the arena.
			if (m_Controller is ArenaControl && target.Region != null && target.Region != this)
			{
				//from.SendMessage("You cannot perform harmful acts  on your people outside the arena.");
				return false;
			}
			// We cannot do anything to people outside the arena.
			if (m_Controller is LadderAreaControl && target.Region != null && target.Region is CustomRegion && ((CustomRegion)target.Region).Controller != null && ((CustomRegion)target.Region).Controller is ArenaControl)
			{
				//from.SendMessage("You cannot perform harmful acts on people in the arena");
				return false;
			}
			// END ADDED FOR LADDER

			return base.AllowHarmful( from, target );
		}

		public override bool AllowHousing( Mobile from, Point3D p )
		{
			return m_Controller.AllowHousing;
		}

		public override bool AllowSpawn()
		{
			return m_Controller.AllowSpawn;
		}

		public override bool CanUseStuckMenu( Mobile m )
		{
			if ( ! m_Controller.CanUseStuckMenu )
				m.SendMessage( "You cannot use the Stuck menu here." );
			return m_Controller.CanUseStuckMenu;
		}

		public override bool OnDamage( Mobile m, ref int Damage )
		{
			if ( !m_Controller.CanBeDamaged )
			{
				m.SendMessage( "You cannot be damaged here." );
			}

			return m_Controller.CanBeDamaged;
		}
		public override bool OnResurrect( Mobile m )
		{
			if ( ! m_Controller.CanRessurect && m.AccessLevel == AccessLevel.Player)
				m.SendMessage( "You cannot ressurect here." );
			return m_Controller.CanRessurect;
		}

		public override bool OnBeginSpellCast( Mobile from, ISpell s )
		{
			if ( from.AccessLevel == AccessLevel.Player )
			{
				bool restricted = m_Controller.IsRestrictedSpell( s );
				if ( restricted )
				{
					from.SendMessage( "You cannot cast that spell here." );
					return false;
				}

				//if ( s is EtherealSpell && !CanMountEthereal ) Grr, EthereealSpell is private :<
				if ( ! m_Controller.CanMountEthereal && ((Spell)s).Info.Name == "Ethereal Mount" ) //Hafta check with a name compare of the string to see if ethy
				{
					from.SendMessage( "You cannot mount your ethereal here." );
					return false;
				}
			}

			//Console.WriteLine( m_Controller.GetRegistryNumber( s ) );

			//return base.OnBeginSpellCast( from, s );
			return true;	//Let users customize spells, not rely on weather it's guarded or not.
		}

		public override bool OnDecay( Item item )
		{
			return m_Controller.ItemDecay;
		}

		public override bool OnHeal( Mobile m, ref int Heal )
		{
			if ( !m_Controller.CanHeal )
			{
				m.SendMessage( "You cannot be healed here." );
			}

			return m_Controller.CanHeal;
		}

		public override bool OnSkillUse( Mobile m, int skill )
		{
			bool restricted = m_Controller.IsRestrictedSkill( skill );
			if ( restricted && m.AccessLevel == AccessLevel.Player )
			{
				m.SendMessage( "You cannot use that skill here." );
				return false;
			}

			return base.OnSkillUse( m, skill );
		}

		public override void OnExit( Mobile m )
		{
			if ( m_Controller.ShowExitMessage )
				m.SendMessage("You have left {0}", this.Name );

			//**** Added for ladder
			if (m_Controller is ArenaControl && !m_Controller.Deleted)
			{
				((ArenaControl)m_Controller).Exited(m);
			}
			base.OnExit( m );

		}

		public override void OnEnter( Mobile m )
		{
			if ( m_Controller.ShowEnterMessage )
				m.SendMessage("You have entered {0}", this.Name );

			base.OnEnter( m );
		}



		public override bool OnMoveInto( Mobile m, Direction d, Point3D newLocation, Point3D oldLocation )
		{
			if( m_Controller.CannotEnter && ! this.Contains( oldLocation ) && m.AccessLevel < AccessLevel.GameMaster )
			{
				m.SendMessage( "You cannot enter this area." );
				return false;
			}

			return true;
		}

		public override TimeSpan GetLogoutDelay( Mobile m )
		{
			if( m.AccessLevel == AccessLevel.Player )
				return m_Controller.PlayerLogoutDelay;

			return base.GetLogoutDelay( m );
		}



		public override bool OnDoubleClick( Mobile m, object o )
		{
			if( o is BasePotion && !m_Controller.CanUsePotions )
			{
				m.SendMessage( "You cannot drink potions here." );
				return false;
			}

			if( o is Corpse )
			{
				Corpse c = (Corpse)o;

				bool canLoot;

				if( c.Owner == m )
					canLoot = !m_Controller.CannotLootOwnCorpse;
				else if ( c.Owner is PlayerMobile )
					canLoot =  m_Controller.CanLootPlayerCorpse;
				else
					canLoot =  m_Controller.CanLootNPCCorpse;

				if( !canLoot )
					m.SendMessage( "You cannot loot that corpse here." );

				if ( m.AccessLevel >= AccessLevel.GameMaster && !canLoot )
				{
					m.SendMessage( "This is unlootable but you are able to open that with your Godly powers." );
					return true;
				}

				return canLoot;
			}


			return base.OnDoubleClick( m, o );
		}

		public override bool OnSingleClick(Mobile from, object o)
		{
			if ( (!(o is Mobile)) || !IsGameRegion )
				return base.OnSingleClick(from, o);

			Mobile m = (Mobile)o;
			CTFTeam team = CTFGame.FindTeamFor(m);
			if (team != null)
			{
				string msg;
				Item[] items = null;

				if (m.Backpack != null)
					items = m.Backpack.FindItemsByType(typeof(CTFFlag));

				if (items == null || items.Length == 0)
				{
					msg = String.Format("(Team: {0})", team.Name);
				}
				else
				{
					StringBuilder sb = new StringBuilder("(Team: ");
					sb.Append(team.Name);
					sb.Append(" -- Flag");
					if (items.Length > 1)
						sb.Append("s");
					sb.Append(": ");

					for (int j = 0; j < items.Length; j++)
					{
						CTFFlag flag = (CTFFlag)items[j];

						if (flag != null && flag.Team != null)
						{
							if (j > 0)
								sb.Append(", ");

							sb.Append(flag.Team.Name);
						}
					}

					sb.Append(")");
					msg = sb.ToString();
				}
				m.PrivateOverheadMessage(Network.MessageType.Label, team.Hue, true, msg, from.NetState);
			}

			return true;
		}

		public override void AlterLightLevel( Mobile m, ref int global, ref int personal )
		{
			if( m_Controller.LightLevel >= 0 )
				global = m_Controller.LightLevel;
			else
				base.AlterLightLevel( m, ref global, ref personal );
		}

        public bool NoMurderCounts
        {
            get { return m_Controller.NoMurderCounts; }
        }

        public bool NoFactionEffects
        {
            get { return m_Controller.NoFactionEffects; }
        }

        public bool NoPvPPoints
        {
            get { return m_Controller.NoPvPPoints; }
        }

        public bool CannotEnter
        {
            get { return m_Controller.CannotEnter; }
        }

        public bool NoFameKarma
        {
            get { return m_Controller.NoFameKarma; }
        }

        public bool CannotTakeRewards
        {
            get { return m_Controller.CannotTakeRewards; }
        }

        public bool CannotTrade
        {
            get { return m_Controller.CannotTrade; }
        }

        public bool AlwaysGrey
        {
            get { return m_Controller.AlwaysGrey; }
        }

        public bool CannotStun
        {
            get { return m_Controller.IsRestrictedSkill((int) SkillName.Anatomy); }
        }

        public bool CannotDisarm
        {
            get { return m_Controller.IsRestrictedSkill((int) SkillName.ArmsLore); }
        }

        public bool NoPoisonSkillEffects
        {
            get { return m_Controller.IsRestrictedSkill((int)SkillName.Poisoning); }
        }

		public bool DeleteCorpsesOnDeath
		{
			get { return m_Controller.DeleteCorpsesOnDeath; }
		}

		public bool IsGameRegion
		{
			get { return m_Controller.IsGameRegion; }
		}

        public Point3D LogoutMoveLocation
        {
            get { return m_Controller.LogoutMoveLocation; }
        }
        public Map LogoutMoveMap
        {
            get { return m_Controller.LogoutMoveMap; }
        }
        public bool CastWithoutReagents
        {
            get { return m_Controller.CastWithoutReagents; }
        }
    }
}