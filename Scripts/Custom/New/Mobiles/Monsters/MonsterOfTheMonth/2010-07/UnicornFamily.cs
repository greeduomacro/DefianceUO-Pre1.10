using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Targeting;
using Server.Misc;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    #region UnicornQueenMOTM class
    [CorpseName("a royal unicorn corpse")]
    public class UnicornQueenMOTM : BaseCreature
    {
        private TimeSpan respawn_interval = TimeSpan.FromSeconds(Utility.RandomMinMax(200, 1200));
        private DateTime respawn_previous;

        private ArrayList m_pups;
        int pupCount = Utility.RandomMinMax(3,4);

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 3; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override bool CanRegenHits { get { return true; } }
	  public override bool BardImmune{ get{ return true; } }


        [CommandProperty(AccessLevel.GameMaster)]
        public bool RespawnPups
        {
            get { return false; }
            set { if (value) SpawnBabies(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan RespawnNext
        {
            get { return (respawn_previous + respawn_interval) - DateTime.Now; }
        }

        [Constructable]
        public UnicornQueenMOTM() : base(AIType.AI_Mage, FightMode.Agressor, 10, 1, 0.1, 0.3)
        {
            Name = "Queen Of Unicorns";
            Body = 0x101;
            BaseSoundID = 0x4BC;
		Hue = 1150;

            SetStr(91, 110);
            SetDex(76, 95);
            SetInt(300);

            SetHits(1200,1500);
            SetMana(300);

            SetDamage(11, 21);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.MagicResist, 150.0, 180.0);
            SetSkill(SkillName.Tactics, 50.1, 70.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 2000;
            Karma = 2000;

            VirtualArmor = 30;
            m_pups = new ArrayList();

            respawn_previous = DateTime.Now;
            Timer.DelayCall(TimeSpan.FromSeconds(15), new TimerCallback(SpawnBabies));
        }

	  public override void OnDeath( Container c )
	  {
			if (Utility.Random( 150 ) <  1 )
			c.DropItem( new UnicornQueenStatueMOTM() );

            	base.OnDeath( c );
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

		//Melee damage to controlled mobiles is multiplied by 8
		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)to;

				if ( bc.Controlled )
					damage *= 15;
			}
		}

        public void SpawnBabies()
        {

            Defrag();
            int family = m_pups.Count;

            if (family >= pupCount)
                return;

            //Say( "family {0}, should be {1}", family, pupCount );

            Map map = this.Map;

            if (map == null)
                return;

            int hr = (int)((this.RangeHome + 1) / 2);

            for (int i = family; i < pupCount; ++i)
            {
                UnicornPup pup = new UnicornPup();

                bool validLocation = false;
                Point3D loc = this.Location;

                for (int j = 0; !validLocation && j < 10; ++j)
                {
                    int x = X + Utility.Random(10) - 5;
                    int y = Y + Utility.Random(10) - 5;
                    int z = map.GetAverageZ(x, y);

                    if (validLocation = map.CanFit(x, y, this.Z, 16, false, false))
                    {
                        loc = new Point3D(x, y, Z);
                        break;
                    }
                    else if (validLocation = map.CanFit(x, y, z, 16, false, false))
                    {
                        loc = new Point3D(x, y, z);
                        break;
                    }
                }

                pup.Queen = this;
                pup.Team = this.Team;
                pup.Home = this.Location;
                pup.RangeHome = (hr > 4 ? 4 : hr);

                pup.MoveToWorld(loc, map);
                m_pups.Add(pup);
            }

            respawn_previous = DateTime.Now;
        }

        protected override void OnLocationChange(Point3D oldLocation)
        {
            try
            {
                foreach (Mobile m in m_pups)
                {
                    if (m is UnicornPup && m.Alive && ((UnicornPup)m).ControlMaster == null)
                    {
                        ((UnicornPup)m).Home = this.Location;
                    }
                }
            }
            catch { }

            base.OnLocationChange(oldLocation);
        }

        public void Defrag()
        {
            for (int i = 0; i < m_pups.Count; ++i)
            {
                try
                {
                    object o = m_pups[i];

                    UnicornPup pup = o as UnicornPup;

                    if (pup == null || !pup.Alive)
                    {
                        m_pups.RemoveAt(i);
                        --i;
                    }

                    else if (pup.Controlled || pup.IsStabled)
                    {
                        pup.Queen = null;
                        m_pups.RemoveAt(i);
                        --i;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nException Caught in UnicornQueenMOTM Defrag: \n{0}\n", e);
                }
            }
        }

        public override void OnThink()
        {
            if(DateTime.Now > (respawn_previous + respawn_interval))
                Timer.DelayCall(TimeSpan.Zero, new TimerCallback(SpawnBabies));

            base.OnThink();
        }

        public override bool OnBeforeDeath()
        {
            Defrag();
            foreach (Mobile m in m_pups)
            {
                if (m is UnicornPup && m.Alive)
                    m.Kill();
            }

            return base.OnBeforeDeath();
        }

        public override void OnDelete()
        {
            Defrag();
            foreach (Mobile m in m_pups)
            {
                if (m is UnicornPup && m.Alive)
                    m.Delete();
            }

            base.OnDelete();
        }

        public UnicornQueenMOTM(Serial serial) : base(serial)
        {
            m_pups = new ArrayList();
            respawn_previous = DateTime.Now;
            //Timer.DelayCall(TimeSpan.FromMinutes(1), new TimerCallback(SpawnBabies));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.WriteMobileList(m_pups, true);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_pups = reader.ReadMobileList();
        }
    }
    #endregion

    #region UnicornPup class
    [CorpseName("a tiny corpse")]
    public class UnicornPup : BaseCreature
    {
        public override int Meat { get { return 1; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
	  public override bool HasBreath{ get{ return true; } } // fire breath enabled
	  public override Poison PoisonImmune{ get{ return Poison.Lethal; } }



        private UnicornQueenMOTM m_queen;

        [CommandProperty(AccessLevel.GameMaster)]
        public UnicornQueenMOTM Queen
        {
            get { return m_queen; }
            set { m_queen = value; }
        }

        [Constructable]
        public UnicornPup() : base(AIType.AI_Animal, FightMode.Agressor, 10, 1, 0.1, 0.3)
        {
            Name = "a baby unicorn";
            Body = 0x7A;
            BaseSoundID = 0x4BC;
		Hue = 22222;

            SetStr(100, 200);
            SetDex(30, 50);
            SetInt(70, 40);

            SetHits(25000);
            SetMana(0);

            SetDamage(10, 20);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 15);

            SetSkill(SkillName.MagicResist, 22.1, 47.0);
            SetSkill(SkillName.Tactics, 19.2, 31.0);
            SetSkill(SkillName.Wrestling, 19.2, 46.0);

            Fame = 100;
            Karma = 100;

            VirtualArmor = 10;
        }

	  public override void OnDeath( Container c )
	  {
		 if (Utility.Random( 200 ) <  1 )
		 c.DropItem( new UnicornRareMOTM() );

             base.OnDeath( c );
	  }


        public override void OnCombatantChange()
        {
            // call base method?
            if (Combatant != null && Combatant.Alive && m_queen != null && m_queen.Combatant == null)
                m_queen.Combatant = Combatant;
        }

	  //Melee damage to controlled mobiles is multiplied by 10
	  public override void AlterMeleeDamageTo( Mobile to, ref int damage )
	  {
		if ( to is BaseCreature )
		{
			BaseCreature bc = (BaseCreature)to;

			if ( bc.Controlled )
			damage *= 10;
		}
	  }

	  //Melee damage from controlled mobiles is divided by 10
	  public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
	  {
		if ( from is BaseCreature )
		{
			BaseCreature bc = (BaseCreature)from;

			if ( bc.Controlled )
			damage /= 10;
		}
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
				case 0: m.Hits +=( Utility.Random( 40, 80 ) ); break;
				case 1: m.Hits += ( Utility.Random( 50, 90 ) ); break;
				case 2: m.Hits +=( Utility.Random( 100, 150 ) ); break;
			}
		from.PlaySound( 0x1F2 );
		from.SendMessage("Magic seems to heal this creature!!!");
		m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
	  }

        public UnicornPup(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_queen);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_queen = (UnicornQueenMOTM)reader.ReadMobile();
        }
    }
    #endregion
}