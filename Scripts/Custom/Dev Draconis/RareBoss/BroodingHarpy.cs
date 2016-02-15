using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	public class BroodingHarpy : BaseRareBoss
	{
		[Constructable]
		public BroodingHarpy() : base( AIType.AI_Melee )
		{
			Name = "a brooding harpy";
			Body = 30;
			Hue = 1717;
			BaseSoundID = 402;

			SetHits( 300, 400 );
		}

		public override bool DoSpawnMobile { get { return true; } }

		public override int GetAttackSound()
		{
			return 916;
		}

		public override int GetAngerSound()
		{
			return 916;
		}

		public override int GetDeathSound()
		{
			return 917;
		}

		public override int GetHurtSound()
		{
			return 919;
		}

		public override int GetIdleSound()
		{
			return 918;
		}

		public override int Meat{ get{ return 8; } }
		public override MeatType MeatType{ get{ return MeatType.Bird; } }
		public override int Feathers{ get{ return 200; } }

		public BroodingHarpy( Serial serial ) : base( serial )
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