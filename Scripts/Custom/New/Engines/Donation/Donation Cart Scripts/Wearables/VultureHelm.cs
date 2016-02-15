using System;
using Server;

namespace Server.Items
{
	public class VultureHelm : BaseArmor
	{
		public override int InitMinHits{ get{ return 0; } }
		public override int InitMaxHits{ get{ return 0; } }

		public override int AosStrReq{ get{ return 40; } }
		public override int OldStrReq{ get{ return 10; } }

		public override int ArmorBase{ get{ return 10; } }

		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Leather; } }
                public override ArmorMeditationAllowance DefMedAllowance{ get{ return ArmorMeditationAllowance.All; } }

		[Constructable]
		public VultureHelm() : base( 12650 )
		{
			Name = "Vultures Serrated Beak";
			Weight = 1.0;
			Layer = Layer.Helm;
		}

		public VultureHelm( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( (int) 0 );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			int version = reader.ReadEncodedInt();
		}
	}
}