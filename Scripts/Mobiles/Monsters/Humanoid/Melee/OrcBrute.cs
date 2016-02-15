using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Misc;

namespace Server.Mobiles
{
	[CorpseName( "an orcish corpse" )]
	public class OrcBrute : BaseCreature
	{
        private const int MAX_ORC_LORDS = 10;

        private ArrayList m_orcLordList;

        [Constructable]
		public OrcBrute() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Body = 189;

			Name = "an orc brute";
			BaseSoundID = 0x45A;

			SetStr( 767, 945 );
			SetDex( 66, 75 );
			SetInt( 46, 70 );

			SetHits( 476, 552 );

			SetDamage( 20, 25 );

			SetSkill( SkillName.Macing, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 125.1, 140.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.Wrestling, 90.1, 100.0 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 50;

			PackGold( 700, 900 );
			PackItem( new ShadowIronOre( 25 ) );
			PackItem( new IronIngot( 10 ) );
			PackWeapon( 0, 5 );
			PackArmor( 1, 5 );

			if ( 0.05 > Utility.RandomDouble() )
				PackItem( new OrcishKinMask() );

			if ( 0.2 > Utility.RandomDouble() )
				PackItem( new BolaBall() );
		}

		public override bool BardImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override int Meat{ get{ return 2; } }

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.SavagesAndOrcs; }
		}

		public override bool IsEnemy( Mobile m )
		{
			Item helm = m.FindItemOnLayer( Layer.Helm );
			if ( m.Player && helm is OrcishKinMask || helm is OrcishKinRPMask )
				return false;

			return base.IsEnemy( m );
		}

		public override void AggressiveAction( Mobile aggressor, bool criminal )
		{
			base.AggressiveAction( aggressor, criminal );

			Item item = aggressor.FindItemOnLayer( Layer.Helm );

			if ( item is OrcishKinMask )
			{
				AOS.Damage( aggressor, 50, 0, 100, 0, 0, 0 );
				item.Delete();
				aggressor.FixedParticles( 0x36BD, 20, 10, 5044, EffectLayer.Head );
				aggressor.PlaySound( 0x307 );
			}
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool AutoDispel{ get{ return true; } }

		public override void OnDamagedBySpell( Mobile caster )
		{
			if ( caster == this )
				return;

			SpawnOrcLord( caster );
		}

        private int GetOrcLordCount()
        {
            if (m_orcLordList == null || m_orcLordList.Count == 0) return 0;
            //Defragment list
            for (int i = m_orcLordList.Count - 1; i >= 0; i--)
                if (m_orcLordList[i] == null || !(m_orcLordList[i] is OrcishLord) || ((OrcishLord)m_orcLordList[i]).Deleted)
                    m_orcLordList.RemoveAt(i);
            return m_orcLordList.Count;
        }

        private void AddOrcLord(OrcishLord orcLord)
        {
            if (m_orcLordList == null) m_orcLordList = new ArrayList();
            m_orcLordList.Add(orcLord);
        }

        public void SpawnOrcLord(Mobile target)
		{
			Map map = target.Map;

			if ( map == null )
				return;

            if (GetOrcLordCount() < MAX_ORC_LORDS)
			{
				BaseCreature orc = new SpawnedOrcishLord();
                AddOrcLord((OrcishLord)orc);

				orc.Team = this.Team;
				orc.Map = map;

				bool validLocation = false;

				for ( int j = 0; !validLocation && j < 10; ++j )
				{
					int x = target.X + Utility.Random( 3 ) - 1;
					int y = target.Y + Utility.Random( 3 ) - 1;
					int z = map.GetAverageZ( x, y );

					if ( validLocation = map.CanFit( x, y, this.Z, 16, false, false ) )
						orc.Location = new Point3D( x, y, Z );
					else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
						orc.Location = new Point3D( x, y, z );
				}

				if ( !validLocation )
					orc.Location = target.Location;

				orc.Combatant = target;
			}
		}

		public OrcBrute( Serial serial ) : base( serial )
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