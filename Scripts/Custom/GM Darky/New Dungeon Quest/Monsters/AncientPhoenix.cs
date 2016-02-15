using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a phoenix corpse" )]
	public class AncientPhoenix : BaseCreature
	{
		[Constructable]
		public AncientPhoenix() : base( AIType.AI_Mage, FightMode.Agressor, 10, 1, 0.1, 0.2 )
		{
			Name = "an ancient phoenix";
			Body = 5;
			Hue = 0x674;
			BaseSoundID = 0x8F;
			Kills = 10;
			ShortTermMurders = 10;

			SetStr( 650, 750 );
			SetDex( 375, 375 );
			SetInt( 1000, 1000 );

			SetHits( 950, 1050 );

			SetDamage( 25, 29 );

			SetSkill( SkillName.EvalInt, 95.2, 98.0 );
			SetSkill( SkillName.Magery, 97.2, 98.9 );
			SetSkill( SkillName.Meditation, 85.1, 88.0 );
			SetSkill( SkillName.MagicResist, 126.0, 135.0 );
			SetSkill( SkillName.Tactics, 120.1, 130.0 );
			SetSkill( SkillName.Wrestling, 120.1, 130.0 );

			Fame = 25000;
			Karma = -25000;

			VirtualArmor = 60;

			switch( Utility.Random(100) )
			{
				case 0: PackItem( new DarkIronWire() ); break;
			}

			PackGold( 1200, 2000 );
			PackArmor( 0, 4 );
			PackWeapon( 0, 5 );
			PackArmor( 0, 5 );
			PackWeapon( 0, 4 );
			PackSlayer();

		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 250 ) <  1 )
				c.DropItem( new TreeStumps() );

			
			if ( Utility.Random( 75 ) <  1 )
				c.DropItem( new BasicPinkCarpet( PieceType.EastEdge ) );

            		base.OnDeath( c );
		}

		public override int Meat{ get{ return 4; } }
		public override MeatType MeatType{ get{ return MeatType.Bird; } }
		public override int Feathers{ get{ return 100; } }

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
				damage *= 3;
		}

		public AncientPhoenix( Serial serial ) : base( serial )
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