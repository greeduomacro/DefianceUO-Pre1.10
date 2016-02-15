using System;
using Server.Mobiles;
using Server.Accounting;
using Server.Network;
using Server.Targeting;

namespace Server.Scripts.Commands
{
    public class FlameCircleHide
    {
        public static void Initialize()
        {
            PersonalHides.Register("troystaff", new HidingHandler(FlameCircleHide_OnHide));
        }

        private static void DelayEffect( object state )
        {
            object[] states = (object[])state;
            Mobile m = states[0] as Mobile;
            int stage = (int)states[1];

			if ( m == null || m.Deleted )
				return;

            int baseHue = 0;

            switch (stage)
            {
                case 2:
                    {
                        // outer circle
			baseHue = 0x501;
                        Effects.PlaySound(m.Location, m.Map, 0x356);
			
                        //left
                        Effects.SendLocationEffect(new Point3D(m.X - 3, m.Y + 3, m.Z), m.Map, 0x3709, 30, baseHue, 0);
                        //left top
                        Effects.SendLocationEffect(new Point3D(m.X - 4, m.Y, m.Z), m.Map, 0x3709, 30, baseHue, 0);
                        //top
                        Effects.SendLocationEffect(new Point3D(m.X - 3, m.Y - 3, m.Z), m.Map, 0x3709, 30, baseHue, 0);
                        //right top
                        Effects.SendLocationEffect(new Point3D(m.X, m.Y - 4, m.Z), m.Map, 0x3709, 30, baseHue, 0);
                        //right
                        Effects.SendLocationEffect(new Point3D(m.X + 3, m.Y - 3, m.Z), m.Map, 0x3709, 30, baseHue, 0);
                        //right bottom
                        Effects.SendLocationEffect(new Point3D(m.X + 4, m.Y, m.Z), m.Map, 0x3709, 30, baseHue, 0);
                        //bottom
                        Effects.SendLocationEffect(new Point3D(m.X + 3, m.Y + 3, m.Z), m.Map, 0x3709, 30, baseHue, 0);
                        //left bottom
                        Effects.SendLocationEffect(new Point3D(m.X, m.Y + 4, m.Z), m.Map, 0x3709, 30, baseHue, 0);

                        Timer.DelayCall(TimeSpan.FromSeconds(0.5), new TimerStateCallback(DelayEffect), new object[] { m, ++stage });
                        break;
                    }

                case 3:
                    {
                        // inner circle
                        baseHue = 0x804;
                        Effects.PlaySound(m.Location, m.Map, 0x356);
                        Effects.SendLocationEffect(new Point3D(m.X - 2, m.Y, m.Z), m.Map, 0x3709, 30, baseHue, 0);
                        Effects.SendLocationEffect(new Point3D(m.X, m.Y - 2, m.Z), m.Map, 0x3709, 30, baseHue, 0);
                        Effects.SendLocationEffect(new Point3D(m.X, m.Y + 2, m.Z), m.Map, 0x3709, 30, baseHue, 0);
                        Effects.SendLocationEffect(new Point3D(m.X + 2, m.Y, m.Z), m.Map, 0x3709, 30, baseHue, 0);
                        Timer.DelayCall(TimeSpan.FromSeconds(0.7), new TimerStateCallback(DelayEffect), new object[] { m, ++stage });
                        break;
                    }

                case 4:
                    {
                        // flame on staffposition
                        baseHue = 0x26;
                        Effects.PlaySound(m.Location, m.Map, 0x356);
                        Effects.SendLocationEffect(m.Location, m.Map, 0x3709, 30, baseHue, 0);
                        Timer.DelayCall(TimeSpan.FromSeconds(0.8), new TimerStateCallback(DelayEffect), new object[] { m, ++stage });
                        break;
                   }

                case 5:
                    {
                        // hide
                        Effects.PlaySound(m.Location, m.Map, 0x347);
                        m.Hidden = !m.Hidden;
                        m.Frozen = false;
                        break;
                    }
            }
        }

        public static void FlameCircleHide_OnHide( Mobile from )
        {
            if ( from == null || from.Deleted )
                return;

            if (!from.Hidden)
            {
                from.Direction = Direction.Down;
                from.Animate(17, 4, 1, true, false, 0);
            }
			else
				from.Direction = Direction.South;

            from.Frozen = true;
            Timer.DelayCall(TimeSpan.FromSeconds(0.2), new TimerStateCallback(DelayEffect), new object[] { from, 2 });
        }
    }
}