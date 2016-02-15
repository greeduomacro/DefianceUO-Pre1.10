using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a ratman archer corpse" )]
	public class RatmanArcher : BaseCreature
	{
		[Constructable]
		public RatmanArcher() : base( AIType.AI_Archer, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "ratman" );
			Body = 0x8E;
			BaseSoundID = 437;

			SetStr( 146, 180 );
			SetDex( 101, 130 );
			SetInt( 116, 140 );

			SetHits( 88, 108 );

			SetDamage( 4, 10 );

			SetSkill( SkillName.Anatomy, 60.2, 100.0 );
			SetSkill( SkillName.Archery, 80.1, 90.0 );
			SetSkill( SkillName.MagicResist, 65.1, 90.0 );
			SetSkill( SkillName.Tactics, 50.1, 75.0 );
			SetSkill( SkillName.Wrestling, 50.1, 75.0 );

			Fame = 6500;
			Karma = -6500;

			VirtualArmor = 56;

			AddItem( new Bow() );
			PackItem( new Arrow( Utility.Random( 10, 40 ) ) );
			PackGold( 150, 250 );
			PackArmor( 0, 2 );
			PackWeapon( 0, 2 );
		}

		public override int Hides{ get{ return 12; } }
		public override HideType HideType{ get{ return HideType.Spined; } }

		public RatmanArcher( Serial serial ) : base( serial )
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

			if ( Body == 42 )
			{
				Body = 0x8E;
				Hue = 0;
			}
		}
	}
}