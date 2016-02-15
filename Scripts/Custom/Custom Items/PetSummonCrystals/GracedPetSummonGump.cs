using System;
using System.Collections;
using Server;
using Server.Guilds;
using Server.Network;
using Server.Prompts;
using Server.Targeting;
using Server.Items;
using Server.Mobiles;

namespace Server.Gumps
{
	public class GracedPetSummonGump : Gump
	{
		private PlayerMobile m_Mobile;
		private GracedPetSummonBall m_GracedPetSummonBall;

		public GracedPetSummonGump( PlayerMobile user, GracedPetSummonBall ball ) : base( 20, 30 )
		{
			m_Mobile = user;
			m_GracedPetSummonBall = ball;

			Dragable = true;

			AddPage( 0 );
			AddBackground( 0, 0, 200, 315, 2520 );

			ArrayList pets = GetPetList( m_Mobile );

			AddHtml( 30, 20, 180, 30, "", false, false );
			AddHtml( 30, 35, 180, 30, "IMPORTANT!", false, false );
			AddHtml( 30, 50, 180, 30, "Cost: 1,000 gold.", false, false );
			AddHtml( 30, 65, 180, 30, "Pets lose no skills.", false, false );
			AddHtml( 30, 70, 180, 30, "", false, false );

			for ( int i = 0; i < pets.Count; i++ )
			{
				AddButton( 30, 95 + 30 * i, 4005, 4007, i + 1, GumpButtonType.Reply, 0 );
				AddHtml( 65, 95 + 30 * i, 145, 30, ((Mobile)pets[i]).Name, false, false );
			}

			AddButton( 30, 245, 4005, 4007, 0, GumpButtonType.Reply, 0 );
			AddHtml( 65, 245, 145, 30, "Exit", false, false ); // EXIT
		}

		public static void EnsureClosed( Mobile m )
		{
			m.CloseGump( typeof( GracedPetSummonGump ) );
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			int id = info.ButtonID;

			if ( m_GracedPetSummonBall.Deleted || id <= 0 )
				return;

			id --;

			ArrayList pets = GetPetList( m_Mobile );

			if ( id < pets.Count && id >= 0 )
			{
				if ( m_Mobile.BankBox.ConsumeTotal( typeof( Gold ), 1000 ) )
				{
					m_GracedPetSummonBall.UseCharge();
                    	              //SufferSkillLoss( (Mobile)pets[id], 10 );
					((Mobile)pets[id]).MoveToWorld( m_Mobile.Location, m_Mobile.Map );
					m_Mobile.SendMessage( "You pay the fee and your pet is returned to you." );
				}
				else
				{
					m_Mobile.SendMessage( "You need 1,000 gold in your bank to return your pet." );
				}
			}
		}

		//public void SufferSkillLoss( Mobile mobile, double percentage )
		//{
		//	for ( int i = 0; i < mobile.Skills.Length; ++i )
		//	{
		//		mobile.Skills[i].Base = mobile.Skills[i].Base * (1 - percentage / 100);
		//	}
		//}

		public ArrayList GetPetList( PlayerMobile from )
		{
			ArrayList pets = new ArrayList();

			foreach ( Mobile m in World.Mobiles.Values )
			{
				if ( m is BaseCreature )
				{
					BaseCreature bc = (BaseCreature)m;

					if ( bc is IMount && ((IMount)bc).Rider != null )
						continue;

					if ( (bc.Controlled && bc.ControlMaster == from) )
						pets.Add( bc );
				}
			}

			return pets;
		}
	}
}