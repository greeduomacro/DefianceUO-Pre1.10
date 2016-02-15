using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Xanthos.Evo
{
	[CorpseName( "a dragon corpse" )]
	public class RaelisDragon : BaseEvo, IEvoCreature
	{
		public override BaseEvoSpec GetEvoSpec()
		{
			return RaelisDragonSpec.Instance;
		}

		public override BaseEvoEgg GetEvoEgg()
		{
			return new RaelisDragonEgg();
		}

		public override bool AddPointsOnDamage { get { return true; } }
		public override bool AddPointsOnMelee { get { return false; } }
		public override Type GetEvoDustType() { return typeof( RaelisDragonDust ); }

		public override bool HasBreath{ get{ return true; } }
		public override int BreathRange{ get{ return RangePerception / 2; } }

		public RaelisDragon( string name ) : base( name )
		{
		}

                public override int BreathComputeDamage()
                {
                return 50;
                }

		public RaelisDragon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write( (int)0 );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}