using System;
using Server;
using Server.Accounting;
using Server.Mobiles;

namespace Server.Items
{
	public class DonationTeleporter : Teleporter
	{
		public DonationTeleporter() : this( new Point3D( 0, 0, 0 ), null, false )
		{
		}

		public DonationTeleporter( Point3D pointDest, Map mapDest ) : this( pointDest, mapDest, false )
		{
		}

		public DonationTeleporter( Point3D pointDest, Map mapDest, bool creatures ) : base( pointDest, mapDest, creatures )
		{
			Name = "Donation Teleporter";
		}

		public DonationTeleporter( Serial serial ) : base ( serial )
		{
		}

		public override void StartTeleport( Mobile m )
		{
			if ( m != null && m is PlayerMobile && ((PlayerMobile)m).HasDonated )
				base.StartTeleport( m );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
		}
	}
}