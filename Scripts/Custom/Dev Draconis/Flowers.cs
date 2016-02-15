using System;
using System.Collections;
using Server;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Mobiles;
using Server.Items;
namespace Server.Items
{
 	public class Flowers : Item
 	{
		[Constructable]
  		public Flowers() : base()
  		{
			ItemID = Utility.RandomList( 3203, 3204, 3205, 3206, 3207, 3208, 3209, 3210, 3211, 3212, 3213, 3214, 3262, 3263, 3264, 3265 );
			Movable = false;

			Timer.DelayCall( TimeSpan.FromSeconds( 30 ), new TimerCallback( Explode ) );
  		}

		private void Explode()
		{
			ArrayList toDamage = new ArrayList();

			foreach( Mobile m in GetMobilesInRange( 1 ) )
			{
                if (m.CanBeDamaged() && m.Alive)
                {
                    if (m.Player)
                        toDamage.Add(m);
                    else if ( m is BaseCreature && ( ((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned ) )
                        toDamage.Add(m);
                }
			}

			foreach ( Mobile m in toDamage )
			{
				m.Poison = Poison.Lethal;
				Spells.SpellHelper.Damage(TimeSpan.FromTicks(0), m, 15);
			}

			Effects.SendLocationParticles( EffectItem.Create( this.Location, this.Map, EffectItem.DefaultDuration ), 0x36B0, 1, 14, 63, 7, 9915, 0 );
			Effects.PlaySound( this.Location, this.Map, 0x229 );
			this.Delete();
		}

  		public Flowers(Serial serial) : base(serial)
  		{
  		}

  		public override void Serialize(GenericWriter writer)
  		{
  			base.Serialize(writer);
 			writer.Write((int)0);
  		}

  		public override void Deserialize(GenericReader reader)
  		{
  			base.Deserialize(reader);
  			int version = reader.ReadInt();
  		}
  	}
}