using System;
using System.Collections;
using Server.Items;

namespace Server.Items
{
    public /*static(1.1 S:)*/ class TemporaryItemSystem
    {
        private static ArrayList m_Enlisted = new ArrayList();
        private static DateTime m_Now = DateTime.Now;

        public static void Initialize()
        {
            new InternalTimer().Start();
        }

        public static void Verify(ITempItem item)
        {
            if (!m_Enlisted.Contains(item))
                m_Enlisted.Add(item);

            TimeSpan ts = item.RemovalTime - m_Now;
            int daysleft = ts.Days;

            if (daysleft < 1)
                Timer.DelayCall(TimeSpan.Zero, new TimerStateCallback(Remove_OnCallback), item);

            else
            {
                item.DaysLeft = daysleft;
                item.CreateCustomName();
            }
        }

        private static void Remove_OnCallback(object o)
        {
            Item item = o as Item;
            ITempItem temp = o as ITempItem;

            if (item != null && !item.Deleted)
            {
                item.Delete();
                if (temp != null)
                    m_Enlisted.Remove(temp);
            }
        }

        public static void VerifyAll()
        {
            m_Now = DateTime.Now;
            foreach (ITempItem iti in m_Enlisted)
                Verify(iti);
        }

        private class InternalTimer : Timer
        {
            public InternalTimer()
                : base(TimeSpan.FromHours(0.1), TimeSpan.FromHours(12.0))
            {
            }

            protected override void OnTick()
            {
                TemporaryItemSystem.VerifyAll();
            }
        }
    }

    public interface ITempItem
    {
        DateTime RemovalTime { get;}
        int DaysLeft { get; set;}
        void CreateCustomName();
    }
}