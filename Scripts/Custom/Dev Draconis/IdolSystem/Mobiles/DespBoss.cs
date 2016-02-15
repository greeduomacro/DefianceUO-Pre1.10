using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Engines.IdolSystem;
using Server.EventPrizeSystem;
using Server.Mobiles;

namespace Server.Mobiles
{
	public class DespBoss : BaseMiniBoss
	{
		private Timer m_Timer;
		static bool m_Active;
		WhichHalf m_Half;
		MagicalRareType m_Rare;

		[CommandProperty( AccessLevel.GameMaster )]
		public static bool Active
		{
			get{ return m_Active; }
			set{ m_Active = value; }
		}

		[Constructable]
		public DespBoss() : base( AIType.AI_Mage )
		{
			Name = "Idol Keeper";
			Title = "of Despise";
			Hue = 2010;
			Body = 18;
			BaseSoundID = 367;
			m_Active = true;
			m_Half = WhichHalf.Left;
			m_Rare = MagicalRareType.Three;

			SetStr( 200, 250 );
			SetDex( 180, 200 );
			SetInt( 250, 300 );

			SetHits( 5500 );

			SetDamage( 18, 22 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 35, 40 );
			SetResistance( ResistanceType.Fire, 15, 25 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 15, 25 );
			SetResistance( ResistanceType.Energy, 15, 25 );

			SetSkill( SkillName.EvalInt, 110.0 );
			SetSkill( SkillName.Magery, 110.0 );
			SetSkill( SkillName.Meditation, 110.0 );
			SetSkill( SkillName.MagicResist, 220.0 );
			SetSkill( SkillName.Tactics, 60.0 );
			SetSkill( SkillName.Wrestling, 75.0 );
			SetSkill( SkillName.DetectHidden, 200.0 );

			Fame = 10000;
			Karma = -10000;

			VirtualArmor = 1;

			m_Timer = new PartnerTimer( this );
			m_Timer.Start();
		}

		public override int DoMoreDamageToPets { get { return 10; } }
		public override int DoLessDamageFromPets { get { return 10; } }
		public override bool DoEarthquake { get { return true; } }

		public override void OnDeath( Container c )
		{
			m_Active = false;

			if ( Utility.Random( 4 ) < 1 )
				c.DropItem( new Half( m_Half ) );

			if ( Utility.Random( 8 ) < 1 )
				c.DropItem( new MagicalRare( m_Rare ) );

			base.OnDeath( c );
		}

		public override bool CanRummageCorpses{ get{ return true; } }

		private class PartnerTimer : Timer
		{
			private Mobile m_Owner;

			public PartnerTimer( Mobile owner ) : base( TimeSpan.FromSeconds( 5.0 ))
			{
				m_Owner = owner;
			}

			protected override void OnTick()
			{
				if ( m_Owner.Deleted )
				{
					Stop();
					return;
				}

			Map map = m_Owner.Map;

			if ( map == null )
				return;

			DespBossTwo wife = new DespBossTwo( m_Owner );
                    	wife.MoveToWorld( m_Owner.Location, m_Owner.Map );
			}
		}



		public override void OnThink()
		{
			Point3D p = Location;

			double srcSkill = Skills[SkillName.DetectHidden].Value;
			int range = (int)(srcSkill / 10.0);

			if ( !CheckSkill( SkillName.DetectHidden, 0.0, 100.0 ) )
				range /= 2;

			if ( range > 0 )
			{
				IPooledEnumerable inRange = Map.GetMobilesInRange( p, range );

				foreach ( Mobile trg in inRange )
				{
					if ( trg.Hidden && this != trg )
					{
						double ss = srcSkill + Utility.Random( 21 ) - 10;
						double ts = trg.Skills[SkillName.Hiding].Value + Utility.Random( 21 ) - 10;

						if ( AccessLevel >= trg.AccessLevel && ( ss >= ts ) )
						{
							trg.RevealingAction();
							trg.SendLocalizedMessage( 500814 ); // You have been revealed!
						}
					}
				}
				inRange.Free();
			}
			base.OnThink();
		}

		public DespBoss( Serial serial ) : base( serial )
		{
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

            m_Active = true;
		}
        public override void OnAfterDelete()
        {
            m_Active = false;
            base.OnAfterDelete();
        }
	}
}