using System;
using Server;
using Server.Items;
using Server.Gumps;

namespace Server.Items
{
	//[FlipableAttribute( 0x1E5E, 0x1E5F )]
	public class HelpInfoStone : Item
	{
		[Constructable]
		public HelpInfoStone() : base( 3803 )
		{
			Hue = 1365;
			Name = "Stone of Enlightenment";
			Movable = false;
		}

		public HelpInfoStone( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.SendGump( new CommandListGump( 0, from, null ) );
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