using System;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	public class PaladinSword : BaseSword
	{
		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.CrushingBlow; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ParalyzingBlow; } }

		public override int AosStrengthReq{ get{ return 55; } }
		public override int AosMinDamage{ get{ return 22; } }
		public override int AosMaxDamage{ get{ return 31; } }
		public override int AosSpeed{ get{ return 18; } }

		public override int OldStrengthReq{ get{ return 40; } }
		public override int OldMinDamage{ get{ return 6; } }
		public override int OldMaxDamage{ get{ return 34; } }
		public override int OldSpeed{ get{ return 30; } }

		public override int DefHitSound{ get{ return 0x237; } }
		public override int DefMissSound{ get{ return 0x23A; } }

		[Constructable]
		public PaladinSword() : base( 0x26CE )
		{
			Weight = 15.0;
		}

		public PaladinSword( Serial serial ) : base( serial )
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

	public class EtherealRideGump : Gump
	{
		private EtherealDeed m_Deed;

		public EtherealRideGump ( EtherealDeed deed ) : base ( 220, 182 )
		{
			m_Deed = deed;
			int x = 30;
			int x2 = 65;
			int y = 35;
			int id = 1;

			AddPage ( 0 );

			AddImage( 0, 0, 0x820 );
			AddImage( 18, 30, 0x821);
			AddImage( 18, 85, 0x822 );
			AddImage( 18, 135, 0x823 );

			AddHtml( 60, 5, 275, 20, "<b>Choose your ethereal mount:</b>", false, false);

			AddButton( x, y, 4023, 4025, id, GumpButtonType.Reply, 0 );
			AddLabel( x2, y, 0x480, "Horse" );
			id=id+1;
			y=y+25;

			AddButton( x, y, 4023, 4025, id, GumpButtonType.Reply, 0 );
			AddLabel( x2, y, 0x480, "Ostard" );
			id=id+1;
			y=y+25;

			AddButton( x, y, 4023, 4025, id, GumpButtonType.Reply, 0 );
			AddLabel( x2, y, 0x480, "Llama" );
			id=id+1;
			y=y+25;

			AddButton( x, y, 4023, 4025, id, GumpButtonType.Reply, 0 );
			AddLabel( x2, y, 0x480, "Kirin" );
			id=id+1;

			if (!m_Deed.IsDonation)
			{
				y = 35;
				x = 150;
				x2 = 185;

				AddButton( x, y, 4023, 4025, id, GumpButtonType.Reply, 0 );
				AddLabel( x2, y, 0x480, "Unicorn" );
				id=id+1;
				y=y+25;

				AddButton( x, y, 4023, 4025, id, GumpButtonType.Reply, 0 );
				AddLabel( x2, y, 0x480, "Ridgeback" );
				id=id+1;
				y=y+25;

				AddButton( x, y, 4023, 4025, id, GumpButtonType.Reply, 0 );
				AddLabel( x2, y, 0x480, "Beetle" );
				id=id+1;
				y=y+25;

				AddButton( x, y, 4023, 4025, id, GumpButtonType.Reply, 0 );
				AddLabel( x2, y, 0x480, "Swamp Dragon" );
			}
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;
			Container bp = from.Backpack;

			if (m_Deed == null || m_Deed.Deleted)
				return;

			switch (info.ButtonID)
			{
				case 1:
				{
					bp.DropItem( new EtherealHorse() );
					break;
				}
				case 2:
				{
					bp.DropItem( new EtherealOstard() );
					break;
				}
				case 3:
				{
					bp.DropItem( new EtherealLlama() );
					break;
				}
				case 4:
				{
					bp.DropItem( new EtherealKirin() );
					break;
				}
				case 5:
				{
					bp.DropItem( new EtherealUnicorn() );
					break;
				}
				case 6:
				{
					bp.DropItem( new EtherealRidgeback() );
					break;
				}
				case 7:
				{
					bp.DropItem( new EtherealBeetle() );
					break;
				}
				case 8:
				{
					bp.DropItem( new EtherealSwampDragon() );
					break;
				}
			}
			m_Deed.Delete();
		}
	}

	public class EtherealDeed : Item
	{
		private bool m_IsDonation;

		public bool IsDonation
		{
			get{ return m_IsDonation; } set{ m_IsDonation = value; }
		}

		[Constructable]
		public EtherealDeed() : this(false)
		{
		}

		[Constructable]
		public EtherealDeed(bool donation) : base(0x14ef)
		{
			Weight = .5;
			Name = "ethereal deed";
			m_IsDonation = donation;
		}

		public EtherealDeed(Serial serial) : base(serial)
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

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsChildOf( from ) && !from.HasGump( typeof(EtherealRideGump) ) )
			{
				from.SendGump( new EtherealRideGump( this ) );
			}
		}
	}

	public class MiningStone : Item
	{
		[Constructable]
		public MiningStone() : base(0xed5)
		{
			Name = "GM Mining";
			Movable = false;
		}

		public MiningStone(Serial serial) : base(serial)
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

		public override void OnDoubleClick( Mobile from )
		{
			SkillName skills;
			PlayerMobile pm = from as PlayerMobile;

			try
			{
				skills = (SkillName)Enum.Parse( typeof( SkillName ), "Mining", true );
			}
			catch
			{
				from.SendLocalizedMessage( 1005631 ); // You have specified an invalid skill to set.
				return;
			}

			Skill skill = from.Skills[skills];
			skill.Base = 135;
			pm.SandMining = true;
			pm.StoneMining = true;

			from.SendMessage( "You now are a Legendary Miner and know how to mine sand and stone!" );
		}
	}

	public class HairCuttingScissors : Item
	{
		[Constructable]
		public HairCuttingScissors() : base( 0xF9F )
		{
			Weight = 1.0;
			Hue = 916;
			Name = "Hair Cutting Scissors";
		}

		public HairCuttingScissors( Serial serial ) : base( serial )
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
			if ( IsChildOf( from ) )
			{
				Item item = from.FindItemOnLayer( Layer.Hair );
				if ( item != null )
				{
					from.PlaySound( 0x248 );
					item.Delete();
				}
			}
		}
	}

	public class FacialHairCuttingScissors : Item
	{
		[Constructable]
		public FacialHairCuttingScissors() : base( 0xF9F )
		{
			Weight = 1.0;
			Hue = 921;
			Name = "Facial Hair Cutting Scissors";
		}

		public FacialHairCuttingScissors( Serial serial ) : base( serial )
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
			if ( IsChildOf( from ) )
			{
				Item item = ( from.FindItemOnLayer( Layer.FacialHair ) );
				if ( item != null )
				{
					from.PlaySound( 0x248 );
					item.Delete();
				}
			}
		}
	}

	public class HairGump : Gump
	{
		private Item m_Deed;

		public HairGump( Mobile from, Item deed ) : base( 120, 152 )
		{
			m_Deed = deed;
			int x = 30;
			int x2 = 65;
			int y = 35;
			int id = 1;

			from.CloseGump( typeof( HairGump ) );

			AddPage ( 0 );

			AddImage( 0, 0, 0x820 );
			AddImage( 18, 35, 0x821);
			AddImage( 18, 95, 0x822 );
			AddImage( 18, 155, 0x823 );

			AddHtml( 70, 5, 275, 20, "<b>Choose your hair style:</b>", false, false);

			AddButton( x, y, 4023, 4025, id, GumpButtonType.Reply, 0 ); // height: 20; width: 34;
			AddLabel( x2, y, 0x480, "Short Hair" );
			id=id+1;
			y=y+25;

			AddButton( x, y, 4023, 4025, id, GumpButtonType.Reply, 0 ); // height: 20; width: 34;
			AddLabel( x2, y, 0x480, "Long Hair" );
			id=id+1;
			y=y+25;

			AddButton( x, y, 4023, 4025, id, GumpButtonType.Reply, 0 ); // height: 20; width: 34;
			AddLabel( x2, y, 0x480, "Pony Tail" );
			id=id+1;
			y=y+25;

			AddButton( x, y, 4023, 4025, id, GumpButtonType.Reply, 0 ); // height: 20; width: 34;
			AddLabel( x2, y, 0x480, "Mohawk" );
			id=id+1;
			y=y+25;

			AddButton( x, y, 4023, 4025, id, GumpButtonType.Reply, 0 ); // height: 20; width: 34;
			AddLabel( x2, y, 0x480, "Pageboy" );
			id=id+1;

			y = 35;
			x = 150;
			x2 = 185;

			AddButton( x, y, 4023, 4025, id, GumpButtonType.Reply, 0 ); // height: 20; width: 34;
			AddLabel( x2, y, 0x480, "\"Buns\" Hair" );
			id=id+1;
			y=y+25;

			AddButton( x, y, 4023, 4025, id, GumpButtonType.Reply, 0 ); // height: 20; width: 34;
			AddLabel( x2, y, 0x480, "Afro" );
			id=id+1;
			y=y+25;

			AddButton( x, y, 4023, 4025, id, GumpButtonType.Reply, 0 ); // height: 20; width: 34;
			AddLabel( x2, y, 0x480, "Receeding Hair" );
			id=id+1;
			y=y+25;

			AddButton( x, y, 4023, 4025, id, GumpButtonType.Reply, 0 ); // height: 20; width: 34;
			AddLabel( x2, y, 0x480, "2 Pig-Tails" );
			id=id+1;
			y=y+25;

			AddButton( x, y, 4023, 4025, id, GumpButtonType.Reply, 0 ); // height: 20; width: 34;
			AddLabel( x2, y, 0x480, "Krisna Hair" );
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;
			Container bp = from.Backpack;
			int hue = 0;

			Item item = null;
			if ( (item = from.FindItemOnLayer( Layer.Hair )) != null )
				hue = item.Hue;
			else if ( (item = from.FindItemOnLayer( Layer.FacialHair )) != null )
				hue = item.Hue;

			if (info.ButtonID == 1)
			{
				RmvEquip( from );
				from.EquipItem( new ShortHair( hue ) );
				from.PlaySound( 0x248 );
				m_Deed.Delete();
			}
			else if (info.ButtonID == 2)
			{
				RmvEquip( from );
				from.EquipItem( new LongHair( hue ) );
				from.PlaySound( 0x248 );
				m_Deed.Delete();
			}
			else if (info.ButtonID == 3)
			{
				RmvEquip( from );
				from.EquipItem( new PonyTail( hue ) );
				from.PlaySound( 0x248 );
				m_Deed.Delete();
			}
			else if (info.ButtonID == 4)
			{
				RmvEquip( from );
				from.EquipItem( new Mohawk( hue ) );
				from.PlaySound( 0x248 );
				m_Deed.Delete();
			}
			else if (info.ButtonID == 5)
			{
				RmvEquip( from );
				from.EquipItem( new PageboyHair( hue ) );
				from.PlaySound( 0x248 );
				m_Deed.Delete();
			}
			else if (info.ButtonID == 6)
			{
				RmvEquip( from );
				from.EquipItem( new BunsHair( hue ) );
				from.PlaySound( 0x248 );
				m_Deed.Delete();
			}
			else if (info.ButtonID == 7)
			{
				RmvEquip( from );
				from.EquipItem( new Afro( hue ) );
				from.PlaySound( 0x248 );
				m_Deed.Delete();
			}
			else if (info.ButtonID == 8)
			{
				RmvEquip( from );
				from.EquipItem( new ReceedingHair( hue ) );
				from.PlaySound( 0x248 );
				m_Deed.Delete();
			}
			else if (info.ButtonID == 9)
			{
				RmvEquip( from );
				from.EquipItem( new TwoPigTails( hue ) );
				from.PlaySound( 0x248 );
				m_Deed.Delete();
			}
			else if (info.ButtonID == 10)
			{
				RmvEquip( from );
				from.EquipItem( new KrisnaHair( hue ) );
				from.PlaySound( 0x248 );
				m_Deed.Delete();
			}
		}

		private static void RmvEquip( Mobile from )
		{
			Item item = null;
			if ( (item = from.FindItemOnLayer( Layer.Hair )) != null )
				item.Delete();
		}
	}

	public class HairDeed : Item
	{
		[Constructable]
		public HairDeed() : base( 0x14f0 )
		{
			Weight = .5;
			Hue = 921;
			Name = "Hair Deed";
		}

		public HairDeed( Serial serial ) : base( serial )
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
			if ( IsChildOf( from ) )
			{
				from.SendGump( new HairGump( from, this ) );
			}
		}
	}

	public class FacialHairGump : Gump
	{
		private Item m_Deed;

		public FacialHairGump( Mobile from, Item deed ) : base( 120, 152 )
		{
			m_Deed = deed;
			int x = 25;
			int x2 = 60;
			int y = 35;
			int id = 1;

			from.CloseGump( typeof( FacialHairGump ) );

			AddPage ( 0 );

			AddImage( 0, 0, 0x820 );
			AddImage( 18, 30, 0x821);
			AddImage( 18, 85, 0x822 );
			AddImage( 18, 135, 0x823 );

			AddHtml( 50, 5, 275, 20, "<b>Choose your facial hair style:</b>", false, false);

			AddButton( x, y, 4023, 4025, id, GumpButtonType.Reply, 0 ); // height: 20; width: 34;
			AddLabel( x2, y, 0x480, "Long Beard" );
			id=id+1;
			y=y+25;

			AddButton( x, y, 4023, 4025, id, GumpButtonType.Reply, 0 ); // height: 20; width: 34;
			AddLabel( x2, y, 0x480, "Short Beard" );
			id=id+1;
			y=y+25;

			AddButton( x, y, 4023, 4025, id, GumpButtonType.Reply, 0 ); // height: 20; width: 34;
			AddLabel( x2, y, 0x480, "Goatee" );
			id=id+1;
			y=y+25;

			AddButton( x, y, 4023, 4025, id, GumpButtonType.Reply, 0 ); // height: 20; width: 34;
			AddLabel( x2, y, 0x480, "Mustache" );
			id=id+1;
			y=y+25;

			y = 35;
			x = 135;
			x2 = 170;

			AddButton( x, y, 4023, 4025, id, GumpButtonType.Reply, 0 ); // height: 20; width: 34;
			AddLabel( x2, y, 0x480, "Med-Short Beard" );
			id=id+1;
			y=y+25;

			AddButton( x, y, 4023, 4025, id, GumpButtonType.Reply, 0 ); // height: 20; width: 34;
			AddLabel( x2, y, 0x480, "Med-Long Beard" );
			id=id+1;
			y=y+25;

			AddButton( x, y, 4023, 4025, id, GumpButtonType.Reply, 0 ); // height: 20; width: 34;
			AddLabel( x2, y, 0x480, "Vandyke" );
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;
			Container bp = from.Backpack;
			int hue = 0;

			Item item = null;
			if ( (item = from.FindItemOnLayer( Layer.Hair )) != null )
				hue = item.Hue;
			else if ( (item = from.FindItemOnLayer( Layer.FacialHair )) != null )
				hue = item.Hue;

			if (info.ButtonID == 1)
			{
				RmvEquip( from );
				from.EquipItem( new LongBeard( hue ) );
				from.PlaySound( 0x248 );
				m_Deed.Delete();
			}
			else if (info.ButtonID == 2)
			{
				RmvEquip( from );
				from.EquipItem( new ShortBeard( hue ) );
				from.PlaySound( 0x248 );
				m_Deed.Delete();
			}
			else if (info.ButtonID == 3)
			{
				RmvEquip( from );
				from.EquipItem( new Goatee( hue ) );
				from.PlaySound( 0x248 );
				m_Deed.Delete();
			}
			else if (info.ButtonID == 4)
			{
				RmvEquip( from );
				from.EquipItem( new Mustache( hue ) );
				from.PlaySound( 0x248 );
				m_Deed.Delete();
			}
			else if (info.ButtonID == 5)
			{
				RmvEquip( from );
				from.EquipItem( new MediumShortBeard( hue ) );
				from.PlaySound( 0x248 );
				m_Deed.Delete();
			}
			else if (info.ButtonID == 6)
			{
				RmvEquip( from );
				from.EquipItem( new MediumLongBeard( hue ) );
				from.PlaySound( 0x248 );
				m_Deed.Delete();
			}
			else if (info.ButtonID == 7)
			{
				RmvEquip( from );
				from.EquipItem( new Vandyke( hue ) );
				from.PlaySound( 0x248 );
				m_Deed.Delete();
			}
		}

		private static void RmvEquip( Mobile from )
		{
			Item item = null;
			if ( (item = from.FindItemOnLayer( Layer.FacialHair )) != null )
				item.Delete();
		}
	}

	public class FacialHairDeed : Item
	{
		[Constructable]
		public FacialHairDeed() : base( 0x14f0 )
		{
			Weight = .5;
			Hue = 921;
			Name = "Facial Hair Deed";
		}

		public FacialHairDeed( Serial serial ) : base( serial )
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
			if ( IsChildOf( from ) )
			{
				if ( !from.Female)
				{
					from.SendGump( new FacialHairGump( from, this ) );
				}
				else
				{
					from.SendMessage( "You are a woman. And women don't grow facial hair!" );
				}
			}
		}
	}
}