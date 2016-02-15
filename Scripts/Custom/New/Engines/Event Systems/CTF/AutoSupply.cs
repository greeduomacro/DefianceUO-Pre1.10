using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class AutoSupply : Item
	{
        private bool ethereals;

		[Constructable]
		public AutoSupply() : base( 0x1183 )
		{
			this.Movable = false;
			this.Name = "Supply Stone";
            this.GiveEthereals = false;
		}

		public AutoSupply( Serial serial ) : base(serial)
		{
		}

        [CommandProperty(AccessLevel.GameMaster)]
        public bool GiveEthereals
        {
            get { return ethereals; }
            set { ethereals = value; }
        }

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            if (version == 1)
                GiveEthereals = reader.ReadBool();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)1 );//version
            writer.Write(GiveEthereals);
		}

		public override void OnDoubleClick( Mobile from )
		{
            //Al:Unified checks and added pedantic visibility check.
            if (!from.InRange(GetWorldLocation(), 4))
            {
                from.SendLocalizedMessage(500446); // That is too far away.
                return;
            }
            else if ((!this.Visible) || !from.InLOS(this.GetWorldLocation()))
            {
                from.SendLocalizedMessage(502800); // You can't see that.
                return;
            }

			from.SendMessage( "You have been given some supplies based on your skills." );

            // Ethereals
            // will just be given out, when GiveEthereals is true and the user does not have a GameEthereal nor is mounted.
            if (GiveEthereals)
            {
                Item eth = from.Backpack.FindItemByType(typeof(GameEthereal));
                if (eth == null && !from.Mounted)
                    PackItem(from, new GameEthereal());
            }

			//4 pouches
			for (int i=0;i<4;++i)
			{
				Pouch p = new Pouch();
				//p.TrapType = TrapType.MagicTrap;
				//p.TrapPower = 1;
				//p.Hue = 0x25;
				PackItem( from, p );
			}

			//PackItem( from, new GreaterExplosionPotion() );
			//PackItem( from, new TotalRefreshPotion() );
			//PackItem( from, new GreaterCurePotion() );
			GiveLeatherArmor( from );

            Spellbook book = Spellbook.FindRegular(from);//Spellbook book = from.GetSpellbook( typeof( Spellbook ) ) as Spellbook;
            if (book != null)
            {
                if (book.Content != ulong.MaxValue)
                    book.Content = ulong.MaxValue;
            }
            else
            {
                book = new Spellbook();
                book.Content = ulong.MaxValue;//all spells
                book.LootType = LootType.Regular;
                book.Weight = 11;
                PackItem(from, book);
            }

            if ( from.Skills[SkillName.Magery].Value >= 50.0 )
			{
				PackItem( from, new BagOfReagents( 100 ) );
				if ( from.Skills[SkillName.Parry].Value >= 50.0 )
				{
					GiveItem( from, new MetalKiteShield() );
				}
			}
			else
			{
                PackItem(from, new BagOfReagents(10));
                //for(int i=0;i<3;i++)
					//PackItem( from, new GreaterHealPotion() );
			}

			if ( from.Skills[SkillName.Healing].Value >= 50.0 )
				PackItem( from, new Bandage( 100 ) );

			if ( from.Skills[SkillName.Fencing].Value >= 50.0 )
			{
				PackItem( from, new Kryss() );
                                PackItem( from, new WarFork() );
				PackItem( from, new ShortSpear() );
				if ( from.Skills[SkillName.Parry].Value >= 50.0 )
				{
					PackItem( from, new MetalKiteShield() );
				}
				else
				{
					GiveItem( from, new Spear() );
				}
			}

			if ( from.Skills[SkillName.Swords].Value >= 50.0 )
			{
				if ( from.Skills[SkillName.Parry].Value >= 50.0 )
				{
					GiveItem( from, new MetalKiteShield() );
				}

				if ( from.Skills[SkillName.Lumberjacking].Value >= 50.0 )
				{
					GiveItem( from, new Hatchet() );
					PackItem( from, new LargeBattleAxe() );
				}

				PackItem( from, new Halberd() );
				GiveItem( from, new Katana() );
			}

			if ( from.Skills[SkillName.Macing].Value >= 50.0 )
			{
				if ( from.Skills[SkillName.Parry].Value >= 50.0 )
					GiveItem( from, new MetalKiteShield() );
				GiveItem( from, new QuarterStaff() );
				PackItem( from, new WarHammer() );
                                PackItem( from, new WarAxe() );
			}

			if ( from.Skills[SkillName.Archery].Value >= 50.0 )
			{
				GiveItem( from, new Bow() );
				PackItem( from, new Crossbow() );
				PackItem( from, new HeavyCrossbow() );

				PackItem( from, new Bolt( 100 ) );
				PackItem( from, new Arrow( 100 ) );
			}

			if ( from.Skills[SkillName.Tailoring].Value >= 50.0 )
			{
				PackItem( from, new SewingKit() );
				PackItem( from, new Cloth( 25 ) );
				PackItem( from, new Leather( 100 ) );
			}

			if ( from.Skills[SkillName.Blacksmith].Value >= 50.0 )
			{
				PackItem( from, new Tongs() );
				PackItem( from, new IronIngot( 300 ) );
			}

			if ( from.Skills[SkillName.Poisoning].Value >= 50.0 )
			{
				for (int i=0;i<3;i++)
					PackItem( from, new GreaterPoisonPotion() );
			}
		}

		public static void GiveItem( Mobile m, Item item )
		{
			if ( item is BaseArmor )
				((BaseArmor)item).Quality = ArmorQuality.Exceptional;
			else if ( item is BaseWeapon )
				((BaseWeapon)item).Quality = WeaponQuality.Exceptional;

			Item move = m.FindItemOnLayer( item.Layer );
			if ( move != null )
			{
				if ( !m.PlaceInBackpack( move ) )
				{
					item.Delete();
					return;
				}
			}

			if ( !m.EquipItem( item ) && !m.PlaceInBackpack( item ) )
				item.Delete();
		}

		public static void PackItem( Mobile m, Item item )
		{
			if ( item is BaseArmor )
				((BaseArmor)item).Quality = ArmorQuality.Exceptional;
			else if ( item is BaseWeapon )
				((BaseWeapon)item).Quality = WeaponQuality.Exceptional;

			if ( !m.PlaceInBackpack( item ) )
				item.Delete();
		}

		public static void GiveBoneArmor( Mobile m )
		{
			GiveItem( m, new BoneArms() );
			GiveItem( m, new BoneChest() );
			GiveItem( m, new BoneLegs() );
			GiveItem( m, new BoneGloves() );
		}

		public static void GiveLeatherArmor( Mobile m )
		{
			GiveItem( m, new LeatherArms() );
			GiveItem( m, new LeatherChest() );
			GiveItem( m, new LeatherGloves() );
			GiveItem( m, new LeatherGorget() );
			GiveItem( m, new LeatherLegs() );
		}
	}
}