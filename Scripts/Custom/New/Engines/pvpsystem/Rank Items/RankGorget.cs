using System;
using Server;
using Server.Mobiles;
using Server.FSPvpPointSystem;

namespace Server.Items
{
	public class RankGorget : LeatherGorget
	{
		[Constructable]
		public RankGorget()
		{
			Resource = CraftResource.BarbedLeather;
		}

		public override bool OnEquip( Mobile from )
		{
			FSPvpSystem.PvpStats ps = FSPvpSystem.GetPvpStats( from );
			if ( PvpRankInfo.GetInfo( ps.RankType ).Rank > 1 ) // Change Min Rank To Use Here!!!
				return true;
			else
			{
				from.SendMessage( "You lack the rank to use this item." );
				from.SendMessage( "The item has been removed." );
				this.Delete();
				return false;
			}
		}

		public RankGorget( Serial serial ) : base( serial )
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