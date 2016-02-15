using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Engines.IdolSystem;
using Server.Spells;
using Server.Spells.Seventh;
using Server.Spells.Fifth;

namespace Server.Mobiles
{
	public class HythBoss : BaseMiniBoss
	{
		static bool m_Active;
		IdolType m_Type;

		[CommandProperty( AccessLevel.GameMaster )]
		public static bool Active
		{
			get{ return m_Active; }
			set{ m_Active = value; }
		}

		[Constructable]
		public HythBoss () : base( AIType.AI_Mage )
		{
			Name = "Idol Keeper";
			Title = "of Hythloth";
			Body = 149;
			Hue = 1154;
			BaseSoundID = 0x4B0;
			m_Active = true;
			m_Type = IdolType.Hythloth;

			SetStr( 550, 620 );
			SetDex( 120, 160 );
			SetInt( 500, 600 );

			SetHits( 6000 );

			SetDamage( 28, 35 );

			SetDamageType( ResistanceType.Physical, 75 );
			SetDamageType( ResistanceType.Energy, 25 );

			SetResistance( ResistanceType.Physical, 80, 90 );
			SetResistance( ResistanceType.Fire, 70, 80 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 50, 60 );
			SetResistance( ResistanceType.Energy, 50, 60 );

			SetSkill( SkillName.EvalInt, 130.0 );
			SetSkill( SkillName.Magery, 120.0 );
			SetSkill( SkillName.Meditation, 110.0 );
			SetSkill( SkillName.MagicResist, 180.0 );
			SetSkill( SkillName.Tactics, 150.0 );
			SetSkill( SkillName.Wrestling, 110.0 );

			Fame = 24000;
			Karma = -24000;

			VirtualArmor = 80;
		}

		public override bool DoTeleport { get { return true; } }
		public override int DoPolymorphOnGaveMelee { get { return 3; } }
		public override int DoPolymorphHue { get { return 1175; } }
		public override int DoAreaDrainLife { get { return 25; } }

		public override void OnDeath( Container c )
		{
			m_Active = false;

			if ( Utility.Random( 5 ) < 1 )
				c.DropItem( new Idol( m_Type ) );

			if ( Utility.Random( 8 ) < 1 )
				c.DropItem( new RareCollector() );

			base.OnDeath( c );
		}

		public HythBoss( Serial serial ) : base( serial )
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