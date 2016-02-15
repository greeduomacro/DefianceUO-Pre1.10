using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
	public class Brigandboss : BaseCreature
	{
		public override bool ClickTitle{ get{ return false; } }

		[Constructable]
		public Brigandboss() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			SpeechHue = Utility.RandomDyedHue();
			Title = "the master brigand";
			Hue = Utility.RandomSkinHue();

			if ( this.Female = Utility.RandomBool() )
			{
				Body = 0x191;
				Name = NameList.RandomName( "female" );
				AddItem( new Skirt( Utility.RandomNeutralHue() ) );
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "male" );
				AddItem( new ShortPants( Utility.RandomNeutralHue() ) );
			}

			SetStr( 125, 250 );
			SetDex( 81, 95 );
			SetInt( 61, 75 );

			SetHits( 2000 );
			SetMana( 10000 );

			SetDamage( 40, 60 );

			SetSkill( SkillName.MagicResist, 180.0, 190.0 );
			SetSkill( SkillName.Magery, 225.0, 250.0 );
			SetSkill( SkillName.EvalInt, 225.0, 250.0 );
			SetSkill( SkillName.Tactics, 120.0, 150.0 );
			SetSkill( SkillName.Wrestling, 120.0, 150.0 );
			SetSkill( SkillName.Anatomy, 120.0, 150.0 );

			Fame = 20000;
			Karma = -20000;

			AddItem( new Boots( Utility.RandomNeutralHue() ) );
			AddItem( new FancyShirt());
			AddItem( new Bandana());
			AddItem( Server.Items.Hair.GetRandomHair( Female ) );

			PackReg( 30 );

            if (Utility.Random(75) < 1) //125
            {
                switch (Utility.Random(5))
                {
                    case 0: PackItem(new Brigandboots()); break;
                    case 1: PackItem(new Brigandpants()); break;
                    case 2: PackItem(new Brigandbandana()); break;
                    case 3: PackItem(new Brigandskirt()); break;
                    case 4: PackItem(new Brigandshirt()); break;
                }
            }
        }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich );
		}

		//Spell damage from controlled mobiles is scaled down by 0.10
		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			if ( caster is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)caster;

				if ( bc.Controlled )
				scalar = 0.10;
			}
		}

		//Melee damage from controlled mobiles is divided by 6
		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Controlled )
				damage /= 6;
			}
		}

		public override bool DisallowAllMoves{ get{ return true; } }
		public override bool AlwaysMurderer{ get{ return true; } }
		public override Poison HitPoison{ get{ return Poison.Lethal; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 5; } }
		public override bool BardImmune{ get{ return true; } }
		public override bool AutoDispel{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }

		public Brigandboss( Serial serial ) : base( serial )
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
	}
}