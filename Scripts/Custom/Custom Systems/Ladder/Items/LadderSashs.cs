/*
*	Ladder system by Morten Legarth (c)taunted.dk ( legarth@taunted.dk )
*	Version: v0.01 -  25-05-2004
*
*	File Description:	A sash that is automatically maintained on a player
*						with a certain rank. This feature is experimental.
*
*
*	This system has been written for use at the Blitzkrieg free-shard
*	http://blitzkrieg.dorijan.de . Unauthorized reproduction is prohibited.
*/

using System;
using Server.Items;

namespace Server.Ladder
{
	public class LadderSash : BodySash
	{
        int m_Rank;
		int m_Points;

		public LadderSash(int rank, int points)
		{
            Layer = Layer.Earrings;
            Movable = false;
            Configure(rank, points);

        }

        public void Configure(int rank, int points)
        {
            m_Rank = rank;
            m_Points = points;

            switch (m_Rank)
            {
                case 1:
                    Name = "Grandmaster Duelist";
                    Hue = 1260;
                    break;
                case 2:
                    Name = "Master Duelist";
                    Hue = 1266;
                    break;
                case 3:
                    Name = "Adept Duelist";
                    Hue = 1278;
                    break;
                default:
                    Delete();
                    break;
            }
        }

		public override void OnSingleClick(Mobile from)
		{
		        base.OnSingleClick(from);
			this.LabelTo(from, "Honor: "+ m_Points);
		}


		public LadderSash(Serial serial) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			writer.Write(m_Rank);
			writer.Write(m_Points);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			m_Rank = reader.ReadInt();
			m_Points = reader.ReadInt();
		}
	}
}