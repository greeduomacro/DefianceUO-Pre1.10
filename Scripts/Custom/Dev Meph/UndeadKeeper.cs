using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an unatural corpse" )]
	public class UndeadKeeper : BaseCreature
	{
		[Constructable]
		public UndeadKeeper() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.6, 1.2 )
		{
			Name = "guardian of the dead";
			Body = 40;
			Hue = 1409;

			SetStr( 801, 900 );
			SetDex( 65, 80 );
			SetInt( 36, 50 );

			SetHits( 1000 );

			SetDamage( 10, 23 );

			SetSkill( SkillName.MagicResist, 90.1, 95.0 );
			SetSkill( SkillName.Tactics, 70.1, 85.0 );
			SetSkill( SkillName.Wrestling, 65.1, 80.0 );

			Fame = 10000;
			Karma = -10000;

			PackReg( 30 );

			if ( Utility.Random( 200 ) < 1 ) PackItem( new Tombstone() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich );
		}

		//Melee damage to controlled mobiles is multiplied by 10
		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)to;

				if ( bc.Controlled )
				damage *= 7;
			}
		}

		//Melee damage from controlled mobiles is divided by 20
		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Controlled )
				damage /= 20;
			}
		}

		//Spell damage from controlled mobiles is scaled down by 0.01
		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			if ( caster is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)caster;

				if ( bc.Controlled )
				scalar = 0.01;
			}
		}

		public override Poison HitPoison{ get{ return Poison.Deadly; } }
		public override bool AutoDispel{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }

		public UndeadKeeper( Serial serial ) : base( serial )
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