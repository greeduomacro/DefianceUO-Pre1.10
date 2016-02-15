using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a demonic corpse" )]
	public class DemonScholar : BaseCreature
	{
		[Constructable]
		public DemonScholar() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a demonic scholar";
			Body = 400;
			BaseSoundID = 412;
			Hue = 1175;

			SetStr( 416, 505 );
			SetDex( 146, 165 );
			SetInt( 566, 655 );

			SetHits( 250, 303 );

			SetDamage( 11, 13 );

			SetSkill( SkillName.EvalInt, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 150.5, 200.0 );
			SetSkill( SkillName.Tactics, 50.1, 70.0 );
			SetSkill( SkillName.Wrestling, 60.1, 80.0 );

			Fame = 18000;
			Karma = -18000;

			VirtualArmor = 50;

			Lantern hands = new Lantern();
			hands.Name = "latern of souls";
			hands.Hue = 1258;
			hands.Movable = false;
			AddItem( hands );

			Spellbook hands2 = new Spellbook();
			hands2.Name = "ancient spellbook";
			hands2.Hue = 1158;
			hands2.Movable = false;
			hands2.Content = ulong.MaxValue;
			AddItem( hands2 );

			HoodedShroudOfShadows chest = new HoodedShroudOfShadows();
			chest.Name = "hooded shroud";
			chest.Hue = 1175;
			chest.Movable = false;
			AddItem( chest );

			Sandals feet = new Sandals();
			feet.Name = "sandals";
			feet.Hue = 1258;
			feet.Movable = false;
			AddItem( feet );

			PackReg( 30 );

			if ( Utility.Random( 2500 ) == 0 ) PackItem( new DemonSandals() );
			if ( Utility.Random( 2500 ) == 0 ) PackItem( new DemonDoublet() );
			if ( Utility.Random( 2500 ) == 0 ) PackItem( new DemonWizardsHat() );
			if ( Utility.Random( 2500 ) == 0 ) PackItem( new DemonHalfApron() );
			if ( Utility.Random( 2500 ) == 0 ) PackItem( new DemonSkirt() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.MedScrolls, 4 );
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

		public DemonScholar( Serial serial ) : base( serial )
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