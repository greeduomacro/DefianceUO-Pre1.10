using System;
using Server.Items;

namespace Server.Items
{
	public class AncientSamuraiHelm : BaseArmor
	{
		public override int BasePhysicalResistance{ get{ return 15; } }
		public override int BaseFireResistance{ get{ return 10; } }
		public override int BaseColdResistance{ get{ return 10; } }
		public override int BasePoisonResistance{ get{ return 15; } }
		public override int BaseEnergyResistance{ get{ return 10; } }

		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }

		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Plate; } }

		[Constructable]
		public AncientSamuraiHelm() : base( 0x236C )
		{
			Name = "Ancient Samurai Helm";
			Weight = 5.0;
			LootType = LootType.Blessed;
			ArmorAttributes.SelfRepair = 10;
			Attributes.DefendChance = 15;
			Attributes.WeaponDamage = 20;
			Attributes.WeaponSpeed = 10;
			ArmorAttributes.MageArmor = 1;
			ArmorAttributes.LowerStatReq = 100;
		}

		public AncientSamuraiHelm( Serial serial ) : base( serial )
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

			if ( Weight == 1.0 )
				Weight = 5.0;
		}
	}
}