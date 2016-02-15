using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Misc;
using Server.Mobiles;

namespace Server.Items
{
	public class FameIounStone : Item
	{
		[Constructable]
		public FameIounStone() : base( 0x2809 )
		{
			base.Weight = 1.0;
			base.Name = "valor ioun stone";
                        Hue = 1265;
                        LootType = LootType.Cursed;
		}

		public FameIounStone( Serial serial ) : base( serial )
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

		public override void OnDoubleClick( Mobile from )
		{
                        PlayerMobile mobile = from as PlayerMobile;

                        if ( !IsChildOf( from.Backpack ) )
                        {
                          from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
                        }
                        else
                        {
			  mobile.Fame += 10000;
                          mobile.SendMessage( 0x5, "Using the Ioun Stone made you gain an enormous amount of fame." );
                          this.Delete();
                        }
		}
	}
}