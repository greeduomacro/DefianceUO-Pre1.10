using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Spells;
using Server.Gumps;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a poisonous corpse" )]
	public class Venomspitter : BaseCreature
	{
		[Constructable]
		public Venomspitter() : base( AIType.AI_Melee, FightMode.Closest, 20, 1, 0.08, 0.1 )
		{
			Name = "a deviate venomspitter";
			Body = 249;
			BaseSoundID = 1260;
			Kills = 5;

			SetStr( 425, 450 );
			SetDex( 171, 190 );
			SetInt( 6, 10 );

			SetHits( 1050, 1150 );

			SetDamage( 33, 35 );

			SetSkill( SkillName.MagicResist, 51.1, 55.0 );
			SetSkill( SkillName.Tactics, 135.1, 150.0 );
			SetSkill( SkillName.Anatomy, 85.1, 90.0 );
			SetSkill( SkillName.Wrestling, 135.1, 150.0 );

			Fame = 24000;
			Karma = -24000;

			VirtualArmor = 30;

			switch ( Utility.Random( 8 ) )
			{
				case 0: PackItem( new GreaterAgilityPotion() ); break;
				case 1: PackItem( new GreaterExplosionPotion() ); break;
				case 2: PackItem( new GreaterCurePotion() ); break;
				case 3: PackItem( new GreaterHealPotion() ); break;
				case 4: PackItem( new NightSightPotion() ); break;
				case 5: PackItem( new GreaterPoisonPotion() ); break;
				case 6: PackItem( new TotalRefreshPotion() ); break;
				case 7: PackItem( new GreaterStrengthPotion() ); break;
			}

			PackItem( new Nightshade( Utility.Random( 12, 18 ) ) );
			PackItem( new Bandage( Utility.RandomMinMax( 25, 40 ) ) );
			PackGold( 3400, 4650 );

			switch ( Utility.Random( 2 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 6 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 7 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}

			switch ( Utility.Random( 8 ) )
			{
				case 0: PackWeapon( 2, 5 ); break;
				case 1: PackArmor( 2, 5 ); break;
			}

			switch ( Utility.Random( 10 ) )
			{
				case 0: PackWeapon( 3, 5 ); break;
				case 1: PackArmor( 3, 5 ); break;
			}

		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 300 ) == 0 )
				c.AddItem( new MinotaurSmallAxe() );
			base.OnDeath( c );
		}

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
				damage *= 3;
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
			from.SendMessage("Magic seems to heal this creature!!!");
			m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Lesser; } }
		public override bool CanRummageCorpses{ get{ return true; } }
     		public override bool Unprovokable { get { return true; } }
		public override bool Uncalmable{ get{ return true; } }
		public override int Meat{ get{ return 50; } }

		public override void OnGaveMeleeAttack( Mobile defender )
			{
			base.OnGaveMeleeAttack( defender );

			if ( 0.2 >= Utility.RandomDouble() )
				ApplyPoison();
			}

		public void ApplyPoison()
		{
			Map map = this.Map;

			if ( map == null )
				return;

			ArrayList targets = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 20 ) )
			{
				if ( m == this || !CanBeHarmful( m ) )
					continue;

				if ( m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != this.Team) )
					targets.Add( m );
				else if ( m.Player )
					targets.Add( m );

				PlaySound( 0x231 );
				Effects.SendLocationEffect( m, m.Map, 0x113A, 20, 10 );
				m.ApplyPoison( m, Poison.Lethal );
				LocalOverheadMessage( MessageType.Regular, 0x44, 1010523 ); // A toxic vapor envelops thee.
			}
		}

		public Venomspitter( Serial serial ) : base( serial )
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