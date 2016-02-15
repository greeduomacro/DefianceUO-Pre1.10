using System;
using Server;
using Server.Items;
using Server.Engines.CannedEvil;
using Server.Mobiles;

namespace Server.Mobiles
{
	public class OldBarkskin : BaseCreature
	{
		private int webid;

		[Constructable]
		public OldBarkskin() : base( AIType.AI_Melee, FightMode.Closest, 20, 1, 0.5, 1.0 )
		{
			Body = 285;
			Kills = 5;
			Name = "Old Barkskin";
			webid = 12320;

			BaseSoundID = 442;

			SetStr(950, 1000);
			SetDex(120, 130);
			SetInt(40, 60);

			SetHits(2000, 2250);

			SetDamage(37, 43);

			SetSkill(SkillName.MagicResist, 330.7, 340.0);
			SetSkill(SkillName.Tactics, 97.6, 100.0);
			SetSkill(SkillName.Anatomy, 77.6, 100.0);
			SetSkill(SkillName.Wrestling, 127.6, 135.0);

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 100;
			Timer mass = new MassWebTimer(this);
			mass.Start();

			PackItem( new Log( 100 ) );
			PackGold( 2350, 2550 );
			PackWeapon( 0, 5 );
			PackArmor( 0, 5 );

			switch ( Utility.Random( 3 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 6 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 10 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}
		}

 		public override Poison PoisonImmune { get { return Poison.Greater; } }
		public override double BreathDamageScalar { get { return 0.0; } }
		public override int BreathEffectItemID { get { return webid; } }
		public override double BreathDamageDelay { get { return 0.0; } }
		public override bool AutoDispel { get { return true; } }

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is EnergyVortex || to is BladeSpirits )
				damage *= 20;
		}

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
			{
			scalar = 0.0; // Immune to magic
			}

		public override void OnGotMeleeAttack(Mobile attacker)

		{
			base.OnGotMeleeAttack(attacker);

		}

		public override void OnDamagedBySpell(Mobile from)
		{
			base.OnDamagedBySpell(from);
			if (from.Player || (from is BaseCreature && ((BaseCreature)from).Controlled))
				BreathStart(from);

		}
		public override void BreathStart(Mobile target)
		{
			base.BreathStart(target);
			webid = 12320 + Utility.Random(4);
			Timer web = new CastWebTimer(this, target, webid);
			web.Start();

		}

		public class CastWebTimer : Timer
		{
			private Mobile target;
			private OldBarkskin mep;
			private int webid;

			public CastWebTimer(OldBarkskin mep, Mobile target, int webid) : base(TimeSpan.FromSeconds( 2 ))
			{
				this.mep = mep;
				this.target = target;
				this.webid = webid;
			}

			protected override void OnTick()
			{
				if (mep != null && target != null && target.Alive && mep.Alive && mep.CanBeHarmful(target, false))
				{
					BrambleRoot web = new BrambleRoot(mep, target, webid);
					web.MoveToWorld(target.Location, target.Map);
					target.Paralyze(TimeSpan.FromSeconds(15));
				}

			}

		}

		public class MassWebTimer : Timer
		{
			private OldBarkskin mep;

			public MassWebTimer(OldBarkskin mep) : base(TimeSpan.FromSeconds( 15), TimeSpan.FromSeconds( 25))
			{
				this.mep = mep;
			}

			protected override void OnTick()
			{
				if (mep.Map != null && mep != null)
				{
					foreach (Mobile m in mep.GetMobilesInRange(15))
					{

						if (mep != m && m != null && m.Alive && (m.Player || (m is BaseCreature && ((BaseCreature)m).Controlled)) && mep.CanBeHarmful(m, false) && Utility.RandomDouble() > 0.15)
						{
							mep.BreathStart(m);
						}

					}
				}
				else
					this.Stop();

			}

		}


		public OldBarkskin(Serial serial) : base( serial )
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 100 ) == 0 )
				c.DropItem( new MagicalShortbow() );
			base.OnDeath( c );
		}
	}
}


namespace Server.Items
{
	public class BrambleRoot : Item
	{
		private OldBarkskin mep;
		[Constructable]
		public BrambleRoot(OldBarkskin mep, Mobile target, int webid) : base( webid)
		{
			Name = "Rooting Brambles";
			Movable = false;
			this.mep = mep;
			Timer webT = new WebTimer(mep, target, this);
			webT.Start();
		}

		public BrambleRoot(Serial serial) : base( serial )
		{

		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
			writer.Write(mep);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			mep = (OldBarkskin)reader.ReadMobile();
		}


		public class WebTimer : Timer
		{
			private Mobile target;
			private BrambleRoot web;
			private OldBarkskin mep;

			public WebTimer(OldBarkskin mep, Mobile target, BrambleRoot web) : base(TimeSpan.FromSeconds( 15 + Utility.Random(25)))
			{
				this.mep = mep;
				this.target = target;
				this.web = web;
			}

			protected override void OnTick()
			{
				if (web != null)
					web.Delete();
			}

		}


		public class PullTimer : Timer
		{
			private Mobile target;
			private BrambleRoot web;
			private OldBarkskin mep;

			public PullTimer(OldBarkskin mep, Mobile target, BrambleRoot web) : base(TimeSpan.FromSeconds( 4 ))
			{
				this.mep = mep;
				this.target = target;
				this.web = web;
			}

			protected override void OnTick()
			{
				if (mep != null && target != null && target.Alive && mep.Alive
					   && target.Location == web.Location && target.Map == web.Map && mep.CanBeHarmful(target, false))
				{

					target.Location = mep.Location;
					mep.Combatant = target;
				}

			}
		}
	}
}