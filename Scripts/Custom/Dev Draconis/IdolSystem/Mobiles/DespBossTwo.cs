using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.EventPrizeSystem;
using Server.Engines.IdolSystem;

namespace Server.Mobiles
{
	[CorpseName( "an idol keeperess's corpse" )]
	public class DespBossTwo : BaseMiniBoss
	{
		private Mobile m_Owner;
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
		public DespBossTwo( Mobile owner ) : base( AIType.AI_Mage )
		{
			Title = "of Despise";
			Name = "Idol Keeperess";
			Hue = 2013;
			Body = 18;
			BaseSoundID = 367;
			m_Active = true;
			m_Half = WhichHalf.Right;
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

			m_Owner = owner;
		}

		public override int DoMoreDamageToPets { get { return 10; } }
		public override int DoLessDamageFromPets { get { return 10; } }
		public override bool DoEarthquake { get { return true; } }

		public override void OnDeath( Container c )
		{
			m_Active = false;

			if ( Utility.Random( 5 ) < 1 )
				c.DropItem( new Half( m_Half ) );

			if ( Utility.Random( 10 ) < 1 )
				c.DropItem( new MagicalRare( m_Rare ) );

			base.OnDeath( c );
		}

		public override bool CanRummageCorpses{ get{ return true; } }

		public override void OnThink()
		{
			Map map = this.Map;

			if ( m_Owner != null && m_Owner.Alive && !InRange( m_Owner, 12 ) )
			{
				Point3D from = this.Location;
                       		Point3D to = m_Owner.Location;

                        	this.Location = to;
	                        this.ProcessDelta();
        	                this.Say("Dont leave me my love!");

        	                Effects.SendLocationParticles(EffectItem.Create(from, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
                	        Effects.SendLocationParticles(EffectItem.Create(to, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);

	                        Effects.PlaySound(to, map, 0x1FE);
			}
			else
			{
			base.OnThink();
			}
		}

		public DespBossTwo( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_Active );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_Active = reader.ReadBool();
		}
	}
}