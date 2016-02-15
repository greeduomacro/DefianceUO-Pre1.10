using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a rotting corpse" )]
	public class InfestZombie : Zombie
	{
		private Mobile m_Owner;

		[Constructable]
		public InfestZombie() : base()
		{
			Name = "an infestation zombie";
			SetHits( 70, 80 );
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool BardImmune{ get{ return true; } }

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			if ( defender is PlayerMobile && ( defender.BodyValue == 400 || defender.BodyValue == 401 ) && CanBeHarmful(defender, false) )
			{
				Item robe = defender.FindItemOnLayer( Layer.OuterTorso );

				if ( robe != null && robe.Movable )
					defender.AddToBackpack( robe );

				Item[] items = defender.Backpack.FindItemsByType( typeof( Spellbook ) );

				foreach ( Spellbook book in items )
				{
					book.Delete();
				}

				defender.BodyMod = 155;
				defender.NameMod = "an infestation zombie";
				defender.Hidden = true;
				defender.Combatant = null;
				this.Combatant = null;
				defender.AddItem( new ZIRobe( defender ) );
			}
		}

		public override bool IsEnemy( Mobile m )
		{
			if ( m.Player && m.FindItemOnLayer( Layer.OuterTorso ) is ZIRobe )
				return false;

			return base.IsEnemy( m );
		}

		public override void AggressiveAction( Mobile aggressor, bool criminal )
		{
			base.AggressiveAction( aggressor, criminal );

			Item item = aggressor.FindItemOnLayer( Layer.OuterTorso );

			if ( item is ZIRobe )
			{
				item.Delete();
				AOS.Damage( aggressor, 200, 0, 100, 0, 0, 0 );
			}
		}

		public InfestZombie( Serial serial ) : base( serial )
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