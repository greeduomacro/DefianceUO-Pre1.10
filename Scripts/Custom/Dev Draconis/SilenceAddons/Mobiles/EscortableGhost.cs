using System;
using Server;
using Server.Items;
using EDI = Server.Mobiles.EscortDestinationInfo;

namespace Server.Mobiles
{
	public class EscortableGhost : BaseEscortable
	{
		private DateTime m_NextSpeechTime;

		[Constructable]
		public EscortableGhost():base()
		{
			m_NextSpeechTime = DateTime.Now + TimeSpan.FromMinutes(Utility.RandomMinMax(1, 2));
		}

		private static int GetRandomHue()
		{
			switch (Utility.Random(5))
			{
				default:
				case 0: return Utility.RandomBlueHue();
				case 1: return Utility.RandomGreenHue();
				case 2: return Utility.RandomRedHue();
				case 3: return Utility.RandomYellowHue();
				case 4: return Utility.RandomNeutralHue();
			}
		}

		public override void InitBody()
		{
			switch (Utility.Random(2))
			{
				case 0: Name = "a wandering spirit"; break;
				case 1: Name = "a lost soul"; break;
			}
			Body = 970;
			Hue = 22222;
			Blessed = true;

			SetStr(20);
			SetDex(20);
			SetInt(20);

			SetHits(50);

			SetDamage(1, 10);
		}

		public override void InitOutfit()
		{
			HoodedShroudOfShadows robe = new HoodedShroudOfShadows();
			robe.Hue = 22222;
			robe.Name = "";
			robe.Movable = false;
			robe.LootType = LootType.Blessed;
			AddItem(robe);
		}

		public override void OnThink()
		{
			if (DateTime.Now >= m_NextSpeechTime && ActiveSpeed != 0.05)
			{
				m_NextSpeechTime = DateTime.Now + TimeSpan.FromMinutes(Utility.RandomMinMax(2, 3));
				switch (Utility.Random(4))
				{
					case 0: Say("I want to leave this place and roam britianna"); break;
					case 1: Say("Im lost...."); break;
					case 2: Say("I wish to return to my home city"); break;
					case 3: Say("I need to leave this place and go..."); break;
				}
			}
			base.OnThink();
		}

		public override bool SayDestinationTo(Mobile m)
		{
			EDI dest = GetDestination();

			if (dest == null || !m.Alive)
				return false;

			Mobile escorter = GetEscorter();

			if (escorter == null)
			{
				Say("I wish to return to {0}, please kind mortal will you take me there?", (dest.Name == "Ocllo" && m.Map == Map.Trammel) ? "Haven" : dest.Name);
				return true;
			}
			else if (escorter == m)
			{
				Say("Thank you mortal, I shall reward you when we reach {0}.", (dest.Name == "Ocllo" && m.Map == Map.Trammel) ? "Haven" : dest.Name);
				return true;
			}

			return false;
		}

		public override bool AcceptEscorter(Mobile m)
		{
			EDI dest = GetDestination();

			if (dest == null)
				return false;

			Mobile escorter = GetEscorter();

			if (escorter != null || !m.Alive)
				return false;

            BaseEscortable escortable = (BaseEscortable)EscortTable[m];

			if (escortable != null && !escortable.Deleted && escortable.GetEscorter() == m)
			{
				Say("I see you are already guiding a fellow spirit.");
				return false;
			}
			else if (m is PlayerMobile && (((PlayerMobile)m).LastEscortTime + EscortDelay) >= DateTime.Now)
			{
				int minutes = (int)Math.Ceiling(((((PlayerMobile)m).LastEscortTime + EscortDelay) - DateTime.Now).TotalMinutes);

				Say("You must rest {0} minute{1} before can you guide me.", minutes, minutes == 1 ? "" : "s");
				return false;
			}
			else if (SetControlMaster(m))
			{
				LastSeenEscorter = DateTime.Now;

				if (m is PlayerMobile)
					((PlayerMobile)m).LastEscortTime = DateTime.Now;

				Say("Thank you mortal, I shall reward you when we reach {0}.", (dest.Name == "Ocllo" && m.Map == Map.Trammel) ? "Haven" : dest.Name);
				EscortTable[m] = this;
				StartFollow();
				return true;
			}

			return false;
		}

		public override Mobile GetEscorter()
		{
			if (!Controlled)
				return null;

			Mobile master = ControlMaster;

			if (master == null)
				return null;

			if (master.Deleted || master.Map != this.Map || !master.InRange(Location, 30) || !master.Alive)
			{
				StopFollow();

				TimeSpan lastSeenDelay = DateTime.Now - LastSeenEscorter;

				if (lastSeenDelay >= TimeSpan.FromMinutes(2.0))
				{
					master.SendMessage("You have lost the restless soul you were guiding");
					Say("Oh no I have lost the mortal guiding me...I will roam this world forever...");

					SetControlMaster(null);
					EscortTable.Remove(master);

					Timer.DelayCall(TimeSpan.FromSeconds(5.0), new TimerCallback(Delete));
					return null;
				}
				else
				{
					ControlOrder = OrderType.Stay;
					return master;
				}
			}

			if (ControlOrder != OrderType.Follow)
				StartFollow(master);

			LastSeenEscorter = DateTime.Now;
			return master;
		}

		public override bool CheckAtDestination()
		{
			EDI dest = GetDestination();

			if (dest == null)
				return false;

			Mobile escorter = GetEscorter();

			if (escorter == null)
				return false;

			if (dest.Contains(Location))
			{
				Say("You have brought me home, I thank thee mortal and I hope this token of my appreciation helps you, now farewell!", escorter.Name);

				// not going anywhere
				Destination = null;
				DestinationString = null;

				escorter.AddToBackpack(new SpiritGem());

				StopFollow();
				SetControlMaster(null);
				EscortTable.Remove(escorter);
				BeginDelete();

				Misc.Titles.AwardFame(escorter, 10, true);

				bool gainedPath = false;

				PlayerMobile pm = escorter as PlayerMobile;

				if (pm != null)
				{
					if (pm.CompassionGains > 0 && DateTime.Now > pm.NextCompassionDay)
					{
						pm.NextCompassionDay = DateTime.MinValue;
						pm.CompassionGains = 0;
					}

					if (pm.CompassionGains >= 5) // have already gained 5 points in one day, can gain no more
					{
						pm.SendLocalizedMessage(1053004); // You must wait about a day before you can gain in compassion again.
					}
					else if (VirtueHelper.Award(pm, VirtueName.Compassion, 1, ref gainedPath))
					{
						if (gainedPath)
							pm.SendLocalizedMessage(1053005); // You have achieved a path in compassion!
						else
							pm.SendLocalizedMessage(1053002); // You have gained in compassion.

						pm.NextCompassionDay = DateTime.Now + TimeSpan.FromDays(1.0); // in one day CompassionGains gets reset to 0
						++pm.CompassionGains;
					}
					else
					{
						pm.SendLocalizedMessage(1053003); // You have achieved the highest path of compassion and can no longer gain any further.
					}
				}

				return true;
			}

			return false;
		}

		public EscortableGhost(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}