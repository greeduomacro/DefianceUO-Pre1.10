using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an air elemental corpse" )]
	public class AirElemental : BaseCreature
	{
		[Constructable]
		public AirElemental () : base( AIType.AI_Mage, FightMode.Weakest, 10, 1, 0.1, 0.2 )
		{
			Name = "an air elemental";
			Body = 13;
			Hue = 0x4001;
			BaseSoundID = 263;

			SetStr( 126, 152 );
			SetDex( 166, 185 );
			SetInt( 102, 121 );

			SetDamage( 8, 10 );

			SetSkill( SkillName.Magery, 67.1, 78.4 );
			SetSkill( SkillName.MagicResist, 63.1, 74.8 );
			SetSkill( SkillName.Tactics, 62.1, 78.0 );
			SetSkill( SkillName.Wrestling, 66.5, 81.5 );

			Fame = 4000;
			Karma = -4000;

			VirtualArmor = 40;
			ControlSlots = 2;

			switch ( Utility.Random( 15 ) )
			{
				case 0: PackItem( new AgilityPotion() ); break;
				case 1: PackItem( new ExplosionPotion() ); break;
				case 2: PackItem( new CurePotion() ); break;
				case 3: PackItem( new HealPotion() ); break;
				case 4: PackItem( new NightSightPotion() ); break;
				case 5: PackItem( new PoisonPotion() ); break;
				case 6: PackItem( new RefreshPotion() ); break;
				case 7: PackItem( new StrengthPotion() ); break;
			}

			PackGold( 200, 300 );

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackScroll( 1, 6 ); break;
			}
		}

		public override void OnDeath( Container c )
	  	{
			if ( Utility.Random( 75 ) <  1 )
				c.DropItem( new BasicBlueCarpet( PieceType.NECorner ) );

			base.OnDeath( c );
	  	}

		public override int TreasureMapLevel{ get{ return 2; } }
		public override bool AlwaysMurderer{ get{ return true; } }

		public AirElemental( Serial serial ) : base( serial )
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