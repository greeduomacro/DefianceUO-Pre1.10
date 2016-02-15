using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Xanthos.Evo
{
	public class EvoSpiderEvoDust : BaseEvoDust
	{
		[Constructable]
		public EvoSpiderEvoDust() : this( 1 )
		{
		}

		[Constructable]
		public EvoSpiderEvoDust( int amount ) : base( amount )
		{
			Amount = amount;
			Name = "spider mucas";
			Hue = 1175;
		}

		public EvoSpiderEvoDust( Serial serial ) : base ( serial )
		{
		}

		public override BaseEvoDust NewDust()
		{
			return new EvoSpiderEvoDust();
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