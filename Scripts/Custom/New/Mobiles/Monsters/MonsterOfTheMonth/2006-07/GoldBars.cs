using System;
using Server;

namespace Server.Items
{
    public class GoldBars : Item
    {
        [Constructable]
        public GoldBars() : this(7146)
        {
        }

        [Constructable]
        public GoldBars( int itemID ) : base( itemID )
        {
            Movable = true;
            Weight = 1;
            Name = "golden bars";
        }

        public GoldBars( Serial serial ) : base( serial )
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