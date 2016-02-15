/**
*	Ladder system by Morten Legarth (c)taunted.dk ( legarth@taunted.dk )
*	Version: v0.10 -  26-02-2005
*
*	This system has been written for use at the Blitzkrieg frees-shard
*	http://blitzkrieg.dorijan.de . Unauthorized reproduction or redistribution
*	is prohibited.
*
*							ArenaControl.cs
*						-------------------------
*
*	File Description:	The Arena Controller is a modified version of
*						the Region Controller. Tailored for use in the
*						ladder system. It controls the arena rules.
*
*
*/

using System;
using System.Collections;
using Server.Items;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Ladder
{

    public class ArenaControl : RegionControl
    {
        private Point3D m_StartLoc1;
        private Point3D m_StartLoc2;
        private Point3D m_OutLoc;
        private Point3D m_GateLoc;
        private Map m_OutMap;
        private bool m_InUse;
        private DateTime m_CleanupTimeoutTime;
        private ArenaCleanUpTimer m_Timer;
        private ConfirmationMoongate m_Gate;
        private LadderAreaControl m_LAC;

        [Constructable]
        public ArenaControl() : base()
        {
            this.Name = "Arena Controller";
            this.ItemID = 5394;
			this.Priority = CustomRegionPriority.HighestPriority;
		}

        public override void OnDoubleClick(Mobile m)
        {
            if (m.AccessLevel >= AccessLevel.GameMaster)
            {
                if (!m_InUse)
                {
                    if (m_LAC != null && m_LAC.Arenas.Contains(this))
                    {
                        m.SendMessage("This Arena has been removed from the active arena table while editing. Remember to press UPDATE when finished.");
                        m_LAC.Arenas.Remove(this);
                    }

                    m.CloseGump(typeof(ArenaControlGump));
                    m.SendGump(new ArenaControlGump(this));

                    m.CloseGump(typeof(RemoveAreaGump));
                    m.SendGump(new RemoveAreaGump(this));
                }
                else
                    m.SendMessage("You cannot edit this arena while it is in use.");
            }
        }

        public void Exited(Mobile m)
        {
			if (m.Alive)
			{
				//m.Hits = MaxHits;
				m.Heal(100);
				m.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist);
				m.PlaySound(0x1F2);
            }
            //Al: Fix for getting out of the Arena flagged.
            PlayerMobile pm = m as PlayerMobile;
            if (pm != null) pm.ClearAggression();
			if (MyRegion != null && MyRegion.Players.Count == 0 && m_InUse)
            {
                Clean();
            }
        }

        public void CleanUp()
        {
            CanBeDamaged = false; // no reskilling plz
            m_Gate = new ConfirmationMoongate(m_OutLoc, m_OutMap);

            m_Gate.GumpWidth = 420;
            m_Gate.GumpHeight = 280;
            m_Gate.TitleNumber = 1062108;
            m_Gate.TitleColor = 0x7800;
            m_Gate.MessageColor = 0x7F00;
            m_Gate.MessageString = "If you leave the Arena now, you wont be able to return and loot. When the arena is purged, all items remaing on player corpses will be returned to the owners backpack.";

            m_Gate.Dispellable = false;
            m_Gate.Movable = false;
            m_Gate.Hue = 1168;
            m_Gate.Name = "Exit Gate";
            m_Gate.MoveToWorld(m_GateLoc, this.Map);


            m_CleanupTimeoutTime = DateTime.Now + TimeSpan.FromMinutes(1.0);
            m_Timer = new ArenaCleanUpTimer(TimeSpan.FromMinutes(1.0), this);
            m_Timer.Start();
        }

        public void Clean()
        {
            if (this.Deleted || Map == null)
                return;
            if (m_Timer != null)
                m_Timer.Stop();

            ArrayList toDelete = new ArrayList();
            ArrayList toRelocate = new ArrayList();

            foreach (Rectangle2D rect in this.Coords)
            {
                IPooledEnumerable eable = this.Map.GetObjectsInBounds(rect);
                foreach (object o in eable)
                {
                    if (o != null && o is Item && ((Item)o).Movable == true || o is Corpse || o is Moongate)
                    {
                        toDelete.Add(o);
                        if (o is Corpse && ((Corpse)o).Owner != null)
                        {

                            Corpse c = (Corpse)o;
                            if (c.Owner.Alive && c.Owner.Backpack != null)
                            {
                                Item[] items = c.FindItemsByType(typeof(Item), false);
                                for (int i = 0; i < items.Length; i++)
                                {
                                    if (items[i].Layer != Layer.Hair && items[i].Layer != Layer.FacialHair)
                                        c.Owner.AddToBackpack(items[i]);
                                }
                                c.Owner.SendMessage("The remaning loot from the corpse of you last duel was added to your backpack.");
                            }
                            else if (!c.Owner.Alive && c.Owner.BankBox != null)
                            {
                                Item[] items = c.FindItemsByType(typeof(Item), false);
                                for (int i = 0; i < items.Length; i++)
                                {
                                    if (items[i].Layer != Layer.Hair && items[i].Layer != Layer.FacialHair)
                                        c.Owner.BankBox.DropItem(items[i]);
                                }
                                c.Owner.SendMessage("The remaning loot from the corpse of you last duel was added to your Bank Box.");
                            }
                        }
                    }
                    else if (o is Mobile)
                        toRelocate.Add(o);
                }
                eable.Free();
            }

            for (int i = 0; i < toDelete.Count; i++)
            {
                ((Item)toDelete[i]).Delete();
            }

			this.InUse = false;

			foreach (Mobile m in toRelocate)
            {
                m.Map = this.OutMap;
                m.Location = this.OutLoc;
				//m.Hits = MaxHits;
				m.Heal(100);
				m.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist);
				m.PlaySound(0x1F2);
			}

        }

        public ArenaControl(Serial serial) : base( serial )
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
            writer.Write(m_StartLoc1);
            writer.Write(m_StartLoc2);
            writer.Write(m_OutLoc);
            writer.Write(m_GateLoc);
            writer.Write(m_OutMap);
            writer.Write(m_InUse);
            writer.Write(m_Gate);
            writer.Write(m_LAC);

            TimeSpan span = m_CleanupTimeoutTime - DateTime.Now;
            writer.Write(span);

        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_StartLoc1 = reader.ReadPoint3D();
            m_StartLoc2 = reader.ReadPoint3D();
            m_OutLoc = reader.ReadPoint3D();
            m_GateLoc = reader.ReadPoint3D();
            m_OutMap = reader.ReadMap();
            m_InUse = reader.ReadBool();
            m_Gate = (ConfirmationMoongate)reader.ReadItem();
            m_LAC = (LadderAreaControl)reader.ReadItem();

            TimeSpan span = reader.ReadTimeSpan();
            m_CleanupTimeoutTime = DateTime.Now + span;
            if (span > TimeSpan.Zero)
            {
                m_Timer = new ArenaCleanUpTimer(span, this);
                m_Timer.Start();
            }
        }


        public Point3D StartLoc1
        {
            get { return m_StartLoc1; }
            set { m_StartLoc1 = value; }
        }
        public Point3D StartLoc2
        {
            get { return m_StartLoc2; }
            set { m_StartLoc2 = value; }
        }

        public Point3D GateLoc
        {
            get { return m_GateLoc; }
            set { m_GateLoc = value; }
        }
        public Point3D OutLoc
        {
            get { return m_OutLoc; }
            set { m_OutLoc = value; }
        }

        public Map OutMap
        {
            get { return m_OutMap; }
            set { m_OutMap = value; }
        }

        public bool InUse
        {
            get { return m_InUse; }
            set { m_InUse = value; }
        }

        public Moongate Gate
        {
            get { return m_Gate; }
        }

        public LadderAreaControl LAC
        {
            get { return m_LAC; }
            set { m_LAC = value; }
        }

        public override void OnDelete()
        {
            if (m_LAC != null && m_LAC.Arenas.Contains(this))
                m_LAC.Arenas.Remove(this);

            base.OnDelete();
        }


    }

    public class ArenaCleanUpTimer : Timer
    {
        private ArenaControl m_ArenaControl;

        public ArenaCleanUpTimer(TimeSpan delay, ArenaControl ac) : base(delay)
        {
            this.m_ArenaControl = ac;
        }
        protected override void OnTick()
        {
            m_ArenaControl.Clean();
        }

    }
}