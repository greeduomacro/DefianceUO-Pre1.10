using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a whipping vine corpse" )]
	public class WhippingVine : BaseCreature
	{
		[Constructable]
		public WhippingVine() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a whipping vine";
			Body = 8;
			Hue = 0x851;
			BaseSoundID = 352;

			SetStr( 268, 300 );
			SetDex( 81, 96 );
			SetInt( 26, 40 );

			SetDamage( 7, 25 );

			SetSkill( SkillName.MagicResist, 70.0 );
			SetSkill( SkillName.Tactics, 70.0 );
			SetSkill( SkillName.Wrestling, 73.0 );

			Fame = 1000;
			Karma = -1000;

			VirtualArmor = 30;

			PackReg( 1, 6 );
			PackItem( new FertileDirt( Utility.RandomMinMax( 1, 10 ) ) );

			if ( 0.2 >= Utility.RandomDouble() )
				PackItem( new ExecutionersCap() );

			PackItem( new Vines() );
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }

		public WhippingVine( Serial serial ) : base( serial )
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