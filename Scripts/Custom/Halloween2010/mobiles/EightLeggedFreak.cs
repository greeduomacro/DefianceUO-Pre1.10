using System;
using Server;
using Server.Items;
using Server.Engines.CannedEvil;
using Server.Mobiles;
using System.Collections;

namespace Server.Mobiles
{
	public class EightLeggedFreak : BaseCreature
	{
		private int webid;

		[Constructable]
		public EightLeggedFreak() : base( AIType.AI_Mage, FightMode.Closest, 20, 1, 0.3, 0.5 )
		{
			Body = 11;
			Name = "Eight Legged Freak";
			webid = 4306;
			BaseSoundID = 9042;
			Hue = 0x4DA;

			SetStr(300, 500);
			SetDex(120, 130);
			SetInt(400, 600);

			SetHits( 2500 );

			SetDamage(20, 30);

			SetSkill(SkillName.Magery, 120.0);
			SetSkill(SkillName.MagicResist, 100.0);
			SetSkill(SkillName.Tactics, 97.6, 100.0);
			SetSkill(SkillName.Anatomy, 77.6, 100.0);
			SetSkill(SkillName.Wrestling, 127.6, 135.0);

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 100;

			Timer mass = new MassWebTimer(this);
			mass.Start();

		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 50 ) < 1 )
			c.DropItem( new HalloweenCandle() );

			if ( Utility.Random( 10 ) < 1 )
			c.DropItem( new HalloweenStatue() );

			base.OnDeath( c );
		}


		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 2 );
		}

		public override bool AlwaysMurderer {get { return true; } }
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



		public override void OnDamagedBySpell(Mobile from)
		{
			base.OnDamagedBySpell(from);
			if (from.Player || (from is BaseCreature && ((BaseCreature)from).Controlled))
				BreathStart(from);

		}
		public override void BreathStart(Mobile target)
		{
			base.BreathStart(target);
			webid = 4306;
			Timer web = new CastWebTimer(this, target, webid);
			web.Start();

		}

		public class CastWebTimer : Timer
		{
			private Mobile target;
			private EightLeggedFreak mep;
			private int webid;

			public CastWebTimer(EightLeggedFreak mep, Mobile target, int webid) : base(TimeSpan.FromSeconds( 2 ))
			{
				this.mep = mep;
				this.target = target;
				this.webid = webid;
			}

			protected override void OnTick()
			{
				if (mep != null && target != null && target.Alive && mep.Alive && mep.CanBeHarmful(target, false))
				{
					ParalyseWeb web = new ParalyseWeb(mep, target, webid);
					web.MoveToWorld(target.Location, target.Map);
					target.Paralyze(TimeSpan.FromSeconds(15));
				}

			}

		}

		public class MassWebTimer : Timer
		{
			private EightLeggedFreak mep;

			public MassWebTimer(EightLeggedFreak mep) : base(TimeSpan.FromSeconds( 15), TimeSpan.FromSeconds( 25))
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


		public EightLeggedFreak(Serial serial) : base( serial )
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

	}
}


namespace Server.Items
{
	public class ParalyseWeb : Item
	{
		private EightLeggedFreak mep;
		[Constructable]
		public ParalyseWeb(EightLeggedFreak mep, Mobile target, int webid) : base( webid)
		{
			Name = "Sticking webs";
			Movable = false;
			this.mep = mep;
			Timer webT = new WebTimer(mep, target, this);
			webT.Start();
		}

		public ParalyseWeb(Serial serial) : base( serial )
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
			mep = (EightLeggedFreak)reader.ReadMobile();
		}


		public class WebTimer : Timer
		{
			private Mobile target;
			private ParalyseWeb web;
			private EightLeggedFreak mep;

			public WebTimer(EightLeggedFreak mep, Mobile target, ParalyseWeb web) : base(TimeSpan.FromSeconds( 15 + Utility.Random(25)))
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
			private ParalyseWeb web;
			private EightLeggedFreak mep;

			public PullTimer(EightLeggedFreak mep, Mobile target, ParalyseWeb web) : base(TimeSpan.FromSeconds( 4 ))
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