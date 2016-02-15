using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a noxious corpse" )]
	public class NoxiousWarrior : BaseCreature
	{
		public override bool IsScaryToPets{ get{ return true; } }

		[Constructable]
		public NoxiousWarrior() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.15, 0.2 )
		{
			Name = "a noxious warrior";
			Body = 0x47;
			Hue = 1367;
			BaseSoundID = 589;

			SetStr( 336, 385 );
			SetDex( 96, 115 );
			SetInt( 31, 55 );

			SetHits( 1002, 1231 );
			SetMana( 0 );

			SetDamage( 12, 23 );

			SetSkill( SkillName.MagicResist, 90.3, 95.0 );
			SetSkill( SkillName.Tactics, 90.1, 93.0 );
			SetSkill( SkillName.Wrestling, 90.1, 95.2 );
			SetSkill( SkillName.Poisoning, 97.1, 98.6 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 40;

			switch ( Utility.Random( 4 ) )
			{
				case 0: PackItem( new GreaterCurePotion() ); break;
				case 1: PackItem( new GreaterPoisonPotion() ); break;
			}

			PackGold( 600, 750 );

			switch ( Utility.Random( 2 ) )
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
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackItem( new Bandage( Utility.RandomMinMax( 50, 100 ) ) ); break;
			}

			if ( Utility.Random( 125 ) == 0 )
				PackItem( new RareBlueCarpet( PieceType.SWCorner ));
		}

		public override int Meat{ get{ return 10; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Greater; } }
		public override double HitPoisonChance{ get{ return 0.50; } }
		public override bool Uncalmable{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 5; } }

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
				damage *= 5;
		}

		public NoxiousWarrior( Serial serial ) : base( serial )
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