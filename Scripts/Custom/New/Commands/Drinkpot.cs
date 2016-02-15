using System;
using System.Collections;
using Server;
using System.IO;
using Server.Items;
using Server.Mobiles;

namespace Server.Scripts.Commands
{

	public class DrinkHeal
	{

		public static void Initialize()
		{
			Server.Commands.Register("Drinkheal", AccessLevel.Player, new CommandEventHandler(DrinkHeal_OnCommand));
		}



		[Usage("DrinkHeal")]
		[Description("Drinks a healing potion if any available.")]
		public static void DrinkHeal_OnCommand(CommandEventArgs e)
		{
			// Added to fix the missing check to region. Ugly fix but this script is so ugly anyways. :p
			if(e.Mobile.Region != null && !e.Mobile.Region.OnDoubleClick(e.Mobile, new HealPotion()))
				return;

			LesserHealPotion lh_potion = (LesserHealPotion)e.Mobile.Backpack.FindItemByType(typeof(LesserHealPotion));
			HealPotion mh_potion = (HealPotion)e.Mobile.Backpack.FindItemByType(typeof(HealPotion));
			GreaterHealPotion gh_potion = (GreaterHealPotion)e.Mobile.Backpack.FindItemByType(typeof(GreaterHealPotion));



			int lhp = e.Mobile.Backpack.GetAmount(typeof(LesserHealPotion));

			if (lhp != 0)
			{
				e.Mobile.SendMessage("Lesser heal potion found");
				lh_potion.OnDoubleClick(e.Mobile);
				Targeting.Target.Cancel(e.Mobile);
			}
			else
			{
				int mhp = e.Mobile.Backpack.GetAmount(typeof(HealPotion));

				if (mhp != 0)
				{
					e.Mobile.SendMessage("Heal potion found");
					mh_potion.OnDoubleClick(e.Mobile);
					Targeting.Target.Cancel(e.Mobile);
				}
				else
				{
					int ghp = e.Mobile.Backpack.GetAmount(typeof(GreaterHealPotion));

					if (ghp != 0)
					{

						e.Mobile.SendMessage("Greater heal potion found");
						gh_potion.OnDoubleClick(e.Mobile);
						Targeting.Target.Cancel(e.Mobile);


					}
					else
					{
						e.Mobile.SendMessage("Healing potion not found");
					}
				}
			}
		}
	}

	public class DrinkCure
	{

		public static void Initialize()
		{
			Server.Commands.Register("DrinkCure", AccessLevel.Player, new CommandEventHandler(DrinkCure_OnCommand));
		}

		[Usage("DrinkCure")]
		[Description("Drinks a cure potion if any available.")]
		public static void DrinkCure_OnCommand(CommandEventArgs e)
		{
            // Added to fix the missing check to region. Ugly fix but this script is so ugly anyways. :p
			if (e.Mobile.Region != null && !e.Mobile.Region.OnDoubleClick(e.Mobile, new CurePotion()))
				return;

            LesserCurePotion lc_potion = (LesserCurePotion)e.Mobile.Backpack.FindItemByType(typeof(LesserCurePotion));
			CurePotion mc_potion = (CurePotion)e.Mobile.Backpack.FindItemByType(typeof(CurePotion));
			GreaterCurePotion gc_potion = (GreaterCurePotion)e.Mobile.Backpack.FindItemByType(typeof(GreaterCurePotion));

			int lcp = e.Mobile.Backpack.GetAmount(typeof(LesserCurePotion));

			if (lcp != 0)
			{
                Targeting.Target.Cancel(e.Mobile);
                e.Mobile.SendMessage("Lesser cure potion found");
                new InternalTimer(e.Mobile, lc_potion).Start();
			}
			else
			{
				int mcp = e.Mobile.Backpack.GetAmount(typeof(CurePotion));

				if (mcp != 0)
				{
                    Targeting.Target.Cancel(e.Mobile);
                    e.Mobile.SendMessage("Cure potion found");
                    new InternalTimer(e.Mobile, mc_potion).Start();
				}
				else
				{
					int gcp = e.Mobile.Backpack.GetAmount(typeof(GreaterCurePotion));

					if (gcp != 0)
					{
                        Targeting.Target.Cancel(e.Mobile);
                        e.Mobile.SendMessage("Greater cure potion found");
                        new InternalTimer(e.Mobile, gc_potion).Start();
					}
					else
					{
						e.Mobile.SendMessage("Cure potion not found");
					}
				}
			}
		}
        //Al: Using timer to add a little delay to potions and make sure that the target is actually
        //    cancelled before the potion is drunken.
        private class InternalTimer : Timer
        {
            private Mobile m_From;
            private Item m_Potion;

            public InternalTimer(Mobile from, Item potion) : base(TimeSpan.FromSeconds(0.15))
            {
                m_From = from;
                m_Potion = potion;
                Priority = TimerPriority.FiftyMS;
            }

            protected override void OnTick()
            {
                if (m_Potion != null && !m_Potion.Deleted && m_Potion is BasePotion)
                {
                    m_Potion.OnDoubleClick(m_From);
                }
            }
        }
	}

	public class DrinkRefresh
	{

		public static void Initialize()
		{
			Server.Commands.Register("DrinkRefresh", AccessLevel.Player, new CommandEventHandler(DrinkRefresh_OnCommand));
		}


		[Usage("DrinkRefresh")]
		[Description("Drinks a refresh potion if any available.")]
		public static void DrinkRefresh_OnCommand(CommandEventArgs e)
		{
			// Added to fix the missing check to region. Ugly fix but this script is so ugly anyways. :p
			if (e.Mobile.Region != null && !e.Mobile.Region.OnDoubleClick(e.Mobile, new RefreshPotion()))
				return;

			RefreshPotion r_potion = (RefreshPotion)e.Mobile.Backpack.FindItemByType(typeof(RefreshPotion));
			TotalRefreshPotion tr_potion = (TotalRefreshPotion)e.Mobile.Backpack.FindItemByType(typeof(TotalRefreshPotion));

			int rp = e.Mobile.Backpack.GetAmount(typeof(RefreshPotion));

			if (rp != 0)
			{
				e.Mobile.SendMessage("Refresh potion found");
				r_potion.OnDoubleClick(e.Mobile);
				Targeting.Target.Cancel(e.Mobile);
			}
			else
			{
				int trp = e.Mobile.Backpack.GetAmount(typeof(TotalRefreshPotion));

				if (trp != 0)
				{
					e.Mobile.SendMessage("Total refresh potion found");
					tr_potion.OnDoubleClick(e.Mobile);
					Targeting.Target.Cancel(e.Mobile);
				}
				else
				{
					e.Mobile.SendMessage("Refresh potion not found");
				}
			}
		}
	}
	public class ThrowExplo
	{

		public static void Initialize()
		{
			Server.Commands.Register("ThrowExplo", AccessLevel.Player, new CommandEventHandler(ThrowExplosion_OnCommand));
		}

		[Usage("ThrowExplo")]
		[Description("Uses an explosion potion if any available.")]
		public static void ThrowExplosion_OnCommand(CommandEventArgs e)
		{
			// Added to fix the missing check to region. Ugly fix but this script is so ugly anyways. :p
			if (e.Mobile.Region != null && !e.Mobile.Region.OnDoubleClick(e.Mobile, new ExplosionPotion()))
				return;


			LesserExplosionPotion le_potion = (LesserExplosionPotion)e.Mobile.Backpack.FindItemByType(typeof(LesserExplosionPotion));
			ExplosionPotion me_potion = (ExplosionPotion)e.Mobile.Backpack.FindItemByType(typeof(ExplosionPotion));
			GreaterExplosionPotion ge_potion = (GreaterExplosionPotion)e.Mobile.Backpack.FindItemByType(typeof(GreaterExplosionPotion));

			int lep = e.Mobile.Backpack.GetAmount(typeof(LesserExplosionPotion));

			if (lep != 0)
			{
				e.Mobile.SendMessage("Lesser explosion potion found");
				le_potion.OnDoubleClick(e.Mobile);
			}
			else
			{
				int mep = e.Mobile.Backpack.GetAmount(typeof(ExplosionPotion));

				if (mep != 0)
				{
					e.Mobile.SendMessage("Explosion potion found");
					me_potion.OnDoubleClick(e.Mobile);
				}
				else
				{
					int gep = e.Mobile.Backpack.GetAmount(typeof(GreaterExplosionPotion));

					if (gep != 0)
					{
						e.Mobile.SendMessage("Greater explosion potion found");
						ge_potion.OnDoubleClick(e.Mobile);
					}
					else
					{
						e.Mobile.SendMessage("Explosion potion not found");
					}
				}
			}
		}
	}
}