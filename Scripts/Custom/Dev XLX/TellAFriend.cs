/*
 * RunUO Shard Referral System
 * Author: Shadow1980
 * Files: TellAFriend.cs
 * Version 1.5
 * Public Release: 17-04-2006 || Latest Release 20-04-2006
 *
 * Description:
 * This system allows you to reward players for bringing friends into the shard.
 * When a new player joins, they receive a gump asking them who referred them to the shard.
 * They can enter the account name of the person in question there, which will add two tags to their account.
 * v1.4+ they can also target a player character ingame and there is no mention of Account Name anywhere.
 * Once certain configurable conditions are met, the referrer will receive a reward.
 * Everything is handled on login, so to receive a reward for a referral both accounts have to remain active.
 *
 * Please note only the referrer receives a reward, but you can easely give a reward to the new player as well.
 * To do this, uncomment lines 71 and 72. The reward can be found at line 251. Modify as you see fit.
 */
using System;
using Server.Accounting;
using Server.Network;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server
{
	public class TellAFriend
	{
		// Configure Required Ingame Time For Both New Player and Referrer Before a Reward is given:
		public static readonly TimeSpan RewardTime = TimeSpan.FromHours( 120.0 );
        // Both Accounts need to have logged in during the last x days set here:
		public static readonly DateTime mindate = DateTime.Now - TimeSpan.FromDays( 7.0 );
		// New Player Account has this many days to enter a referrer & also requires to be this old before a reward is given to the referrer:
		public static readonly DateTime age = DateTime.Now - TimeSpan.FromDays( 7.0 );
        // Edit Shard Name
		public static readonly string TAFShardName = "Defiance";

		public static void Initialize()
		{
			EventSink.Login += new LoginEventHandler( TAFLogin );
		}

		private static void TAFLogin( LoginEventArgs args )
		{
			Mobile m = args.Mobile;
			Account ac = (Account)m.Account;
			bool toldfriend = ToldAFriend( m );
			bool gotfriend = GotAFriend( m );
			if ( ac.Created >= age )
			{
				if ( !toldfriend )
				{
				m.SendGump( new TAFGump( m ) );
				}
			}
            else if (toldfriend)
            {
                string friend = ac.GetTag("Referrer");
                if (friend == null)
                {
                    ac.RemoveTag("ToldAFriend");
                    ac.RemoveTag("Referrer");
                }
                else
                {

                    Account friendacct = Accounts.GetAccount(friend);
                    if (friendacct == null)
                    {
                        ac.RemoveTag("ToldAFriend");
                        ac.RemoveTag("Referrer");
                    }
                    else
                    {
                        if (ac.LastLogin > mindate && friendacct.LastLogin > mindate && ac.TotalGameTime >= RewardTime && friendacct.TotalGameTime >= RewardTime)
                        {
                            m.SendMessage(String.Format("Your friend will receive a reward for referring you to {0} next time (s)he logs in.", TAFShardName));
                            //m.SendMessage( String.Format( "You receive a reward for your loyalty to {0}.", TAFShardName ) );
                            //m.AddToBackpack( new ReferrerReward() );
                            if (Convert.ToBoolean(ac.GetTag("GotAFriend")))
                            {
                                string friends = ac.GetTag("GotFriend") + "," + ac.ToString();
                                friendacct.SetTag("GotFriend", friends);
                            }
                            else
                            {
                                friendacct.SetTag("GotAFriend", "true");
                                friendacct.SetTag("GotFriend", ac.ToString());
                            }
                            ac.RemoveTag("Referrer");
                            ac.RemoveTag("ToldAFriend");
                        }
                    }
                }
            }
            else if (gotfriend)
            {
                string friend = ac.GetTag("GotFriend");
                string[] friends = friend.Split(',');
                for (int i = 0; i < friends.Length; ++i)
                {
                    m.SendMessage(String.Format("You receive a reward for referring one of your friends to {0}.", TAFShardName));
                    //m.AddToBackpack( new ReferrerReward() );
                    m.AddToBackpack(new SoulStone(m.Account.ToString()));
                }
                ac.RemoveTag("GotAFriend");
                ac.RemoveTag("GotFriend");
            }
		}

		public class TAFGump : Gump
		{
			private NetState m_State;
			public TAFGump( Mobile from ) : this( from, "" )
			{
			}
			private string tere;
			private const int LabelColor32 = 0xFFFFFF;

			public string Center( string text )
			{
				return String.Format( "<CENTER>{0}</CENTER>", text );
			}

			public string Color( string text, int color )
			{
				return String.Format( "<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", color, text );
			}
			public TAFGump( Mobile from, string initialText ) : base( 30, 20 )
			{
				if ( from == null )
					return;

           	 	this.AddPage(1);
				this.AddBackground(50, 0, 479, 309, 9390);
				Mobile m_from = from;
				Account tgt = (Account)from.Account;
				int terg = tgt.TotalGameTime.Days;
				int terh = tgt.TotalGameTime.Hours;
				int teri = tgt.TotalGameTime.Minutes;
				int terj = tgt.TotalGameTime.Seconds;
				tere = from.Name;

				//this.AddImage(15, 0, 10400);
				//this.AddImage(15, 225, 10402);
				this.AddImage(481, 0, 10410);
				//this.AddImage(485, 225, 10412);
				this.AddImage(49, 15, 990);
				this.AddImage(89, 46, 1025);
				this.AddLabel(205, 43, 88, "Account Name" );
				this.AddLabel(205, 57, 0x480, from.Account.ToString() );
				this.AddLabel(355, 43, 88, "Online Character" );
				this.AddLabel(355, 57, 50, tere );
				this.AddLabel(205, 80, 88, "Total Game Time" );
				this.AddLabel(205, 100, 50, terg.ToString() + " Days." );
				this.AddLabel(205, 115, 50, terh.ToString() + " Hours." );
				this.AddLabel(205, 130, 50, teri.ToString() + " Minutes." );
				this.AddLabel(205, 145, 50, terj.ToString() + " Seconds." );
				bool toldfriend = ToldAFriend( from );
				if ( !toldfriend )
				{
				this.AddLabel(205, 175, 50, String.Format("Who referred you to {0}?", TAFShardName ) );
				this.AddButton(450, 200, 4023, 4025, 1, GumpButtonType.Reply, 0); //Okay for acct name button
				this.AddImageTiled(300, 200, 140, 20, 0xBBC );
				this.AddTextEntry(300, 200, 140, 20, 1152, 2, "");
				this.AddLabel(205, 200, 88, "Account Name:" );
				this.AddLabel(205, 230, 88, "Or target your friend's character:" );
				this.AddButton(450, 230, 4023, 4025, 2, GumpButtonType.Reply, 0); //Target player button
				}
				else
				{
				this.AddLabel(205, 200, 88, "You already entered a referrer." );
				}
				this.AddHtml(205, 255, 205, 56,  Color( Center( initialText ), 0xFF0000 ) , false, false);
		 	}

			public override void OnResponse( NetState state, RelayInfo info )
			{
				Mobile from = state.Mobile;
				Account acct = (Account)from.Account;
				int id = info.ButtonID;

				if( id == 1 )
				{
					string input = info.GetTextEntry( 2 ).Text;
					Account tafacc = Accounts.GetAccount( input );
					string initialText = "";

					if ( tafacc == null )
					{
						initialText = String.Format( "Account: '{0}' NOT found", input );
					}
					else if ( input == "" )
					{
						initialText = "Please enter a valid Account name.";
					}
					else if ( input == acct.ToString() )
					{
						initialText = "You can't enter your own Account name!";
					}
					else
					{
						initialText = String.Format( "{0} Marked as Referrer", tafacc );
						acct.SetTag( "ToldAFriend", "true" );
						acct.SetTag( "Referrer", tafacc.ToString() );
					}
					from.SendGump( new TAFGump( from, initialText ) );
				}
				if ( id == 2 )
				{
					from.BeginTarget( 10, false, TargetFlags.None, new TargetCallback( TAFTarget ) );
					from.SendMessage( String.Format("Please target the character of the person who referred you to {0}", TAFShardName ) );
				}
			}
            public void TAFTarget( Mobile from, object target )
            {
            	string initialText = "";
            	if ( target is PlayerMobile && target != null )
            	{
            		Mobile friend = (Mobile)target;
            		Account fracct = (Account)friend.Account;
					Account acct = (Account)from.Account;

					if ( fracct == acct )
					{
					    initialText = "You can't be your own referrer!";
					}
					else
					{
						initialText = String.Format( "{0} Marked as Referrer", friend.Name );
						friend.SendMessage( String.Format("{0} has just marked you as referrer to {1}.", from.Name, TAFShardName ) );
						acct.SetTag( "ToldAFriend", "true" );
						acct.SetTag( "Referrer", fracct.ToString() );
					}
            	}
            	else
            	{
            		initialText = "Please select a player character.";
            	}
            	from.SendGump( new TAFGump( from, initialText ) );
            }
		}
	 	private static bool ToldAFriend( Mobile m )
        {
            Account acct=(Account)m.Account;
            bool told = Convert.ToBoolean( acct.GetTag("ToldAFriend") );
            if ( !told )
               	return false;
            return true;
    	}
        private static bool GotAFriend( Mobile m )
        {
        	Account acct=(Account)m.Account;
           	bool got = Convert.ToBoolean( acct.GetTag("GotAFriend") );
            if ( !got )
            	return false;
           	return true;
    	}
    }
	public class ReferrerReward : HalfApron
	{
		[Constructable]
		public ReferrerReward() : base()
		{
			Name = String.Format("{0} Referrer Apron", TellAFriend.TAFShardName );
			Hue = 1266;
			LootType = LootType.Blessed;
			Attributes.DefendChance = 10;
			Resistances.Poison = 5;
		}

		public ReferrerReward(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}