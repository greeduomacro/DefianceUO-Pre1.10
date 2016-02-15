using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a mutated dread spider corpse" )]
	public class MutatedDreadSpider : BaseCreature
	{

		[Constructable]
		public MutatedDreadSpider () : base( AIType.AI_Mage, FightMode.Weakest, 10, 1, 0.1, 0.2 )
		{
			Name = "a mutated dread spider";
			Body = 173;
			BaseSoundID = 0x183;
			ShortTermMurders = 10;
			Kills = 10;

			SetStr( 496, 520 );
			SetDex( 525, 750 );
			SetInt( 486, 510 );

			SetHits( 1228, 1442 );

			SetDamage( 14, 17 );

			SetSkill( SkillName.EvalInt, 119.1, 120.0 );
			SetSkill( SkillName.Anatomy, 65.1, 70.1 );
			SetSkill( SkillName.Magery, 99.1, 99.8 );
			SetSkill( SkillName.Meditation, 50.1, 60.0 );
			SetSkill( SkillName.MagicResist, 150.1, 175.0 );
			SetSkill( SkillName.Tactics, 100.1, 100.1 );
			SetSkill( SkillName.Poisoning, 94.1, 96.0 );
			SetSkill( SkillName.Wrestling, 100.1, 100.1 );

			Fame = 25000;
			Karma = -25000;

			VirtualArmor = 90;

			switch( Utility.Random(75) )
	{
			case 0: PackItem( new DarkIronWire() ); break;
	}

			PackGold( 2000, 3000 );
			PackItem( new DeadlyPoisonPotion() );
			PackItem( new DeadlyPoisonPotion() );
			PackItem( new DeadlyPoisonPotion() );
			PackSlayer();
			PackItem( new SpidersSilk( Utility.RandomMinMax( 15, 75 ) ) );

			switch ( Utility.Random( 2 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 3 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}

			switch ( Utility.Random( 4 ) )
			{
				case 0: PackWeapon( 2, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackWeapon( 2, 5 ); break;
				case 1: PackArmor( 3, 5 ); break;
			}

            }

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Lethal; } }
		public override bool BardImmune{ get{ return true;} }
		public override bool AutoDispel{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 5; } }

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
				damage *= 3;
		}

		public MutatedDreadSpider( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
		public override void OnDeath( Container c )
		{
			Item item = null;
			switch( Utility.Random(800) )
				{
           		case 0: c.DropItem( item = new Server.Items.GrayBrickFireplaceEastDeed() ); break;
			case 1: c.DropItem( item = new Server.Items.GrayBrickFireplaceSouthDeed() ); break;
			case 2: c.DropItem( item = new Server.Items.SandstoneFireplaceEastDeed() ); break;
			case 3: c.DropItem( item = new Server.Items.SandstoneFireplaceSouthDeed() ); break;
			case 4: c.DropItem( item = new Server.Items.StoneFireplaceEastDeed() ); break;
			case 5: c.DropItem( item = new Server.Items.StoneFireplaceSouthDeed() ); break;
			        }
			base.OnDeath( c );
		}
	}
}