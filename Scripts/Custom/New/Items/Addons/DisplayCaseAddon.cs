using Server.Multis;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public class DisplayCaseAddon : BaseAddon
	{


		public override BaseAddonDeed Deed
		{
			get
			{
				DisplayCaseAddonDeed deed = new DisplayCaseAddonDeed();
				return deed;
			}
		}

		[Constructable]
		public DisplayCaseAddon( bool east )
		{
			if ( east )
			{
                        AddonComponent comp = new AddonComponent( 0xB06 );
                        comp.Name = "wooden display case";
                        AddComponent( comp, 0, 1, 0 );

                        comp = new AddonComponent( 0xB07 );
                        comp.Name = "wooden display case";
			AddComponent( comp, 0, 0, 0 );

                        comp = new AddonComponent( 0xB08 );
                        comp.Name = "wooden display case";
			AddComponent( comp, 0, -1, 0 );

                        comp = new AddonComponent( 0xB03 );
                        comp.Name = "wooden display case";
                        AddComponent( comp, 0, 1, 3 );

                        comp = new AddonComponent( 0xB04 );
                        comp.Name = "wooden display case";
			AddComponent( comp, 0, 0, 3 );

                        comp = new AddonComponent( 0xB05 );
                        comp.Name = "wooden display case";
			AddComponent( comp, 0, -1, 3 );

			}
			else
			{
			AddonComponent comp = new AddonComponent( 0xB02 );
                        comp.Name = "wooden display case";
                        AddComponent( comp, -1, 0, 0 );

                        comp = new AddonComponent( 0xB01 );
                        comp.Name = "wooden display case";
			AddComponent( comp, 0, 0, 0 );

                        comp = new AddonComponent( 0xB00 );
                        comp.Name = "wooden display case";
			AddComponent( comp, 1, 0, 0 );

                        comp = new AddonComponent( 0xAFF );
                        comp.Name = "wooden display case";
                        AddComponent( comp, -1, 0, 3 );

                        comp = new AddonComponent( 0xAFE );
                        comp.Name = "wooden display case";
			AddComponent( comp, 0, 0, 3 );

                        comp = new AddonComponent( 0xAFD );
                        comp.Name = "wooden display case";
			AddComponent( comp, 1, 0, 3 );
			}
		}

		public DisplayCaseAddon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version

		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}

	public class DisplayCaseAddonDeed : BaseAddonDeed
	{

		//public override int LabelNumber { get { return 1049773; } } // deed for a stone ankh

		public bool m_East;

		public override BaseAddon Addon
		{
			get
			{
				DisplayCaseAddon ankh = new DisplayCaseAddon( m_East );
				return ankh;
			}
		}

		[Constructable]
		public DisplayCaseAddonDeed()
		{
			LootType = LootType.Blessed;
                        Name = "a wooden display case deed";
		}

		public override void OnDoubleClick( Mobile from )
		{
			if (IsChildOf( from.Backpack ))
			{
				BaseHouse house = BaseHouse.FindHouseAt( from );

				if (house != null && house.IsOwner( from ))
				{
					from.CloseGump( typeof( InternalGump ) );
					from.SendGump( new InternalGump( this ) );
				}
				else
				{
					from.SendLocalizedMessage( 502092 ); // You must be in your house to do this.
				}
			}
			else
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
		}

		private void SendTarget( Mobile m )
		{
			base.OnDoubleClick( m );
		}

		private class InternalGump : Gump
		{
			private DisplayCaseAddonDeed m_Deed;

			public InternalGump( DisplayCaseAddonDeed deed ) : base( 150, 50 )
			{
				Closable = false;

				m_Deed = deed;

				AddBackground( 0, 0, 350, 250, 0xA28 );

				AddItem( 242, 52, 0x206D );
				AddButton( 70, 35, 0x868, 0x869, 1, GumpButtonType.Reply, 0 ); // South

                                AddItem( 90, 52, 0x206F );
				AddButton( 185, 35, 0x868, 0x869, 2, GumpButtonType.Reply, 0 ); // East
			}

			public override void OnResponse( NetState sender, RelayInfo info )
			{
				if ( m_Deed.Deleted )
					return;

				m_Deed.m_East = (info.ButtonID == 2);
				m_Deed.SendTarget( sender.Mobile );
			}
		}

		public DisplayCaseAddonDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int)0 ); // version

		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();

		}
	}
}