using System;
using Server;
using Server.Engines.CannedEvil;

namespace Server.Items
{
	public class ChampionIdol : Item
	{
		private ChampionSpawn m_Spawn;

		[CommandProperty( AccessLevel.GameMaster )]
		public ChampionSpawn Spawn{ get{ return m_Spawn; } set{ m_Spawn = value; InvalidateProperties(); } }


		[Constructable]
		public ChampionIdol( ChampionSpawn spawn ) : base( 0x1F18 )
		{
			m_Spawn = spawn;

			Name = "Idol of Champion";

			Movable = false;
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			if ( m_Spawn != null )
				m_Spawn.Delete();
		}

		public ChampionIdol( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_Spawn );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_Spawn = reader.ReadItem() as ChampionSpawn;

					if ( m_Spawn == null )
						Delete();

					break;
				}
			}
		}
	}
}