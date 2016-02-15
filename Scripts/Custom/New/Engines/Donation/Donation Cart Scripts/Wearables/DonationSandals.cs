using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	public class DonationSandals : Sandals
	{
		[Constructable]
		public DonationSandals() : this( 0 )
		{
		}

		[Constructable]
		public DonationSandals( int hue ) : base( hue )
		{
			Weight = 1.0;
                        Hue = 1177;
			LootType = LootType.Blessed;
			Name = "I do my part to keep shard Online";
		}

		public DonationSandals( Serial serial ) : base( serial )
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

		/*public override bool OnDroppedInto( Mobile from, Container target, Point3D p )
		{
			if (from.AccessLevel < AccessLevel.GameMaster && target.Parent != from)
				return false;
			return base.OnDroppedInto( from, target, p );
		}*/

		/*public override bool OnDroppedOnto( Mobile from, Item target )
		{
			if (from.AccessLevel < AccessLevel.GameMaster && target.Parent != from)
				return false;
			return base.OnDroppedOnto( from, target );
		}*/

		/*public override bool OnDroppedToMobile( Mobile from, Mobile target )
		{
			if (from.AccessLevel < AccessLevel.GameMaster && target.AccessLevel < AccessLevel.GameMaster)
				return false;
			return base.OnDroppedToMobile( from, target );
		}*/

		/*public override bool OnDroppedToWorld( Mobile from, Point3D p )
		{
			if (from.AccessLevel < AccessLevel.GameMaster)
				return false;
			return base.OnDroppedToWorld( from, p );
		}*/

		/*public override bool AllowSecureTrade( Mobile from, Mobile to, Mobile newOwner, bool accepted )
		{
			if (from.AccessLevel < AccessLevel.GameMaster && to.AccessLevel < AccessLevel.GameMaster)
				return false;
			return base.AllowSecureTrade( from, to, newOwner, accepted );
		}*/
	}
}