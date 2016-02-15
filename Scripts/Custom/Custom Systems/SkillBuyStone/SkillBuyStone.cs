		////////////////////////////////////////////////////////////////////////////////////////
	   /////                                                                            ////////
	  //////    Version: 1.0        Author: Vorspire        Shard: Alternate-PK         ////////
	 ///////    Edited by A_Li_N                                                        ////////
	////////                                                                            ////////
	////////    QuakeNet: #Alternate-PK		MSN: alere_flammas666@hotmail.com           ////////
	////////                                                                            ////////
	////////    Description: This stone allows players to increase skills based on      ////////
	////////                 the settings you chose. This stone is fully custom-        ////////
	////////                 -isable and includes an experience feature, if your        ////////
	////////                 shard uses a similar experience system. Everything in      ////////
	////////                 this script is straight forward and easy to under-         ////////
	////////                 -stand. On behalf of Alternate-PK, I hope you enjoy        ////////
	////////                 this script to its full potential.                         ////////
	////////                                                                            ////////
	////////    Distribution: This script can be freely distributed, as long as the     ////////
	////////                  credit notes are left intact.	This script can also be     ////////
	////////                  modified, as long as the credit notes are left intact.    ///////
	////////                                                                            //////
	/////////////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;

namespace Server.Items
{
	public class SkillBuyStone : Item
	{
		//Start Prices
		private int m_PriceInGold = 50000;
		private double m_MaxCanBuyTo = 60.0;
		private int m_SkillIncrease = 5;
		public ArrayList m_alDisabledSkills;

		public bool m_CoolLooking = false; //Toggles Alpha Areas on/off (Default: false/off) / can be used ingame

		[CommandProperty( AccessLevel.Seer )]
		public bool CoolLooking{get{return m_CoolLooking;}set{m_CoolLooking = value;InvalidateProperties();}}

		[CommandProperty( AccessLevel.Seer )]
		public int SkillIncrease{get{return m_SkillIncrease;}set{m_SkillIncrease = value;InvalidateProperties();}}

		[CommandProperty( AccessLevel.Seer )]
		public double MaxCanBuyTo{get{return m_MaxCanBuyTo;}set{m_MaxCanBuyTo = value;InvalidateProperties();}}

		//Prices
		[CommandProperty( AccessLevel.Seer )]
		public int PriceInGold{get{return m_PriceInGold;}set{m_PriceInGold = value;InvalidateProperties();}}
		//End Prices

		[Constructable]
		public SkillBuyStone() : base( 0xED4 )
		{
			Movable = false;
			Hue = 2036;
			Name = "Skill Purchasing Stone";
			m_alDisabledSkills = new ArrayList();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !from.InRange( GetWorldLocation(), 2 ) )
				from.SendLocalizedMessage( 500446 ); // That is too far away.
			else
				from.SendGump( new SkillStoneGump( this, from, 0 ) );
		}

		public SkillBuyStone( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			// Version 1
			writer.Write( m_alDisabledSkills.Count );
			for ( int i=0;i<m_alDisabledSkills.Count;i++ )
				writer.Write( (int)m_alDisabledSkills[i] );

			// Version 0
			writer.Write( m_PriceInGold );
			writer.Write( m_MaxCanBuyTo );
			writer.Write( m_SkillIncrease );
			writer.Write( m_CoolLooking );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch( version )
			{
				case 1:
					m_alDisabledSkills = new ArrayList();
					int size = reader.ReadInt();
					for ( int i=0;i<size;i++ )
						m_alDisabledSkills.Add( (SkillName) reader.ReadInt() );
					goto case 0;
				case 0:
					m_PriceInGold = reader.ReadInt();
					m_MaxCanBuyTo = reader.ReadDouble();
					m_SkillIncrease = reader.ReadInt();
					m_CoolLooking = reader.ReadBool();
					break;
			}

			if( version == 0 )
				m_alDisabledSkills = new ArrayList();
		}
	}
}