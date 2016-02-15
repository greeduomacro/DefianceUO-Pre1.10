using System;
using Server.Misc;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	public class AuroraSkycaller : BaseCreature
	{
		[Constructable]
		public AuroraSkycaller():base( AIType.AI_Mage, FightMode.Weakest, 10, 1, 0.15, 0.2 )
		{
			Body = 0x191;
			Hue = 0x3F6;
			Name = "Aurora Skycaller";
			Kills = 10;
			ShortTermMurders = 10;

			SetStr( 356, 396 );
			SetDex( 45, 55 );
			SetInt( 830, 953 );

			SetHits( 3500 );
			SetMana( 9000 );
			SetDamage( 25, 30 );

			SetSkill( SkillName.Wrestling, 111.3, 117.8 );
			SetSkill( SkillName.Tactics, 110.5, 117.0 );
			SetSkill( SkillName.MagicResist, 140.6, 156.8);
			SetSkill( SkillName.Magery, 97.7, 99.6 );
			SetSkill( SkillName.EvalInt, 95.1, 98.1 );
			SetSkill( SkillName.Meditation, 61.1, 70.1 );

			Fame = 25000;
			Karma = -25000;

			VirtualArmor = 5;

			Item GoldBracelet = new GoldBracelet();
			GoldBracelet.Movable=false;
			GoldBracelet.Hue=1165;
		        EquipItem( GoldBracelet );

                        Item FloppyHat = new FloppyHat();
			FloppyHat.Movable=false;
			FloppyHat.Hue=1157;
			EquipItem( FloppyHat );

			Item PlainDress = new PlainDress();
			PlainDress.Movable=false;
			PlainDress.Hue=1157;
			EquipItem( PlainDress );

                        Item Sandals = new Sandals();
			Sandals.Movable=false;
			Sandals.Hue=1157;
			EquipItem( Sandals );

                        Item Necklace = new Necklace();
			Necklace.Movable=false;
			Necklace.Hue=1165;
			EquipItem( Necklace );

			Item GoldRing = new GoldRing();
			GoldRing.Movable=false;
			GoldRing.Hue=1165;
			EquipItem( GoldRing );

			switch( Utility.Random(10) )
			{
				case 0: PackItem( new DarkIronWire() ); break;
			}

			PackGold( 3200, 4000 );
			PackArmor( 0, 5 );
			PackWeapon( 0, 5 );
			PackArmor( 0, 5 );
			PackWeapon( 0, 5 );
			PackArmor( 0, 5 );
			PackWeapon( 0, 5 );
			PackArmor( 0, 5 );
			PackWeapon( 0, 5 );
			PackArmor( 0, 5 );
			PackWeapon( 0, 5 );
		}


		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 250 ) <  1 )
				c.DropItem( new TarotCard() );

            		base.OnDeath( c );
		}


		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool AutoDispel{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool BardImmune{ get{ return true;} }
		public override bool ShowFameTitle{ get{ return false; } }
		public override Poison PoisonImmune{ get{ return Poison.Greater; } }

		public override void CheckReflect( Mobile caster, ref bool reflect )
		{
			reflect = true; // Every spell is reflected back to the caster
		}

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
				damage *= 0;
		}

		public override void OnGaveMeleeAttack(Mobile defender)
		{
			double chanceofspecialmove = .30;
			double random = Utility.RandomDouble();
			if (chanceofspecialmove > random)
			{
			defender.SendLocalizedMessage( 1060085 ); // Your attacker strikes with lightning speed!

			defender.PlaySound( 40 );
			defender.BoltEffect( 0 );

			// Swing again:

			// If no combatant, wrong map, one of us is a ghost, or cannot see, or deleted, then stop combat
			if ( defender == null || defender.Deleted || this.Deleted || defender.Map != this.Map || !defender.Alive || !this.Alive || !this.CanSee( defender ) )
			{
				this.Combatant = null;
				return;
			}

			IWeapon weapon = this.Weapon;

			if ( weapon == null )
				return;

			if ( !this.InRange( defender, weapon.MaxRange ) )
				return;

			if ( this.InLOS( defender ) )
			{
				BaseWeapon.InDoubleStrike = true;
				this.RevealingAction();
				this.NextCombatTime = DateTime.Now + weapon.OnSwing( this, defender );
				BaseWeapon.InDoubleStrike = false;
			}


			}
		}

		public AuroraSkycaller( Serial serial ) : base( serial )
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
		}

	}
}