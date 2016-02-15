using System;
using System.Collections;
using Server;
using Server.Network;
using Server.Targeting;
using Server.ContextMenus;

namespace Server.Items
{
	public class HalloweenStatue : Item
	{
		private const int m_Partial = 2;
		private const int m_Completed = 30;

		private int m_Quantity;

		[CommandProperty( AccessLevel.GameMaster )]
		public int Quantity
		{
			get { return m_Quantity; }
			set
			{
				if ( value <= 1 )
					m_Quantity = 1;
				else if ( value >= m_Completed )
					m_Quantity = m_Completed;
				else
					m_Quantity = value;

				if ( m_Quantity < m_Partial )
					ItemID = 0xF8B;
				else if ( m_Quantity < m_Completed )
					ItemID = 0x1364;
				else
					ItemID = 0x3B0F;

				InvalidateProperties();
			}
		}

		[Constructable]
		public HalloweenStatue() : base( 0xF8B )
		{
			Hue = 0x846;

			m_Quantity = 1;
		}

		public override void OnSingleClick( Mobile from )
		{
			if ( m_Quantity < m_Partial )
				LabelTo( from, "a piece of a purified statue" );
			else if ( m_Quantity < m_Completed )
				LabelTo( from, "an intermediate product of purified statue");
			else
				LabelTo( from, "<page in for a GM to insert your name>, Purifier of Evil");
		}

		public override void GetContextMenuEntries( Mobile from, ArrayList list )
		{
			base.GetContextMenuEntries( from, list );

			if ( from.Alive && m_Quantity >= m_Partial && m_Quantity < m_Completed && IsChildOf( from.Backpack ) )
				list.Add( new DisassembleEntry( this ) );
		}

		private class DisassembleEntry : ContextMenuEntry
		{
			private HalloweenStatue m_HalloweenStatue;

			public DisassembleEntry( HalloweenStatue halloweenstatue ) : base( 6142 )
			{
				m_HalloweenStatue = halloweenstatue;
			}

			public override void OnClick()
			{
				Mobile from = Owner.From;
				if ( !m_HalloweenStatue.Deleted && m_HalloweenStatue.Quantity >= HalloweenStatue.m_Partial && m_HalloweenStatue.Quantity < HalloweenStatue.m_Completed && m_HalloweenStatue.IsChildOf( from.Backpack ) && from.CheckAlive() )
				{
					for ( int i = 0; i < m_HalloweenStatue.Quantity - 1; i++ )
						from.AddToBackpack( new HalloweenStatue() );

					m_HalloweenStatue.Quantity = 1;
				}
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( m_Quantity < m_Completed )
			{
				if ( !IsChildOf( from.Backpack ) )
					from.Send( new MessageLocalized( Serial, ItemID, MessageType.Regular, 0x2C, 3, 500309, "", "" ) ); // Nothing Happens.
				else
					from.Target = new InternalTarget( this );
			}
		}

		private class InternalTarget : Target
		{
			private HalloweenStatue m_HalloweenStatue;

			public InternalTarget( HalloweenStatue halloweenstatue ) : base( -1, false, TargetFlags.None )
			{
				m_HalloweenStatue = halloweenstatue;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				Item targ = targeted as Item;
				if ( m_HalloweenStatue.Deleted || m_HalloweenStatue.Quantity >= HalloweenStatue.m_Completed || targ == null )
					return;

				if ( m_HalloweenStatue.IsChildOf( from.Backpack ) && targ.IsChildOf( from.Backpack ) && targ is HalloweenStatue && targ != m_HalloweenStatue )
				{
					HalloweenStatue targHalloweenStatue = (HalloweenStatue)targ;
					if ( targHalloweenStatue.Quantity < HalloweenStatue.m_Completed )
					{
						if ( targHalloweenStatue.Quantity + m_HalloweenStatue.Quantity <= HalloweenStatue.m_Completed )
						{
							targHalloweenStatue.Quantity += m_HalloweenStatue.Quantity;
							m_HalloweenStatue.Delete();
						}
						else
						{
							int delta = HalloweenStatue.m_Completed - targHalloweenStatue.Quantity;
							targHalloweenStatue.Quantity += delta;
							m_HalloweenStatue.Quantity -= delta;
						}

						from.Send( new AsciiMessage( targHalloweenStatue.Serial, targHalloweenStatue.ItemID, MessageType.Regular, 0x59, 3, m_HalloweenStatue.Name, "Something Happened." ) );

						return;
					}
				}

				from.Send( new MessageLocalized( m_HalloweenStatue.Serial, m_HalloweenStatue.ItemID, MessageType.Regular, 0x2C, 3, 500309, m_HalloweenStatue.Name, "" ) ); // Nothing Happens.
			}
		}

		public HalloweenStatue( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.WriteEncodedInt( m_Quantity );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_Quantity = reader.ReadEncodedInt();
		}
	}
}