using System;
using System.Collections;
using Server;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.EventPrizeSystem;

namespace Server.Mobiles
{
	[CorpseName( "a rotting corpse" )]
	public class WalkingDead : Zombie
	{
		[Constructable]
		public WalkingDead() : base()
		{
			Name = "a walking dead";
			Hue = 1196;

			SetHits( 140, 160 );

			SetSkill(SkillName.MagicResist, 1000.0);
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 200 ) < 1 )
			c.DropItem( new WalkingDeadStatue() );

			if ( Utility.Random( 100 ) < 1 )
			c.DropItem( new BronzePrizeToken() );


			base.OnDeath( c );
		}

		public override bool BardImmune{ get{ return true;} }

		public override void Damage(int amount, Mobile from)
		{
            if (from is PlayerMobile)
            {
                BaseWeapon bw = from.FindItemOnLayer(Layer.OneHanded) as BaseWeapon;
                BaseWeapon bw2 = from.FindItemOnLayer(Layer.TwoHanded) as BaseWeapon;
                BaseWeapon bw3 = from.FindItemOnLayer(Layer.FirstValid) as BaseWeapon;

                if (bw == null && bw2 == null && bw3 == null)
                    amount = 0;

            }
            else
                amount = 0;
			base.Damage(amount, from);
		}


		public WalkingDead( Serial serial ) : base( serial )
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