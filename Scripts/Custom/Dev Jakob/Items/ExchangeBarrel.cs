using System;
using System.Collections;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Regions;

namespace Server.Items
{
	public class ExchangeBarrel : Barrel
	{
		Type m_TurnInType, m_TurnOutType;
		int m_TurnInAmount, m_TurnOutAmount;

		[CommandProperty(AccessLevel.GameMaster)]
		public Type TurnInType
		{
			get { return m_TurnInType; }
			set { m_TurnInType = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public Type TurnOutType
		{
			get { return m_TurnOutType; }
			set { m_TurnOutType = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int TurnInAmount
		{
			get { return m_TurnInAmount; }
			set { m_TurnInAmount = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int TurnOutAmount
		{
			get { return m_TurnOutAmount; }
			set { m_TurnOutAmount = value; }
		}

		[Constructable]
		public ExchangeBarrel() : base()
		{
			Hue = 2207;
			Movable = false;
			Name = "an exchange barrel";
			m_TurnInType = typeof( Gold );
			m_TurnOutType = typeof( Gold );
			m_TurnInAmount = 1;
			m_TurnOutAmount = 1;
		}

		public ExchangeBarrel( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );

			writer.Write( m_TurnInType.ToString() );
			writer.Write( m_TurnOutType.ToString() );
			writer.Write( m_TurnInAmount );
			writer.Write( m_TurnOutAmount );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_TurnInType = ScriptCompiler.FindTypeByFullName(reader.ReadString());
					m_TurnOutType = ScriptCompiler.FindTypeByFullName(reader.ReadString());
					m_TurnInAmount = reader.ReadInt();
					m_TurnOutAmount = reader.ReadInt();
					break;
				}
			}
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			if ( !(dropped is Container) )
			{
				from.SendMessage( "Put the items in a bag and drop the bag here!" );

			}
			else
			{
				int c = 0;
				foreach( Item item in ((Container)dropped).Items )
					if ( item.GetType() == m_TurnInType )
						c++;
				if ( c >= m_TurnInAmount )
				{
					int deleted = 0;
					for ( int i = 0; i < ((Container)dropped).Items.Count; i++ )
					{
						Item item = ((Container)dropped).Items[i] as Item;
						if ( item.GetType() == m_TurnInType )
						{
							item.Delete();
							deleted++;
							i--;
							if ( deleted >= m_TurnInAmount )
							{
								for ( int j = 0; j < m_TurnOutAmount; j++ )
								{
									object o = Activator.CreateInstance( m_TurnOutType );
									Item newitem = (Item)o;
									if ( newitem != null )
									{
										((Container)dropped).Items.Add( newitem );
									}
								}

								from.SendMessage( "The items in the bag turn into something else..." );

								break;
							}
						}
					}
				}
				else
				{
					from.SendMessage( "There's not enough items of the correct type in the bag." );
				}
			}
			return false;
		}

		public override bool OnDragDropInto( Mobile from, Item item, Point3D p )
		{
			return OnDragDrop( from, item );
		}
	}
}