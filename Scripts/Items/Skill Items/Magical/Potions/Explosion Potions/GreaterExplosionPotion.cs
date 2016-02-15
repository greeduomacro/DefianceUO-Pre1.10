using System;
using Server;

namespace Server.Items
{
	public class GreaterExplosionPotion : BaseExplosionPotion
	{
		//new changes 20-02-09
		public override int MinDamage { get { return Core.AOS ? 4 : 6; } }
		public override int MaxDamage { get { return Core.AOS ? 6: 9; } }
		public override double Delay{ get{ return 5.0; } }

		[Constructable]
		public GreaterExplosionPotion() : base( PotionEffect.ExplosionGreater )
		{

		}

		public GreaterExplosionPotion( Serial serial ) : base( serial )
		{
		}

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