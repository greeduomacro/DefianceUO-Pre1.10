using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a water elemental corpse" )]
	public class WaterElemental : BaseCreature
	{
		[Constructable]
		public WaterElemental () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a water elemental";
			Body = 16;
			BaseSoundID = 278;

			SetStr( 126, 155 );
			SetDex( 66, 85 );
			SetInt( 101, 125 );

			SetHits( 86, 103 );

			SetDamage( 7, 9 );

			SetSkill( SkillName.EvalInt, 60.1, 95.0 );
			SetSkill( SkillName.Magery, 60.1, 95.0 );
			SetSkill( SkillName.MagicResist, 100.1, 115.0 );
			SetSkill( SkillName.Tactics, 50.1, 70.0 );
			SetSkill( SkillName.Wrestling, 50.1, 70.0 );

			Fame = 4500;
			Karma = -4500;

			VirtualArmor = 40;
			ControlSlots = 3;
			CanSwim = true;

			PackPotion();
			PackGold( 200, 250 );
			PackItem( new BlackPearl( 5 ) );
		}

		public override void OnDeath( Container c )
	  	{
			if ( Utility.Random( 75 ) <  1 )
				c.DropItem( new BasicBlueCarpet( PieceType.NWCorner ) );

			base.OnDeath( c );
	  	}

		public override int TreasureMapLevel{ get{ return 2; } }

		public WaterElemental( Serial serial ) : base( serial )
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