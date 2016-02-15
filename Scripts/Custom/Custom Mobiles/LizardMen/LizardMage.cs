using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a glowing lizardman corpse" )]
	public class LizardMage : BaseCreature
	{
		[Constructable]
		public LizardMage() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "lizardman" );
			Body = Utility.RandomList( 35, 36 );
			BaseSoundID = 417;
			Hue = 353;

			SetStr( 146, 180 );
			SetDex( 101, 130 );
			SetInt( 186, 210 );

			SetHits( 88, 108 );

			SetDamage( 7, 14 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 40, 45 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 10, 20 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.EvalInt, 70.1, 80.0 );
			SetSkill( SkillName.Magery, 70.1, 80.0 );
			SetSkill( SkillName.MagicResist, 65.1, 90.0 );
			SetSkill( SkillName.Tactics, 50.1, 75.0 );
			SetSkill( SkillName.Wrestling, 50.1, 75.0 );

			Fame = 7500;
			Karma = -7500;

			VirtualArmor = 44;

			PackGold( 75, 125 );
			PackReg( 6 );
			PackScroll( 1, 7 );

			PackNecroScroll( 9 ); // Poison Strike
			PackNecroScroll( 8 ); // Pain Spike
			PackNecroScroll( 3 ); // Curse Weapon
			PackNecroScroll( 1 ); // Blood Oath
			PackNecroScroll( 4 ); // Evil Omen
			PackNecroScroll( 7 ); // Mind Rot
			PackNecroScroll( 2 ); // Corpse Skin
			PackNecroScroll( 15 ); // Wraith Form
			PackNecroScroll( 5 ); // Horrific Beast
			PackNecroScroll( 0 ); // Animate Dead
		}

		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 8; } }
		public override HideType HideType{ get{ return HideType.Spined; } }

		public LizardMage( Serial serial ) : base( serial )
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

			if ( Body == 42 )
			{
				Body = 0x8F;
				Hue = 0;
			}
		}
	}
}