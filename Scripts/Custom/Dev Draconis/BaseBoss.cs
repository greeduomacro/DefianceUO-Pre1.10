using System;
using System.Collections;
using Server;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Seventh;
using Server.Spells.Fifth;
using Server.Targeting;
using Server.EventPrizeSystem;

namespace Server.Mobiles
{
	//Note: BaseBoss is not constructable, i.e. it can't be added ingame.
	public class BaseBoss : BaseCreature
	{
		public BaseBoss(): this( AIType.AI_Mage )
		{
            		setup();
		}

		public BaseBoss(AIType aiType)	: this(aiType, FightMode.Closest)
		{
            		setup();
        	}

		public BaseBoss(AIType aiType, FightMode mode) : base(aiType, mode, 18, 1, 0.1, 0.2)
		{
            		setup();
        	}

		public BaseBoss(Serial serial) : base(serial)
		{
		}

        	private void setup()
        	{
            		Body = 999;
            		Name = "BaseBoss";

            		SetStr(100, 100);
            		SetDex(100, 100);
            		SetInt(100, 100);

            		SetHits(100);
            		SetStam(100, 100);

            		SetDamage(40, 50);

            		SetSkill(SkillName.Anatomy, 100.0);
            		SetSkill(SkillName.MagicResist, 100.0);
            		SetSkill(SkillName.Tactics, 100.0);
            		SetSkill(SkillName.DetectHidden, 200.0);

            		Fame = 1000;
            		Karma = -1000;

            		VirtualArmor = 130;

            		Backpack pack = new Backpack();
            		AddItem(pack);
        	}


		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}

		//Here's an important part. We define a set of properties
		//with standard values of false/0/1. Those are used in the methods
		//such as OnThink() etc. to determine if special abilities should be used.
		//The keyword virtual states that all classes inherited from BaseBoss
		//can override them.
        	
		public virtual int CanBandageSelf { get { return 0; } }
        	public virtual int DoAreaDrainLife { get { return 0; } }
		public virtual bool DoTeleport { get { return false; } }
		public virtual int DoMoreDamageToPets { get { return 1; } }
		public virtual int DoLessDamageFromPets { get { return 1; } }
		public virtual bool DoAlwaysReflect { get { return false; } }
		public virtual int DoAreaDrainMana { get { return 0; } }
		public virtual int DoAreaDrainStam { get { return 0; } }
		public virtual bool DoProvoPets { get { return false; } }
		public virtual int DoLessMagicDamageFromPets { get { return 1; } }
		public virtual int DoPolymorphOnGaveMelee { get { return 0; } }
		public virtual int DoPolymorphHue { get { return 0; } }
		public virtual bool DoDistributeTokens { get { return false; } }
		public virtual bool DoSpawnMobile { get { return false; } }
		public virtual bool DoLeechLife { get { return false; } }
		public virtual bool DoEarthquake { get { return false; } }
		public virtual bool DoDetectHidden { get { return false; } }
		public virtual bool DoDistributeLoot { get { return false; } }
		public virtual bool DoSpawnGoldOnDeath { get { return false; } }
		public virtual bool DoImmuneToPets { get { return false; } }
		public virtual bool DoImmuneToPlayers { get { return false; } }
		public virtual bool DoHealMobiles { get { return false; } }
		public virtual int CanCastReflect{ get { return 0; } }
		public virtual int DoWeaponsDoMoreDamage{ get { return 0; } }
		public virtual int CanCheckReflect{ get { return 0; } }
		public virtual bool DoSpawnEvil{ get { return false; } }
		public virtual bool DoSkillLoss{ get { return false; } }
		public virtual bool DoDisarmPlayer{ get { return false; } }
		public virtual int DoStunPunch{ get { return 0; } }
		public virtual bool ThrowAtomicBomb{ get { return false; } }

		public override void OnGaveMeleeAttack(Mobile defender)
		{
			base.OnGaveMeleeAttack(defender);

			if (DoPolymorphOnGaveMelee > 0 && 0.80 >= Utility.RandomDouble())
				Polymorph(defender);

			if ( DoSpawnEvil && 0.2 >= Utility.RandomDouble() )
                		SpawnEvil( defender );

			if ( DoEarthquake && 0.1 >= Utility.RandomDouble() )
				Earthquake();

			if ( DoSkillLoss && 0.1 >= Utility.RandomDouble())
				ApplySkillLoss( defender );

            		if (DoAreaDrainLife > 0 && 0.25 >= Utility.RandomDouble())
				DrainLife();

            		if (DoAreaDrainMana > 0 && 0.25 >= Utility.RandomDouble())
				DrainMana();

            		if (DoAreaDrainStam > 0 && 0.25 >= Utility.RandomDouble())
				DrainStam();

			if (DoProvoPets && 0.33 >= Utility.RandomDouble() && defender is BaseCreature)
			{
				BaseCreature c = (BaseCreature)defender;

				if (c.Controlled && c.ControlMaster != null)
				{
					c.ControlTarget = c.ControlMaster;
					c.ControlOrder = OrderType.Attack;
					c.Combatant = c.ControlMaster;
				}
			}

			if (DoSpawnMobile && 0.05 >= Utility.RandomDouble())
			{
				BaseCreature spawn = new DecayingSpawn(this);

				spawn.BodyValue = this.BodyValue;
				spawn.Hue = this.Hue;
				spawn.BaseSoundID = this.BaseSoundID;
				spawn.Team = this.Team;
				spawn.MoveToWorld(defender.Location, defender.Map);
				spawn.Combatant = defender;
				spawn.Say("I am here my master!");
			}

            		if (DoDisarmPlayer && 0.25 >= Utility.RandomDouble() && defender is PlayerMobile)
			{
				Item toDisarm = defender.FindItemOnLayer( Layer.OneHanded );

				if ( toDisarm == null || !toDisarm.Movable )
				    toDisarm = defender.FindItemOnLayer( Layer.TwoHanded );

				Container pack = defender.Backpack;

                		if (toDisarm != null && toDisarm.Movable && defender.Backpack != null)
                		{
                    			pack.DropItem(toDisarm);
                    			defender.SendMessage("The intensity of the attack disarms you!");
                		}
			}

            			if (DoStunPunch > 0 && 0.25 >= Utility.RandomDouble())
				{
					defender.Freeze( TimeSpan.FromSeconds( DoStunPunch ) );
					defender.SendMessage( "You have been stunned!" );
				}
		}

		public override void OnGotMeleeAttack(Mobile attacker)
		{
			base.OnGotMeleeAttack(attacker);

			if ( DoSpawnEvil && 0.2 >= Utility.RandomDouble() )
                		SpawnEvil( attacker );

            		if (DoAreaDrainLife > 0 && 0.25 >= Utility.RandomDouble())
				DrainLife();

            		if (DoAreaDrainMana > 0 && 0.25 >= Utility.RandomDouble())
				DrainMana();

            		if (DoAreaDrainStam > 0 && 0.25 >= Utility.RandomDouble())
				DrainStam();

			if (DoProvoPets && 0.33 >= Utility.RandomDouble() && attacker is BaseCreature)
			{
				BaseCreature c = (BaseCreature)attacker;

				if (c.Controlled && c.ControlMaster != null)
				{
					c.ControlTarget = c.ControlMaster;
					c.ControlOrder = OrderType.Attack;
					c.Combatant = c.ControlMaster;
				}
			}

			if (DoEarthquake && 0.1 >= Utility.RandomDouble() )
				Earthquake();

			if (DoSpawnMobile && 0.1 >= Utility.RandomDouble())
			{
				BaseCreature spawn = new DecayingSpawn(this);

				spawn.BodyValue = this.BodyValue;
				spawn.Hue = this.Hue;
				spawn.BaseSoundID = this.BaseSoundID;
				spawn.Team = this.Team;
				spawn.MoveToWorld(attacker.Location, attacker.Map);
				spawn.Combatant = attacker;
				spawn.Say("I am here my master!");
			}
		}

		public override void OnDamagedBySpell(Mobile caster)
		{
			base.OnDamagedBySpell(caster);

			if (DoTeleport && 0.25 >= Utility.RandomDouble())
				Teleport(caster);

			if (DoSpawnMobile && 0.1 >= Utility.RandomDouble())
			{
				BaseCreature spawn = new DecayingSpawn(this);

				spawn.BodyValue = this.BodyValue;
				spawn.Hue = this.Hue;
				spawn.BaseSoundID = this.BaseSoundID;
				spawn.Team = this.Team;
				spawn.MoveToWorld(caster.Location, caster.Map);
				spawn.Combatant = caster;
				spawn.Say("I am here my master!");
			}

            		if (CanCastReflect > 0 && 0.05 >= Utility.RandomDouble())
			{
				this.Say("In Jux Sanct");
				this.FixedParticles( 0x375A, 10, 15, 5037, EffectLayer.Waist );
				this.PlaySound( 0x1E9 );
				this.MagicDamageAbsorb = CanCastReflect;
			}

			if ( ThrowAtomicBomb && 0.1 >=Utility.RandomDouble() )
			{
				Effects.SendMovingEffect( this, caster, 0xF0D & 0x3FFF, 7, 0, false, false, 1161, 0 );
				Effects.SendMovingEffect( this, caster, 0xF0D & 0x3FFF, 7, 0, false, false, 349, 0 );
				Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( Bomb ), caster );
			}
		}

		public void Bomb( object state )
		{
			Mobile caster = (Mobile)state;

			Atomic bomb = new Atomic();
			bomb.MoveToWorld( caster.Location, caster.Map );
		}

		public override void Damage( int amount, Mobile from )
		{
            		if (DoImmuneToPets && from is BaseCreature)
				amount = (int)(0);

			if (DoImmuneToPlayers &&  from is PlayerMobile )
				amount = (int)(0);

			base.Damage( amount, from );
		}

		public override void AlterDamageScalarFrom(Mobile caster, ref double scalar)
		{
			if (DoWeaponsDoMoreDamage != 1 && caster is BaseCreature)
			{
				BaseCreature bc = (BaseCreature)caster;

				if (bc.Controlled || bc.Summoned || bc.BardTarget == this)
					scalar = DoWeaponsDoMoreDamage;
			}
		}

		public override void AlterMeleeDamageTo(Mobile to, ref int damage)
		{
            		if (DoMoreDamageToPets != 1 && to is BaseCreature)
			{
				BaseCreature bc = (BaseCreature)to;

				if (bc.Controlled || bc.Summoned || bc.BardTarget == this)
					damage *= DoMoreDamageToPets;
			}
		}

		public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
		{
			if ( DoWeaponsDoMoreDamage != 0 && from is PlayerMobile )
			{
				BaseSword bs = from.FindItemOnLayer( Layer.OneHanded ) as BaseSword;
				if ( bs != null )
				damage *= DoWeaponsDoMoreDamage;

				BasePoleArm BPA = from.FindItemOnLayer( Layer.TwoHanded ) as BasePoleArm;
				if ( BPA != null )
				damage *= DoWeaponsDoMoreDamage;

				BaseSpear BP = from.FindItemOnLayer( Layer.TwoHanded ) as BaseSpear;
				if ( BP != null )
				damage *= DoWeaponsDoMoreDamage;

				BaseAxe BA = from.FindItemOnLayer( Layer.TwoHanded ) as BaseAxe;
				if ( BA != null )
				damage *= DoWeaponsDoMoreDamage;

				BaseRanged BR = from.FindItemOnLayer( Layer.TwoHanded ) as BaseRanged;
				if ( BR != null )
				damage *= DoWeaponsDoMoreDamage;
			}

            		if (DoLessDamageFromPets != 1 && from is BaseCreature)
			{
				BaseCreature bc = (BaseCreature)from;

				if (bc.Controlled || bc.Summoned || bc.BardTarget == this)
					damage /= DoLessDamageFromPets;
			}
		}

		public override void CheckReflect(Mobile caster, ref bool reflect)
		{
			if ( CanCheckReflect == 0 )
                		base.CheckReflect(caster, ref reflect);
            		else if (CanCheckReflect == 4)
				reflect = true;
            		else if (CanCheckReflect == 1 && 0.25 >= Utility.RandomDouble())
				reflect = true;
            		else if (CanCheckReflect == 2 && 0.50 >= Utility.RandomDouble())
				reflect = true;
            		else if (CanCheckReflect == 3 && 0.75 >= Utility.RandomDouble())
				reflect = true;
            		else if (CanCheckReflect == 5 && caster.Body.IsMale)
				reflect = true;
            		else if (CanCheckReflect == 6 && caster.Body.IsFemale)
				reflect = true;
		}

		public void Polymorph(Mobile m)
		{
			if (!m.CanBeginAction(typeof(PolymorphSpell)) || !m.CanBeginAction(typeof(IncognitoSpell)) || m.IsBodyMod)
				return;

			IMount mount = m.Mount;

			if (mount != null)
				mount.Rider = null;

			if (m.Mounted)
				return;

			if (m.BeginAction(typeof(PolymorphSpell)))
			{
				Item disarm = m.FindItemOnLayer(Layer.OneHanded);

				if (disarm != null && disarm.Movable)
					m.AddToBackpack(disarm);

				disarm = m.FindItemOnLayer(Layer.TwoHanded);

				if (disarm != null && disarm.Movable)
					m.AddToBackpack(disarm);

				m.BodyMod = DoPolymorphOnGaveMelee;
				m.HueMod = DoPolymorphHue;
				new ExpirePolymorphTimer(m).Start();

				if ( DoSkillLoss )
				ApplySkillLoss( m );
			}
		}

		private class ExpirePolymorphTimer : Timer
		{
			private Mobile m_Owner;

			public ExpirePolymorphTimer(Mobile owner)
				: base(TimeSpan.FromMinutes(1.5))
			{
				m_Owner = owner;

				Priority = TimerPriority.OneSecond;
			}

			protected override void OnTick()
			{
				if (!m_Owner.CanBeginAction(typeof(PolymorphSpell)))
				{
					m_Owner.BodyMod = 0;
					m_Owner.HueMod = -1;
					m_Owner.EndAction(typeof(PolymorphSpell));
				}
			}
		}


		#region Skill Loss
		public const double SkillLossFactor = 1.0 / 5;
		public static readonly TimeSpan SkillLossPeriod = TimeSpan.FromMinutes( 1.5 );

		private static Hashtable m_SkillLoss = new Hashtable();

		private class SkillLossContext
		{
			public Timer m_Timer;
			public ArrayList m_Mods;
		}

		public static void ApplySkillLoss( Mobile mob )
		{
			SkillLossContext context = (SkillLossContext)m_SkillLoss[mob];

			if ( context != null )
				return;

			context = new SkillLossContext();
			m_SkillLoss[mob] = context;

			ArrayList mods = context.m_Mods = new ArrayList();

			for ( int i = 0; i < mob.Skills.Length; ++i )
			{
				Skill sk = mob.Skills[i];
				double baseValue = sk.Base;

				if ( baseValue > 0 )
				{
					SkillMod mod = new DefaultSkillMod( sk.SkillName, true, -(baseValue * SkillLossFactor) );

					mods.Add( mod );
					mob.AddSkillMod( mod );
				}
			}

			context.m_Timer = Timer.DelayCall( SkillLossPeriod, new TimerStateCallback( ClearSkillLoss_Callback ), mob );
		}

		private static void ClearSkillLoss_Callback( object state )
		{
			ClearSkillLoss( (Mobile) state );
		}

		public static void ClearSkillLoss( Mobile mob )
		{
			SkillLossContext context = (SkillLossContext)m_SkillLoss[mob];

			if ( context == null )
				return;

			m_SkillLoss.Remove( mob );

			ArrayList mods = context.m_Mods;

			for ( int i = 0; i < mods.Count; ++i )
				mob.RemoveSkillMod( (SkillMod) mods[i] );

			context.m_Timer.Stop();
		}
		#endregion

		public void DrainLife()
		{
			ArrayList list = new ArrayList();

			foreach (Mobile m in this.GetMobilesInRange(3))
			{
				if (m == this || !CanBeHarmful(m))
					continue;

				if (m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != this.Team))
					list.Add(m);
				else if (m.Player)
					list.Add(m);
			}

			foreach (Mobile m in list)
			{
				DoHarmful(m);

				m.FixedParticles(0x374A, 10, 15, 5013, 0x496, 0, EffectLayer.Waist);
				m.PlaySound(0x231);

				m.SendMessage("You feel the life drain out of you!");

				int toDrain = DoAreaDrainLife;

				Hits += toDrain;
				m.Damage(toDrain, this);
			}
		}

		public void DrainMana()
		{
			ArrayList list = new ArrayList();

			foreach (Mobile m in this.GetMobilesInRange(3))
			{
				if (m == this || !CanBeHarmful(m))
					continue;

				if (m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != this.Team))
					list.Add(m);
				else if (m.Player)
					list.Add(m);
			}

			foreach (Mobile m in list)
			{

				m.FixedParticles(0x374A, 10, 15, 5013, 0x496, 0, EffectLayer.Waist);
				m.PlaySound(0x231);

				m.SendMessage("You feel your mana draining away!");

				int toDrain = DoAreaDrainMana;
				m.Mana -= toDrain;
				Mana += toDrain;
			}
		}

		public void DrainStam()
		{
			ArrayList list = new ArrayList();

			foreach (Mobile m in this.GetMobilesInRange(3))
			{
				if (m == this || !CanBeHarmful(m))
					continue;

				if (m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != this.Team))
					list.Add(m);
				else if (m.Player)
					list.Add(m);
			}

			foreach (Mobile m in list)
			{

				m.FixedParticles(0x374A, 10, 15, 5013, 0x496, 0, EffectLayer.Waist);
				m.PlaySound(0x231);

				m.SendMessage("You feel more and more fatigued!");

				int toDrain = DoAreaDrainStam;
				m.Stam -= toDrain;
				Stam += toDrain;
			}
		}

		public void Teleport(Mobile caster)
		{
			Map map = this.Map;

			if (map != null)
			{

				for (int i = 0; i < 10; ++i)
				{
					int x = X + (Utility.RandomMinMax(-1, 1));
					int y = Y + (Utility.RandomMinMax(-1, 1));
					int z = Z;

					if (!map.CanFit(x, y, z, 16, false, false))
						continue;

					Point3D from = caster.Location;
					Point3D to = new Point3D(x, y, z);

					if (!InLOS(to))
						continue;

					caster.Location = to;
					caster.ProcessDelta();
					caster.Combatant = null;
					this.Combatant = caster;
					caster.Freeze(TimeSpan.FromSeconds(1.5));

					Effects.SendLocationParticles(EffectItem.Create(from, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
					Effects.SendLocationParticles(EffectItem.Create(to, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);

					Effects.PlaySound(to, map, 0x1FE);
				}
			}
		}

		public void Earthquake()
		{
			Map map = this.Map;

			if (map == null)
				return;

			ArrayList targets = new ArrayList();

			foreach (Mobile m in this.GetMobilesInRange(8))
			{
				if (m == this || !CanBeHarmful(m))
					continue;

				if (m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != this.Team))
					targets.Add(m);
				else if (m.Player)
					targets.Add(m);
			}

			PlaySound(0x2F3);

			for (int i = 0; i < targets.Count; ++i)
			{
				Mobile m = (Mobile)targets[i];

				double damage = m.Hits * 0.6;

				if (damage < 10.0)
					damage = 10.0;
				else if (damage > 75.0)
					damage = 75.0;

				DoHarmful(m);

				AOS.Damage(m, this, (int)damage, 100, 0, 0, 0, 0);

				if (m.Alive && m.Body.IsHuman && !m.Mounted)
					m.Animate(20, 7, 1, true, false, 0); // take hit
			}
		}

		private DateTime m_NextAbilityTime;
        	private bool m_Bandage;

        	public bool Bandage
        	{
            		get { return m_Bandage; }
            		set { m_Bandage = value; }
        	}

        	public void BandageSelf()
        	{
            		if (this.Poisoned == true)
                		this.CurePoison(this);
            		else
                	this.Hits += CanBandageSelf;
            		this.PlaySound(0x57);
            		m_Bandage = false;
        	}

		private void DoAreaLeech()
		{
			m_NextAbilityTime += TimeSpan.FromSeconds( 2.5 );

			this.Say( true, "Blood! I vant your blood!" );
			this.FixedParticles( 0x376A, 10, 10, 9537, 33, 0, EffectLayer.Waist );

			Timer.DelayCall( TimeSpan.FromSeconds( 5.0 ), new TimerCallback( DoAreaLeech_Finish ) );
		}

		private void DoAreaLeech_Finish()
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 6 ) )
			{
				if ( this.CanBeHarmful( m ) && this.IsEnemy( m ) )
					list.Add( m );
			}

			if ( list.Count == 0 )
			{
				this.Say( true, "Ah the chase is on. Your blood will be mine soon!" );
			}
			else
			{
				double scalar;

				if ( list.Count == 1 )
					scalar = 0.50;
				else if ( list.Count == 2 )
					scalar = 0.40;
				else
					scalar = 0.20;

				for ( int i = 0; i < list.Count; ++i )
				{
					Mobile m = (Mobile)list[i];

					int damage = (int)(m.Hits * scalar);

					damage += Utility.RandomMinMax( -5, 5 );

					if ( damage < 1 )
						damage = 1;

					m.MovingParticles( this, 0x36F4, 1, 0, false, false, 32, 0, 9535,    1, 0, (EffectLayer)255, 0x100 );
					m.MovingParticles( this, 0x0001, 1, 0, false,  true, 32, 0, 9535, 9536, 0, (EffectLayer)255, 0 );

					this.DoHarmful( m );
					this.Hits += AOS.Damage( m, this, damage, 100, 0, 0, 0, 0 );
				}

				this.Say( true, "If i cannot have thine blood then i will destroy thee!" );
			}
		}

		private void DoFocusedLeech( Mobile combatant, string message )
		{
			this.Say( true, message );

			Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), new TimerStateCallback( DoFocusedLeech_Stage1 ), combatant );
		}

		private void DoFocusedLeech_Stage1( object state )
		{
			Mobile combatant = (Mobile)state;

			if ( this.CanBeHarmful( combatant ) )
			{
				this.MovingParticles( combatant, 0x36FA, 1, 0, false, false, 1108, 0, 9533, 1,    0, (EffectLayer)255, 0x100 );
				this.MovingParticles( combatant, 0x0001, 1, 0, false,  true, 1108, 0, 9533, 9534, 0, (EffectLayer)255, 0 );
				this.PlaySound( 0x1FB );

				Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( DoFocusedLeech_Stage2 ), combatant );
			}
		}

		private void DoFocusedLeech_Stage2( object state )
		{
			Mobile combatant = (Mobile)state;

			if ( this.CanBeHarmful( combatant ) )
			{
				combatant.MovingParticles( this, 0x36F4, 1, 0, false, false, 32, 0, 9535, 1,    0, (EffectLayer)255, 0x100 );
				combatant.MovingParticles( this, 0x0001, 1, 0, false,  true, 32, 0, 9535, 9536, 0, (EffectLayer)255, 0 );

				this.PlaySound( 0x209 );
				this.DoHarmful( combatant );
				this.Hits += AOS.Damage( combatant, this, Utility.RandomMinMax( 30, 40 ) - (Core.AOS ? 0 : 10), 100, 0, 0, 0, 0 );
			}
		}

		public override void OnThink()
		{
			if ( DateTime.Now >= m_NextAbilityTime && DoLeechLife)
			{
				Mobile combatant = this.Combatant;

				if ( combatant != null && combatant.Map == this.Map && combatant.InRange( this, 15 ) )
				{
					m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 8, 12 ) );

					int ability = Utility.Random( 6 );

					switch ( ability )
					{
						case 0: DoFocusedLeech( combatant, "I shall bathe in thy blood!" ); break;
						case 1: DoFocusedLeech( combatant, "I rebuke thee, maggot, and consume your life giving blood!" ); break;
						case 2: DoFocusedLeech( combatant, "I devour your life's essence to strengthen my being!" ); break;
						case 3: DoFocusedLeech( combatant, "Your blood is mine for the taking!" ); break;
						case 4: DoAreaLeech(); break;
					}
				}
			}

			if ( DoDetectHidden )
			{
				Point3D p = Location;

				double srcSkill = Skills[SkillName.DetectHidden].Value;
				int range = (int)(srcSkill / 10.0);

				if (!CheckSkill(SkillName.DetectHidden, 0.0, 100.0))
				range /= 2;

				if (range > 0 && Map != null)
				{
					IPooledEnumerable inRange = Map.GetMobilesInRange(p, range);

					foreach (Mobile trg in inRange)
					{
						if (trg.Hidden && this != trg)
						{
							double ss = srcSkill + Utility.Random(21) - 10;
							double ts = trg.Skills[SkillName.Hiding].Value + Utility.Random(21) - 10;

							if (AccessLevel >= trg.AccessLevel && (ss >= ts))
							{
								trg.RevealingAction();
								trg.SendLocalizedMessage(500814); // You have been revealed!
							}
						}
					}
					inRange.Free();
				}
			}

			if ( DoHealMobiles )
			{
				ArrayList list = new ArrayList();

				foreach ( Mobile m in this.GetMobilesInRange( 18 ) )
				{
					if ( m.Hits == m.HitsMax || !(m is BaseCreature))
						continue;

					BaseCreature bc = (BaseCreature)m;

					if ( bc.Team == this.Team )
						list.Add( m );
				}

				foreach ( Mobile m in list )
				{
					m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
					m.Hits += 40;
					if ( m.Poison != null )
					{
						m.Poison = null;
					}
				}
			}
            
			if (CanBandageSelf > 0)
            		{
                		if (m_Bandage == false && (this.Poisoned == true || this.Hits < this.HitsMax))
                		{
                    			m_Bandage = true;
                    			Timer.DelayCall(TimeSpan.FromSeconds(5.0), new TimerCallback(BandageSelf));
                		}
            		}
            		base.OnThink();
        	}

		public void SpawnEvil( Mobile target )
      		{
        		Map map = this.Map;

     		 	if ( map == null )
	                return;

		        int spawned = 0;

        		foreach ( Mobile m in this.GetMobilesInRange( 10 ) )
   			{
				if ( m is Skeleton || m is Lich || m is LichLord )
                   		++spawned;
           		}

           		if ( spawned < 10 )
           		{
             			int newSpawned = Utility.RandomMinMax( 1, 6 );

              			for ( int i = 0; i < newSpawned; ++i )
              			{
                 			BaseCreature spawn;

                   			switch ( Utility.Random( 5 ) )
               				{
                       				default:
                        			case 0: case 1: spawn = new BoneKnight(); break;
                        			case 2: case 3: spawn = new Lich(); break;
                        			case 4:         spawn = new LichLord(); break;
                    			}

                    				spawn.Team = this.Team;
                    				spawn.Map = map;

                    				bool validLocation = false;

                    			for ( int j = 0; !validLocation && j < 10; ++j )
                    			{
                       				int x = X + Utility.Random( 3 ) - 1;
                        			int y = Y + Utility.Random( 3 ) - 1;
                        			int z = map.GetAverageZ( x, y );

                        			if ( validLocation = map.CanFit( x, y, this.Z, 16, false, false ) )
                            			spawn.Location = new Point3D( x, y, Z );
                        			else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
                            			spawn.Location = new Point3D( x, y, z );
                    			}

                    			if ( !validLocation )
                        		spawn.Location = this.Location;

                    		spawn.Combatant = target;
                		}
            		}
        	}

		public void DistributeLoot()
		{
			if (Map != Map.Felucca)
				return;

			ArrayList PlayersToGiveTo = new ArrayList();

			ArrayList list = Aggressors;
			for (int i = 0; i < list.Count; ++i)
			{
				AggressorInfo info = (AggressorInfo)list[i];

				if (info.Attacker.Player && info.Attacker.Alive && (DateTime.Now - info.LastCombatTime) < TimeSpan.FromSeconds(60.0) && !PlayersToGiveTo.Contains(info.Attacker))
					PlayersToGiveTo.Add(info.Attacker);
			}

			list = Aggressed;
			for (int i = 0; i < list.Count; ++i)
			{
				AggressorInfo info = (AggressorInfo)list[i];

				if (info.Defender.Player && info.Defender.Alive && (DateTime.Now - info.LastCombatTime) < TimeSpan.FromSeconds(60.0) && !PlayersToGiveTo.Contains(info.Defender))
					PlayersToGiveTo.Add(info.Defender);
			}

			if (PlayersToGiveTo.Count <= 0)
				return;

			if (Backpack == null)
				return;

			ArrayList ItemsToGive = new ArrayList();

			foreach (object obj in Backpack.Items)
				ItemsToGive.Add(obj);

			foreach (Item item in ItemsToGive)
			{
				if (item.Movable == false || item.LootType == LootType.Blessed || item.LootType == LootType.Newbied)
					continue;
				PlayerMobile player = PlayersToGiveTo[Utility.Random(PlayersToGiveTo.Count)] as PlayerMobile;
				if (player != null && player.Backpack != null)
				{
					player.SendMessage("You have been given some loot.");
					player.Backpack.DropItem(item);
				}
			}
		}

		public override bool OnBeforeDeath()
		{
			if (DoDistributeLoot)
				DistributeLoot();

			if (DoDistributeTokens)
				Token();

			if (DoSpawnGoldOnDeath)
			{
				Map map = this.Map;

				if (map != null)
				{
					for (int x = -12; x <= 12; ++x)
					{
						for (int y = -12; y <= 12; ++y)
						{
							double dist = Math.Sqrt(x * x + y * y);

							if (dist <= 12)
								new GoodiesTimer(map, X + x, Y + y).Start();
						}
					}
				}
			}
			return base.OnBeforeDeath();
		}

		public void Token()
		{
			ArrayList toGive = new ArrayList();
			ArrayList rights = BaseCreature.GetLootingRights(this.DamageEntries,this.HitsMax);

			for (int i = rights.Count - 1; i >= 0; --i)
			{
				DamageStore ds = (DamageStore)rights[i];

				if (ds.m_HasRight)
					toGive.Add(ds.m_Mobile);
			}

			if (toGive.Count == 0)
				return;

			// Randomize
			for (int i = 0; i < toGive.Count; ++i)
			{
				int rand = Utility.Random(toGive.Count);
				object hold = toGive[i];
				toGive[i] = toGive[rand];
				toGive[rand] = hold;
			}

			for (int i = 0; i < 10; ++i)
			{
				Mobile m = (Mobile)toGive[i % toGive.Count];

				if (Utility.Random(10) < 3)
				{
					m.AddToBackpack(new SilverPrizeToken());
					m.SendMessage("You have received a silver token!");
				}
				if (Utility.Random(10) < 7)
				{
					m.AddToBackpack(new BronzePrizeToken());
					m.SendMessage("You have received a bronze token!");
				}
			}
		}

		public override void OnDeath(Container c)
		{
			base.OnDeath(c);
		}

		private class GoodiesTimer : Timer
		{
			private Map m_Map;
			private int m_X, m_Y;

			public GoodiesTimer(Map map, int x, int y)
				: base(TimeSpan.FromSeconds(Utility.RandomDouble() * 10.0))
			{
				m_Map = map;
				m_X = x;
				m_Y = y;
			}

			protected override void OnTick()
			{
				int z = m_Map.GetAverageZ(m_X, m_Y);
				bool canFit = m_Map.CanFit(m_X, m_Y, z, 6, false, false);

				for (int i = -3; !canFit && i <= 3; ++i)
				{
					canFit = m_Map.CanFit(m_X, m_Y, z + i, 6, false, false);

					if (canFit)
						z += i;
				}

				if (!canFit)
					return;

				Gold g = new Gold(300, 600);

				g.MoveToWorld(new Point3D(m_X, m_Y, z), m_Map);

				if (0.5 >= Utility.RandomDouble())
				{
					switch (Utility.Random(3))
					{
						case 0: // Fire column
						{
								Effects.SendLocationParticles(EffectItem.Create(g.Location, g.Map,
								EffectItem.DefaultDuration), 0x3709, 10, 30, 5052);
								Effects.PlaySound(g, g.Map, 0x208);

								break;
						}
						case 1: // Explosion
						{
								Effects.SendLocationParticles(EffectItem.Create(g.Location, g.Map,
								EffectItem.DefaultDuration), 0x36BD, 20, 10, 5044);
								Effects.PlaySound(g, g.Map, 0x307);

								break;
						}
						case 2: // Ball of fire
						{
								Effects.SendLocationParticles(EffectItem.Create(g.Location, g.Map,
								EffectItem.DefaultDuration), 0x36FE, 10, 10, 5052);

								break;
						}
					}
				}
			}
		}
	}
}