//Dev Al@05/21/2006
using System;

namespace Server.Items
{
	public class DamageableWall : Item
	{
        private int m_hitsmax = 10;
        private int m_hits = 10;
        private int m_originalitemid;
        private string m_originalname;
        private bool m_isdestroyed = false;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Hits
        {
            get { return m_hits; }
            set
            {
                if (value > 0)
                {
                    m_hits = value;
                    IsDestroyed = false;
                }
                else
                {
                    m_hits = 0;
                    IsDestroyed = true;
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int HitsMax
        {
            get { return m_hitsmax; }
            set
            {
                m_hitsmax = value >= 0 ? value : 0;
                Hits = m_hitsmax;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsDestroyed
        {
            get { return m_isdestroyed; }
            set
            {
                if (value == m_isdestroyed) return;
                m_isdestroyed = value;
                if (value)
                {
                    m_originalitemid = ItemID;
                    m_originalname = Name;
                    ItemID = 0x1ea7; //Worldgem bit
                    Name = "Damageable Wall [destroyed]";
                    Visible = false;
                    m_hits = 0;
                }
                else
                {
                    ItemID = m_originalitemid;
                    Name = m_originalname;
                    Visible = true;
                    m_hits = m_hitsmax;
                }
            }
        }

        [Constructable]
        public DamageableWall() : base(128)
        {
            Movable = false;
        }

        public DamageableWall(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
            if (from.AccessLevel>=AccessLevel.GameMaster) IsDestroyed = false;
        }

        public void OnDamage(Mobile caster, int damagevalue)
        {
            double percentage = ((double)m_hits) / ((double)m_hitsmax);
            string damagestring;
            if (percentage >= 0.95)
                damagestring = "in perfect condition.";
            else if (percentage >= 0.75)
                damagestring = "in good condition.";
            else if (percentage >= 0.5)
                damagestring = "slightly damaged.";
            else if (percentage >= 0.25)
                damagestring = "greatly damaged.";
            else
                damagestring = "about to collapse.";

            caster.SendMessage("You damage the structure. It is " + damagestring);
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3709, 9, 32, 5022);
            if (damagevalue >= Hits)
            {
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x36BD, 9, 32, 5022);
                Effects.PlaySound(Location, Map, 0x307);
                IsDestroyed = true;
            }
            else
            {
                Effects.PlaySound(Location, Map, 0x208);
                Hits -= damagevalue;
            }
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
            writer.Write((int)m_hitsmax);
            writer.Write((int)m_hits);
            writer.Write((int)m_originalitemid);
            writer.Write((string)m_originalname);
            writer.Write((bool)m_isdestroyed);
        }

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
            m_hitsmax = reader.ReadInt();
            m_hits = reader.ReadInt();
            m_originalitemid = reader.ReadInt();
            m_originalname = reader.ReadString();
            m_isdestroyed = reader.ReadBool();
		}
	}
}