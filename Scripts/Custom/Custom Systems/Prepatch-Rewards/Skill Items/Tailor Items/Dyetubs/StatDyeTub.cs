using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Misc;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
	public interface IDyableS
	{
		bool DyeS( Mobile from, StatDyeTub sender );
	}


	public class StatDyeTub : Item
	{
		private bool m_Redyable;
		private int m_DyedHue;

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (bool) m_Redyable );
			writer.Write( (int) m_DyedHue );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_Redyable = reader.ReadBool();
					m_DyedHue = reader.ReadInt();

					break;
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Redyable
		{
			get
			{
				return m_Redyable;
			}
			set
			{
				m_Redyable = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int DyedHue
		{
			get
			{
				return m_DyedHue;
			}
			set
			{
				if ( m_Redyable )
				{
					m_DyedHue = value;
					Hue = value;
				}
			}
		}

		[Constructable]
		public StatDyeTub() : base( 0xFAB )
		{
			Weight = 10.0;
			m_Redyable = true;
			LootType = LootType.Newbied;
			Name = "Reward Statuette Dye Tub";
		}

		public StatDyeTub( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.InRange( this.GetWorldLocation(), 1 ) )
			{
				from.SendMessage( "Select the statuette to dye." );
				from.Target = new InternalTarget( this );
			}
			else
			{
				from.SendLocalizedMessage( 500446 ); // That is too far away.
			}
		}

		private class InternalTarget : Target
		{
			private StatDyeTub m_STub;

			public InternalTarget( StatDyeTub tub ) : base( 1, false, TargetFlags.None )
			{
				m_STub = tub;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is IDyableS && targeted is Item )
				{
					if ( !from.InRange( m_STub.GetWorldLocation(), 1 ) || !from.InRange( ((Item)targeted).GetWorldLocation(), 1 ) )
					{
						from.SendLocalizedMessage( 500446 ); // That is too far away.
					}
					else if ( ((Item)targeted).Parent is Mobile )
					{
						from.SendMessage( "You cannot dye that in it's current location." );
					}
					else if ( ((IDyableS)targeted).DyeS( from, m_STub ) )
					{
						from.PlaySound( 0x23E );
					}
				}
				else
				{
					from.SendLocalizedMessage( 1042083 ); // You can not dye that.
				}
			}
		}
	}

	public class StatDyeTubGump : Gump
	{
		private HueList[] m_Lists;
		public StatDyeTub m_Tub;

		public StatDyeTubGump( StatDyeTub tub ) : base( 50,50 )
		{
        	m_Tub = tub;
			HueList[] checkLists;

			checkLists = HueList.HueLists;

			m_Lists = new HueList[checkLists.Length];

			for ( int i = 0; i < m_Lists.Length; ++i )
				m_Lists[i] = checkLists[i];

			AddPage( 0 );
			AddBackground( 0, 0, 450, 450, 5054 );	// Outer gump border
			AddBackground( 10, 10, 430, 430, 3000 );	// Gump background

			AddButton( 20, 400, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0 );
			AddLabel( 55, 400, 0x00, "OKAY" );

			AddButton( 200, 400, 0xFA5, 0xFA7, 2, GumpButtonType.Reply, 0 );
			AddLabel( 235, 400, 0x00, "DEFAULT" );

			for ( int i = 0; i < checkLists.Length; ++i )
			{
				AddButton( 30, 85 + (i * 25), 0x1468, 0x1468, 0, GumpButtonType.Page, Array.IndexOf( m_Lists, checkLists[i] ) + 1 );
				AddLabel( 55, 85 + (i * 25), 0, checkLists[i].HueGroup );
			}

			for ( int i = 0; i < m_Lists.Length; ++i )
				RenderPage( i, Array.IndexOf( checkLists, m_Lists[i] ) );
		}

		private void RenderPage( int index, int offset )
		{
			HueList list = m_Lists[index];

			AddPage( index + 1 );

			AddButton( 30, 85 + (offset * 25), 0x1468, 0x1468, 0, GumpButtonType.Page, index + 1 );
			AddLabel( 55, 85 + (offset * 25), 0, list.HueGroup );

			HueEntry[] entries = list.Entries;

			for ( int i = 0; i < entries.Length; ++i )
			{
				AddRadio( 255, 90 + (i * 25), 210, 211, false, (index * 100) + i );
				AddLabel( 275, 90 + (i * 25), entries[i].Hue, "*****" );
				//AddItem( 315, 88 + (i * 25), 7609, entries[i].Hue);
			}
		}


      public override void OnResponse( NetState state, RelayInfo info ) //Function for GumpButtonType.Reply Buttons
      {
	      Mobile from = state.Mobile;

			if ( info.ButtonID == 0 ) // Cancel
				return;

			if ( info.ButtonID == 2 ) // Default color
			{
				Effects.PlaySound( from.Location, from.Map, 0x23E); // Sound 574
				m_Tub.Hue = 0x00;
				m_Tub.DyedHue = 0x00;
				return;
			}

			int[] switches = info.Switches;

			if ( switches.Length == 0 )
				return;

			int switchID = switches[0];
			int listIndex = switchID / 100;
			int listEntry = switchID % 100;

			if ( listIndex < 0 || listIndex >= m_Lists.Length )
				return;

			HueList list = m_Lists[listIndex];

			if ( listEntry < 0 || listEntry >= list.Entries.Length )
				return;

			HueEntry entry = list.Entries[listEntry];

			m_Tub.Hue = entry.Hue + 1;	// Add one to 0-base for proper hue
			m_Tub.DyedHue = entry.Hue + 1;
			Effects.PlaySound( from.Location, from.Map, 0x23E); // Sound 574: The dyetub
			return;
		}
	}
}