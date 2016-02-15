using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
        [CorpseName( "QueenDrow corpse" )]
	public class QueenDrow : BaseCreature
	{
		[Constructable]
		public QueenDrow() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "female" );
			Title = "Queen Drow";
			Body = 401;
			BaseSoundID = 333;
                        Hue = 1000;

			SetStr( 780, 840 );
			SetDex( 95, 100 );
			SetInt( 47, 90 );

                        SetHits( 470, 530 );

			SetDamage( 22, 30 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Fire, 20 );
			SetDamageType( ResistanceType.Cold, 20 );
			SetDamageType( ResistanceType.Poison, 20 );
			SetDamageType( ResistanceType.Energy, 20 );

			SetResistance( ResistanceType.Physical, 30 );
			SetResistance( ResistanceType.Fire, 30 );
			SetResistance( ResistanceType.Cold, 30 );
			SetResistance( ResistanceType.Poison, 30 );
			SetResistance( ResistanceType.Energy, 30 );

			SetSkill( SkillName.EvalInt, 100.0 );
			SetSkill( SkillName.Magery, 100.0 );
			SetSkill( SkillName.Meditation, 120.0 );
			SetSkill( SkillName.MagicResist, 150.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 120.0 );

			Fame = 1000;
			Karma = -1000;

                        //AddItem( new Silver(4000));

			AddItem( new Boots( Utility.RandomNeutralHue() ) );
			AddItem( new FemalePlateChest());
			AddItem( new Cloak( 139 ));

			PackItem( new Longsword() );
			PackScroll( 2, 3 );
			PackArmor( 1, 1 );
			PackWeapon( 1, 1 );
			PackWeapon( 1, 1 );
			PackSlayer();
			if( Utility.Random( 100 ) < 100 )

			switch ( Utility.Random( 500 ))
			{
				case 0: AddItem( new Longsword() ); break;
				case 1: AddItem( new Cutlass() ); break;
				case 2: AddItem( new Broadsword() ); break;
				case 3: AddItem( new Axe() ); break;
				case 4: AddItem( new Club() ); break;
				case 5: AddItem( new Dagger() ); break;
				case 6: AddItem( new Spear() ); break;
				case 7: AddItem( new CarpetCenter() ); break;
				case 8: AddItem( new CarpetCorner1() ); break;
				case 9: AddItem( new CarpetCorner2() ); break;
				case 10: AddItem( new CarpetCorner3() ); break;
				case 11: AddItem( new CarpetCorner4() ); break;
				case 12: AddItem( new CarpetPattern() ); break;
				case 13: AddItem( new CarpetPlain() ); break;
				case 14: AddItem( new CarpetSide1() ); break;
				case 15: AddItem( new CarpetSide2() ); break;
				case 16: AddItem( new CarpetSide3() ); break;
				case 17: AddItem( new CarpetSide4() ); break;






			}

			Item hair = new Item( Utility.RandomList( 0x203C, 0x203D ) );
			hair.Hue = 000;
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem( hair );

			PackGold( 400, 900 );
		}

		public override bool AlwaysMurderer{ get{ return true; } }

		public QueenDrow( Serial serial ) : base( serial )
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