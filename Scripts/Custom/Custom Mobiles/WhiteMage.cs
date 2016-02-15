using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an evil WhiteMage corpse" )]
	public class WhiteMage : BaseCreature
	{
		[Constructable]
		public WhiteMage() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a White mage";
			Title = "";
			Body = 124;
                        Hue = 1150;



                        SetStr( 81, 105 );
			SetDex( 91, 115 );
			SetInt( 96, 120 );

			SetHits( 49, 63 );

			SetDamage( 5, 10 );

			SetSkill( SkillName.EvalInt, 75.1, 100.0 );
			SetSkill( SkillName.Magery, 75.1, 100.0 );
			SetSkill( SkillName.MagicResist, 75.0, 97.5 );
			SetSkill( SkillName.Tactics, 65.0, 87.5 );
			SetSkill( SkillName.Wrestling, 20.2, 60.0 );

			Fame = 2500;
			Karma = -2500;

			VirtualArmor = 16;

			PackReg( 6 );
			PackGold( 125, 175 );
			PackScroll( 2, 7 );
                        PackItem( new Robe( Utility.RandomNeutralHue() ) ); // TODO: Proper hue
			PackItem( new Sandals() );

                                 switch ( Utility.Random( 30 ))
        		 {
           			case 0: PackItem( new IceStaff() ); break;
        		 }
}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override int Meat{ get{ return 1; } }

		public WhiteMage( Serial serial ) : base( serial )
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