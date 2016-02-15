using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Misc;
using Server.Mobiles;

namespace Server.Items
{
	public class KarmaIounStone : Item
	{
		[Constructable]
		public KarmaIounStone() : base( 0x2809 )
		{
			base.Weight = 1.0;
			base.Name = "dreaded ioun stone";
                        Hue = 2106;
                        LootType = LootType.Cursed;
		}

		public KarmaIounStone( Serial serial ) : base( serial )
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
			  mobile.Karma -= 10000;
                          mobile.SendMessage( 0x5, "Using the Ioun Stone makes you loose a great amount of karma!" );
                          this.Delete();
                        }
		}
	}
}