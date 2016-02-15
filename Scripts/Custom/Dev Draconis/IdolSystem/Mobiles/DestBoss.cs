using System;
using Server;
using Server.Items;
using Server.Engines.IdolSystem;

namespace Server.Mobiles
{
	public class DestBoss : BaseMiniBoss
	{
		static bool m_Active;
		IdolType m_Type;
		MagicalRareType m_Rare;

		[CommandProperty( AccessLevel.GameMaster )]
		public static bool Active
		{
			get{ return m_Active; }
			set{ m_Active = value; }
		}

		[Constructable]
		public DestBoss () : base( AIType.AI_Mage )
		{
			Name = "Idol Keeper";
			Title = "of Destard";
			Body = 104;
			Hue = 1109;
			BaseSoundID = 0x488;
			m_Active = true;
			m_Type = IdolType.Destard;
			m_Rare = MagicalRareType.Four;

			SetStr( 700, 800 );
			SetDex( 150, 200 );
			SetInt( 750, 850 );

			SetHits( 4500 );

			SetDamage( 29, 35 );

			SetDamageType( ResistanceType.Physical, 75 );
			SetDamageType( ResistanceType.Fire, 25 );

			SetResistance( ResistanceType.Physical, 75, 80 );
			SetResistance( ResistanceType.Fire, 40, 60 );
			SetResistance( ResistanceType.Cold, 40, 60 );
			SetResistance( ResistanceType.Poison, 70, 80 );
			SetResistance( ResistanceType.Energy, 40, 60 );

			SetSkill( SkillName.EvalInt, 125.0 );
			SetSkill( SkillName.Magery, 120.0 );
			SetSkill( SkillName.MagicResist, 200.0 );
			SetSkill( SkillName.Meditation, 110.0 );
			SetSkill( SkillName.Tactics, 160.0 );
			SetSkill( SkillName.Wrestling, 180.0 );

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 80;
		}

		public override void OnDeath( Container c )
		{
			m_Active = false;

			if ( Utility.Random( 5 ) < 1 )
				c.DropItem( new Idol( m_Type ) );

			if ( Utility.Random( 8 ) < 1 )
				c.DropItem( new MagicalRare( m_Rare ) );

			base.OnDeath( c );
		}

		public override bool HasBreath{ get{ return true; } }
		public override int BreathEffectHue{ get{ return 0x454; } }
		public override int BreathComputeDamage()
		{
			return (int)(this.Hits*0.02);
		}

		public override bool AutoDispel{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override int Meat{ get{ return 19; } }
		public override int Hides{ get{ return 20; } }
		public override HideType HideType{ get{ return HideType.Barbed; } }
		public override int DoMoreDamageToPets { get { return 10; } }
		public override int DoLessDamageFromPets { get { return 10; } }
		public override bool DoProvoPets { get { return true; } }
		public override bool DoTeleport { get { return true; } }

		public DestBoss ( Serial serial ) : base( serial )
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