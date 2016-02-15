using System;
using Server;
using System.Collections;

namespace Server.Items
{
	public class OddLookingKey : SelfDestructingItem
	{
		[Constructable]
		public OddLookingKey() : base()
		{
			Movable = true;
			Weight = 5;
			Name = "an odd looking key";
			Hue = 1157;
			LootType = LootType.Regular;
			ItemID = 0x2002;
            ShowTimeLeft = true;

			TimeLeft = 300; //30mins
			Running = true;
		}

		public override void OnSingleClick( Mobile from )
		{
			LabelTo( from, "the key is slowly fading away" );
			base.OnSingleClick(from);
		}

		public OddLookingKey( Serial serial ) : base( serial )
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