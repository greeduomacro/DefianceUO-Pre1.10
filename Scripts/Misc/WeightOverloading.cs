using System;
using Server;
using Server.Mobiles;

namespace Server.Misc
{
	public enum DFAlgorithm
	{
		Standard,
		PainSpike
	}

	public class WeightOverloading
	{
		// setup for the old stamina system. that setting do nothing when the new StamSystem is activated.
		private const int m_TiredAfterSteps = 12; // how many steps you can take with full hits befoer losing one stamina on foot
		private const int m_HorseMoreSteps = 3; // how many times more steps a horse can take w/o losing stamine
		private const double m_ScaleWithHits = 0.5;  // 1.0 = it all depends on hits, 0.0 = It doesn#t depends on hits at all. 0.5 = half on hits/half global


		public static void Initialize()
		{
			EventSink.Movement += new MovementEventHandler(EventSink_Movement);
		}

		private static DFAlgorithm m_DFA;

		public static DFAlgorithm DFA
		{
			get{ return m_DFA; }
			set{ m_DFA = value; }
		}

		public static void FatigueOnDamage(Mobile m, int damage)
		{
			double fatigue = 0.0;

			switch (m_DFA)
			{
				case DFAlgorithm.Standard:
					{
						fatigue = (damage * (100.0 / m.Hits) * ((double)m.Stam / 100)) - 5.0;
						break;
					}
				case DFAlgorithm.PainSpike:
					{
						fatigue = (damage * ((100.0 / m.Hits) + ((50.0 + m.Stam) / 100) - 1.0)) - 5.0;
						break;
					}
			}

			if (fatigue > 0)
				m.Stam -= (int)fatigue;
		}

		public const int OverloadAllowance = 4; // We can be four stones overweight without getting fatigued

		public static int GetMaxWeight(Mobile m)
		{
			return 40 + (int)(3.5 * m.Str);
		}

		public static void EventSink_Movement(MovementEventArgs e)
		{
			Mobile from = e.Mobile;

			if (!from.Player || !from.Alive || from.AccessLevel >= AccessLevel.GameMaster)
				return;

			int maxWeight = GetMaxWeight(from) + OverloadAllowance;
			int overWeight = (Mobile.BodyWeight + from.TotalWeight) - maxWeight;

			if (overWeight > 0)
			{
				from.Stam -= GetStamLoss(from, overWeight, (e.Direction & Direction.Running) != 0);

				if (from.Stam == 0)
				{
					from.SendLocalizedMessage(500109); // You are too fatigued to move, because you are carrying too much weight!
					e.Blocked = true;
					return;
				}
			}

			// old stamina handling. activated when the new stamsystem (SytamSystem.cs) is deacticated.
			if (!StamSystem.active)
			{
				if (from is PlayerMobile)
				{
					int amt = (int)((m_ScaleWithHits + (m_ScaleWithHits * ((double)from.Hits / (double)from.HitsMax))) * (double)(from.Mounted ? m_TiredAfterSteps * m_HorseMoreSteps : m_TiredAfterSteps));
					PlayerMobile pm = (PlayerMobile)from;

					if ((++pm.StepsTaken % amt) == 0)
						--from.Stam;
				}
			}
		}

		public static int GetStamLoss(Mobile from, int overWeight, bool running)
		{
			int loss = 5 + (overWeight / 25);

			if (from.Mounted)
				loss /= 3;

			if (running)
				loss *= 2;

			return loss;
		}

		public static bool IsOverloaded(Mobile m)
		{
			if (!m.Player || !m.Alive || m.AccessLevel >= AccessLevel.GameMaster)
				return false;

			return ((Mobile.BodyWeight + m.TotalWeight) > (GetMaxWeight(m) + OverloadAllowance));
		}
	}
}