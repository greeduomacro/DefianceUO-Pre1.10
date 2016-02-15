using System;
using Server;
using Server.Items;
using Server.Engines.CannedEvil;
using Server.Mobiles;

namespace Server.Mobiles
{
	public class CalebMephitis : BaseChampion
	{
		public override ChampionSkullType SkullType { get { return ChampionSkullType.Venom; } }

		private int webid;

		[Constructable]
		public CalebMephitis() : base( AIType.AI_Melee )
		{
			Body = 173;
			Name = "Mephitis";
			webid = 4310;

			BaseSoundID = 0x183;

			SetStr(505, 1000);
			SetDex(102, 300);
			SetInt(402, 600);

			SetHits(3000);
			SetStam(105, 600);

			SetDamage(21, 33);

			SetDamageType(ResistanceType.Physical, 50);
			SetDamageType(ResistanceType.Poison, 50);

			SetResistance(ResistanceType.Physical, 75, 80);
			SetResistance(ResistanceType.Fire, 60, 70);
			SetResistance(ResistanceType.Cold, 60, 70);
			SetResistance(ResistanceType.Poison, 100);
			SetResistance(ResistanceType.Energy, 60, 70);

			SetSkill(SkillName.MagicResist, 70.7, 140.0);
			SetSkill(SkillName.Tactics, 97.6, 100.0);
			SetSkill(SkillName.Wrestling, 97.6, 100.0);

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 80;
			Timer mass = new MassWebTimer(this);
			mass.Start();
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.UltraRich, 3);
		}

		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override Poison HitPoison { get { return Poison.Lethal; } }
		public override int BreathFireDamage { get { return 0; } }
		public override double BreathDamageScalar { get { return 0.0; } }
		public override int BreathEffectItemID { get { return webid; } }
		public override int BreathColdDamage { get { return 0; } }
		public override int BreathEnergyDamage { get { return 0; } }
		public override int BreathPhysicalDamage { get { return 0; } }
		public override int BreathPoisonDamage { get { return 20; } }
		public override double BreathDamageDelay { get { return 0.0; } }
		public override bool AutoDispel { get { return true; } }
		public override void OnGotMeleeAttack(Mobile attacker)
		{
			base.OnGotMeleeAttack(attacker);

			// TODO: Web ability
		}

		public override void OnDamagedBySpell(Mobile from)
		{
			base.OnDamagedBySpell(from);
			if (from.Player || ((BaseCreature)from).Controlled)
				BreathStart(from);

		}
		public override void BreathStart(Mobile target)
		{
			base.BreathStart(target);
			webid = 4306 + Utility.Random(6);
			Timer web = new CastWebTimer(this, target, webid);
			web.Start();

			// TODO: Web ability
		}

		public class CastWebTimer : Timer
		{
			private Mobile target;
			private CalebMephitis mep;
			private int webid;

			public CastWebTimer(CalebMephitis mep, Mobile target, int webid) : base(TimeSpan.FromSeconds( 2 ))
			{
				this.mep = mep;
				this.target = target;
				this.webid = webid;
			}

			protected override void OnTick()
			{
				if (mep != null && target != null && target.Alive && mep.Alive)
				{
					MephitisWeb web = new MephitisWeb(mep, target, webid);
					web.MoveToWorld(target.Location, target.Map);
					target.Paralyze(TimeSpan.FromSeconds(10));
				}

			}

		}

		public class MassWebTimer : Timer
		{
			private CalebMephitis mep;

			public MassWebTimer(CalebMephitis mep) : base(TimeSpan.FromSeconds( 20), TimeSpan.FromSeconds( 30))
			{
				this.mep = mep;
			}

			protected override void OnTick()
			{
				if (mep.Map != null && mep != null)
				{
					foreach (Mobile m in mep.GetMobilesInRange(12))
					{

						if (mep != m && m != null && m.Alive && (m.Player || (m is BaseCreature && ((BaseCreature)m).Controlled)) && Utility.RandomDouble() > 0.15)
						{
							mep.BreathStart(m);
						}

					}
				}
				else
					this.Stop();

			}

		}


		public CalebMephitis(Serial serial) : base( serial )
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
	public class MephitisWeb : Item
	{
		private CalebMephitis mep;
		[Constructable]
		public MephitisWeb(CalebMephitis mep, Mobile target, int webid) : base( webid)
		{
			Name = "Web of Mephitis";
			Movable = false;
			this.mep = mep;
			Timer pullT = new PullTimer(mep, target, this);
			Timer webT = new WebTimer(mep, target, this);
			webT.Start();
			pullT.Start();
		}

		public MephitisWeb(Serial serial) : base( serial )
		{

		}

		public override bool OnMoveOver(Mobile m)
		{
			bool temp = base.OnMoveOver(m);
			if (mep != null && m != null && m.Alive && mep.Alive && (m.Player || ((BaseCreature)m).Controlled))
			{
				Timer pullT = new PullTimer(mep, m, this);
				m.Paralyze(TimeSpan.FromSeconds(10));
				pullT.Start();
			}
			return temp;
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
			mep = (CalebMephitis)reader.ReadMobile();
		}


		public class WebTimer : Timer
		{
			private Mobile target;
			private MephitisWeb web;
			private CalebMephitis mep;

			public WebTimer(CalebMephitis mep, Mobile target, MephitisWeb web) : base(TimeSpan.FromSeconds( 20 + Utility.Random(30)))
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
			private MephitisWeb web;
			private CalebMephitis mep;

			public PullTimer(CalebMephitis mep, Mobile target, MephitisWeb web) : base(TimeSpan.FromSeconds( 4 ))
			{
				this.mep = mep;
				this.target = target;
				this.web = web;
			}

			protected override void OnTick()
			{
				if (mep != null && target != null && target.Alive && mep.Alive
					   && target.Location == web.Location && target.Map == web.Map)
				{

					target.Location = mep.Location;
					mep.Combatant = target;
				}

			}

		}
	}


}