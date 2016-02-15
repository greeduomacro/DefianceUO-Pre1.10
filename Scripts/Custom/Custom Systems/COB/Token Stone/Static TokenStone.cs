//==============================================//
// Created by Dupre					//
//==============================================//
using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Menus;
using Server.Menus.Questions;
using Server.Misc;

namespace Server.Items
{

   public class StaticTokenStone : Item
   {
	[Constructable]
	public StaticTokenStone() : base( 4466 )
	{
		Hue = TokenSettings.Token_Colour;
		Movable = false;
		Name = "a reward Stone";
	}
	public StaticTokenStone( Serial serial ) : base( serial )
	{
	}
	public override void OnDoubleClick( Mobile from )

	{
	if ( from.InRange( this.GetWorldLocation(), 2 ) )
	{
		from.SendGump( new TokenStoneGump( from ) );
	}
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