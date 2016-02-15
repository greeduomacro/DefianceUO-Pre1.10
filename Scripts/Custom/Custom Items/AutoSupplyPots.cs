using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class AutoSupplyPots : Item
	{
		private int m_HealPots;
		private int m_CurePots;
		private int m_RefreshPots;
		private int m_AgilityPots;
		private int m_StrengthPots;
		private int m_ExplosionPots;
		private int m_PoisionPots;
		private int m_NightSightPots;
		private int m_EmptyPots;

		[Constructable]
		public AutoSupplyPots() : base( 0x1173 )
		{
			this.Movable = false;
			this.Hue = 522; // 0x20A
			this.Name = "Potion Supply Stone";
		}

		public AutoSupplyPots( Serial serial ) : base(serial)
		{
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int HealPots
		{
			get { return m_HealPots; }
			set { m_HealPots = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int CurePots
		{
			get { return m_CurePots; }
			set { m_CurePots = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int RefreshPots
		{
			get { return m_RefreshPots; }
			set { m_RefreshPots = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int AgilityPots
		{
			get { return m_AgilityPots; }
			set { m_AgilityPots = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int StrengthPots
		{
			get { return m_StrengthPots; }
			set { m_StrengthPots = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int ExplosionPots
		{
			get { return m_ExplosionPots; }
			set { m_ExplosionPots = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int PoisionPots
		{
			get { return m_PoisionPots; }
			set { m_PoisionPots = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int NightSightPots
		{
			get { return m_NightSightPots; }
			set { m_NightSightPots = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int EmptyPots
		{
			get { return m_EmptyPots; }
			set { m_EmptyPots = value; }
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();

			switch ( version )
			{
				case 0:
				{
					m_HealPots = reader.ReadInt();
					m_CurePots = reader.ReadInt();
					m_RefreshPots = reader.ReadInt();
					m_AgilityPots = reader.ReadInt();
					m_StrengthPots = reader.ReadInt();
					m_ExplosionPots = reader.ReadInt();
					m_PoisionPots = reader.ReadInt();
					m_NightSightPots = reader.ReadInt();
					m_EmptyPots = reader.ReadInt();
					break;
				}
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

            writer.WriteEncodedInt((int)0);//version

            writer.Write((int)m_HealPots);
            writer.Write((int)m_CurePots);
            writer.Write((int)m_RefreshPots);
            writer.Write((int)m_AgilityPots);
            writer.Write((int)m_StrengthPots);
            writer.Write((int)m_ExplosionPots);
            writer.Write((int)m_PoisionPots);
            writer.Write((int)m_NightSightPots);
            writer.Write((int)m_EmptyPots);
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !from.InRange( GetWorldLocation(), 4 ) )
				//from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
                from.SendMessage("I can't reach that.");
			else
			{
				for (int i = 0; i < m_HealPots; i++)
					PackItem(from, new GreaterHealPotion());

				for (int i = 0; i < m_CurePots; i++)
					PackItem(from, new GreaterCurePotion());

				for (int i = 0; i < m_RefreshPots; i++)
					PackItem(from, new TotalRefreshPotion());

				for (int i = 0; i < m_AgilityPots; i++)
					PackItem(from, new GreaterAgilityPotion());

				for (int i = 0; i < m_StrengthPots; i++)
					PackItem(from, new GreaterStrengthPotion());

				for (int i = 0; i < m_ExplosionPots; i++)
					PackItem(from, new GreaterExplosionPotion());

				for (int i = 0; i < m_PoisionPots; i++)
					PackItem(from, new GreaterPoisonPotion());

				for (int i = 0; i < m_NightSightPots; i++)
					PackItem(from, new NightSightPotion());

				for (int i = 0; i < m_EmptyPots; i++)
					PackItem(from, new Bottle());

				from.SendMessage( "You have been given some potions." );
			}
		}

		public static void PackItem( Mobile m, Item item )
		{
			if ( !m.PlaceInBackpack( item ) )
				item.Delete();
		}
	}
}