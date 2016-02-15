using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.ContextMenus;
using Server.Gumps;
using Server.Misc;
using Server.Network;
using Server.Spells;
using Server.Engines.Quests.Doom;

namespace Server.Mobiles
{
	[CorpseName( "Ciiss' Corpse" )]
	public class Ciiss : Mobile
	{
                public virtual bool IsInvulnerable{ get{ return true; } }
		[Constructable]
		public Ciiss()
		{
			Name = "Ciiss";
                        Title = "the Snake Worshipper";
			Body = 401;
			CantWalk = true;
			Hue = 2006;

			Item Boots = new Boots();
			Boots.Movable=false;
			Boots.Hue=1175;
			EquipItem( Boots );

                        Item HoodedShroudOfShadows = new HoodedShroudOfShadows();
			HoodedShroudOfShadows.Movable=false;
			HoodedShroudOfShadows.Hue=1175;
			EquipItem( HoodedShroudOfShadows );

                        Item LeatherGloves = new LeatherGloves();
			LeatherGloves.Movable=false;
			LeatherGloves.Hue=1175;
                        EquipItem( LeatherGloves );

			int hairHue = 1741;
			AddItem( new LongHair( hairHue ) );
			Blessed = true;
		}



		public Ciiss( Serial serial ) : base( serial )
		{
		}

		public override void GetContextMenuEntries( Mobile from, ArrayList list )
	        {
	                base.GetContextMenuEntries( from, list );
        	        list.Add( new CiissEntry( from, this ) );
	        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}

		public class CiissEntry : ContextMenuEntry
		{
			private Mobile m_Mobile;
			private Mobile m_Giver;

			public CiissEntry( Mobile from, Mobile giver ) : base( 6146, 3 )
			{
				m_Mobile = from;
				m_Giver = giver;
			}

			public override void OnClick()
			{


                          if( !( m_Mobile is PlayerMobile ) )
					return;

				PlayerMobile mobile = (PlayerMobile) m_Mobile;

				{
					if ( ! mobile.HasGump( typeof( CiissGump ) ) )
					{
						mobile.SendGump( new CiissGump( mobile ));

					}
				}
			}
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
         	        Mobile m = from;
			PlayerMobile mobile = m as PlayerMobile;

			if ( mobile != null)
			{
				if( dropped is OphidianStaff )
         		{
         			if(dropped.Amount!=1)
         			{
					this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "Noo.., thatssss not it...", mobile.NetState );
         				return false;
         			}

					dropped.Delete();
					mobile.AddToBackpack( new OrangePetals( 15 ) );
					mobile.AddToBackpack( new BankCheck( 25000 ));
					mobile.SendGump( new CiissFinishGump());



					return true;
         		}
				else if ( dropped is OphidianStaff)
				{
				this.PrivateOverheadMessage( MessageType.Regular, 1153, 1054071, mobile.NetState );
         			return false;
				}
         		else
         		{
					this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "I have no need of thissss!", mobile.NetState );
     			}
			}
			return false;
		}
	}
}