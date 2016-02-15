using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a noxious corpse" )]
	public class NoxiousArcher : BaseCreature
	{
		public override bool IsScaryToPets{ get{ return true; } }

		[Constructable]
		public NoxiousArcher() : base( AIType.AI_Archer, FightMode.Closest, 10, 1, 0.15, 0.2 )
		{
			Name = "a noxious archer";
			Body = 0x46;
			Hue = 2127;
			BaseSoundID = 589;

			SetStr( 246, 280 );
			SetDex( 121, 140 );
			SetInt( 116, 140 );

			SetHits( 1088, 1208 );

			SetDamage( 8, 14 );

			SetSkill( SkillName.Anatomy, 40.2, 50.0 );
			SetSkill( SkillName.Archery, 94.1, 96.0 );
			SetSkill( SkillName.MagicResist, 95.1, 96.0 );
			SetSkill( SkillName.Tactics, 90.1, 95.0 );
			SetSkill( SkillName.Wrestling, 90.1, 95.0 );
			SetSkill( SkillName.Poisoning, 98.1, 99.4 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 56;

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackItem( new GreaterCurePotion() ); break;
				case 1: PackItem( new GreaterPoisonPotion() ); break;
			}

			AddItem( new Bow() );
			PackGold( 750, 1050 );

			switch ( Utility.Random( 2 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 3 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 4 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}

			switch ( Utility.Random( 10 ) )
			{
				case 0: PackItem( new Arrow( Utility.Random( 50, 100 ) ) ); break;
				case 1: PackItem( new Bandage( Utility.RandomMinMax( 50, 100 ) ) ); break;
			}
		}

		public override int Meat{ get{ return 10; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Greater; } }
		public override double HitPoisonChance{ get{ return 0.10; } }
		public override bool Uncalmable{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 5; } }

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
				damage *= 5;
		}

		public NoxiousArcher( Serial serial ) : base( serial )
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
			switch( Utility.Random(500) )
				{
			case 0: c.DropItem( item = new SilenceHelm() ); break;
			case 1: c.DropItem( item = new SilenceLeggings() ); break;
			case 2: c.DropItem( item = new SilenceNecklace() ); break;
			case 3: c.DropItem( item = new SilenceRobe() ); break;
			case 4: c.DropItem( item = new SilenceShirt() ); break;
			case 5: c.DropItem( item = new SilenceShoes() ); break;
			        }
			base.OnDeath( c );
		}
	}
}