using System;
using Server;

namespace Server.Items
{
	public class RavenHelm : BaseArmor
	{
		public override int InitMinHits{ get{ return 0; } }
		public override int InitMaxHits{ get{ return 0; } }

		public override int AosStrReq{ get{ return 40; } }
		public override int OldStrReq{ get{ return 10; } }

		public override int ArmorBase{ get{ return 10; } }

		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Leather; } }
                public override ArmorMeditationAllowance DefMedAllowance{ get{ return ArmorMeditationAllowance.All; } }

		[Constructable]
		public RavenHelm() : base( 11122 )
		{
			Name = "Ravens Headdress of Spiritry";
			Weight = 1.0;
			Layer = Layer.Helm;
		}

		public RavenHelm( Serial serial ) : base( serial )
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