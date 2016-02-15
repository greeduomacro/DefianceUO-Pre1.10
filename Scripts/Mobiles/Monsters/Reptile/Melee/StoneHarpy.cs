using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a stone harpy corpse" )]
	public class StoneHarpy : BaseCreature
	{
		[Constructable]
		public StoneHarpy() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a stone harpy";
			Body = 73;
			BaseSoundID = 402;

			SetStr( 296, 320 );
			SetDex( 86, 110 );
			SetInt( 51, 75 );

			SetHits( 178, 192 );
			SetMana( 0 );

			SetDamage( 10, 16 );

			SetSkill( SkillName.MagicResist, 50.1, 65.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.Wrestling, 90.1, 100.0 );

			Fame = 4500;
			Karma = -4500;

			VirtualArmor = 50;

			PackGem();
			PackGem();
			PackGem();
			PackGem();
			PackGold( 400, 500 );
		}

		public override void OnDeath( Container c )
	  	{
			if ( Utility.Random( 75 ) <  1 )
				c.DropItem( new BasicPinkCarpet( PieceType.NorthEdge ) );

			base.OnDeath( c );
	  	}

		public override int Meat{ get{ return 1; } }
		public override int Feathers{ get{ return 100; } }

		public StoneHarpy( Serial serial ) : base( serial )
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