using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class pvpbag : Bag
    {
        private static int[] m_Shapes = new int[] { 0xE76, 0xE76, 0xE76, 0xE76 };

        [Constructable]
        public pvpbag() : this(1)
        {
        }

        [Constructable]
        public pvpbag(int amount) : base()
        {
            ItemID = m_Shapes[Utility.Random(m_Shapes.Length)];
            for (int i = 0; i < 12; i++)
                DropItem(new TotalRefreshPotion());
            for (int i = 0; i < 5; i++)
                DropItem(new GreaterAgilityPotion());
            for (int i = 0; i < 12; i++)
                DropItem(new GreaterStrengthPotion());
            for (int i = 0; i < 22; i++)
                DropItem(new GreaterHealPotion()); ;
            for (int i = 0; i < 18; i++)
                DropItem(new GreaterCurePotion());
            for (int i = 0; i < 15; i++)
                DropItem(new GreaterExplosionPotion());
            DropItem(new BagOfReagents(125));
            for (int i = 0; i < 5; i++)
                    {
                        TrapableContainer cont = new Pouch();
                        cont.TrapPower = 1;
                        cont.TrapType = TrapType.MagicTrap;
                        DropItem(cont);
                    }
            DropItem(new Bandage(100));
            //DropItem( new WarHammer() );
            //DropItem( new Kryss() );
        }

        public pvpbag(Serial serial)
            : base(serial)
        {
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