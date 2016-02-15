
using System;
using Server;
using System.Collections;

namespace Server.Items
	{
	public class PurpleBall : SelfDestructingItem
		{

			[Constructable]
			public PurpleBall() : this( 1 )
			{
			}

			[Constructable]
			public PurpleBall( int amountFrom, int amountTo ) : this( Utility.RandomMinMax( amountFrom, amountTo ) )
			{
			}
			[Constructable]

			public PurpleBall( int amount ) : base()
				{


					Name = "purple ball of knowledge";
					Hue = 722;
					Stackable = true;
					Weight = 2.0;
					Amount = amount;
					ItemID = 0x1869;
					LootType = LootType.Regular;
					ShowTimeLeft = true;

					TimeLeft = 172800;
					Running = true;


				}

public PurpleBall( Serial serial ) : base( serial )
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