using System;
using Server.Items;

namespace Server.Items
{
	public class HealStone : Item
	{
		[Constructable]
		public HealStone() : base( 0xED4 )
		{
			Movable = false;
			Hue = 1278;
			Name = "a Rejuvenation Stone";
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.InRange( this.GetWorldLocation(), 1 ) )
			{
				from.Hits += 111;
				from.Mana += 111;
                                from.Stam += 111;
				from.SendMessage( "Rejuvenation Sucessfull." );
			}
			else
			from.SendLocalizedMessage( 500446 ); // That is too far away.
		}

		public HealStone( Serial serial ) : base( serial )
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