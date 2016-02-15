using System;
using Server;
using Server.Mobiles;

namespace Server.Factions
{
	public class PlayerState : IComparable
	{
		private Mobile m_Mobile;
		private Faction m_Faction;
		private PlayerStateCollection m_Owner;
		// jakob, added this
		private int m_StolenSigilsCount;
		// end
		private int m_KillPoints;
		// jakob, added this
		private DateTime m_Joined;
		// end
		private DateTime m_Leaving;
		// jakob, added this
		private DateTime m_LastKill;
		// end
		// Kamron, added this
		private DateTime m_LastDecay;
		//end
		private MerchantTitle m_MerchantTitle;
		private RankDefinition m_Rank;
		private SilverGivenCollection m_SilverGiven;

		private Town m_Sheriff;
		private Town m_Finance;

		public Mobile Mobile{ get{ return m_Mobile; } }
		public Faction Faction{ get{ return m_Faction; } }
		public PlayerStateCollection Owner{ get{ return m_Owner; } }
		// jakob, public accessor
		public int StolenSigilsCount{ get{ return m_StolenSigilsCount; } set{ m_StolenSigilsCount = value; } }
		// end
		public int KillPoints{ get{ return m_KillPoints; } set{ m_KillPoints = value; } }
		public MerchantTitle MerchantTitle{ get{ return m_MerchantTitle; } set{ m_MerchantTitle = value; Invalidate(); } }
		public RankDefinition Rank{ get{ return m_Rank; } set{ m_Rank = value; Invalidate(); } }
		public Town Sheriff{ get{ return m_Sheriff; } set{ m_Sheriff = value; Invalidate(); } }
		public Town Finance{ get{ return m_Finance; } set{ m_Finance = value; Invalidate(); } }
		public SilverGivenCollection SilverGiven{ get{ return m_SilverGiven; } }

		// jakob, added these
		public DateTime LastKill { get{ return m_LastKill; } set { m_LastKill = value; } }

		public DateTime Joined{ get{ return m_Joined; } set { m_Joined = value; } }
		// end

		// Kamron, added this
		public DateTime LastDecay{ get{ return m_LastDecay; } set{ m_LastDecay = value; } }

		public void DecayKills()
		{
			if ( m_KillPoints <= 0 )
				return;
			TimeSpan killtime = DateTime.Now - m_LastKill;
			TimeSpan decaytime = DateTime.Now - m_LastDecay;
			if ( killtime.Days > 0 && decaytime >= TimeSpan.FromDays( 365.0 ) )
			{
				m_KillPoints -= Math.Min( m_KillPoints, decaytime.Days );
				m_LastDecay = DateTime.Now;
				m_Mobile.SendMessage( "You have lost {0} faction points for inactivity.", killtime.Days );
			}
		}
		//end

		public DateTime Leaving{ get{ return m_Leaving; } set{ m_Leaving = value; } }

		public bool IsLeaving{ get{ return ( m_Leaving > DateTime.MinValue ); } }

		public bool CanGiveSilverTo( Mobile mob )
		{
			if ( m_SilverGiven == null )
				return true;

			for ( int i = 0; i < m_SilverGiven.Count; ++i )
			{
				SilverGivenEntry sge = m_SilverGiven[i];

				if ( sge.IsExpired )
					m_SilverGiven.RemoveAt( i-- );
				else if ( sge.GivenTo == mob )
					return false;
			}

			return true;
		}

		public void OnGivenSilverTo( Mobile mob )
		{
			if ( m_SilverGiven == null )
				m_SilverGiven = new SilverGivenCollection();

			m_SilverGiven.Add( new SilverGivenEntry( mob ) );
		}

		public void Invalidate()
		{
			if ( m_Mobile is PlayerMobile )
				((PlayerMobile)m_Mobile).InvalidateProperties();
		}

		public void Attach()
		{
			if ( m_Mobile is PlayerMobile )
				((PlayerMobile)m_Mobile).FactionPlayerState = this;
		}

		public PlayerState( Mobile mob, Faction faction, PlayerStateCollection owner )
		{
			m_Mobile = mob;
			m_Faction = faction;
			m_Owner = owner;

			// jakob, added these
			m_Joined = DateTime.Now;
			m_LastKill = DateTime.Now;
			// end

			// Kamron, added this
			m_LastDecay = DateTime.Now;
			// end

			m_Rank = faction.Definition.Ranks[faction.Definition.Ranks.Length - 1];

			Attach();
			Invalidate();
		}

		public PlayerState( GenericReader reader, Faction faction, PlayerStateCollection owner )
		{
			m_Faction = faction;
			m_Owner = owner;

			int version = reader.ReadEncodedInt();

			switch ( version )
			{
				// Kamron, deserialize this
				case 2:
				{
					m_LastDecay = reader.ReadDateTime();
					goto case 1;
				}
				// end
				// jakob, make sure to deserialize these
				case 1:
				{
					m_Joined = reader.ReadDateTime();
					m_LastKill = reader.ReadDateTime();
					m_StolenSigilsCount = reader.ReadEncodedInt();

					goto case 0;
				}
				// end
				case 0:
				{
					m_Mobile = reader.ReadMobile();

					m_KillPoints = reader.ReadEncodedInt();
					m_MerchantTitle = (MerchantTitle)reader.ReadEncodedInt();

					m_Leaving = reader.ReadDateTime();

					break;
				}
			}

			// Kamron, added this
			if ( m_LastDecay == DateTime.MinValue )
				m_LastDecay = m_LastKill;
			DecayKills();
			// end

			Attach();
		}

		public void Serialize( GenericWriter writer )
		{
			// jakob, 1 instead of 0
			// Kamron, 2 instead of 1
			writer.WriteEncodedInt( (int) 2 ); // version
			// end

			// Kamron, serialize this
			writer.Write( (DateTime) m_LastDecay );
			// end


			// jakob, serialize these
			writer.Write( (DateTime) m_Joined );
			writer.Write( (DateTime) m_LastKill );
			writer.WriteEncodedInt( (int) m_StolenSigilsCount );
			// end

			writer.Write( (Mobile) m_Mobile );

			writer.WriteEncodedInt( (int) m_KillPoints );
			writer.WriteEncodedInt( (int) m_MerchantTitle );

			writer.Write( (DateTime) m_Leaving );
		}

		public static PlayerState Find( Mobile mob )
		{
			if ( mob is PlayerMobile )
				return ((PlayerMobile)mob).FactionPlayerState;

			return null;
		}

		public int CompareTo( object obj )
		{
			return m_KillPoints - ((PlayerState)obj).m_KillPoints;
		}
	}
}