using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Factions;

namespace Server.Mobiles
{
	[CorpseName( "a demonic corpse" )]
	public class Rexxar : BaseBoss
	{

		[Constructable]
		public Rexxar () : base( AIType.AI_Melee, FightMode.Closest )
		{
			Name = "Rexxar";
			Title = "The murderer";
			Body = 400;
			BaseSoundID = 427;
			Hue = 1175;

			SetStr( 200, 400 );
			SetDex( 100, 150 );
			SetInt( 46, 70 );

			SetHits( 9000 );

			SetDamage( 20, 25 );

			SetSkill( SkillName.MagicResist, 125.1, 140.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.Wrestling, 90.1, 100.0 );

			Fame = 150000;
			Karma = -150000;

			VirtualArmor = 50;

			Bandana bandana = new Bandana();
			bandana.Hue = 1175;
			bandana.Movable = false;
			AddItem( bandana );

			ChainChest chest = new ChainChest();
			chest.Hue = 1645;
			chest.Movable = false;
			AddItem( chest );

			PlateArms arms = new PlateArms();
			arms.Hue = 1645;
			arms.Movable = false;
			AddItem( arms );

			PlateLegs legs = new PlateLegs();
			legs.Hue = 1645;
			legs.Movable = false;
			AddItem( legs );

			BodySash sash = new BodySash();
			sash.Hue = 1175;
			sash.Movable = false;
			AddItem( sash );

			DoubleAxe axe = new DoubleAxe();
			axe.Hue = 1175;
			axe.Movable = false;
			AddItem( axe );

		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
		}

		public override void OnDeath( Container c )
	  	{
			if ( Utility.Random( 95 ) <  1 )
				c.DropItem( new RexxarRare() );

			base.OnDeath( c );
	  	}

		public override int CanBandageSelf { get { return 20; } }
		public override bool DoAlwaysReflect { get { return true; } }
		public override bool DoDisarmPlayer{ get { return true; } }
		public override bool AutoDispel{ get{ return true; } }
		public override bool ThrowAtomicBomb{ get { return true; } }


		//Melee damage from controlled mobiles is divided by 2
		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Controlled )
				damage /= 4;
			}
		}

		//Melee damage to controlled mobiles is multiplied by 2
		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)to;

				if ( bc.Controlled )
				damage *= 3;
			}
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 6; } }
		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool ShowFameTitle{ get{ return true; } }

		public Rexxar( Serial serial ) : base( serial )
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