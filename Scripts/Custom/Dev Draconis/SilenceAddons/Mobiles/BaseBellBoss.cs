using System;
using System.Collections;
using Server;
using Server.Items;
//using Server.Engines.SilenceAddon;
using Server.EventPrizeSystem;

namespace Server.Mobiles
{
    [CorpseName("a glowing corpse")]
    public abstract class BaseBellBoss : BaseCreature
    {
        public BaseBellBoss(AIType aiType) : this(aiType, FightMode.Closest)
        {
        }

        public BaseBellBoss(AIType aiType, FightMode mode) : base(aiType, mode, 18, 1, 0.05, 0.2)
        {
        }

        public BaseBellBoss(Serial serial) : base(serial)
        {
        }

        public override bool BardImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override bool CanDestroyObstacles { get { return true; } }
        public override bool AutoDispel { get { return true; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 3);
        }

        public override void OnDeath(Container c)
        {
            if (Utility.Random(5) < 1)
                switch (Utility.Random(5))
                {
                    case 0: c.DropItem(new SpecialHairDye()); break;
                    case 1: c.DropItem(new SpecialBeardDye()); break;
                    case 2: c.DropItem(new ClothingBlessDeed()); break;
                    case 3: c.DropItem(new NameChangeDeed()); break;
                    case 4: c.DropItem(new WeddingDeed()); break;
                }
            base.OnDeath(c);
        }

        public void Token() //same as dragchamp cept fewer tokens and no golden
        {
            ArrayList toGive = new ArrayList();
            ArrayList rights = BaseCreature.GetLootingRights(this.DamageEntries, this.HitsMax);

            for (int i = rights.Count - 1; i >= 0; --i)
            {
                DamageStore ds = (DamageStore)rights[i];

                if (ds.m_HasRight)
                    toGive.Add(ds.m_Mobile);
            }

            if (toGive.Count == 0)
                return;

            for (int i = 0; i < toGive.Count; ++i)
            {
                int rand = Utility.Random(toGive.Count);
                object hold = toGive[i];
                toGive[i] = toGive[rand];
                toGive[rand] = hold;
            }

            for (int i = 0; i < 10; ++i)
            {
                Mobile m = (Mobile)toGive[i % toGive.Count];
                if (Utility.Random(4) < 1)
                {
                    m.AddToBackpack(new SilverPrizeToken());
                    m.SendMessage("You have received a silver token!");
                }
                else if (Utility.Random(2) < 1)
                {
                    m.AddToBackpack(new BronzePrizeToken());
                    m.SendMessage("You have received a bronze token!");
                }
            }
        }

        public override bool OnBeforeDeath()  //same as dragchamp
        {
            if (!NoKillAwards)
            {
                Token();

                Map map = this.Map;

                if (map != null)
                {
                    for (int x = -12; x <= 8; ++x)
                    {
                        for (int y = -12; y <= 8; ++y)
                        {
                            double dist = Math.Sqrt(x * x + y * y);

                            if (dist <= 12)
                                new GoodiesTimer(map, X + x, Y + y).Start();
                        }
                    }
                }
            }
            return base.OnBeforeDeath();
        }

        private class GoodiesTimer : Timer  //same as dragchamp
        {
            private Map m_Map;
            private int m_X, m_Y;

            public GoodiesTimer(Map map, int x, int y)
                : base(TimeSpan.FromSeconds(Utility.RandomDouble() * 10.0))
            {
                m_Map = map;
                m_X = x;
                m_Y = y;
            }

            protected override void OnTick()
            {
                int z = m_Map.GetAverageZ(m_X, m_Y);
                bool canFit = m_Map.CanFit(m_X, m_Y, z, 6, false, false);

                for (int i = -3; !canFit && i <= 3; ++i)
                {
                    canFit = m_Map.CanFit(m_X, m_Y, z + i, 6, false, false);

                    if (canFit)
                        z += i;
                }

                if (!canFit)
                    return;

                Gold g = new Gold(200, 500);

                g.MoveToWorld(new Point3D(m_X, m_Y, z), m_Map);

                if (0.5 >= Utility.RandomDouble())
                {
                    switch (Utility.Random(3))
                    {
                        case 0: // Fire column
                            {
                                Effects.SendLocationParticles(EffectItem.Create(g.Location, g.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 5052);
                                Effects.PlaySound(g, g.Map, 0x208);

                                break;
                            }
                        case 1: // Explosion
                            {
                                Effects.SendLocationParticles(EffectItem.Create(g.Location, g.Map, EffectItem.DefaultDuration), 0x36BD, 20, 10, 5044);
                                Effects.PlaySound(g, g.Map, 0x307);

                                break;
                            }
                        case 2: // Ball of fire
                            {
                                Effects.SendLocationParticles(EffectItem.Create(g.Location, g.Map, EffectItem.DefaultDuration), 0x36FE, 10, 10, 5052);

                                break;
                            }
                    }
                }
            }
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