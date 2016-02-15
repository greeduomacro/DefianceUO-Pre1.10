using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Engines.IdolSystem;

namespace Server.Mobiles
{
	public class CoveBoss : BaseMiniBoss
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
		public CoveBoss() : base( AIType.AI_Melee )
		{
			Name = "Idol Keeper";
			Title = "of Covetous";
			Hue = 1235;
			Body = 73;
			BaseSoundID = 402;
			m_Active = true;
			m_Type = IdolType.Covetous;
			m_Rare = MagicalRareType.One;

			SetStr( 780, 1000 );
			SetDex( 150, 200 );
			SetInt( 51, 75 );

			SetHits( 15000 );

			SetDamage( 40, 50 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 25, 30 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 10, 30 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.MagicResist, 80.0 );
			SetSkill( SkillName.Tactics, 250.0 );
			SetSkill( SkillName.Wrestling, 250.0 );
			SetSkill( SkillName.DetectHidden, 200.0 );

			Fame = 4500;
			Karma = -4500;

			VirtualArmor = 1000;
		}

        	public override void OnAfterDelete()
        	{
            		m_Active = false;
            		base.OnAfterDelete();
        	}

		public override void OnDeath( Container c )
		{
			m_Active = false;

			if ( Utility.Random( 5 ) < 1 )
				c.DropItem( new Idol( m_Type ) );

			if ( Utility.Random( 4 ) < 1 )
				c.DropItem( new MagicalRare( m_Rare ) );

			base.OnDeath( c );
		}

		public override int GetAttackSound()
		{
			return 916;
		}

		public override int GetAngerSound()
		{
			return 916;
		}

		public override int GetDeathSound()
		{
			return 917;
		}

		public override int GetHurtSound()
		{
			return 919;
		}

		public override int GetIdleSound()
		{
			return 918;
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override int Meat{ get{ return 4; } }
		public override MeatType MeatType{ get{ return MeatType.Bird; } }
		public override int Feathers{ get{ return 50; } }

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
					if ( trg.Hidden && this != trg && AccessLevel >= trg.AccessLevel )
					{
						double ss = srcSkill + Utility.Random( 21 ) - 10;
						double ts = trg.Skills[SkillName.Hiding].Value + Utility.Random( 21 ) - 10;

						if (ss >= ts)
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

		public CoveBoss( Serial serial ) : base( serial )
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
            		m_Active = true; //Since a CoveBoss is deserialized it does exist...
		}
	}
}