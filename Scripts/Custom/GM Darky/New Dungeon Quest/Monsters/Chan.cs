using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a magical corpse" )]
	public class Chan : BaseCreature
	{
		[Constructable]
		public Chan() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Chan";
			Body = 788;

			SetStr( 550 );
			SetDex( 150 );
			SetInt( 50 );

			SetHits( 31000 );

			SetDamage( 30, 35 );

			SetSkill( SkillName.MagicResist, 150 );
			SetSkill( SkillName.Tactics, 225 );
			SetSkill( SkillName.Wrestling, 225 );

			Fame = 20000;
			Karma = -20000;

			VirtualArmor = 10;

			PackGold( 7500, 8500 );

			PackWeapon( 1, 5 );

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}

			switch ( Utility.Random( 10 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}

			switch ( Utility.Random( 15 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}

			switch ( Utility.Random( 20 ) )
			{
				case 0: PackWeapon( 2, 5 ); break;
				case 1: PackArmor( 2, 5 ); break;
			}

			switch ( Utility.Random( 25 ) )
			{
				case 0: PackWeapon( 3, 5 ); break;
				case 1: PackArmor( 3, 5 ); break;
			}
		}

		public override int GetIdleSound()
		{
			return 178;
		}

		public override int GetAngerSound()
		{
			return 179;
		}

		public override int GetAttackSound()
		{
			return 704;
		}

		public override int GetHurtSound()
		{
			return 180;
		}

		public override int GetDeathSound()
		{
			return 683;
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool DisallowAllMoves{ get{ return true; } }
		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool Uncalmable{ get{ return true; } }

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			scalar = 0.0; // Immune to magic
		}

		public Chan( Serial serial ) : base( serial )
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
			c.DropItem( new ChanHeart() );
			base.OnDeath( c );
		}
	}
}