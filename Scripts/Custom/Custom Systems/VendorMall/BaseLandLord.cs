using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Network;
using Server.Multis;
using Server.Targeting;
using Server.Accounting;
using Server.Misc;
using Server.Gumps;

namespace Server.Mobiles
{
	public abstract class BaseLandLord : BaseVendor
	{

		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos{ get { return m_SBInfos; } }


		private int m_RentCost = 250;

		[CommandProperty(AccessLevel.GameMaster)]
		public int RentCost
		{
			get { return m_RentCost;}
			set { m_RentCost = value;}
		}


		public BaseHouse m_Godhouse;

	// set the house by double clicking the landlord and then targetting a house sign
	//	[CommandProperty(AccessLevel.GameMaster)]
		public BaseHouse Godhouse
		{
			get { return m_Godhouse;}
			set { m_Godhouse = value;}
		}


		public override void OnDoubleClick( Mobile from )
		{
			if ( from.AccessLevel >= AccessLevel.GameMaster )
			{
				from.SendMessage( "Target the house sign of the house to assign this landlord to." );
				from.Target = new InternalTarget( this );
			}
			else
				base.OnDoubleClick( from );
		}


		public override void InitSBInfo()
		{
		}

		public static void GetHouseName( BaseHouse house )
		{  // this doesn't seem to do anything. Nothing else seems to use it.
			BaseHouse godhouse = house;
		}

		public virtual void SayPriceTo( Mobile m )
		{
			SayTo( m, String.Format( "The rent payment is {0} gold pieces.", RentCost ) );
		}

		public override bool HandlesOnSpeech( Mobile from )
		{
			if ( from.InRange( this.Location, 4 ) )
				return true;

			return base.HandlesOnSpeech( from );
		}

		public override void OnSpeech( SpeechEventArgs e )
		{
			Mobile from = e.Mobile;

			if ( !e.Handled && from is PlayerMobile && from.InRange( this.Location, 4 ))
			{
				if ( e.HasKeyword( 0x0004 ) ) // *join*
				{
					SayPriceTo( from );

					e.Handled = true;
				}
			}

			base.OnSpeech( e );
		}

		public override bool OnGoldGiven( Mobile from, Gold dropped )
		{
			if ( m_Godhouse == null )
			{
				from.SendMessage( "No house has been assigned yet." );
				return false;
			}

			if ( from is PlayerMobile && dropped.Amount == RentCost )
			{
				if ( m_Godhouse.IsFriend( from )  )
				{
					SayTo( from, "You are already a friend of this house!" );
					return false;
				}

				SayTo( from, "You have now rented a spot for your vendor. Good luck!" );

				m_Godhouse.AddFriend( m_Godhouse.Owner, from );

				return true;
			}
			else if ( dropped.Amount > RentCost )
			{
				SayTo( from, "That's too much!" );
				SayPriceTo( from );
				return false;
			}
			else
			{
				SayTo( from, "You didn't pay me enough gold!" );
				SayPriceTo( from );
				return false;
			}

			return base.OnGoldGiven( from, dropped );
		}

		public BaseLandLord( string title ) : base( title )
		{
			Title = String.Format( "the {0} ", title );
		}

		public BaseLandLord( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( (int) m_RentCost);
			writer.Write( (BaseHouse) m_Godhouse);

		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				{
					m_RentCost = reader.ReadInt();
					goto case 0;
					break;
				}

				case 0:
				{
					m_Godhouse = reader.ReadItem() as BaseHouse;
					break;
				}
			}
		}

		private class InternalTarget : Target
		{
			private BaseLandLord m_landlord;

			public InternalTarget( BaseLandLord landlord ) : base( 30, false, TargetFlags.None ) // range, allowground, flags
			{
				m_landlord = landlord;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{

				if ( targeted is HouseSign )
				{
					HouseSign sign = (HouseSign) targeted;

					if ( sign.Owner == null )
					{
						from.SendMessage( "That house sign does not seem to have a house associated with it." );
					}
					else
					{
						m_landlord.Godhouse = sign.Owner;
						from.SendMessage( "House assigned successfully." );
					}

				}
				else
					from.SendMessage( "That does not appear to be a house sign." );
			}
		}
	}
}