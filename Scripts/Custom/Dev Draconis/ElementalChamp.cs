using System;
using System.Collections;
using Server;
using Server.Misc;
using Server.Items;
using Server.EventPrizeSystem;
using Xanthos.Evo;

namespace Server.Mobiles
{
    [CorpseName( "a godly corpse" )]
    public class ElementalChamp : BaseBoss
    {
	IdolType m_Type;

        [Constructable]
        public ElementalChamp()
            : base(AIType.AI_Mage, FightMode.Closest)
        {
            Name = "Thar'las";
            Body = 0xF;
            BaseSoundID = 274;

            SetStr(1100, 1190);
            SetDex(150, 160);
            SetInt(500, 600);

            SetHits(10000, 12000);
            SetMana(3450, 3650);

            SetDamage(47, 52);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 45, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 100.0, 175.0);
            SetSkill(SkillName.EvalInt, 80.1, 105.0);
            SetSkill(SkillName.Magery, 80.1, 95.0);
            SetSkill(SkillName.Meditation, 110.2, 150.0);
            SetSkill(SkillName.MagicResist, 220.0, 220.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 100;

            PackGold(600, 800);
            PackScroll(1, 8);
            PackSlayer();
            PackWeapon(2, 5);
            PackWeapon(4, 5);
            PackArmor(2, 5);
            PackArmor(4, 5);

			m_Type = IdolType.FireLord;

            World.Broadcast(0x35, true, "The eternal God of the Core has emerged from his fiery lair! Seek him out and destroy him!!");
        }

        public override bool BardImmune{ get{ return true; } }
        public override bool CanRummageCorpses{ get{ return true; } }
        public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
        public override int TreasureMapLevel{ get{ return 5; } }
        public override int Meat{ get{ return 1; } }
		public override bool CanDestroyObstacles { get { return true; } }

		public override bool DoDistributeTokens { get { return true; } }
		public override bool DoSpawnGoldOnDeath { get { return true; } }
		public override bool DoDetectHidden { get { return true; } }
		public override int DoLessDamageFromPets { get { return 2; } }

		public override void OnDeath( Container c )
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
			switch ( Utility.Random( 35 ) )
			{
				case 0: c.DropItem( new ChampionSandals() ); break;
				case 1: c.DropItem( new ChampionCloak() ); break;
			}

			if (Utility.Random(4) < 1)
			switch ( Utility.Random( 25 ) )
			{
				case 0: c.DropItem( new ChampionRing() ); break;
				case 1: c.DropItem( new ChampionNecklace() ); break;
			}

			if ( Utility.Random( 5 ) == 0 )
				c.DropItem( new Idol(m_Type) );


			base.OnDeath( c );
		}

		public override void OnGaveMeleeAttack(Mobile defender)
		{
			base.OnGaveMeleeAttack(defender);

			BaseCreature c = defender as BaseCreature;
			if ( c != null && c.Controlled && c.ControlMaster != null && (c is EvolutionDragon || c is RaelisDragon || c is EvoSpider || c is EvoHiryu) )
				ProvoEvo( c );
		}

		public override void OnGotMeleeAttack(Mobile attacker)
		{
			base.OnGotMeleeAttack(attacker);

			BaseCreature c = attacker as BaseCreature;

			if ( c != null && c.Controlled && c.ControlMaster != null && (c is EvolutionDragon || c is RaelisDragon || c is EvoSpider || c is EvoHiryu) )
			ProvoEvo( c );
				
		}

		public void ProvoEvo(Mobile evo)
		{
			BaseCreature bc = (BaseCreature)evo;

			bc.ControlTarget = bc.ControlMaster;
			bc.ControlOrder = OrderType.Attack;
			bc.Combatant = bc.ControlMaster;
			BaseBoss.ApplySkillLoss ( bc.ControlMaster );
		}

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
  		{
  			 if (from is EvolutionDragon || from is RaelisDragon || from is EvoSpider || from is EvoHiryu) 
  		 	 damage/=40;
  		}

		private DateTime m_NextActionTime;

		public override void OnThink()
		{
			if ( DateTime.Now >= m_NextActionTime )
			{
				Mobile combatant = this.Combatant;
                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 15) && this.Blessed == false)
                {
                    m_NextActionTime = DateTime.Now + TimeSpan.FromSeconds(90);

                    if ( Utility.RandomBool() )
						DoAtomicBomb(combatant, "Flee mortals or the eternal heat of the core will burn your bodies!");
					else
						DoSummon(combatant, "My burning Sons! Emerge from our fiery lair and vanquish these mortals!");
                }
			}

			base.OnThink();}

        private void DoAtomicBomb(Mobile combatant, string message)
        {
            this.Say(true, message);

                Mobile from = (Mobile)combatant;
                Map map = from.Map;

                if (map == null)
                    return;

                int count = 1;

                for (int i = 0; i < count; ++i)
                {
                    int x = from.X + Utility.RandomMinMax(-1, 1);
                    int y = from.Y + Utility.RandomMinMax(-1, 1);
                    int z = from.Z;

                    if (!map.CanFit(x, y, z, 16, false, true))
                    {
                        z = map.GetAverageZ(x, y);

                        if (z == from.Z || !map.CanFit(x, y, z, 16, false, true))
                            continue;
                    }

                    Atomic bomb = new Atomic();




                    bomb.MoveToWorld(new Point3D(x, y, z), map);
                }

        }

	private class InternalTimer : Timer
		{
			private Mobile m_Blessed;

			public InternalTimer( Mobile DoSummon ) : base( TimeSpan.FromSeconds( 60.0 ) )
			{
				Priority = TimerPriority.OneSecond;

				m_Blessed = DoSummon;
			}

			protected override void OnTick()
			{
				if (m_Blessed != null && !m_Blessed.Deleted)
                    m_Blessed.Blessed = false;
			}
		}

	private void DoSummon( Mobile combatant, string message )
	{
		Blessed = true;
        this.Freeze(TimeSpan.FromSeconds(60.0));
		new InternalTimer( this ).Start();

		Map map = this.Map;

			if ( map == null )
				return;

			int newSpawned = 6;

              		for ( int i = 0; i < newSpawned; ++i )
              		{
                 			BaseCreature spawn;

							if ( Utility.Random( 6 ) < 4 )
								spawn = new FireElemental();
							else
								spawn = new FireElemental(); //new GreaterFireElemental();

				spawn.Team = this.Team;
                    		spawn.Map = map;
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

				spawn.MoveToWorld( loc, map );
			}

	}
        public override void Damage(int amount, Mobile from)
        {
            if (from is PlayerMobile)
                amount /= 10;
            base.Damage(amount, from);
        }

        public ElementalChamp( Serial serial ) : base( serial )
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