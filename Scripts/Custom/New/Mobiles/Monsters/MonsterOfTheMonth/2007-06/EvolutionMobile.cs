using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
	public class EvolutionMobile : BaseCreature
	{
		ShrineType m_Type;

		private int m_EvolveValue;

		public int EvolveValue
        	{
          	  	get { return m_EvolveValue; }
           	 	set { m_EvolveValue = value; }
        	}

		private int m_WeakenType;

		public int WeakenType
        	{
          	  	get { return m_WeakenType; }
           	 	set { m_WeakenType = value; }
        	}

		// 1 = BladesWeak, 2 = PetWeak, 3 = MagicWeak, 4 = HumanMeleeWeak

		[Constructable]
		public EvolutionMobile() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Hagunemnom";
			Hue = 0;
			BodyValue = Utility.RandomList( 248, 251, 257, 263 );
			m_WeakenType = 1;
			m_EvolveValue = 2500;
			BaseSoundID = 100;

			switch ( Utility.Random( 10 ) )
			{
				case 0: m_Type = ShrineType.Chaos; break;
				case 1: m_Type = ShrineType.Compassion; break;
				case 2: m_Type = ShrineType.Honesty; break;
				case 3: m_Type = ShrineType.Honour; break;
				case 4: m_Type = ShrineType.Humility; break;
				case 5: m_Type = ShrineType.Justice; break;
				case 6: m_Type = ShrineType.Sacrifice; break;
				case 7: m_Type = ShrineType.Spirituality; break;
				case 8: m_Type = ShrineType.Valour; break;
				case 9: m_Type = ShrineType.Wisdom; break;
			}

			SetStr( 500 );
			SetDex( 200 );
			SetInt( 1000 );

			SetHits( 3000 );

			SetDamage( 20, 30 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 25, 30 );
			SetResistance( ResistanceType.Cold, 25, 30 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.Poisoning, 200.0 );
			SetSkill( SkillName.MagicResist, 180.0 );
			SetSkill( SkillName.Wrestling, 125.0 );
			SetSkill( SkillName.Magery, 125.0 );
			SetSkill( SkillName.EvalInt, 125.0 );
			SetSkill( SkillName.Meditation, 125.0 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 50;

			PackGold( 500 );
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 5 ) < 1 )
			c.DropItem( new ShrineRiddle( m_Type) );

			base.OnDeath( c );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.Rich );
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool CanDestroyObstacles { get { return true; } }

		public override void OnThink()
		{
			if( this.Hits < this.m_EvolveValue )
			{
				this.Say( "*Evolves into a creature immune to your attacks*" );

				if( this.m_WeakenType == 1 )
				{
					int ability = Utility.Random( 3 );

					switch ( ability )
					{
						case 0: DoWeakToPets(); break;
						case 1: DoWeakToMagic(); break;
						case 2: DoWeakToHumanMelee(); break;
				}
				}
				else if( this.m_WeakenType == 2 )
				{
					int ability = Utility.Random( 3 );

					switch ( ability )
					{
						case 0: DoWeakToBlades(); break;
						case 1: DoWeakToMagic(); break;
						case 2: DoWeakToHumanMelee(); break;
					}
				}
				else if( this.m_WeakenType == 3 )
				{
					int ability = Utility.Random( 3 );

					switch ( ability )
					{
						case 0: DoWeakToBlades(); break;
						case 1: DoWeakToPets(); break;
						case 2: DoWeakToHumanMelee(); break;
					}
				}
				else if( this.m_WeakenType == 4 )
				{
					int ability = Utility.Random( 3 );

					switch ( ability )
					{
						case 0: DoWeakToBlades(); break;
						case 1: DoWeakToMagic(); break;
						case 2: DoWeakToPets(); break;
					}
				}
			}

			if ( this.m_WeakenType == 2 || this.m_WeakenType == 3 || this.m_WeakenType == 4 )
			{
				ArrayList list = new ArrayList();

				foreach ( Item item in this.GetItemsInRange( 1 ) )
				{
                    if (item is Corpse)
                    {
                        Corpse c = (Corpse)item;
                        if (c.Owner == null || !(c.Owner is PlayerMobile))
                            list.Add(item);
                    }
				}

				foreach ( Item item in list )
				{
					this.Say( "*Devours the corpse and regenerates slightly*" );
					this.Hits += 250;
					item.Delete();
				}
			}
			base.OnThink();
		}

		public void DoWeakToBlades()
		{
			this.WeakenType = 1;
			this.BodyValue = Utility.RandomList( 248, 251, 257, 263 );
			this.Int = 1000;
			this.Mana = 1000;
			this.Stam = 200;
			this.BaseSoundID = 100;
			this.EvolveValue = this.EvolveValue - 500;
		}

		public void DoWeakToPets()
		{
			this.WeakenType = 2;
			this.BodyValue = Utility.RandomList( 75, 83, 18, 53 );
			this.Int = 1;
			this.Mana = 1;
			this.Stam = 200;
			this.BaseSoundID = 604;
			this.EvolveValue = this.EvolveValue - 500;
		}

		public void DoWeakToMagic()
		{
			this.WeakenType = 3;
			this.BodyValue = Utility.RandomList( 107, 108, 109, 110, 111, 112, 113 );
			this.Int = 1;;
			this.Mana = 1;
			this.Stam = 200;
			this.BaseSoundID = 268;
			this.EvolveValue = this.EvolveValue - 500;
		}

		public void DoWeakToHumanMelee()
		{
			this.WeakenType = 4;
			this.BodyValue = Utility.RandomList( 990, 991, 994 );
			this.Int = 5000;
			this.Mana = 5000;
			this.Stam = 200;
			this.BaseSoundID = 0;
			this.EvolveValue = this.EvolveValue - 500;
		}

		public override void Damage( int amount, Mobile from )
		{
			if ( this.m_WeakenType == 1 )
			{
				if ( from is BaseCreature )
				{
					BaseCreature bc = (BaseCreature)from;

					if ( bc is BladeSpirits )
					{
						amount = amount;
					}
					else
					{
						amount = (int)(0);
					}
				}
				else
				{
					amount = (int)(0);
				}
			}

			if ( this.m_WeakenType == 2 )
			{
				if ( from is BaseCreature )
				{
					BaseCreature bc = (BaseCreature)from;

					if ( bc.Controlled )
					{
						amount = amount;
					}
					else
					{
						amount = (int)(0);
					}
				}
				else
				{
					amount = (int)(0);
				}
			}

			if ( this.m_WeakenType == 3 )
			{
				if ( from is PlayerMobile )
				{
					BaseWeapon bw = from.FindItemOnLayer( Layer.OneHanded ) as BaseWeapon;
					BaseWeapon bw2 = from.FindItemOnLayer( Layer.TwoHanded ) as BaseWeapon;
					if ( bw == null && bw2 == null )
					{
						amount = amount;
					}
					else
					{
						amount = (int)(0);
					}
				}
				else
				{
					amount = (int)(0);
				}
			}

			if ( this.m_WeakenType == 4 )
			{
				if ( from is PlayerMobile )
				{
					BaseWeapon bw3 = from.FindItemOnLayer( Layer.OneHanded ) as BaseWeapon;
					BaseWeapon bw4 = from.FindItemOnLayer( Layer.TwoHanded ) as BaseWeapon;
					if ( bw3 != null || bw4 != null)
					{
						amount = amount;
					}
					else
					{
						amount = (int)(0);
					}
				}
				else
				{
					amount = (int)(0);
				}
			}
			base.Damage( amount, from );
		}

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)to;

				if ( this.m_WeakenType == 1 || this.m_WeakenType == 3 || this.m_WeakenType == 4 )
				{
					if ( bc.Controlled )
					damage *= 5;
				}

				if ( this.m_WeakenType == 2 || this.m_WeakenType == 3 || this.m_WeakenType == 4 )
				{
					if ( bc.BardTarget == this )
					damage *= 10;
				}
			}

			if ( to is PlayerMobile )
			{
				if ( this.m_WeakenType == 3 )
				{
					damage *= 5;
				}

				if ( this.m_WeakenType == 1 || this.m_WeakenType == 2 )
				{
					damage *= 2;
				}

				if ( this.m_WeakenType == 4 )
				{
					damage /= 2;
				}
			}
		}

		public EvolutionMobile( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			writer.Write( m_EvolveValue );
			writer.Write( m_WeakenType );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			m_EvolveValue = reader.ReadInt();
			m_WeakenType = reader.ReadInt();
		}
	}
}