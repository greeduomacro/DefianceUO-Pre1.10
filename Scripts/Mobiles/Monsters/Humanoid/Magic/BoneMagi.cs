using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a bone magi corpse" )]
	public class BoneMagi : BaseCreature
	{
		[Constructable]
		public BoneMagi() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a bone magi";
			Body = 148;
			BaseSoundID = 451;

			SetStr( 76, 98 );
			SetDex( 56, 75 );
			SetInt( 186, 209 );

			SetDamage( 3, 7 );

			SetSkill( SkillName.Magery, 66.1, 73.0 );
			SetSkill( SkillName.Tactics, 45.1, 60.0 );
			SetSkill( SkillName.Wrestling, 51.1, 57.0 );

			Fame = 3000;
			Karma = -3000;

			VirtualArmor = 38;

			PackGem();
			PackReg( 3 );
			PackItem( new Bone() );
			PackGold( 50, 200 );
			PackScroll( 0, 5 );
			PackArmor( 0, 3 );
			PackWeapon( 0, 3 );
                        //PackJewel( 0, 01 );
		}

		public override Poison PoisonImmune{ get{ return Poison.Regular; } }

		public BoneMagi( Serial serial ) : base( serial )
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