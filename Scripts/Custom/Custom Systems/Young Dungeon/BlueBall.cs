using System;
using Server;
using System.Collections;

namespace Server.Items
	{
	public class BlueBall : SelfDestructingItem
		{
		[Constructable]
		public BlueBall() : this( 1 )
			{
			}
			[Constructable]
			public BlueBall( int amountFrom, int amountTo ) : this( Utility.RandomMinMax( amountFrom, amountTo ) )
			{
			}
			[Constructable]
			public BlueBall( int amount ) : base()
			{

			Name = "blue ball of knowledge";
			Hue = 1365;
			Stackable = true;
			Weight = 2.0;
			Amount = amount;
			ItemID = 0x1869;
			LootType = LootType.Regular;
			ShowTimeLeft = true;

			TimeLeft = 345600;
			Running = true;

}
public BlueBall( Serial serial ) : base( serial )
{}
public override void Serialize( GenericWriter writer )
{
base.Serialize( writer );
writer.Write( (int) 0 ); // version
}
public override void Deserialize( GenericReader reader )
{
base.Deserialize( reader ); int version = reader.ReadInt(); }}}