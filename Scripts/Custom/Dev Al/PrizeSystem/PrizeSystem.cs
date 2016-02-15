using System;
using System.Collections;
using Server.Items;
using Server.Mobiles;
using System.Reflection;
using Server.Logging;

namespace Server.EventPrizeSystem
{
    public class PrizeSystem
    {
        private static ArrayList ms_prizedefinitions = new ArrayList();
        public static ArrayList PrizeDefinitions { get { return ms_prizedefinitions; } }

        public static void Initialize()
        {
            //Tickets
            ms_prizedefinitions.Add(new PrizeDefinition(typeof(SilverPrizeToken), 0xEF3, "Silver Token", typeof(BronzePrizeToken), 10, "10 bronze tokens"));
            ms_prizedefinitions.Add(new PrizeDefinition(typeof(GoldenPrizeToken), 0xEF3, "Golden Token", typeof(SilverPrizeToken), 10, "10 silver tokens"));
            ms_prizedefinitions.Add(new PrizeDefinition(typeof(BronzePrizeToken), 10, 0xEF3, "10 Bronze Tokens", typeof(SilverPrizeToken), 1, "1 silver token"));
            ms_prizedefinitions.Add(new PrizeDefinition(typeof(SilverPrizeToken), 10, 0xEF3, "10 Silver Tokens", typeof(GoldenPrizeToken), 1, "1 golden token"));

            //Small Prizes
            ms_prizedefinitions.Add(new PrizeDefinition(typeof(BagOfReagents), 100, 0xE76, "Bag Of 100 Reagents", typeof(BronzePrizeToken), 1, "1 bronze token"));
            ms_prizedefinitions.Add(new PrizeDefinition(typeof(DonationSkillBall), 1, 0xE73, "+1 Limited Skill Ball", typeof(BronzePrizeToken), 5, "5 bronze tokens"));
            ms_prizedefinitions.Add(new PrizeDefinition(typeof(NameChangeDeed), 0x14F0, "Name Change Deed", typeof(SilverPrizeToken), 3, "3 silver tokens"));
            ms_prizedefinitions.Add(new PrizeDefinition(typeof(SpecialHairDye), 0xE26, "Special Hair Dye", typeof(SilverPrizeToken), 3, "3 silver tokens"));
            ms_prizedefinitions.Add(new PrizeDefinition(typeof(SpecialBeardDye), 0xE26, "Special Beard Dye", typeof(SilverPrizeToken), 3, "3 silver tokens"));
            ms_prizedefinitions.Add(new PrizeDefinition(typeof(CarpetAddonDeed), 0x14F0, "Carpet Addon Deed", typeof(SilverPrizeToken), 5, "5 silver tokens"));

            //Medium Prizes
            ms_prizedefinitions.Add(new PrizeDefinition(typeof(CTFTicket), 0xE26, "CTF Ticket", typeof(GoldenPrizeToken), 2, "2 golden tokens"));
            ms_prizedefinitions.Add(new PrizeDefinition(typeof(TCTicket), 0xE26, "TC Ticket", typeof(GoldenPrizeToken), 2, "2 golden tokens"));
            ms_prizedefinitions.Add(new PrizeDefinition(typeof(CSTicket), 0xE26, "CS Ticket", typeof(GoldenPrizeToken), 2, "2 golden tokens"));
            ms_prizedefinitions.Add(new PrizeDefinition(typeof(KillDeed), 0x14F0, "No Murder Count Deed", typeof(GoldenPrizeToken), 3, "3 golden tokens"));
            ms_prizedefinitions.Add(new PrizeDefinition(typeof(LimitedSevenGMSkillBall), 0xE73, "Limited Seven GM Skillball", typeof(GoldenPrizeToken), 3, "3 golden tokens"));
            ms_prizedefinitions.Add(new PrizeDefinition(typeof(ArcheryButteDeed), 0x14F0, "Archery Butte Deed", typeof(GoldenPrizeToken), 5, "5 golden tokens"));

            //Large Prizes
            ms_prizedefinitions.Add(new PrizeDefinition(typeof(PersonalizationDeed), 0xE26, "Personalization Deed", typeof(GoldenPrizeToken), 20, "20 golden tokens"));
            ms_prizedefinitions.Add(new PrizeDefinition(typeof(ShroudTicket), 0xE26, "Colored Shroud Ticket", typeof(GoldenPrizeToken), 30, "30 golden tokens"));
            ms_prizedefinitions.Add(new PrizeDefinition(typeof(BloodHueTicket), 0xE26, "Blood Hue Ticket", typeof(GoldenPrizeToken), 40, "40 golden tokens"));
            ms_prizedefinitions.Add(new PrizeDefinition(typeof(FireHueTicket), 0xE26, "Fire Hue Ticket", typeof(GoldenPrizeToken), 40, "40 golden tokens"));
            ms_prizedefinitions.Add(new PrizeDefinition(typeof(DarkPurpleHueTicket), 0xE26, "Dark Purple Hue Ticket", typeof(GoldenPrizeToken), 40, "40 golden tokens"));
            ms_prizedefinitions.Add(new PrizeDefinition(typeof(LayeredSpellbookTicket), 0xE26, "Layered Spellbook Ticket", typeof(GoldenPrizeToken), 50, "50 golden tokens"));
        }

    }

    class PrizeDefinition
    {
        private Type m_type;
        private int m_itemParameter;
        private int m_itemID;
        //private int m_hue;
        private string m_name;

        private Type m_priceType;
        //private int m_priceItemID;
        private int m_priceAmount;
        private string m_priceString;

        public PrizeDefinition(Type type, int itemID, string name,
            Type priceType, int priceAmount, string priceString)
            : this(type, -1, itemID, name, priceType, priceAmount, priceString) { }

        public PrizeDefinition(Type type, int itemParameter, int itemID, string name,
        Type priceType, int priceAmount, string priceString) //itemParameter -1 = default constructor
        {
            m_type = type;
            m_itemParameter = itemParameter;
            m_itemID = itemID;
            //m_hue = hue;
            m_name = name;

            m_priceType = priceType;
            //m_priceItemID = priceItemID;
            m_priceAmount = priceAmount;
            m_priceString = priceString;
        }

        public int ItemID { get { return m_itemID; } }
        public string Name { get { return m_name; } }

        //public int PriceItemID { get { return m_priceItemID; } }
        public int PriceAmount { get { return m_priceAmount; } }
        public string PriceString { get { return m_priceString; } }

        public bool TryGiveOut(Mobile m)
        {
            PlayerMobile pm = m as PlayerMobile;
            if (pm != null && !pm.Deleted && pm.Backpack != null)
            {
                if (pm.Backpack.ConsumeTotal(m_priceType, m_priceAmount))
                {
                    Item newItem = null;
                    if (m_itemParameter == -1)
                    {
                        newItem = Activator.CreateInstance(m_type) as Item;
                    }
                    else
                    {
                        ConstructorInfo[] ctors = m_type.GetConstructors();
                        //Find 1-int-constructor (for amount/skillball bonus/etc.)
                        for (int i = 0; i < ctors.Length && newItem==null; ++i)
                        {
                            ConstructorInfo ctor = ctors[i];
                            ParameterInfo[] paramList = ctor.GetParameters();
                            if (paramList.Length == 1 && paramList[0].ParameterType.Name == "Int32")
                            {
                                object[] o = new object[1];
                                o[0] = m_itemParameter;
                                newItem = ctor.Invoke(o) as Item;
                            }
                        }
                    }
                    if (newItem != null)
                    {
                        if (!pm.AddToBackpack(newItem)) newItem.MoveToWorld(pm.Location);
                        pm.SendMessage("The item has been placed in your backpack.");
                        return true;
                    }
                    else
                    {
                        GeneralLogging.WriteLine("PrizeSystem", "Acc:{0} purchased {1} and was charged {2} but did not receive the item.", pm.Account.ToString(), m_name, m_priceString);
                    }
                }
                else
                {
                    pm.SendMessage("You need {0} to purchase this item.", this.PriceString);
                }
            }
            return false;
        }


    }
}