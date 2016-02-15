using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a cronicorn corpse" )]
	public class Cronicorn : BaseCreature
	{
		[Constructable]
		public Cronicorn() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.2 )
		{
			Name = "a cronicorn";
			BaseSoundID = 0x4BC;
			Hue = 1175;
			Body = 122;
			Kills = 5;

			SetStr( 796, 825 );
			SetDex( 196, 215 );
			SetInt( 86, 95 );

			SetDamage( 28, 32 );

			SetSkill( SkillName.MagicResist, 135.3, 140.0 );
			SetSkill( SkillName.Tactics, 96.1, 98.5 );
			SetSkill( SkillName.Wrestling, 98.5, 99.5 );

			Fame = 15000;
			Karma = 15000;

			VirtualArmor = 60;

			PackGold( 500, 1100 );

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackReg( 11 ); break;
				case 1: PackScroll( 4, 8 ); break;
				case 2: PackScroll( 4, 8 ); break;
				case 3: PackReg( 12 ); break;
				case 4: PackReg( 10 ); break;
			}

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}

			if ( Utility.Random( 75 ) == 0 )
				PackItem( new RareBlueCarpet( PieceType.NWCorner ));
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override int Meat{ get{ return 3; } }
		public override int Hides{ get{ return 20; } }
		public override HideType HideType{ get{ return HideType.Horned; } }

		public Cronicorn( Serial serial ) : base( serial )
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