using System;
using System.Collections;
using Server;
using Server.Misc;
using Server.Items;
using Server.EventPrizeSystem;

namespace Server.Mobiles
{
    [CorpseName("a godly corpse")]
    public class DragChamp : BaseBoss
    {
	private IdolType m_Type;

        [Constructable]
        public DragChamp()
            : base(AIType.AI_Mage, FightMode.Closest)
        {
            Name = "Borkarth";
            Body = 0x3E;
            BaseSoundID = 362;
            Hue = 1152;

            SetStr(1000, 1000);
            SetDex(150, 150);
            SetInt(15000, 15000);

            SetHits(17000);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 45, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 225.1, 250.0);
            SetSkill(SkillName.EvalInt, 160.1, 200.0);
            SetSkill(SkillName.Magery, 115.5, 154.0);
            SetSkill(SkillName.Meditation, 225.1, 250.0);
            SetSkill(SkillName.MagicResist, 350.0, 400.0);
            SetSkill(SkillName.Tactics, 190.1, 200.0);
            SetSkill(SkillName.Wrestling, 75.0, 90.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;

            PackGold(6000, 8000);
            PackScroll(2, 8);
            PackArmor(2, 5);
            PackWeapon(3, 5);
            PackWeapon(5, 5);
            PackSlayer();

		m_Type = IdolType.DragonKing;

            World.Broadcast(0x35, true, "The Lord of the Brood has awoken and seeks to spread his corruption across the lands! You must discover his lair and vanquish him at all costs!");
        }

        public override bool BardImmune { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override int TreasureMapLevel { get { return 5; } }
        public override int Meat { get { return 1; } }
	public override bool CanDestroyObstacles { get { return true; } }

        public override bool HasBreath { get { return true; } }
        public override int BreathComputeDamage()
        {
            return (int)75;
        }

		public override bool DoDistributeTokens { get { return true; } }
		public override bool DoSpawnGoldOnDeath { get { return true; } }
		public override bool DoDetectHidden { get { return true; } }
		public override bool DoProvoPets { get { return true; } }
		public override int DoMoreDamageToPets { get { return 10; } }
		public override int CanCheckReflect{ get { return 1; } }


		public override void OnDeath(Container c)
        	{
			int cnt = 1;
            if ( Utility.Random( 4 ) < 1 ) cnt++;
            if ( Utility.Random( 5 ) < 1 ) cnt++;

            for (int i = 0; i < cnt; ++i)
            {
                switch (Utility.Random(5))
                {
                    case 0: c.DropItem(new SpecialHairDye()); break;
                    case 1: c.DropItem(new SpecialBeardDye()); break;
                    case 2: c.DropItem(new ClothingBlessDeed()); break;
                    case 3: c.DropItem(new NameChangeDeed()); break;
				case 4: c.DropItem(new WeddingDeed()); break;
                }
			}

            if (Utility.Random(5) < 1)
                switch (Utility.Random(5))
                {
                    case 0: c.DropItem(new LampPost1()); break;
                    case 1: c.DropItem(new LampPost2()); break;
                    case 2: c.DropItem(new LampPost3()); break;
                    case 3: c.DropItem(new CoveredChair()); break;
                    case 4: c.DropItem(new RuinedDrawers()); break;
                }

           if (Utility.Random(25) < 1)
                switch (Utility.Random(2))
               {
                    case 0: c.DropItem(new CursedClothingBlessDeed()); break;
                  case 1: c.DropItem(new HolyDeedofBlessing()); break;
               }

            if (Utility.Random(10) < 1)
                switch (Utility.Random(2))
                {
                    case 0: c.DropItem(new ChampionShirt()); break;
                    case 1: c.DropItem(new ChampionDoublet()); break;
                }

            if (Utility.Random(4) < 1)
                switch (Utility.Random(2))
                {
                    case 0: c.DropItem(new ChampionRing()); break;
                    case 1: c.DropItem(new ChampionNecklace()); break;
                }

			if (Utility.Random(2) < 1)
				c.DropItem(new Idol(m_Type));


            base.OnDeath(c);
        }

        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            if (from is PlayerMobile && from.Weapon != null)
            {
                if (from.Weapon is BaseSword ||
                    from.Weapon is BasePoleArm ||
                    from.Weapon is BaseSpear ||
                    from.Weapon is BaseAxe
                )
                    damage *= 3;
                else if (from.Weapon is BaseRanged)
                    damage *= 2;
            }
            else
            {
                if (from is BaseCreature)
                {
                    BaseCreature bc = (BaseCreature)from;
                    if (bc.Controlled || bc.Summoned || bc.BardTarget == this)
                        damage /= 10;
                }
            }
        }

        public override void OnDamagedBySpell(Mobile caster)
        {
            if (caster != this)
            {
                BaseCreature spawn = new DecayingWyvern(this);

                spawn.Team = this.Team;
                spawn.MoveToWorld(caster.Location, caster.Map);
                spawn.Combatant = caster;

                Say("*Borkarth summons a Wyvern to help him!*");
            }

            base.OnDamagedBySpell(caster);
        }

        public DragChamp(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}