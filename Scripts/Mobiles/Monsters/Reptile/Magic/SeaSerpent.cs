using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a sea serpents corpse" )]
	[TypeAlias( "Server.Mobiles.Seaserpant" )]
	public class SeaSerpent : BaseCreature
	{
		[Constructable]
		public SeaSerpent() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a sea serpent";
			Body = 150;
			BaseSoundID = 447;

			SetStr( 168, 225 );
			SetDex( 58, 85 );
			SetInt( 53, 95 );

			SetHits( 110, 127 );

			SetDamage( 7, 13 );

			SetSkill( SkillName.MagicResist, 60.1, 75.0 );
			SetSkill( SkillName.Tactics, 60.1, 70.0 );
			SetSkill( SkillName.Wrestling, 60.1, 70.0 );
			SetSkill( SkillName.Magery, 30.1, 40.0 );
			SetSkill( SkillName.EvalInt, 30.1, 40.0 );

			Fame = 6000;
			Karma = -6000;

			VirtualArmor = 30;
			CanSwim = true;
			CantWalk = true;

			switch ( Utility.Random( 25 ))
         {
            case 0: PackItem( new SpecialFishingNet() ); break;
         }

		}

		public override int TreasureMapLevel{ get{ return 2; } }

		// TODO: Fish steaks??
		public override int Hides{ get{ return 10; } }
		public override HideType HideType{ get{ return HideType.Horned; } }
		public override int Scales{ get{ return 8; } }
		public override ScaleType ScaleType{ get{ return ScaleType.Blue; } }

		public SeaSerpent( Serial serial ) : base( serial )
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