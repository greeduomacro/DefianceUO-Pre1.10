using System;
using Server.Mobiles;
using Server.Items;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "a mouse corpse" )]
	public class Mouse : BaseCreature
	{
		static bool m_Trapped;

        	public static bool Trapped
        	{
          	  	get { return m_Trapped; }
           	 	set { m_Trapped = value; }
        	}

		[Constructable]
		public Mouse() : base( AIType.AI_Melee, FightMode.Agressor, 10, 1, 0.005, 0.001 )
		{
			Name = "a mouse";
			Body = 0xD7;
			BaseSoundID = 0x188;
			Hue = 1154;
			m_Trapped = false;

			SetStr( 100, 150 );
			SetDex( 200, 300 );
			SetInt( 20, 40 );

			SetHits( 1000, 1200 );
			SetMana( 0 );

			SetDamage( 30, 40 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 15, 20 );
			SetResistance( ResistanceType.Fire, 5, 10 );
			SetResistance( ResistanceType.Poison, 25, 35 );

			SetSkill( SkillName.MagicResist, 180.0 );
			SetSkill( SkillName.Tactics, 120.0 );
			SetSkill( SkillName.Wrestling, 150.0 );

			Fame = 9000;
			Karma = -9000;

			VirtualArmor = 40;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
		}

        	public override bool CanRummageCorpses{ get{ return true; } }
	        public override Poison PoisonImmune{ get{ return Poison.Greater; } }
        	public override int TreasureMapLevel{ get{ return 4; } }
	        public override int Meat{ get{ return 1; } }
		public override bool AutoDispel{ get{ return true; } }

		private DateTime m_NextAbilityTime;

		public override void OnThink()
		{
			if ( DateTime.Now >= m_NextAbilityTime )
			{
				this.Say( "*sniffs*" );
				PlaySound( 1092 );
				this.Freeze(TimeSpan.FromSeconds(2.0));
				Timer.DelayCall( TimeSpan.FromSeconds( 2.0 ), new TimerCallback( DoPounce ) );
				m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 30, 45 ) );
			}

			if ( m_Trapped == false )
			{
				foreach ( Item item in this.GetItemsInRange( 0 ) )
				{
					if ( item is PentagramAddon )
					{
						this.Hue = 1150;
						this.BoltEffect( 0 );
						this.PlaySound( 1475 );
						this.Say( "*a mouse is severely weakened*" );
						m_Trapped = true;
					}
				}
			}
			base.OnThink();
		}

		public override void Damage( int amount, Mobile from )
		{
			if ( m_Trapped == false )
			{
				amount = (int)(0);
			}
			base.Damage( amount, from );
		}

		public void DoPounce()
		{
			ArrayList topounce = new ArrayList();

			foreach ( Item item in this.GetItemsInRange( 8 ) )
			{
				if ( item is CheeseWheel || item is CheeseWedge )
					topounce.Add( item );
			}

			if ( topounce.Count == 1 )
			{
				foreach ( Item item in topounce )
				{
					this.Say( "*pounces*" );
					Point3D from = this.Location;
                        		Point3D to = item.Location;
                        		this.Location = to;
                        		this.ProcessDelta();
					this.Freeze(TimeSpan.FromSeconds(2.0));
					PlaySound ( 59 );
					item.Delete();
				}
			}
			else if ( topounce.Count > 1 )
			{
				foreach ( Item item in topounce )
				{
					Rat rat = new Rat();
					rat.Tamable = false;
					rat.Hue = this.Hue;
					rat.Name = "a lesser mouse";
					rat.Controlled = true;
					rat.ControlMaster = this;
					rat.ControlOrder = OrderType.Follow;
					rat.ControlTarget = this;
					this.Freeze(TimeSpan.FromSeconds(2.0));
					rat.MoveToWorld( item.Location, item.Map );
					PlaySound ( 59 );
					item.Delete();
					rat.Freeze(TimeSpan.FromSeconds(1.0));
				}
			}
			else
			{
				this.Say( "eeek" );
				this.Freeze(TimeSpan.FromSeconds(2.0));

				ArrayList toreclaim = new ArrayList();

				foreach ( Mobile m in this.GetMobilesInRange( 15 ) )
				{
					if ( m is Rat )
					{
						Rat rat = (Rat)m;

						if ( rat.Hue == 1150 || rat.Hue == 1154 )
						toreclaim.Add( m );
					}
				}

				if ( toreclaim.Count > 0 )
				{
					foreach ( Mobile m in toreclaim )
					{
						PlaySound( 251 );
						this.Hits += m.Hits;
						m.Delete();
					}
				}
			}
		}

		public override void OnDeath( Container c )
		{
			m_Trapped = false;

			if ( Utility.Random( 10 ) < 1 )
			c.DropItem( new PoemPartsTwo() );

			if ( Utility.Random( 50 ) < 1 )
			c.DropItem( new CatStatue() );

			base.OnDeath( c );
		}

		public Mouse(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			m_Trapped = true;
		}
	}
}