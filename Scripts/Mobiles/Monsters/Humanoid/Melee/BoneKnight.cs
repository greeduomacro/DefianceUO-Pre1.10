using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a bone knight corpse" )]
	public class BoneKnight : BaseCreature
	{
		[Constructable]
		public BoneKnight() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a bone knight";
			Body = 57;
			BaseSoundID = 451;

			SetStr( 211, 246 );
			SetDex( 74, 95 );
			SetInt( 36, 54 );

			SetDamage( 8, 18 );

			SetSkill( SkillName.MagicResist, 66.0, 79.6 );
			SetSkill( SkillName.Tactics, 88.6, 99.0 );
			SetSkill( SkillName.Wrestling, 87.5, 95.0 );

			Fame = 3000;
			Karma = -3000;

			VirtualArmor = 40;

			switch ( Utility.Random( 10 ) )
			{
				case 0: PackItem( new PlateArms() ); break;
				case 1: PackItem( new PlateChest() ); break;
				case 2: PackItem( new PlateGloves() ); break;
				case 3: PackItem( new PlateGorget() ); break;
				case 4: PackItem( new PlateLegs() ); break;
				case 5: PackItem( new PlateHelm() ); break;
			}

			PackItem( new Scimitar() );
			PackGold( 50, 150 );
			PackItem( new WoodenShield() );
		}

		public BoneKnight( Serial serial ) : base( serial )
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