using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a sea monsters corpse" )]
	public class SeaMonster : BaseCreature
	{
		[Constructable]
		public SeaMonster() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "creature from the depths";
			Body = 77;
			BaseSoundID = 353;
			Hue = 2306;

			SetStr( 125, 150 );
			SetDex( 30, 40 );
			SetInt( 350, 400 );

			SetHits( 2000 );
			SetMana( 300 );

			SetDamage( 65, 70 );

			SetSkill( SkillName.MagicResist, 190, 200 );
			SetSkill( SkillName.Tactics, 90, 100 );
			SetSkill( SkillName.Wrestling, 90, 100 );
			SetSkill( SkillName.Anatomy, 90, 100);
			SetSkill( SkillName.Magery, 100, 110);
			SetSkill( SkillName.EvalInt, 100, 110);

			Fame = 23000;
			Karma = -23000;

			VirtualArmor = 50;

			PackGold( 750, 1000 );
			CanSwim = true;
			CantWalk = true;

			if ( Utility.Random( 150 ) < 1 ) PackItem( new MonsterChest() );

		}




			//spawns a deepseaserpent when hit by a ratio of 1 in 15
			public override void OnGotMeleeAttack( Mobile defender )
		{
			base.OnGotMeleeAttack( defender );

			if ( Utility.Random( 15 ) < 1 ) SpawnDeepSeaSerpent( defender );
		}

		public void SpawnDeepSeaSerpent( Mobile target )
		{
			int deep = 0;

			foreach ( Mobile m in this.GetMobilesInRange( 10 ) )
			{
				if ( m is DeepSeaSerpent )
					++deep;
			}

			if ( deep < 5 )
		{
			BaseCreature serpent = new DeepSeaSerpent();

			Map map = target.Map;

			Point3D loc = this.Location;
			bool validLocation = false;

			for ( int j = 0; !validLocation && j < 10; ++j )
			{
				int x = this.X + 5;
				int y = this.Y + 5;
				int z = map.GetAverageZ( x, y );
			}

			serpent.MoveToWorld( loc, map );
		}
		}

		//spawns a deepseaserpent when hit (with magic) by a ratio of 1 in 15
			public override void OnDamagedBySpell( Mobile caster )
		{
			base.OnDamagedBySpell( caster );

			if ( Utility.Random( 15 ) < 1 ) SpawnDeepSeaSerpent2( caster );
		}

		public void SpawnDeepSeaSerpent2( Mobile target )
		{
			int deep = 0;

			foreach ( Mobile m in this.GetMobilesInRange( 10 ) )
			{
				if ( m is DeepSeaSerpent )
					++deep;
			}

			if ( deep < 5 )
			{
			BaseCreature serpent = new DeepSeaSerpent();

			Map map = target.Map;

			Point3D loc = this.Location;
			bool validLocation = false;

			for ( int j = 0; !validLocation && j < 10; ++j )
			{
				int x = this.X + 5;
				int y = this.Y + 5;
				int z = map.GetAverageZ( x, y );
			}

			serpent.MoveToWorld( loc, map );
		}
		}

			//multiply melee damage from players by 5
			//Melee damage from controlled mobiles is divided by 15
			public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is PlayerMobile )
				damage *= 5;

			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Controlled )
				damage /= 15;
			}
		}

			//multiply spell damage from players by a scalar of 1.25
			//Spell damage from controlled mobiles is scaled down by 0.01
			public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			if ( caster is PlayerMobile )
				scalar = 1.30;

			if ( caster is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)caster;

				if ( bc.Controlled )
				scalar = 0.01;
			}
		}


		//Melee damage to controlled mobiles is multiplied by 2
		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)to;

				if ( bc.Controlled )
				damage *= 2;
			}
		}


		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich );
		}


		public override int TreasureMapLevel{ get{ return 5; } }
		public override bool CanRummageCorpses{ get{ return true; } }

		public SeaMonster( Serial serial ) : base( serial )
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