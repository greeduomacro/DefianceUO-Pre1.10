using System;
using Server;

namespace Server.Items
{
	public abstract class BaseBodyPart : Item
	{
		public virtual TimeSpan DecayDelay { get { return TimeSpan.FromMinutes( 5.0 ); } }
		public DateTime DecayStarted;

		private Timer m_DeleteTimer;

		public BaseBodyPart( int itemid ) : base( itemid )
		{
			DecayStarted = DateTime.Now;
			m_DeleteTimer = Timer.DelayCall( DecayDelay, new TimerCallback( Delete ) );
		}

		public BaseBodyPart( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( DecayStarted );
			//writer.Write( DecayDelay );

		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			DecayStarted = reader.ReadDateTime();
			//reader.ReadTimeSpan();

			if ( DecayStarted - DecayDelay < DateTime.Now )
				m_DeleteTimer = Timer.DelayCall( TimeSpan.Zero, new TimerCallback( Delete ) );
			else
				m_DeleteTimer = Timer.DelayCall( DecayDelay - (DateTime.Now - DecayStarted), new TimerCallback( Delete ) );
		}
	}
}