using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a minion corpse" )]
	public class FadingMinion : Ghoul
	{
		[Constructable]
		public FadingMinion() : base()
		{
			Name = "a fading minion";
			Hue = 16385;
			Body = 26;
			BaseSoundID = 0x482;
		}

		public FadingMinion( Serial serial ) : base( serial )
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

	[CorpseName( "a minion corpse" )]
	public class FieryMinion : Ghoul
	{
		[Constructable]
		public FieryMinion() : base()
		{
			Name = "a fiery minion";
			Hue = 1281;
			BodyValue = 58;
			BaseSoundID = 466;
		}

		public FieryMinion( Serial serial ) : base( serial )
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

	[CorpseName( "a minion corpse" )]
	public class IcyMinion : Ghoul
	{
		[Constructable]
		public IcyMinion() : base()
		{
			Name = "an icy minion";
			Hue = 1150;
			Body = Utility.RandomList( 60, 61 );
			BaseSoundID = 362;
		}

		public IcyMinion( Serial serial ) : base( serial )
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

	[CorpseName( "a minion corpse" )]
	public class ShadowMinion : Ghoul
	{
		[Constructable]
		public ShadowMinion() : base()
		{
			Name = "a shadow minion";
			Hue = 2235;
			Body = 184;
			BaseSoundID = 0;

			Robe robe = new Robe();
			robe.Hue = 836;
			robe.LootType = LootType.Blessed;
			AddItem( robe );
		}

		public override bool AlwaysMurderer{ get{ return true; } }

		public ShadowMinion( Serial serial ) : base( serial )
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