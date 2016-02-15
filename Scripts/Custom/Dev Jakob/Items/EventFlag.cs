using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class EventFlag : Item
	{
		private Mobile m_Owner;

		[Constructable]
		public EventFlag() : base( 0x1627 )
		{
			Weight = 4.0;
			//Movable = false;
			Hue = 1153;
			Name = "Event Flag";
		}

		public EventFlag( Serial serial ) : base( serial )
		{
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner{ get{ return m_Owner; } set{ m_Owner = value; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );

			writer.Write( m_Owner );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_Owner = reader.ReadMobile();
					break;
				}
			}
		}

		public override void OnAdded( object parent )
		{
			if ( m_Owner != null )
			{
				m_Owner.SolidHueOverride = -1;
				m_Owner = null;
			}

			if (RootParent is Mobile)
			{
				m_Owner = ((Mobile)RootParent);
				m_Owner.SolidHueOverride = 1150;
			}
			else
				m_Owner = null;
		}

                public static bool ExistsOn( Mobile mob )
		{
			Container pack = mob.Backpack;

			return ( pack != null && pack.FindItemByType( typeof( EventFlag ) ) != null );
		}

		public override void Delete()
		{
			if ( m_Owner != null )
			{
				m_Owner.SolidHueOverride = -1;
				m_Owner = null;
			}
			base.Delete();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if (!(RootParent is Mobile) && (from.InRange( this.GetWorldLocation(), 2 ) || VerifyMove( from )) && from.Backpack.FindItemByType( typeof(EventFlag), true) == null)
				from.AddToBackpack( this );
			else
				from.SendMessage( "You already possess a flag" );
		}

		public override DeathMoveResult OnInventoryDeath( Mobile parent )
		{
			if ( parent != null )
			{
				parent.SolidHueOverride = -1;
			}
			if (!VerifyMove(parent))
			{
				PlayerMobile killer = null;
				if ( parent is PlayerMobile )
				{
					TimeSpan lastTime = TimeSpan.MaxValue;

					for ( int i = 0; i < parent.Aggressors.Count; ++i )
					{
						AggressorInfo info = (AggressorInfo)parent.Aggressors[i];
						if ( info.Attacker != null && info.Attacker is PlayerMobile && info.Attacker.Alive && (DateTime.Now - info.LastCombatTime) < lastTime && (DateTime.Now - info.LastCombatTime) < TimeSpan.FromMinutes( 2.0 ) && info.Attacker.Backpack != null && info.Attacker.Backpack.FindItemByType( typeof(EventFlag), true) == null )
						{
							killer = info.Attacker as PlayerMobile;
							lastTime = (DateTime.Now - info.LastCombatTime);
						}
					}

					for ( int i = 0; i < parent.Aggressed.Count; ++i )
					{
						AggressorInfo info = (AggressorInfo)parent.Aggressed[i];
						if ( info.Defender != null && info.Defender is PlayerMobile && info.Defender.Alive && (DateTime.Now - info.LastCombatTime) < lastTime && (DateTime.Now - info.LastCombatTime) < TimeSpan.FromMinutes( 2.0 ) && info.Defender.Backpack != null && info.Defender.Backpack.FindItemByType( typeof(EventFlag), true) == null )
						{
							killer = info.Defender as PlayerMobile;
							lastTime = (DateTime.Now - info.LastCombatTime);
						}
					}
				}
				if ( killer == null || killer.Backpack == null )
				{
					this.Delete();
					new EventFlag().MoveToWorld( parent.Location, parent.Map);
				}
				else
				{
					this.Delete();
					killer.PlaceInBackpack( new EventFlag() );
				}
			}
			return base.OnInventoryDeath( parent );
		}

		public override bool VerifyMove( Mobile from )
		{
			return ( from.AccessLevel >= AccessLevel.GameMaster );
		}

		public override bool Decays{ get{ return false; } }
	}
}