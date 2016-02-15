using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Multis;

namespace Server.Mobiles
{
	public class Barber : BaseVendor
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos{ get { return m_SBInfos; } }

		[Constructable]
		public Barber() : base("the barber")
		{
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBBarber() );
		}

		public Barber( Serial serial ) : base( serial )
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
	}

	public class SBBarber : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBBarber()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				//Add( new GenericBuyInfo( "Special Hair Dye", typeof( SpecialHairDye ), 40, 999, 0xE26, 0 ) );
				//Add( new GenericBuyInfo( "Special Beard Dye", typeof( SpecialBeardDye ), 40, 999, 0xE26, 0 ) );

				Add( new GenericBuyInfo( "Hair Deed", typeof( HairDeed ), 25000, 100, 0x14f0, 0 ) );
				Add( new GenericBuyInfo( "Facial Hair Deed", typeof( FacialHairDeed ), 25000, 100, 0x14f0, 0 ) );

				Add( new GenericBuyInfo( "Hair Cutting Scissors", typeof( HairCuttingScissors ), 5000, 100, 0xF9F, 916 ) );
				Add( new GenericBuyInfo( "Facial Hair Cutting Scissors", typeof( FacialHairCuttingScissors ), 5000, 100, 0xF9F, 921 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
			}
		}
	}

	public class Dealer : BaseVendor
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos{ get { return m_SBInfos; } }

		[Constructable]
		public Dealer() : base("the dealer")
		{
			Item DeathShroud=new Items.DeathShroud();
			Item Shoes=new Items.Shoes();

			DeathShroud.Hue=1146;
			Shoes.Hue=1146;

			ArrayList m_items = new ArrayList( Items );
			for (int i=0; i < m_items.Count; i++)
			{
			   Item item = (Item)m_items[i];
			   if (( item != Backpack ) && ( item != BankBox ))
			      item.Delete();
			}

			AddItem(DeathShroud);
			AddItem(Shoes);
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBDrugDealer() );
		}

		public Dealer( Serial serial ) : base( serial )
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

		private static void RmvEquip( Mobile m_from, Layer layer )
		{
			if ( m_from.FindItemOnLayer( layer ) != null )
			{
				Item item = m_from.FindItemOnLayer( layer );
				item.Delete();
			}
		}
	}

	public class SBDrugDealer : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBDrugDealer()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( "Marijuana", typeof( Marijuana ), 30, 1000, 0xf88, 0x23c ) );
				Add( new GenericBuyInfo( "Opium", typeof( Opium ), 5, 100, 0xf7a, 37 ) );
				Add( new GenericBuyInfo( "Crack Rock", typeof( CrackRock ), 10, 25, 0xf8b, 0x38 ) );
				Add( new GenericBuyInfo( "LSD", typeof( LSD ), 25, 125, 0xf8e, 0x44 ) );
				Add( new GenericBuyInfo( "Large Bong", typeof( LargeBong ), 100, 10, 0x183d, 1150 ) );
				Add( new GenericBuyInfo( "Small Bong", typeof( SmallBong ), 50, 10, 0x182d, 1150 ) );
				Add( new GenericBuyInfo( "Crack Pipe", typeof( CrackPipe ), 200, 5, 0xe28, 1150 ) );
				Add( new GenericBuyInfo( "Syringe", typeof( Syringe ), 10, 100, 0xfb8, 1150 ) );
				Add( new GenericBuyInfo( "Party Pack", typeof( PartyPack ), 10, 100, 0xE76, 0x48 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
			}
		}
	}

	[CorpseName( "a glass chicken corpse" )]
	public class GlassChicken : BaseCreature
	{
		[Constructable]
		public GlassChicken() : base( AIType.AI_Animal, FightMode.Agressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a glass chicken";
			Hue = 0x4001;
			Body = 0xD0;
			BaseSoundID = 0x6E;

			SetStr( 100 );
			SetDex( 100 );
			SetInt( 100 );

			SetHits( 1 );
			SetMana( 0 );

			SetDamage( 100 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 0, 0 );
			SetResistance( ResistanceType.Energy, 0, 0 );
			SetResistance( ResistanceType.Fire, 100, 100 );
			SetResistance( ResistanceType.Cold, 100, 100 );
			SetResistance( ResistanceType.Poison, 100, 100 );

			SetSkill( SkillName.MagicResist, 50.0 );
			SetSkill( SkillName.Tactics, 120.0 );
			SetSkill( SkillName.Anatomy, 120.0 );
			SetSkill( SkillName.Wrestling, 120.0 );

			Fame = 100000;
			Karma = 0;

			VirtualArmor = 0;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 10.0;
		}

		public GlassChicken(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	[CorpseName( "a glass cat corpse" )]
	[TypeAlias( "Server.Mobiles.Housecat" )]
	public class GlassCat : BaseCreature
	{
		[Constructable]
		public GlassCat() : base( AIType.AI_Animal, FightMode.Agressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a glass cat";
			Body = 0xC9;
			Hue = 0x4001;
			BaseSoundID = 0x69;

			SetStr( 100 );
			SetDex( 100 );
			SetInt( 100 );

			SetHits( 1 );
			SetMana( 0 );

			SetDamage( 100 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 0 );
			SetResistance( ResistanceType.Energy, 0 );
			SetResistance( ResistanceType.Fire, 100 );
			SetResistance( ResistanceType.Cold, 100 );
			SetResistance( ResistanceType.Poison, 100 );

			SetSkill( SkillName.MagicResist, 50.0 );
			SetSkill( SkillName.Tactics, 120.0 );
			SetSkill( SkillName.Anatomy, 120.0 );
			SetSkill( SkillName.Wrestling, 120.0 );

			Fame = 150;
			Karma = 150;

			VirtualArmor = 0;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 10.0;
		}

		public GlassCat(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	[CorpseName( "a glass dog corpse" )]
	public class GlassDog : BaseCreature
	{
		[Constructable]
		public GlassDog() : base( AIType.AI_Animal, FightMode.Agressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a glass dog";
			Body = 0xD9;
			Hue = 0x4001;
			BaseSoundID = 0x85;

			SetStr( 100 );
			SetDex( 100 );
			SetInt( 100 );

			SetHits( 1 );
			SetMana( 0 );

			SetDamage( 100 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 0 );
			SetResistance( ResistanceType.Energy, 0 );
			SetResistance( ResistanceType.Fire, 100 );
			SetResistance( ResistanceType.Cold, 100 );
			SetResistance( ResistanceType.Poison, 100 );

			SetSkill( SkillName.MagicResist, 50.0 );
			SetSkill( SkillName.Tactics, 120.0 );
			SetSkill( SkillName.Anatomy, 120.0 );
			SetSkill( SkillName.Wrestling, 120.0 );

			Fame = 300;
			Karma = 300;

			VirtualArmor = 0;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 10.0;
		}

		public GlassDog(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

}