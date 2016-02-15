using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a fire elemental corpse" )]
	public class FireElemental : BaseCreature
	{
		[Constructable]
		public FireElemental () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a fire elemental";
			Body = 15;
			BaseSoundID = 274;

			SetStr( 129, 151 );
			SetDex( 166, 185 );
			SetInt( 101, 119 );

			SetDamage( 7, 9 );

			SetSkill( SkillName.Magery, 67.1, 77.0 );
			SetSkill( SkillName.MagicResist, 79.2, 94.8 );
			SetSkill( SkillName.Tactics, 80.1, 95.0 );
			SetSkill( SkillName.Wrestling, 75.1, 99.0 );

			Fame = 4500;
			Karma = -4500;

			VirtualArmor = 40;
			ControlSlots = 4;

			AddItem( new LightSource() );

			PackGem();
			PackItem( new SulfurousAsh( 3 ) );
			PackGold( 250, 350 );
		}
		
		public override void OnDeath( Container c )
	  	{
			if ( Utility.Random( 75 ) <  1 )
				c.DropItem( new BasicBlueCarpet( PieceType.EastEdge ) );

			base.OnDeath( c );
	  	}

		public override int TreasureMapLevel{ get{ return 2; } }
		public override bool AlwaysMurderer{ get{ return true; } }

		public FireElemental( Serial serial ) : base( serial )
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