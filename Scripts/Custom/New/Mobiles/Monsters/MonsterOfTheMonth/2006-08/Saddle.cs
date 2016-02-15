using System;
using Server;

namespace Server.Items
{
    public class Saddle : Item
    {
        [Constructable]
        public Saddle() : this(3896)
        {
        }

        [Constructable]
        public Saddle( int itemID ) : base( itemID )
        {
            Movable = true;
            Weight = 1;
            Name = "saddle";
        }

        public Saddle( Serial serial ) : base( serial )
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