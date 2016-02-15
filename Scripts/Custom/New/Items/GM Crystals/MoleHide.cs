using System;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public struct MoleHideInfo
	{
		private Point3D m_Location;
		public Point3D Location{ get{ return m_Location; } set{ m_Location = value; } }
		private Map m_Map;
		public Map Map{ get{ return m_Map; } set{ m_Map = value; } }
		private Mobile m_User;
		public Mobile User{ get{ return m_User; } set{ m_User = value; } }
		private int m_Count;
		public int Count{ get{ return m_Count; } set{ m_Count = value; } }

		public MoleHideInfo( Point3D loc, Map map, Mobile user )
		{
			m_Location = loc;
			m_Map = map;
			m_User = user;
			m_Count = 9;
		}
	}

	public class MoleHide : BaseGMJewel
	{
		public override bool CastHide{ get{ return false; } }

		public override void HideEffects(Mobile from)
		{
			if (from.Hidden)
			{
				from.Z -= 10;
				from.Hidden = false;
				MoleHideInfo info = new MoleHideInfo(from.Location, from.Map, from);
				Timer.DelayCall( TimeSpan.FromMilliseconds( 100 ), new TimerStateCallback( DoIncZ_Callback ), info );
			}
			else
			{
				MoleHideInfo info = new MoleHideInfo(from.Location, from.Map, from);
				Timer.DelayCall( TimeSpan.FromMilliseconds( 100 ), new TimerStateCallback( DoDecZ_Callback ), info );
			}
			from.PlaySound( 0x244 );
		}

		private void DoIncZ_Callback( object molehideinfo )
		{
			MoleHideInfo info = (MoleHideInfo)molehideinfo;
			info.User.Z++;
			info.Count--;
			if (info.Count >= 0)
				Timer.DelayCall( TimeSpan.FromMilliseconds( 100 ), new TimerStateCallback( DoIncZ_Callback ), info );
			else
				info.User.EndAction( typeof( MoleHide ) );
		}

		private void DoDecZ_Callback( object molehideinfo )
		{
			MoleHideInfo info = (MoleHideInfo)molehideinfo;
			info.User.Z--;
			info.Count--;
			if (info.Count >= 0)
				Timer.DelayCall( TimeSpan.FromMilliseconds( 100 ), new TimerStateCallback( DoDecZ_Callback ), info );
			else
			{
				info.User.EndAction( typeof( MoleHide ) );
				info.User.Hidden = true;
				info.User.Z += 10;
			}
		}

		[Constructable]
		public MoleHide() : base(AccessLevel.GameMaster, 0xCB, 0x1ECD )
		{
			Name = "GM Mole Ball";
			Hue = 1717;
		}

		public MoleHide( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( !from.BeginAction( typeof(MoleHide) ) )
				return;
			base.OnDoubleClick( from );
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
	}
}