using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Factions;

namespace Server.Mobiles
{
	[CorpseName( "a demonic corpse" )]
	public class DemonExec : BaseCreature
	{
		public override Faction FactionAllegiance{ get{ return Minax.Instance; } }

		[Constructable]
		public DemonExec () : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a demonic executioner";
			Body = 400;
			BaseSoundID = 427;
			Hue = 1175;

			SetStr( 767, 945 );
			SetDex( 66, 75 );
			SetInt( 46, 70 );

			SetHits( 476, 552 );

			SetDamage( 20, 25 );

			SetSkill( SkillName.MagicResist, 125.1, 140.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.Wrestling, 90.1, 100.0 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 50;

			NorseHelm helm = new NorseHelm();
			helm.Hue = 1175;
			helm.Movable = false;
			AddItem( helm );

			PlateChest chest = new PlateChest();
			chest.Hue = 1175;
			chest.Movable = false;
			AddItem( chest );

			PlateArms arms = new PlateArms();
			arms.Hue = 1175;
			arms.Movable = false;
			AddItem( arms );

			PlateLegs legs = new PlateLegs();
			legs.Hue = 1175;
			legs.Movable = false;
			AddItem( legs );

			Kilt kilt = new Kilt();
			kilt.Hue = 1158;
			kilt.Movable = false;
			AddItem( kilt );

			LongHair hair = new LongHair();
			hair.Hue = 1154;
			hair.Movable = false;
			hair.Layer = Layer.Hair;
			AddItem( hair );

			Halberd hands = new Halberd();
			hands.Hue = 1258;
			hands.Movable = false;
			AddItem( hands );

			if ( Utility.Random( 750 ) == 0 ) PackItem( new DemonSandals() );
			if ( Utility.Random( 750 ) == 0 ) PackItem( new DemonDoublet() );
			if ( Utility.Random( 750 ) == 0 ) PackItem( new DemonWizardsHat() );
			if ( Utility.Random( 750 ) == 0 ) PackItem( new DemonHalfApron() );
			if ( Utility.Random( 750 ) == 0 ) PackItem( new DemonSkirt() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
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

		//Melee damage from controlled mobiles is divided by 10
		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Controlled )
				damage /= 10;
			}
		}

				//Melee damage to controlled mobiles is multiplied by 6
				public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)to;

				if ( bc.Controlled )
				damage *= 6;
			}
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Regular; } }
		public override int TreasureMapLevel{ get{ return 4; } }
		public override bool BardImmune{ get{ return true; } }
		public override bool AlwaysMurderer{ get{ return true; } }

		public override bool ShowFameTitle{ get{ return false; } }

		public DemonExec( Serial serial ) : base( serial )
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