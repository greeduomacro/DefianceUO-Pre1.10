using System;
using Server;

namespace Server.Items
{
	[Flipable(0x2AC3, 0x2AC0)]
	public class FountainOfLife : Item
    {
        [Constructable]
		public FountainOfLife() : this(0x2AC3)
        {
        }

        [Constructable]
        public FountainOfLife( int itemID ) : base( itemID )
        {
            Movable = true;
            Weight = 6;
            Name = "Fountain of Life";
            //Hue = 1175;
        }

        public FountainOfLife( Serial serial ) : base( serial )
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