using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Targeting;
using Server.Misc;

namespace Server.Mobiles
{
	[CorpseName( "an ettin's corpse" )]
	public class EttinElder : BaseCreature
	{

		[Constructable]
		public EttinElder() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an ettin elder";
			Body = 2;
			Hue = 22222;
			BaseSoundID = 367;

			SetStr( 126, 155 );
			SetDex( 40 );
			SetInt( 71, 92 );

			SetHits( 2000 );

			SetDamage( 65, 75 );

			SetSkill( SkillName.MagicResist, 190.0 );
			SetSkill( SkillName.Wrestling, 65.0 );


			Fame = 3500;
			Karma = -3500;

			VirtualArmor = 34;

			//1 in 250 chance of getting an ettinhammer to drop as loot
			if ( Utility.Random( 250 ) < 1 ) PackItem( new ettinhammer() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 2 );
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

				//Melee damage to controlled mobiles is multiplied by 5
				public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)to;

				if ( bc.Controlled )
				damage *= 5;
			}
		}


		public override int TreasureMapLevel{ get{ return 5; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool BardImmune{ get{ return true; } }
		public override bool AutoDispel{ get{ return true; } }

		public EttinElder( Serial serial ) : base( serial )
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