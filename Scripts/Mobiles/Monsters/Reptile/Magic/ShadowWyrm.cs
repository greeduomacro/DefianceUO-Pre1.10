using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a shadow wyrm corpse" )]
	public class ShadowWyrm : BaseCreature
	{
		[Constructable]
		public ShadowWyrm() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a shadow wyrm";
			Body = 106;
			BaseSoundID = 362;

			SetStr( 898, 1030 );
			SetDex( 68, 200 );
			SetInt( 488, 620 );

			SetHits( 558, 599 );

			SetDamage( 29, 35 );

			SetSkill( SkillName.EvalInt, 95.1, 130.0 );
			SetSkill( SkillName.Magery, 95.1, 130.0 );
			SetSkill( SkillName.Meditation, 52.5, 75.0 );
			SetSkill( SkillName.MagicResist, 100.3, 130.0 );
			SetSkill( SkillName.Tactics, 97.6, 100.0 );
			SetSkill( SkillName.Wrestling, 97.6, 100.0 );

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 70;

			PackGem( 5, 10 );
			PackGold( 1000, 1400 );
			PackSlayer();

			switch ( Utility.Random( 2 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 2 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 2 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}

			switch ( Utility.Random( 10 ) )
			{
				case 0: PackWeapon( 3, 5 ); break;
				case 1: PackArmor( 3, 5 ); break;
			}

                           switch ( Utility.Random( 500 ))
        		 {
           			case 0: PackItem( new CoveredChair() ); break;
        		 }
			
			if ( Utility.Random( 100 ) == 0 )
				PackItem( new RareCreamCarpet(PieceType.SWCorner) );
}

		public override bool AutoDispel{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override Poison HitPoison{ get{ return Poison.Deadly; } }
		public override int TreasureMapLevel{ get{ return 5; } }
		public override int Meat{ get{ return 19; } }
		public override int Hides{ get{ return 20; } }
		public override int Scales{ get{ return 10; } }
		public override ScaleType ScaleType{ get{ return ScaleType.Black; } }
		public override HideType HideType{ get{ return HideType.Barbed; } }

		public ShadowWyrm( Serial serial ) : base( serial )
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