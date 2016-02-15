using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a giant sea monster corpse" )]
	public class GiantSeaMonster : BaseCreature
	{
		[Constructable]
		public GiantSeaMonster () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a giant sea monster";
			Body = 77;
			BaseSoundID = 450;

			SetStr( 1300, 1400 );
			SetDex( 125, 195 );
			SetInt( 45006, 46206 );
			SetMana( 46000, 46000 );
			SetHits( 5500, 5500 );

			SetSkill( SkillName.EvalInt, 155.1, 130.0 );
			SetSkill( SkillName.Magery, 310.1, 350.0 );
			SetSkill( SkillName.Meditation, 155.1, 130.0 );
			SetSkill( SkillName.MagicResist, 160.5, 130.5 );
			SetSkill( SkillName.Tactics, 210.1, 180.0 );
			SetSkill( SkillName.Wrestling, 110.1, 110.0 );

			Fame = 50000;
			Karma = -50000;
			Hue = 1345;
			VirtualArmor = 40;
			ControlSlots = 6;
			CanSwim = true;
			CantWalk = true;
			PackGem();
			PackGem();
			PackGem();
			PackGem();
			PackGem();
			PackGem();
			PackGem();
			PackGem();
			PackPotion();
			PackPotion();
			PackPotion();
			PackGold( 1000, 6000 );
			PackScroll( 12, 12 );
			PackMagicItems( 3, 5, 0.95, 0.95 );
			PackMagicItems( 3, 5, 0.80, 0.65 );
			PackMagicItems( 4, 5, 0.80, 0.65 );
			PackSlayer();

			switch ( Utility.Random( 1000 ))
			{
			case 0: PackItem( new BloodstoneStatueMaker() ); break;
			case 1: PackItem( new GraniteStatueMaker() ); break;
			case 2: PackItem( new AlabasterStatueMaker() ); break;
			case 3: PackItem( new MarbleStatueMaker() ); break;

			}

		}

		public override int TreasureMapLevel{ get{ return 2; } }

		public GiantSeaMonster( Serial serial ) : base( serial )
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