using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a blood wisp corpse" )]
	public class BloodWisp : BaseCreature
	{
		[Constructable]
		public BloodWisp() : base( AIType.AI_Mage, FightMode.Agressor, 10, 1, 0.1, 0.2 )
		{
			Name = "a blood wisp";
			Body = 58;
			BaseSoundID = 466;
			Hue = 232;
			Kills = 10;
			ShortTermMurders = 10;

			SetStr( 325, 325 );
			SetDex( 225, 225 );
			SetInt( 2000, 2000 );

			SetHits( 1500, 1500 );

			SetDamage( 15, 15 );

			SetSkill( SkillName.EvalInt, 500.0 );
			SetSkill( SkillName.Magery, 100.0 );
			SetSkill( SkillName.MagicResist, 200.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Meditation, 50.0 );
			SetSkill( SkillName.Wrestling, 100.0 );

			Fame = 25000;
			Karma = -25000;

			VirtualArmor = 90;			

			PackGold( 500, 2000 );
			PackSlayer();
			AddItem( new LightSource() );

				if ( 0.02 > Utility.RandomDouble() )
					PackItem( new DarkIronWire() );

			switch ( Utility.Random( 3 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 4 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 6 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}

		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 250 ) <  1 )
				c.DropItem( new BloodWispStatue() );

            		base.OnDeath( c );
		}

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
				damage *= 3;
		}

		public BloodWisp( Serial serial ) : base( serial )
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