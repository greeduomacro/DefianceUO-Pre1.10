using System;
using Server;

namespace Server.Items
{

	public class PhoenixGorget : StuddedGorget
	{
		public override int InitMinHits{ get{ return 9999; } }
		public override int InitMaxHits{ get{ return 9999; } }

		public override int LabelNumber{ get{ return 1041604; } } //studded gorget of the phoenix

        private bool m_IsRewardItem;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsRewardItem
		{
			get { return m_IsRewardItem; }
			set { m_IsRewardItem = value; }
		}

		[Constructable]
		public PhoenixGorget() : base()
		{
			Hue = 1358;
			LootType = LootType.Blessed;
			ProtectionLevel = ArmorProtectionLevel.Defense;
			m_IsRewardItem = true;
		}

		public PhoenixGorget( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int)0 ); // version
			writer.Write( m_IsRewardItem );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
			m_IsRewardItem = reader.ReadBool();
		}
	}

	public class PhoenixGloves : RingmailGloves
	{

		public override int InitMinHits{ get{ return 9999; } }
		public override int InitMaxHits{ get{ return 9999; } }

		public override int LabelNumber{ get{ return 1041605; } } //ringmail gloves of the phoenix

        private bool m_IsRewardItem;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsRewardItem
		{
			get { return m_IsRewardItem; }
			set { m_IsRewardItem = value; }
		}

		[Constructable]
		public PhoenixGloves() : base()
		{
			Hue = 1358;
			LootType = LootType.Blessed;
			ProtectionLevel = ArmorProtectionLevel.Defense;
			m_IsRewardItem = true;
		}

		public PhoenixGloves( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int)0 ); // version
			writer.Write( m_IsRewardItem );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
			m_IsRewardItem = reader.ReadBool();
		}
	}

	public class PhoenixTunic : RingmailChest
	{

		public override int InitMinHits{ get{ return 9999; } }
		public override int InitMaxHits{ get{ return 9999; } }

		public override int LabelNumber{ get{ return 1041606; } } //ringmail tunic of the phoenix

        private bool m_IsRewardItem;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsRewardItem
		{
			get { return m_IsRewardItem; }
			set { m_IsRewardItem = value; }
		}

		[Constructable]
		public PhoenixTunic() : base()
		{
			Hue = 1358;
			LootType = LootType.Blessed;
			ProtectionLevel = ArmorProtectionLevel.Defense;
			m_IsRewardItem = true;
		}

		public PhoenixTunic( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int)0 ); // version
			writer.Write( m_IsRewardItem );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
			m_IsRewardItem = reader.ReadBool();
		}
	}

	public class PhoenixSleeves : RingmailArms
	{

		public override int InitMinHits{ get{ return 9999; } }
		public override int InitMaxHits{ get{ return 9999; } }

		public override int LabelNumber{ get{ return 1041607; } } //ringmail sleeves of the phoenix

        private bool m_IsRewardItem;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsRewardItem
		{
			get { return m_IsRewardItem; }
			set { m_IsRewardItem = value; }
		}

		[Constructable]
		public PhoenixSleeves() : base()
		{
			Hue = 1358;
			LootType = LootType.Blessed;
			ProtectionLevel = ArmorProtectionLevel.Defense;
			m_IsRewardItem = true;
		}

		public PhoenixSleeves( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int)0 ); // version
			writer.Write( m_IsRewardItem );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
			m_IsRewardItem = reader.ReadBool();
		}
	}

	public class PhoenixLegs : RingmailLegs
	{

		public override int InitMinHits{ get{ return 9999; } }
		public override int InitMaxHits{ get{ return 9999; } }

		public override int LabelNumber{ get{ return 1041608; } } //ringmail leggings of the phoenix

        private bool m_IsRewardItem;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsRewardItem
		{
			get { return m_IsRewardItem; }
			set { m_IsRewardItem = value; }
		}

		[Constructable]
		public PhoenixLegs() : base()
		{
			Hue = 1358;
			LootType = LootType.Blessed;
			ProtectionLevel = ArmorProtectionLevel.Defense;
			m_IsRewardItem = true;
		}

		public PhoenixLegs( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int)0 ); // version
			writer.Write( m_IsRewardItem );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
			m_IsRewardItem = reader.ReadBool();
		}
	}

	public class PhoenixHelm : NorseHelm
	{

		public override int InitMinHits{ get{ return 9999; } }
		public override int InitMaxHits{ get{ return 9999; } }

		public override int LabelNumber{ get{ return 1041609; } } //norse helm of the phoenix

        private bool m_IsRewardItem;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsRewardItem
		{
			get { return m_IsRewardItem; }
			set { m_IsRewardItem = value; }
		}

		[Constructable]
		public PhoenixHelm() : base()
		{
			Hue = 1358;
			LootType = LootType.Blessed;
			ProtectionLevel = ArmorProtectionLevel.Defense;
			m_IsRewardItem = true;
		}

		public PhoenixHelm( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int)0 ); // version
			writer.Write( m_IsRewardItem );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
			m_IsRewardItem = reader.ReadBool();
		}
	}


	public class PhoenixTicket : Item
	{
		public override int LabelNumber{ get{ return 1041234; } } //Ticket for a piece of phoenix armor

        private bool m_IsRewardItem;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsRewardItem
		{
			get { return m_IsRewardItem; }
			set { m_IsRewardItem = value; }
		}

		[Constructable]
		public PhoenixTicket() : base( 0x14EF )
		{
			Hue = 1358;
			Weight = 1.0;
			LootType = LootType.Blessed;
			m_IsRewardItem = true;
		}

		//public override bool DisplayLootType{ get{ return false; } }

		public PhoenixTicket( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version
			writer.Write( m_IsRewardItem );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
			m_IsRewardItem = reader.ReadBool();
		}

				public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			else if ( PlaceArmor( from ) )
				from.SendLocalizedMessage( 500720 );
			else
				Delete();
		}

		public bool PlaceArmor( Mobile to )
		{
			//return false if failed to place
			Item item = Activator.CreateInstance(m_PhoenixList[Utility.Random( m_PhoenixList.Length )]) as Item;
			return item == null || !to.PlaceInBackpack( item );
		}

		private static Type[] m_PhoenixList = new Type[]
			{
				typeof( PhoenixGorget ),	typeof( PhoenixGloves ),	typeof( PhoenixTunic ),
				typeof( PhoenixSleeves ),	typeof( PhoenixLegs ),		typeof( PhoenixHelm )
			};
	}
}