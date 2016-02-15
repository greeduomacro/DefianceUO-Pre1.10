using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a dragon corpse" )]
	public class MotmDragon : BaseCreature
	{
		[Constructable]
		public MotmDragon() : base( AIType.AI_Mage, FightMode.Evil, 10, 1, 0.2, 0.4 )
		{
			Name = "Drachenstein";
			Body = 103;
			Hue = 1145;
			BaseSoundID = 362;

			SetStr( 111, 140 );
			SetDex( 201, 220 );
			SetInt( 1001, 1040 );

			SetHits( 480 );

			SetDamage( 15, 30 );


			SetSkill( SkillName.EvalInt, 100.1, 110.0 );
			SetSkill( SkillName.Magery, 110.1, 120.0 );
			SetSkill( SkillName.Meditation, 100.0 );
			SetSkill( SkillName.MagicResist, 100.0 );
			SetSkill( SkillName.Tactics, 75.0, 95.0 );
			SetSkill( SkillName.Wrestling, 75.0, 100.0 );

			Fame = 10000;
			Karma = 10000;

			VirtualArmor = 215;

			PackReg( 25 );

			if ( Utility.Random( 75 ) < 1 ) PackItem( new Motmrock() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 1 );
			AddLoot( LootPack.FilthyRich, 1 );
			AddLoot( LootPack.Gems, 5 );
			AddLoot( LootPack.MedScrolls );
		}

		//spawns a dragon when hit (with magic) by a ratio of 1 in 3
			public override void OnDamagedBySpell( Mobile caster )
		{
			base.OnDamagedBySpell( caster );

			if ( Utility.Random( 3 ) < 1 ) SpawnDragon( caster );
		}

		public void SpawnDragon( Mobile target )
		{
			int drag = 0;

			foreach ( Mobile m in this.GetMobilesInRange( 20 ) )
			{
				if ( m is Dragon )
					++drag;
			}

			if ( drag < 5 )
			{
			BaseCreature dragon = new Dragon();

			Map map = target.Map;

			Point3D loc = this.Location;
			bool validLocation = false;

			for ( int j = 0; !validLocation && j < 10; ++j )
			{
				int x = this.X + 5;
				int y = this.Y + 5;
				int z = map.GetAverageZ( x, y );
			}

			dragon.MoveToWorld( loc, map );
		}
		}

		//Spell damage from controlled mobiles is scaled down by 0.1
		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			if ( caster is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)caster;

				if ( bc.Controlled )
				scalar = 0.1;
			}
		}

		//Melee damage from controlled mobiles is divided by 40 + if summoned damage x3
		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Controlled )
				damage /= 40;
			}

			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Summoned )
				damage *= 3;
			}
		}

				//Melee damage to controlled mobiles is multiplied by 10
				public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)to;

				if ( bc.Controlled )
				damage *= 10;
			}
		}



		public override bool HasBreath{ get{ return true; } } // fire breath enabled
		public override HideType HideType{ get{ return HideType.Barbed; } }
		public override int Hides{ get{ return 40; } }
		public override int Meat{ get{ return 10; } }
		public override int Scales{ get{ return 12; } }
		public override ScaleType ScaleType{ get{ return ( Utility.RandomBool() ? ScaleType.Black : ScaleType.White ); } }
		public override int TreasureMapLevel{ get{ return 5; } }

		public MotmDragon( Serial serial ) : base( serial )
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