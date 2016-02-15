using System;
using Server;
using System.Collections;

namespace Server.Items
{
	public class YoungParchment: SelfDestructingItem
	{
		[Constructable]
		public YoungParchment() : base()
		{
			Movable = true;
			Weight = 5;
			Name = "a special parchment";
			Hue = 1158;
			LootType = LootType.Blessed;
			ItemID = 8792;
            	ShowTimeLeft = true;

			TimeLeft = 604800;
			Running = true;
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.BankBox.DropItem( new BagOfReagents( 100 ) );
			PotionKeg keg = new PotionKeg();
			keg.Type = PotionEffect.HealGreater;
			keg.Held = 100;
			keg.Hue = 54;
			from.BankBox.DropItem( keg );
			keg = new PotionKeg();
			keg.Type = PotionEffect.CureGreater;
			keg.Held = 100;
			keg.Hue = 43;
			from.BankBox.DropItem( keg );
			from.BankBox.DropItem( new BankCheck( 5000 ) );
			Delete();
		}

		public YoungParchment( Serial serial ) : base( serial )
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