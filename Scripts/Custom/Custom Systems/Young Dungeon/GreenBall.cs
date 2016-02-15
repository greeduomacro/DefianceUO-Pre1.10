
using System;
using Server;
using System.Collections;

namespace Server.Items
	{
	public class GreenBall : SelfDestructingItem
		{

			[Constructable]
			public GreenBall() : this( 1 )
			{
			}

			[Constructable]
			public GreenBall( int amountFrom, int amountTo ) : this( Utility.RandomMinMax( amountFrom, amountTo ) )
			{
			}
			[Constructable]

			public GreenBall( int amount ) : base()
				{


					Name = "green ball of knowledge";
					Hue = 2003;
					Stackable = true;
					Weight = 2.0;
					Amount = amount;
					ItemID = 0x1869;
					LootType = LootType.Regular;
					ShowTimeLeft = true;

					TimeLeft = 345600;
					Running = true;


				}

public GreenBall( Serial serial ) : base( serial )
{
}
public override void Serialize( GenericWriter writer )
{
base.Serialize( writer );
writer.Write( (int) 0 ); // version
}
public override void Deserialize( GenericReader reader )
{
base.Deserialize( reader ); int version = reader.ReadInt();
}
}
}