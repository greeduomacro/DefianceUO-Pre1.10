using System;
using System.Collections;
using Server;
using Server.Items;
//using Server.Engines.SilenceAddon;

namespace Server.Mobiles
{
	[CorpseName( "a corpse" )]
	public abstract class BaseFakeMob : BaseCreature
	{
		public BaseFakeMob( AIType aiType ) : this( aiType, FightMode.Closest )
		{
		}

		public BaseFakeMob( AIType aiType, FightMode mode ) : base( aiType, mode, 18, 1, 0.1, 0.2 )
		{
		}

		public BaseFakeMob( Serial serial ) : base( serial )
		{
		}

		public override bool BardImmune{ get{ return true;} }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool CanDestroyObstacles { get { return true; } }
		public override bool AutoDispel { get { return true; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}