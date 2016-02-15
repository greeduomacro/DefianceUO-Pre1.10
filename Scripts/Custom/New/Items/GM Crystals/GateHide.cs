using System;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public class GateHide : BaseGMJewel
	{
		public override bool CastArea{ get{ return true; } }

		public override void HideEffects(Mobile from)
		{
			Entity entity = new Entity( from.Serial, from.Location, from.Map );

			Effects.SendLocationParticles( entity, m_RedGate ? 0x1AE5 : 0x1AF3, 8, 26, m_GateHue > 0 ? (m_GateHue-1) : 0, 0, 0, 0 );
			Effects.PlaySound( entity.Location, entity.Map, m_GateSound );
			Timer.DelayCall(TimeSpan.FromSeconds( 1.25 ), new TimerStateCallback(Anim_Continue), new object[]{ from, entity } );
		}

		public void Anim_Continue( object o )
		{
			object[] objs = (object[])o;
			Mobile from = objs[0] as Mobile;
			Entity entity = objs[1] as Entity;

			Moongate gate = new Moongate( false );
			if ( m_RedGate )
				gate.ItemID = 0xDDA;
			gate.Hue = m_GateHue > 0 ? m_GateHue : 0;
			gate.MoveToWorld( entity.Location, entity.Map );
			gate.TargetMap = Map.Internal;

			Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ) , new TimerStateCallback( ChangeHide ), from );
			Timer.DelayCall(TimeSpan.FromSeconds( 3.0 ), new TimerStateCallback( KillGate ), gate );
		}

		public void ChangeHide( object o )
		{
			Mobile from = o as Mobile;
			if ( from != null && !from.Deleted )
				from.Hidden = !from.Hidden;
		}

		public void KillGate( object o )
		{
			Moongate gate = o as Moongate;
			if ( gate != null && !gate.Deleted )
			{
				Effects.SendLocationParticles( EffectItem.Create( gate.Location, gate.Map, EffectItem.DefaultDuration ), 0x376A, 9, 20, 5042 );
				Effects.PlaySound( gate.Location, gate.Map, 0x201 );
				gate.Delete();
			}
		}

		private int m_GateHue;
		private int m_GateSound;
		private bool m_RedGate;

		[CommandProperty( AccessLevel.GameMaster )]
		public int GateHue{ get{ return m_GateHue; } set{ m_GateHue = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int GateSound{ get{ return m_GateSound; } set{ m_GateSound = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool RedGate{ get{ return m_RedGate; } set{ m_RedGate = value; } }

		[Constructable]
		public GateHide() : base( AccessLevel.GameMaster, 1154, 0x1ECD )
		{
			Name = "GM Gate Ball";
			m_GateSound = 496;
			m_GateHue = 0;
		}
		public GateHide( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 2 ); // version

			writer.Write( m_RedGate );

			writer.Write( m_GateHue );
			writer.Write( m_GateSound );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();

			switch ( version )
			{
				case 2:
				{
					m_RedGate = reader.ReadBool();
					m_GateHue = reader.ReadInt();
					m_GateSound = reader.ReadInt();
					break;
				}
				case 1:
				{
					/*m_FlameSound =*/ reader.ReadInt();
					/*m_FlameHue =*/ reader.ReadInt();
					m_GateHue = reader.ReadInt();
					m_GateSound = reader.ReadInt();
					break;
				}
				case 0:
				{
					//m_FlameSound = 0x225;
					//m_FlameHue = 0;
					m_RedGate = true;
					m_GateSound = 496;
					m_GateHue = Hue-1;
					break;
				}
			}
		}
	}
}