using System;
using System.Collections;
using Server.Items;
using Server.Spells.Seventh;
using Server.Spells.Fifth;
using Server.Spells;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
	public class Zendella : BaseCreature
	{

		private static bool m_Talked;

		string[] kfcsay = new string[]
		{
		"Cast out from our world...",
		"In rememberance of our noble leaders...",
		"We who remain true shall rise again...",
		};

		public override bool IsScaryToPets{ get{ return true; } }

		[Constructable]
		public Zendella() : base( AIType.AI_Mage, FightMode.Weakest, 10, 1, 0.2, 0.4)
		{

			Name = "Zendella Kxriss";
			//Title= "";
			Hue= 2221;
			Body = 401;
			SpeechHue= 2304;
			BaseSoundID = 0;
			Team = 0;
                        //new EtherealHorse().Rider = this;

			SetStr( 275, 375);
			SetDex( 40, 75);
			SetInt( 150, 200);

			SetHits(230, 375);
			SetMana(400, 450);

			SetSkill( SkillName.Tactics, 100.7, 100.4);
			SetSkill( SkillName.MagicResist, 250.4, 250.7);
                        SetSkill( SkillName.Magery, 120.4, 120.7);
			SetSkill( SkillName.Macing, 110.4, 110.7);
                        SetSkill( SkillName.EvalInt, 160.4, 190.7);

                        Fame=15000;
			Karma=-15000;

			VirtualArmor= 80;

			Item BlackStaff = new BlackStaff();
			BlackStaff.Movable=false;
			BlackStaff.Hue=2306;
		        EquipItem( BlackStaff );

                        //Item BoneHelm = new BoneHelm();
			//BoneHelm.Movable=false;
			//BoneHelm.Hue=38;
			//EquipItem( BoneHelm );

			Item HoodedShroudOfShadows = new HoodedShroudOfShadows();
			HoodedShroudOfShadows.Movable=false;
			HoodedShroudOfShadows.Hue=2118;
			HoodedShroudOfShadows.Name="death shroud";
			EquipItem( HoodedShroudOfShadows );

                        Item LeatherGloves = new LeatherGloves();
			LeatherGloves.Movable=false;
			LeatherGloves.Hue=1;
                        EquipItem( LeatherGloves );

                        //Item LongPants = new LongPants();
			//LongPants.Movable=false;
			//LongPants.Hue=1;
			//EquipItem( LongPants );

			Item Sandals = new Sandals();
			Sandals.Movable=false;
			Sandals.Hue=1;
			EquipItem( Sandals );

			//Item Cloak = new Cloak();
			//Cloak.Movable=false;
			//Cloak.Hue=0xFFFF;
			//EquipItem( Cloak );

			Item hair = new Item( 0x203B);
			hair.Hue = 2306;
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem( hair );

			PackGold( 400, 3100);
			PackArmor( 0, 5 );
			PackWeapon( 0, 5 );
			PackArmor( 0, 3 );
			PackWeapon( 0, 2 );

                                switch ( Utility.Random( 2 ))
        		 {
           			case 0: PackItem( new ZendellSoul() ); break;
        		 }
		}

                public override bool AutoDispel{ get{ return true; } }
		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool BardImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }

		public override bool ShowFameTitle{ get{ return false; } }
		public override bool ClickTitle{ get{ return false; } }


	        public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.25 >= Utility.RandomDouble() && attacker is BaseCreature )
			{
				BaseCreature c = (BaseCreature)attacker;

				if ( c.Controlled && c.ControlMaster != null )
				{
					c.ControlTarget = c.ControlMaster;
					c.ControlOrder = OrderType.Attack;
					c.Combatant = c.ControlMaster;
				}
			}
		}

		private DateTime m_NextAbilityTime;

		private void DoAreaLeech()
		{
			m_NextAbilityTime += TimeSpan.FromSeconds( 10.0 );

			this.Say( true, "We who remain true shall rise again." );
			this.FixedParticles( 0x376A, 10, 10, 9537, 33, 0, EffectLayer.Waist );

			Timer.DelayCall( TimeSpan.FromSeconds( 10.0 ), new TimerCallback( DoAreaLeech_Finish ) );
		}

		private void DoAreaLeech_Finish()
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 5 ) )
			{
				if ( this.CanBeHarmful( m ) && this.IsEnemy( m ) )
					list.Add( m );
			}

			if ( list.Count == 0 )
			{
				this.Say( true, "From dark grounds the black wisps shall rise!" );
			}
			else
			{
				double scalar;

				if ( list.Count == 1 )
					scalar = 0.75;
				else if ( list.Count == 2 )
					scalar = 0.50;
				else
					scalar = 0.25;

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

				this.Say( true, "The true path to Order is through Chaos." );
			}
		}

		private void DoFocusedLeech( Mobile combatant, string message )
		{
			this.Say( true, message );

			Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( DoFocusedLeech_Stage1 ), combatant );
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
			if ( DateTime.Now >= m_NextAbilityTime )
			{
				Mobile combatant = this.Combatant;

				if ( combatant != null && combatant.Map == this.Map && combatant.InRange( this, 12 ) )
				{
					m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 10, 15 ) );

					int ability = Utility.Random( 4 );

					switch ( ability )
					{
						case 0: DoFocusedLeech( combatant, "In remembrance of our noble leaders." ); break;
						case 1: DoFocusedLeech( combatant, "Cast out from our world by those who would not believe." ); break;
						case 2: DoFocusedLeech( combatant, "Only then balance can be found." ); break;
						case 3: DoAreaLeech(); break;
							// TODO: Resurrect ability
					}
				}
			}
}
////
				public void SpawnEvil( Mobile target )
		{
			Map map = this.Map;

			if ( map == null )
				return;

			int spawned = 0;

			foreach ( Mobile m in this.GetMobilesInRange( 10 ) )
			{
if ( m is EvilMage || m is IceSerpent || m is IceSerpent )
					++spawned;
			}

			if ( spawned < 10 )
			{
				int newSpawned = Utility.RandomMinMax( 1, 2 );

				for ( int i = 0; i < newSpawned; ++i )
				{
					BaseCreature spawn;

					switch ( Utility.Random( 2 ) )
					{
						default:
						//case 0: case 1:	spawn = new EtherealWarrior(); break;
						//case 2: case 3:	spawn = new EvilMage(); break;
						case 4:			spawn = new ShadowWisp(); break;
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


		public Zendella( Serial serial ) : base( serial )
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