using System;
using Server;
using Server.Multis;
using Server.Gumps;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	public class WerewolfStatue : Item
	{
		private bool m_TurnedOn;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool TurnedOn
		{
			get{ return m_TurnedOn; }
			set{ m_TurnedOn = value; InvalidateProperties(); }
		}

		[Constructable]
		public WerewolfStatue() : this( 0x2770 )
		{
		}

		[Constructable]
		public WerewolfStatue( int itemID ) : base( itemID )
		{
			Movable = true;
			Weight = 5;
			Name = "a werewolf statue";
			Hue = 2312;
		}

		public override bool HandlesOnMovement{ get{ return m_TurnedOn && IsLockedDown; } }

		private static string[] m_Howls = new string[]{"I shall split thy blood!","Get lost human!","AaRrrrooooooooo!"};

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if ( m_TurnedOn && IsLockedDown && (!m.Hidden || m.AccessLevel == AccessLevel.Player) && Utility.InRange( m.Location, this.Location, 2 ) && !Utility.InRange( oldLocation, this.Location, 2 ) )
			{
				if ( 25 > Utility.Random( 100 ) )
				{
					PublicOverheadMessage( MessageType.Regular, Utility.RandomDyedHue(), false, (m_Howls[Utility.Random(m_Howls.Length)] ) );
					Effects.PlaySound( this.Location, this.Map, Utility.RandomList( 105, 106 ) );
				}
			}

			base.OnMovement( m, oldLocation );
		}

		public WerewolfStatue( Serial serial ) : base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_TurnedOn )
				list.Add( 502695 ); // turned on
			else
				list.Add( 502696 ); // turned off
		}

		public bool IsOwner( Mobile mob )
		{
			BaseHouse house = BaseHouse.FindHouseAt( this );

			return ( house != null && house.IsOwner( mob ) );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsOwner( from ) )
			{
				OnOffGump onOffGump = new OnOffGump( this );
				from.SendGump( onOffGump );
			}
			else
			{
				from.SendLocalizedMessage( 502691 ); // You must be the owner to use this.
			}
		}

		public override void OnSingleClick( Mobile from )
		{
			if ( IsLockedDown )
			{
				string[] touch = {"I shall split thy blood!","Get lost human!","AaRrrrooooooooo!"};
				if ( 25 > Utility.Random( 100 ) )
				{
					PublicOverheadMessage( MessageType.Regular, Utility.RandomDyedHue(), false, (touch[Utility.Random(5)] ) );
					Effects.PlaySound( this.Location, this.Map, Utility.RandomList( 107, 108, 109 ) );;
				}
			}
			base.OnSingleClick(from);
		}

		private class OnOffGump : Gump
		{
			private WerewolfStatue m_Statuette;

			public OnOffGump( WerewolfStatue statuette ) : base( 150, 200 )
			{
				m_Statuette = statuette;

				AddBackground( 0, 0, 300, 150, 0xA28 );

				AddHtmlLocalized( 45, 20, 300, 35, statuette.TurnedOn ? 1011035 : 1011034, false, false ); // [De]Activate this item

				AddButton( 40, 53, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 80, 55, 65, 35, 1011036, false, false ); // OKAY

				AddButton( 150, 53, 0xFA5, 0xFA7, 0, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 190, 55, 100, 35, 1011012, false, false ); // CANCEL
			}

			public override void OnResponse( NetState sender, RelayInfo info )
			{
				Mobile from = sender.Mobile;

				if ( info.ButtonID == 1 )
				{
					bool newValue = !m_Statuette.TurnedOn;
					m_Statuette.TurnedOn = newValue;

					if ( newValue && !m_Statuette.IsLockedDown )
						from.SendLocalizedMessage( 502693 ); // Remember, this only works when locked down.
				}
				else
				{
					from.SendLocalizedMessage( 502694 ); // Cancelled action.
				}
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version

			writer.Write( (bool) m_TurnedOn );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();

			switch ( version )
			{
				case 0:
				{
					m_TurnedOn = reader.ReadBool();
					break;
				}
			}
		}
	}
}