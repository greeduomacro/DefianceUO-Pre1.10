using System;
using Server.Mobiles;
using Xanthos.Evo;
using Server.Network;

namespace Server.Misc
{
	public class StamSystem
	{
		// When false, the old dfi stam handling is used
		public const bool active = false;

		// only active for PVPers?
		// (requires active = true)
		public const bool pvpersonly = true;

		// a player is not considered as a pvp participant, if his last pvp action was "pvpduration" seconds ago.
		public const int PVPduration = 120;

		// only for developing/debugging. do not use the debug = true in release!
		public const bool debug = false;

		// how many steps for 1 stamine decrease by foot?
		public const int stamUnitFoot = 12;

		// how many steps for 1 stamina decrease on a lichsteed?
		public const int stamUnitLichSteed = 2;

		// how many steps for 1 stamina decrease by basemounts (except lich steeds)?
		public const int stamUnitMount = 2;

		// what base/max stam should a ethereal mount have?
		public const int etherealstam = 90;

		// Every how many seconds should an ethereal mount regenerate stamina points?
		// ethyregendelay = 0 means no regeneration
		public const int ethyregendelay = 1;
		public const int ethyregenrate = 3;

		// Every how many seconds should a  mount (not lich steed, not ethy) regenerate stamina points?
		// mountregendelay = 0 means normal passive regeneration (from the original stam handling)
		public const int mountregendelay = 1;
		public const int mountregenrate = 3;

		// Every how many seconds should a lich steed regenerate stamina points?
		// steedregendelay = 0 means normal passive regeneration (from the original stam handling)
		public const int steedregendelay = 1;
		public const int steedregenrate = 3;

		// "all follow me" increases stam of a mount by a random range of minimal
		public const int followStamBonusMin = 15;
		// and maximal
		public const int followStamBonusMax = 25;
		// points.

		// how many stam points should get lich steeds when they were feeded?
		// 0 = no extra bonus; orginal feed bonus.
		public const int LichSteedFeedBonus = 50;

		// extra stam loss when unmounted and hits < how many percent ?
		// 0 = no extra loss with low hits
		public const int lowhitsloss = 15;

		// extra stam loss when mounted and hits < how many percent ?
		// 0 = no extra loss with low hits
		public const int lowhitslossOnMount = 15;

		public enum MountKind
		{
			normalmount, lichsteed, ethereal
		}

		#region todo
		// TODO
		// o may nightmares exclude from all follow me
		// o ingame configuration
		#endregion

		#region description
		/*
         * Description:
         *
         * That stamina system has to be activated by the "active" const to "true"
         * When not "active is set to "false", the original dfi stamina handling is
         * active.
         * When activated, this system only workes for running (not walking!) players.
         * Staff can travel without any restictions.
         *
         * PVPersOnly Option:
         * This option depends on "active" set to "true".
         * When "pvpersonly" is set to "true", the stamsystem is only active for players
         * that are currently participating in PVP fights.
         * A player is not considered as a pvp participant, if his last pvp action was
         * "pvpduration" seconds ago. As PVP action counts harming other players or get
         * harmed by a player.
         *
         * Without mount:
         * The players mount loses one stamina point by running "stamUnitFoot" steps.
         * Usual passive stamina regeneration when "mountregendelay" is set to "0".
		 * When "mountregendelay" is > 0, the mount regenerates "mountregenrate" every
		 * "mountregendelay" seconds.
         * When the player is below "lowhitsloss" percent hits, his stamina will decrease
         * doubled faster. This can be deactivated by setting "lowhitsloss" to "0".
         *
         * Mounted on a ethreal mount:
         * The ethereal mount has a base stamina of "etherealstam".
         * It loses  stamina point by running "stamUnitMount" steps and
         * it regenerates to full stamina by remounting it.
         * mountregendelay
         * The ethereal will gain "ethyregenrate" stam point every "ethyregendelay" seconds.
         *
         * Mounted on a BaseMount:
         * It loses one stamina point by running "stamUnitMount" steps (same as ethereals).
         * Usual passive stamina regeneration.
         * When telling the mount "all follow me" (when the mount is beside the rider), the mount
         * gets back from "followStamBonusMin" points to "followStamBonusMax" points of stamina (random).
         *
         * Mounted on a EvoHiryu aka Lich Steed:
         * It loses one stamina point by running "stamUnitLichSteed" steps.
         * This mount got an extra option, because the lich steed has an exorbitant high stamina.
         * So it should lose stamina faser. Doubled fast seems to be a good choice here.
         * An extra stamina regeneration can be set up with "LichSteedFeedBonus".
         * Set it to the value of stamina points that a lichsteed should gain when it will be feeded
         * while it is not in a fight. If the steed gained stamina because of feeding, the owner will
         * get the message "Your pet feels regenerated.".
         * This extra stam regeneration possiblity was implemented, because the steed (should) lose
         * stamina faster but has an extra regeneration method.
         * The "all follow me" regeneration could not be used here, to prevent abusing it in pvm fights
         * or lichsteed training.
		 * Same passive stamina regen controls with "steedregendelay" and "stedregenrate" as ethereals
		 * and normal mounts.
         *
		 * When the player is below "lowhitslossOnMount" percent hits, his mounts stamina will decrease
         * doubled faster. That counts for all types of mounts.
		 * This can be deactivated by setting "lowhitslossOnMount" to "0".
		 *
         * Attention:
         * Never set "debug" to "true" in a release!
         * The debug mode is only for developing/debugging issues.
         *
         * Minkio
         *
		 */

		#endregion

		public static void Initialize()
		{
			if (active)
			{
				EventSink.Movement += new MovementEventHandler(StamSystem_Movement);
				Console.WriteLine("Stamina System activated");
				if (debug)
					Console.WriteLine("WARNING: STAMSYSTEM DEBUG MODE ENABLED!");
			}
		}

		public static void StamSystem_Movement(MovementEventArgs e)
		{
			Mobile from = e.Mobile;
			PlayerMobile pm = from as PlayerMobile;

			if (pm == null || (pm.LastPVP == DateTime.MinValue && pvpersonly) ||
				!from.Alive || from.AccessLevel >= AccessLevel.GameMaster)
			{
				#region debug
				if (debug)
					from.SendMessage("MinValue");
				#endregion

				return;
			}

			if (active_for_player(pm))
			{
				Item m_item = from.FindItemOnLayer(Layer.Mount);
				MountItem m_mountitem;
				int m_mountstam = 0;
				bool m_running = false;

				// Check if the palyer runs or walkes and write it to m_running
				if ((e.Direction & Direction.Running) != 0)
					m_running = true;

				pm.StepsTaken++;

				// prepare mount handling
				m_mountitem = m_item as MountItem;
				if (m_mountitem != null && m_mountitem.Mount != null)
				{
					m_mountstam = ((Mobile)m_mountitem.Mount).Stam;
					if (m_mountitem.Mount is Xanthos.Evo.EvoHiryu)
						mount_stam_regen(mountregenrate, pm, MountKind.lichsteed);
					else
						mount_stam_regen(mountregenrate, pm, MountKind.normalmount);

					#region debug
					if (StamSystem.debug)
					{
						if (m_mountitem.Mount is EvoHiryu)
						{
							from.SendMessage("Mounted: LichSteed    | Stam: " + m_mountstam);
						}
						else
						{
							from.SendMessage("Mounted: BaseMount    | Stam: " + m_mountstam);
						}
					}
					#endregion

				}

				// prepare ethereal handling
				else if (m_item is EtherealMount)
				{
					m_mountstam = pm.EtherealStam;
					mount_stam_regen(ethyregenrate, pm, MountKind.ethereal);

					#region debug
					if (StamSystem.debug)
						from.SendMessage("Mounted: EthrealMount | Stam: " + m_mountstam);
					#endregion
				}

				// Handling on mount
				if (m_item != null && from.Mounted && from is PlayerMobile && m_running)
				{
					// when horse has no stam left, block the rider
					if (m_mountstam == 0)
					{
						from.SendMessage("Your mount is too fatigued to move.");
						e.Blocked = true;
						return;
					}

					// extra stam loss with low hits On Mount
					if (StamSystem.lowhitslossOnMount != 0 && ((from.Hits * 100) / Math.Max(from.HitsMax, 1)) < StamSystem.lowhitslossOnMount)
					{
						int stamUnitOnMounted = (m_item is EtherealMount) ? stamUnitLichSteed : stamUnitMount;
						if ((stamUnitOnMounted % 2) != 0)
							stamUnitOnMounted++;

						if ((pm.StepsTaken % (stamUnitOnMounted / 2)) == 0)
						{
							if (m_item is EtherealMount)
								pm.EtherealStam--;
							else
								((Mobile)m_mountitem.Mount).Stam--;
						}
					}


					// Normal stam decreasement to the mount when its ridden
					if (m_item is EtherealMount)
					{
						if ((pm.StepsTaken % StamSystem.stamUnitMount) == 0)
							pm.EtherealStam--;
					}
					else if (m_mountitem != null && m_mountitem.Mount is Mobile &&
							((pm.StepsTaken % (m_mountitem.Mount is EvoHiryu ? StamSystem.stamUnitLichSteed : StamSystem.stamUnitMount)) == 0))
					{
						((Mobile)m_mountitem.Mount).Stam--;
					}


					// warning message to the rider, when the mount has 30 & 15 stam left
                                        if (m_mountstam == 30)
					{
						from.LocalOverheadMessage(MessageType.Regular, 0x49, false, "Your mount is tiring from running.");
					}
					else if (m_mountstam == 15)
					{
						from.LocalOverheadMessage(MessageType.Regular, 0x49, false, "Your mount is extremly fatigued.");
					}
				}

				// Handling w/o mount
				else if (from is PlayerMobile && m_running)
				{
					// when player has 0 stam, block him
					if (from.Stam == 0)
					{
						from.SendLocalizedMessage(500110); // You are too fatigued to move.
						e.Blocked = true;
						return;
					}

					// extra stam loss with low hits
					if (StamSystem.lowhitsloss != 0 && ((from.Hits * 100) / Math.Max(from.HitsMax, 1)) < StamSystem.lowhitsloss)
					{
						int evenUnitFoot = StamSystem.stamUnitFoot;
						if ((evenUnitFoot % 2) != 0)
							evenUnitFoot++;

						if ((pm.StepsTaken % (evenUnitFoot / 2)) == 0)
							--from.Stam;
					}

					// Normal decreasement by foot
					if ((pm.StepsTaken % StamSystem.stamUnitFoot) == 0)
						--from.Stam;
				}

				#region debug
				if (StamSystem.debug)
				{
					//Console.WriteLine("WARNING: Stamina System Debug enabled.");

					if ((e.Direction & Direction.Running) != 0)
						from.SendMessage("running");
					else
						from.SendMessage("walking");

					from.SendMessage("Player Stam: " + from.Stam);

					from.SendMessage("---");
				}
				#endregion // debug
			}
			else
			{
				pm.LastPVP = DateTime.MinValue;
			}
		}

		public static bool active_for_player(PlayerMobile pm)
		{
			PlayerMobile from = pm as PlayerMobile;
			if (from == null)
				return false;

			TimeSpan secSinceLastPVP = DateTime.Now - from.LastPVP;
			return (active && (!pvpersonly || secSinceLastPVP.TotalSeconds < PVPduration));
		}

		public static void mount_stam_regen(int gainmultiplicator, PlayerMobile rider, MountKind mounttype)
		{
			PlayerMobile pm = rider as PlayerMobile;
			if (pm == null)
				return;

			if (pm.LastMountStamGain == DateTime.MinValue)
			{
				rider.LastMountStamGain = DateTime.Now;
				return;
			}

			TimeSpan sinceLastStep = (DateTime.Now - pm.LastMountStamGain);
			int regendelay;

			if (mounttype == MountKind.ethereal)
				regendelay = StamSystem.ethyregendelay;

			else if (mounttype == MountKind.lichsteed)
				regendelay = StamSystem.steedregendelay;

			else
				regendelay = StamSystem.mountregendelay;

			int stamBonus = ((int)sinceLastStep.TotalSeconds / regendelay) * gainmultiplicator;

			if (stamBonus > 0)
			{
				if (mounttype == MountKind.ethereal)
				{
					if (pm.EtherealStam + stamBonus > StamSystem.etherealstam)
						pm.EtherealStam = StamSystem.etherealstam;
					else if (stamBonus > 0)
						pm.EtherealStam = pm.EtherealStam + stamBonus;
				}
				else if (pm.Mount != null)
				{
					Mobile mount = pm.Mount as Mobile;
					if (mount == null)
						return;

					if (mount.Stam + stamBonus > mount.StamMax)
						mount.Stam = mount.StamMax;
					else
						mount.Stam = mount.Stam + stamBonus;
				}

				pm.LastMountStamGain = DateTime.Now;
			}
		}
	}
}