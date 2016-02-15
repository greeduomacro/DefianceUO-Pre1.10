using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
    public enum PromotionalTokenType { SoulStone, SoulStoneFragment }

    public class PromotionalToken : Item
    {
        public override int LabelNumber { get { return 1070997; } } // A promotional token
        private PromotionalItem m_PromotionalItem;
        private PromotionalTokenType m_BoundItem;
        private static bool m_NeedAccountDonation;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool NeedAccountDonation
        {
            get { return m_NeedAccountDonation; }
            set { m_NeedAccountDonation = value; this.InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public PromotionalTokenType BoundItem
        {
            get { return m_BoundItem; }
            set
            {
                m_BoundItem = value;
                m_PromotionalItem = m_Table[(int)value]; this.InvalidateProperties();
            }
        }

        #region PromotionalItem
        public class PromotionalItem
        {
            public Type m_Type;
            public int m_iClilocName;
            public int m_iClaimCliloc;

            public PromotionalItem(Type type, int clilocName, int claimCliloc)
            {
                m_Type = type;
                m_iClilocName = clilocName;
                m_iClaimCliloc = claimCliloc;
            }
        }
        #endregion

        private static PromotionalItem[] m_Table = new PromotionalItem[]
			{
				new PromotionalItem( typeof(SoulStone), 1030899, 1070743 ),
				new PromotionalItem( typeof(SoulStoneFragment), 1071000, 1070976 ),
			};

        [Constructable]
        public PromotionalToken()
            : this(PromotionalTokenType.SoulStoneFragment, false)
        {
        }

        public PromotionalToken(PromotionalTokenType promotionalTokenType, bool needAccountDonation)
            : base(0x2AAA)
        {
            m_NeedAccountDonation = needAccountDonation;
            m_BoundItem = promotionalTokenType;
            m_PromotionalItem = m_Table[(int)promotionalTokenType];
            LootType = LootType.Blessed;
            Weight = 5.0;
            Light = LightType.Empty;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(1070998, string.Format("#{0}", m_PromotionalItem.m_iClilocName)); // Use this to redeem<br>Your ~1_PROMO~

            if (m_NeedAccountDonation)
                list.Add(1070722, "(Donation Account Needed)");
        }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);

            LabelTo(from, 1049644, string.Format("#{0}", m_PromotionalItem.m_iClilocName));
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
            else if (from is PlayerMobile && !((PlayerMobile)from).HasDonated && m_NeedAccountDonation)
            {
                from.SendMessage("Only players with Donation status on their accounts can use this.");
            }
            else if (from.BankBox == null)
            {
                from.SendMessage("Error! Could not find Bankbox.");
            }
            else if (from.Account == null)
            {
                from.SendMessage("Error! Could not find Account.");
            }
            else
            {
                Item createdItem = (Item)Activator.CreateInstance(m_PromotionalItem.m_Type);
                // *** Add item changes here ***

                if (createdItem is SoulStone)
                    ((SoulStone)createdItem).Account = from.Account.ToString();

                // *** ***
                from.BankBox.AddItem(createdItem);
                from.SendLocalizedMessage(m_PromotionalItem.m_iClaimCliloc);
                this.Delete();
            }
        }

        public PromotionalToken(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version

            writer.Write((bool)m_NeedAccountDonation);
            writer.Write((int)m_BoundItem);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();

            m_NeedAccountDonation = reader.ReadBool();
            m_BoundItem = (PromotionalTokenType)reader.ReadInt();
            m_PromotionalItem = m_Table[(int)m_BoundItem];
        }
    }
}