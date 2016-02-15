using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Engines.IdolSystem;
using Server.EventPrizeSystem;

namespace Server.Mobiles
{
	[CorpseName( "an idol keeper's corpse" )]
	public abstract class BaseMiniBoss : BaseBoss
	{
		public BaseMiniBoss() : base( AIType.AI_Mage )
		{
            		setup();
        	}

		public BaseMiniBoss(AIType aiType) : this(aiType, FightMode.Closest)
		{
            		setup();
        	}

		public BaseMiniBoss(AIType aiType, FightMode mode) : base(aiType, mode)
		{
            		setup();
        	}

		public BaseMiniBoss( Serial serial ) : base( serial )
		{
		}

        	private void setup()
        	{
            		Name = "Idol Keeper";
            		Kills = 5;
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

		public override void OnDeath( Container c )
		{
            		if (Utility.Random(200) < 1)
            		{
               			UncutCloth cloth = new UncutCloth();
                		cloth.Hue = 1156;
                		c.DropItem(cloth);
            		}
			base.OnDeath( c );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 3 );
		}

		public virtual bool DoElementalChamp{ get { return false; } }
		public virtual bool DoEightLeech{ get { return false; } }
		public virtual bool DoDarkMasterMorph { get { return false; } }
        	public virtual bool DoSpawnWyvern { get { return false; } }

		private DateTime m_NextEightLeech;
		private DateTime m_NextElementalChampAbility;


        	public override void OnDamagedBySpell(Mobile caster)
        	{
            		base.OnDamagedBySpell(caster);
            		if ((DoSpawnWyvern && caster != this))
			{
				BaseCreature spawn = new DecayingWyvern( this );

				spawn.Team = this.Team;
				spawn.MoveToWorld( caster.Location, caster.Map );
				spawn.Combatant = caster;

				Say("*{0} summons a Wyvern to help him!*",Name);
			}
        	}

		public override void OnGaveMeleeAttack(Mobile defender)
		{
            		if (DoElementalChamp && Utility.Random(2) < 1)
            		{
            			Map map = this.Map;

				if ( map != null )
				{
					// try 10 times to find a teleport spot
                    			for (int i = 0; i < 10; ++i)
                    			{
                        			int x = X + (Utility.RandomMinMax(-10, 15));
                        			int y = Y + (Utility.RandomMinMax(-10, 15));
                        			int z = Z;

                        			if (!map.CanFit(x, y, z, 16, false, false))
                            			continue;

                        			Point3D from = defender.Location;
                        			Point3D to = new Point3D(x, y, z);

                        			if (!InLOS(to))
                            			continue;

                        			defender.Location = to;
                        			defender.ProcessDelta();
                        			defender.Combatant = null;
                        			defender.Freeze(TimeSpan.FromSeconds(5.0));

                        			Effects.SendLocationParticles(EffectItem.Create(from, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
                        			Effects.SendLocationParticles(EffectItem.Create(to, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);

                        			Effects.PlaySound(to, map, 0x1FE);
                        			DoECTeleport(this);
                    			}
                		}
			}
			base.OnGaveMeleeAttack(defender);
		}

		public override void OnThink()
		{
            		if (DoElementalChamp && DateTime.Now >= m_NextElementalChampAbility)
			{
				Mobile combatant = this.Combatant;

                		if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 15) && this.Blessed == false)
               	 		{
                   			m_NextElementalChampAbility = DateTime.Now + TimeSpan.FromSeconds(90);

                    			int ability = Utility.Random(2);

                    			switch (ability)
                    			{
                        			case 0: DoAtomicBomb(combatant, "Flee mortals or the eternal heat of the Core will burn your bodies!"); break;
                        			case 1: DoSummon(combatant, "My burning sons! Emerge from our fiery lair and vanquish these mortals!"); break;
                    			}
                		}
			}

            		if (DoEightLeech && DateTime.Now >= m_NextEightLeech)
			{
				Mobile combatant = this.Combatant;

				if ( combatant != null && combatant.Map == this.Map && combatant.InRange( this, 15 ) )
				{
					m_NextEightLeech = DateTime.Now + TimeSpan.FromMinutes( Utility.RandomMinMax( 4, 6 ) );

					int ability = Utility.Random( 3 );

					switch ( ability )
					{
						case 0: EightLeech( combatant, "Time to die, mortals!" ); break;
						case 1: EightLeech( combatant, "Come to me my prey!" ); break;
						case 2: EightLeech( combatant, "Now is the time of demise!" ); break;
					}
				}
			}

			if ( DoDarkMasterMorph )
			{
				if ( this.Int == 10000 && this.Hits < this.Int - 5000 )
				{
					Map map = this.Map;

					if ( map == null )
					return;

					int newSpawned = 25;

                    for ( int i = 0; i < newSpawned; ++i )
                    {
	                    ShadowMinion spawn = new ShadowMinion();

                        spawn.Team = this.Team;
                        spawn.Map = map;
                        bool validLocation = false;
                        Point3D loc = this.Location;

                        for ( int j = 0; !validLocation && j < 10; ++j )
                        {
                            int x = X + Utility.Random( 5 );
                            int y = Y + Utility.Random( 5 );
                            int z = map.GetAverageZ( x, y );

                            if ( validLocation = map.CanFit( x, y, this.Z, 16, false, false ) )
	                            loc = new Point3D( x, y, Z );
                            else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
	                            loc = new Point3D( x, y, z );
                        }

                        spawn.MoveToWorld( loc, map );
					}
					EthyDarkMaster ethy = new EthyDarkMaster();
					ethy.Hits = 7500;
					ethy.Team = this.Team;
					ethy.Hidden = true;
					ethy.Blessed = true;
					ethy.Freeze(TimeSpan.FromSeconds(30.0));
					ethy.MoveToWorld( this.Location, this.Map );
					this.Delete();
				}

				else if ( this.Int != 5000 && this.Hits < this.Int - 5000 )
				{
					this.Hidden = true;
					this.Freeze(TimeSpan.FromSeconds(30.0));
					int ability = Utility.Random( 3 );

					if ( this.BodyValue == 400 )
					{
						switch ( ability )
						{
							case 0: DoEthyThar(); break;
							case 1: DoEthyBork(); break;
							case 2: DoEthyLich(); break;
						}
					} else if ( this.BodyValue == 0x3E ) {
						switch ( ability )
						{
							case 0: DoEthyThar(); break;
							case 1: DoMaster(); break;
							case 2: DoEthyLich(); break;
						}
					} else if ( this.BodyValue == 0xF ) {
						switch ( ability )
						{
							case 0: DoMaster(); break;
							case 1: DoEthyBork(); break;
							case 2: DoEthyLich(); break;
						}
					} else if ( this.BodyValue == 0x4F ) {
						switch ( ability )
						{
							case 0: DoEthyThar(); break;
							case 1: DoEthyBork(); break;
							case 2: DoMaster(); break;
						}
					}
				}
			}
			base.OnThink();
		}

		public void DoEthyThar()
		{
			Map map = this.Map;

			if ( map == null )
				return;

			int newSpawned = 25;

            for ( int i = 0; i < newSpawned; ++i )
            {
                FieryMinion spawn = new FieryMinion();

                spawn.Team = this.Team;
                spawn.Map = map;
                bool validLocation = false;
                Point3D loc = this.Location;

                for ( int j = 0; !validLocation && j < 10; ++j )
                {
                    int x = X + Utility.Random( 5 );
                    int y = Y + Utility.Random( 5 );
                    int z = map.GetAverageZ( x, y );

                    if ( validLocation = map.CanFit( x, y, this.Z, 16, false, false ) )
                        loc = new Point3D( x, y, Z );
                    else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
                        loc = new Point3D( x, y, z );
                }

				spawn.MoveToWorld( loc, map );
			}
			EthyElementalChamp ethy = new EthyElementalChamp();
			ethy.Hits = this.Hits;
			ethy.Int = this.Int - 5000;
			ethy.Team = this.Team;
			ethy.Hidden = true;
			ethy.Blessed = true;
			ethy.Freeze(TimeSpan.FromSeconds(30.0));
			ethy.MoveToWorld( this.Location, this.Map );
			this.Delete();
		}

		public void DoEthyBork()
		{
			Map map = this.Map;

			if ( map == null )
				return;

			int newSpawned = 25;

      		for ( int i = 0; i < newSpawned; ++i )
      		{
         		IcyMinion spawn = new IcyMinion();

		        spawn.Team = this.Team;
            		        spawn.Map = map;
		        bool validLocation = false;
		        Point3D loc = this.Location;

		        for ( int j = 0; !validLocation && j < 10; ++j )
		        {
			        int x = X + Utility.Random( 5 );
			        int y = Y + Utility.Random( 5 );
			        int z = map.GetAverageZ( x, y );

			        if ( validLocation = map.CanFit( x, y, this.Z, 16, false, false ) )
				        loc = new Point3D( x, y, Z );
			        else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
				        loc = new Point3D( x, y, z );
		        }

				spawn.MoveToWorld( loc, map );
        	}
			EthyDragChamp ethy = new EthyDragChamp();
			ethy.Hits = this.Hits;
			ethy.Int = this.Int - 5000;
			ethy.Hidden = true;
			ethy.Team = this.Team;
			ethy.Blessed = true;
			ethy.Freeze(TimeSpan.FromSeconds(30.0));
			ethy.MoveToWorld( this.Location, this.Map );
			this.Delete();
		}

		public void DoEthyLich()
		{
			Map map = this.Map;

			if ( map == null )
				return;

			int newSpawned = 25;

      		for ( int i = 0; i < newSpawned; ++i )
      		{
         		FadingMinion spawn = new FadingMinion();

		        spawn.Team = this.Team;
            		        spawn.Map = map;
		        bool validLocation = false;
		        Point3D loc = this.Location;

		        for ( int j = 0; !validLocation && j < 10; ++j )
		        {
			        int x = X + Utility.Random( 5 );
			        int y = Y + Utility.Random( 5 );
			        int z = map.GetAverageZ( x, y );

			        if ( validLocation = map.CanFit( x, y, this.Z, 16, false, false ) )
				        loc = new Point3D( x, y, Z );
			        else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
				        loc = new Point3D( x, y, z );
		        }

		        spawn.MoveToWorld( loc, map );
			}
			EthyLichChamp ethy = new EthyLichChamp();
			ethy.Hits = this.Hits;
			ethy.Int = this.Int - 5000;
			ethy.Hidden = true;
			ethy.Team = this.Team;
			ethy.Blessed = true;
			ethy.Freeze(TimeSpan.FromSeconds(30.0));
			ethy.MoveToWorld( this.Location, this.Map );
			this.Delete();
		}

		public void DoMaster()
		{
			Map map = this.Map;

			if ( map == null )
				return;

			int newSpawned = 25;

      		for ( int i = 0; i < newSpawned; ++i )
      		{
         		ShadowMinion spawn = new ShadowMinion();

		        spawn.Team = this.Team;
            		        spawn.Map = map;
		        bool validLocation = false;
		        Point3D loc = this.Location;

		        for ( int j = 0; !validLocation && j < 10; ++j )
		        {
			        int x = X + Utility.Random( 5 );
			        int y = Y + Utility.Random( 5 );
			        int z = map.GetAverageZ( x, y );

			        if ( validLocation = map.CanFit( x, y, this.Z, 16, false, false ) )
				        loc = new Point3D( x, y, Z );
			        else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
				        loc = new Point3D( x, y, z );
		        }

				spawn.MoveToWorld( loc, map );
			}
			DarkMaster ethy = new DarkMaster();
			ethy.Hits = this.Hits;
			ethy.Int = this.Int - 5000;
			ethy.Hidden = true;
			ethy.Team = this.Team;
			ethy.Blessed = true;
			ethy.Freeze(TimeSpan.FromSeconds(30.0));
			ethy.MoveToWorld( this.Location, this.Map );
			this.Delete();
		}

		private void EightLeech( Mobile combatant, string message )
		{
			this.Say( true, message );

			Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( DoEightLeech_Stage1 ), combatant );
		}

		private int valuex;
		private int valuey;

		private void DoEightLeech_Stage1( object state )
		{
			Mobile combatant = (Mobile)state;
			Map map = this.Map;

			if ( this.CanBeHarmful( combatant ) )
			{
				ArrayList list = new ArrayList();

			    foreach ( Mobile m in this.GetMobilesInRange( 15 ) )
			    {
				    if ( this.CanBeHarmful( m ) && this.IsEnemy( m ) )
					    list.Add( m );
			    }

			    if ( list.Count == 0 )
			    {
				    this.Say( true, "So you elude my power...So be it!" );
			    }
			    else if ( list.Count <= 8 )
			    {
				    for ( int i = 0; i < list.Count; ++i )
				    {
					    Mobile m = (Mobile)list[i];

					    int pos = Utility.Random( 8 );
                        switch ( pos )
					    {
						    case 0: valuex = -4; valuey = 0; break;
						    case 1: valuex = -3; valuey = 3; break;
						    case 2: valuex = 0; valuey = 4; break;
						    case 3: valuex = 3; valuey = 3; break;
						    case 4: valuex = 4; valuey = 0; break;
						    case 5: valuex = 3; valuey = -3; break;
						    case 6: valuex = 0; valuey = -4; break;
						    case 7: valuex = -3; valuey = -3; break;
					    }

					    int x = this.X + valuex;
					    int y = this.Y + valuey;
					    int z = Z;

            		    if (!map.CanFit(x, y, z, 16, false, false))
                		    continue;

            		    Point3D from = m.Location;
            		    Point3D to = new Point3D(x, y, z);

            		    if (!InLOS(to))
                		    continue;

            		    m.Location = to;
            		    m.ProcessDelta();
            		    m.Combatant = null;
            		    m.Freeze(TimeSpan.FromSeconds(6.0));
		                this.Freeze(TimeSpan.FromSeconds(6.0));

            		    Effects.SendLocationParticles(EffectItem.Create(from, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
            		    Effects.SendLocationParticles(EffectItem.Create(to, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);

            		    Effects.PlaySound(to, map, 0x1FE);
            		    Timer.DelayCall( TimeSpan.FromSeconds( 5.0 ), new TimerStateCallback( DoEightLeech_Stage2 ), m );
				    }

				    this.Say( true, "Now you shall feel my power!" );
			    }
			    else if ( list.Count > 8 )
			    {
				    this.Say( true, "There are too many of you, so I will destory one of you!");

				    if ( this.CanBeHarmful( combatant ) )
				    {
					    this.MovingParticles( combatant, 0x379F, 7, 0, false, true, 3043, 4043, 0x211 );
					    this.PlaySound( 0x20A );

					    this.DoHarmful( combatant );
					    combatant.Hits -= 150;
				    }
			    }
			}
		}

		private void DoEightLeech_Stage2( object state )
		{
			Mobile combatant = (Mobile)state;
			this.Say( true, "Muhahaha, pathetic creatures!" );

			foreach ( Mobile m in this.GetMobilesInRange( 4 ) )
			{
                if (CanBeHarmful(m))
                {
                    DoHarmful(m);
                    m.MovingParticles(this, 0x36F4, 1, 0, false, false, 32, 0, 9535, 1, 0, (EffectLayer)255, 0x100);
                    m.MovingParticles(this, 0x0001, 1, 0, false, true, 32, 0, 9535, 9536, 0, (EffectLayer)255, 0);
                    this.PlaySound(0x209);
                    m.Hits -= 70;
                    this.Hits += 70;
                }
			}
		}

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
               	BaseCreature spawn = new FireElemental();

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

		private static int[] m_Offsets = new int[]
			{
				-1, -1,
				-1,  0,
				-1,  1,
				0, -1,
				0,  1,
				1, -1,
				1,  0,
				1,  1
			};

        	private Mobile m_Owner;
        	public void DoECTeleport( Mobile owner )
        	{
           		m_Owner = owner;

    		    Map map = m_Owner.Map;

	            if ( map == null )
		            return;

	            Mobile toTeleport = null;

	            foreach ( Mobile m in m_Owner.GetMobilesInRange( 8 ) )
	            {
		            if ( m != m_Owner && m.Player && m_Owner.CanBeHarmful( m ) && m_Owner.CanSee( m ) )
		            {
			            toTeleport = m;
			            break;
		            }
	            }

	            if ( toTeleport != null )
	            {
		            int offset = Utility.Random( 8 ) * 2;

		            Point3D to = m_Owner.Location;

		            for ( int i = 0; i < m_Offsets.Length; i += 2 )
		            {
			            int x = m_Owner.X + m_Offsets[(offset + i) % m_Offsets.Length];
			            int y = m_Owner.Y + m_Offsets[(offset + i + 1) % m_Offsets.Length];

			            if ( map.CanSpawnMobile( x, y, m_Owner.Z ) )
			            {
				            to = new Point3D( x, y, m_Owner.Z );
				            break;
			            }
			            else
			            {
				            int z = map.GetAverageZ( x, y );

				            if ( map.CanSpawnMobile( x, y, z ) )
				            {
					            to = new Point3D( x, y, z );
					            break;
				            }
			            }
		            }

		            Mobile m = toTeleport;

		            Point3D from = m.Location;

		            m.Location = to;

		            Server.Spells.SpellHelper.Turn( m_Owner, toTeleport );
		            Server.Spells.SpellHelper.Turn( toTeleport, m_Owner );

		            m.ProcessDelta();

		            Effects.SendLocationParticles( EffectItem.Create( from, m.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
		            Effects.SendLocationParticles( EffectItem.Create(   to, m.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 5023 );

		            m.PlaySound( 0x1FE );

		            m_Owner.Combatant = toTeleport;
		            this.Combatant = m_Owner;
	            }
        	}

		public void LesserToken()
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

			for ( int i = 0; i < 4; ++i )
			{
				Mobile m = (Mobile)toGive[i % toGive.Count];
				if ( Utility.Random( 2 ) < 1 )
				{
				m.AddToBackpack( new SilverPrizeToken() );
				m.SendMessage( "You have received a silver token!" );
				}
			}
		}

		public override bool OnBeforeDeath()
		{
			if ( !NoKillAwards )
			{
				LesserToken();

				Map map = this.Map;

				if ( map != null )
				{
					for ( int x = -5; x <= 5; ++x )
					{
						for ( int y = -5; y <= 5; ++y )
						{
							double dist = Math.Sqrt(x*x+y*y);

							if ( dist <= 5 )
								new GoldTimer( map, X + x, Y + y ).Start();
						}
					}
				}
			}

			return base.OnBeforeDeath();
		}

		private class GoldTimer : Timer
		{
			private Map m_Map;
			private int m_X, m_Y;

			public GoldTimer( Map map, int x, int y ) : base( TimeSpan.FromSeconds( Utility.RandomDouble() * 10.0 ) )
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

				Gold g = new Gold( 50, 100 );

				g.MoveToWorld( new Point3D( m_X, m_Y, z ), m_Map );

				if ( 0.5 >= Utility.RandomDouble() )
				{
					switch ( Utility.Random( 3 ) )
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
		}
	}
}