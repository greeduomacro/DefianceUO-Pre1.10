using System;
using Server.Items;
using Server.Gumps;
using Server.FSPvpPointSystem;

namespace Server.Items
{
	public class PvpRewardStone : Item
	{
		[Constructable]
		public PvpRewardStone() : base( 0xED4 )
		{
			Movable = false;
			Hue = 1157;
			Name = "Pvp Reward Stone";
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.CloseGump( typeof( PvpRewardGump ) );
			from.SendGump( new PvpRewardGump() );
		}

		public PvpRewardStone( Serial serial ) : base( serial )
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