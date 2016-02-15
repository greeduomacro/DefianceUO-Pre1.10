using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a gargoyle corpse" )]
	public class Gargoyle : BaseCreature
	{
		[Constructable]
		public Gargoyle() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a gargoyle";
			Body = 4;
			BaseSoundID = 372;

			SetStr( 159, 175 );
			SetDex( 76, 94 );
			SetInt( 81, 100 );

			SetDamage( 7, 14 );

			SetSkill( SkillName.Magery, 75.1, 86.0 );
			SetSkill( SkillName.MagicResist, 70.1, 85.0 );
			SetSkill( SkillName.Tactics, 50.1, 70.0 );
			SetSkill( SkillName.Wrestling, 40.1, 80.0 );

			Fame = 3500;
			Karma = -3500;

			VirtualArmor = 32;

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackGem(); break;
				case 1: PackPotion(); break;
				case 2: PackItem( new Arrow( Utility.Random( 10, 15 ) ) ); break;
				case 3: PackScroll( 1, 5 ); break;
			}

			PackGold( 50, 150 );

			if ( 0.05 > Utility.RandomDouble() )
				PackItem( new GargoylesPickaxe() );
		}

		public override void OnDeath( Container c )
	  	{
			if ( Utility.Random( 75 ) <  1 )
				c.DropItem( new BasicPinkCarpet( PieceType.SWCorner ) );

			base.OnDeath( c );
	  	}

		public override int TreasureMapLevel{ get{ return 1; } }
		public override int Meat{ get{ return 1; } }

		public Gargoyle( Serial serial ) : base( serial )
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