using System;
using Server;
using Server.Items;
using Server.Engines.Quests.Collector;

namespace Server.Mobiles
{
	[CorpseName( "a shrieking willow corpse" )]
	public class ShriekingWillow : BaseCreature
	{
		[Constructable]
		public ShriekingWillow() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.4, 0.4 )
		{
			Name = "a shrieking willow";
			Body = 47;
			BaseSoundID = 442;
			Hue = 2118;

			SetStr( 106, 215 );
			SetDex( 86, 95 );
			SetInt( 201, 250 );

			SetHits( 745, 885 );
			SetMana( 345, 485 );

			SetDamage( 10, 15 );

			SetSkill( SkillName.EvalInt, 27.1, 30.0 );
			SetSkill( SkillName.Magery, 80.1, 84.0 );
			SetSkill( SkillName.MagicResist, 200.1, 205.0 );
			SetSkill( SkillName.Tactics, 75.1, 80.0 );
			SetSkill( SkillName.Wrestling, 75.1, 80.0 );

			Fame = 15500;
			Karma = -15500;

			VirtualArmor = 30;

			switch( Utility.Random(125) )
			{
				case 0: PackItem( new EnchantedWood() ); break;
			}

			PackGold( 350, 550 );

			switch ( Utility.Random( 3 ) )
			{
				case 0: PackItem( new Log( Utility.RandomMinMax( 5, 25 ) ) ); break;
				case 1: PackItem( new FertileDirt( Utility.RandomMinMax( 5, 25 ) ) ); break;
			}

			switch ( Utility.Random( 2 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 2 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 4 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}

			switch ( Utility.Random( 4 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}

				if ( 0.10 > Utility.RandomDouble() )
					PackItem( new Obsidian() );

		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 250 ) <  1 )
				c.DropItem( new FallenLog() );

            		base.OnDeath( c );
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override int TreasureMapLevel{ get{ return 4; } }

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
				damage *= 10;
		}

		public ShriekingWillow( Serial serial ) : base( serial )
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