using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class RechargeOrb : Item
	{
		[Constructable]
		public RechargeOrb() : base(6249)
		{
			Movable = true;
			Weight = 1;
			Name = "a righteous orb";
			Hue = 1153;
			LootType = LootType.Regular;
		}

		public RechargeOrb(Serial serial) : base(serial)
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

		public override void OnDoubleClick(Mobile from)
		{
			if (from == null || from.Backpack == null)
				return;

			if (!IsChildOf(from.Backpack))
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
			else
			{
				from.RevealingAction();
				from.BeginTarget(1, false, TargetFlags.None, new TargetCallback(RechargeOrb_OnTarget));
			}
		}

		public void RechargeOrb_OnTarget(Mobile from, object o)
		{
			if (from == null)
				return;

			MysticKeys keys = o as MysticKeys;
			if (keys == null || keys.Deleted)
			{
				from.SendMessage("You can not use this on that.");
				return;
			}

			if (keys.Charged)
				from.SendMessage("Those mystical keys are already charged!");
			else
			{
				if (keys.LastRecharged + TimeSpan.FromDays(7.0) < DateTime.Now)
				{
					keys.Charge();
					from.SendMessage("You use the orb to recharge your mystical keys.");
					Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0, 0, 0, 0, 0, 5060, 0);
					Effects.PlaySound(from.Location, from.Map, 0x243);
					Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(from.X - 6, from.Y - 6, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
					Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(from.X - 4, from.Y - 6, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
					Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(from.X - 6, from.Y - 4, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
					Effects.SendTargetParticles(from, 0x375A, 35, 90, 0x00, 0x00, 9502, (EffectLayer)255, 0x100);
					this.Delete();
				}
				else
				{
					TimeSpan t = (keys.LastRecharged + TimeSpan.FromDays(7) - DateTime.Now);
					from.SendMessage("You have to wait {0:F0} days before recharging the keys again.", Math.Ceiling(t.TotalDays));
				}
			}
		}
	}
}