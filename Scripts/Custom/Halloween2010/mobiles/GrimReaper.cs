using System;
using Server;
using Server.Items;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "a horrific corpse" )]
	public class GrimReaper : BaseCreature
	{
		[Constructable]
		public GrimReaper() : base( AIType.AI_Melee, FightMode.Evil, 10, 1, 0.2, 0.4 )
		{
			Name = "Grim Reaper";
			Body = 400;
			Hue = 0x497;
			BaseSoundID = 427;

			SetStr( 200, 220 );
			SetDex( 201, 220 );
			SetInt( 200, 250 );

			SetHits( 2000, 2300 );

			SetDamage( 20, 40 );


			SetSkill( SkillName.EvalInt, 100.1, 110.0 );
			SetSkill( SkillName.Magery, 70.0);
			SetSkill( SkillName.Meditation, 100.0 );
			SetSkill( SkillName.Tactics, 100.0);
			SetSkill( SkillName.Wrestling, 150.0);

			Fame = 10000;
			Karma = -10000;

			VirtualArmor = 100;

			HoodedShroudOfShadows chest = new HoodedShroudOfShadows();
			chest.Name = "Shroud of the Grim Reaper";
			chest.Hue = 0x807;
			chest.Movable = false;
			AddItem( chest );

			Scythe hands = new Scythe();
			hands.Hue = 0x485;
			hands.Movable = false;
			AddItem( hands );

			WizardGlasses head = new WizardGlasses();
			head.Hue = 0x485;
			head.Movable = false;
			AddItem( head );

		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 1 );
			AddLoot( LootPack.Gems, 5 );
			AddLoot( LootPack.MedScrolls );
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 40 ) < 1 )
			c.DropItem( new HalloweenCandle() );

			if ( Utility.Random( 10 ) < 1 )
			c.DropItem( new HalloweenStatue() );

			base.OnDeath( c );
		}


		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( Utility.Random( 10 ) == 0 ) SpawnReaper( attacker );
		}

		public void SpawnReaper( Mobile target )
		{
			int reap = 0;

			foreach ( Mobile m in this.GetMobilesInRange( 20 ) )
			{
				if ( m is Reaper )
					++reap;
			}

			if ( reap < 5 )
			{
				BaseCreature reaper = new Reaper();

				Map map = target.Map;

				Point3D loc = this.Location;
				bool validLocation = false;

				for ( int j = 0; !validLocation && j < 10; ++j )
				{
					int x = this.X + 5;
					int y = this.Y + 5;
					int z = map.GetAverageZ( x, y );
				}

				reaper.MoveToWorld( loc, map );
			}
		}

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Controlled )
				damage /= 10;
			}

			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Summoned )
				damage /= 3;
			}
		}

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)to;

				if ( bc.Controlled )
				damage *= 15;
			}
		}



		public override int Meat{ get{ return 10; } }
		public override int TreasureMapLevel{ get{ return 5; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool BardImmune{ get{ return true; } }


		public GrimReaper( Serial serial ) : base( serial )
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