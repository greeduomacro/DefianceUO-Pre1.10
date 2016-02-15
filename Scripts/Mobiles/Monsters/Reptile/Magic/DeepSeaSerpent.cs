using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a deep sea serpents corpse" )]
	public class DeepSeaSerpent : BaseCreature
	{
		[Constructable]
		public DeepSeaSerpent() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a deep sea serpent";
			Body = 145;
			BaseSoundID = 447;

			SetStr( 251, 425 );
			SetDex( 87, 135 );
			SetInt( 87, 155 );

			SetHits( 151, 255 );

			SetDamage( 6, 14 );

			SetSkill( SkillName.MagicResist, 60.1, 75.0 );
			SetSkill( SkillName.Tactics, 60.1, 70.0 );
			SetSkill( SkillName.Wrestling, 60.1, 70.0 );
			SetSkill( SkillName.Magery, 40.1, 50.0 );
			SetSkill( SkillName.EvalInt, 40.1, 50.0 );

			Fame = 6000;
			Karma = -6000;

			VirtualArmor = 60;
			CanSwim = true;
			CantWalk = true;

			switch ( Utility.Random( 5 ))
         {
            case 0: PackItem( new SpecialFishingNet() ); break;
         }
			PackItem( new BlackPearl( 8 ) );
                        PackJewel( 0.03 );
                        PackJewel( 0.02 );
                        PackJewel( 0.01 );
     		}

		public override int Meat{ get{ return 10; } }
		public override int Scales{ get{ return 10; } }
		public override ScaleType ScaleType{ get{ return ScaleType.Blue; } }
		public override int Hides{ get{ return 15; } }
		public override HideType HideType{ get{ return HideType.Horned; } }

		public DeepSeaSerpent( Serial serial ) : base( serial )
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