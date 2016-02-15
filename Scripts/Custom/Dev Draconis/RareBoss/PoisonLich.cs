using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class PoisonLich : BaseRareBoss
	{
		[Constructable]
		public PoisonLich() : base( AIType.AI_Mage )
		{
			Name = "Lord of Venom";
			Body = 79;
			Hue = 2006;
			BaseSoundID = 412;
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Lethal; } }
		public override double HitPoisonChance{ get{ return 0.80; } }
		public override int DoStunPunch{ get { return 2; } }

		public PoisonLich( Serial serial ) : base( serial )
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