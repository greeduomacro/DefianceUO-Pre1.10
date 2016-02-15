#region AuthorHeader
//
//	EvoSystem version 1.6, by Xanthos
//
//  Mercenary is based on a concept by Raelis, Sadoul and Grae
//
#endregion AuthorHeader
using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Misc;
using Server.SkillHandlers;
using System.Collections;
using Server.Targeting;

namespace Xanthos.Evo
{
	[CorpseName( "a mercenary corpse" )]
	public class Mercenary : BaseEvo, IEvoCreature
	{
		public override BaseEvoSpec GetEvoSpec()
		{
			return MercenarySpec.Instance;
		}

		public override BaseEvoEgg GetEvoEgg()
		{
			return null;
		}

		public override bool AddPointsOnDamage { get { return false; } }
		public override bool AddPointsOnMelee { get { return true; } }
		public override Type GetEvoDustType() { return null; }

		public Mercenary( string name ) : base( name, AIType.AI_Melee, 0.01 )
		{
			SpeechHue = Utility.RandomDyedHue();
			Hue = Utility.RandomSkinHue();
			Item hair = new Item( Utility.RandomList( 0x203B, 0x2049, 0x2048, 0x204A ) );
			hair.Hue = Utility.RandomNondyedHue();
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem( hair );

			if ( Female = Utility.RandomBool() )
				Body = 0x191;
			else
			{
				Body = 0x190;
				if ( Utility.RandomBool() )
				{
					Item beard = new Item( Utility.RandomList( 0x203E, 0x203F, 0x2040, 0x2041, 0x204B, 0x204C, 0x204D ) );
					beard.Hue = hair.Hue;
					beard.Layer = Layer.FacialHair;
					beard.Movable = false;
					AddItem( beard );
				}
			}

			Item weapon;
			switch ( Utility.Random( 6 ) )
			{
				case 0: weapon = new Kryss(); break;
				case 1: weapon = new Scimitar(); break;
				case 2: weapon = new WarAxe(); break;
				case 3: weapon = new Cutlass(); break;
				case 4: weapon = new HammerPick(); break;
				default: weapon = new WarFork(); break;
			}
			AddItem( weapon );

			if ( null == Backpack )
			{
				Container pack = new Backpack();
				pack.Movable = false;
				AddItem( pack );
			}
			AddItem( new Robe() );
		}

		public Mercenary( Serial serial ) : base( serial ) {}

		public override FoodType FavoriteFood{ get{ return FoodType.Eggs | FoodType.Fish | FoodType.Meat | FoodType.FruitsAndVegies; } }

		protected override void PackSpecialItem()
		{
			Item item;

			switch ( Utility.Random( 5 ) )
			{
				case 0:	item = (Item)new MercenaryDeed(); break;
				case 1: item = (Item)new EtherealLlama(); break;
				case 2: item = (Item)new PetLeash(); break;
				case 3: item = (Item)new HoodedShroudOfShadows(); break;
				default: ((BallOfSummoning)(item = (Item)new BallOfSummoning())).Charges = 2000; break;
			}
			item.LootType = LootType.Blessed;
			PackItem( item );
		}

		public override void Evolve( bool hatching )
		{
			base.Evolve( hatching );
			if ( m_Stage == m_FinalStage - 1 )
				Title = ", Servant of " + ControlMaster.Name;
			else if ( m_Stage == m_FinalStage )
				Title = ", Avenger of " + ControlMaster.Name;
		}

		public override bool HandlesOnSpeech( Mobile from ) { return true; }

		static string [] keyWords = { "", " restyle", " dress", " undress", " mount", " dismount", " stats", " unload", " list", " grab", " loot", " attack", " tithe", " help" };
		enum Command { None = 0, Restyle, Dress, Undress, Mount, Dismount, Stats, Unload, List, Grab, Loot, Attack, Tithe, Help, Last = Command.Help, }

		public override void OnSpeech( SpeechEventArgs e )
		{
			Mobile from = e.Mobile;
			int command;

			if ( ControlMaster != from )
				return;

			if ( !Alive )
			{
				base.OnSpeech( e );
				return;
			}

			for ( command = (int)Command.Last; command > (int)Command.None; command-- )
			{
				if ( e.Speech.ToLower().IndexOf( Name.ToLower() + keyWords[ command ] ) >= 0 )
					break;
			}

			switch ( command )
			{
				case (int)Command.Restyle:
					from.SendGump( new Xanthos.Evo.HairstylistBuyGump( from, this ) );
					break;
				case (int)Command.Dress:
					Say("I shall attmept to equip all the items in my pack.");
					{
						ArrayList items = Backpack.Items;

						for ( int i = items.Count - 1; i >= 0; --i )
						{
							Item item = (Item)items[i];
							if ( item is BaseWeapon )	// If any conflicting items are found skip equipping the item
							{
								Item itemEquipped = FindItemOnLayer( Layer.TwoHanded );
								if ( null != itemEquipped && ((BaseWeapon)item).CheckConflictingLayer( this, itemEquipped, Layer.TwoHanded ))
									continue;

								itemEquipped = FindItemOnLayer( Layer.OneHanded );
								if ( null != itemEquipped && ((BaseWeapon)item).CheckConflictingLayer( this, itemEquipped, Layer.OneHanded ))
									continue;

								itemEquipped = FindItemOnLayer( Layer.FirstValid );
								if ( null != itemEquipped && ((BaseWeapon)item).CheckConflictingLayer( this, itemEquipped, Layer.FirstValid ))
									continue;
							}
							else if ( !( item is BaseClothing || item is BaseArmor || item is BaseJewel ) || null != FindItemOnLayer( item.Layer ) )
								continue;

							Backpack.DropItem( item ); // No conlficts, go ahead and equip
							AddItem( item );
						}
					}
					break;
				case (int)Command.Undress:
					Say("I shall give to you everything I am wearing.");
					{
						ArrayList items = Items;

						for ( int i = items.Count - 1; i >= 0; --i )
						{
							Item item = (Item)items[i];
							if ( !( item is Container || item is IMountItem ) && item.Layer != Layer.FacialHair && item.Layer != Layer.Hair )
								from.AddToBackpack( item );
						}
					}
					break;
				case (int)Command.Mount:
					if ( null == Mount )
					{
						IMount mount = FindMyMount( Backpack );

						if ( null == mount )
							from.Target = new MountTarget( from, this );
						else
							mount.Rider = this;
					}
					break;
				case (int)Command.Dismount:
					if ( null != Mount )
					{
						IMount mount = FindMyMount( null );

						if ( null != mount )
						{
							mount.Rider = null;
							if ( mount is EtherealMount )
								Backpack.DropItem( mount as EtherealMount );
							else
								((BaseMount)mount).ControlOrder = OrderType.Follow;
						}
					}
					break;
				case (int)Command.Stats:
					from.SendGump( new AnimalLoreGump( this ) );
					break;
				case (int)Command.Unload:
					Say("I shall give to you everything in my pack.");
					{
						ArrayList items = Backpack.Items;

						for ( int i = items.Count - 1; i >= 0; --i )
						{
							from.AddToBackpack( (Item)items[i] );
						}
					}
					break;
				case (int)Command.List:
					Say("I am carrying:");
					foreach( Item item in Backpack.Items )
					{
						if ( null != item )
							Say( "{0} {1}", item.Amount, item.GetType().Name );
					}
					break;
				case (int)Command.Grab:
					GrabItems( false );
					break;
				case (int)Command.Loot:
					GrabItems( true );
					break;
				case (int)Command.Tithe:
					{
						Container pack = Backpack;
						if ( null != pack )
						{
							Item item = pack.FindItemByType( typeof( Gold ) );
							int tithe;

							if ( null != item && item.Amount > 0 && pack.ConsumeTotal( typeof( Gold ), (tithe = item.Amount) ))
							{
								Emote( "*" + Name + " tithes gold as a sign of devotion.*" );
								TithingPoints += tithe;
								PlaySound( 0x243 );
								PlaySound( 0x2E6 );
							}
							else
								Say( "I do not have enough gold to tithe." );
						}
					}
					break;
				case (int)Command.Help:
					Say("I will follow these commands: restyle, dress, undress, mount, dismount, unload, list, grab, loot, attack, tithe and stats.");
					break;
				case (int)Command.Attack:
					switch ( Utility.Random( 3 ) )
					{
						case 0: WeaponAbility.SetCurrentAbility( this, ((BaseWeapon)Weapon).PrimaryAbility ); break;
						case 1: WeaponAbility.SetCurrentAbility( this, ((BaseWeapon)Weapon).SecondaryAbility ); break;
						case 2: new Server.Spells.Chivalry.ConsecrateWeaponSpell( this, null ).Cast(); break;
					}
					goto default;
				default:
					base.OnSpeech( e );
					return;
			}
			e.Handled = true;
		}

		public override void OnThink()
		{
			base.OnThink();

			if ( Hits < HitsMax - 50 )
			{
				if ( null != BandageContext.GetContext( this ))
				{
					Item item = Backpack.FindItemByType( typeof(Bandage) );

					if ( null != item && null != BandageContext.BeginHeal( this , this ))
						item.Consume( 1 );
				}
			}
			if ( Hunger < 15 )
			{
				Item item = Backpack.FindItemByType( typeof( Food ) );

				if ( null != item )
					((Food)item).Eat( this );
			}
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
		}

		public void GrabItems( bool ignorNoteriety )
		{
			ArrayList items = new ArrayList();
			bool rejected = false;
			bool lootAdded = false;

			Emote( "*Rummages through items on the ground.*" );

			foreach ( Item item in GetItemsInRange( 2 ) )
			{
				if ( item.Movable )
					items.Add( item );
				else if ( item is Corpse )
				{	// Either criminally loot any corpses or loot only corpses that we have rights to
					if ( ignorNoteriety  || NotorietyHandlers.CorpseNotoriety( this, (Corpse)item ) != Notoriety.Innocent )
					{
						foreach ( Item corpseItem in ((Corpse)item).Items )
							items.Add( corpseItem );
					}
				}
			}
			foreach ( Item item in items )
			{
				if ( !Backpack.CheckHold( this, item, false, true ) )
					rejected = true;
				else
				{
					bool isRejected;
					LRReason isReject;

					NextActionTime = DateTime.Now;
					Lift( item, item.Amount, out isRejected, out isReject );

					if ( !rejected )
					{
						Drop( this, Point3D.Zero );
						lootAdded = true;
					}
				}
			}
			if ( lootAdded )
				PlaySound( 0x2E6 ); //drop gold sound
			if ( rejected )
				Say( "I need a bigger pack for all these things." );
		}

		public IMount FindMyMount( Container pack )
		{
			ArrayList items = ( null == pack ) ? Items : pack.Items;

			foreach ( Item item in items )
			{
				if ( item is IMountItem )
					return ((IMountItem)item).Mount;

				else if ( item.Layer == Layer.Mount )
					return (IMount)item;
			}
			return null;
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			PlayerMobile player = from as PlayerMobile;
			Item itemEquipped;

			if ( ControlMaster != from || player == null || !Alive )
				return base.OnDragDrop( from, dropped );

			if ( dropped is BaseWeapon )
			{
				if ( m_Stage < m_FinalStage - 1 && ((BaseWeapon)dropped).PlayerConstructed )
				{
					from.SendMessage( Name + " must attain Servant to accept crafted weapons.");
				}
				else if ( m_Stage < m_FinalStage && ((BaseWeapon)dropped).ArtifactRarity != 0 )
				{
					from.SendMessage( Name + " must attain Avenger to accept artifact weapons.");
				}
				else	// Make sure there are no confliciting weapons/shields equipped.
				{
					itemEquipped = FindItemOnLayer( Layer.TwoHanded );

					if ( null != itemEquipped && ((BaseWeapon)dropped).CheckConflictingLayer( this, itemEquipped, Layer.TwoHanded ))
						from.AddToBackpack( itemEquipped );

					itemEquipped = FindItemOnLayer( Layer.OneHanded );
					if ( null != itemEquipped && ((BaseWeapon)dropped).CheckConflictingLayer( this, itemEquipped, Layer.OneHanded ))
						from.AddToBackpack( itemEquipped );

					itemEquipped = FindItemOnLayer( Layer.FirstValid );
					if ( null != itemEquipped && ((BaseWeapon)dropped).CheckConflictingLayer( this, itemEquipped, Layer.FirstValid ))
						from.AddToBackpack( itemEquipped );

					Backpack.DropItem( dropped );
					AddItem( dropped );
					from.SendMessage( "You give " + Name + " a weapon." );
					return true;
				}
			}
			else if ( dropped is BaseArmor )
			{
				if ( m_Stage < m_FinalStage - 1 && ((BaseArmor)dropped).PlayerConstructed )
				{
					from.SendMessage( Name + " must attain Servant to accept crafted armor.");
				}
				else if ( m_Stage < m_FinalStage && ((BaseArmor)dropped).ArtifactRarity != 0 )
				{
					from.SendMessage( Name + "  must attain Avenger to accept artifact armor.");
				}
				else
				{
					BaseArmor armor = (BaseArmor)dropped;

					if ( !armor.AllowMaleWearer && Body.IsMale )
					{
						from.SendLocalizedMessage( 1010388 ); // Only females can wear this.
						from.AddToBackpack( armor );
					}
					else if ( !armor.AllowFemaleWearer && Body.IsFemale )
					{
						from.SendMessage( "Only males can wear this." );
						from.AddToBackpack( armor );
					}
					else
					{
						itemEquipped = FindItemOnLayer( dropped.Layer );
						if ( null != itemEquipped )
							from.AddToBackpack( itemEquipped );

						Backpack.DropItem( dropped );
						AddItem( dropped );
						from.SendMessage("You give " + Name + " a piece of armor.");
						return true;
					}
				}
			}
			else if ( dropped is BaseClothing || dropped is BaseJewel )
			{
				itemEquipped = FindItemOnLayer( dropped.Layer );
				if ( null != itemEquipped )
					from.AddToBackpack( itemEquipped );

				Backpack.DropItem( dropped );
				AddItem( dropped );
				from.SendMessage("You give " + Name + " an item of " + (dropped is BaseJewel ? "jewelry." : "clothing.") );
				return true;
			}
			else if ( dropped is Arrow || dropped is Bolt || dropped is Bandage )
			{
				Backpack.DropItem( dropped );
				from.SendMessage("You give " + Name + " supplies.");
				return true;
			}

			return base.OnDragDrop( from, dropped );
		}

		public override bool CheckFeed( Mobile from, Item dropped )
		{
			bool bonded = IsBonded;
			bool result = base.CheckFeed( from, dropped );

			if ( bonded != IsBonded)	// this puts the new bonded control master in the title
			{
				if ( m_Stage == m_FinalStage - 1 )
					Title = ", Servant of " + ControlMaster.Name;
				else if ( m_Stage == m_FinalStage )
					Title = ", Avenger of " + ControlMaster.Name;
			}
			return result;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write( (int)0 );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	internal class MountTarget : Target
	{
		private Mercenary m_Merc;

		public MountTarget( Mobile from, Mercenary merc ) : base( 1, false, TargetFlags.None )
		{
			m_Merc = merc;
			from.SendMessage( "Choose a mount you would like to place " + m_Merc.Name + " on." );
		}

		protected override void OnTarget( Mobile from, object o )
		{
			DoOnTarget( from, o, m_Merc );
		}

		public static void DoOnTarget( Mobile from, object o, Mercenary merc )
		{
			EtherealMount ethy = o as EtherealMount;
			if ( null != ethy )
			{
				if ( null != ethy.Rider )
					from.SendMessage( "This ethereal mount is already in use by someone else." );

				else if ( !ethy.IsChildOf( from.Backpack ) )
					from.SendMessage( "The ethereal mount must be in your pack for you to use it." );
				else
					ethy.Rider = merc;
			}
			else
			{
				BaseMount mount = o as BaseMount;

				if ( null == mount )
					from.SendMessage( "That is not a mount." );

				else if ( null != mount.Rider )
					from.SendMessage( "This mount is already in use by someone else." );

				else if ( mount.ControlMaster != from )
					from.SendMessage( "You do not own this mount." );
				else
					mount.Rider = merc;
			}
		}
	}
}