using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Prompts;

namespace Server.Items
{
	public class WhisperingRoseDeed : Item
	{
		[Constructable]
		public WhisperingRoseDeed() : base( 0x14F0 )
		{
			Hue = 0x26;
			LootType = LootType.Blessed;
			Name = "a whispering rose deed";
		}

		public WhisperingRoseDeed( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 );
			}
			else
			{
				from.SendMessage( "Whom is this rose to?" );
				from.Prompt = new WhisperingRoseToPrompt( this );
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

	public class WhisperingRose : Item
	{
		[Constructable]
		public WhisperingRose( string from, string to ) : base( 0x18E9 )
		{
			LootType = LootType.Blessed;
			Name = "a whispering rose from " + from + " to " + to;
		}

		public WhisperingRose( Serial serial ) : base( serial )
		{
		}

		public override void OnSingleClick( Mobile from )
		{
			LabelTo( from, Name );
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

	public class WhisperingRoseToPrompt : Prompt
	{
		public WhisperingRoseDeed deed;

		public WhisperingRoseToPrompt( WhisperingRoseDeed item )
		{
			deed = item;
		}

		public override void OnCancel( Mobile from )
		{
			from.SendMessage( "You decide not to create the rose at this time." );
		}

		public override void OnResponse( Mobile from, string text )
		{
			if( text.Length >= 1 )
			{
				deed.Delete();
				from.AddToBackpack( new WhisperingRose( from.Name, text ) );
				from.SendMessage( "The whispering rose has been added to your backpack." );
			}
			else
			{
				from.SendMessage( "You must address the rose to someone!" );
			}
		}
	}
}