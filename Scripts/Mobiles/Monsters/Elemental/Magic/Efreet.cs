using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an efreet corpse" )]
	public class Efreet : BaseCreature
	{
		[Constructable]
		public Efreet () : base( AIType.AI_Mage, FightMode.Weakest, 10, 1, 0.1, 0.2 )
		{
			Name = "an efreet";
			Body = 131;
			BaseSoundID = 768;

			SetStr( 326, 354 );
			SetDex( 271, 278 );
			SetInt( 177, 195 );

			SetDamage( 12, 15 );

			SetSkill( SkillName.Magery, 67.1, 77.0 );
			SetSkill( SkillName.MagicResist, 61.1, 74.0 );
			SetSkill( SkillName.Tactics, 61.1, 79.0 );
			SetSkill( SkillName.Wrestling, 64.1, 79.0 );

			Fame = 10000;
			Karma = -10000;

			VirtualArmor = 56;

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackGem(); break;
				case 1: PackGem(); break;
				case 2: PackPotion(); break;
				case 3: PackItem( new Arrow( Utility.Random( 10, 15 ) ) ); break;
			}

			PackGold( 350, 500 );

			switch ( Utility.Random( 2 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 3 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

                        switch ( Utility.Random( 500 ))
    			     {
       				    case 0: PackItem( new DaemonHelm() ); break;
       				    case 1: PackItem( new DaemonChest() ); break;
        			    case 2: PackItem( new DaemonLegs() ); break;
         			    case 3: PackItem( new DaemonArms() ); break;
         			    case 4: PackItem( new DaemonGloves() ); break;
      			     }
		}

		public override int TreasureMapLevel{ get{ return 3; } }

		public Efreet( Serial serial ) : base( serial )
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