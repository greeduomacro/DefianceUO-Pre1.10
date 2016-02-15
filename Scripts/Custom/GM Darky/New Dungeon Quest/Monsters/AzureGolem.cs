using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a shimmering golem corpse" )]
	public class AzureGolem : BaseCreature
	{
		[Constructable]
		public AzureGolem() : base( AIType.AI_Melee, FightMode.Weakest, 10, 1, 0.2, 0.3 )
		{
			Name = "an azure golem";
			Body = 752;
			BaseSoundID = 541;
			Hue = 185;
			Kills = 10;
			ShortTermMurders = 10;

			SetStr( 600, 700 );
			SetDex( 300, 500 );
			SetInt( 0, 0 );

			SetHits( 1000, 1300 );

			SetDamage( 7, 10 );

			SetSkill( SkillName.MagicResist, 1000.0 );
			SetSkill( SkillName.Tactics, 150.0 );
			SetSkill( SkillName.Wrestling, 100.0 );
			SetSkill( SkillName.Anatomy, 90.0 );

			Fame = 25000;
			Karma = -25000;

			VirtualArmor = 0;

			Item item = null;
			switch( Utility.Random(25) )
		{
			case 0: PackItem( item = new ClockworkAssembly() ); break;
		}
			if (item != null)
			{
			item.Movable = true;
			item.ItemID = (Utility.RandomBool() ? 7712 : 4084 );
			item.Name = "Book of Golem Crafting";
			item.Hue = 0;
			}

			PackGold( 1500, 3500 );
			PackArmor( 0, 5 );
			PackArmor( 0, 5 );
			PackWeapon( 0, 5 );
			PackWeapon( 0, 5 );
			PackSlayer();

				if ( 0.10 > Utility.RandomDouble() )
					PackItem( new PowerCrystal() );

				if ( 0.50 > Utility.RandomDouble() )
					PackItem( new Gears(5) );

				if ( 0.03 > Utility.RandomDouble() )
					PackItem( new DarkIronWire() );
		}

		public override int GetAngerSound()
		{
			return 541;
		}

		public override int GetIdleSound()
		{
			return 541;
		}

		public override int GetAttackSound()
		{
			return 562;
		}

		public override int GetHurtSound()
		{
			return 320;
		}

		public override int GetDeathSound()
		{
			return 542;
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			defender.Damage( Utility.Random( 1, 6 ), this );
			defender.Mana -= Utility.Random( 10, 50 );
		}

		public override bool BardImmune{ get{ return true;} }
		public override bool AutoDispel{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override int TreasureMapLevel{ get{ return 5; } }

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
			{
			scalar = 0.0; // Immune to magic
			}

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
				damage *= 5;
		}

	        public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );


			attacker.Damage( Utility.Random( 1, 3 ), this );
			attacker.Mana -= Utility.Random( 10, 50 );

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

		public AzureGolem( Serial serial ) : base( serial )
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