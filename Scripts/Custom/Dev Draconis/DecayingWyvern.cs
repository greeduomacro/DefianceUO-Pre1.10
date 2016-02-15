using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a wyvern corpse" )]
	public class DecayingWyvern : BaseCreature
	{
		private Mobile m_Owner;
		private DateTime m_ExpireTime;

		[Constructable]
		public DecayingWyvern ( Mobile owner ) : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a summoned wyvern";
			Body = 62;
			Hue = 1153;
			BaseSoundID = 362;

			SetStr( 202, 240 );
			SetDex( 153, 172 );
			SetInt( 51, 90 );

			SetHits( 125, 141 );

			SetDamage( 15, 25 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Poison, 50 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 90, 100 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.Poisoning, 60.1, 80.0 );
			SetSkill( SkillName.MagicResist, 100.1, 105.0 );
			SetSkill( SkillName.Tactics, 65.1, 90.0 );
			SetSkill( SkillName.Wrestling, 100.1, 105.0 );

			Fame = 2000;
			Karma = -2000;

			VirtualArmor = 80;

			m_Owner = owner;
			m_ExpireTime = DateTime.Now + TimeSpan.FromMinutes( Utility.RandomMinMax( 10, 15 ));

		}

		public override bool ReacquireOnMovement{ get{ return true; } }

		public override bool BardImmune { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override Poison HitPoison { get { return Poison.Lethal; } }

		public override void OnThink()
		{
			bool expired;

			expired = ( DateTime.Now >= m_ExpireTime );

			if ( !expired && m_Owner != null )
				expired = m_Owner.Deleted || Map != m_Owner.Map || !InRange( m_Owner, 30 );

			if ( expired )
			{
				PlaySound( GetIdleSound() );
				Delete();
			}
			else
			{
				base.OnThink();
			}
		}

		public override int GetAttackSound()
		{
			return 713;
		}

		public override int GetAngerSound()
		{
			return 718;
		}

		public override int GetDeathSound()
		{
			return 716;
		}

		public override int GetHurtSound()
		{
			return 721;
		}

		public override int GetIdleSound()
		{
			return 725;
		}

		public DecayingWyvern( Serial serial ) : base( serial )
		{
		}

		public override void AlterMeleeDamageTo(Mobile to, ref int damage)
		{
			if (to is BaseCreature)
			{
				BaseCreature bc = (BaseCreature)to;

				if (bc.Controlled || bc.Summoned || bc.BardTarget == this)
					damage *= 10;
			}
		}

		public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
		{
			if (from is BaseCreature)
			{
				BaseCreature bc = (BaseCreature)from;

				if (bc.Controlled || bc.Summoned || bc.BardTarget == this)
					damage /= 10;
			}
		}

		public override void CheckReflect(Mobile caster, ref bool reflect)
		{
			reflect = true;
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