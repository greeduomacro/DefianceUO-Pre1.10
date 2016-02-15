using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a rabid polar bears corpse" )]
	public class RabidBear : BaseCreature
	{
		[Constructable]
		public RabidBear() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.6, 1.2 )
		{
			Name = "a rabid polar bear";
			Body = 213;
			Hue = 2066;
			MagicDamageAbsorb = 150;

			SetStr( 801, 900 );
			SetDex( 80, 100 );
			SetInt( 36, 50 );

			SetHits( 1000 );

			SetDamage( 50, 60 );

			SetSkill( SkillName.MagicResist, 90.1, 95.0 );
			SetSkill( SkillName.Tactics, 70.1, 85.0 );
			SetSkill( SkillName.Wrestling, 65.1, 80.0 );

			Fame = 10000;
			Karma = -10000;

			PackReg( 30 );
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.75 >= Utility.RandomDouble() )
				DrainLife();
		}

		public void DrainLife()
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 8 ) )
			{
				if ( m == this || !CanBeHarmful( m ) )
					continue;

				if ( m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != this.Team) )
					list.Add( m );
				else if ( m.Player )
					list.Add( m );
			}

			foreach ( Mobile m in list )
			{
				DoHarmful( m );

				m.FixedParticles( 0x374A, 10, 15, 5013, 2066, 0, EffectLayer.Waist );
				m.PlaySound( 0x231 );

				m.SendMessage( "You feel the life drain out of you!" );

				int toDrain = Utility.RandomMinMax( 20, 40 );

				Hits += toDrain;
				m.Damage( toDrain, this );
			}
		}

		//Melee damage to controlled mobiles is multiplied by 2
		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)to;

				if ( bc.Controlled )
				damage *= 2;
			}
		}

		//Melee damage from controlled mobiles is divided by 30
		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Controlled )
				damage /= 30;
			}
		}

		//Spell damage from controlled mobiles is scaled down by 0.01 - from players is scaled up by 1.25
		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			if ( caster is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)caster;

				if ( bc.Controlled )
				scalar = 0.01;
			}

			if ( caster is PlayerMobile )
			{
			 scalar *= 1.20;
			}
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich );
		}

        public override void OnDeath(Container c)
        {
            if (Utility.Random(100) < 1) c.AddItem(new IcePile());
            base.OnDeath(c);
        }

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool AutoDispel{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override int TreasureMapLevel{ get{ return 5; } }
		public override bool CanRummageCorpses{ get{ return true; } }

		public RabidBear( Serial serial ) : base( serial )
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