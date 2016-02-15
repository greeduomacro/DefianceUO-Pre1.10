using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class ElderElemental : BaseRareBoss
	{
		[Constructable]
		public ElderElemental () : base( AIType.AI_Mage )
		{
			Name = "an elder elemental";
			Body = 162;
			Hue = Utility.RandomList( 1367, 1157, 1175, 1158, 1156, 1154 );
			BaseSoundID = 263;
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool DoProvoPets { get { return true; } }

		public ElderElemental( Serial serial ) : base( serial )
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