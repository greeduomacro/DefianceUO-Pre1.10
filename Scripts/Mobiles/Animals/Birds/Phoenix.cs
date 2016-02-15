using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a phoenix corpse" )]
	public class Phoenix : BaseCreature
	{
		[Constructable]
		public Phoenix() : base( AIType.AI_Mage, FightMode.Agressor, 10, 1, 0.1, 0.3 )
		{
			Name = "a phoenix";
			Body = 5;
			Hue = 0x674;
			BaseSoundID = 0x8F;

			SetStr( 804, 900 );
			SetDex( 402, 500 );
			SetInt( 504, 700 );

			SetHits( 804, 900 );

			SetDamage( 30 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Fire, 50 );

			SetSkill( SkillName.EvalInt, 90.2, 100.0 );
			SetSkill( SkillName.Magery, 90.2, 100.0 );
			SetSkill( SkillName.Meditation, 75.1, 100.0 );
			SetSkill( SkillName.MagicResist, 145.0, 155.0 );
			SetSkill( SkillName.Tactics, 100.1, 110.0 );
			SetSkill( SkillName.Wrestling, 100.1, 110.0 );

			Fame = 15000;
			Karma = 0;

			VirtualArmor = 80;

                        PackGold( 600, 800 );
			PackArmor( 0, 5 );
			PackWeapon( 0, 5 );

		}

		public override int Meat{ get{ return 1; } }
		public override MeatType MeatType{ get{ return MeatType.Bird; } }
		public override int Feathers{ get{ return 36; } }

		public Phoenix( Serial serial ) : base( serial )
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