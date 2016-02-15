using System;
using Server;
using Server.Items;
using Server.Spells;
using Server.Spells.Seventh;
using Server.Spells.Fifth;
using Server.Engines.CannedEvil;

namespace Server.Mobiles
{
	[CorpseName( "an ancient lich corpse" )] // TODO: Corpse name?
	public class AncientLich : BaseCreature
	{
		[Constructable]
		public AncientLich() : base( AIType.AI_Mage, FightMode.Strongest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "ancient lich" );
			Body = 78;
			BaseSoundID = 412;

			SetStr( 416, 505 );
			SetDex( 96, 115 );
			SetInt( 966, 1045 );

			SetHits( 590, 615 );

			SetDamage( 15, 27 );

			SetSkill( SkillName.EvalInt, 120.1, 150.0 );
			SetSkill( SkillName.Magery, 120.1, 130.0 );
			SetSkill( SkillName.Meditation, 100.1, 101.0 );
			SetSkill( SkillName.Poisoning, 100.1, 101.0 );
			SetSkill( SkillName.MagicResist, 175.2, 200.0 );
			SetSkill( SkillName.Tactics, 95.1, 100.0 );
			SetSkill( SkillName.Wrestling, 95.1, 100.0 );

			Fame = 23000;
			Karma = -23000;

			VirtualArmor = 60;


                               switch ( Utility.Random( 500 ))
        		  {
           			case 0: PackItem( new CandelabraStand() ); break;

			  }

                        Katana weapon = new Katana();

			weapon.DamageLevel = (WeaponDamageLevel)Utility.Random( 0, 5 );
			weapon.DurabilityLevel = (WeaponDurabilityLevel)Utility.Random( 0, 5 );
			weapon.AccuracyLevel = (WeaponAccuracyLevel)Utility.Random( 0, 5 );

			PackItem( weapon );
                        PackJewel( 0.6 );

			PackGold( 1000, 1250 );
			PackReg( Utility.RandomMinMax( 15, 50 ) );
			PackItem( Loot.RandomArmorOrShieldOrWeapon() );

			switch ( Utility.Random( 2 ) )
			{
				case 0: PackArmor( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 2 ) )
			{
				case 0: PackArmor( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackArmor( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}

			switch ( Utility.Random( 10 ) )
			{
				case 0: PackArmor( 2, 5 ); break;
				case 1: PackArmor( 2, 5 ); break;
			}

			// TODO: Bone armor
		}

		public override bool Unprovokable{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override int TreasureMapLevel{ get{ return 5; } }

		public void Polymorph( Mobile m )
		{
			if ( !m.CanBeginAction( typeof( PolymorphSpell ) ) || !m.CanBeginAction( typeof( IncognitoSpell ) ) || m.IsBodyMod )
				return;

			IMount mount = m.Mount;

			if ( mount != null )
				mount.Rider = null;

			if ( m.Mounted )
				return;

			if ( m.BeginAction( typeof( PolymorphSpell ) ) )
			{
				Item disarm = m.FindItemOnLayer( Layer.OneHanded );

				if ( disarm != null && disarm.Movable )
					m.AddToBackpack( disarm );

				disarm = m.FindItemOnLayer( Layer.TwoHanded );

				if ( disarm != null && disarm.Movable )
					m.AddToBackpack( disarm );

				m.BodyMod = 50;
				m.HueMod = 0;

			}
		}

		public void SpawnUndeads( Mobile target )
		{
			Map map = this.Map;

			if ( map == null )
				return;

			int undeads = 0;

			foreach ( Mobile m in this.GetMobilesInRange( 10 ) )
			{
				if ( m is Skeleton || m is SkeletalMage || m is Zombie || m is BoneMagi || m is Ghoul || m is Wraith || m is Shade || m is Spectre )
					++undeads;
			}

			if ( undeads < 7 )
			{
				PlaySound( 586 );

				int newUndeads = Utility.RandomMinMax( 2, 5 );

				for ( int i = 0; i < newUndeads; ++i )
				{
					BaseCreature undead;

					switch ( Utility.Random( 16 ) )
					{
						default:
						case 0: case 1:	undead = new Skeleton(); break;
						case 2: case 3:	undead = new SkeletalMage(); break;
						case 4:	case 5:	undead = new BoneMagi(); break;
						case 6:	case 7:	undead = new Zombie(); break;
						case 8:	case 9:	undead = new Ghoul(); break;
						case 10: case 11: undead = new Wraith(); break;
						case 12: case 13: undead = new Shade(); break;
						case 14: case 15: undead = new Spectre(); break;
					}

					undead.Team = this.Team;

					bool validLocation = false;
					Point3D loc = this.Location;

					for ( int j = 0; !validLocation && j < 10; ++j )
					{
						int x = X + Utility.Random( 3 ) - 1;
						int y = Y + Utility.Random( 3 ) - 1;
						int z = map.GetAverageZ( x, y );

						if ( validLocation = map.CanFit( x, y, this.Z, 16, false, false ) )
							loc = new Point3D( x, y, Z );
						else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
							loc = new Point3D( x, y, z );
					}

					undead.MoveToWorld( loc, map );
					undead.Combatant = target;
				}
			}
		}

		public void DoSpecialAbility( Mobile target )
		{
			if ( 0.2 >= Utility.RandomDouble() ) // 20% chance to spawn more undead
				SpawnUndeads( target );

			if ( Hits < 400 && !IsBodyMod ) // The Ancient Lich is low on life, polymorphs into a skeleton.
				Polymorph( this );
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			DoSpecialAbility( attacker );
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			DoSpecialAbility( defender );
		}

		public AncientLich( Serial serial ) : base( serial )
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