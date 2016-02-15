using Server;
using System;
using Server.Mobiles;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;
using Server.Regions;
using System.Collections;

namespace Server.Items
{
	public class SpiritGem : Item
	{

        private DeleteTimer m_Timer;
        private int m_TimeLeft;
        private const int DELETE_AFTER_SECONDS = 1200; //20 minutes

        [CommandProperty(AccessLevel.GameMaster)]
        public int TimeLeft
        {
            get { return m_TimeLeft; }
            set { if (value >= 0) m_TimeLeft = value; }
        }

		[Constructable]
		public SpiritGem() : base( 0x1EA7 )
		{
			Weight = 5;
			Name = "a spirit gem";
			Hue = 1159;
            m_TimeLeft = DELETE_AFTER_SECONDS;
            m_Timer = new DeleteTimer(this);
		}

		public SpiritGem( Serial serial ) : base( serial )
		{
            //On server restart the timer is reset to 120 seconds which ensures that the item actually decays.
            //This is inaccurate but this way we don't have to save the value and restore it.
            m_TimeLeft = DELETE_AFTER_SECONDS;
            m_Timer = new DeleteTimer(this);
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

        public override void OnSingleClick(Mobile from)
        {
            if (m_TimeLeft > 0)
            {
                int minutes = m_TimeLeft / 60;
                this.LabelTo(from, String.Format("This gem will decay in {0} minutes.", minutes % 60));
            }
            base.OnSingleClick(from);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == null || from.Backpack == null)
                return;
            else if (!IsChildOf(from.Backpack))
                from.SendMessage("The gem must be in your back pack to use");
            else if (from.Region is GuardedRegion && !((GuardedRegion)from.Region).IsDisabled())
                from.SendMessage("You are in a guarded region and cannot use this item here");
            else if (from.Region is FeluccaDungeon)
            {
                from.RevealingAction();
                from.SendMessage("You use the gem and release some ethereal guardians.");
                DoSpawn(from, this);
            }
            else
                from.SendMessage("You are not in a dungeon and cannot use this item here");
        }

		public void DoSpawn( Mobile from, Item item )
		{
			Map map = this.Map;

				if ( map == null )
					return;

				int newSpawned = Utility.RandomMinMax( 2, 5 );

              	for ( int i = 0; i < newSpawned; ++i )
              	{
					EthyMob spawn = new EthyMob();

					bool validLocation = false;
					Point3D loc = this.Location;

					for ( int j = 0; !validLocation && j < 10; ++j )
					{
						int x = from.X + Utility.Random( -5, 5 );
						int y = from.Y + Utility.Random( -5, 5 );
						int z = map.GetAverageZ( x, y );

						if ( validLocation = map.CanFit( x, y, this.Z, 16, false, false ) )
							loc = new Point3D( x, y, Z );
						else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
							loc = new Point3D( x, y, z );
					}

					spawn.MoveToWorld( loc, map );
					item.Delete();
				}
		}
        private class DeleteTimer : Timer
        {
            SpiritGem m_Item;

            public DeleteTimer(SpiritGem item)
                : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
            {
                m_Item = item;
                Start();
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Item == null || m_Item.Deleted)
                {
                    Stop();
                    return;
                }

                if (m_Item.TimeLeft <= 0)
                {
                    m_Item.Delete();
                    Stop();
                    return;
                }
                m_Item.TimeLeft--;
            }
        }
	}
}