using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "an ogre corpse" )]
	public class Ogre : BaseCreature
	{
		[Constructable]
		public Ogre () : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an ogre";
			Body = 1;
			BaseSoundID = 427;

			SetStr( 148, 194 );
			SetDex( 48, 60 );
			SetInt( 46, 65 );

			SetDamage( 9, 11 );

			SetSkill( SkillName.MagicResist, 55.1, 70.0 );
			SetSkill( SkillName.Tactics, 60.1, 70.0 );
			SetSkill( SkillName.Wrestling, 74.1, 81.0 );

			Fame = 3000;
			Karma = -3000;

			VirtualArmor = 32;

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackGem(); break;
				case 1: PackPotion(); break;
				case 2: PackItem( new Arrow( Utility.Random( 1, 10 ) ) ); break;
			}

			Club weapon = new Club();

			weapon.DamageLevel = (WeaponDamageLevel)Utility.Random( 0, 3 );
			weapon.DurabilityLevel = (WeaponDurabilityLevel)Utility.Random( 0, 3 );
			weapon.AccuracyLevel = (WeaponAccuracyLevel)Utility.Random( 0, 3 );

			PackItem( weapon );

			PackGold( 50, 150 );
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			defender.Stam -= Utility.Random( 1, 5 );
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override int Meat{ get{ return 2; } }

		public Ogre( Serial serial ) : base( serial )
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