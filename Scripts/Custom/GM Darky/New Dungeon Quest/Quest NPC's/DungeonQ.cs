using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.ContextMenus;
using Server.Gumps;
using Server.Misc;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
	[CorpseName( "Ghastly Corpse" )]
	public class GhostOfThePast : Mobile
	{
                public virtual bool IsInvulnerable{ get{ return true; } }
		[Constructable]
		public GhostOfThePast()
		{
			Name = "Ghost of the Past";
			Body = 0x3CA;
			CantWalk = true;
			Hue = 22222;
			Hits = 1000;
		}

		public GhostOfThePast( Serial serial ) : base( serial )
		{
		}

		public override void GetContextMenuEntries( Mobile from, ArrayList list )
	        {
	                base.GetContextMenuEntries( from, list );
        	        list.Add( new GhostOfThePastEntry( from, this ) );
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

		public class GhostOfThePastEntry : ContextMenuEntry
		{
			private Mobile m_Mobile;
			private Mobile m_Giver;

			public GhostOfThePastEntry( Mobile from, Mobile giver ) : base( 6146, 3 )
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
					if ( ! mobile.HasGump( typeof( DungeonQuestGump ) ) )
					{
						mobile.SendGump( new DungeonQuestGump( mobile ));
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
				if( dropped is DarkIronWire )
         			{
         				if(dropped.Amount!=10)
         				{
         					return false;
         				}

					dropped.Delete();

					mobile.AddToBackpack( new BloodKey() );
					mobile.SendGump( new DungeonQuestGump1(m) );


					return true;
         			}
				else if ( dropped is DarkIronWire )
				{
				this.PrivateOverheadMessage( MessageType.Regular, 1153, 1054071, mobile.NetState );
         			return false;
				}
         			else
         			{
					this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "That is not what I'm seeking.", mobile.NetState );
     				}
				if( dropped is EnchantedWood )
         			{
         				if(dropped.Amount!=10)
         				{
         					return false;
         				}

					dropped.Delete();

					mobile.AddToBackpack( new WoodenKey() );
					mobile.AddToBackpack( new HeroVialBag() );
					mobile.SendGump( new DungeonQuestGump2( mobile ));


					return true;
         			}
				else if ( dropped is EnchantedWood)
				{
				this.PrivateOverheadMessage( MessageType.Regular, 1153, 1054071, mobile.NetState );
         			return false;
				}
         			else
         			{
					this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "That is not what I'm seeking.", mobile.NetState );
     				}
				if( dropped is BloodOfHeroes )
         			{
         				if(dropped.Amount!=20)
         				{
         					return false;
         				}

					dropped.Delete();

					mobile.AddToBackpack( new ShimmeringKey() );
					mobile.AddToBackpack( new StrangeKnife() );
					mobile.SendGump( new DungeonQuestGump3(m) );


					return true;
         			}
				else if ( dropped is BloodOfHeroes )
				{
				this.PrivateOverheadMessage( MessageType.Regular, 1153, 1054071, mobile.NetState );
         			return false;
				}
         			else
         			{
					this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "That is not what I'm seeking.", mobile.NetState );
     				}
				if( dropped is BeastHide )
         			{
         				if(dropped.Amount!=1)
         				{
         					return false;
         				}

					dropped.Delete();

					mobile.AddToBackpack( new ClawKey() );
					mobile.AddToBackpack( new NoxiousGemBag() );
					mobile.SendGump( new DungeonQuestGump4(m) );


					return true;
         			}
				else if ( dropped is BeastHide )
				{
				this.PrivateOverheadMessage( MessageType.Regular, 1153, 1054071, mobile.NetState );
         			return false;
				}
         			else
         			{
					this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "That is not what I'm seeking.", mobile.NetState );
     				}
				if( dropped is NoxiousEssence )
         			{
         				if(dropped.Amount!=50)
         				{
         					return false;
         				}

					dropped.Delete();

					mobile.AddToBackpack( new VenomKey() );
					mobile.AddToBackpack( new JalindeLetter() );
					mobile.SendGump( new DungeonQuestGump5(m) );


					return true;
         			}
				else if ( dropped is NoxiousEssence )
				{
				this.PrivateOverheadMessage( MessageType.Regular, 1153, 1054071, mobile.NetState );
         			return false;
				}
         			else
         			{
					this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "That is not what I'm seeking.", mobile.NetState );
     				}
			}
		return false;
		}
	}
}