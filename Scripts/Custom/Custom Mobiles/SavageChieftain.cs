using System;
using System.Collections;
using Server;
using Server.Misc;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
	[CorpseName( "a savage corpse" )]
	public class SavageChieftain : BaseCreature
	{
		[Constructable]
		public SavageChieftain() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "the savage chieftain";


			Body = 185;

			SetStr( 1026, 1145 );
			SetDex( 91, 110 );
			SetInt( 161, 185 );
			SetHits( 2056, 2235 );
			SetDamage( 40, 60 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 30, 40 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.EvalInt, 140.0, 210.0 );
			SetSkill( SkillName.Magery, 172.5, 205.0 );
			SetSkill( SkillName.Meditation, 77.5, 100.0 );
			SetSkill( SkillName.MagicResist, 120.5, 140.0 );
			SetSkill( SkillName.Tactics, 112.5, 135.0 );
			SetSkill( SkillName.Archery, 162.5, 185.0 );

			Fame = 100000;
			Karma = -100000;

			PackGold( 6000, 9000 );
			PackReg( 100, 150 );
			PackItem( new Bandage( Utility.RandomMinMax( 10, 150 ) ) );

			if ( 0.1 > Utility.RandomDouble() )
				PackItem( new TribalBerry() );

			AddItem( new BoneArms() );
			AddItem( new BoneLegs() );
			AddItem( new BoneGloves() );
			AddItem( new BoneChest() );
			//AddItem( new deermask() );
			AddItem( new ChieftainsBow() );
 		}

		public override int Meat{ get{ return 1; } }
		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool ShowFameTitle{ get{ return false; } }

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.SavagesAndOrcs; }
		}

		public override bool IsEnemy( Mobile m )
		{
			if ( m.BodyMod == 183 || m.BodyMod == 184 )
				return false;

			return base.IsEnemy( m );
		}

		public override void AggressiveAction( Mobile aggressor, bool criminal )
		{
			base.AggressiveAction( aggressor, criminal );

			if ( aggressor.BodyMod == 183 || aggressor.BodyMod == 184 )
			{
				AOS.Damage( aggressor, 50, 0, 100, 0, 0, 0 );
				aggressor.BodyMod = 0;
				aggressor.HueMod = -1;
				aggressor.FixedParticles( 0x36BD, 20, 10, 5044, EffectLayer.Head );
				aggressor.PlaySound( 0x307 );
				aggressor.SendLocalizedMessage( 1040008 ); // Your skin is scorched as the tribal paint burns away!

				if ( aggressor is PlayerMobile )
					((PlayerMobile)aggressor).SavagePaintExpiration = TimeSpan.Zero;
			}
		}

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is Dragon || to is WhiteWyrm || to is SwampDragon || to is Drake || to is Nightmare || to is Daemon )
				damage *= 5;
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.1 > Utility.RandomDouble() )
				BeginSavageDance();
		}

		public void BeginSavageDance()
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 8 ) )
			{
				if ( m != this && m is SavageChieftain )
					list.Add( m );
			}

			Animate( 111, 5, 1, true, false, 0 ); // Do a little dance...

			if ( AIObject != null )
				AIObject.NextMove = DateTime.Now + TimeSpan.FromSeconds( 1.0 );

			if ( list.Count >= 3 )
			{
				for ( int i = 0; i < list.Count; ++i )
				{
					SavageChieftain dancer = (SavageChieftain)list[i];

					dancer.Animate( 111, 5, 1, true, false, 0 ); // Get down tonight...

					if ( dancer.AIObject != null )
						dancer.AIObject.NextMove = DateTime.Now + TimeSpan.FromSeconds( 1.0 );
				}

				Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerCallback( EndSavageDance ) );
			}
		}

		public void EndSavageDance()
		{
			if ( Deleted )
				return;

			ArrayList list = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 8 ) )
				list.Add( m );

			if ( list.Count > 0 )
			{
				switch ( Utility.Random( 3 ) )
				{
					case 0: /* greater heal */
					{
						foreach ( Mobile m in list )
						{
							bool isFriendly = ( m is Savage || m is SavageRider || m is SavageChieftain || m is SavageRidgeback );

							if ( !isFriendly )
								continue;

							if ( m.Poisoned || MortalStrike.IsWounded( m ) || !CanBeBeneficial( m ) )
								continue;

							DoBeneficial( m );

							// Algorithm: (40% of magery) + (1-10)

							int toHeal = (int)(Skills[SkillName.Magery].Value * 0.4);
							toHeal += Utility.Random( 1, 10 );

							m.Heal( toHeal );

							m.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
							m.PlaySound( 0x202 );
						}

						break;
					}
					case 1: /* lightning */
					{
						foreach ( Mobile m in list )
						{
							bool isFriendly = ( m is Savage || m is SavageRider || m is SavageChieftain || m is SavageRidgeback );

							if ( isFriendly )
								continue;

							if ( !CanBeHarmful( m ) )
								continue;

							DoHarmful( m );

							double damage;

							if ( Core.AOS )
							{
								int baseDamage = 6 + (int)(Skills[SkillName.EvalInt].Value / 5.0);

								damage = Utility.RandomMinMax( baseDamage, baseDamage + 3 );
							}
							else
							{
								damage = Utility.Random( 12, 9 );
							}

							m.BoltEffect( 0 );

							SpellHelper.Damage( TimeSpan.FromSeconds( 0.25 ), m, this, damage, 0, 0, 0, 0, 100 );
						}

						break;
					}
					case 2: /* poison */
					{
						foreach ( Mobile m in list )
						{
							bool isFriendly = ( m is Savage || m is SavageRider || m is SavageChieftain || m is SavageRidgeback );

							if ( isFriendly )
								continue;

							if ( !CanBeHarmful( m ) )
								continue;

							DoHarmful( m );

							if ( m.Spell != null )
								m.Spell.OnCasterHurt();

							m.Paralyzed = false;

							double total = Skills[SkillName.Magery].Value + Skills[SkillName.Poisoning].Value;

							double dist = GetDistanceToSqrt( m );

							if ( dist >= 3.0 )
								total -= (dist - 3.0) * 10.0;

							int level;

							if ( total >= 200.0 && Utility.Random( 1, 100 ) <= 10 )
								level = 3;
							else if ( total > 170.0 )
								level = 2;
							else if ( total > 130.0 )
								level = 1;
							else
								level = 0;

							m.ApplyPoison( this, Poison.GetPoison( level ) );

							m.FixedParticles( 0x374A, 10, 15, 5021, EffectLayer.Waist );
							m.PlaySound( 0x474 );
						}

						break;
					}
				}
			}
		}

		public SavageChieftain( Serial serial ) : base( serial )
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