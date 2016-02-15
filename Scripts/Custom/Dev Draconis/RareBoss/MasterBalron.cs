using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class MasterBalron : BaseRareBoss
	{
		[Constructable]
		public MasterBalron () : base( AIType.AI_Mage )
		{
			Name = "Master of the Abyss";
			Hue = 1157;
			Body = 9;
			BaseSoundID = 357;

			SetStr( 1000, 1200 );

			SetHits( 1400, 1500 );

			VirtualArmor = 200;
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override int Meat{ get{ return 2; } }
		public override int CanCheckReflect{ get { return 6; } }
		public override bool DoSkillLoss{ get { return true; } }

		public MasterBalron( Serial serial ) : base( serial )
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