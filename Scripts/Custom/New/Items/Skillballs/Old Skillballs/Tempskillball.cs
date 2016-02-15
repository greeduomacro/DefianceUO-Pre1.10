// Sunny: uor devs, this file has been messed up by me,
// i have made new cunstructors for this file.
// i have added a maximum duration of 365 days, you can remove this ofc.
// the property can be accessed by GM and up.


//	RunUO script: Skill Ball
//	Copyright (c) 2003 by Wulf C. Krueger <wk@mailstation.de>
//
//	This script is free software; you can redistribute it and/or modify
//	it under the terms of the GNU General Public License as published by
//	the Free Software Foundation; version 2 of the License applies.
//
//	This program is distributed in the hope that it will be useful,
//	but WITHOUT ANY WARRANTY; without even the implied warranty of
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//	GNU General Public License for more details.
//
//	Please do NOT remove or change this header.

//	Official Defiance(c) TempSkillBall - by [Dev]Kamron aka XxSP1DERxX - Kamron@defianceuo.com

using System;
using Server.Network;
using Server.Items;
using Server.Gumps;
using System.Collections;
using Server.Mobiles;

namespace Server.Items
{
    public class TempSkillBall : Item, ITempItem
	{
		private int m_SkillBonus;
        private DateTime m_RemovalTime;
        private int m_DaysLeft;

        public DateTime RemovalTime { get { return m_RemovalTime; } }
        public int DaysLeft { get { return m_DaysLeft; } set { m_DaysLeft = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int LeftDays
        {
            get { return m_DaysLeft; }
            set
            {
                int min = Math.Min(value, 365);
                m_RemovalTime = DateTime.Now + TimeSpan.FromDays(min);
                m_DaysLeft = min;
                CreateCustomName();
            }
        }

		[CommandProperty( AccessLevel.GameMaster )]
		public int SkillBonus
		{
			get { return m_SkillBonus; }
			set
			{
				m_SkillBonus = value;
                CreateCustomName();
			}
		}

		[Constructable]
		public TempSkillBall( int SkillBonus, int days ) : base( 3699 )
		{
            m_RemovalTime = DateTime.Now + TimeSpan.FromDays(Math.Min(days, 365));
            TemporaryItemSystem.Verify(this);
			m_SkillBonus = SkillBonus;
            CreateCustomName();
			Hue = 1266;
		}

		[Constructable]
		public TempSkillBall() : this( 0, 30 )
		{
		}

        public TempSkillBall(Serial serial)
            : base(serial)
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( (this.SkillBonus == 0) && (from.AccessLevel < AccessLevel.GameMaster) )
			{
				from.SendMessage("This skill ball isn't charged. Please page for a GM.");
				return;
			}
			else if ( (from.AccessLevel >= AccessLevel.GameMaster) && (this.SkillBonus == 0) )
			{
				from.SendGump( new PropertiesGump( from, this ) );
				return;
			}
			else if ( !IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
            else if (!from.HasGump(typeof(TempSkillBallGump)))
				from.SendGump( new TempSkillBallGump( from, this ) );
			else
				from.SendMessage("You're already using a skill ball.");
		}

        public void CreateCustomName()
        {
            Name = String.Format("an unlimited TempSkillBall for {0} skill points, {1} days left to use this", m_SkillBonus, m_DaysLeft);
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			writer.Write( m_SkillBonus );
            writer.Write(m_RemovalTime);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch (version)
			{
				case 0 :
				{
					m_SkillBonus = reader.ReadInt();
                    m_RemovalTime = reader.ReadDateTime();
                    TemporaryItemSystem.Verify(this);
					break;
				}
			}

		}
	}
}

namespace Server.Gumps
{
	public class TempSkillBallGump : Gump
	{
		private const int FieldsPerPage = 14;

		private Skill m_Skill;
		private TempSkillBall m_skb;

		public TempSkillBallGump ( Mobile from, TempSkillBall skb ) : base ( 20, 30 )
		{
			m_skb = skb;

			AddPage ( 0 );
			AddBackground( 0, 0, 260, 351, 5054 );

			AddImageTiled( 10, 10, 240, 23, 0x52 );
			AddImageTiled( 11, 11, 238, 21, 0xBBC );

			AddLabel( 65, 11, 0, "Skills you can raise" );

			AddPage( 1 );

			int page = 1;
			int index = 0;

			Skills skills = from.Skills;

			int number;
			if ( Core.AOS )
				number = 0;
			else
				number = 5;

			for ( int i = 0; i < ( skills.Length - number ); ++i )
			{
				if ( index >= FieldsPerPage )
				{
					AddButton( 231, 13, 0x15E1, 0x15E5, 0, GumpButtonType.Page, page + 1 );

					++page;
					index = 0;

					AddPage( page );

					AddButton( 213, 13, 0x15E3, 0x15E7, 0, GumpButtonType.Page, page - 1 );
				}

				Skill skill = skills[i];

				if ( (skill.Base + m_skb.SkillBonus) <= 100 && skill.Lock != SkillLock.Locked && skill.Lock != SkillLock.Down )
				{
					AddImageTiled( 10, 32 + (index * 22), 240, 23, 0x52 );
					AddImageTiled( 11, 33 + (index * 22), 238, 21, 0xBBC );

					AddLabelCropped( 13, 33 + (index * 22), 150, 21, 0, skill.Name );
					AddImageTiled( 180, 34 + (index * 22), 50, 19, 0x52 );
					AddImageTiled( 181, 35 + (index * 22), 48, 17, 0xBBC );
					AddLabelCropped( 182, 35 + (index * 22), 234, 21, 0, skill.Base.ToString( "F1" ) );

					if ( from.AccessLevel >= AccessLevel.Player )
						AddButton( 231, 35 + (index * 22), 0x15E1, 0x15E5, i + 1, GumpButtonType.Reply, 0 );
					else
						AddImage( 231, 35 + (index * 22), 0x2622 );

					++index;
				}
			}
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;

			if ( from == null || m_skb.Deleted || !m_skb.IsChildOf(from.Backpack) )
				return;

			if ( info.ButtonID > 0 )
			{
				m_Skill = from.Skills[(info.ButtonID-1)];

				if ( m_Skill == null )
					return;

				double count = from.Skills.Total / 10;
				double cap = from.SkillsCap / 10;
				ArrayList decreased = new ArrayList();
				double decreaseamount = 0.0;
				double bonuscopy = m_skb.SkillBonus;
				if ( (count + bonuscopy) > cap )
				{
					for (int i = 0;i < from.Skills.Length;i++ )
					{
						if (from.Skills[i].Lock == SkillLock.Down && from.Skills[i].Base > 0.0)
						{
							decreased.Add(from.Skills[i]);
							decreaseamount += from.Skills[i].Base;
						}
					}

					if (decreaseamount <= 0)
					{
						from.SendMessage("You have exceeded the skill cap and do not have a skill set to be decreased.");
						return;
					}
				}

				if (m_Skill.Base + bonuscopy <= 100)
				{
					if ((cap - count) + decreaseamount >= bonuscopy)
					{
						if (cap - count >= bonuscopy)
						{
							m_Skill.Base += bonuscopy;
							bonuscopy = 0;
						}
						else
						{
							m_Skill.Base += bonuscopy;
							bonuscopy -= cap - count;

							foreach( Skill s in decreased)
							{
								if (s.Base >= bonuscopy)
								{
									s.Base -= bonuscopy;
									bonuscopy = 0;
								}
								else
								{
									bonuscopy -= s.Base;
									s.Base = 0;
								}

								if (bonuscopy == 0)
									break;
							}
						}
						m_skb.Delete();
						((PlayerMobile)from).Young = false;
					}
					else
						from.SendMessage("You must have enough skill set down to compensate for the skill gain.");

				}
				else
					from.SendMessage("You have to choose another skill.");
			}
		}
	}
}