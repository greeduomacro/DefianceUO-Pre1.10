using System;
using Server.Items;
using Server.Mobiles;

namespace Server.ContextMenus
{
	public class OpenBankEntry : ContextMenuEntry
	{
		private Mobile m_Banker;

		public OpenBankEntry( Mobile from, Mobile banker ) : base( 6105, 12 )
		{
			m_Banker = banker;
		}

		public override void OnClick()
		{
			if ( !Owner.From.CheckAlive() )
				return;

            Banker banker = m_Banker as Banker;
            if (banker == null || (banker.LOSCheck && !banker.InLOS(this.Owner.From)))
                return;

			if ( Owner.From.Criminal )
			{
				m_Banker.Say( 500378 ); // Thou art a criminal and cannot access thy bank box.
			}
			else
			{
				BankBox box = this.Owner.From.BankBox;

				if ( box != null )
					box.Open();
			}
		}
	}
}