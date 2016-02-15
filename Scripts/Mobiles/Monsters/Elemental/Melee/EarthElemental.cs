using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "an earth elemental corpse" )]
	public class EarthElemental : BaseCreature
	{
		[Constructable]
		public EarthElemental() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an earth elemental";
			Body = 14;
			BaseSoundID = 268;

			SetStr( 134, 152 );
			SetDex( 66, 84 );
			SetInt( 71, 92 );

			SetDamage( 9, 16 );

			SetSkill( SkillName.MagicResist, 52.1, 87.0 );
			SetSkill( SkillName.Tactics, 65.1, 91.6 );
			SetSkill( SkillName.Wrestling, 65.1, 92.9 );

			Fame = 3500;
			Karma = -3500;

			VirtualArmor = 34;
			ControlSlots = 2;

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackItem( new IronOre( Utility.RandomMinMax( 5, 15 ) ) ); break;

			}

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackItem( new FertileDirt( Utility.RandomMinMax( 1, 2 ) ) ); break;
				case 1: PackItem( new MandrakeRoot( Utility.RandomMinMax( 2, 5 ) ) ); break;
			}


			PackGem();
			PackGem();
			PackGold( 150, 250 );

                                switch ( Utility.Random( 500 ))
        		 {
           			case 0: PackItem( new RuinedBooks() ); break;
        		 }

                }

		public override void OnDeath( Container c )
	  	{
			if ( Utility.Random( 75 ) <  1 )
				c.DropItem( new BasicBlueCarpet( PieceType.SECorner ) );

			base.OnDeath( c );
	  	}

		public override int TreasureMapLevel{ get{ return 1; } }

		public EarthElemental( Serial serial ) : base( serial )
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