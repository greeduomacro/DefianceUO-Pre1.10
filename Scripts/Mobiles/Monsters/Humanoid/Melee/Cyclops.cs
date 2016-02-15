using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a cyclops corpse" )]
	public class Cyclops : BaseCreature
	{
		[Constructable]
		public Cyclops() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a cyclops";
			Body = 75;
			BaseSoundID = 604;

			SetStr( 339, 381 );
			SetDex( 100, 115 );
			SetInt( 31, 55 );

			SetDamage( 12, 23 );

			SetSkill( SkillName.MagicResist, 77.6, 102.1 );
			SetSkill( SkillName.Tactics, 80.6, 97.9 );
			SetSkill( SkillName.Wrestling, 83.1, 91.0 );

			Fame = 4500;
			Karma = -4500;

			VirtualArmor = 48;

			switch ( Utility.Random( 4 ) )
			{
				case 0: PackGem(); break;
				case 1: PackPotion(); break;
				case 2: PackItem( new Arrow( Utility.Random( 1, 10 ) ) ); break;
			}

			PackGold( 300, 350 );
			PackArmor( 0, 4 );
			PackWeapon( 0, 4 );
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			defender.Stam -= Utility.Random( 5, 10 );
		}

		public override int Meat{ get{ return 4; } }
		public override int TreasureMapLevel{ get{ return 3; } }

		public Cyclops( Serial serial ) : base( serial )
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