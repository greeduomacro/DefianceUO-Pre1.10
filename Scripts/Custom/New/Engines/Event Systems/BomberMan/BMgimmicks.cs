using System;
using Server.Mobiles;

namespace Server.Items
{
	public class BMBasegimmick : Item
	{
		private BomberManGame m_game;

		public BMBasegimmick(BomberManGame game) : base(14138)
		{
			Movable = false;
			m_game = game;
			Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(SelfDestructionCallback));
		}

		public BMBasegimmick(Serial serial) : base(serial)
		{
		}

		public void SelfDestructionCallback()
		{
			this.Delete();
		}

		public override bool OnMoveOver(Mobile m)
		{
			if (m == null || !(m is PlayerMobile) || !m.Alive)
				return true;

			if (m_game == null || m_game.Running == false)
			{
				this.Delete();
				return true;
			}

			Item bombplacer = m.Backpack.FindItemByType(typeof(BMbombplacer), true);
			if (bombplacer != null && bombplacer is BMbombplacer)
			{
				ApplyEffect(m, (BMbombplacer)bombplacer);
				((BMbombplacer)bombplacer).PlayerStats.GimmicksTaken++;
			}

			this.Delete();
			return true;
		}

		public virtual void ApplyEffect(Mobile m, BMbombplacer bp)
		{
			if (m == null || bp == null)
				return;

			Effects.PlaySound(this.Location, this.Map, 1317);
		}

		public override void Serialize(GenericWriter writer)
		{
		}

		public override void Deserialize(GenericReader reader)
		{
		}
	}

	public class BMrangebonus : BMBasegimmick
	{
		public BMrangebonus(BomberManGame game) : base(game)
		{

			Name = "range bonus";
			Hue = 1365; //blue
		}

		public override void ApplyEffect(Mobile from, BMbombplacer bp)
		{
			if (from == null || bp == null)
				return;

			base.ApplyEffect(from, bp);

			bp.DetonationRange++;
			from.SendMessage(38, "Your bomb detonation range increased!");
			Effects.PlaySound(this.Location, this.Map, 1317);
			this.Delete();
		}

		public BMrangebonus(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
		}

		public override void Deserialize(GenericReader reader)
		{
		}
	}

	public class BMdamagebonus : BMBasegimmick
	{
		public BMdamagebonus(BomberManGame game) : base(game)
		{
			Name = "damage bonus";
			Hue = 38; // red

		}

		public override void ApplyEffect(Mobile from, BMbombplacer bp)
		{
			if (from == null || bp == null)
				return;

			base.ApplyEffect(from, bp);

			bp.DetonationDamage += 20;
			from.SendMessage(38, "Your bomb detonation damage increased!");
			Effects.PlaySound(this.Location, this.Map, 1317);
			this.Delete();
		}

		public BMdamagebonus (Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
		}

		public override void Deserialize(GenericReader reader)
		{
		}
	}

	public class BMbombbonus : BMBasegimmick
	{
		public BMbombbonus(BomberManGame game) : base(game)
		{
			Name = "extra bomb";
			Hue = 1367; // green
		}

		public override void ApplyEffect(Mobile from, BMbombplacer bp)
		{
			if (from == null || bp == null)
				return;

			base.ApplyEffect(from, bp);

			from.SendMessage(38, "You got an extra bomb!");
			bp.MaxNumberOfBombs++;
			Effects.PlaySound(this.Location, this.Map, 1317);
			this.Delete();
		}

		public BMbombbonus(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
		}

		public override void Deserialize(GenericReader reader)
		{
		}
	}

	public class BMfullhits : BMBasegimmick
	{
		public BMfullhits(BomberManGame game) : base(game)
		{
			Name = "full hits";
			Hue = 1161; // yellow
		}

		public override void ApplyEffect(Mobile from, BMbombplacer bp)
		{
			if (from == null || bp == null)
				return;

			base.ApplyEffect(from, bp);

			from.Hits = from.HitsMax;
			from.SendMessage(38, "You were healed.");
			Effects.PlaySound(this.Location, this.Map, 1317);
			this.Delete();
		}

		public BMfullhits(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
		}

		public override void Deserialize(GenericReader reader)
		{
		}
	}
}