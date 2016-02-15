using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Engines.Quests.Collector;

namespace Server.Mobiles
{
	[CorpseName( "a harpy hag corpse" )]
	public class HarpyHag : BaseCreature
	{
		[Constructable]
		public HarpyHag() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.12, 0.2 )
		{
			Name = "a harpy hag";
			Body = 73;
			BaseSoundID = 402;
			Hue = 1227;

			SetStr( 326, 350 );
			SetDex( 186, 210 );
			SetInt( 71, 95 );

			SetHits( 198, 232 );
			SetMana( 0 );

			SetDamage( 15, 23 );

			SetSkill( SkillName.MagicResist, 90.1, 105.0 );
			SetSkill( SkillName.Tactics, 90.1, 110.0 );
			SetSkill( SkillName.Wrestling, 90.1, 110.0 );
			SetSkill( SkillName.Anatomy, 90.1, 110.0 );

			Fame = 9500;
			Karma = -9500;

			VirtualArmor = 70;

			PackGem();
			PackPotion();
			PackGold( 275, 375 );
			PackArmor( 0, 3 );
			PackWeapon( 0, 5 );
			PackArmor( 0, 5 );
			PackWeapon( 0, 3 );
			PackItem( Loot.RandomWeapon() );

				if ( 0.06 > Utility.RandomDouble() )
					PackItem( new Obsidian() );
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			defender.Damage( Utility.Random( 5, 5 ), this );
			defender.Mana -= Utility.Random( 20, 30 );
		}

	        public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			attacker.Damage( Utility.Random( 5, 5 ), this );
			attacker.Mana -= Utility.Random( 20, 30 );
		}

		public override int Meat{ get{ return 7; } }
		public override int Feathers{ get{ return 150; } }
		public override int TreasureMapLevel{ get{ return 3; } }
		public override bool Unprovokable{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }

		public HarpyHag( Serial serial ) : base( serial )
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
	}
}