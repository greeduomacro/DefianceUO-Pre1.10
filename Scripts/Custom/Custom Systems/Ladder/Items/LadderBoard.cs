/**
*	Ladder system by Morten Legarth (c)taunted.dk ( legarth@taunted.dk )
*	Version: v0.10 -  26-02-2005
*
*	This system has been written for use at the Blitzkrieg frees-shard
*	http://blitzkrieg.dorijan.de . Unauthorized reproduction or redistribution
*	is prohibited.
*
*							LadderBoard.cs
*						-------------------------
*
*	File Description:	The Ladder Board is the interface for
*						displaying results and such.
*
*/

using System;
using Server.Mobiles;
using Server.Network;

namespace Server.Ladder
{

	public class LadderBoard : Item
	{
		[Constructable]
		public LadderBoard() : base( 0x1E5E )
		{
			Movable = false;
			Name = "a ladder board";
		}

		public LadderBoard(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);// version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (from != null && from is PlayerMobile)
			{
				if (!from.InRange(GetWorldLocation(), 3))
					from.SendLocalizedMessage(500446); // That is too far away.
				else
				{
					from.SendGump(new ChooseGump());
				}
			}
		}
	}
}