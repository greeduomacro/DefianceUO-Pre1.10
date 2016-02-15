using System;
using Server;
using Server.Misc;
using Server.Items;
using System.Collections;
using Server.Spells.Fifth;
using Server.Spells.Seventh;
//using Server.Engines.SilenceAddon;

namespace Server.Mobiles
{
    public class Alfirix : BaseBellBoss
    {
        static bool m_Active;

        [CommandProperty(AccessLevel.GameMaster)]
        public static bool Active
        {
            get { return m_Active; }
            set { m_Active = value; }
        }

        [Constructable]
        public Alfirix()
            : base(AIType.AI_Melee)
        {
            Name = "Alfirix the unicorn";
            Body = 122;
            BaseSoundID = 1212;
            Kills = 5;
            m_Active = true;

            SetStr(800);
            SetDex(350);
            SetInt(500);

            SetHits(15000);

            SetDamage(5, 35);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 35, 45);
            SetResistance(ResistanceType.Fire, 20, 40);
            SetResistance(ResistanceType.Cold, 10, 30);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.MagicResist, 1000.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 90.0);

            Fame = 10000;
            Karma = 10000;

            VirtualArmor = 30;
        }

        public override void OnDeath(Container c)
        {
            m_Active = false;

            if (Utility.Random(2) < 1)
                c.DropItem(new MysticKeySinglePart(4));

            if (Utility.Random(10) < 1)
                c.DropItem(new SaviourSash());
            base.OnDeath(c);
        }

        public override void OnDamagedBySpell(Mobile caster)
        {
            base.OnDamagedBySpell(caster);

            caster.Kill();
        }

        public override void OnThink() //taken from Tree.cs and changed to instant kill pets
        {
            ArrayList list = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(18))
            {
                if (m == this || !CanBeHarmful(m))
                    continue;

                if (m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != this.Team))
                    list.Add(m);
            }

            foreach (Mobile m in list)
            {
                DoHarmful(m);

                m.Kill();

            }
            base.OnThink();
        }


        public override void OnGotMeleeAttack(Mobile attacker) //taken from ElementalChamp.cs
        {
            base.OnGotMeleeAttack(attacker);

            if (0.25 >= Utility.RandomDouble())
                DrainStam();

            if (Utility.Random(4) < 1)
            {
                Map map = this.Map;

                if (map != null)
                {
                    for (int i = 0; i < 10; ++i)
                    {
                        int x = X + (Utility.RandomMinMax(-15, 15));
                        int y = Y + (Utility.RandomMinMax(-15, 15));
                        int z = Z;

                        if (!map.CanFit(x, y, z, 16, false, false))
                            continue;

                        Point3D from = attacker.Location;
                        Point3D to = new Point3D(x, y, z);

                        if (!InLOS(to))
                            continue;

                        attacker.Location = to;
                        attacker.ProcessDelta();
                        attacker.Combatant = null;
                        attacker.Freeze(TimeSpan.FromSeconds(10.0));

                        Effects.SendLocationParticles(EffectItem.Create(from, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
                        Effects.SendLocationParticles(EffectItem.Create(to, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);

                        Effects.PlaySound(to, map, 0x1FE);
                    }
                }
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender) // Taken from Questboss.cs and removed the m_ability thing
        {
            base.OnGaveMeleeAttack(defender);

            if (0.80 >= Utility.RandomDouble())
                Polymorph(defender);

            if (0.25 >= Utility.RandomDouble())
                DrainStam();
        }

        public void Polymorph(Mobile m) // Taken from evilvampire.cs and changed the values
        {
            if (!m.CanBeginAction(typeof(PolymorphSpell)) || !m.CanBeginAction(typeof(IncognitoSpell)) || m.IsBodyMod)
                return;

            IMount mount = m.Mount;

            if (mount != null)
                mount.Rider = null;

            if (m.Mounted)
                return;

            if (m.BeginAction(typeof(PolymorphSpell)))
            {
                Item disarm = m.FindItemOnLayer(Layer.OneHanded);

                if (disarm != null && disarm.Movable)
                    m.AddToBackpack(disarm);

                disarm = m.FindItemOnLayer(Layer.TwoHanded);

                if (disarm != null && disarm.Movable)
                    m.AddToBackpack(disarm);

                m.BodyMod = 17;
                m.HueMod = 0;
                new ExpirePolymorphTimer(m).Start();
            }
        }

        private class ExpirePolymorphTimer : Timer
        {
            private Mobile m_Owner;

            public ExpirePolymorphTimer(Mobile owner)
                : base(TimeSpan.FromMinutes(1.5))
            {
                m_Owner = owner;

                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (!m_Owner.CanBeginAction(typeof(PolymorphSpell)))
                {
                    m_Owner.BodyMod = 0;
                    m_Owner.HueMod = -1;
                    m_Owner.EndAction(typeof(PolymorphSpell));
                }
            }
        }


        public void DrainStam() // Taken from Questboss.cs and changed the values and removed the m_ability activation
        {
            ArrayList list = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(5))
            {
                if (m == this || !CanBeHarmful(m))
                    continue;

                if (m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != this.Team))
                    list.Add(m);
                else if (m.Player)
                    list.Add(m);
            }

            foreach (Mobile m in list)
            {

                m.FixedParticles(0x374A, 10, 15, 5013, 0x496, 0, EffectLayer.Waist);
                m.PlaySound(0x231);

                m.SendMessage("You feel more and more fatigued!");

                m.Stam -= 10;
            }
        }

        public Alfirix(Serial serial) : base(serial)
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
			//Explanation see GhostPast
			m_Active = true;
        }
    }
}