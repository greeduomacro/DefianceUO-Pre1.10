using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a centaur corpse" )]
	public class Centaur : BaseCreature
	{
		[Constructable]
		public Centaur() : base( AIType.AI_Archer, FightMode.Agressor, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "centaur" );
			Body = 101;
			BaseSoundID = 678;

			SetStr( 221, 288 );
			SetDex( 142, 239 );
			SetInt( 91, 100 );

			SetDamage( 13, 24 );

			SetSkill( SkillName.Anatomy, 95.1, 113.0 );
			SetSkill( SkillName.Archery, 95.1, 99.8 );
			SetSkill( SkillName.MagicResist, 56.3, 73.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.Wrestling, 95.5, 99.8 );

			Fame = 6500;
			Karma = 0;

			VirtualArmor = 50;

			switch ( Utility.Random( 8 ))
			{
				case 0: PackWeapon( 0, 3 ); break;
				case 1: PackArmor( 0, 4 ); break;
				case 2: PackItem( new Katana() ); break;
				case 3: PackItem( new BodySash() ); break;
				case 4: PackItem( new Halberd() ); break;
				case 5: PackItem( new ChainChest() ); break;
				case 6: PackItem( new StuddedChest() ); break;
				case 7: PackItem( new ChainLegs() ); break;
			}

			Bow bow = new Bow();

			bow.Movable = false;
			bow.Crafter = this;
			bow.Quality = WeaponQuality.Exceptional;

			AddItem( bow );

			PackGold( 250, 300 );
			PackItem( new Arrow( Utility.RandomMinMax( 25, 50 ) ) );
		}

		public Centaur( Serial serial ) : base( serial )
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