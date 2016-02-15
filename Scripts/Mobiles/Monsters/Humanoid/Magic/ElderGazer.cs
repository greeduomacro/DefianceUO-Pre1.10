using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an elder gazer corpse" )]
	public class ElderGazer : BaseCreature
	{
		[Constructable]
		public ElderGazer () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an elder gazer";
			Body = 22;
			BaseSoundID = 377;

			SetStr( 296, 325 );
			SetDex( 86, 101 );
			SetInt( 310, 364 );

			SetHits( 178, 195 );

			SetDamage( 8, 19 );

			SetSkill( SkillName.EvalInt, 25.8, 33.7 );
			SetSkill( SkillName.Magery, 92.1, 99.2 );
			SetSkill( SkillName.MagicResist, 118.1, 128.0 );
			SetSkill( SkillName.Tactics, 82.1, 97.0 );
			SetSkill( SkillName.Wrestling, 85.1, 97.7 );

			Fame = 12500;
			Karma = -12500;

			VirtualArmor = 50;

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 3 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			PackGold( 700, 900 );
			PackGem();
		}

		public ElderGazer( Serial serial ) : base( serial )
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