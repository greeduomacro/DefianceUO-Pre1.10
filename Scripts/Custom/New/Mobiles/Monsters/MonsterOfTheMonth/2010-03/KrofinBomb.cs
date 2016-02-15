using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;

namespace Server.Items
{
	public class KrofinBOMB : Item
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
		public KrofinBOMB() : base( 0x0F04 )
		{
			Name = "A Handmade Krofin Bomb";
			Weight = 20.0;
			Hue = 1175;
			m_DeleteTimer = new DeleteTimer( this );
			m_TimeLeft = 300;
		}

		public override void OnSingleClick( Mobile from )
		{
			int minutes = m_TimeLeft/60;

			this.LabelTo( from, String.Format( "This massive bomb will evaporate in {0} minutes.", minutes%60 ) );

			base.OnSingleClick( from );
		}

		private class DeleteTimer : Timer
		{
			KrofinBOMB m_Item;

			public DeleteTimer( KrofinBOMB item ) : base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
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
					from.LocalOverheadMessage( Network.MessageType.Emote, from.SpeechHue, true,  "An ugly smell floats around along with a wierd ticker..." );
					from.NonlocalOverheadMessage(Network.MessageType.Emote, from.SpeechHue, true, String.Format("{0} uses the Krofin Bomb...", from.Name));

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
			private KrofinBOMB m_Bomb;

			public KrofinBOMB bomb
			{
				get{ return m_Bomb; }
			}

			public ThrowTarget( KrofinBOMB bomb ) : base( 12, true, TargetFlags.None )
			{
				m_Bomb = bomb;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Bomb.Deleted || m_Bomb.Map == Map.Internal )
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

				Effects.SendMovingEffect( from, to, m_Bomb.ItemID & 0x3FFF, 7, 0, false, false, m_Bomb.Hue, 0 );

				m_Bomb.Internalize();
				Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( m_Bomb.Reposition_OnTick ), new object[]{ from, p, map } );
			}
		}

		public void Explode( Mobile from, bool direct, Point3D loc, Map map )
		{
			if ( Deleted )
				return;


			Delete();

			if ( map == null )
				return;

			Effects.PlaySound( loc, map, 0x207 );
			Effects.SendLocationEffect( loc, map, 0x36BD, 20 );

			ArrayList list = new ArrayList();

			foreach ( Mobile m in map.GetMobilesInRange( loc, 0 ) )
			{
				if ( m is HojanKing )
				{
					list.Add( m );
				}
			}

			foreach ( Mobile m in list )
			{
				m.Kill();
			}
		}

		public KrofinBOMB( Serial serial ) : base( serial )
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