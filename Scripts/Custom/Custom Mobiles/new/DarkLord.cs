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
	public class DarkLord : BaseCreature
	{
		private static bool m_Talked;

		string[] kfcsay = new string[]
		{
		"Cast out from our world...",
		"We who remain true shall rise again...",
		};

		public override bool IsScaryToPets{ get{ return true; } }

		[Constructable]
		public DarkLord() : base( AIType.AI_Mage, FightMode.Weakest, 10, 1, 0.1, 0.2)
		{

			Name = "Deimos the Dark Lord";
			Body = 784;
			ShortTermMurders = 10;
			Kills = 10;
			SpeechHue= 2304;
			BaseSoundID = 0x3E9;
                        //new EtherealHorse().Rider = this;

			SetStr( 975, 1075);
			SetDex( 140, 175);
			SetInt( 650, 700);

			SetHits(5300, 5750);
			SetMana(700, 850);

			SetDamage( 15, 25 );

			SetSkill( SkillName.Tactics, 100.7, 100.4);
			SetSkill( SkillName.MagicResist, 320.4, 340.7);
                        SetSkill( SkillName.Magery, 120.4, 130.7);
                        SetSkill( SkillName.Anatomy, 110.4, 130.7);
                        SetSkill( SkillName.Wrestling, 100.4, 120.7);
                        SetSkill( SkillName.Meditation, 310.4, 420.7);
                        SetSkill( SkillName.EvalInt, 160.4, 190.7);

                        Fame=35000;
			Karma=-35000;

			VirtualArmor= 100;

			int gems = Utility.RandomMinMax( 50, 50 );

			for ( int i = 0; i < gems; ++i )
				PackGem();

			PackGold( 54400, 56100);
			PackReg( 500 );
			PackScroll( 1, 8 );
			PackScroll( 1, 8 );
			PackScroll( 1, 8 );
			PackScroll( 1, 8 );
			PackScroll( 1, 8 );
			PackScroll( 1, 8 );
			PackScroll( 1, 8 );
			PackScroll( 1, 8 );
			PackScroll( 1, 8 );
			PackScroll( 1, 8 );
			PackScroll( 1, 8 );
			PackScroll( 1, 8 );
			PackWeapon( 4, 5 );
			PackArmor( 4, 5 );
			PackWeapon( 3, 5 );
			PackArmor( 3, 5 );
			PackWeapon( 3, 5 );
			PackArmor( 3, 5 );
			PackWeapon( 4, 5 );
			PackArmor( 4, 5 );
			PackWeapon( 3, 5 );
			PackArmor( 3, 5 );
			PackWeapon( 4, 5 );
			PackWeapon( 4, 5 );
			PackArmor( 4, 5 );
			PackArmor( 4, 5 );
			PackSlayer();
			PackSlayer();
			PackSlayer();
			PackSlayer();
			PackSlayer();
			PackItem( new IDWand() );
			PackItem( new IDWand() );
			PackItem( new IDWand() );
			PackItem( new IDWand() );
			PackItem( new IDWand() );
			PackItem( new InvisCloak() );
			PackItem( new InvisCloak() );
			PackItem( new InvisBrace() );
			PackItem( new InvisBrace() );
			PackItem( new InvisHat() );
			PackItem( new InvisHat() );


		}

                public override bool AutoDispel{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool Uncalmable{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }

		public override bool ShowFameTitle{ get{ return false; } }
		public override bool ClickTitle{ get{ return false; } }

		private DateTime m_NextAbilityTime;

		private void DoAreaLeech()
		{
			m_NextAbilityTime += TimeSpan.FromSeconds( 10.0 );

			this.Say( true, "In time I shall rise again!" );
			this.FixedParticles( 0x376A, 10, 10, 9537, 33, 0, EffectLayer.Waist );

			Timer.DelayCall( TimeSpan.FromSeconds( 10.0 ), new TimerCallback( DoAreaLeech_Finish ) );
		}

		private void DoAreaLeech_Finish()
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 10 ) )
			{
				if ( this.CanBeHarmful( m ) && this.IsEnemy( m ) )
					list.Add( m );
			}

			if ( list.Count == 0 )
			{
				this.Say( true, "From dark grounds i will rise again!" );
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

				this.Say( true, "The true path to Order is through Chaos!" );
			}
		}

		private void DoFocusedLeech( Mobile combatant, string message )
		{
			this.Say( true, message );

			Timer.DelayCall( TimeSpan.FromSeconds( 2.0 ), new TimerStateCallback( DoFocusedLeech_Stage1 ), combatant );
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
				this.Hits += AOS.Damage( combatant, this, Utility.RandomMinMax( 40, 40 ) - (Core.AOS ? 0 : 10), 100, 0, 0, 0, 0 );
			}
		}

		public override void OnThink()
		{
			if ( DateTime.Now >= m_NextAbilityTime )
			{
				Mobile combatant = this.Combatant;

				if ( combatant != null && combatant.Map == this.Map && combatant.InRange( this, 20 ) )
				{
					m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 15, 15 ) );

					int ability = Utility.Random( 2 );

					switch ( ability )
					{
						case 0: DoFocusedLeech( combatant, "In remembrance of our noble path..." ); break;
						case 1: DoAreaLeech(); break;
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

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Controlled || bc.BardTarget == this )
					damage = 0; // Immune to pets and provoked creatures
			}
		}

		public DarkLord( Serial serial ) : base( serial )
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
		public override void OnDeath( Container c )
		{
			c.DropItem( new EtherealHorse() );
			c.DropItem( new ClothingBlessDeed() );
			c.DropItem( new ClothingBlessDeed() );
			c.DropItem( new BlackDyeTub() );
			c.DropItem( new SpecialHairDye() );
			c.DropItem( new SpecialBeardDye() );
			c.DropItem( new SpecialHairDye() );
			c.DropItem( new SpecialBeardDye() );
			c.DropItem( new NameChangeDeed() );
			c.DropItem( new NameChangeDeed() );
			c.DropItem( new HoodedShroudOfShadows() );

			base.OnDeath( c );
		}

	}
}