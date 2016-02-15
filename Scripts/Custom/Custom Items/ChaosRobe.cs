// part of Public Chaos-Order War system
//scripted by Unclouded.. www.unclouded.tk

using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class ChaosRobe : BaseSuit
	{
		[Constructable]
		public ChaosRobe() : base( AccessLevel.Player, 0x0, 0x2043 )
		{
			LootType = LootType.Newbied;
			Weight = 5.0;
			Name = "An Chaos Robe";
		}

		public ChaosRobe( Serial serial ) : base( serial )
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
		}
	}
}