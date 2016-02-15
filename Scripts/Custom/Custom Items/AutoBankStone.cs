using System;
using System.IO;
using System.Collections;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Gumps;

namespace Server.Items
{
	public class AutoBankStone : Item
	{
		[Constructable]
		public AutoBankStone() : base( 0xED4 )
		{
			Name = "Auto-Bank Stone";
			Movable = false;
			Hue = 0x480;
		}

		public override void OnDoubleClick( Mobile from )
		{
			Backpack bankbag = new Backpack();
			Container mobilePack = from.Backpack;
			BankBox mobileBox = from.BankBox;
			ArrayList equipitems = new ArrayList(from.Items);
			mobileBox.AddItem( bankbag );
			//				this.AddPlayer( player );
			from.Frozen = true;
			from.SendMessage( "All your belongings were transported to your bankbox.  You will need to close you backpack and re-open it for it to display the correct contents.");

			//				if ( Teams.Contains( from ) )
			//				{
			//					from.Frozen = true;
			//				}
			foreach (Item item in equipitems)
			{
				if ((item.Layer != Layer.Bank) && (item.Layer != Layer.Backpack) && (item.Layer != Layer.Hair) && (item.Layer != Layer.FacialHair) && (item.Layer != Layer.Mount))
				{
					mobilePack.DropItem( item );
				}
			}

			ArrayList packitems = new ArrayList( mobilePack.Items );

			foreach (Item items in packitems)
			{
				bankbag.DropItem(items);
			}
		}

		public AutoBankStone( Serial serial ) : base( serial )
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