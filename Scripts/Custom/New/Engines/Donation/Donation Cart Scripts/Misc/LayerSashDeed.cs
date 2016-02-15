using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class LayerSashTarget : Target // Create our targeting class (which we derive from the base target class)
	{
		private LayerSashDeed m_Deed;

		public LayerSashTarget( LayerSashDeed deed ) : base( 1, false, TargetFlags.None )
		{
			m_Deed = deed;
		}

		protected override void OnTarget( Mobile from, object target ) // Override the protected OnTarget() for our feature
		{
			if ( m_Deed.Deleted )
				return;

			if ( !m_Deed.IsChildOf( from.Backpack ) ) // Make sure its in their pack
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else if ( target is BodySash || target is SaviourSash ) //Thx Darky for not making SaviourSash a subclass of BodySash
			{
				Item item = (Item)target;

				if ( item.Layer == Layer.Earrings )
				{
					from.SendMessage( "That sash is already layered." );
				}
				else
				{
					if( item.RootParent != from ) // Make sure its in their pack or they are wearing it
					{
						from.SendMessage( "Invalid target." );
					}
					else
					{
						item.Layer = Layer.Earrings;
						/* Al: It isnt necessary any more
						if(item.Name == null)
							item.Name = item.ItemData.Name + " (layered)";
						else
							item.Name += " (layered)";
						*/
						from.SendMessage( "The sash looks the same but feels a little bit bigger." );

						m_Deed.Delete(); // Delete the deed
					}
				}
			}
			else
			{
				from.SendMessage( "Invalid target." );
			}
		}
	}

	public class LayerSashDeed : Item // Create the item class which is derived from the base item class
	{
		[Constructable]
		public LayerSashDeed() : base( 0x14F0 )
		{
			Weight = 1.0;
                        Hue = 1158;
			Name = "a deed for sash layering";
			LootType = LootType.Blessed;
		}

		public LayerSashDeed( Serial serial ) : base( serial )
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
			LootType = LootType.Blessed;

			int version = reader.ReadInt();
		}

		public override bool DisplayLootType{ get{ return false; } }

		public override void OnDoubleClick( Mobile from ) // Override double click of the deed to call our target
		{
			if ( !IsChildOf( from.Backpack ) ) // Make sure its in their pack
			{
				 from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else
			{
				from.SendMessage( "Target a sash to layer it." );
				from.Target = new LayerSashTarget( this ); // Call our target
			 }
		}
	}
}