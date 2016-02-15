using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a skeletal dragon corpse" )]
	public class SkeletalDragon : BaseCreature
	{
		[Constructable]
		public SkeletalDragon () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a skeletal dragon";
			Body = 104;
			BaseSoundID = 0x488;

			SetStr( 898, 1030 );
			SetDex( 68, 200 );
			SetInt( 488, 620 );

			SetHits( 558, 599 );

			SetDamage( 29, 35 );

			SetSkill( SkillName.EvalInt, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 100.3, 130.0 );
			SetSkill( SkillName.Tactics, 97.6, 100.0 );
			SetSkill( SkillName.Wrestling, 97.6, 100.0 );

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 80;
			Tamable = true;
			MinTameSkill = 120;
			PackGem();
			PackGem();
			PackGem();
			PackJewel( 0.01 );
			PackGold( 1500, 1900 );
			PackArmor( 0, 5 );
			PackWeapon( 0, 5 );
			PackArmor( 0, 5 );
			PackWeapon( 0, 5 );

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}
		}

		public override bool AutoDispel{ get{ return true; } }
		public override int Meat{ get{ return 19; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override int Hides{ get{ return 20; } }
		public override HideType HideType{ get{ return HideType.Barbed; } }

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
				damage *= 2;
		}

		public SkeletalDragon( Serial serial ) : base( serial )
		{
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.25 >= Utility.RandomDouble() && attacker is BaseCreature )
			{
				BaseCreature c = (BaseCreature)attacker;

				if ( c.Controlled && c.ControlMaster != null )
				{
					c.ControlTarget = c.ControlMaster;
					c.ControlOrder = OrderType.Attack;
					c.Combatant = c.ControlMaster;
				}
			}
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