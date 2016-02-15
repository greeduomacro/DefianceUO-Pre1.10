    //////////////////////////////////
   //			           //
  //      Scripted by Raelis      //
 //		             	 //
//////////////////////////////////
using System;
using System.Collections;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using Server.Network;

namespace Server.Items
{
   	public class DragonEgg: Item
   	{
		public bool m_AllowEvolution;
		public Timer m_EvolutionTimer;
		private DateTime m_End;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool AllowEvolution
		{
			get{ return m_AllowEvolution; }
			set{ m_AllowEvolution = value; }
		}

		[Constructable]
		public DragonEgg() : base( 0x0C5D )
		{
			Weight = 11;
			Name = "a dragon egg";
			Hue = 1153;
			AllowEvolution = false;

			m_EvolutionTimer = new EvolutionTimer( this, TimeSpan.FromMinutes( 480.0 ) );
			m_EvolutionTimer.Start();
			m_End = DateTime.Now + TimeSpan.FromMinutes( 480.0 );
		}

            	public DragonEgg( Serial serial ) : base ( serial )
            	{
           	}

		public override void OnDoubleClick( Mobile from )
		{
                        if ( !IsChildOf( from.Backpack ) )
                        {
                                from.SendMessage( "You must have the dragon's egg in your backpack to hatch it." );
                        }

 			else if ( from.Skills[SkillName.AnimalTaming].Base < 95 )
			{
			from.SendMessage( "You are not skilled enough to control such a creature." );
			}

			else if ( AllowEvolution )
			{
				if ( from.Followers + 3 > from.FollowersMax )
					from.SendMessage( "You have too many followers." );
				else
				{
					this.Delete();
					from.SendMessage( "You are now the proud owner of a dragon hatchling!!" );

					EvolutionDragon dragon = new EvolutionDragon();

	         			dragon.Map = from.Map;
	         			dragon.Location = from.Location;

					dragon.Controlled = true;

					dragon.ControlMaster = from;

					dragon.IsBonded = true;
				}
			}
			else
			{
				from.SendMessage( "This egg is not yet ready to be hatched." );
			}
		}

           	public override void Serialize( GenericWriter writer )
           	{
              		base.Serialize( writer );
              		writer.Write( (int) 1 );
					writer.WriteDeltaTime( m_End );
           	}

           	public override void Deserialize( GenericReader reader )
           	{
              		base.Deserialize( reader );
              		int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				{
					m_End = reader.ReadDeltaTime();
					m_EvolutionTimer = new EvolutionTimer( this, m_End - DateTime.Now );
					m_EvolutionTimer.Start();

					break;
				}
				case 0:
				{
					TimeSpan duration = TimeSpan.FromDays( 1.0 );

					m_EvolutionTimer = new EvolutionTimer( this, duration );
					m_EvolutionTimer.Start();
					m_End = DateTime.Now + duration;

					break;
				}
			}
           	}

		private class EvolutionTimer : Timer
		{
			private DragonEgg de;

			private int cnt = 0;

			public EvolutionTimer( DragonEgg owner, TimeSpan duration ) : base( duration )
			{
				de = owner;
			}

			protected override void OnTick()
			{
				cnt += 1;

				if ( cnt == 1 )
				{
					de.AllowEvolution = true;
				}
			}
		}
        }
}