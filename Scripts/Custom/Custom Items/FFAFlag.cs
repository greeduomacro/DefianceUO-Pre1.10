using System;
using Server;

namespace Server.Items
{
	public class FFAFlag : Item
	{
		private Mobile m_Owner;
		private Item m_Hat;

		[Constructable]
		public FFAFlag() : base( 0x1627 )
		{
			Weight = 7.0;
			//Movable = false;
			Hue = 1153;
			Name = "FFA Flag";
		}

		public FFAFlag( Serial serial ) : base( serial )
		{
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner{ get{ return m_Owner; } set{ m_Owner = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public Item Hat{ get{ return m_Hat; } set{ m_Hat = value; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );

			writer.Write( m_Owner );
			writer.Write( m_Hat );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_Owner = reader.ReadMobile();
					m_Hat = reader.ReadItem();
					break;
				}
			}
		}

		public override void OnAdded( object parent )
		{
			if (m_Hat != null)
			{
				m_Hat.Delete();
				m_Hat = null;
			}

			if (RootParent is Mobile)
			{
				m_Hat = new WizardsHat();
				m_Hat.Hue = 1153;
				m_Hat.Movable = false;
				m_Hat.Name = "a white magic hat";
				m_Owner = ((Mobile)RootParent);
				m_Owner.AddItem( m_Hat );
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			if (!(RootParent is Mobile) && (from.InRange( this.GetWorldLocation(), 2 ) || VerifyMove( from )) && from.Backpack.FindItemByType( typeof(FFAFlag), true) == null)
				from.AddToBackpack( this );
			else
				from.SendMessage("You already possess a flag");
		}

		public override void Delete()
		{
			if (m_Hat != null)
				m_Hat.Delete();
			base.Delete();
		}

		public override DeathMoveResult OnInventoryDeath( Mobile parent )
		{
			if (!VerifyMove(parent))
			{
				this.Delete();
				new FFAFlag().MoveToWorld( parent.Location, parent.Map);
			}
			return base.OnInventoryDeath( parent );
		}

		public override bool VerifyMove( Mobile from )
		{
			return ( from.AccessLevel >= AccessLevel.GameMaster );
		}

		public override bool Decays{ get{ return false; } }
	}
}