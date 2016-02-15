using System;
using Server;

namespace Server.Items
{
	[Flipable(0x1169, 0x116A)]
	public class InscribedTombStone : Item
    {
        [Constructable]
		public InscribedTombStone() : this(0x1169)
        {
        }

        [Constructable]
        public InscribedTombStone( int itemID ) : base( itemID )
        {
            Movable = true;
            Weight = 6;
            Name = "a mystic tomb stone";
            //Hue = 1175;
        }

        public InscribedTombStone( Serial serial ) : base( serial )
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