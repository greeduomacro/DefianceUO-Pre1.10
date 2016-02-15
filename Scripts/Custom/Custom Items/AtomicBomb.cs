using System;
using Server;
/*Add Fire Column (flame strike) Animation Effects
Make The Fire Columns (flame strikes) Spread Out To Animate On A 10 Spare Tile Radius
Make A Line To Edit Explosion Damage Range
Make A Ball Of Fire Animation As The Base Explosion Animation (replace explosion with a ball of fire)
Make A Line That Gives A Shard Message When Bomb Is Detonated */


namespace Server.Items
{
   public class AtomicBomb : BaseAtomicBomb
   {
      public override int MinDamage { get { return 1050; } }
      public override int MaxDamage { get { return 1270; } }

      [Constructable]
      public AtomicBomb() : base( PotionEffect.ExplosionGreater )
      {

         Hue = 1161;
         Name = "an atomic bomb";

      }

      public AtomicBomb( Serial serial ) : base( serial )
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