using System;
using Server.Misc;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	public class NameChangeDeed : Item
	{
		[Constructable]
		public NameChangeDeed() : base( 0x14F0 )
		{
			base.Weight = 1.0;
			base.Name = "a name change deed";
		}

		public NameChangeDeed( Serial serial ) : base( serial )
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
			if ( !IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1042001 );
			else
				from.Prompt = new NameChangePrompt(from, this);
		}
	}

	public class NameChangePrompt : Prompt
	{
		private Item m_Item;
		public NameChangePrompt( Mobile from, Item item )
		{
			m_Item = item;
			from.SendMessage("What would you like to change your name to (2-16 characters)?");
		}

		public override void OnResponse( Mobile from, string text )
		{
            if (m_Item == null || m_Item.Deleted || !m_Item.IsChildOf(from.Backpack))
                from.SendLocalizedMessage(1042001);
            else
            {
                text = text.Trim();
                if (!NameVerification.Validate(text, 2, 16, true, false, true, 1, NameVerification.SpaceDashPeriodQuote))
                    from.SendMessage("Names must contain 2-16 alphabetic characters.");
                else
                {
                    from.Name = text;
                    m_Item.Delete();
                }
            }
		}
	}
}