using Server;
using System;
using Server.Items;
using Server.Targeting;
using System.Collections;

namespace Server.Items
{
	public interface IClaimableLoot
	//A class would have been cleaner but I didnt want the serialization
	//to explode due to existing items.
	{
		bool IsLoot { get; set; }
	}

	public class ItemClaimer : SelfDestructingItem
	{
		[Constructable]
		public ItemClaimer() : base()
		{
			Weight = 1;
			Hue = 1159;
			Name = "Reward Claimer";
			LootType = LootType.Blessed;
			Movable = false;
			ShowTimeLeft = true;
			TimeLeft = 18000;
			Running = true;
			ItemID = 0x9F6;
		}

		public ItemClaimer(Serial serial) : base(serial)
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
				if (from == null)
					return;

				from.RevealingAction();
				from.BeginTarget(1, false, TargetFlags.None, new TargetCallback(ItemClaimer_OnTarget));
		}

		public void ItemClaimer_OnTarget(Mobile from, object target)
		{
			if (from == null || from.Backpack == null || target == null)
				return;

			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
			}
			else
			{
				if (target is Item && target is IClaimableLoot && ((IClaimableLoot)target).IsLoot)
				{
					Item i = target as Item;
					from.AddToBackpack(i);
					i.Movable = true;
					((IClaimableLoot)target).IsLoot = false;
					from.SendMessage("You claimed your reward.");
					this.Delete();
				}
			}
		}
	}
}