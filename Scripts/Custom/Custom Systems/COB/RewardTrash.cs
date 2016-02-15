///////////////////////////////////////////
///////  Viago Trash Reward system 2.5.2
////// Created by Viago
///// www.fallensouls.org
//////////////////////////////////////////
using System;
using Server;
using Server.Items;
using System.Collections;
using Server.Multis;
using Server.Mobiles;
using Server.Network;
using Server.Gumps;
using Server.Menus;
using Server.Menus.Questions;


namespace Server.Items
{
	public class RewardTrash : Barrel
	{
		private static ArrayList m_SBInfos = new ArrayList();

		private static ArrayList m_BuyInfo = new ArrayList();
		private static ArrayList m_SellInfo = new ArrayList();

		private static ArrayList m_ForbiddenTypes = new ArrayList();

		[Constructable]
		public RewardTrash()
		{
			Name += "Reward Trash Barrel";
			Weight = 25.0;
			Movable = false;
			Hue = 948;
			Initialize();
		}

		public RewardTrash( Serial serial ) : base( serial )
		{
			Name = "Trash Reward";
			Initialize();
		}

		public override bool TryDropItem( Mobile from, Item item, bool sendFullMessage )
		{
			return DropItem( from, item );
		}

		public override bool OnDragDropInto( Mobile from, Item item, Point3D p )
		{
			return DropItem( from, item );
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

		public static void Initialize()
		{
			//vendor items
			m_SBInfos.Add( new SBAlchemist() );
			m_SBInfos.Add( new SBArchitect() );
			m_SBInfos.Add( new SBBaker() );
			m_SBInfos.Add( new SBBanker() );
			m_SBInfos.Add( new SBBard() );
			m_SBInfos.Add( new SBBarkeeper() );
			m_SBInfos.Add( new SBBeekeeper() );
			m_SBInfos.Add( new SBBowyer() );
			m_SBInfos.Add( new SBButcher() );
			m_SBInfos.Add( new SBCarpenter() );
			m_SBInfos.Add( new SBCobbler() );
			m_SBInfos.Add( new SBCook() );
			m_SBInfos.Add( new SBFarmer() );
			m_SBInfos.Add( new SBFisherman() );
			m_SBInfos.Add( new SBFortuneTeller() );
			m_SBInfos.Add( new SBFurtrader() );
			m_SBInfos.Add( new SBGlassblower() );
			//m_SBInfos.Add( new SBHairStylist() );
			m_SBInfos.Add( new SBHealer() );
			m_SBInfos.Add( new SBHerbalist() );
			m_SBInfos.Add( new SBHolyMage() );
			m_SBInfos.Add( new SBHouseDeed() );
			m_SBInfos.Add( new SBInnKeeper() );
			m_SBInfos.Add( new SBJewel() );
			m_SBInfos.Add( new SBLeatherWorker() );
			m_SBInfos.Add( new SBMage() );
			m_SBInfos.Add( new SBMapmaker() );
			m_SBInfos.Add( new SBProvisioner() );
			m_SBInfos.Add( new SBRancher() );
			m_SBInfos.Add( new SBRealEstateBroker() );
			m_SBInfos.Add( new SBScribe() );
			m_SBInfos.Add( new SBShipwright() );
			m_SBInfos.Add( new SBSmithTools() );
			m_SBInfos.Add( new SBStoneCrafter() );
			m_SBInfos.Add( new SBTailor() );
			m_SBInfos.Add( new SBTanner() );
			m_SBInfos.Add( new SBTavernKeeper() );
			m_SBInfos.Add( new SBTinker() );
			m_SBInfos.Add( new SBVagabond() );
			m_SBInfos.Add( new SBVarietyDealer() );
			m_SBInfos.Add( new SBVeterinarian() );
			m_SBInfos.Add( new SBWaiter() );
			m_SBInfos.Add( new SBWeaver() );

			//weapons
			m_SBInfos.Add( new SBAxeWeapon() );
			m_SBInfos.Add( new SBKnifeWeapon() );
			m_SBInfos.Add( new SBMaceWeapon() );
			m_SBInfos.Add( new SBPoleArmWeapon() );
			m_SBInfos.Add( new SBRangedWeapon() );
			m_SBInfos.Add( new SBSpearForkWeapon() );
			m_SBInfos.Add( new SBStavesWeapon() );
			m_SBInfos.Add( new SBSwordWeapon() );

			//armors
			m_SBInfos.Add( new SBChainmailArmor() );
			m_SBInfos.Add( new SBHelmetArmor() );
			m_SBInfos.Add( new SBLeatherArmor() );
			m_SBInfos.Add( new SBMetalShields() );
			m_SBInfos.Add( new SBPlateArmor() );
			m_SBInfos.Add( new SBRingmailArmor() );
			m_SBInfos.Add( new SBStuddedArmor() );
			m_SBInfos.Add( new SBWoodenShields() );

			//custom: edit SBCustom.cs
			m_SBInfos.Add( new SBCustom() );

			m_BuyInfo.Clear();
			m_SellInfo.Clear();

			for ( int i = 0; i < m_SBInfos.Count; i++ ) // compile lists for item values
			{
				SBInfo sbInfo = (SBInfo)m_SBInfos[i];
				m_BuyInfo.AddRange( sbInfo.BuyInfo );
				m_SellInfo.Add( sbInfo.SellInfo );
			}

			//add forbidden items here
			m_ForbiddenTypes.Add( typeof(Gold) );
			m_ForbiddenTypes.Add( typeof(Robe) );
			m_ForbiddenTypes.Add( typeof(DeathRobe) );
			m_ForbiddenTypes.Add( typeof(BaseClothing) );
			m_ForbiddenTypes.Add( typeof(GuildDeed) );

		}

		private bool DropItem( Mobile from, Item item )
		{
			if ( item == null )
				return false;

			int value = GetItemValue( item );

			if ( value == -1 || IsForbiddenType( item.GetType() ) || item.Stackable || item.LootType == LootType.Blessed || item.LootType == LootType.Newbied || item is Container ) // if it's not in the list of accepted items, or on list of forbidden items, or is stackable or is blessed
			{
				from.SendMessage( "This item is not part of the cleaning-up system." ); // sorry, out of luck
				return false;
			}
			else
			{
				if ( IsMagicArmor(item) ) // if magic armor add some value to basevalue
					value += GetMagicArmorValue(item);
				else if ( IsMagicWeapon(item) ) // if magic weapon add some value to basevalue
					value += GetMagicWeaponValue(item);
				else if ( IsMagicInstrument(item) )
					value += GetMagicInstrumentValue(item);

				from.SendMessage( "Base value: " + value + " gold." );

				int coppervalue = value / 10; // 1 copper = 10 gold. TODO: replace this with better formula

				if(coppervalue < 1) // just to make sure we don't give 0 copper
					coppervalue = 1;

				item.Delete(); // delete item
				from.AddToBackpack( new Tokens(coppervalue) ); // add copper to player
				from.SendMessage( "As a token of our graditude you get " + coppervalue + " copper coin" + (coppervalue > 1 ? "s" : "") + "." ); // tell player how much copper he got
				return true;
			}
		}

		private int GetItemValue( Item item )
		{
			IShopSellInfo[] sellinfo = (IShopSellInfo[])m_SellInfo.ToArray( typeof( IShopSellInfo ) ); // load list compiled earlier

			foreach ( IShopSellInfo ssi in sellinfo ) // check through list if item is in there
				foreach ( Type type in ssi.Types )
					if ( item.GetType() == type ) // if it is:
						return ssi.GetBuyPriceFor( item ); // return item's value
			return -1; // else return -1
		}

		private int GetMagicWeaponValue( Item item )
		{
			BaseWeapon weapon = (BaseWeapon)item;
			int acc = (int)weapon.AccuracyLevel;
			int dur = (int)weapon.DurabilityLevel;
			int dam = (int)weapon.DamageLevel;
			return (int)(10 * Math.Pow(2.2, (double)acc) + 10 * Math.Pow(1.6, (double)dur) + 10 * Math.Pow(2.8, (double)dam) + 10 * Math.Pow(1.6, (double)(acc + dur + dam))); // fairly balanced formula for magic weapons
		}

		private int GetMagicArmorValue( Item item )
		{
			BaseArmor armor = (BaseArmor)item;
			int dur = (int)armor.Durability;
			int pro = (int)armor.ProtectionLevel;
			return (int)(15 * Math.Pow(1.6, (double)dur) + 15 * Math.Pow(2.6, (double)pro) + 10 * Math.Pow(1.8, (double)(dur + pro))); // fairly balanced formula for magic armor
		}

		private int GetMagicInstrumentValue( Item item )
		{
			return 1500 + Utility.Random( 0, 200 );
		}

		private bool IsMagicWeapon( Item item )
		{
			if( !(item is BaseWeapon) ) // if not a weapon, return false
				return false;
			BaseWeapon weapon = (BaseWeapon)item;
			if ( !(weapon.AccuracyLevel > 0) && !(weapon.DurabilityLevel > 0) && !(weapon.DamageLevel > 0) ) // if the weapon doesn't have any bonuses, it is not magic
				return false;
			return true;
		}

		private bool IsMagicArmor( Item item )
		{
			if( !(item is BaseArmor) ) // if not an armor, return false
				return false;
			BaseArmor armor = (BaseArmor)item;
			if ( !(armor.ProtectionLevel > 0) && !(armor.Durability > 0) ) // if the armor doesn't have any bonuses, it is not magic
				return false;
			return true;
		}

		private bool IsMagicInstrument( Item item )
		{
			if ( !(item is BaseInstrument) )
				return false;
			BaseInstrument instrument = (BaseInstrument)item;
			if ( !(instrument.Slayer > 0) )
				return false;
			return true;
		}

		private bool IsForbiddenType( Type testtype )
		{
			foreach ( Type type in m_ForbiddenTypes ) // check through list of forbidden items
				if ( type == testtype ) // if found return true
					return true;
			return false;
		}
	}
}