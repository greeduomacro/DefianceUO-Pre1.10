using System;
using Server;

namespace Server.Items
{
    public class BattleStandard : Item
    {
        [Constructable]
        public BattleStandard () : this(1056)
        {
        }

        [Constructable]
        public BattleStandard ( int itemID ) : base( itemID )
        {
            Movable = true;
            Weight = 10;
            Name = "a gruesome battle standard";
        }

        public BattleStandard(Serial serial) : base(serial)
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int) 0 );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );
            int version = reader.ReadInt();
        }
    }
}