using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	public class Betrayer : BaseCreature
	{
		[Constructable]
		public Betrayer() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "male" );
			Title = "the betrayer";
			Body = 767;


			SetStr( 401, 500 );
			SetDex( 81, 100 );
			SetInt( 151, 200 );

			SetHits( 241, 300 );

			SetDamage( 16, 22 );

			SetSkill( SkillName.Anatomy, 90.1, 100.0 );
			SetSkill( SkillName.EvalInt, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 50.1, 100.0 );
			SetSkill( SkillName.Meditation, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 120.1, 130.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.Wrestling, 90.1, 100.0 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 65;
			SpeechHue = Utility.RandomDyedHue();

			PackGem();

			switch ( Utility.Random( 4 ) )
			{
				case 0: PackItem( new Katana() ); break;
				case 1: PackItem( new BodySash() ); break;
				case 2: PackItem( new Halberd() ); break;
				case 3: PackItem( new LapHarp() ); break;
			}

			PackGold( 180, 240 );
			PackWeapon( 0, 4 );
			PackWeapon( 0, 3 );
			PackArmor( 0, 4 );
			PackArmor( 0, 3 );
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool BardImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }

		public Betrayer( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}