using System;
using Server;


namespace Server.Items
{
	public class IdolBossLoot : Item
	{
		[Constructable]
		public IdolBossLoot() : base()
		{
			Movable = true;
			Weight = 5;
			ItemID = Utility.RandomList( 12411, 12412, 12413, 12414, 12415, 12416, 12417, 12418, 12419, 4723, 4722, 4721, 4720, 4719, 4718, 4717, 4716, 4715);
		}

		public IdolBossLoot( Serial serial ) : base( serial )
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