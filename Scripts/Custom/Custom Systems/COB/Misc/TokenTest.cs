//
// Thanks to Mark (RunUO IRC) for completely "reworking" the code
// even if it is a bit... wonky on the numbers now
//

using System;
using System.Collections;
using Server.Items;
using Server.Misc;

namespace Server.Mobiles
{
	public class TokenValidate
	{
		public static void TokenTest(Mobile m, BaseCreature bc)
		{
			if ( m.Backpack == null )
				return;

			int karma = Math.Abs( bc.Karma );
			int tokenbase = ( bc.TotalGold + karma + bc.Fame + ((bc.Hits+bc.Stam+bc.Mana)/3)) / 6000;
			int maxtokens = 6 + ( 100 * tokenbase );
			int mintokens = TokenSettings.Loot_Difference*(maxtokens/100);

			int tokenstogive = Utility.Random( mintokens, maxtokens );
			bool tokensgiven = false;

			foreach( Item i in m.Backpack.Items )
			{
				if( i is TokenBag && !tokensgiven)
				{
					Tokens t = new Tokens( tokenstogive );
					if ( ((Container)i).TryDropItem( m, t, true ) )
					{
						m.SendMessage( "You have received {0} tokens", tokenstogive );
						tokensgiven = true;
					}
					else
						t.Delete();
				}
				if ( tokensgiven )
					break;
			}
		}
	}
}