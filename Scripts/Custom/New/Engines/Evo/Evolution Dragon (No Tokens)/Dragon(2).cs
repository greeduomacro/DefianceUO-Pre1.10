    //////////////////////////////////
   //			           //
  //      Scripted by Raelis      //
 //		             	 //
//////////////////////////////////
using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a dragon corpse" )]
	public class Dragon2 : BaseCreature
	{
		[Constructable]
		public Dragon2 () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a dragon";
			Body = 12;
			Hue = 16385;
			BaseSoundID = 362;

			SetStr( 796, 785 );
			SetDex( 86, 155 );
			SetInt( 586, 606 );

			SetHits( 700, 780 );

			SetDamage( 24, 30 );

			ControlSlots = 3;
			MinTameSkill = 98.9;

			SetDamageType( ResistanceType.Physical, 100 );
			SetDamageType( ResistanceType.Fire, 50 );
			SetDamageType( ResistanceType.Cold, 50 );
			SetDamageType( ResistanceType.Poison, 50 );
			SetDamageType( ResistanceType.Energy, 50 );

			SetResistance( ResistanceType.Physical, 80 );
			SetResistance( ResistanceType.Fire, 80 );
			SetResistance( ResistanceType.Cold, 80 );
			SetResistance( ResistanceType.Poison, 80 );
			SetResistance( ResistanceType.Energy, 80 );

			SetSkill( SkillName.Magery, 90.1, 100.0 );
			SetSkill( SkillName.Meditation, 90.1, 100.0 );
			SetSkill( SkillName.EvalInt, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 95.1, 100.0 );
			SetSkill( SkillName.Tactics, 69.3, 84.0 );
			SetSkill( SkillName.Wrestling, 69.3, 84.0 );
			SetSkill( SkillName.Anatomy, 69.3, 84.0 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 80;

			PackGold( 2300, 2600 );
			PackMagicItems( 4, 5, 0.95, 0.95 );
			PackMagicItems( 4, 5, 0.80, 0.65 );
			PackMagicItems( 4, 5, 0.80, 0.65 );

			if ( Utility.RandomDouble() <= 0.55 )
			{
				int amount = Utility.RandomMinMax( 1, 5 );

				PackItem( new DragonDust(amount) );
			}
		}

		public override bool AutoDispel{ get{ return true; } }
		public override int BreathRange{ get{ return RangePerception / 2; } }

		public Dragon2( Serial serial ) : base( serial )
		{
		}

		public override bool CanBeControlledBy( Mobile m )
 		{
 		if ( m.Skills[SkillName.AnimalTaming].Base < 98 )
 		return false;
 		return base.CanBeControlledBy( m );
 		}

		private DateTime m_NextBreathe;

		public override void OnActionCombat()
		{
			Mobile combatant = Combatant;

			if ( combatant == null || combatant.Deleted || combatant.Map != Map || !InRange( combatant, 12 ) || !CanBeHarmful( combatant ) || !InLOS( combatant ) )
				return;

			if ( DateTime.Now >= m_NextBreathe )
			{
				Breathe( combatant );

				m_NextBreathe = DateTime.Now + TimeSpan.FromSeconds( 12.0 + (3.0 * Utility.RandomDouble()) ); // 12-15 seconds
			}
		}

		public void Breathe( Mobile m )
		{
			DoHarmful( m );

			new BreatheTimer( m, this ).Start();

			this.Frozen = true;

			this.MovingParticles( m, 0x1FBF, 1, 0, false, true, ( this.Hue - 1 ), 0, 9502, 6014, 0x11D, EffectLayer.Waist, 0 );
		}

		private class BreatheTimer : Timer
		{
			private Dragon2 d;
			private Mobile m_Mobile;

			public BreatheTimer( Mobile m, Dragon2 owner ) : base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
			{
				d = owner;
				m_Mobile = m;
			}

			protected override void OnTick()
			{
				int damagemin = d.Hits / 20;
				int damagemax = d.Hits / 25;
				d.Frozen = false;

				m_Mobile.PlaySound( 0x11D );
				AOS.Damage( m_Mobile, Utility.RandomMinMax( damagemin, damagemax ), 0, 100, 0, 0, 0 );
				Stop();
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}