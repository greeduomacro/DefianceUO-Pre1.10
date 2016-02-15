// created on 27/6/2003 at 22:11
// By raistlin
using System;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Multis;
using Server.Targeting;

namespace Server.Items
{
	public class FlamingHead : BaseTrap
	{
		//[Constructable]
		public FlamingHead() : base( 0x10FC )
		{
			Movable = false;
			Name = "a flaming head";
		}

		public override bool PassivelyTriggered{ get{ return true; } }
		public override TimeSpan PassiveTriggerDelay{ get{ return TimeSpan.FromSeconds( 3.0 ); } }
		public override int PassiveTriggerRange{ get{ return 1; } }
		public override TimeSpan ResetDelay{ get{ return TimeSpan.FromSeconds( 2.0 ); } }

		public override void OnTrigger( Mobile from )
		{
			Effects.SendLocationParticles( EffectItem.Create( Location, Map, EffectItem.DefaultDuration ), 0x10FC, 10, 30, 5052 );
			Effects.PlaySound( Location, Map, 0x227 );
		}

		public FlamingHead( Serial serial ) : base( serial )
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
			BaseHouse house = BaseHouse.FindHouseAt( this );
			if ( !( house == null || !house.IsOwner( from ) || !house.IsCoOwner( from ) ) && !from.HasGump( typeof(FlameCloseGump) ) )
			{
				from.SendGump( new FlameCloseGump( this ) );
			}
		}
	}

	public class FlamingHeadDeed : Item
	{
		//[Constructable]
		public FlamingHeadDeed() : base(0x14F0)
		{
			Name = "a flaming head deed";
			LootType = LootType.Blessed;
			Weight = 1.0;
		}

		public FlamingHeadDeed( Serial serial ) : base( serial )
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

		public override void OnDoubleClick( Mobile from ) // Override double click of the deed to call our target
		{
			if ( !IsChildOf( from.Backpack ) ) // Make sure its in their pack
			{
				 from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else
			{
				from.SendMessage( "Where would you like to place this flaming head?" );
				from.Target = new FlamingHeadTarget( this ); // Call our target
			 }
		}
		private class FlamingHeadTarget : Target
		{
			private Item m_Deed;
			public FlamingHeadTarget( Item item ) : base( -1, true, TargetFlags.None)
			{
				m_Deed = item;

				CheckLOS = false;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				IPoint3D p = targeted as IPoint3D;

				if ( p == null || m_Deed.Deleted )
					return;
				if ( m_Deed.IsChildOf( from.Backpack ) )
				{
					BaseHouse house = BaseHouse.FindHouseAt( from );

					if ( !( house == null || !house.IsOwner( from ) || !house.IsCoOwner( from ) ) )
					{
						Item flame = new FlamingHead();
						flame.Map = from.Map;
						flame.Location = new Point3D( p );
						m_Deed.Delete();
					}
					else
						from.SendLocalizedMessage( 500274 ); // You can only place this in a house that you own!
				}
				else
					from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
		}
	}
}

namespace Server.Gumps
{
	public class FlameCloseGump : Gump
	{
		private Item m_Item;

		public void AddButtonLabel( int x, int y, int buttonID, string text )
		{
			AddButton( x, y - 1, 4005, 4007, buttonID, GumpButtonType.Reply, 0 );
			AddLabel( x + 35, y, 0, text );
		}

		public FlameCloseGump( Item item ) : base(0,0)
		{
			m_Item = item;
			Closable = false;
			Dragable = true;

			AddPage(0);

			AddBackground( 0, 0, 215, 180, 5054);
			AddBackground( 10, 10, 195, 160, 3000);
			AddLabel( 20, 40, 0, "Do you wish to re-deed this");
			AddLabel( 20, 60, 0, "flaming head?");
			AddButtonLabel( 20, 110, 1, "CONTINUE" );
			AddButtonLabel( 20, 135, 0, "CANCEL" );
		}

		public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
		{
			if (info.ButtonID == 1)
			{
				m_Item.Delete();
				sender.Mobile.AddToBackpack( new FlamingHeadDeed() );
			}
		}
	}
}