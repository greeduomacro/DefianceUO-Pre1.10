using System;
using Server;
using Server.Misc;
using Server.Items;
using System.Collections;
using Server.Spells.Seventh;
using Server.Spells.Fifth;
//using Server.Engines.SilenceAddon;

namespace Server.Mobiles
{
	public class Zirux : BaseBellBoss
	{
		static bool m_Active;

		[CommandProperty( AccessLevel.GameMaster )]
		public static bool Active
		{
			get{ return m_Active; }
			set{ m_Active = value; }
		}

		[Constructable]
		public Zirux() : base( AIType.AI_Mage )
		{
			Name = "Zirux Windcaller";
			Body = 311;
			Kills = 5;
			m_Active = true;

			SetStr( 750 );
			SetDex( 300 );
			SetInt( 1200 );

			SetHits( 6000 );

			SetDamage( 25, 40 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Energy, 50 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 20, 40 );
			SetResistance( ResistanceType.Cold, 10, 30 );
			SetResistance( ResistanceType.Poison, 5, 10 );
			SetResistance( ResistanceType.Energy, 50, 70 );

			SetSkill( SkillName.MagicResist, 250 );
			SetSkill( SkillName.EvalInt, 200.0 );
			SetSkill( SkillName.Magery, 200.0 );
			SetSkill( SkillName.Tactics, 140 );
			SetSkill( SkillName.Wrestling, 140 );

			Fame = 8000;
			Karma = 8000;

			VirtualArmor = 140;
		}

		public override void OnDeath( Container c )
		{
			m_Active = false;

			if ( Utility.Random( 2 ) < 1 )
			c.DropItem( new MysticKeySinglePart(2) );

			if ( Utility.Random( 10 ) < 1 )
			c.DropItem( new ElegantGown() );

			base.OnDeath( c );
		}

		public override void OnDamagedBySpell( Mobile caster ) // Taken from questboss.cs and the m_ability removed
		{
			base.OnDamagedBySpell( caster );

			if ( 0.25 >= Utility.RandomDouble() )
				Teleport( caster );
		}

		public override void OnGotMeleeAttack( Mobile attacker ) // Taken from questboss.cs and the m_ability removed
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.25 >= Utility.RandomDouble() )
				DrainMana();

			if ( 0.33 >= Utility.RandomDouble() && attacker is BaseCreature )
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

		public override void OnGaveMeleeAttack( Mobile defender ) // Taken from questboss.cs and the m_ability removed
		{
			base.OnGaveMeleeAttack( defender );

			if ( 0.80 >= Utility.RandomDouble())
				Polymorph( defender );

			if ( 0.25 >= Utility.RandomDouble() )
				DrainMana();
		}

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar ) // Taken from questboss.cs and the m_ability removed
		{
			if ( caster is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)caster;

				if ( bc.Controlled || bc.Summoned || bc.BardTarget == this )
				scalar = 0.33;
			}
		}

		public override void AlterMeleeDamageTo( Mobile to, ref int damage ) // Taken from questboss.cs and the m_ability removed and values changed
		{
			if ( to is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)to;

				if ( bc.Controlled || bc.Summoned || bc.BardTarget == this )
				damage *= 3;
			}
		}

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage ) // Taken from questboss.cs and the m_ability removed and values changed
		{
			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Controlled || bc.Summoned || bc.BardTarget == this )
				damage /= 2;
			}
		}

		public void Polymorph( Mobile m ) //taken from evilvampire.cs and the values changed
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

				m.BodyMod = 311;
				m.HueMod = 0;
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


		public void DrainMana() // Taken from questboss.cs and the m_ability removed and values changed
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

				m.Mana -= Utility.RandomMinMax( 20, 30 );
			}
		}

		public void Teleport( Mobile caster ) // Taken from questboss.cs and values changed (note this.Combatant = caster; was added and i have emailed you this change to questboss.cs too
        	{
          		Map map = this.Map;

			if ( map != null )
			{

                    for (int i = 0; i < 12; ++i)
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

		public Zirux( Serial serial ) : base( serial )
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
			//Explanation see GhostPast
			m_Active = true;
		}
	}
}