using System;
using Server;
using Server.Items;
using Server.Engines.Quests.Collector;

namespace Server.Mobiles
{
	[CorpseName( "an fairy dragon corpse" )]
	public class FairyDragon : BaseCreature
	{
		[Constructable]
		public FairyDragon () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.08, 0.2 )
		{
			Name = "a fairy dragon";
			Body = 176;
			Hue = 22222;
			BaseSoundID = 362;
			Kills = 10;
			ShortTermMurders = 10;

			SetStr( 896, 985 );
			SetDex( 146, 165 );
			SetInt( 786, 875 );

			SetHits( 1000, 1100 );

			SetDamage( 30, 37 );

			SetSkill( SkillName.EvalInt, 95.1, 98.0 );
			SetSkill( SkillName.Magery, 98.1, 100.0 );
			SetSkill( SkillName.Meditation, 92.5, 95.0 );
			SetSkill( SkillName.MagicResist, 145.5, 150.0 );
			SetSkill( SkillName.Tactics, 97.6, 100.0 );
			SetSkill( SkillName.Wrestling, 97.6, 100.0 );

			Fame = 25000;
			Karma = -25000;

			VirtualArmor = 140;

			switch( Utility.Random(100) )
			{
				case 0: PackItem( new EnchantedWood() ); break;
			}

			PackGold( 800, 1500 );
			PackSlayer();
			PackArmor( 0, 5 );
			PackArmor( 0, 5 );

			switch ( Utility.Random( 4 ) )
			{
				case 0: PackItem( new Obsidian() ); break;
				case 1: PackItem( new Obsidian() ); break;
			}

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

				if ( Utility.Random( 75 ) == 0 )
				PackItem( new RareCreamCarpet( PieceType.EastEdge ));

       		}
		
		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 250 ) <  1 )
				c.DropItem( new GiantMushroom() );

            		base.OnDeath( c );
		}


                public override bool AutoDispel{ get{ return true; } }
		public override int Meat{ get{ return 5; } }
		public override int Scales{ get{ return 15; } }
		public override ScaleType ScaleType{ get{ return (ScaleType)Utility.Random( 4 ); } }
		public override Poison PoisonImmune{ get{ return Poison.Regular; } }
		public override int TreasureMapLevel{ get{ return 5; } }

		public override void CheckReflect( Mobile caster, ref bool reflect )
		{
			reflect = true; // Every spell is reflected back to the caster
		}

		public FairyDragon( Serial serial ) : base( serial )
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