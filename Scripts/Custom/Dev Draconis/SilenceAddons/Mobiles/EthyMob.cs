using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class EthyMob : BaseCreature
	{

		public override bool DeleteCorpseOnDeath{ get{ return true; } }
        private DateTime m_ExpireTime;


		[Constructable]
		public EthyMob () : base( AIType.AI_Mage, FightMode.Agressor, 10, 1, 0.2, 0.4 )
		{
			Name = "an ethereal guardian";
			Body = ( Utility.RandomList( 1, 2, 3, 4, 7, 9, 10, 11, 12, 13, 14, 15, 16, 21, 22 ) );
			BaseSoundID = 1154;
			Hue = 22222;

			SetStr( 500 );
			SetDex( 300 );
			SetInt( 5000 );

			SetHits( 1000 );

			SetDamage( 30, 40 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Fire, 25 );
			SetDamageType( ResistanceType.Energy, 25 );

			SetResistance( ResistanceType.Physical, 65, 80 );
			SetResistance( ResistanceType.Fire, 60, 80 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Anatomy, 100.0 );
			SetSkill( SkillName.EvalInt, 150.0 );
			SetSkill( SkillName.Magery, 120.0 );
			SetSkill( SkillName.Meditation, 200.0 );
			SetSkill( SkillName.MagicResist, 200.0 );
			SetSkill( SkillName.Tactics, 150.0 );
			SetSkill( SkillName.Wrestling, 120.0 );

			Fame = 6000;
			Karma = -6000;

			VirtualArmor = 150;
            m_ExpireTime = DateTime.Now + TimeSpan.FromMinutes(30);
        }

        public override void OnThink()
        {
            bool expired = (DateTime.Now >= m_ExpireTime);
            if (expired)
                Delete();
            else
                base.OnThink();
        }

		public override bool OnBeforeDeath()
		{
			if ( !base.OnBeforeDeath() )
				return false;

			if (!NoKillAwards)
			{
				SoulCrystal crystal = new SoulCrystal();
				crystal.MoveToWorld(Location, Map);
			}

			Effects.SendLocationEffect( Location, Map, 0x376A, 10, 1 );
			return true;
		}


		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override bool BardImmune{ get{ return true; } }

		public EthyMob( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}