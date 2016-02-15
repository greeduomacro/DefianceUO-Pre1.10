using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests.Collector;

namespace Server.Mobiles
{
	[CorpseName( "a basilisk corpse" )]
	public class Basilisk : BaseCreature
	{
		[Constructable]
		public Basilisk() : base( AIType.AI_Melee, FightMode.Weakest, 10, 1, 0.2, 0.2 )
		{
			Name = "a basilisk";
			Body = 0xCE;
			Hue = Utility.RandomList( 2201, 2308, 2422, 2110, 2010 );
			BaseSoundID = 0x5A;

			SetStr( 450, 475 );
			SetDex( 150, 200 );

			SetHits( 400, 425 );

			SetDamage( 15, 25 );

			SetSkill( SkillName.MagicResist, 25.1, 100.0 );
			SetSkill( SkillName.Tactics, 20.1, 105.0 );
			SetSkill( SkillName.Anatomy, 20.1, 105.0 );
			SetSkill( SkillName.Wrestling, 20.1, 105.0 );
			SetSkill( SkillName.Poisoning, 100.0, 100.0 );

			Fame = 9000;
			Karma = -9000;

			VirtualArmor = 50;

			Tamable = true;
			ControlSlots = 4;
			MinTameSkill = 99.1;

			switch( Utility.Random(150) )
			{
				case 0: PackItem( new EnchantedWood() ); break;
			}

			PackGold( 500, 750 );
			PackWeapon( 0, 4 );
			PackArmor( 0, 4 );
			PackSlayer();
			PackItem( new Nightshade( Utility.Random( 2, 10 ) ) );

				if ( 0.1 > Utility.RandomDouble() )
					PackItem( new Obsidian() );
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 250 ) <  1 )
				c.DropItem( new WoodenBoard() );

            		base.OnDeath( c );
		}

		public override int Hides{ get{ return 15; } }
		public override HideType HideType{ get{ return HideType.Horned; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override bool BardImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override Poison HitPoison{ get{ return Poison.Deadly; } }
		public override double HitPoisonChance{ get{ return 0.50; } }

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
				damage *= 2;
			else if ( to is Horse || to.Player )
				damage = 0;
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			defender.Paralyze( new TimeSpan(0, 0, 0, 15, 0 ) );
			base.OnGaveMeleeAttack( defender );
		}

		public Basilisk(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
            		//Al: Even though the Poisoning skill is not used players like it to show 100
            		SetSkill(SkillName.Poisoning, 100);

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