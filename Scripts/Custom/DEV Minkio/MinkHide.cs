// Personal hider of Minkio. Do not touch or get asskicked!

using System;
using Server.Mobiles;
using Server.Accounting;
using Server.Network;
using Server.Targeting;

namespace Server.Scripts.Commands
{
	public class PersonalHideMinkstaff
	{
		public static void Initialize()
		{
			PersonalHides.Register("infra", new HidingHandler(Mink_OnHide));
		}

		private static void DestroyGate_Callback(object state)
		{
			object[] states = (object[])state;
			Mobile from = states[0] as Mobile;
			Item gate = states[1] as Item;

			if ( from == null || from.Deleted || gate == null || gate.Deleted )
				return;

			Effects.SendLocationParticles((IEntity)gate, 0x36BD, 20, 10, 5044); // explosion
			Effects.PlaySound(gate.Location,gate.Map,0x307); // explosion

			gate.Delete();
			from.PlaySound(0x11d);
		}

		private static void GoInGate_Callback(object state)
		{
			object[] states = (object[])state;
			Mobile from = states[0] as Mobile;
			Item gate = states[1] as Item;

			if ( from == null || from.Deleted || gate == null || gate.Deleted )
				return;

			from.Hidden = true;
			Effects.PlaySound(from.Location, from.Map, 0x1fe);
			Timer.DelayCall(TimeSpan.FromSeconds(0.7), new TimerStateCallback(DestroyGate_Callback), new object[] { from, gate });
		}

		private static void ChangeDirection_Callback(object state)
		{
			object[] states = (object[])state;
			Mobile from = states[0] as Mobile;
			Item gate = states[1] as Item;

			if ( from == null || from.Deleted || gate == null || gate.Deleted )
				return;

			from.Direction = Direction.North;
			Timer.DelayCall(TimeSpan.FromSeconds(0.5), new TimerStateCallback(GoInGate_Callback), new object[] { from, gate });
		}

		private static void ComeOutOfGate_Callback(object state)
		{
			object[] states = (object[])state;
			Mobile from = states[0] as Mobile;
			Item gate = states[1] as Item;

			if ( from == null || from.Deleted || gate == null || gate.Deleted )
				return;

			Effects.PlaySound(from.Location, from.Map, 0x1fe);
			from.Direction = Direction.South;
			from.Hidden = !from.Hidden;
			Timer.DelayCall(TimeSpan.FromSeconds(0.5), new TimerStateCallback(DestroyGate_Callback), new object[] { from, gate });
		}

		private static void PlaceFakeGate_Callback(object m)
		{
			Mobile from = m as Mobile;
			if ( from == null || from.Deleted )
				return;

			Item gate = new Item();
			gate.ItemID = 3948;
			gate.Hue = 37;
			gate.Movable = false;
			gate.MoveToWorld(from.Location, from.Map);

			if (from.Hidden)
				Timer.DelayCall(TimeSpan.FromSeconds(0.5), new TimerStateCallback(ComeOutOfGate_Callback), new object[] { from, gate });
			else
				Timer.DelayCall(TimeSpan.FromSeconds(0.5), new TimerStateCallback(ChangeDirection_Callback), new object[] { from, gate });
		}

		private static void OpenGateAni_Callback(object m)
		{
			Mobile from = m as Mobile;
			if ( from == null || from.Deleted )
				return;

			Effects.SendLocationParticles(from, 0x1af3, 10, 30, 37, 0, 5052, 0);
			Effects.PlaySound(from.Location, from.Map, 0x209);
			Timer.DelayCall(TimeSpan.FromSeconds(1.4), new TimerStateCallback(PlaceFakeGate_Callback), from);
		}


		public static void Mink_OnHide( Mobile from )
		{
			if ( from == null || from.Deleted )
				return;

			Point3D loc = from.Location;

			if ( from.Hidden )
				Timer.DelayCall(TimeSpan.FromMilliseconds(0.0), new TimerStateCallback(OpenGateAni_Callback), from);
			else
			{
				from.Direction = Direction.South;
				from.Animate(17, 4, 1, true, false, 0);
				Timer.DelayCall(TimeSpan.FromMilliseconds(0.0), new TimerStateCallback(OpenGateAni_Callback), from);
			}
		}
	}
}