using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using System.Collections;
using Server.FSPvpPointSystem;

namespace Server.Items
{
	[FlipableAttribute( 7774, 7775 )]
	public class PvpScoreBoard : Item
	{
		[Constructable]
		public PvpScoreBoard() : base( 7774 )
		{
			Movable = false;
			Name = "Pvp Score Board";
		}

		public override void OnDoubleClick( Mobile from )
		{
			//FSPvpSystem.CheckTopWinners();
			from.SendGump( new PvpSelectGump() );

			//int count = 1;

			//foreach ( FSPvpSystem.PvpStats ps in FSPvpSystem.Winners )
			//{
			//	from.SendMessage( "{0}. {1} with {2} wins.", count, ps.Owner.Name, ps.Wins );
			//	count += 1;
			//}
		}

		public PvpScoreBoard( Serial serial ) : base( serial )
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