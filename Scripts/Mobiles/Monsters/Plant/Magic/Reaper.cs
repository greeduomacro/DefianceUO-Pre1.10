using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a reapers corpse" )]
	public class Reaper : BaseCreature
	{
		[Constructable]
		public Reaper() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a reaper";
			Body = 47;
			BaseSoundID = 442;

			SetStr( 66, 201 );
			SetDex( 66, 75 );
			SetInt( 101, 235 );

			SetDamage( 9, 11 );

			SetSkill( SkillName.EvalInt, 1.5, 24.4 );
			SetSkill( SkillName.Magery, 93.1, 99.9 );
			SetSkill( SkillName.MagicResist, 105.1, 121.0 );
			SetSkill( SkillName.Tactics, 45.1, 57.0 );
			SetSkill( SkillName.Wrestling, 56.1, 63.0 );

			Fame = 3500;
			Karma = -3500;

			VirtualArmor = 40;

			if ( 0.25 > Utility.RandomDouble() )
				PackItem( new Board( 10 ) );
			else
				PackItem( new Log( 10 ) );

			PackGold( 0, 150 );
			PackItem( new MandrakeRoot( 5 ) );
		}

		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		public override int TreasureMapLevel{ get{ return 2; } }
		public override bool DisallowAllMoves{ get{ return true; } }

		public Reaper( Serial serial ) : base( serial )
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