using System;

namespace Server.EventPrizeSystem
{
	public class BronzePrizeToken : Item
	{
		[Constructable]
		public BronzePrizeToken() : this( 1 )
		{
		}

		[Constructable]
		public BronzePrizeToken( int amount ) : base( 0x1F5F )
		{
			Stackable = true;
			Weight = 1.0;
            Hue = 0x972;
            Name = "bronze reward token";
			Amount = amount;
		}

		public BronzePrizeToken( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
            ItemID = 0x1F5F; //Because the ItemID changed.
		}

        public override Item Dupe(int amount)
        {
            return base.Dupe(new BronzePrizeToken(amount), amount);
        }

    }

    public class SilverPrizeToken : Item
    {
        [Constructable]
        public SilverPrizeToken() : this(1) { }

        [Constructable]
        public SilverPrizeToken(int amount) : base(0x1F5F)
        {
            Stackable = true;
            Weight = 1.0;
            Hue = 0x961;
            Name = "silver reward token";
            Amount = amount;
        }

        public SilverPrizeToken(Serial serial) : base(serial)
        {
        }

        public override Item Dupe(int amount)
        {
            return base.Dupe(new SilverPrizeToken(amount), amount);
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
            ItemID = 0x1F5F; //Because the ItemID changed.
        }
    }



    public class GoldenPrizeToken : Item
    {
        [Constructable]
        public GoldenPrizeToken() : this(1) { }

        [Constructable]
        public GoldenPrizeToken(int amount) : base(0x1F5F)
        {
            Stackable = true;
            Weight = 1.0;
            Hue = 0x8a5;
            Name = "golden reward token";
            Amount = amount;
        }

        public GoldenPrizeToken(Serial serial) : base(serial) { }

        public override Item Dupe(int amount)
        {
            return base.Dupe(new GoldenPrizeToken(amount), amount);
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
            ItemID = 0x1F5F; //Because the ItemID changed.
        }
    }
}