using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;
using Server.EventPrizeSystem;
using Server.Engines.IdolSystem;

namespace Server.Mobiles
{
	public class DarkMaster : BaseMiniBoss
	{
		private Item m_GateItem;

		private class SpawnEntry
		{
			public Point3D m_Location;
			public Point3D m_Entrance;

			public SpawnEntry( Point3D loc, Point3D ent )
			{
				m_Location = loc;
				m_Entrance = ent;
			}
		}

		private static SpawnEntry[] m_Entries = new SpawnEntry[]
			{
				new SpawnEntry( new Point3D( 5284, 798, 0 ), new Point3D( 1176, 2638, 0 ) ),
				new SpawnEntry( new Point3D( 5751, 1297, 0 ), new Point3D( 5765, 2914, 34 ) ),
				new SpawnEntry( new Point3D( 5594, 241, 0 ), new Point3D( 509, 1569, 0 ) ),
				new SpawnEntry( new Point3D( 5390, 180, 10 ), new Point3D( 509, 1569, 0 ) ),
				new SpawnEntry( new Point3D( 5519, 898, 30 ), new Point3D( 1300, 1073, 0 ) ),
				new SpawnEntry( new Point3D( 5399, 786, 65 ), new Point3D( 1300, 1073, 0 ) ),
				new SpawnEntry( new Point3D( 5954, 227, 22 ), new Point3D( 4725, 3826, 0 ) ),
				new SpawnEntry( new Point3D( 6086, 179, 0 ), new Point3D( 4725, 3826, 0 ) ),
				new SpawnEntry( new Point3D( 5500, 2001, 0 ), new Point3D( 2502, 922, 0 ) ),
				new SpawnEntry( new Point3D( 5579, 1856, 0 ), new Point3D( 2502, 922, 0 ) )
			};

		private static bool ms_Active;

		public static bool Active { get { return ms_Active; } }


		public static DarkMaster Spawn( Point3D platLoc, Map platMap )
		{
			if ( IdolPedestal.DarkMasterActive() )
				return null;

			SpawnEntry entry = m_Entries[Utility.Random( m_Entries.Length )];

			DarkMaster darkmaster = new DarkMaster();

			darkmaster.MoveToWorld( entry.m_Location, Map.Felucca );

			darkmaster.m_GateItem = new DarkMasterGate( platLoc, platMap, entry.m_Entrance, Map.Felucca );

			return darkmaster;
		}

		[Constructable]
		public DarkMaster() : base()
		{
			ms_Active = true;

			Name = "Deimos";
			Title = "the Dark Master";
			Hue = 22222;
			BodyValue = 400;
			BaseSoundID = 1001;
			Team = 2;

			SetStr( 1000 );
			SetDex( 175 );
			SetInt( 50000 );

			SetHits( 50000 );

			SetDamage( 15, 15 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 25, 30 );
			SetResistance( ResistanceType.Cold, 25, 30 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.Anatomy, 125.0 );
			SetSkill( SkillName.EvalInt, 150.0 );
			SetSkill( SkillName.Magery, 175.0 );
			SetSkill( SkillName.Poisoning, 100.0 );
			SetSkill( SkillName.MagicResist, 200.0 );
			SetSkill( SkillName.Wrestling, 125.0 );
			SetSkill( SkillName.Tactics, 125.0 );
			SetSkill( SkillName.Meditation, 300.0 );
			SetSkill( SkillName.DetectHidden, 150.0 );

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 100;

			WizardsHat hat = new WizardsHat();
			hat.Hue = 1157;
			hat.LootType = LootType.Blessed;
			AddItem( hat );

			Sandals foot = new Sandals();
			foot.Hue = 1157;
			foot.LootType = LootType.Blessed;
			AddItem( foot );

			Robe robe = new Robe();
			robe.Hue = 22222;
			robe.LootType = LootType.Blessed;
			AddItem( robe );

			Cloak back = new Cloak();
			back.Hue = 1157;
			back.LootType = LootType.Blessed;
			AddItem ( back );

			Spellbook book = new Spellbook();
			book.Hue = 1157;
			book.Content = 18446744073709551615;
			book.Movable = false;
			AddItem( book );

			BodySash top = new BodySash();
			top.Hue = 1157;
			top.Layer = Layer.Earrings;
			top.LootType = LootType.Blessed;
			AddItem( top );

		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool BardImmune{ get{ return true;} }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool CanDestroyObstacles { get { return true; } }
		public override bool AutoDispel { get { return true; } }
		public override bool DoDarkMasterMorph { get { return true; } }
		public override int DoMoreDamageToPets { get { return 5; } }
		public override int DoLessDamageFromPets { get { return 10; } }
		public override bool DoEarthquake { get { return true; } }
		public override int CanCastReflect{ get { return 120; } }

		public override void OnGotMeleeAttack(Mobile attacker)
		{
			base.OnGotMeleeAttack(attacker);

			if (0.05 >= Utility.RandomDouble())
			{
				ClonedDarkMaster clone = new ClonedDarkMaster( this );
				clone.Team = this.Team;
				clone.Combatant = attacker;
				clone.MoveToWorld( attacker.Location, attacker.Map );
			}
		}

		public override void OnGaveMeleeAttack(Mobile defender)
		{
			base.OnGaveMeleeAttack(defender);

			if (0.05 >= Utility.RandomDouble())
			{
				ClonedDarkMaster clone = new ClonedDarkMaster( this );
				clone.Team = this.Team;
				clone.Combatant = defender;
				clone.MoveToWorld( defender.Location, defender.Map );
			}
		}

		public override void OnDamagedBySpell( Mobile caster )
		{
			if ( caster.InRange( this, 8 ) )
			{
				caster.MovingParticles( this, 0x36F4, 1, 0, false, false, 32, 0, 9535, 1,    0, (EffectLayer)255, 0x100 );
				caster.MovingParticles( this, 0x0001, 1, 0, false,  true, 32, 0, 9535, 9536, 0, (EffectLayer)255, 0 );
				this.PlaySound( 0x209 );

				caster.Mana -= caster.Mana;
				caster.Hits -= caster.Hits;
				caster.Stam -= caster.Stam;
				this.Mana += caster.Mana;
				this.Hits += caster.Hits;
				this.Stam += caster.Stam;
			}

			base.OnDamagedBySpell( caster );
		}

		public DarkMaster( Serial serial ) : base( serial )
		{
			ms_Active = true;
		}

		public override void OnAfterDelete()
		{
			ms_Active = false;
			base.OnAfterDelete();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_GateItem );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_GateItem = reader.ReadItem();
		}
	}

	[CorpseName( "the glowing remains of a god" )]
	public class EthyDarkMaster : BaseMiniBoss
	{
		private Timer m_Timer;
		private static bool ms_Active;
		public static bool Active { get { return ms_Active; } }

		//[Constructable]
		public EthyDarkMaster() : base()
		{
			ms_Active = true;
			Name = "Deimos";
			Title = "the Dark Master";
			Hue = 22222;
			BodyValue = 400;
			BaseSoundID = 1001;
			Team = 2;

			SetStr( 1500 );
			SetDex( 250 );
			SetInt( 50000 );

			SetHits( 50000 );

			SetDamage( 30, 30 );

			SetSkill( SkillName.Anatomy, 150.0 );
			SetSkill( SkillName.EvalInt, 200.0 );
			SetSkill( SkillName.Magery, 220.0 );
			SetSkill( SkillName.Poisoning, 200.0 );
			SetSkill( SkillName.MagicResist, 250.0 );
			SetSkill( SkillName.Wrestling, 150.0 );
			SetSkill( SkillName.Tactics, 150.0 );
			SetSkill( SkillName.Meditation, 600.0 );
			SetSkill( SkillName.DetectHidden, 200.0 );

			Fame = 50000;
			Karma = -50000;

			VirtualArmor = 200;

			WizardsHat hat = new WizardsHat();
			hat.Hue = 1150;
			hat.LootType = LootType.Blessed;
			AddItem( hat );

			Sandals foot = new Sandals();
			foot.Hue = 1150;
			foot.LootType = LootType.Blessed;
			AddItem( foot );

			Robe robe = new Robe();
			robe.Hue = 22222;
			robe.LootType = LootType.Blessed;
			AddItem( robe );

			Cloak back = new Cloak();
			back.Hue = 1150;
			back.LootType = LootType.Blessed;
			AddItem ( back );

			Spellbook book = new Spellbook();
			book.Hue = 1150;
			book.Content = 18446744073709551615;
			book.Movable = false;
			AddItem( book );

			BodySash top = new BodySash();
			top.Hue = 1150;
			top.Layer = Layer.Earrings;
			top.LootType = LootType.Blessed;
			AddItem( top );

			m_Timer = new AppearTimer( this );
			m_Timer.Start();
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool BardImmune{ get{ return true;} }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool CanDestroyObstacles { get { return true; } }
		public override bool AutoDispel { get { return true; } }
		public override bool DoEightLeech{ get { return true; } }
		public override int DoMoreDamageToPets { get { return 5; } }
		public override int DoLessDamageFromPets { get { return 15; } }
		public override bool DoEarthquake { get { return true; } }
		public override int CanCastReflect{ get { return 120; } }

		private class AppearTimer : Timer
		{
			private Mobile m_Unhide;

			public AppearTimer( Mobile unhide ) : base( TimeSpan.FromSeconds( 30.0 ))
			{
				Priority = TimerPriority.OneSecond;
				m_Unhide = unhide;
			}

			protected override void OnTick()
			{
				m_Unhide.Hidden = false;
				m_Unhide.Blessed = false;
			}
		}

		public override void OnGotMeleeAttack(Mobile attacker)
		{
			base.OnGotMeleeAttack(attacker);

			if (0.20 >= Utility.RandomDouble())
			{
				ClonedDarkMaster clone = new ClonedDarkMaster( this );
				clone.Team = this.Team;
				clone.Combatant = attacker;
				clone.MoveToWorld( attacker.Location, attacker.Map );
			}
		}

		public override void OnGaveMeleeAttack(Mobile defender)
		{
			base.OnGaveMeleeAttack(defender);

			if (0.10 >= Utility.RandomDouble())
				if (0.05 >= Utility.RandomDouble())
			{
				ClonedDarkMaster clone = new ClonedDarkMaster( this );
				clone.Team = this.Team;
				clone.Combatant = defender;
				clone.MoveToWorld( defender.Location, defender.Map );
			}
		}

		public override void OnDeath(Container c)
		{
			if ( Utility.Random( 10 ) == 0 )
				c.DropItem( new LayerSashDeed() );

			if ( Utility.Random( 250 ) == 0 )
				c.DropItem(new SpecialEtherealMountDeed());

			if (Utility.Random(20) < 1) c.DropItem( new ChampionHat() ) ;
			if (Utility.Random(500) < 1) c.DropItem( new ChampionShroud() );

			MysticKeySinglePart fp = new MysticKeySinglePart( 6 );
			fp.Movable = true;
			c.DropItem( fp );

			switch ( Utility.Random( 100 ) )
			{
				case 0: c.DropItem(new ChampionCloak()); break;
				case 1: c.DropItem(new ChampionDoublet()); break;
				case 2: c.DropItem(new ChampionKilt()); break;
				case 3: c.DropItem(new ChampionNecklace()); break;
				case 4: c.DropItem(new ChampionPants()); break;
				case 5: c.DropItem(new ChampionRing()); break;
				case 6: c.DropItem(new ChampionSandals()); break;
				case 7: c.DropItem(new ChampionShirt()); break;
			}

			int bonus = 0;

			for (int cnt = 3;cnt < 0; cnt--)
			{
				if ( bonus < 3 && Utility.Random(4) == 0 )
				{
					bonus++;
					cnt++;
				}

				switch (Utility.Random(4))
				{
					case 0: c.DropItem(new SpecialHairDye()); break;
					case 1: c.DropItem(new SpecialBeardDye()); break;
					case 2: c.DropItem(new ClothingBlessDeed()); break;
					case 3: c.DropItem(new NameChangeDeed()); break;
				}
			}

			ms_Active = false;
			base.OnDeath(c);
		}

		public void GreaterTokens()
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

			for (int i = 0; i < 72; ++i)
			{
				Mobile m = (Mobile)toGive[i % toGive.Count];

				if (Utility.Random(72) < 4)
				{
					m.AddToBackpack(new GoldenPrizeToken());
					m.SendMessage("You have received a gold token!");
				}
				else if (Utility.Random(72) < 19)
				{
					m.AddToBackpack(new SilverPrizeToken());
					m.SendMessage("You have received a silver token!");
				}
				else if (Utility.Random(72) < 34)
				{
					m.AddToBackpack(new BronzePrizeToken());
					m.SendMessage("You have received a bronze token!");
				}
			}
		}

		public void HarrowerTicket()
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

			for ( int i = 0; i < 6; ++i )
			{
				Mobile m = (Mobile)toGive[i % toGive.Count];

				m.AddToBackpack( new HarrowerTicket() );
				m.SendMessage( "You have received a super champion deed!" );
			}
		}

		public override bool OnBeforeDeath()
		{
			if ( !NoKillAwards )
			{
				GreaterTokens();
				HarrowerTicket();

				Map map = this.Map;

				if ( map != null )
				{
					for ( int x = -6; x <= 6; ++x )
					{
						for ( int y = -6; y <= 6; ++y )
						{
							double dist = Math.Sqrt(x*x+y*y);

							if ( dist <= 6 )
								new GoodiesTimer( map, X + x, Y + y ).Start();
						}
					}
				}
			}
			return base.OnBeforeDeath();
		}

		public override void OnAfterDelete()
		{
			ms_Active = false;
			base.OnAfterDelete();
		}

		private class GoodiesTimer : Timer
		{
			private Map m_Map;
			private int m_X, m_Y;

			public GoodiesTimer( Map map, int x, int y ) : base( TimeSpan.FromSeconds( Utility.RandomDouble() * 10.0 ) )
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

				Gold g = new Gold( 1000, 1500 );

				g.MoveToWorld( new Point3D( m_X, m_Y, z ), m_Map );

				switch ( Utility.Random( 6 ) )
				{
					case 0: // Fire column
					{
						Effects.SendLocationParticles( EffectItem.Create( g.Location, g.Map, EffectItem.DefaultDuration ), 0x3709, 10, 30, 5052 );
						Effects.PlaySound( g, g.Map, 0x208 );

						break;
					}
					case 1: // Explosion
					{
						Effects.SendLocationParticles( EffectItem.Create( g.Location, g.Map, EffectItem.DefaultDuration ), 0x36BD, 20, 10, 5044 );
						Effects.PlaySound( g, g.Map, 0x307 );

						break;
					}
					case 2: // Ball of fire
					{
						Effects.SendLocationParticles( EffectItem.Create( g.Location, g.Map, EffectItem.DefaultDuration ), 0x36FE, 10, 10, 5052 );

						break;
					}
				}
			}
		}

		public EthyDarkMaster( Serial serial ) : base( serial )
		{
			ms_Active = true;
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

	public class EthyDragChamp : BaseMiniBoss
	{
		private Timer m_Timer;
		private static bool ms_Active;
		public static bool Active { get { return ms_Active; } }

		//[Constructable]
		public EthyDragChamp() : base ()
		{
			ms_Active = true;
			Hue = 22222;
			Name = "Master of the Brood";
			Kills = 5;
				Body = 0x3E;
					BaseSoundID = 362;

			SetStr( 1000, 1000 );
					SetDex( 150, 150 );
			SetHits( 50000 );
			SetInt( 50000 );

			SetDamage( 10, 15 );

			SetSkill( SkillName.Anatomy, 225.1, 250.0 );
					SetSkill( SkillName.EvalInt, 160.1, 200.0 );
					SetSkill( SkillName.Magery, 115.5, 154.0 );
					SetSkill( SkillName.Meditation, 225.1, 250.0 );
					SetSkill( SkillName.MagicResist, 350.0, 400.0 );
					SetSkill( SkillName.Tactics, 190.1, 200.0 );
					SetSkill( SkillName.Wrestling, 190.1, 200.0 );

			m_Timer = new AppearTimer( this );
			m_Timer.Start();

			VirtualArmor = 60;
		}

		private class AppearTimer : Timer
		{
			private Mobile m_Unhide;

			public AppearTimer( Mobile unhide ) : base( TimeSpan.FromSeconds( 30.0 ))
			{
				Priority = TimerPriority.OneSecond;
				m_Unhide = unhide;
			}

			protected override void OnTick()
			{
				m_Unhide.Hidden = false;
				m_Unhide.Blessed = false;
			}
		}

		public override bool DoDarkMasterMorph { get { return true; } }
		public override bool DoSpawnWyvern{ get { return true; } }
		public override int CanCheckReflect{ get { return 1; } }
		public override int DoWeaponsDoMoreDamage{ get { return 3; } }
		public override bool DoProvoPets { get { return true; } }
		public override int DoMoreDamageToPets { get { return 10; } }
		public override int DoLessDamageFromPets { get { return 10; } }
		public override bool HasBreath{ get{ return true; } }
		public override int BreathComputeDamage()
		{
			return (int)75;
		}

		public EthyDragChamp(Serial serial) : base(serial)
		{
			ms_Active = true;
		}

		public override void OnAfterDelete()
		{
			ms_Active = false;
			base.OnAfterDelete();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
			m_Timer = new AppearTimer( this );
					m_Timer.Start();
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class EthyElementalChamp : BaseMiniBoss
	{
		private Timer m_Timer;
		private static bool ms_Active;
		public static bool Active { get {return ms_Active;}}

		//[Constructable]
		public EthyElementalChamp() : base ()
		{
			ms_Active = true;
			Hue = 22222;
			Name = "Master of the Elements";
			Body = 0xF;
					BaseSoundID = 274;

					SetStr(1100, 1190);
					SetDex(150, 160);
			SetHits( 50000 );
			SetInt( 50000 );

			SetDamage(47, 52);

			SetSkill(SkillName.Anatomy, 100.0, 175.0);
					SetSkill(SkillName.EvalInt, 80.1, 105.0);
					SetSkill(SkillName.Magery, 80.1, 95.0);
					SetSkill(SkillName.Meditation, 110.2, 150.0);
					SetSkill(SkillName.MagicResist, 220.0, 220.0);
					SetSkill(SkillName.Tactics, 90.1, 100.0);
					SetSkill(SkillName.Wrestling, 90.1, 100.0);

			m_Timer = new AppearTimer( this );
			m_Timer.Start();

			VirtualArmor = 80;
		}

		private class AppearTimer : Timer
		{
			private Mobile m_Unhide;

			public AppearTimer( Mobile unhide ) : base( TimeSpan.FromSeconds( 30.0 ))
			{
				Priority = TimerPriority.OneSecond;
				m_Unhide = unhide;
			}

			protected override void OnTick()
			{
				m_Unhide.Hidden = false;
				m_Unhide.Blessed = false;
			}
		}

		public override bool DoDarkMasterMorph { get { return true; } }
		public override bool DoElementalChamp{ get { return true; } }
		public override int DoLessDamageFromPets { get { return 2; } }

		public EthyElementalChamp(Serial serial) : base(serial)
		{
			ms_Active = true;
		}

		public override void OnAfterDelete()
		{
			ms_Active = false;
			base.OnAfterDelete();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
			m_Timer = new AppearTimer( this );
					m_Timer.Start();
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class EthyLichChamp : BaseMiniBoss
	{
		private Timer m_Timer;
		private static bool ms_Active;
		public static bool Active { get { return ms_Active; } }

		//[Constructable]
		public EthyLichChamp() : base ()
		{
			ms_Active = true;
			Hue = 22222;
			Name = "Master of the Dead";
			Body = 0x4F;
					Hue = 1175;

					SetStr( 650, 1000 );
					SetDex( 150, 200 );
			SetHits( 50000 );
			SetInt( 50000 );

			 SetDamage( 23, 27 );

			SetSkill( SkillName.Meditation, 350.0, 350.0 );
					SetSkill( SkillName.EvalInt, 190.1, 210.0 );
					SetSkill( SkillName.Magery, 175.1, 190.0 );
					SetSkill( SkillName.MagicResist, 115, 120.0 );
					SetSkill( SkillName.Tactics, 100.0, 100.0 );
					SetSkill( SkillName.Wrestling, 135.0, 155.0 );

			m_Timer = new AppearTimer( this );
			m_Timer.Start();

			VirtualArmor = 50;
		}

		private class AppearTimer : Timer
		{
			private Mobile m_Unhide;

			public AppearTimer( Mobile unhide ) : base( TimeSpan.FromSeconds( 30.0 ))
			{
				Priority = TimerPriority.OneSecond;
				m_Unhide = unhide;
			}

			protected override void OnTick()
			{
				m_Unhide.Hidden = false;
				m_Unhide.Blessed = false;
			}
		}

		public override int DoPolymorphOnGaveMelee { get { return 51; } }
		public override int DoPolymorphHue { get { return 1175; } }
		public override bool DoSkillLoss{ get { return true; } }
		public override bool DoDarkMasterMorph { get { return true; } }
		public override bool DoLeechLife { get { return true; } }
		public override bool DoEarthquake { get { return true; } }
		public override bool DoSpawnEvil{ get { return true; } }

		public EthyLichChamp(Serial serial) : base(serial)
		{
			ms_Active = true;
		}

		public override void OnAfterDelete()
		{
			ms_Active = false;
			base.OnAfterDelete();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
			m_Timer = new AppearTimer( this );
					m_Timer.Start();
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class ClonedDarkMaster : BaseMiniBoss
	{
		private Mobile m_Owner;
		[Constructable]
		public ClonedDarkMaster( Mobile owner ) : base()
		{
			Name = "Deimos";
			Title = "the Dark Master";
			Hue = 22222;
			BodyValue = 400;
			BaseSoundID = 1001;

			SetStr( 500 );
			SetDex( 100 );
			SetInt( 1500 );

			SetHits( 500 );

			SetDamage( 10, 15 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 25, 30 );
			SetResistance( ResistanceType.Cold, 25, 30 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.Anatomy, 100.0 );
			SetSkill( SkillName.EvalInt, 100.0 );
			SetSkill( SkillName.Magery, 115.0 );
			SetSkill( SkillName.Poisoning, 100.0 );
			SetSkill( SkillName.MagicResist, 150.0 );
			SetSkill( SkillName.Wrestling, 105.0 );
			SetSkill( SkillName.Tactics, 105.0 );
			SetSkill( SkillName.Meditation, 200.0 );

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 100;

			m_Owner = owner;

			WizardsHat hat = new WizardsHat();
			hat.Hue = 1157;
			hat.LootType = LootType.Blessed;
			AddItem( hat );

			Sandals foot = new Sandals();
			foot.Hue = 1157;
			foot.LootType = LootType.Blessed;
			AddItem( foot );

			Robe robe = new Robe();
			robe.Hue = 22222;
			robe.LootType = LootType.Blessed;
			AddItem( robe );

			Cloak back = new Cloak();
			back.Hue = 1157;
			back.LootType = LootType.Blessed;
			AddItem ( back );

			Spellbook book = new Spellbook();
			book.Hue = 1157;
			book.Content = 18446744073709551615;
			book.Movable = false;
			AddItem( book );

			BodySash top = new BodySash();
			top.Hue = 1157;
			top.Layer = Layer.Earrings;
			top.LootType = LootType.Blessed;
			AddItem( top );

		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool BardImmune{ get{ return true;} }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool CanDestroyObstacles { get { return true; } }
		public override bool AutoDispel { get { return true; } }
		public override int DoMoreDamageToPets { get { return 5; } }
		public override int DoLessDamageFromPets { get { return 10; } }


		public override void OnThink()
		{
			Map map = this.Map;

			if ( m_Owner != null && m_Owner.Alive && !InRange( m_Owner, 12 ) )
			{
				Point3D from = this.Location;
							Point3D to = m_Owner.Location;

							this.Location = to;
							this.ProcessDelta();

							Effects.SendLocationParticles(EffectItem.Create(from, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
							Effects.SendLocationParticles(EffectItem.Create(to, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);

							Effects.PlaySound(to, map, 0x1FE);
			}

			if ( m_Owner.Hits < 1 )
			{
				this.Delete();
			}
			else
			{
			base.OnThink();
			}
		}

		public ClonedDarkMaster( Serial serial ) : base( serial )
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