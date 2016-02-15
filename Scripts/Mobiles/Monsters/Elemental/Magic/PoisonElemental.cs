using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a poison elemental corpse" )]
	public class PoisonElemental : BaseCreature
	{
		[Constructable]
		public PoisonElemental () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2 )
		{
			Name = "a poison elemental";
			Body = 162;
			BaseSoundID = 263;

			SetStr( 486, 515 );
			SetDex( 166, 185 );
			SetInt( 391, 435 );

			SetHits( 376, 399 );

			SetDamage( 14, 20 );

			SetSkill( SkillName.EvalInt, 80.1, 105.0 );
			SetSkill( SkillName.Magery, 80.1, 95.0 );
			SetSkill( SkillName.Meditation, 80.2, 120.0 );
			SetSkill( SkillName.Poisoning, 100.1, 110.0 );
			SetSkill( SkillName.MagicResist, 95.2, 115.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.Wrestling, 90.1, 100.0 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 70;

			PackGold( 600, 800 );
			PackGem();
			PackGem();
			PackScroll( 1, 7 );
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

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 15 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}

                           switch ( Utility.Random( 300 ))
        		 {
           			case 0: PackItem( new LampPost2() ); break;
        		 }
}

		public override void OnDeath( Container c )
	  	{
			if ( Utility.Random( 75 ) <  1 )
				c.DropItem( new BasicBlueCarpet( PieceType.Centre ) );

			base.OnDeath( c );
	  	}


		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }

		public override Poison HitPoison{ get{ return Poison.Lethal; } }
		public override double HitPoisonChance{ get{ return 0.75; } }

		public override int TreasureMapLevel{ get{ return 5; } }

		public PoisonElemental( Serial serial ) : base( serial )
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