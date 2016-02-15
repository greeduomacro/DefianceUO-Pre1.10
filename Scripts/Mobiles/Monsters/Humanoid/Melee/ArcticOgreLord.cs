using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "an ogre lords corpse" )]
	[TypeAlias( "Server.Mobiles.ArticOgreLord" )]
	public class ArcticOgreLord : BaseCreature
	{
		[Constructable]
		public ArcticOgreLord() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an arctic ogre lord";
			Body = 135;
			BaseSoundID = 427;

			SetStr( 858, 914 );
			SetDex( 67, 75 );
			SetInt( 52, 59 );

			SetDamage( 25, 30 );

			SetSkill( SkillName.MagicResist, 125.1, 135.4 );
			SetSkill( SkillName.Tactics, 93.1, 97.0 );
			SetSkill( SkillName.Wrestling, 93.1, 98.2 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 50;

			PackItem( new Club() );
			PackGem();
			PackGold( 500, 700 );
			PackArmor( 0, 5 );
			PackWeapon( 0, 5 );
                        PackJewel( 0.01 );
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override int TreasureMapLevel{ get{ return 3; } }

		public ArcticOgreLord( Serial serial ) : base( serial )
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