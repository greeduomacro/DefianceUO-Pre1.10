using System;
using Server;
using System.Collections;

namespace Server.Items
{
	public class SoulCrystal : SelfDestructingItem
	{
		[Constructable]
		public SoulCrystal() : base()
		{
			Movable = true;
			Weight = 5;
			Name = "a soul crystal";
			Hue = 1175;
			LootType = LootType.Regular;
			ItemID = 0x1F1C;
            ShowTimeLeft = true;

			TimeLeft = 1800; //30mins
			Running = true;
		}

		public override void OnSingleClick( Mobile from )
		{
			LabelTo( from, "the crystal is slowly fading away" );
			base.OnSingleClick(from);
		}

		public SoulCrystal( Serial serial ) : base( serial )
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