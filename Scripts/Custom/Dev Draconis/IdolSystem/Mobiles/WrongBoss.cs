using System;
using Server;
using Server.Items;
using System.Collections;
using Server.Targeting;
using Server.Engines.IdolSystem;

namespace Server.Mobiles
{
	public class WrongBoss : BaseMiniBoss
	{
		static bool m_Active;
		IdolType m_Type;
		MagicalRareType m_Rare;

		[CommandProperty( AccessLevel.GameMaster )]
		public static bool Active
		{
			get{ return m_Active; }
			set{ m_Active = value; }
		}

		[Constructable]
		public WrongBoss () : base( AIType.AI_Mage )
		{
			Name = "Idol Keeper";
			Title = "of Wrong";
			Body = 400;
			Hue = 0;
			m_Active = true;
			m_Type = IdolType.Wrong;
			m_Rare = MagicalRareType.Six;

			SetStr( 500, 550 );
			SetDex( 200, 250 );
			SetInt( 450, 550 );

			SetHits( 5500 );

			SetDamage( 28, 30 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 35, 40 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.EvalInt, 140.0 );
			SetSkill( SkillName.Magery, 140.0 );
			SetSkill( SkillName.Meditation, 300.0 );
			SetSkill( SkillName.MagicResist, 300.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 90.0 );

			Fame = 9500;
			Karma = -10500;

			VirtualArmor = 10;

			Robe robe = new Robe();
			robe.Hue = 1109;
			robe.Name = "Robe of the Idol Keeper";
			robe.LootType = LootType.Blessed;
			AddItem( robe );
		}

		public override void OnDeath( Container c )
		{
			m_Active = false;

			if ( Utility.Random( 5 ) < 1 )
				c.DropItem( new Idol( m_Type ) );

			if ( Utility.Random( 8 ) < 1 )
				c.DropItem( new MagicalRare( m_Rare ) );

			base.OnDeath( c );
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool CanDestroyObstacles { get { return true; } }
		public override bool DoEarthquake { get { return true; } }
		public override int CanCheckReflect{ get { return 4; } }

	  	public override void OnGotMeleeAttack( Mobile attacker )
        	{
            		base.OnGaveMeleeAttack( attacker );
            		if ( Utility.Random( 4 ) < 1 )
            		{
           			Map map = this.Map;

				if ( map != null )
				{
					for (int i = 0; i < 10; ++i)
                    			{
                       			int x = X + (Utility.RandomMinMax(-10, 15));
		                        int y = Y + (Utility.RandomMinMax(-10, 15));
                		        int z = Z;

		                        if (!map.CanFit(x, y, z, 16, false, false))
                		            continue;

		                        Point3D from = attacker.Location;
                		        Point3D to = new Point3D(x, y, z);

		                        if (!InLOS(to))
                		            continue;

		                        attacker.Location = to;
                		        attacker.ProcessDelta();
		                        attacker.Combatant = null;
                		        attacker.Freeze(TimeSpan.FromSeconds(10.0));

		                        Effects.SendLocationParticles(EffectItem.Create(from, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
		                        Effects.SendLocationParticles(EffectItem.Create(to, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);

		                        Effects.PlaySound(to, map, 0x1FE);
                			}
                		}
			}
             	}

		public WrongBoss( Serial serial ) : base( serial )
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
            m_Active = true;
		}
        public override void OnAfterDelete()
        {
            m_Active = false;
            base.OnAfterDelete();
        }
	}
}