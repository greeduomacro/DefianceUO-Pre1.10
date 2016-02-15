using System;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Accounting;
using Server.Logging;
using Server.Gumps;
using Server.Targeting;
using Server.Prompts;

namespace Server.Items
{
	public class MembershipTicket : Item
	{
		private TimeSpan m_MemberShipTime;

		[CommandProperty(AccessLevel.Administrator)]
		public TimeSpan MemberShipTime
		{
			get { return m_MemberShipTime; }
			set
			{
				m_MemberShipTime = value;

				if (m_MemberShipTime == TimeSpan.MaxValue)
				{
					Name = "a ticket for permanent membership";
				}
				else
				{
					Name = "a membership ticket for " + m_MemberShipTime.Days + " days";
				}
			}
		}

		[Constructable]
		public MembershipTicket() : base( 0x14F0 )
		{
			Weight = 1.0;
			Name = "a membership ticket";
			LootType = LootType.Blessed;
		}

		public MembershipTicket(Serial serial) : base( serial )
		{
		}

        	public override void OnDoubleClick( Mobile from )
		{
			if ( from.Backpack == null || !IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			else
				from.SendGump( new MemberAcceptGump( from, this ) );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
			writer.Write(m_MemberShipTime);

		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			switch (version)
			{
				case 0:
					m_MemberShipTime = TimeSpan.Zero;
					break;
				case 1:
					m_MemberShipTime = reader.ReadTimeSpan();
					break;
				default:
					m_MemberShipTime = TimeSpan.Zero;
					break;
			}
		}

	}

	public class MemberAcceptGump : Gump
    	{
        	private Mobile m_Mobile;
        	private MembershipTicket m_Deed;
		private TimeSpan m_MemberShipTime;

		public MemberAcceptGump(Mobile from, MembershipTicket deed) : base(30, 20)
		{
            		m_Mobile = from;
            		m_Deed = deed;

			this.Closable = true;
            		this.Disposable = true;
            		this.Dragable = true;
            		this.Resizable = false;

            		AddPage(0);
           	 	AddBackground(20, 117, 460, 302, 9200);
            		AddImage(169, 162, 1418);
            		AddImage(34, 131, 2520);
            		AddImage(72, 131, 2521);
            		AddImage(34, 169, 2523);
            		AddImage(72, 169, 2524);
            		AddImage(242, 169, 2525);
            		AddImage(34, 281, 2526);
            		AddImage(72, 281, 2527);
            		AddImage(242, 281, 2528);
            		AddImage(242, 131, 2522);
            		AddTextEntry(69, 166, 191, 19, 0, 0, @"You are about to activate the ");
            		AddTextEntry(71, 223, 189, 20, 0, 0, @"account. You will not be able");
            		AddTextEntry(70, 194, 200, 20, 0, 0, @"Defiance Membership to your");
            		AddTextEntry(71, 253, 200, 20, 0, 0, @"to undo this.");
            		AddTextEntry(336, 123, 132, 20, 0, 0, @"Defiance Membership");
            		AddTextEntry(82, 301, 200, 20, 0, 0, @"Do you agree ?");
            		AddButton(44, 384, 247, 248, 0, GumpButtonType.Reply, 0);
            		AddButton(389, 384, 241, 242, 1, GumpButtonType.Reply, 0);
            		AddImage(32, 294, 52);
            		Account a = from.Account as Account;
		}

        
        public override void OnResponse(NetState sender, RelayInfo info)
        {
		Mobile from = sender.Mobile;
            	Mobile m = sender.Mobile;

		if ( m_Deed.Deleted || m_Mobile == null || m_Mobile.Backpack == null || !m_Deed.IsChildOf( sender.Mobile.Backpack ))
		return;
      
        
       			if ( info.ButtonID == 0 )
			{
			     if (m == null || m.Deleted)
                             return;

			     if (m != null && m.NetState != null)
			     {
				if (m_Deed.MemberShipTime == TimeSpan.Zero)
				{    
					if (m.AccessLevel < AccessLevel.GameMaster)
					{
						m.SendMessage("There was a problem with this ticket. You need to give this to an Administrator and ask him to use it.");
					}
					
                                      	else if (m.AccessLevel < AccessLevel.Administrator)
					{
						m.SendMessage("Only an person with Administrator accesslevel can fix this ticket. Find one and ask him to doubleclick ticket for instructions");
					}
					
                                      	else if (m.AccessLevel == AccessLevel.Administrator)
					{
						m.SendMessage("This is ticket probably from the first version of the ticket. It didn't save properbly and you need to set the MemberShipTime "+
								"with [props .The tickets name should tell you how many days to set it to(Hint: 24 hrs = 1 day). If the ticket is for permanent "+
								"membership. That cannot be set in props. You have to [add goldendontationbox and get it from there.");
					}
				}
				               
                             	else if (m.Backpack != null)
				{
					DateTime DonationStart = DateTime.MinValue;
					TimeSpan DonationDuration = TimeSpan.Zero;

					try
					{
						DonationStart = DateTime.Parse(((Account)from.Account).GetTag("DonationStart"));
						DonationDuration = TimeSpan.Parse(((Account)from.Account).GetTag("DonationDuration"));
					}
					catch
					{
					}

                    			//Al: added logging
                    			GeneralLogging.WriteLine("Donation", "Donationticketuse: Acc:{0}, DStart: {2}, DDur: {3}, Serial:{4}, Tickettime: {1}"
                        		,from.Account, m_Deed.MemberShipTime, DonationStart, DonationDuration, Serial);

                    			//Al: Also reset DonationStart when the person is no active donator
                    			if ((DonationStart == DateTime.MinValue && DonationDuration == TimeSpan.Zero)||(DateTime.Now - DonationStart > DonationDuration))
					{
						try
						{
							((Account)from.Account).SetTag("DonationStart", DateTime.Now.ToString());
							((Account)from.Account).SetTag("DonationDuration", m_Deed.MemberShipTime.ToString());
							from.SendMessage("Your donation status has been updated.");
							//this.Delete();
                            		
							m_Deed.Delete();
                            				GeneralLogging.WriteLine("Donation", "    Acc:{0}, no or outdated previous donation, new DStart: {1}, new DDur: {2}"
                                			, from.Account, DateTime.Now.ToString(), m_Deed.MemberShipTime.ToString());
                        			}
					
						catch	
						{
							from.SendMessage("An error ocurred trying to update your donation status. Contact an Administrator.");
                            				GeneralLogging.WriteLine("Donation", "    Acc:{0}, error updating donation (1) please contact a developer"
                                			, from.Account);
                        			}
					}
				
					else if (DonationDuration == TimeSpan.MaxValue)
					{
						//already at max
						from.SendMessage("You are already at permanent membership status.");
                        			GeneralLogging.WriteLine("Donation", "    Acc:{0}, already at permanent status."
                            			, from.Account, DonationStart, DonationDuration);
                    			}
				
					else
					{
						//existing, active donation
					
						try
						{
                            				//Avoid overflow
							if (m_Deed.MemberShipTime == TimeSpan.MaxValue)
								DonationDuration = m_Deed.MemberShipTime;
							
							else
								DonationDuration += m_Deed.MemberShipTime;

								((Account)from.Account).SetTag("DonationDuration", DonationDuration.ToString());
								from.SendMessage("Your donation status has been updated.");
								//this.Delete();
                           					m_Deed.Delete();
                            					GeneralLogging.WriteLine("Donation", "    Acc:{0}, active donation, new DonationStart: {1}, new DonationDuration: {2}"
                                				, from.Account, DonationStart, DonationDuration);
                        			}
					
						catch
						{
							from.SendMessage("An error ocurred trying to update your donation status. Contact an Administrator.");
                            				GeneralLogging.WriteLine("Donation", "    Acc:{0}, error updating donation (2) please contact a developer"
                                			, from.Account);
                        			}
					}
			        }
				else
				{
					from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
				}
			    }
		
                        
                        //m_Deed.Delete();
                        
                    	}
                

		    else
                    {
                        from.CloseGump(typeof(MemberAcceptGump));
                        
                    }
              }
        }
    
}
