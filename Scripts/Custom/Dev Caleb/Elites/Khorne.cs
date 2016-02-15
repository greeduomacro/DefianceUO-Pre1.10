using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Engines.CannedEvil;
using Server.Misc;

namespace Server.Mobiles
{
	public class Khorne : BaseElite
	{
		public bool cbd = true;

		[Constructable]
		public Khorne() : base( AIType.AI_Mage )
		{

			Name = "Khorne";
			BaseSoundID = 412;
			Body = 78;

			SetStr( 305, 425 );
			SetDex( 72, 150 );
			SetInt( 1000, 1500 );

			SetHits( 1500 );
			SetStam( 102, 300 );

			SetDamage( 45, 60 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Cold, 40 );
			SetDamageType( ResistanceType.Energy, 40 );

			SetResistance( ResistanceType.Physical, 55, 65 );
			SetResistance( ResistanceType.Fire, 25, 30 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 50, 60 );
			SetResistance( ResistanceType.Energy, 25, 30 );

			SetSkill( SkillName.EvalInt, 120.1, 130.0 );
			SetSkill( SkillName.Magery, 120.1, 130.0 );
			SetSkill( SkillName.Meditation, 100.1, 101.0 );
			SetSkill( SkillName.Poisoning, 100.1, 101.0 );
			SetSkill( SkillName.MagicResist, 175.2, 200.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.Wrestling, 75.1, 100.0 );

			Karma = -40000;

			PackGold(25000);

			VirtualArmor = 30;


			Bardiche weapon = new Bardiche();

			weapon.DamageLevel = (WeaponDamageLevel)Utility.Random( 3, 5 );
			weapon.DurabilityLevel = (WeaponDurabilityLevel)Utility.Random( 3, 5 );
			weapon.AccuracyLevel = (WeaponAccuracyLevel)Utility.Random( 3, 5 );


		}




		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool BardImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override int TreasureMapLevel{ get{ return 5; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override bool ClickTitle{ get{ return false; } }
        public override bool HasAntiTaming{ get{ return true; } }


		// still 12 4 1 false false -1
		// move 12 4 1 true false 0
		public override void OnDamage( int amount, Mobile from, bool willKill)
		{
			base.OnDamage(amount, from, willKill);
			if(Hits < 500 && !Frozen && Body != 785)
			{
				Frozen = true;
				//Combatant = null;

				if (AIObject != null)
					AIObject.NextMove = DateTime.Now + TimeSpan.FromSeconds(3.0);

				Direction = Direction.South;



				MagicDamageAbsorb = 500;
				MeleeDamageAbsorb = 500;
				Animate(12, 4, 1, true, false, 0);


				Yell("Ort Vas Ylem Rel");
				PlaySound(0x263);
				Animate(12, 4, 1, false, false, -1);

				new BoltTimer(this).Start();
				Timer.DelayCall(TimeSpan.FromSeconds(4.0), new TimerCallback(FinishMorph));

			}
		}

		public void FinishMorph()
		{
			PlaySound(0x20F);
			FixedParticles(0x376A, 1, 31, 9961, 1160, 0, EffectLayer.Waist);
			FixedParticles(0x37C4, 1, 31, 9502, 43, 2, EffectLayer.Waist);
			FightMode = FightMode.Closest;
			Frozen = false;
			Body = 785;
			Hits = 1500;
			BaseSoundID = 357;
			MagicDamageAbsorb = 0;
			MeleeDamageAbsorb = 0;


		}

		public class BoltTimer : Timer
		{
			private Khorne m_K;
			int m_TimesFired = 0;

			public BoltTimer(Khorne k) : base( TimeSpan.FromSeconds( 0.5 ), TimeSpan.FromSeconds( 0.5 ) )
			{
				this.m_K = k;
			}

			protected override void OnTick()
			{
				m_TimesFired++;
				m_K.BoltEffect(0);
				if (m_TimesFired >= 6)
					this.Stop();
			}
		}


		public Khorne( Serial serial ) : base( serial )
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