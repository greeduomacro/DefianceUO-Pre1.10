using System;
using Server;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
	public class DTOrc : BaseCreature
	{
		private CTFGame m_Game;
		private CTFTeam m_Team;
		private int m_TeamID;
		private int m_Timer;
		private int m_Points;

		[Constructable()]
        public DTOrc() : base( AIType.AI_Healer, FightMode.None, 10, 1, 0.2, 0.4 )
        {
            Body = 17;
            BaseSoundID = 0x45A;
            CantWalk = true;

            m_TeamID = -1;
            m_Timer = 30;
            m_Points = 10;

            SetStr( 100 );
            SetDex( 100 );
            SetInt( 100 );

            SetHits( 500 );
            SetMana( 200 );

            SetSkill( SkillName.Meditation, 100.0 );
            SetSkill( SkillName.Magery, 100.0 );
            SetSkill( SkillName.MagicResist, 200.0 );
            SetSkill( SkillName.Wrestling, 100.0 );
        }

        public override void Damage(int amount, Mobile from)
        {
            if (m_Game != null && from != null)
            {
                if (!m_Game.Running)
                    amount = 0;

                Item item = from.FindItemOnLayer(Layer.OuterTorso);

                if (item is CTFRobe)
                {
                    if (item.Hue == m_Team.Hue)
                        amount = 0;
                }
                else
                    amount = 0;
            }
            else
                amount = 0;

            base.Damage(amount, from);
        }

		public override void OnDamage(int amount, Mobile from, bool willKill)
		{
			if ( willKill == true && m_Game != null && from != null)
			{
				CTFTeam team = m_Game.GetTeam( from );

				willKill = false;
				this.Blessed = true;
				this.Hidden = true;
				this.Combatant = null;
				this.Freeze( TimeSpan.FromSeconds( m_Timer + 1 ) );

				from.SendMessage( "You killed the {0} orc!", m_Team.Name );
                            	this.Game.PlayerMessage( "{0} ({1}) killed the {2} orc!", from.Name, team.Name, m_Team.Name );
                            	team.Points += m_Points;

				Timer.DelayCall( TimeSpan.FromSeconds( m_Timer ), new TimerCallback( RTimer ) );
			}

			base.OnDamage(amount, from, willKill);
		}

		public void RTimer()
		{
            if (Game != null)
            {
                this.Hits = this.HitsMax;
                this.Blessed = false;
                this.Hidden = false;
                this.Game.PlayerMessage("The {0} Orc has respawned", m_Team.Name);
            }
		}


        public override void AggressiveAction(Mobile aggressor, bool criminal)
        {
            base.AggressiveAction(aggressor, criminal);

            if (m_Game != null && m_Game.Running && aggressor != null)
            {
                Item item = aggressor.FindItemOnLayer(Layer.OuterTorso);

                if (item is CTFRobe)
                {
                    CTFTeam team = m_Game.GetTeam(aggressor);

                    if (team != null && team.Name == m_Team.Name)
                    {
                        AOS.Damage(aggressor, 200, 0, 100, 0, 0, 0);
                        this.Game.PlayerMessage("{0} ({1}) was killed for betraying his orc!", aggressor.Name, team.Name);
                        aggressor.BoltEffect(0);
                        aggressor.PlaySound(0x307);
                    }
                }
            }
        }

		public DTOrc( Serial serial ) : base(serial)
		{
		}

		public CTFTeam CTFTeam { get { return m_Team; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int TeamID
		{
			get
			{
				UpdateTeam();
				if ( m_Team != null )
					return m_Team.UId;
				else
					return m_TeamID;
			}
			set
			{
				m_TeamID = value;
				UpdateTeam();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public CTFGame Game
		{
			get{ return m_Game; }
			set
			{
				m_Game = value;
				UpdateTeam();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int RespawnTimer
		{
			get{ return m_Timer; }
			set{ m_Timer = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int PointsPerDeath
		{
			get{ return m_Points; }
			set{ m_Points = value; }
		}

		public void UpdateTeam()
		{
			if ( m_Game != null && m_TeamID != -1 )
			{
				m_Team = m_Game.GetTeam( m_TeamID );
				if ( m_Team != null )
				{
					this.Hue = m_Team.Hue;
					this.Name = m_Team.Name + " Orc";
				}
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.AccessLevel >= AccessLevel.GameMaster )
			{
				UpdateTeam();
				if ( m_Team != null )
					from.SendGump( new PropertiesGump( from, m_Team ) );
				else
					from.SendMessage( "Set game and team on props first!" );
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );

			writer.Write( m_TeamID );
			writer.Write( m_Game );
			writer.Write( m_Timer );
			writer.Write( m_Points );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_TeamID = reader.ReadInt();
					m_Game = reader.ReadItem() as CTFGame;
					m_Timer = reader.ReadInt();
					m_Points = reader.ReadInt();
					break;
				}
			}
		}
	}
}