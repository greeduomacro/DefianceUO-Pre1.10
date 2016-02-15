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

   [FlipableAttribute( 0xEDC, 0xEDB )]
   public class TokenStone : Item
   {
	[Constructable]
	public TokenStone() : base( 0x1869 )
	{
		Hue = TokenSettings.Token_Colour;
		Movable = true;
		Name = "a reward stone";
		LootType = LootType.Blessed;
	}
	public TokenStone( Serial serial ) : base( serial )
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