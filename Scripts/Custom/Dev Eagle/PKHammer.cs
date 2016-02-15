using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x1439, 0x1438 )]
	public class PKHammer : BaseBashing
	{
		public override int OldStrengthReq{ get{ return 40; } }
		public override int OldMinDamage{ get{ return 1; } }
		public override int OldMaxDamage{ get{ return 10; } }
		public override int OldSpeed{ get{ return 31; } }

		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Bash2H; } }

		[Constructable]
		public PKHammer() : base( 0x1439 )
		{
			Name = "hammer of the elite";
			Weight = 10.0;
			Layer = Layer.TwoHanded;
			LootType = LootType.Newbied;
			Hue = 1221;
		}

		public PKHammer( Serial serial ) : base( serial )
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