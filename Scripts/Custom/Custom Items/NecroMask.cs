using System;
using Server.Items;

namespace Server.Items
{
	public class NecromaticMask : BaseArmor
	{
		public override int BasePhysicalResistance{ get{ return 5; } }
		public override int BaseFireResistance{ get{ return 1; } }
		public override int BaseColdResistance{ get{ return 3; } }
		public override int BasePoisonResistance{ get{ return 3; } }
		public override int BaseEnergyResistance{ get{ return 5; } }

		public override int InitMinHits{ get{ return 30; } }
		public override int InitMaxHits{ get{ return 50; } }

		public override int AosStrReq{ get{ return 30; } }
		public override int OldStrReq{ get{ return 10; } }

		public override int ArmorBase{ get{ return 20; } }

		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Leather; } }
		public override CraftResource DefaultResource{ get{ return CraftResource.RegularLeather; } }

		public override ArmorMeditationAllowance DefMedAllowance{ get{ return ArmorMeditationAllowance.All; } }

		[Constructable]
		public NecromaticMask() : base( 0x154B )
		{
			Hue = 0x485;
			Weight = 1;
			Name = "Necromatic Mask";
		}

		public NecromaticMask( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}