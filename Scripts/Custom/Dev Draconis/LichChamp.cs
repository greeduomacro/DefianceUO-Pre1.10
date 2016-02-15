using System;
using Server;
using Server.Items;
using Server.Spells;
using System.Collections;
using Server.Spells.Seventh;
using Server.Spells.Fifth;
using Server.EventPrizeSystem;

namespace Server.Mobiles
{
    public class LichChamp : BaseBoss
    {
		IdolType m_Type;

        [Constructable]
        public LichChamp()
            : base(AIType.AI_Mage, FightMode.Closest)
        {
            Name = "Lord Zwiebus";
            Body = 0x4F;
            Hue = 1175;

            SetStr(650, 1000);
            SetDex(150, 200);
            SetInt(1000, 1200);
            SetHits(13500);
            SetDamage(23, 27);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Cold, 60);
            SetDamageType(ResistanceType.Energy, 40);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 40, 50);
            SetSkill(SkillName.Meditation, 350.0, 350.0);
            SetSkill(SkillName.EvalInt, 190.1, 210.0);
            SetSkill(SkillName.Magery, 175.1, 190.0);
            SetSkill(SkillName.MagicResist, 115, 120.0);
            SetSkill(SkillName.Tactics, 100.0, 100.0);
            SetSkill(SkillName.Wrestling, 135.0, 155.0);

            Fame = 100000;
            Karma = -225000;

            PackGold(4000, 6000);
            PackScroll(3, 8);
            PackScroll(3, 8);
            PackMagicItems(1, 5, 0.80, 0.75);
            PackMagicItems(3, 5, 0.60, 0.45);
            PackSlayer(1);

            VirtualArmor = 50;

			m_Type = IdolType.Undeath;


			World.Broadcast(0x35, true, "The Undying Lord of Death has crossed the border between the living world and his own. Force him back or suffer eternal pain!");
        }
        public override bool CanRummageCorpses { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override int TreasureMapLevel { get { return 5; } }
        public override bool AlwaysMurderer { get { return true; } }
        public override bool BardImmune { get { return true; } }
		public override bool CanDestroyObstacles { get { return true; } }

		public override bool DoDistributeTokens { get { return true; } }
		public override bool DoSpawnGoldOnDeath { get { return true; } }
		public override int DoPolymorphOnGaveMelee { get { return 51; } }
		public override int DoPolymorphHue { get { return 1175; } }
		public override bool DoLeechLife { get { return true; } }
		public override bool DoSpawnEvil{ get { return true; } }
		public override bool DoEarthquake { get { return true; } }
		public override bool DoSkillLoss{ get { return true; } }

       public override void OnDeath(Container c)
        {
            int cnt = 1;
            if ( Utility.Random( 4 ) < 1 ) cnt++;
            if ( Utility.Random( 5 ) < 1 ) cnt++;

            for (int i = 0; i < cnt; ++i)
            {
                switch (Utility.Random(5))
                {
                    case 0: c.DropItem(new SpecialHairDye()); break;
                    case 1: c.DropItem(new SpecialBeardDye()); break;
                    case 2: c.DropItem(new ClothingBlessDeed()); break;
                    case 3: c.DropItem(new NameChangeDeed()); break;
			case 4: c.DropItem(new WeddingDeed()); break;
                }
			}

            if (Utility.Random(5) < 1)
                switch (Utility.Random(5))
                {
                    case 0: c.DropItem(new LampPost1()); break;
                    case 1: c.DropItem(new LampPost2()); break;
                    case 2: c.DropItem(new LampPost3()); break;
                    case 3: c.DropItem(new CoveredChair()); break;
                    case 4: c.DropItem(new RuinedDrawers()); break;
                }

            if (Utility.Random(25) < 1)
                switch (Utility.Random(2))
                {
                    case 0: c.DropItem(new CursedClothingBlessDeed()); break;
                   case 1: c.DropItem(new HolyDeedofBlessing()); break;
                }

            if (Utility.Random(10) < 1)
                switch (Utility.Random(2))
                {
                    case 0: c.DropItem(new ChampionPants()); break;
                    case 1: c.DropItem(new ChampionKilt()); break;
                }

            if (Utility.Random(4) < 1)
                switch (Utility.Random(2))
                {
                    case 0: c.DropItem(new ChampionRing()); break;
                    case 1: c.DropItem(new ChampionNecklace()); break;
                }

			if (Utility.Random(2) < 1)
				c.DropItem(new Idol(m_Type));


            base.OnDeath(c);
        }

		private DateTime m_NextFreezeTime;

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextFreezeTime)
            {
                Mobile combatant = this.Combatant;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 15))
                {
                    m_NextFreezeTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 40));

					DoFreeze(combatant, "Tremble before me mortal!");
                }
            }

            base.OnThink();
        }

        public void DoFreeze(Mobile combatant, string message)
        {
            this.Say(true, message);
            m_Table[combatant] = Timer.DelayCall(TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(1.0), new TimerStateCallback(DoEffect), new object[] { combatant, 0 });
        }

        private static Hashtable m_Table = new Hashtable();

        public static bool UnderEffect(Mobile m)
        {
            return m_Table.Contains(m);
        }

        public static void StopEffect(Mobile m, bool message)
        {
            Timer t = (Timer)m_Table[m];

            if (t != null)
            {
                if (message)
                    m.SendMessage("Your weakened state breaks though the fear!");

                t.Stop();
                m_Table.Remove(m);
            }
        }

        public void DoEffect(object state)
        {
            if (!Alive) return;
            object[] states = (object[])state;

            Mobile m = (Mobile)states[0];
            int count = (int)states[1];

            if (!m.Alive)
            {
                StopEffect(m, false);
            }
            else
            {

                if (m.Hits < 50)
                {
                    StopEffect(m, true);
                }
                else
                {
                    if ((count % 4) == 0)
                    {
                        m.LocalOverheadMessage(Network.MessageType.Emote, m.SpeechHue, true, "* You are burned alive! *");
                        m.NonlocalOverheadMessage(Network.MessageType.Emote, m.SpeechHue, true, String.Format("* {0} is burned alive! *", m.Name));
                    }

                    m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                    m.PlaySound(0x54);
                    m.Freeze(TimeSpan.FromSeconds(2.0));
                    m.SendMessage("You are terrified to the very core of your being!");
					m.RevealingAction();
					m.SendLocalizedMessage(500814); // You have been revealed!

                    AOS.Damage(m, this, Utility.RandomMinMax(15, 20) - (Core.AOS ? 0 : 10), 100, 0, 0, 0, 0);

                    states[1] = count + 1;

                    if (!m.Alive)
                        StopEffect(m, false);
                }
            }
        }

        public LichChamp(Serial serial)
            : base(serial)
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
        }
    }
}