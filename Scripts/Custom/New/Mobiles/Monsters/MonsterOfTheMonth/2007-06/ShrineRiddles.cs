using System;
using Server;
using Server.EventPrizeSystem;

namespace Server.Items
{
	public enum ShrineType
	{
		Chaos,
		Compassion,
		Honesty,
		Honour,
		Humility,
		Justice,
		Sacrifice,
		Spirituality,
		Valour,
		Wisdom,
	}

	public class ShrineRiddle : Item
	{
		private int m_TrysLeft;

		private int TrysLeft
		{
			get{ return m_TrysLeft; }
			set{ m_TrysLeft = value; }
		}

		public ShrineType m_Type;

		public ShrineType Type{ get{ return m_Type; } set{ m_Type = value; InvalidateProperties(); } }

		[Constructable]
		public ShrineRiddle( ShrineType type ) : base( 0x14F0 )
		{
			m_Type = type;
			m_TrysLeft = Utility.RandomMinMax( 3, 6 );
			Hue = Utility.RandomMetalHue();

			if ( this.m_Type == ShrineType.Chaos )
			{
				switch ( Utility.Random( 3 ) )
				{
					case 0: Name = "Disorder"; break;
					case 1: Name = "Entropy"; break;
					case 2: Name = "On the edge of the mountains of wind will you find the opposite of order"; break;
				}
			}

			if ( this.m_Type == ShrineType.Compassion )
			{
				switch ( Utility.Random( 3 ) )
				{
					case 0: Name = "To share your fellow man's suffering"; break;
					case 1: Name = "An attempt to alleviate the suffering of someone as if the suffering were your own"; break;
					case 2: Name = "Walk into the desert and you shall find that empathy grows"; break;
				}
			}

			if ( this.m_Type == ShrineType.Honesty )
			{
				switch ( Utility.Random( 3 ) )
				{
					case 0: Name = "I cannot tell a lie"; break;
					case 1: Name = "I promise to tell the truth, the whole truth and nothing but the truth"; break;
					case 2: Name = "Walk through the ice and you shall find truth"; break;
				}
			}

			if ( this.m_Type == ShrineType.Honour )
			{
				switch ( Utility.Random( 3 ) )
				{
					case 0: Name = "With nobility of soul, magnanimity, and a scorn of meanness"; break;
					case 1: Name = "Sometimes to satisfy it you have to duel"; break;
                    case 2: Name = "Travel to one of the mainland's southern tips and you might just find the code"; break;
				}
			}

			if ( this.m_Type == ShrineType.Humility )
			{
				switch ( Utility.Random( 3 ) )
				{
					case 0: Name = "I am unpretentious and modest"; break;
					case 1: Name = "Someone who does not think that he or she is better or more important than others"; break;
					case 2: Name = "Walk though the land of fire and you discover you are from the earth"; break;
				}
			}

			if ( this.m_Type == ShrineType.Justice )
			{
				switch ( Utility.Random( 3 ) )
				{
					case 0: Name = "The proper ordering of things and persons within a society"; break;
					case 1: Name = "To kill a murderer is to gain in this"; break;
					case 2: Name = "Somewhere around the large circular lake is where you will find the reason for punishment"; break;
				}
			}

			if ( this.m_Type == ShrineType.Sacrifice )
			{
				switch ( Utility.Random( 3 ) )
				{
					case 0: Name = "I offer to you the heart of this goat"; break;
					case 1: Name = "Selfless deeds to aid others"; break;
					case 2: Name = "Far to the north is where I will slaughter this bull in your name my lord"; break;
				}
			}

			if ( this.m_Type == ShrineType.Spirituality )
			{
				switch ( Utility.Random( 3 ) )
				{
					case 0: Name = "I shall travel the path of enlightenment"; break;
					case 1: Name = "Attempting to form a closer connection to the creator"; break;
					case 2: Name = "Above a small pool of water I connect to something greater then my self"; break;
				}
			}

			if ( this.m_Type == ShrineType.Valour )
			{
				switch ( Utility.Random( 3 ) )
				{
					case 0: Name = "I have courage"; break;
					case 1: Name = "Fortitude and bravery traits of this"; break;
					case 2: Name = "Travel to an island in the south and you will find a sword etched in marble"; break;
				}
			}

			if ( this.m_Type == ShrineType.Wisdom )
			{
				switch ( Utility.Random( 3 ) )
				{
					case 0: Name = "Accumulated philosophic or scientific knowledge"; break;
					case 1: Name = "The only one of ten to be hidden away in a dungeon"; break;
					case 2: Name = "Only deep within contempt will one find good judgment"; break;
				}
			}
		}

		public override void OnSingleClick( Mobile from )
		{
			int trysleft = m_TrysLeft;
			this.LabelTo( from, String.Format( "a riddle with {0} attempts left to solve", trysleft) );
			base.OnSingleClick( from );
		}

		public override void OnDoubleClick( Mobile from )
		{
			Point3D chaos = new Point3D( 1457, 844, 5 );
			Point3D compassion = new Point3D( 1858, 875, -1 );
			Point3D honesty = new Point3D( 4209, 564, 47 );
			Point3D honour = new Point3D( 1726, 3528, 3 );
			Point3D humility = new Point3D( 4273, 3697, 0 );
			Point3D justice = new Point3D( 1300, 633, 16 );
			Point3D sacrifice = new Point3D( 3354, 289, 4 );
			Point3D spirituality = new Point3D( 1594, 2490, 20 );
			Point3D valour = new Point3D( 2491, 3931, 5 );
			Point3D wisdom = new Point3D( 5458, 610, 50 );

			if (from == null || from.Backpack == null )
				return;

			if ( this.m_Type == ShrineType.Chaos && from.InRange( chaos, 1 ) || this.m_Type == ShrineType.Compassion && from.InRange( compassion, 1 ) || this.m_Type == ShrineType.Honesty && from.InRange( honesty, 1 ) || this.m_Type == ShrineType.Honour && from.InRange( honour, 1 ) || this.m_Type == ShrineType.Humility && from.InRange( humility, 1 ) || this.m_Type == ShrineType.Justice && from.InRange( justice, 1 ) || this.m_Type == ShrineType.Sacrifice && from.InRange( sacrifice, 1 ) || this.m_Type == ShrineType.Spirituality && from.InRange( spirituality, 1 ) || this.m_Type == ShrineType.Valour && from.InRange( valour, 1 ) || this.m_Type == ShrineType.Wisdom && from.InRange( wisdom, 1 ) )
			{
				if ( Utility.Random( 30 ) < 1 )
				{
					from.AddToBackpack( new RareLavaTile() );
					from.SendMessage( "You have been rewarded with the main prize" );
				}
				else if ( Utility.Random( 15 ) < 1 )
				{
					from.AddToBackpack( new JuneBook() );
					from.SendMessage( "You have been rewarded with the MotM chain prize" );
				}
				else
				{
                    if ( Utility.Random( 10 ) < 1 )
                    {
                        from.AddToBackpack( new Gold( 2000, 3000 ) );
                        from.SendMessage( "Your luck is poor this time and only receive some gold..." );
                    }
                    else
                    {
                        from.AddToBackpack( new BronzePrizeToken() );
                        from.SendMessage( "You have been rewarded with a bronze prize token" );
                    }
				}

				this.Delete();
			}
			else
			{
				from.SendMessage( "You are not at the correct location!" );
				if ( m_TrysLeft == 1 )
			 	{
					from.SendMessage( "You failed to solve the riddle and now it destroys itself" );
					this.Delete();
				}
				else
				{
					this.m_TrysLeft -= 1;
				}
			}
		}

		public ShrineRiddle( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_Type );
			writer.Write( (int) m_TrysLeft );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_Type = (ShrineType)reader.ReadInt();
					m_TrysLeft = reader.ReadInt();

					break;
				}
			}
		}
	}
}