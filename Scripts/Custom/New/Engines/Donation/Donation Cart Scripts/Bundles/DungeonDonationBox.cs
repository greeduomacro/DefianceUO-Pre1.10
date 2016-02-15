using System;
using Server.Mobiles;
using Server.Multis.Deeds;

namespace Server.Items
{
	[DynamicFliping]
	[Flipable( 0xE41, 0xE40 )]
	public class DungeonDonationBox : MetalGoldenChest
	{
		[Constructable]
		public DungeonDonationBox()
		{
			Weight = 1.0;
			Hue = 1109;
			Item item = null;
			Name = "dungeon decoration chest";

			PlaceItemIn( 16, 51, (item = new BoneThrone()) );
                        PlaceItemIn( 16, 51, (item = new BoneThrone()) );
			PlaceItemIn( 58, 70, (item = new InscribedTombStone()) );
                        PlaceItemIn( 58, 70, (item = new InscribedTombStone()) );
                        PlaceItemIn( 58, 70, (item = new IronMaiden()) );

			PlaceItemIn( 44, 57, (new BoneTable()) );
                        PlaceItemIn( 44, 57, (new BoneTable()) );

                        BaseContainer cont;
			PlaceItemIn( 131, 121, (cont = new Bag()) );
			cont.Hue = 2413;

			cont.PlaceItemIn( 131, 75, (item = new OpenCoffinAddonDeed()) );
                        cont.PlaceItemIn( 131, 75, (item = new OpenCoffinAddonDeed()) );


			cont.PlaceItemIn( 140, 51, (item = new StoneCoffinAddonDeed()) );
                        cont.PlaceItemIn( 140, 51, (item = new StoneCoffinAddonDeed()) );


			cont.PlaceItemIn( 58, 83, (item = new ShadowAltarAddonDeed()) );
                        cont.PlaceItemIn( 58, 83, (item = new ShadowAltarAddonDeed()) );

                        cont.PlaceItemIn( 56, 50, (item = new BoneCouchAddonDeed()) );
                        cont.PlaceItemIn( 56, 50, (item = new BoneCouchAddonDeed()) );

		}

		public DungeonDonationBox( Serial serial ) : base( serial )
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