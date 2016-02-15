using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Spells;
using Server.Gumps;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a flaming corpse" )]
	public class BerserkLivingDead : BaseCreature
	{
		[Constructable]
		public BerserkLivingDead() : base( AIType.AI_Melee, FightMode.Closest, 20, 1, 0.3, 0.1 )
		{
			Name = "a berserk living dead";
			Body = 0x1F;
			BaseSoundID = 0x39D;
			Hue = 0x4AA;

			SetStr( 150, 200 );
			SetDex( 171, 190 );
			SetInt( 6, 10 );

			SetHits( 1050, 1500 );

			SetDamage( 10, 20 );

			SetSkill( SkillName.MagicResist, 51.1, 55.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Anatomy, 85.1, 90.0 );
			SetSkill( SkillName.Wrestling, 100.0 );

			Fame = 24000;
			Karma = -24000;

			VirtualArmor = 40;

			PackItem( new Bandage( Utility.RandomMinMax( 10, 20 ) ) );
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 30 ) < 1 )
			c.DropItem( new HalloweenCandle() );

			if ( Utility.Random( 10 ) < 1 )
			c.DropItem( new HalloweenStatue() );

			base.OnDeath( c );
		}


		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
				damage *= 5;
		}

		public override void OnDamagedBySpell( Mobile from )
		{
			this.Combatant = from;

			if (from.Combatant == null)
				return;

			Mobile m = from.Combatant;

			if (m.Combatant == null)
				return;

			if ( Alive )
			switch ( Utility.Random( 3 ) )
			{
				case 0: m.Hits +=( Utility.Random( 20, 40 ) ); break;
				case 1: m.Hits += ( Utility.Random( 30, 60 ) ); break;
				case 2: m.Hits +=( Utility.Random( 40, 70 ) ); break;
			}
			from.PlaySound( 0x1F2 );
			from.SendMessage("This creature is immune to magery, purely evil!");
			m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Lethal; } }
		public override bool CanRummageCorpses{ get{ return true; } }
     		public override bool Unprovokable { get { return true; } }
		public override bool Uncalmable{ get{ return true; } }


		public BerserkLivingDead( Serial serial ) : base( serial )
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