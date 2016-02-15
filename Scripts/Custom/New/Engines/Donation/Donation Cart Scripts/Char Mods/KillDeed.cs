using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Items
{
	public class KillDeed : Item
	{
		[Constructable]
		public KillDeed()
		{
                        LootType = LootType.Regular;
			ItemID = 5360;
			Weight = 0.0;
			Name = "no murder count deed";
		}

		public KillDeed( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			//PlayerMobile pm = (PlayerMobile)from;
			//if ( pm.Rank >= 6 ) // Change Min Rank To Use Here!!!
			{
				if ( IsChildOf (from.Backpack))
				{
					if ( from.ShortTermMurders == 0 && from.Kills == 0 )
					{
						from.SendMessage( "You have no murders to disolve." );
					}
					else
					{
						from.ShortTermMurders = 0;
						from.Kills = 0;
						from.SendMessage( "Your long and short term murders have been disolved." );
						this.Delete();
					}
				}
				else
				{
					from.SendMessage( "You must have this in your pack to use it." );
				}
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