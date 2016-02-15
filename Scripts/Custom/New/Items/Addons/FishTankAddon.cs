using Server.Multis;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public class FishTankAddon : BaseAddon
	{


		public override BaseAddonDeed Deed
		{
			get
			{
				FishTankAddonDeed deed = new FishTankAddonDeed();
				return deed;
			}
		}

		[Constructable]
		public FishTankAddon( bool east )
		{
			if ( east )
			{
                        AddonComponent comp = new AddonComponent( 0x3062 );
                        comp.Name = "fish tank";
                        AddComponent( comp, 0, 0, 0 );

                        comp = new AddonComponent( 0x3063 );
                        comp.Name = "fish tank";
			AddComponent( comp, 0, -1, 0 );

			}
			else
			{
			AddonComponent comp = new AddonComponent( 0x3060 );
                        comp.Name = "fish tank";
                        AddComponent( comp, 0, 0, 0 );

                        comp = new AddonComponent( 0x3061 );
                        comp.Name = "fish tank";
			AddComponent( comp, -1, 0, 0 );
			}
		}

		public FishTankAddon( Serial serial ) : base( serial )
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

	public class FishTankAddonDeed : BaseAddonDeed
	{

		//public override int LabelNumber { get { return 1049773; } } // deed for a stone ankh

		public bool m_East;

		public override BaseAddon Addon
		{
			get
			{
				FishTankAddon ankh = new FishTankAddon( m_East );
				return ankh;
			}
		}

		[Constructable]
		public FishTankAddonDeed()
		{
			LootType = LootType.Blessed;
                        Name = "a fish tank deed";
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
			private FishTankAddonDeed m_Deed;

			public InternalGump( FishTankAddonDeed deed ) : base( 150, 50 )
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

		public FishTankAddonDeed( Serial serial ) : base( serial )
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