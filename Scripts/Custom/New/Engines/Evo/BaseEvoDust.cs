#region AuthorHeader
//
//	EvoSystem version 1.6, by Xanthos
//
//
#endregion AuthorHeader
using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Xanthos.Evo
{
	public abstract class BaseEvoDust: Item
	{
		public BaseEvoDust() : this( 1 )
		{
		}

		public BaseEvoDust( int amount ) : base( 0x26B8 )
		{
			Stackable = true;
			Weight = 0.0;
			Amount = amount;
			Name = "evolution dust";
			Hue = 1153;
		}

		public BaseEvoDust( Serial serial ) : base ( serial )
		{
		}

		// Define as concrete in subclasses and return an instance of your BaseEvoDust subclass there
		public abstract BaseEvoDust NewDust();

		public override Item Dupe( int amount )
		{
			return base.Dupe( NewDust(), amount );
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