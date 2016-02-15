using System;
using System.Collections;
using Server;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Mobiles;

namespace Server.Items
{
    public class Atomic : Item
    {
        private Timer m_Timer;
        [Constructable]
        public Atomic() : base(0xF0D)
        {
            Movable = false;
            Hue = 1161;
            Name = "a bomb";

            m_Timer = new DelayTimer(this);
            m_Timer.Start();

        }
        private static bool LeveledExplosion = true; // Should explosion potions explode other nearby potions?
        private static bool InstantExplosion = false; // Should explosion potions explode on impact?
        private const int ExplosionRange = 10;     // How long is the blast radius?

        public Atomic(Serial serial) : base(serial)
        {

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Timer = new DelayTimer(this);
            m_Timer.Start();
        }


        public void Explode( bool direct, Point3D loc, Map map)
        {
            if (Deleted)
                return;

            Delete();

            if (map == null)
                return;

            Effects.PlaySound(loc, map, 0x207);
            for (int i = 0; i < 20; i++)
            {
                Point3D temp1 = new Point3D(loc.X, loc.Y, (loc.Z + i));
                Effects.SendLocationEffect(temp1, map, 0x3709, 60);
            }

            IPooledEnumerable eable = LeveledExplosion ? map.GetObjectsInRange(loc, ExplosionRange) : map.GetMobilesInRange(loc, ExplosionRange);
            ArrayList toExplode = new ArrayList();

            foreach (object o in eable)
            {
                if (o is Mobile)
                {
					if(o is ElementalChamp) { }
					else
	                    toExplode.Add(o);
                }

                else if (o is Atomic && o != this)
                {
                    toExplode.Add(o);
                }
            }

            eable.Free();

            for (int i = 0; i < toExplode.Count; ++i)
            {
                object o = toExplode[i];

                if (o is Mobile)
                {
                    Mobile m = (Mobile)o;

                    Spells.SpellHelper.Damage(TimeSpan.FromTicks(0), m, 40);
                }
                else if (o is Atomic)
                {
                    Atomic pot = (Atomic)o;

                    pot.Explode( false, pot.GetWorldLocation(), pot.Map);
                }
            }
            if (map != null)
            {
                for (int x = -8; x <= 8; ++x)
                {
                    for (int y = -8; y <= 8; ++y)
                    {
                        double dist = Math.Sqrt(x * x + y * y);

                        if (dist <= 8)
                        {
                            Explotion(loc, map, X + x, Y + y);
                        }
                    }
                }
            }

        }
        public void Explotion(Point3D location, Map m_Map, int x, int y)
        {
            Point3D loc = location;
            Map map = m_Map;
            int m_X = x;
            int m_Y = y;

            int z = map.GetAverageZ(m_X, m_Y);
            bool canFit = map.CanFit(m_X, m_Y, z, 6, false, false);

            for (int i = -3; !canFit && i <= 3; ++i)
            {
                canFit = map.CanFit(m_X, m_Y, z + i, 6, false, false);

                if (canFit)
                    z += i;
            }

            if (!canFit)
                return;

            Point3D temp1 = new Point3D(m_X, m_Y, z);
            Effects.SendLocationEffect(temp1, map, 0x3709, 60);
        }


        private class DelayTimer : Timer
        {
            private Atomic m_Potion;
            private int m_Index;

            public DelayTimer( Atomic potion) : base(TimeSpan.FromSeconds(0.75), TimeSpan.FromSeconds(1.0))
            {
                Priority = TimerPriority.FiftyMS;
                m_Potion = potion;
                m_Index = 3;
                m_Potion.Internalize();
            }

            protected override void OnTick()
            {
                if (m_Potion.Deleted)
                {
                    Stop();
                }
                else if (m_Index == 0)
                {
                    m_Potion.Explode( true, m_Potion.Location, m_Potion.Map);
                    Stop();
                }
                else
                {
                    m_Potion.PublicOverheadMessage(MessageType.Regular, 0x22, false, m_Index.ToString());
                    --m_Index;
                }
            }
        }
    }
}