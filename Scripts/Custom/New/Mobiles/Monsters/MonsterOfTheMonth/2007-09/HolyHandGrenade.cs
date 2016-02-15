using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;

namespace Server.Items
{
	public class HolyHandGrenade : Item
	{
		private Timer m_Timer;

		DeleteTimer m_DeleteTimer;
		int m_TimeLeft;

		public int TimeLeft
		{
			get{ return m_TimeLeft;}
			set{ m_TimeLeft = value; }
		}

		[Constructable]
		public HolyHandGrenade() : base( 0xE73 )
		{
			Name = "Holy Hand Grenade of Antioch";
			Weight = 10.0;
			Hue = 1717;
			m_DeleteTimer = new DeleteTimer( this );
			m_TimeLeft = 600;
		}

		public override void OnSingleClick( Mobile from )
		{
			int minutes = m_TimeLeft/60;

			this.LabelTo( from, String.Format( "This divine weapon will evaporate in {0} minutes.", minutes%60 ) );

			base.OnSingleClick( from );
		}

		private class DeleteTimer : Timer
		{
			HolyHandGrenade m_Item;

			public DeleteTimer( HolyHandGrenade item ) : base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
			{
				m_Item = item;
				Start();
				Priority = TimerPriority.OneSecond;
			}

			protected override void OnTick()
			{
				if ( m_Item == null || m_Item.Deleted )
				{
					Stop();
					return;
				}

				if ( m_Item.TimeLeft <= 0 )
				{
                    BrotherMaynard.SellBomb = true;
                    BrotherMaynard.Attempts++;
                    m_Item.Delete();
					Stop();
					return;
				}

				m_Item.TimeLeft--;
			}
		}

		public virtual object FindParent( Mobile from )
		{
			Mobile m = this.HeldBy;

			if ( m != null && m.Holding == this )
				return m;

			object obj = this.RootParent;

			if ( obj != null )
				return obj;

			if ( Map == Map.Internal )
				return from;

			return this;
		}

		public override void OnDoubleClick( Mobile from )
		{
         		from.RevealingAction();

			if ( !Movable )
				return;

			if ( from.InRange( this.GetWorldLocation(), 1 ) )
			{
         			from.Target = new ThrowTarget( this );

				if ( m_Timer == null )
				{
					from.LocalOverheadMessage( Network.MessageType.Emote, from.SpeechHue, true,  "You begin to prepare the Holy Hand Grenade..." );
					from.NonlocalOverheadMessage(Network.MessageType.Emote, from.SpeechHue, true, String.Format("{0} begins to prepare a Holy Hand Grenade...", from.Name));

					m_Timer = Timer.DelayCall( TimeSpan.FromSeconds( 0.75 ), TimeSpan.FromSeconds( 1.0 ), 5, new TimerStateCallback( Detonate_OnTick ), new object[]{ from, 4 } );
				}
			}
			else
			{
				from.SendLocalizedMessage( 502138 );
			}
      		}

		private void Detonate_OnTick( object state )
		{
			if ( Deleted )
				return;

			object[] states = (object[])state;
			Mobile from = (Mobile)states[0];
			int timer = (int)states[1];

			object parent = FindParent( from );

			if ( timer == 0 )
			{
				Point3D loc;
				Map map;

				if ( parent is Item )
				{
					Item item = (Item)parent;

					loc = item.GetWorldLocation();
					map = item.Map;
				}
				else if ( parent is Mobile )
				{
					Mobile m = (Mobile)parent;

					loc = m.Location;
					map = m.Map;
				}
				else
				{
					return;
				}

				Explode( from, true, loc, map );
			}
			else
			{
				if ( parent is Item )
					((Item)parent).PublicOverheadMessage( MessageType.Regular, 0x22, false, timer.ToString() );
				else if ( parent is Mobile )
					((Mobile)parent).PublicOverheadMessage( MessageType.Regular, 0x22, false, timer.ToString() );

				states[1] = timer - 1;
			}
		}

		private void Reposition_OnTick( object state )
		{
			if ( Deleted )
				return;

			object[] states = (object[])state;
			Mobile from = (Mobile)states[0];
			IPoint3D p = (IPoint3D)states[1];
			Map map = (Map)states[2];

			Point3D loc = new Point3D( p );
			MoveToWorld( loc, map );
		}

		private class ThrowTarget : Target
		{
			private HolyHandGrenade m_Grenade;

			public HolyHandGrenade Grenade
			{
				get{ return m_Grenade; }
			}

			public ThrowTarget( HolyHandGrenade grenade ) : base( 12, true, TargetFlags.None )
			{
				m_Grenade = grenade;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Grenade.Deleted || m_Grenade.Map == Map.Internal )
					return;

				IPoint3D p = targeted as IPoint3D;

				if ( p == null )
					return;

				Map map = from.Map;

				if ( map == null )
					return;

				from.RevealingAction();

				IEntity to;

				if ( p is Mobile )
					to = (Mobile)p;
				else
					to = new Entity( Serial.Zero, new Point3D( p ), map );

				Effects.SendMovingEffect( from, to, m_Grenade.ItemID & 0x3FFF, 7, 0, false, false, m_Grenade.Hue, 0 );

				m_Grenade.Internalize();
				Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( m_Grenade.Reposition_OnTick ), new object[]{ from, p, map } );
			}
		}

		public void Explode( Mobile from, bool direct, Point3D loc, Map map )
		{
			if ( Deleted )
				return;

			BrotherMaynard.SellBomb = true;
			BrotherMaynard.Attempts = BrotherMaynard.Attempts + 1;

			Delete();

			if ( map == null )
				return;

			Effects.PlaySound( loc, map, 0x207 );
			Effects.SendLocationEffect( loc, map, 0x36BD, 20 );

			ArrayList list = new ArrayList();

			foreach ( Mobile m in map.GetMobilesInRange( loc, 2 ) )
			{
				if ( m is BigTeethRabbit )
				{
					list.Add( m );
				}
			}

			foreach ( Mobile m in list )
			{
				m.Kill();
			}
		}

		public HolyHandGrenade( Serial serial ) : base( serial )
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