using System;
using System.Collections;
using Server;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.Items
{
	public class Quad : Item
	{
		private SkillMod m_SkillMod0;
		private SkillMod m_SkillMod1;
		//private SkillMod m_SkillMod2;

		[Constructable]
		public Quad() : this( 0 )
		{
		}

		[Constructable]
		public Quad( int hue ) : base( 0x37F5 )
		{
			Name = "Quad Damage";
			Movable = false;
			Hue = 1265;
		}

		public void AddMods( Mobile from )
		{
			if ( m_SkillMod0 == null )
				m_SkillMod0 = new DefaultSkillMod( SkillName.EvalInt, true, 100 );

			if ( m_SkillMod1 == null )
				m_SkillMod1 = new DefaultSkillMod( SkillName.Tactics, true, 100 );

			from.AddSkillMod( m_SkillMod0 );
			from.AddSkillMod( m_SkillMod1 );
		}

		public Quad( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !from.InRange( GetWorldLocation(), 2 ) )
				from.SendLocalizedMessage( 500446 );
			else
			{
				from.SendMessage( 0x35, "Quad Damage Activated!");
				from.SolidHueOverride = 1150;
				//from.Emote( String.Format( "* {0} HAS QUAD DAMAGE *", from.Name ) );
				Consume();
				from.FixedEffect( 0x3728, 10, 15 );
				from.PlaySound( 1002 );
				AddMods( from );
				new CountdownTimer( from, m_SkillMod0, m_SkillMod1 ).Start();
			}
		}

		public class CountdownTimer: Timer
		{
			private int m_Ticker = 30;
			private Mobile m_Mobile;
			//private Quad m_Quad;
			private SkillMod m_SkillMod0;
			private SkillMod m_SkillMod1;
			//private SkillMod m_SkillMod2;

			public CountdownTimer( Mobile mobile, SkillMod skill0, SkillMod skill1 ): base( TimeSpan.Zero, TimeSpan.FromSeconds( 1.0 ) )
			{
				m_Mobile = mobile;
				m_SkillMod0 = skill0;
				m_SkillMod1 = skill1;
				Priority = TimerPriority.TwoFiftyMS;
			}

			private void RemoveMods()
			{
				if ( m_SkillMod0 != null )
					m_Mobile.RemoveSkillMod( m_SkillMod0 );

				if ( m_SkillMod1 != null )
					m_Mobile.RemoveSkillMod( m_SkillMod1 );

				m_SkillMod0 = null;
				m_SkillMod1 = null;

				m_Mobile.SendMessage(1276, "Quad Damage Has Worn Off!");
			}

			protected override void OnTick()
			{
				if ( m_Ticker <= 0 )
				{
					Stop();
					RemoveMods();
					m_Mobile.SolidHueOverride = -1;
				}
				else
				{
					switch ( m_Ticker )
					{
						case 90: case 60: case 30:
						case 20: case 10: case 5:
						case 4: case 3: case 2: case 1:
						m_Mobile.SendMessage( 1276, "{0} seconds remaining", m_Ticker ); break;
					}
					m_Ticker--;
				}
			}
		}
	}
}