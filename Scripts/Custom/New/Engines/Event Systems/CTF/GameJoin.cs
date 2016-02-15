using System;
using System.Collections;
using Server.Items;
using Server.Gumps;

namespace Server.Items
{
	[FlipableAttribute( 0xEDC, 0xEDB )]
	public class GameJoinStone : Item
	{
		private CTFGame m_Game;
		private string m_GameName;
		private bool m_RandomTeam;

		[Constructable]
		public GameJoinStone( string gameName ) : base( 0xEDC )
		{
			m_GameName = gameName;

			Name = m_GameName + " Signup Stone";
			Movable = false;
		}

		public GameJoinStone( Serial serial ) : base( serial )
		{
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool RandomTeam { get { return m_RandomTeam; } set { m_RandomTeam = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public CTFGame Game{ get{ return m_Game; } set{ m_Game = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public string GameName{ get{ return m_GameName; } set{ m_GameName = value; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)1 ); // version
			writer.Write( m_RandomTeam );
			writer.Write( m_GameName );
			writer.Write( m_Game );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			switch ( version )
			{
				case 1:
				{
					m_RandomTeam = reader.ReadBool();
					goto case 0;
				}
				case 0:
				{
					m_GameName = reader.ReadString();
					m_Game = reader.ReadItem() as CTFGame;
					break;
				}
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			if (from == null || from.Deleted)
				return;

			if ( !from.InLOS( this.GetWorldLocation() ) )
			{
				from.SendLocalizedMessage( 502800 ); // You can't see that.
			}
			else if ( from.GetDistanceToSqrt( this.GetWorldLocation() ) > 4 )
			{
				from.SendLocalizedMessage( 500446 ); // That is too far away.
			}
			else if ( !IsNaked( from ) )
			{
				from.SendMessage( "You must be naked to join." );
			}
			else if ( from.Backpack == null )
			{
				from.SendMessage("You can not join without a backpack.");
			}
			else if ( m_Game != null )
			{
				if ( m_Game.OpenJoin )
				{
					if ( m_Game.IsInGame( from ) )
					{
						from.SendMessage( "You are already playing!" );
						//from.SendGump( new GameTeamSelector( m_Game ) );
					}
					else
					{
						if ( from.AccessLevel == AccessLevel.Player )
						{
							from.CloseGump( typeof(GameJoinGump) );
							from.SendGump( new GameJoinGump( m_Game, m_GameName, m_RandomTeam ) );
						}
						else
							from.SendMessage( "It might not be wise for staff to be playing..." );
					}
				}
				else
				{
					from.SendMessage( "{0} join is closed.", m_GameName );
				}
			}
			else
			{
				from.SendMessage( "This stone must be linked to a game stone.  Please contact a game master." );
			}
		}

		public override void OnSingleClick( Mobile from )
		{
			base.OnSingleClick( from );
			LabelTo( from, "[{0} Signup {1}]", m_GameName, m_Game == null ? "UNLINKED" : (m_Game.OpenJoin ? "Open" : "Closed") );
		}

		public static bool IsNaked( Mobile m )
		{
			if (m.Holding != null)
				return false;
			for( int i = 1; i < 29;i++)
			{
				if (i == 9 || i == 15 || i == 24 || i == 11 || i == 16)
					continue;
				Layer layer = (Layer)i;
				Item item = m.FindItemOnLayer( layer );
				if (item == null)
					continue;
				else if (!(item is Container))
					return false;
				else if (((Container)item).Items.Count > 0)
					return false;
			}
			return true;
		}
	}
}