using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a gargoyle corpse" )]
	public class StoneGargoyle : BaseCreature
	{
		[Constructable]
		public StoneGargoyle() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a stone gargoyle";
			Body = 67;
			BaseSoundID = 0x174;

			SetStr( 246, 275 );
			SetDex( 76, 95 );
			SetInt( 81, 105 );

			SetHits( 148, 165 );

			SetDamage( 11, 17 );

			SetSkill( SkillName.MagicResist, 85.1, 100.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.Wrestling, 90.1, 100.0 );

			Fame = 4000;
			Karma = -4000;

			VirtualArmor = 50;

			PackGem();
			PackGem();
			PackScroll( 0, 4 );
			PackPotion();
			PackItem( new Arrow( Utility.Random( 1, 10 ) ) );
			PackGold( 150, 250 );
			PackItem( new IronOre( Utility.RandomMinMax( 1, 10 ) ) );

			if ( 0.05 > Utility.RandomDouble() )
				PackItem( new GargoylesPickaxe() );
		}

		public override int TreasureMapLevel{ get{ return 2; } }

		public StoneGargoyle( Serial serial ) : base( serial )
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