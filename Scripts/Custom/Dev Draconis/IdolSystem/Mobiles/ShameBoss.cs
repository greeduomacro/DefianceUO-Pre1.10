using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Engines.IdolSystem;

namespace Server.Mobiles
{
	public class ShameBoss : BaseMiniBoss
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
		public ShameBoss() : base( AIType.AI_Mage )
		{
			Name = "Idol Keeper";
			Title = "of Shame";
			Hue = 1423;
			Body = 14;
			BaseSoundID = 268;
			m_Active = true;
			m_Type = IdolType.Shame;
			m_Rare = MagicalRareType.Five;

			SetStr( 500, 550 );
			SetDex( 150, 200 );
			SetInt( 500, 600 );

			SetHits( 5500 );

			SetDamage( 25, 30 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 30, 35 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 10, 20 );
			SetResistance( ResistanceType.Poison, 15, 25 );
			SetResistance( ResistanceType.Energy, 15, 25 );

			SetSkill( SkillName.EvalInt, 135.0 );
			SetSkill( SkillName.Magery, 120.0 );
			SetSkill( SkillName.MagicResist, 185.0 );
			SetSkill( SkillName.Meditation, 110.0 );
			SetSkill( SkillName.Tactics, 200.0 );
			SetSkill( SkillName.Wrestling, 150.0 );
			SetSkill( SkillName.DetectHidden, 200.0 );

			Fame = 1500;
			Karma = -1500;

			VirtualArmor = 70;
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

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Lethal; } }
		public override double HitPoisonChance{ get{ return 0.80; } }
		public override bool DoEarthquake { get { return true; } }

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

		public ShameBoss( Serial serial ) : base( serial )
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
            m_Active = true;
        }
        public override void OnAfterDelete()
        {
            m_Active = false;
            base.OnAfterDelete();
        }
	}
}