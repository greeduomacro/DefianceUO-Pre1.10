using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	public abstract class BaseGuard : Mobile
	{
		public static void Spawn( Mobile caller, Mobile target )
		{
			Spawn( caller, target, 1, false );
		}

		public static void Spawn( Mobile caller, Mobile target, int amount, bool onlyAdditional )
		{
			if ( target == null || target.Deleted )
				return;

			foreach ( Mobile m in target.GetMobilesInRange( 15 ) )
			{
				if ( m is BaseGuard )
				{
					BaseGuard g = (BaseGuard)m;

					if ( g.Focus == null ) // idling
					{
						g.Focus = target;

						--amount;
					}
					else if ( g.Focus == target && !onlyAdditional )
					{
						--amount;
					}
				}
			}

			while ( amount-- > 0 )
				caller.Region.MakeGuard( target );
		}

		public BaseGuard( Mobile target )
		{
			if ( target != null )
			{
				Location = target.Location;
				Map = target.Map;

				Effects.SendLocationParticles( EffectItem.Create( Location, Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 5023 );
			}
		}

		public BaseGuard( Serial serial ) : base( serial )
		{
		}

		public override bool OnBeforeDeath()
		{
			Effects.SendLocationParticles( EffectItem.Create( Location, Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );

			PlaySound( 0x1FE );

			Delete();

			return false;
		}

		public abstract Mobile Focus{ get; set; }

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

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			Head head = dropped as Head;
			if ( head != null && head.Owner != null && head.Owner is PlayerMobile )
			{
				PlayerMobile victim = head.Owner as PlayerMobile;
				int bounty = BountyTable.Bounty( victim );
				Say( 500670 ); // Ah, a head! Let me check to see if there is a bounty on this.
				if ( bounty <= 0 )
				{
					Say( 1042854, victim.Name );
				}
				else
				{
					if ( Banker.Deposit( from, bounty ) )
					{
						BountyTable.Remove( victim );
						Say( 1042855, String.Format( "{0}\t{1}", victim.Name, bounty ) );
					}
					else
					{
						Say( "There is bounty on this head, but thy bank box is too full to get it!" );
						return false;
					}
				}
				head.Delete();
			}
			return false;
		}
	}
}