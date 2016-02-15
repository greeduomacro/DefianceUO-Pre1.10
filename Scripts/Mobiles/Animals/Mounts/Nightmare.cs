using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a nightmare corpse" )]
	public class Nightmare : BaseMount
	{
		private static int[] m_IDs = new int[]
			{
				114, 16041,
				116, 16039,
				179, 16055,
			};

		[Constructable]
		public Nightmare() : this( "a nightmare" )
		{
		}

		[Constructable]
		public Nightmare( string name ) : base( name, 116, 16039, AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			int random = Utility.Random( 3 );

			Body = m_IDs[random * 2];
			ItemID = m_IDs[random * 2 + 1];
			BaseSoundID = 0xA8;

			SetStr( 496, 525 );
			SetDex( 96, 116 );
			SetInt( 86, 125 );

			SetHits( 298, 315 );

			SetDamage( 16, 22 );

			SetSkill( SkillName.Anatomy, 11.7, 14.4 );
			SetSkill( SkillName.Meditation, 32.5, 35.0 );
			SetSkill( SkillName.EvalInt, 10.1, 15.3 );
			SetSkill( SkillName.Magery, 10.4, 50.0 );
			SetSkill( SkillName.MagicResist, 85.3, 100.0 );
			SetSkill( SkillName.Tactics, 97.6, 100.0 );
			SetSkill( SkillName.Wrestling, 80.5, 92.5 );

			Fame = 14000;
			Karma = -14000;

			VirtualArmor = 60;

			Tamable = true;
			ControlSlots = 2;
			MinTameSkill = 95.1;

			PackGold( 300, 600 );
			PackArmor( 0, 5 );
			PackWeapon( 0, 5 );
			PackItem( new SulfurousAsh( Utility.RandomMinMax( 3, 8 ) ) );

			if ( Utility.Random( 50 ) == 0 )
				PackItem( new RareBlueCarpet( PieceType.NorthEdge ));
		}

		public override int GetAngerSound()
		{
			if ( !Controlled )
				return 0x16A;

			return base.GetAngerSound();
		}

		public override bool HasBreath{ get{ return true; } } // fire breath enabled
		public override int Meat{ get{ return 5; } }
		public override int Hides{ get{ return 10; } }
		public override HideType HideType{ get{ return HideType.Barbed; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }

		public Nightmare( Serial serial ) : base( serial )
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

			if ( BaseSoundID == 0x16A )
				BaseSoundID = 0xA8;
		}
	}
}