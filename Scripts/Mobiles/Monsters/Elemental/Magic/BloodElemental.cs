using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a blood elemental corpse" )]
	public class BloodElemental : BaseCreature
	{
		[Constructable]
		public BloodElemental () : base( AIType.AI_Mage, FightMode.Weakest, 10, 1, 0.2, 0.4 )
		{
			Name = "a blood elemental";
			Body = Utility.RandomList( 159, 160 );
			BaseSoundID = 278;

			SetStr( 529, 615 );
			SetDex( 68, 85 );
			SetInt( 226, 344 );

			SetDamage( 17, 27 );

			SetSkill( SkillName.EvalInt, 58.0, 80.6 );
			SetSkill( SkillName.Magery, 87.0, 99.4 );
			SetSkill( SkillName.Meditation, 22.1, 61.7 );
			SetSkill( SkillName.MagicResist, 80.9, 94.7 );
			SetSkill( SkillName.Tactics, 86.5, 99.4 );
			SetSkill( SkillName.Wrestling, 82.2, 99.4 );

			Fame = 12500;
			Karma = -12500;

			VirtualArmor = 60;

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackGem(); break;
				case 1: PackScroll( 4, 7 ); break;
				case 2: PackGem(); break;
				case 3: PackGem(); break;
			}

			PackGold( 600, 900 );
			PackArmor( 0, 5 );
			PackWeapon( 0, 4 );
			PackArmor( 0, 5 );
			PackWeapon( 0, 4 );
			PackSlayer();

			switch ( Utility.Random( 500 ))
			{
				case 0: PackItem( new RuinedDrawers() ); break;
			}
			if (2 > Utility.Random(100)) PackItem(new BloodPentagramPart(Utility.RandomMinMax(14,18)));
		}

		public override void OnDeath( Container c )
	  	{
			if ( Utility.Random( 75 ) <  1 )
				c.DropItem( new BasicBlueCarpet( PieceType.SWCorner ) );

			base.OnDeath( c );
	  	}

		public override int TreasureMapLevel{ get{ return 5; } }
		public override bool AlwaysMurderer{ get{ return true; } }

		public BloodElemental( Serial serial ) : base( serial )
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