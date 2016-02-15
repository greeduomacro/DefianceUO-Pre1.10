using System;
using Server.Mobiles;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;
using Server.Gumps;

namespace Server.Items
{
	public class SpecialHairRestylingDeed : Item
	{
		[Constructable]
		public SpecialHairRestylingDeed() : base( 0x14F0 )
		{
			Weight = 1.0;
            Hue = 1153;
            LootType = LootType.Blessed;
            Name = "a special hair restyling deed";
        }

		public SpecialHairRestylingDeed( Serial serial ) : base( serial )
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
			if ( from.Backpack == null || !IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			else
				from.SendGump( new InternalGump( from, this ) );
		}

		private class InternalGump : Gump
		{
			private Mobile m_From;
			private SpecialHairRestylingDeed m_Deed;

			public InternalGump( Mobile from, SpecialHairRestylingDeed deed ) : base( 50, 50 )
			{
				m_From = from;
				m_Deed = deed;

				from.CloseGump( typeof( InternalGump ) );

				AddBackground( 100, 10, 400, 385, 0xA28 );

                AddHtml(100, 25, 400, 35, "<CENTER>SPECIAL HAIRSTYLE SELECTION MENU</CENTER>", false, false);
				AddButton( 175, 340, 0xFA5, 0xFA7, 0x0, GumpButtonType.Reply, 0 ); // CANCEL

                AddHtmlLocalized(210, 342, 90, 35, 1011012, false, false);//CANCEL
                //First Col
                AddBackground( 220, 60, 50, 50, 0xA3C );
				AddBackground( 220, 115, 50, 50, 0xA3C );
				AddBackground( 220, 170, 50, 50, 0xA3C );
				if (from.Female)
				{
					AddBackground(220, 225, 50, 50, 0xA3C);
				}
				else
				{
					AddBackground(220, 280, 50, 50, 0xA3C);
				}

                //Second Col
                AddBackground( 425, 60, 50, 50, 0xA3C );
				AddBackground( 425, 115, 50, 50, 0xA3C );
				AddBackground( 425, 170, 50, 50, 0xA3C );
				AddBackground( 425, 225, 50, 50, 0xA3C );
				AddBackground( 425, 280, 50, 50, 0xA3C );


                AddHtml(150, 75, 80, 35,  "Long Feather", false, false);
                AddHtml(150, 130, 80, 35, "Short Elf" , false, false);
                AddHtml(150, 185, 80, 35, "Mullet" , false, false);
				if (from.Female)
				{
					AddHtml(150, 240, 80, 35, "Flower", false, false);

				}
				else
				{
					AddHtml(150, 295, 80, 35, "Long Elf 2", false, false);
				}

                AddHtml(355, 75, 80, 35,  "Long Elf", false, false);
                AddHtml(355, 130, 80, 35, "Big Knob", false, false);
                AddHtml(355, 185, 80, 35, "Big Braid", false, false);
                AddHtml(355, 240, 80, 35, "Spiked", false, false);
                AddHtml(355, 295, 80, 35, "Buns", false, false);

                //First Col
                AddImage(153, 15, 60917);//LongFeatherHair
                AddImage(153, 70, 60918);//ShortElfHair
                AddImage(153, 125, 60919);//Mullet
				if (from.Female)
				{
					AddImage(153, 180, 60890);//FlowerHair
				}
				else
				{
					AddImage(153, 240, 60891);//LongElfTwoHair
				}
                //Second Col
                AddImage(358, 15, 50891);//LongElfHair
                AddImage(358, 70, 60892);//LongBigKnobHair
                AddImage(358, 120, 60893);//LongBigBraidHair
                AddImage(362, 185, 60895);//SpikedHair
                AddImage(358, 240, 60712);//BunsHair

                //First Col
				AddButton( 118, 73, 0xFA5, 0xFA7,  1, GumpButtonType.Reply, 0 );
				AddButton( 118, 128, 0xFA5, 0xFA7, 2, GumpButtonType.Reply, 0 );
				AddButton( 118, 183, 0xFA5, 0xFA7, 3, GumpButtonType.Reply, 0 );
				if (from.Female)
				{
					AddButton(118, 238, 0xFA5, 0xFA7, 4, GumpButtonType.Reply, 0);
				}
				else
				{
					AddButton(118, 292, 0xFA5, 0xFA7, 5, GumpButtonType.Reply, 0);
				}

                //Second Col
                AddButton( 323, 73, 0xFA5, 0xFA7,  6, GumpButtonType.Reply, 0 );
				AddButton( 323, 128, 0xFA5, 0xFA7, 7, GumpButtonType.Reply, 0 );
				AddButton( 323, 183, 0xFA5, 0xFA7, 8, GumpButtonType.Reply, 0 );
				AddButton( 323, 238, 0xFA5, 0xFA7, 9, GumpButtonType.Reply, 0 );
				AddButton( 323, 292, 0xFA5, 0xFA7, 10, GumpButtonType.Reply, 0 );
			}

			public override void OnResponse( NetState sender, RelayInfo info )
			{
                if (m_Deed.Deleted || m_From.Backpack == null || !m_Deed.IsChildOf(m_From.Backpack))
                    return;

				Item newHair = null;

				switch ( info.ButtonID )
				{
					case 0: return;

                    case 1: newHair = new LongFeatherHair(); break;
                    case 2: newHair = new ShortElfHair(); break;
                    case 3: newHair = new Mullet(); break;
                    case 4:
                        if (!m_From.Female) return;
                        newHair = new FlowerHair();
                        break;
                    case 5:
						if (m_From.Female) return;
						newHair = new FlowerHair();
						newHair = new LongElfTwoHair(); break;
                    case 6: newHair = new LongElfHair(); break;
                    case 7: newHair = new LongBigKnobHair(); break;
                    case 8: newHair = new LongBigBraidHair(); break;
                    case 9: newHair = new SpikedHair(); break;
                    case 10: newHair = new BunsHair(); break;
                }

				if ( m_From is PlayerMobile )
				{
					PlayerMobile pm = (PlayerMobile)m_From;

					pm.SetHairMods( -1, -1 ); // clear any hairmods (disguise kit, incognito)
				}

				Item oldHair = m_From.FindItemOnLayer( Layer.Hair );

				if ( oldHair != null )
					oldHair.Delete();

				if ( newHair != null )
				{
					if ( oldHair != null )
						newHair.Hue = oldHair.Hue;

					m_From.AddItem( newHair );
				}

				m_Deed.Delete();
			}
		}
	}
}