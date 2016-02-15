using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

//Its a modified BellOfTheDead to work with an enum

namespace Server.Engines.SilenceAddon
{
	public enum BellType
	{
		DarkIron,
		Wooden,
		Blood,
		Beast,
		Noxious,
	}

	public class SilenceBell : Item
	{
		private BellType m_Type;

		[CommandProperty( AccessLevel.GameMaster )]
		public BellType Type{ get{ return m_Type; } set{ m_Type = value; } }

		[Constructable]
		public SilenceBell( BellType type ) : base( 0x91A )
		{
			Movable = false;
			m_Type = type;
			switch ( type )
			{
				case BellType.DarkIron: Hue = 0x83A; Name = "Dark Iron Bell"; break;
				case BellType.Wooden: Hue = 0x6D7; Name = "Wooden Bell"; break;
				case BellType.Blood: Hue = 0x485; Name = "Bloody Bell"; break;
				case BellType.Beast: Hue = 0x3E8; Name = "Bell of the Beast"; break;
				case BellType.Noxious: Hue = 0x4F3; Name = "Noxious Bell"; break;
			}
		}

		private SSum m_SSum;
		private BaseFakeMob m_Fake;
		private bool m_Summoning;

		[CommandProperty( AccessLevel.GameMaster, AccessLevel.Administrator )]
		public SSum SSum
		{
			get{ return m_SSum; }
			set{ m_SSum = value; }
		}

		[CommandProperty( AccessLevel.GameMaster, AccessLevel.Administrator )]
		public BaseFakeMob Fake
		{
			get{ return m_Fake; }
			set{ m_Fake = value; }
		}

		[CommandProperty( AccessLevel.GameMaster, AccessLevel.Administrator )]
		public bool Summoning
		{
			get{ return m_Summoning; }
			set{ m_Summoning = value; }
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.InRange( GetWorldLocation(), 2 ) )
			{
				if ( m_Type == BellType.Noxious )
					from.Poison = Poison.Lethal;
				BeginSummon( from );
			}
			else
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
		}

		public virtual void BeginSummon( Mobile from )
		{
			if ( m_SSum != null && !m_SSum.Deleted )
			{
				from.SendMessage("The bell has already been rang, so be patient!" );
			}
			else if ( m_Fake != null && !m_Fake.Deleted )
			{
				from.SendMessage("This bell has been rung not too long ago, you must wait!" );
			}
			else if
				(
					(m_Type == BellType.DarkIron && Tallon.Active) ||
					(m_Type == BellType.Wooden && Zirux.Active) ||
					(m_Type == BellType.Blood && Krog.Active) ||
					(m_Type == BellType.Beast && Alfirix.Active) ||
					(m_Type == BellType.Noxious && Ignis.Active)
				)
				from.SendMessage("The creature has already been summoned. Go forth and vanquish him!");
			else if ( !m_Summoning )
			{
				m_Summoning = true;

				Effects.PlaySound( GetWorldLocation(), Map, 0x100 );

				Timer.DelayCall( TimeSpan.FromSeconds( 8.0 ), new TimerStateCallback( EndSummon ), from );
			}
		}

		public virtual void EndSummon( object state )
		{
			Mobile from = (Mobile)state;

			if ( m_SSum != null && !m_SSum.Deleted )
			{
				from.SendMessage("The bell has already been rang, so be patient!" );
			}
			else if ( m_Fake != null && !m_Fake.Deleted )
			{
				from.SendMessage("This bell has been rung not to long ago, you must wait!" );
			}
			else if ( m_Summoning )
			{
				m_Summoning = false;

				Point3D loc = GetWorldLocation();

				loc.Z -= 16;

				Effects.SendLocationParticles( EffectItem.Create( loc, Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 0, 0, 2023, 0 );
				Effects.PlaySound( loc, Map, 0x1FE );

				switch ( m_Type )
				{
					case BellType.DarkIron: m_SSum = new SSum( BellType.DarkIron ); break;
					case BellType.Wooden: m_SSum = new SSum( BellType.Wooden ); break;
					case BellType.Blood: m_SSum = new SSum( BellType.Blood ); break;
					case BellType.Beast: m_SSum = new SSum( BellType.Beast ); break;
					case BellType.Noxious: m_SSum = new SSum( BellType.Noxious ); break;
				}

				m_SSum.Direction = (Direction)(7 & (4 + (int)from.GetDirectionTo( loc )));;
				m_SSum.MoveToWorld( loc, Map );

				m_SSum.Bell = this;
				m_SSum.AngryAt = from;
				m_SSum.BeginGiveWarning();
				m_SSum.BeginRemove( TimeSpan.FromSeconds( 40.0 ) );
			}
		}

		public SilenceBell( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (Mobile) m_SSum );
			writer.Write( (Mobile) m_Fake );
			writer.Write( (int) m_Type );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_SSum = reader.ReadMobile() as SSum;
			m_Fake = reader.ReadMobile() as BaseFakeMob;
			m_Type = (BellType)reader.ReadInt();

			if ( m_SSum != null )
				m_SSum.Delete();
		}
	}
}