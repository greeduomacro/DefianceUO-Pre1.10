using System;
using Server.Items;
using Server.Targeting;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "a terathan drone corpse" )]
	public class TerathanDrone : BaseCreature
	{
		[Constructable]
		public TerathanDrone() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a terathan drone";
			Body = 71;
			BaseSoundID = 589;

			SetStr( 36, 65 );
			SetDex( 96, 145 );
			SetInt( 21, 45 );

			SetHits( 22, 39 );
			SetMana( 0 );

			SetDamage( 6, 12 );

			SetSkill( SkillName.Poisoning, 40.1, 60.0 );
			SetSkill( SkillName.MagicResist, 30.1, 45.0 );
			SetSkill( SkillName.Tactics, 30.1, 50.0 );
			SetSkill( SkillName.Wrestling, 40.1, 50.0 );

			Fame = 2000;
			Karma = -2000;

			VirtualArmor = 24;

			PackItem( new SpidersSilk( 2 ) );
			PackGold( 50, 100 );

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 10 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

		        if ( 0.01 > Utility.RandomDouble() )
		        PackItem( new OphidianStaff() );
                }

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.TerathansAndOphidians; }
		}

		public TerathanDrone( Serial serial ) : base( serial )
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