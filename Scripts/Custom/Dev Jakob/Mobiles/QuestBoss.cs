using System;
using System.Collections;
using Server;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Factions;
using Server.Spells;
using Server.Spells.Seventh;
using Server.Spells.Fifth;
using Server.EventPrizeSystem;

namespace Server.Mobiles
{
	public enum FactionAllegianceType
	{
		None,
		Minax,
		COM,
		TrueBrits,
		Shadowlords
	}

	public class QuestBoss : BaseCreature
	{
		bool m_Earthquake;
		bool m_DetectHidden;
		bool m_DistributeLoot;
		bool m_SpawnGold;
		bool m_DrainLife;
		bool m_Teleport;
		int m_AlterMeleeDamageTo=1;
		int m_AlterMeleeDamageFrom=1;
		bool m_CheckReflect;
		bool m_DrainMana;
		bool m_DrainStam;
		bool m_ProvoPets;
		bool m_AlterDamageScalarFrom;
		int m_Polymorph;
		bool m_Token;
		bool m_SpawnMobile;
		bool m_Freeze;
		bool m_DestroyObstacles;
        bool m_AutoDispel;
        bool m_DismountPlayer;

        bool m_EMEvolvingOn;
        int m_EMBaseValue = 2500;
        int m_EMValue = 500;
        int m_WeakenType = 1;
        int m_EMProvoBody = 248;
        int m_EMPetsBody = 75;
        int m_EMMagicBody = 110;
        int m_EMMeleeBody = 991;
        bool m_BardImmune;
        bool m_DisarmPlayer;
        int m_StunPunch;
        int m_PolymorphHue = 0;
        bool m_Flowers;
        bool m_DungeonHop;
        bool m_GiveSkillLoss;
        bool m_HealMobiles;
        int m_CastReflect = 0;
        bool m_ThrowAtomicBomb;
        string m_AtomicBombMessage;
        string m_BCDeathMessage;
        string m_GotHitMessage;
        string m_GaveHitMessage;
        string m_GotSpellDamageMessage;

        private DateTime m_NextFlowerTime;
        private DateTime m_NextDungeonHop;

		private FactionAllegianceType m_FactionAllegiance;
		private Poison m_PoisonImmune;

		public override Faction FactionAllegiance{ get{ return GetFactionAllegiance(); } }
		public override Poison PoisonImmune{ get{ return m_PoisonImmune; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public Poison SetPoisonImmune
        {
            get { return m_PoisonImmune; }
            set { m_PoisonImmune = value; }
        }

		[CommandProperty( AccessLevel.GameMaster )]
		public FactionAllegianceType SetFactionAllegiance{ get{ return m_FactionAllegiance; } set{ m_FactionAllegiance = value; } }

		public Faction GetFactionAllegiance()
		{
			switch( m_FactionAllegiance )
			{
				default: return null;
				case FactionAllegianceType.Minax: return Minax.Instance;
				case FactionAllegianceType.COM: return CouncilOfMages.Instance;
				case FactionAllegianceType.TrueBrits: return TrueBritannians.Instance;
				case FactionAllegianceType.Shadowlords: return Shadowlords.Instance;
			}
		}

		[Constructable]
		public QuestBoss() : this( AIType.AI_Mage )
		{
			Body = 999;
			Name = "Generic Quest Boss";

			SetStr( 701, 900 );
			SetDex( 201, 350 );
			SetInt( 51, 100 );

			SetHits( 6000 );
			SetStam( 300, 650 );

			SetDamage( 40, 89 );

			SetDamageType( ResistanceType.Physical, 25 );
			SetDamageType( ResistanceType.Fire, 50 );
			SetDamageType( ResistanceType.Energy, 25 );

			SetResistance( ResistanceType.Physical, 80, 90 );
			SetResistance( ResistanceType.Fire, 80, 90 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 80, 90 );
			SetResistance( ResistanceType.Energy, 80, 90 );

			SetSkill( SkillName.Anatomy, 100.0 );
			SetSkill( SkillName.MagicResist, 200.2, 250.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.DetectHidden, 100.0 );

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 130;

			Backpack pack = new Backpack();

			AddItem( pack );

            m_NextDungeonHop = DateTime.Now + TimeSpan.FromMinutes(Utility.RandomMinMax(8, 10));
        }

		public QuestBoss( AIType aiType ) : this( aiType, FightMode.Closest )
		{
		}

		public QuestBoss( AIType aiType, FightMode mode ) : base( aiType, mode, 18, 1, 0.1, 0.2 )
		{
		}

		public QuestBoss( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 5 ); // version

            //Version 4
			Poison.Serialize( m_PoisonImmune, writer );
			writer.Write((int)m_FactionAllegiance);
            writer.Write(m_BardImmune);
            writer.Write(m_EMBaseValue);
            writer.Write(m_EMValue);
            writer.Write(m_EMProvoBody);
            writer.Write(m_EMPetsBody);
            writer.Write(m_EMMagicBody);
            writer.Write(m_EMMeleeBody);
            writer.Write(m_WeakenType);
            writer.Write(m_EMEvolvingOn);
            writer.Write(m_DisarmPlayer);
            writer.Write(m_StunPunch);
            writer.Write(m_PolymorphHue);
            writer.Write(m_Flowers);
            writer.Write(m_DungeonHop);
            writer.Write(m_GiveSkillLoss);
            writer.Write(m_HealMobiles);
            writer.Write(m_CastReflect);
            writer.Write(m_ThrowAtomicBomb);
            writer.Write(m_AtomicBombMessage);
            writer.Write(m_BCDeathMessage);
            writer.Write(m_GotHitMessage);
            writer.Write(m_GaveHitMessage);
            writer.Write(m_GotSpellDamageMessage);

            //Version 3
            writer.Write(m_AutoDispel);
            writer.Write(m_DismountPlayer);

			writer.Write( m_DrainLife );
			writer.Write( m_Teleport );
			writer.Write( m_AlterMeleeDamageTo );
			writer.Write( m_AlterMeleeDamageFrom );
			writer.Write( m_CheckReflect );
			writer.Write( m_DrainMana );
			writer.Write( m_DrainStam );
			writer.Write( m_ProvoPets );
			writer.Write( m_AlterDamageScalarFrom );
			writer.Write( m_Polymorph );
			writer.Write( m_Token );
			writer.Write( m_SpawnMobile );
			writer.Write( m_Freeze );
			writer.Write( m_DestroyObstacles );

			writer.Write( m_SpawnGold );

			writer.Write( m_Earthquake );
			writer.Write( m_DetectHidden );
			writer.Write( m_DistributeLoot );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 5:
				{
					Poison.Deserialize( reader );
					m_FactionAllegiance = (FactionAllegianceType)reader.ReadInt();
					goto case 4;
				}
                case 4:
                {
                    m_BardImmune = reader.ReadBool();
                    m_EMBaseValue = reader.ReadInt();
                    m_EMValue = reader.ReadInt();
                    m_EMProvoBody = reader.ReadInt();
                    m_EMPetsBody = reader.ReadInt();
                    m_EMMagicBody = reader.ReadInt();
                    m_EMMeleeBody = reader.ReadInt();
                    m_WeakenType = reader.ReadInt();
                    m_EMEvolvingOn = reader.ReadBool();
                    m_DisarmPlayer = reader.ReadBool();
                    m_StunPunch = reader.ReadInt();
                    m_PolymorphHue = reader.ReadInt();
                    m_Flowers = reader.ReadBool();
                    m_DungeonHop = reader.ReadBool();
                    m_GiveSkillLoss = reader.ReadBool();
                    m_HealMobiles = reader.ReadBool();
                    m_CastReflect = reader.ReadInt();
                    m_ThrowAtomicBomb = reader.ReadBool();
                    m_AtomicBombMessage = reader.ReadString();
                    m_BCDeathMessage = reader.ReadString();
                    m_GotHitMessage = reader.ReadString();
                    m_GaveHitMessage = reader.ReadString();
                    m_GotSpellDamageMessage = reader.ReadString();
                    goto case 3;
                }
                case 3:
                {
                    m_AutoDispel = reader.ReadBool();
                    m_DismountPlayer = reader.ReadBool();
                    goto case 2;
                }
                case 2:
				{
					m_DrainLife = reader.ReadBool();
					m_Teleport = reader.ReadBool();
					m_AlterMeleeDamageTo = reader.ReadInt();
					m_AlterMeleeDamageFrom = reader.ReadInt();
					m_CheckReflect = reader.ReadBool();
					m_DrainMana = reader.ReadBool();
					m_DrainStam = reader.ReadBool();
					m_ProvoPets = reader.ReadBool();
					m_AlterDamageScalarFrom = reader.ReadBool();
					m_Polymorph = reader.ReadInt();
					m_Token = reader.ReadBool();
					m_SpawnMobile = reader.ReadBool();
					m_Freeze = reader.ReadBool();
					m_DestroyObstacles = reader.ReadBool();
					goto case 1;
				}
				case 1:
				{
					m_SpawnGold = reader.ReadBool();
					goto case 0;
				}
				case 0:
				{
					m_Earthquake = reader.ReadBool();
					m_DetectHidden = reader.ReadBool();
					m_DistributeLoot = reader.ReadBool();
					break;
				}
			}
		}

		public override bool BardImmune{ get{ return m_BardImmune;} }
		public override bool CanDestroyObstacles { get { return m_DestroyObstacles; } }
        public override bool AutoDispel { get { return m_AutoDispel; } }

        public void Bomb(object state)
        {
            Mobile caster = (Mobile)state;

            Atomic bomb = new Atomic();
            bomb.MoveToWorld(caster.Location, caster.Map);
        }

        public void DungeonHop()
        {
            Atomic bomb = new Atomic();
            bomb.MoveToWorld( this.Location, this.Map );

            Point3D shame = new Point3D( 5414, 20, 20 );
            Point3D destard = new Point3D( 5259, 809, 3 );
            Point3D despise = new Point3D( 5393, 625, 30 );
            Point3D deceit = new Point3D( 5146, 619, -50 );
            Point3D hythloth = new Point3D( 5966, 80, 0 );
            Point3D wrong = new Point3D( 5825, 535, 0 );
            Point3D fire = new Point3D( 5789, 1475, 23 );
            Point3D ice = new Point3D( 5765, 187, -4 );
            Point3D covetous = new Point3D( 5425, 1900, 0 );
            Point3D tera = new Point3D( 5344, 1548, 0 );
            Point3D khaldun = new Point3D( 5463, 1294, 0 );

            int location = Utility.Random( 11 );

            switch ( location )
            {
                case 0: World.Broadcast( 0x35, true, "The creature has fled to Shame!"); this.MoveToWorld( shame, this.Map ); break;
                case 1: World.Broadcast( 0x35, true, "The creature has fled to Destard!"); this.MoveToWorld( destard, this.Map ); break;
                case 2: World.Broadcast( 0x35, true, "The creature has fled to Despise!"); this.MoveToWorld( despise, this.Map ); break;
                case 3: World.Broadcast( 0x35, true, "The creature has fled to Deceit!"); this.MoveToWorld( deceit, this.Map ); break;
                case 4: World.Broadcast( 0x35, true, "The creature has fled to Hythloth!" ); this.MoveToWorld( hythloth, this.Map ); break;
                case 5: World.Broadcast( 0x35, true, "The creature has fled to Wrong!"); this.MoveToWorld( wrong, this.Map ); break;
                case 6: World.Broadcast( 0x35, true, "The creature has fled to Fire!"); this.MoveToWorld( fire, this.Map ); break;
                case 7: World.Broadcast( 0x35, true, "The creature has fled to Ice!"); this.MoveToWorld( ice, this.Map ); break;
                case 8: World.Broadcast( 0x35, true, "The creature has fled to Covetous!"); this.MoveToWorld( covetous, this.Map ); break;
                case 9: World.Broadcast( 0x35, true, "The creature has fled to Terathan Keep!"); this.MoveToWorld( tera, this.Map ); break;
                case 10: World.Broadcast( 0x35, true, "The creature has fled to Khaldun!"); this.MoveToWorld( khaldun, this.Map ); break;
            }
        }

        public void SpawnFlowers(Mobile from)
        {
            if (from == null || from.Map == null) return;
            Map map = from.Map;

            int count = 6;

            for (int i = 0; i < count; ++i)
            {
                int x = from.X + Utility.RandomMinMax(-1, 1);
                int y = from.Y + Utility.RandomMinMax(-1, 1);
                int z = from.Z;

                if (!map.CanFit(x, y, z, 16, false, true))
                {
                    z = map.GetAverageZ(x, y);

                    if (z == from.Z || !map.CanFit(x, y, z, 16, false,true))
                        continue;
                }

                Flowers flowers = new Flowers();

                flowers.MoveToWorld(new Point3D(x, y, z), map);
            }
        }

        public void DoWeakToProvo()
        {
            this.WeakenType = 1;
            this.Say("*Transforms*");
            this.BodyValue = (int)m_EMProvoBody;
            this.Int = 1000;
            this.Mana = 1000;
            this.m_EMBaseValue = this.m_EMBaseValue - (int)m_EMValue;
        }

        public void DoWeakToPets()
        {
            this.WeakenType = 2;
            this.Say("*Transforms*");
            this.BodyValue = (int)m_EMPetsBody;
            this.Int = 1;
            this.Mana = 1;
            this.m_EMBaseValue = this.m_EMBaseValue - (int)m_EMValue;
        }

        public void DoWeakToMagic()
        {
            this.WeakenType = 3;
            this.Say("*Transforms*");
            this.BodyValue = (int)m_EMMagicBody;
            this.Int = 1;
            this.Mana = 1;
            this.m_EMBaseValue = this.m_EMBaseValue - (int)m_EMValue;
        }

        public void DoWeakToHumanMelee()
        {
            this.WeakenType = 4;
            this.Say("*Transforms*");
            this.BodyValue = (int)m_EMMeleeBody;
            this.Int = 5000;
            this.Mana = 5000;
            this.m_EMBaseValue = this.m_EMBaseValue - (int)m_EMValue;
        }

        public override void Damage(int amount, Mobile from)
        {
            if (m_EMEvolvingOn)
            {
                if (this.m_WeakenType == 1)
                {
                    if (from is BaseCreature)
                    {
                        BaseCreature bc = (BaseCreature)from;

                        if (bc.BardTarget != this)
                        {
                            amount = 0;
                        }
                    }
                    else
                    {
                        amount = 0;
                    }
                }

                if (this.m_WeakenType == 2)
                {
                    if (from is BaseCreature)
                    {
                        BaseCreature bc = (BaseCreature)from;

                        if (!bc.Controlled)
                        {
                            amount = 0;
                        }
                    }
                    else
                    {
                        amount = 0;
                    }
                }

                if (this.m_WeakenType == 3)
                {
                    if (from is PlayerMobile)
                    {
                        BaseWeapon bw = from.FindItemOnLayer(Layer.OneHanded) as BaseWeapon;
                        BaseWeapon bw2 = from.FindItemOnLayer(Layer.TwoHanded) as BaseWeapon;
                        if (bw != null || bw2 != null)
                        {
                            amount = 0;
                        }
                    }
                    else
                    {
                        amount = 0;
                    }
                }

                if (this.m_WeakenType == 4)
                {
                    if (from is PlayerMobile)
                    {
                        BaseWeapon bw3 = from.FindItemOnLayer(Layer.OneHanded) as BaseWeapon;
                        BaseWeapon bw4 = from.FindItemOnLayer(Layer.TwoHanded) as BaseWeapon;
                        if (bw3 == null && bw4 == null)
                        {
                            amount = (int)(0);
                        }
                    }
                    else
                    {
                        amount = (int)(0);
                    }
                }
            }
            base.Damage(amount, from);
        }

        #region Skill Loss
        //Copied from LichChamp
        public const double SkillLossFactor = 1.0 / 5;
        public static readonly TimeSpan SkillLossPeriod = TimeSpan.FromMinutes(1.5);

        private static Hashtable m_SkillLoss = new Hashtable();

        private class SkillLossContext
        {
            public Timer m_Timer;
            public ArrayList m_Mods;
        }

        public static void ApplySkillLoss(Mobile mob)
        {
            SkillLossContext context = (SkillLossContext)m_SkillLoss[mob];

            if (context != null)
                return;

            context = new SkillLossContext();
            m_SkillLoss[mob] = context;

            ArrayList mods = context.m_Mods = new ArrayList();

            for (int i = 0; i < mob.Skills.Length; ++i)
            {
                Skill sk = mob.Skills[i];
                double baseValue = sk.Base;

                if (baseValue > 0)
                {
                    SkillMod mod = new DefaultSkillMod(sk.SkillName, true, -(baseValue *
               SkillLossFactor));

                    mods.Add(mod);
                    mob.AddSkillMod(mod);
                }
            }

            context.m_Timer = Timer.DelayCall(SkillLossPeriod, new
         TimerStateCallback(ClearSkillLoss_Callback), mob);
        }

        private static void ClearSkillLoss_Callback(object state)
        {
            ClearSkillLoss((Mobile)state);
        }

        public static void ClearSkillLoss(Mobile mob)
        {
            SkillLossContext context = (SkillLossContext)m_SkillLoss[mob];

            if (context == null)
                return;

            m_SkillLoss.Remove(mob);

            ArrayList mods = context.m_Mods;

            for (int i = 0; i < mods.Count; ++i)
                mob.RemoveSkillMod((SkillMod)mods[i]);

            context.m_Timer.Stop();
        }
        #endregion

        public void DeathMessage()
        {
            World.Broadcast(0x35, true, m_BCDeathMessage);
        }

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

            if ( 0.25 >= Utility.RandomDouble() && m_DismountPlayer )
            {
                IMount mt = defender.Mount;
                if (mt != null)
                {
                    mt.Rider = null;
                    defender.SendMessage("The force of the attack knocks you off your mount!");
                }
            }

            if ( 0.80 >= Utility.RandomDouble() && m_Polymorph > 0 )
				Polymorph( defender );

			if ( 0.1 >= Utility.RandomDouble() && m_Earthquake )
				Earthquake();

			if ( 0.25 >= Utility.RandomDouble() && m_DrainLife )
				DrainLife();

			if ( 0.25 >= Utility.RandomDouble() && m_DrainMana )
				DrainMana();

			if ( 0.25 >= Utility.RandomDouble() && m_DrainStam )
				DrainStam();

			if ( 0.1 >= Utility.RandomDouble() && m_DetectHidden )
				DetectHidden();

			if ( 0.33 >= Utility.RandomDouble() && defender is BaseCreature && m_ProvoPets )
			{
				BaseCreature c = (BaseCreature)defender;

				if ( c.Controlled && c.ControlMaster != null )
				{
					c.ControlTarget = c.ControlMaster;
					c.ControlOrder = OrderType.Attack;
					c.Combatant = c.ControlMaster;
				}
			}

            if (m_GaveHitMessage != null && 0.01 >= Utility.RandomDouble())
                this.Say(m_GaveHitMessage);

            if (0.1 >= Utility.RandomDouble() && m_GiveSkillLoss)
                ApplySkillLoss(defender);

            if (m_DisarmPlayer && 0.25 >= Utility.RandomDouble() && defender is PlayerMobile)
            {
                Item toDisarm=null;
                if (defender.FindItemOnLayer(Layer.OneHanded) != null)
                    toDisarm = defender.FindItemOnLayer(Layer.OneHanded);
                else if (defender.FindItemOnLayer(Layer.TwoHanded) != null)
                    toDisarm = defender.FindItemOnLayer(Layer.TwoHanded);

                if (toDisarm != null && toDisarm.Movable && defender.Backpack != null )
                {
                    defender.Backpack.DropItem(toDisarm);
                    defender.SendMessage("The intensity of the attack disarms you!");
                }
            }

            if (m_StunPunch > 0 && 0.25 >= Utility.RandomDouble())
            {
                defender.Freeze(TimeSpan.FromSeconds(m_StunPunch));
                defender.SendMessage("You have been stunned!");
            }
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.25 >= Utility.RandomDouble() && m_DrainLife )
				DrainLife();

			if ( 0.25 >= Utility.RandomDouble() && m_DrainMana )
				DrainMana();

			if ( 0.25 >= Utility.RandomDouble() && m_DrainStam )
				DrainStam();

			if ( 0.33 >= Utility.RandomDouble() && attacker is BaseCreature && m_ProvoPets )
			{
				BaseCreature c = (BaseCreature)attacker;

				if ( c.Controlled && c.ControlMaster != null )
				{
					c.ControlTarget = c.ControlMaster;
					c.ControlOrder = OrderType.Attack;
					c.Combatant = c.ControlMaster;
				}
			}

			if ( 0.1 >= Utility.RandomDouble() && m_Earthquake )
				Earthquake();

            if (0.01 >= Utility.RandomDouble() && m_GotHitMessage != null)
                this.Say(m_GotHitMessage);

    	}

		public override void OnDamagedBySpell( Mobile caster )
		{
			base.OnDamagedBySpell( caster );

			if ( m_Teleport && 0.25 >= Utility.RandomDouble() )
				Teleport( caster );

            if (m_GotSpellDamageMessage != null && 0.01 >= Utility.RandomDouble())
                this.Say(m_GotSpellDamageMessage);

            if (m_CastReflect > 0 && 0.05 >= Utility.RandomDouble())
            {
                this.Say("In Jux Sanct");
                this.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Waist);
                this.PlaySound(0x1E9);
                this.MagicDamageAbsorb = m_CastReflect;
            }

            if (m_ThrowAtomicBomb && 0.1 >= Utility.RandomDouble())
            {
                if (m_AtomicBombMessage != null)
                    this.Say(m_AtomicBombMessage);

                Effects.SendMovingEffect(this, caster, 0xF0D & 0x3FFF, 7, 0, false, false, 349, 0);
                Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerStateCallback(Bomb), caster);
            }
		}

		public override void OnDamage(int amount, Mobile from, bool willKill)
		{
			if (from != null && 0.1 >= Utility.RandomDouble() && m_SpawnMobile)
			{
				BaseCreature spawn = new DecayingSpawn(this);

				spawn.BodyValue = this.BodyValue;
				spawn.Hue = this.Hue;
				spawn.BaseSoundID = this.BaseSoundID;
				spawn.Team = this.Team;
				spawn.MoveToWorld(from.Location, from.Map);
				spawn.Combatant = from;
				spawn.Say("I am here my master!");
			}
			base.OnDamage(amount, from, willKill);
		}

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			if ( caster is BaseCreature && m_AlterDamageScalarFrom)
			{
				BaseCreature bc = (BaseCreature)caster;

				if ( bc.Controlled || bc.Summoned || bc.BardTarget == this )
				scalar = 0.33;
			}
		}

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)to;

				if ( bc.Controlled || bc.Summoned || bc.BardTarget == this )
				damage *= (int)m_AlterMeleeDamageTo;
			}
		}

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Controlled || bc.Summoned || bc.BardTarget == this )
				damage /= (int)m_AlterMeleeDamageFrom;
			}
		}

		public override void CheckReflect( Mobile caster, ref bool reflect )
		{
			if ( m_CheckReflect )
				reflect = true;
		}

		public void Polymorph( Mobile m )
		{
			if ( !m.CanBeginAction( typeof( PolymorphSpell) ) || !m.CanBeginAction( typeof( IncognitoSpell ) ) || m.IsBodyMod )
				return;

			IMount mount = m.Mount;

			if ( mount != null )
				mount.Rider = null;

			if ( m.Mounted )
				return;

			if ( m.BeginAction( typeof( PolymorphSpell) ) )
			{
				Item disarm = m.FindItemOnLayer( Layer.OneHanded );

				if ( disarm != null && disarm.Movable )
					m.AddToBackpack( disarm );

				disarm = m.FindItemOnLayer( Layer.TwoHanded );

				if ( disarm != null && disarm.Movable )
					m.AddToBackpack( disarm );

				m.BodyMod = (int)m_Polymorph;
				m.HueMod = m_PolymorphHue;
				new ExpirePolymorphTimer( m ).Start();
			}
		}

		private class ExpirePolymorphTimer : Timer
		{
			private Mobile m_Owner;

			public ExpirePolymorphTimer( Mobile owner ) : base( TimeSpan.FromMinutes( 1.5 ) )
			{
				m_Owner = owner;

				Priority = TimerPriority.OneSecond;
			}

			protected override void OnTick()
			{
				if ( !m_Owner.CanBeginAction( typeof( PolymorphSpell ) ) )
				{
					m_Owner.BodyMod = 0;
					m_Owner.HueMod = -1;
					m_Owner.EndAction( typeof( PolymorphSpell ) );
				}
			}
		}

		public void DrainLife()
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 3 ) )
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

				m.FixedParticles( 0x374A, 10, 15, 5013, 0x496, 0, EffectLayer.Waist );
				m.PlaySound( 0x231 );

				m.SendMessage( "You feel the life drain out of you!" );

				int toDrain = Utility.RandomMinMax( 20, 40 );

				Hits += toDrain;
				m.Damage( toDrain, this );
			}
		}

		public void DrainMana()
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 3 ) )
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

				m.FixedParticles( 0x374A, 10, 15, 5013, 0x496, 0, EffectLayer.Waist );
				m.PlaySound( 0x231 );

				m.SendMessage( "You feel your mana draining away!" );

				int toDrain = Utility.RandomMinMax( 10, 30 );
				m.Mana -= toDrain;
				Mana += toDrain;
			}
		}

		public void DrainStam()
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 3 ) )
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

				m.FixedParticles( 0x374A, 10, 15, 5013, 0x496, 0, EffectLayer.Waist );
				m.PlaySound( 0x231 );

				m.SendMessage( "You feel more and more fatigued!" );

				int toDrain = Utility.RandomMinMax( 5, 10 );
				m.Stam -= toDrain;
				Stam += toDrain;
			}
		}

       		public void Teleport( Mobile caster )
        	{
          		Map map = this.Map;

			if ( map != null )
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

			if ( map == null )
				return;

			ArrayList targets = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 8 ) )
			{
				if ( m == this || !CanBeHarmful( m ) )
					continue;

				if ( m is BaseCreature && (((BaseCreature)m).Controlled ||
((BaseCreature)m).Summoned || ((BaseCreature)m).Team != this.Team) )
					targets.Add( m );
				else if ( m.Player )
					targets.Add( m );
			}

			PlaySound( 0x2F3 );

			for ( int i = 0; i < targets.Count; ++i )
			{
				Mobile m = (Mobile)targets[i];

				double damage = m.Hits * 0.6;

				if ( damage < 10.0 )
					damage = 10.0;
				else if ( damage > 75.0 )
					damage = 75.0;

				DoHarmful( m );

				AOS.Damage( m, this, (int)damage, 100, 0, 0, 0, 0 );

				if ( m.Alive && m.Body.IsHuman && !m.Mounted )
					m.Animate( 20, 7, 1, true, false, 0 ); // take hit
			}
		}

		protected void DetectHidden()
		{
			Point3D p = Location;

			double srcSkill = Skills[SkillName.DetectHidden].Value;
			int range = (int)(srcSkill / 10.0);

			if ( !CheckSkill( SkillName.DetectHidden, 0.0, 100.0 ) )
				range /= 2;

			if ( range > 0 && Map != null && Map != Map.Internal )
			{
				IPooledEnumerable inRange = Map.GetMobilesInRange( p, range );

				foreach ( Mobile trg in inRange )
				{
					if ( trg.Hidden && this != trg )
					{
						double ss = srcSkill + Utility.Random( 21 ) - 10;
						double ts = trg.Skills[SkillName.Hiding].Value + Utility.Random( 21 ) - 10;

						if ( AccessLevel >= trg.AccessLevel && ( ss >= ts ) )
						{
							trg.RevealingAction();
							trg.SendLocalizedMessage( 500814 ); // You have been revealed!
						}
					}
				}
				inRange.Free();
			}
		}

		private DateTime m_NextAbilityTime;
		public override void OnThink()
		{
			if ( m_Freeze && DateTime.Now >= m_NextAbilityTime )
			{
				Mobile combatant = this.Combatant;

				if ( combatant != null && combatant.Map == this.Map && combatant.InRange( this, 15 ) )
				{
					m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 8, 12 ) );

					Freeze( combatant, "Tremble before me mortal!" );
				}
			}

            if ( m_HealMobiles )
            {
                ArrayList list = new ArrayList();

                foreach ( Mobile m in this.GetMobilesInRange( 18 ) )
                {
                    if ( m.Hits == m.HitsMax || !(m is BaseCreature) || m == this )
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

            if ( m_Flowers && DateTime.Now >= m_NextFlowerTime )
            {
                Mobile combatant = this.Combatant;

                if ( combatant != null && combatant.Map == this.Map && combatant.InRange( this, 15 ) )
                {
                    m_NextFlowerTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 20, 25 ) );

                    string[] flowers = {"What a beautiful day to grow flowers", "Here little human, have some flowers to brighten your day", "Rise my beauties and brighten my day", "Awww arn't they pretty?", "Flowers for me and flowers for you"};
                    Say(flowers[Utility.Random(5)] );
                    SpawnFlowers( combatant );
                }
            }

            if ( DateTime.Now >= m_NextDungeonHop && m_DungeonHop )
            {
                m_NextDungeonHop = DateTime.Now + TimeSpan.FromMinutes(Utility.RandomMinMax( 8, 10 ) );
                DungeonHop();
            }

            if( m_EMEvolvingOn && this.Hits < this.m_EMBaseValue )
            {
                if( this.m_WeakenType == 1 )
                {
                    int ability = Utility.Random( 3 );
                    switch ( ability )
                    {
                        case 0: DoWeakToPets(); break;
                        case 1: DoWeakToMagic(); break;
                        case 2: DoWeakToHumanMelee(); break;
                    }
                }
                else if( this.m_WeakenType == 2 )
                {
                    int ability = Utility.Random( 3 );
                    switch ( ability )
                    {
                    case 0: DoWeakToProvo(); break;
                    case 1: DoWeakToMagic(); break;
                    case 2: DoWeakToHumanMelee(); break;
                    }
                }
                else if( this.m_WeakenType == 3 )
                {
                    int ability = Utility.Random( 3 );

                    switch ( ability )
                    {
                        case 0: DoWeakToProvo(); break;
                        case 1: DoWeakToPets(); break;
                        case 2: DoWeakToHumanMelee(); break;
                    }
                }
                else if( this.m_WeakenType == 4 )
                {
                    int ability = Utility.Random( 3 );

                    switch ( ability )
                    {
                        case 0: DoWeakToProvo(); break;
                        case 1: DoWeakToMagic(); break;
                        case 2: DoWeakToPets(); break;
                    }
                }
            }

			base.OnThink();}

		public void Freeze( Mobile combatant, string message )
		{
			this.Say( true, message );
			m_Table[combatant] = Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( DoEffect ), new object[]{ combatant, 0 } );
		}

		private static Hashtable m_Table = new Hashtable();

		public static bool UnderEffect( Mobile m )
		{
			return m_Table.Contains( m );
		}

		public static void StopEffect( Mobile m, bool message )
		{
			Timer t = (Timer)m_Table[m];

			if ( t != null )
			{
				if ( message )
					m.SendMessage( "Your weakened state breaks though the fear!" );

				t.Stop();
				m_Table.Remove( m );
			}
		}

		public void DoEffect( object state )
		{
      if (!Alive) return;
			object[] states = (object[])state;

			Mobile m = (Mobile)states[0];
			int count = (int)states[1];

			if ( !m.Alive )
			{
				StopEffect( m, false );
			}
			else
			{

				if (m.Hits < m.HitsMax * 0.5)
				{
					StopEffect( m, true );
				}
				else
				{
					if ( (count % 4) == 0 )
					{
						m.LocalOverheadMessage( Network.MessageType.Emote, m.SpeechHue, true, "* You are burned alive! *" );
						m.NonlocalOverheadMessage( Network.MessageType.Emote, m.SpeechHue, true, String.Format( "* {0} is burned alive! *", m.Name ) );
					}

					m.FixedParticles( 0x3709, 10, 30, 5052, EffectLayer.LeftFoot );
					m.PlaySound( 0x54 );
					m.Freeze( TimeSpan.FromSeconds( 2.0 ) );
					m.SendMessage( "You paralysed with fear!" );

					AOS.Damage( m, this, Utility.RandomMinMax( 15, 20 ) - (Core.AOS ? 0 : 10), 100, 0, 0, 0, 0 );

					states[1] = count + 1;

					if ( !m.Alive )
						StopEffect( m, false );
				}
			}
		}

		public void DistributeLoot()
		{
			if ( Map != Map.Felucca )
				return;

			ArrayList PlayersToGiveTo = new ArrayList();

			ArrayList list = Aggressors;
			for ( int i = 0; i < list.Count; ++i )
			{
				AggressorInfo info = (AggressorInfo)list[i];

				if ( info.Attacker.Player && info.Attacker.Alive && (DateTime.Now - info.LastCombatTime) < TimeSpan.FromSeconds( 60.0 ) && !PlayersToGiveTo.Contains( info.Attacker ) )
					PlayersToGiveTo.Add( info.Attacker );
			}

			list = Aggressed;
			for ( int i = 0; i < list.Count; ++i )
			{
				AggressorInfo info = (AggressorInfo)list[i];

				if ( info.Defender.Player && info.Defender.Alive && (DateTime.Now - info.LastCombatTime) < TimeSpan.FromSeconds( 60.0 ) && !PlayersToGiveTo.Contains( info.Defender ) )
					PlayersToGiveTo.Add( info.Defender );
			}

			if ( PlayersToGiveTo.Count <= 0 )
				return;

			if ( Backpack == null )
				return;

			ArrayList ItemsToGive = new ArrayList();

			foreach( object obj in Backpack.Items )
				ItemsToGive.Add( obj );

			foreach ( Item item in ItemsToGive )
			{
				if ( item.Movable == false || item.LootType == LootType.Blessed || item.LootType == LootType.Newbied )
					continue;
				PlayerMobile player = PlayersToGiveTo[Utility.Random( PlayersToGiveTo.Count )] as PlayerMobile;
				if ( player != null && player.Backpack != null )
				{
					player.SendMessage( "You have been given some loot." );
					player.Backpack.DropItem( item );
				}
			}
		}

		public override bool OnBeforeDeath()
		{
			if ( m_DistributeLoot )
				DistributeLoot();

			if ( m_Token )
				Token();

			if ( m_SpawnGold )
			{
				Map map = this.Map;

				if ( map != null )
				{
					for ( int x = -12; x <= 12; ++x )
					{
						for ( int y = -12; y <= 12; ++y )
						{
							double dist = Math.Sqrt(x*x+y*y);

							if ( dist <= 12 )
								new GoodiesTimer( map, X + x, Y + y ).Start();
						}
					}
				}
			}

            if (m_BCDeathMessage != null)
                DeathMessage();
			return base.OnBeforeDeath();
		}

		public void Token()
		{
			ArrayList toGive = new ArrayList();
			ArrayList rights = BaseCreature.GetLootingRights( this.DamageEntries, this.HitsMax );

			for ( int i = rights.Count - 1; i >= 0; --i )
			{
				DamageStore ds = (DamageStore)rights[i];

				if ( ds.m_HasRight )
					toGive.Add( ds.m_Mobile );
			}

			if ( toGive.Count == 0 )
				return;

			// Randomize
			for ( int i = 0; i < toGive.Count; ++i )
			{
				int rand = Utility.Random( toGive.Count );
				object hold = toGive[i];
				toGive[i] = toGive[rand];
				toGive[rand] = hold;
			}

			for ( int i = 0; i < 10; ++i )
			{
				Mobile m = (Mobile)toGive[i % toGive.Count];

				if ( Utility.Random( 10 ) < 3 )
                {
					m.AddToBackpack( new SilverPrizeToken() );
					m.SendMessage( "You have received a silver token!" );
				}
				if ( Utility.Random( 10 ) < 7 )
				{
					m.AddToBackpack( new BronzePrizeToken() );
					m.SendMessage( "You have received a bronze token!" );
				}
			}
		}

		public override void OnDeath( Container c )
		{
			base.OnDeath( c );
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool DoEarthquake
		{
			get{ return m_Earthquake; }
			set{ m_Earthquake = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool DoDetectHidden
		{
			get{ return m_DetectHidden; }
			set{ m_DetectHidden = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool DoDistributeLoot
		{
			get{ return m_DistributeLoot; }
			set{ m_DistributeLoot = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool DoSpawnGoldOnDeath
		{
			get{ return m_SpawnGold; }
			set{ m_SpawnGold = value; }
		}
        [CommandProperty(AccessLevel.GameMaster)]
        public bool DoAutoDispel
        {
            get { return m_AutoDispel; }
            set { m_AutoDispel = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanDismountPlayer
        {
            get { return m_DismountPlayer; }
            set { m_DismountPlayer = value; }
        }

		private class GoodiesTimer : Timer
		{
			private Map m_Map;
			private int m_X, m_Y;

			public GoodiesTimer( Map map, int x, int y ) : base( TimeSpan.FromSeconds(
Utility.RandomDouble() * 10.0 ) )
			{
				m_Map = map;
				m_X = x;
				m_Y = y;
			}

			protected override void OnTick()
			{
				int z = m_Map.GetAverageZ( m_X, m_Y );
				bool canFit = m_Map.CanFit( m_X, m_Y, z, 6, false, false );

				for ( int i = -3; !canFit && i <= 3; ++i )
				{
					canFit = m_Map.CanFit( m_X, m_Y, z + i, 6, false, false );

					if ( canFit )
						z += i;
				}

				if ( !canFit )
					return;

				Gold g = new Gold( 500, 1000 );

				g.MoveToWorld( new Point3D( m_X, m_Y, z ), m_Map );

				if ( 0.5 >= Utility.RandomDouble() )
				{
					switch ( Utility.Random( 3 ) )
					{
						case 0: // Fire column
						{
							Effects.SendLocationParticles( EffectItem.Create( g.Location, g.Map,
EffectItem.DefaultDuration ), 0x3709, 10, 30, 5052 );
							Effects.PlaySound( g, g.Map, 0x208 );

							break;
						}
						case 1: // Explosion
						{
							Effects.SendLocationParticles( EffectItem.Create( g.Location, g.Map,
EffectItem.DefaultDuration ), 0x36BD, 20, 10, 5044 );
							Effects.PlaySound( g, g.Map, 0x307 );

							break;
						}
						case 2: // Ball of fire
						{
							Effects.SendLocationParticles( EffectItem.Create( g.Location, g.Map,
EffectItem.DefaultDuration ), 0x36FE, 10, 10, 5052 );

							break;
						}
					}
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool DoAreaDrainLife
		{
			get{ return m_DrainLife; }
			set{ m_DrainLife = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool DoTeleport
		{
			get{ return m_Teleport; }
			set{ m_Teleport = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int DoMoreDamageToPets
		{
			get{ return m_AlterMeleeDamageTo; }
			set{ if ( value >= 1 && value <= 20 ) m_AlterMeleeDamageTo = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int DoLessDamageFromPets
		{
			get{ return m_AlterMeleeDamageFrom; }
			set{ if ( value >= 1 && value <= 20 ) m_AlterMeleeDamageFrom = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool DoAlwaysReflect
		{
			get{ return m_CheckReflect; }
			set{ m_CheckReflect = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool DoAreaDrainMana
		{
			get{ return m_DrainMana; }
			set{ m_DrainMana = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool DoAreaDrainStam
		{
			get{ return m_DrainStam; }
			set{ m_DrainStam = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool DoProvoPets
		{
			get{ return m_ProvoPets; }
			set{ m_ProvoPets = value; }
		}

		[CommandProperty( AccessLevel.GameMaster)]
		public bool DoLessMagicDamageFromPets
		{
			get{ return m_AlterDamageScalarFrom; }
			set{ m_AlterDamageScalarFrom = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int DoPolymorphOnGaveMelee
		{
			get{ return m_Polymorph; }
			set
			{
				if ( value >= 1 && value <= 1000 )
				m_Polymorph = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool DoDistributeTokens
		{
			get{ return m_Token; }
			set{ m_Token = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool DoSpawnMobileOnDamage
		{
			get{ return m_SpawnMobile; }
			set{ m_SpawnMobile = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool DoBuringParalyse
		{
			get{ return m_Freeze; }
			set{ m_Freeze = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool DoDestroyObstacles
		{
			get{ return m_DestroyObstacles; }
			set{ m_DestroyObstacles = value; }
		}

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsBardImmune
        {
            get { return m_BardImmune; }
            set { m_BardImmune = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int EMBaseValue
        {
            get { return m_EMBaseValue; }
            set { if (value >= 1000 && value <= 10000) m_EMBaseValue = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int EMValue
        {
            get { return m_EMValue; }
            set { if (value >= 250 && value <= 1000) m_EMValue = value; }
        }

		[CommandProperty(AccessLevel.GameMaster)]
        public int WeakenType
        {
            get { return m_WeakenType; }
            set { m_WeakenType = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int EMProvoBody
        {
            get { return m_EMProvoBody; }
            set { if (value >= 1 && value <= 1000) m_EMProvoBody = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int EMPetsBody
        {
            get { return m_EMPetsBody; }
            set { if (value >= 1 && value <= 1000) m_EMPetsBody = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int EMMagicBody
        {
            get { return m_EMMagicBody; }
            set { if (value >= 1 && value <= 1000) m_EMMagicBody = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int EMMeleeBody
        {
            get { return m_EMMeleeBody; }
            set { if (value >= 1 && value <= 1000) m_EMMeleeBody = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool EMEvolvingOn
        {
            get { return m_EMEvolvingOn; }
            set { m_EMEvolvingOn = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool DisarmPlayer
        {
            get { return m_DisarmPlayer; }
            set { m_DisarmPlayer = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int StunPunchLength
        {
            get { return m_StunPunch; }
            set { if (value >= 0 && value <= 20) m_StunPunch = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanSpawnFlowers
        {
            get { return m_Flowers; }
            set { m_Flowers = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool DoDungeonHop
        {
            get { return m_DungeonHop; }
            set { m_DungeonHop = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool DoSkillLossOnMelee
        {
            get { return m_GiveSkillLoss; }
            set { m_GiveSkillLoss = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool DoHealMobiles
        {
            get { return m_HealMobiles; }
            set { m_HealMobiles = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int CanCastReflect
        {
            get { return m_CastReflect; }
            set { if (value >= 0 && value <= 200) m_CastReflect = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ThrowAtomicBomb
        {
            get { return m_ThrowAtomicBomb; }
            set { m_ThrowAtomicBomb = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string ThrowABMessage
        {
            get { return m_AtomicBombMessage; }
            set { m_AtomicBombMessage = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string BCDeathMessage
        {
            get { return m_BCDeathMessage; }
            set { m_BCDeathMessage = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string GotHitMessage
        {
            get { return m_GotHitMessage; }
            set { m_GotHitMessage = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string GaveHitMessage
        {
            get { return m_GaveHitMessage; }
            set { m_GaveHitMessage = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string GotSpellDamageMessage
        {
            get { return m_GotSpellDamageMessage; }
            set { m_GotSpellDamageMessage = value; }
        }
	}
}