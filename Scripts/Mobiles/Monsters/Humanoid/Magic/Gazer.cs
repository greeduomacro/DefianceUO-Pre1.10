using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a gazer corpse" )]
	public class Gazer : BaseCreature
	{
		[Constructable]
		public Gazer () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a gazer";
			Body = 22;
			BaseSoundID = 377;

			SetStr( 96, 125 );
			SetDex( 86, 105 );
			SetInt( 141, 165 );

			SetHits( 58, 75 );

			SetDamage( 5, 10 );

			SetSkill( SkillName.EvalInt, 50.1, 65.0 );
			SetSkill( SkillName.Magery, 50.1, 65.0 );
			SetSkill( SkillName.MagicResist, 60.1, 75.0 );
			SetSkill( SkillName.Tactics, 50.1, 70.0 );
			SetSkill( SkillName.Wrestling, 50.1, 70.0 );

			Fame = 3500;
			Karma = -3500;

			VirtualArmor = 36;

			PackItem( new Nightshade( 5 ) );
			PackGold( 50, 150 );
			PackPotion();
			PackScroll( 0, 5 );
			PackItem( new Arrow( Utility.Random( 1, 10 ) ) );
			PackArmor( 0, 3 );
			PackWeapon( 0, 3 );
		}

		public override int TreasureMapLevel{ get{ return 1; } }
		public override int Meat{ get{ return 1; } }

		public Gazer( Serial serial ) : base( serial )
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