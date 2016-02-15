using System;
using System.Collections;
using Server.Network;
using Server.Gumps;

namespace Server.Mobiles
{
	public class SpawnerGump : Gump
	{
        public const int DEFAULT_PROBABILTY = 100;

        private Spawner m_Spawner;

		public SpawnerGump( Spawner spawner ) : base( 50, 50 )
		{
			m_Spawner = spawner;

			AddPage( 0 );

			AddBackground( 0, 0, 305, 371, 5054 );

			AddLabel( 95, 1, 0, "Creatures List" );

			AddButton( 5, 347, 0xFB1, 0xFB3, 0, GumpButtonType.Reply, 0 );
			AddLabel( 38, 347, 0x384, "Cancel" );

			AddButton( 5, 325, 0xFB7, 0xFB9, 1, GumpButtonType.Reply, 0 );
			AddLabel( 38, 325, 0x384, "Okay" );

			AddButton( 110, 325, 0xFB4, 0xFB6, 2, GumpButtonType.Reply, 0 );
			AddLabel( 143, 325, 0x384, "Bring to Home" );

			AddButton( 110, 347, 0xFA8, 0xFAA, 3, GumpButtonType.Reply, 0 );
			AddLabel( 143, 347, 0x384, "Total Respawn" );

			for ( int i = 0;  i < 13; i++ )
			{
				AddButton( 5, ( 22 * i ) + 20, 0xFA5, 0xFA7, 4 + (i * 2), GumpButtonType.Reply, 0 );
				AddButton( 38, ( 22 * i ) + 20, 0xFA2, 0xFA4, 5 + (i * 2), GumpButtonType.Reply, 0 );

				AddImageTiled( 71, ( 22 * i ) + 20, 159, 23, 0xA40 ); //creature text box
				AddImageTiled( 72, ( 22 * i ) + 21, 157, 21, 0xBBC ); //creature text box

                AddImageTiled(235, (22 * i) + 20, 35, 23, 0xA40); //probability text box
                AddImageTiled(236, (22 * i) + 21, 33, 21, 0xBBC); //probability text box

                string name = "";
                string probability = "";

				if ( i < spawner.CreaturesName.Count )
				{
					name = (string)spawner.CreaturesName[i];
                    probability = spawner.CreaturesProbability[i].ToString();
					int count = m_Spawner.CountCreatures( name );

					AddLabel( 277, ( 22 * i ) + 20, 0, count.ToString() );
				}

				AddTextEntry( 75, ( 22 * i ) + 21, 154, 21, 0, i, name ); //creature

                AddTextEntry(239, (22 * i) + 21, 30, 21, 0, i + 20, probability); //probability
            }
		}

		public void CreateArray( RelayInfo info, Mobile from, Spawner spawner )
		{
			spawner.CreaturesName = new ArrayList();
            spawner.CreaturesProbability = new ArrayList();

			for ( int i = 0;  i < 13; i++ )
			{
                //Creatures names
                TextRelay te = info.GetTextEntry(i);
				if ( te != null )
				{
					string str = te.Text;

					if ( str.Length > 0 )
					{
						str = str.Trim();

						Type type = SpawnerType.GetType( str );

                        if (type != null)
                        {
                            spawner.CreaturesName.Add(str);
                            //Creatures probabilities
                            te = info.GetTextEntry(20 + i);
                            try
                            {
                                if (te != null && te.Text.Length > 0 && Int32.Parse(te.Text) > 0)
                                    spawner.CreaturesProbability.Add(Int32.Parse(te.Text));
                                else
                                    spawner.CreaturesProbability.Add(DEFAULT_PROBABILTY);
                            }
                            catch { spawner.CreaturesProbability.Add(DEFAULT_PROBABILTY); }
                        }
                        else
                            from.SendMessage("{0} is not a valid type name.", str);
					}
                }
			}
            if (spawner.CreaturesName.Count > 0) spawner.Start();
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			if ( m_Spawner.Deleted )
				return;

			switch ( info.ButtonID )
			{
				case 0: // Closed
				{
					break;
				}
				case 1: // Okay
				{
                    CreateArray(info, state.Mobile, m_Spawner);

					break;
				}
				case 2: // Bring everything home
				{
					m_Spawner.BringToHome();

					break;
				}
				case 3: // Complete respawn
				{
					m_Spawner.Respawn();

					break;
				}
				default:
				{
					int buttonID = info.ButtonID - 4;
					int index = buttonID / 2;
					int type = buttonID % 2;

					TextRelay entry = info.GetTextEntry( index );

					if ( entry != null && entry.Text.Length > 0 )
					{
						if ( type == 0 ) // Spawn creature
							m_Spawner.Spawn( entry.Text );
						else // Remove creatures
							m_Spawner.RemoveCreatures( entry.Text );

						CreateArray( info, state.Mobile, m_Spawner );
					}

					break;
				}
			}
		}
	}
}