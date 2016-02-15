using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a noxious corpse" )]
	public class NoxiousMage : BaseCreature
	{
		public override bool IsScaryToPets{ get{ return true; } }

		[Constructable]
		public NoxiousMage() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.15, 0.2 )
		{
			Name = "a noxious mageweaver";
			Body = 0x48;
			Hue = 1367;
			BaseSoundID = 589;

			SetStr( 536, 585 );
			SetDex( 126, 145 );
			SetInt( 281, 355 );

			SetHits( 1222, 1351 );
			SetMana( 900, 1200 );

			SetDamage( 13, 16 );

			SetSkill( SkillName.EvalInt, 75.1, 80.0 );
			SetSkill( SkillName.Magery, 95.1, 97.0 );
			SetSkill( SkillName.MagicResist, 106.2, 110.0 );
			SetSkill( SkillName.Tactics, 70.1, 75.0 );
			SetSkill( SkillName.Wrestling, 46.1, 50.0 );
			SetSkill( SkillName.Poisoning, 97.1, 98.0 );

			Fame = 18000;
			Karma = -18000;

			VirtualArmor = 20;

			switch ( Utility.Random( 4 ) )
			{
				case 0: PackItem( new GreaterCurePotion() ); break;
				case 1: PackItem( new GreaterPoisonPotion() ); break;
			}

			PackGold( 900, 1200 );

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

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackItem( new Bandage( Utility.RandomMinMax( 50, 100 ) ) ); break;
			}

		}

		public override bool AutoDispel{ get{ return true; } }
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

		public NoxiousMage( Serial serial ) : base( serial )
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