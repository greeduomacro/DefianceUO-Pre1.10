//Al@2006-07-11
using System;
using Server.Mobiles;
using Server.Accounting;

namespace Server.Scripts.Commands
{
	public class PersonalHideAlstaff
	{
		public static void Initialize()
		{
			PersonalHides.Register("phoenixstaff1371", new HidingHandler(Al_OnHide));
		}

		public static void Al_OnHide(Mobile m)
		{
			Point3D loc = m.Location;

			Effects.PlaySound(loc, m.Map, 0x5A7); //collection_reward
			int[] coords = new int[]{ -15, 0, 15 };

			foreach ( int x in coords )
				foreach ( int y in coords )
					MovingParticles(new Point3D(loc.X + x, loc.Y + y, loc.Z), loc, m.Map);

			Timer.DelayCall(TimeSpan.FromSeconds(1), new TimerStateCallback(DelayEffect), new object[] { m, 2 });
		}

		private static void MovingParticles(Point3D p1, Point3D p2, Map map)
		{
			Effects.SendMovingParticles(new Entity(Serial.Zero, p1, map), new Entity(Serial.Zero, p2, map), 0x36F4, 5, 0, false, false, 0, 0, 9535, 1, 0, (EffectLayer)255, 0x100);
		}

		private static void DelayEffect(object state)
		{
			object[] states = (object[])state;
			Mobile m = states[0] as Mobile;
			int stage = (int)states[1];

			if ( m == null || m.Deleted )
				return;

			switch (stage)
			{
				case 2:
					{
						Effects.SendLocationParticles(m, 0x3709, 10, 30, 0x2b, 0, 5052, 0);//Flamestrike
						Timer.DelayCall(TimeSpan.FromSeconds(1), new TimerStateCallback(DelayEffect), new object[] { m, ++stage });
						break;
					}
				case 3:
					{
						Effects.PlaySound(m.Location, m.Map, 0x5Bc);//bladeweave
						m.Hidden = !m.Hidden;
						break;
					}
			}
		}
	}
}