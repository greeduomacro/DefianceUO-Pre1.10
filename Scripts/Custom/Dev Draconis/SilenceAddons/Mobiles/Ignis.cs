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
	public class Ignis : BaseBellBoss // This mobile has almost all Questboss.cs abilites with only the values changed and m_ability removed
	{
		static bool m_Active;

		[CommandProperty( AccessLevel.GameMaster )]
		public static bool Active
		{
			get{ return m_Active; }
			set{ m_Active = value; }
		}

		[Constructable]
		public Ignis() : base( AIType.AI_Mage )
		{
			Name = "Ignis Noxious";
			Body = 806;
			BaseSoundID = 959;
			Kills = 5;
			m_Active = true;

			SetStr( 1500 );
			SetDex( 1500 );
			SetInt( 1500 );

			SetHits( 20000 );
			SetMana( 20000 );

			SetDamage( 30, 50 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Energy, 50 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 20, 40 );
			SetResistance( ResistanceType.Cold, 10, 30 );
			SetResistance( ResistanceType.Poison, 5, 10 );
			SetResistance( ResistanceType.Energy, 50, 70 );

			SetSkill( SkillName.MagicResist, 400.0 );
			SetSkill( SkillName.EvalInt, 200.0 );
			SetSkill( SkillName.Magery, 200.0 );
			SetSkill( SkillName.Tactics, 150 );
			SetSkill( SkillName.Wrestling, 150 );
			SetSkill( SkillName.DetectHidden, 200 );

			Fame = 8000;
			Karma = 8000;

			VirtualArmor = 50;
		}

		public override void OnDeath( Container c )
		{
			m_Active = false;

			c.DropItem( new MysticKeySinglePart(5) );

			if ( Utility.Random( 10 ) < 1 )
			switch ( Utility.Random( 4 ) )
			{
          		        case 0: c.DropItem( new MetalChips() ); break;
          			case 1: c.DropItem( new ElegantGown() ); break;
         		        case 2: c.DropItem( new SaviourSash() ); break;
        		        case 3: c.DropItem( new ThievesCloak() ); break;
			}
			base.OnDeath( c );
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			if ( 0.80 >= Utility.RandomDouble())
				Polymorph( defender );

			if ( 0.1 >= Utility.RandomDouble() )
				Earthquake();

			if ( 0.25 >= Utility.RandomDouble() )
				DrainLife();

			if ( 0.25 >= Utility.RandomDouble() )
				DrainMana();

			if ( 0.25 >= Utility.RandomDouble() )
				DrainStam();

			if ( 0.1 >= Utility.RandomDouble() )
				DetectHidden();

			if ( 0.33 >= Utility.RandomDouble() && defender is BaseCreature )
			{
				BaseCreature c = (BaseCreature)defender;

				if ( c.Controlled && c.ControlMaster != null )
				{
					c.ControlTarget = c.ControlMaster;
					c.ControlOrder = OrderType.Attack;
					c.Combatant = c.ControlMaster;
				}
			}
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.25 >= Utility.RandomDouble() )
				DrainLife();

			if ( 0.25 >= Utility.RandomDouble() )
				DrainMana();

			if ( 0.25 >= Utility.RandomDouble() )
				DrainStam();

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

			if ( 0.1 >= Utility.RandomDouble() )
				Earthquake();
		}

		public override void OnDamagedBySpell( Mobile caster )
		{
			base.OnDamagedBySpell( caster );

			if ( 0.25 >= Utility.RandomDouble() )
				Teleport( caster );

			if ( 0.25 >= Utility.RandomDouble() )
				DrainLife();

			if ( 0.25 >= Utility.RandomDouble() )
				DrainMana();

			if ( 0.25 >= Utility.RandomDouble() )
				DrainStam();
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			if ( from != null && 0.1 >= Utility.RandomDouble() )
			{
				BaseCreature spawn = new DecayingSpawn ( this );

				spawn.BodyValue = this.BodyValue;
				spawn.Hue = this.Hue;
				spawn.BaseSoundID = this.BaseSoundID;
				spawn.Team = this.Team;
				spawn.MoveToWorld( from.Location, from.Map );
				spawn.Combatant = from;
				spawn.Say("I am here my master!");
			}
			base.OnDamage(amount, from, willKill);
		}

		public void Polymorph( Mobile m )
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

				m.BodyMod = 71;
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

		public void DrainLife()
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 12 ) )
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
				DoHarmful( m );

				m.FixedParticles( 0x374A, 10, 15, 5013, 0x496, 0, EffectLayer.Waist );
				m.PlaySound( 0x231 );

				m.SendMessage( "You feel the life drain out of you!" );

				int toDrain = Utility.RandomMinMax( 20, 40 );

				Hits += toDrain;
				m.Damage( toDrain, this );
			}
		}

		public void DrainMana()
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 12 ) )
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

		public void DrainStam()
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 12 ) )
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

				m.SendMessage( "You feel more and more fatigued!" );

				m.Stam -= Utility.RandomMinMax( 20, 25 );
			}
		}

       		public void Teleport( Mobile caster )
        	{
          		Map map = this.Map;

			if ( map != null )
			{

                    for (int i = 0; i < 10; ++i)
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

		public void Earthquake()
		{
			Map map = this.Map;

			if ( map == null )
				return;

			ArrayList targets = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 8 ) )
			{
				if ( m == this || !CanBeHarmful( m ) )
					continue;

				if ( m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != this.Team) )
					targets.Add( m );
				else if ( m.Player )
					targets.Add( m );
			}

			PlaySound( 0x2F3 );

			for ( int i = 0; i < targets.Count; ++i )
			{
				Mobile m = (Mobile)targets[i];

				double damage = m.Hits * 0.6;

				if ( damage < 10.0 )
					damage = 10.0;
				else if ( damage > 75.0 )
					damage = 75.0;

				DoHarmful( m );

				AOS.Damage( m, this, (int)damage, 100, 0, 0, 0, 0 );

				if ( m.Alive && m.Body.IsHuman && !m.Mounted )
					m.Animate( 20, 7, 1, true, false, 0 ); // take hit
			}
		}

		protected void DetectHidden()
		{
			Point3D p = Location;

			double srcSkill = Skills[SkillName.DetectHidden].Value;
			int range = (int)(srcSkill / 10.0);

			if ( !CheckSkill( SkillName.DetectHidden, 0.0, 100.0 ) )
				range /= 2;

			if ( range > 0 )
			{
				IPooledEnumerable inRange = Map.GetMobilesInRange( p, range );

				foreach ( Mobile trg in inRange )
				{
					if ( trg.Hidden && this != trg )
					{
						double ss = srcSkill + Utility.Random( 21 ) - 10;
						double ts = trg.Skills[SkillName.Hiding].Value + Utility.Random( 21 ) - 10;

						if ( AccessLevel >= trg.AccessLevel && ( ss >= ts ) )
						{
							trg.RevealingAction();
							trg.SendLocalizedMessage( 500814 ); // You have been revealed!
						}
					}
				}
				inRange.Free();
			}
		}

		public Ignis( Serial serial ) : base( serial )
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