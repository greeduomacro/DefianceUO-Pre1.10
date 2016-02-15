using System;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Seventh;
using Server.Spells.Fifth;
using Server.Engines.CannedEvil;
using Server.Engines.Quests.Doom;

namespace Server.Mobiles
{
	public class AidonCopy : BaseCreature
	{
		[Constructable]
		public AidonCopy():base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.15, 0.2 )
		{
			Body = 400;
			Hue = 0x3F6;
			Name = "Aidon the Archwizard";
			Kills = 10;
			ShortTermMurders = 10;

			SetStr( 356, 396 );
			SetDex( 125, 135 );
			SetInt( 830, 953 );

			SetDamage( 15, 20 );

			SetSkill( SkillName.Wrestling, 91.3, 97.8 );
			SetSkill( SkillName.Tactics, 91.5, 97.0 );
			SetSkill( SkillName.MagicResist, 140.6, 156.8);
			SetSkill( SkillName.Magery, 96.7, 99.8 );
			SetSkill( SkillName.EvalInt, 75.1, 80.1 );
			SetSkill( SkillName.Meditation, 61.1, 68.1 );

			Fame = 17500;
			Karma = -17500;

			VirtualArmor = 15;

			Item Robe = new Robe();
			Robe.Hue=2112;
			EquipItem( Robe );

                        Item SavageMask = new SavageMask();
			SavageMask.Movable=false;
			SavageMask.Hue=1175;
			EquipItem( SavageMask );

                        Item Sandals = new Sandals();
			Sandals.Movable=false;
			Sandals.Hue=1175;
			EquipItem( Sandals );

			Item GoldRing = new GoldRing();
			GoldRing.Movable=false;
			GoldRing.Hue=1360;
			EquipItem( GoldRing );

			Item hair = new Item( 0x203B);
			hair.Hue = 1072;
			hair.Layer = Layer.Hair;
			AddItem( hair );

			Item beard = new Item( 0x203E);
			beard.Hue = 1072;
			beard.Layer = Layer.FacialHair;
			AddItem( beard );

			switch ( Utility.Random( 10 ) )
			{
				case 0: PackItem( new GreaterAgilityPotion() ); break;
				case 1: PackItem( new GreaterExplosionPotion() ); break;
				case 2: PackItem( new GreaterCurePotion() ); break;
				case 3: PackItem( new GreaterHealPotion() ); break;
				case 4: PackItem( new NightSightPotion() ); break;
				case 5: PackItem( new GreaterPoisonPotion() ); break;
				case 6: PackItem( new TotalRefreshPotion() ); break;
				case 7: PackItem( new GreaterStrengthPotion() ); break;
			}

			switch ( Utility.Random( 20 ) )
			{
				case 0: PackWeapon( 2, 5 ); break;
				case 1: PackArmor( 2, 5 ); break;
			}

			switch ( Utility.Random( 10 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 2 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackReg( 11 ); break;
				case 1: PackScroll( 4, 8 ); break;
				case 2: PackScroll( 6, 7 ); break;
				case 3: PackReg( 12 ); break;
				case 4: PackReg( 10 ); break;
			}

			PackGold( 500, 1000 );

		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool ShowFameTitle{ get{ return false; } }

		public AidonCopy( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override void OnDeath( Container c )
		{
			Item item = null;
			switch( Utility.Random(10) )
				{
			case 0: c.DropItem( item = new FameIounStone() ); break;
			case 1: c.DropItem( item = new KarmaIounStone() ); break;
			        }
			base.OnDeath( c );
		}
	}
}