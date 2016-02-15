using System;
using Server;
using System.Collections;
namespace Server.Items
	{
	public class RedBall : SelfDestructingItem
		{
			[Constructable]
			public RedBall() : this( 1 )
			{
			}

			[Constructable]
			public RedBall( int amountFrom, int amountTo ) : this( Utility.RandomMinMax( amountFrom, amountTo ) )
			{
			}

			[Constructable]

			public RedBall( int amount ) : base(  )
			{

			Name = "red ball of knowledge";
			Hue = 38;
			Stackable = true;
			Weight = 2.0;
			Amount = amount;
			ItemID = 0x1869;
			LootType = LootType.Regular;
			ShowTimeLeft = true;

			TimeLeft = 345600;
			Running = true;


			}

public RedBall( Serial serial ) : base( serial )
{
}
public override void Serialize( GenericWriter writer )
{
base.Serialize( writer );
writer.Write( (int) 0 ); }
public override void Deserialize( GenericReader reader )
{
base.Deserialize( reader ); int version = reader.ReadInt(); }}}