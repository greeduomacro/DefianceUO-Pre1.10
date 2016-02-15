using Server;
using Server.Targeting;

namespace Server.Items
{
    class BaseTinkerTrap : Item
    {
        public BaseTinkerTrap() : base(7194)
        {
			Weight = 4;
        }

        public BaseTinkerTrap(Serial serial):base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == null && !from.Deleted && from.Backpack != null)
                return;

			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
				return;
			}

            if (from.Skills.Tinkering.Base < 60)
            {
                from.SendMessage("You need to have 60 tinkering to use this item;");
                return;
            }

            from.BeginTarget(1, false, TargetFlags.None, new TargetCallback(OnTrap_OnTarget));
            from.SendMessage("Target the container you want to trap.");
        }

        public virtual void OnTrap_OnTarget(Mobile from, object obj)
        {
            if (from == null || from.Deleted)
                return;

            if (obj is LockableContainer)
            {
                LockableContainer cont = (LockableContainer)obj;

				if (cont.TrapType != TrapType.None)
				{
					from.SendMessage("That container is already trapped.");
					return;
				}

				if (!cont.Locked)
				{
					from.SendMessage("The container has to be locked to trap it.");
					return;
				}
            }
            else
            {
                from.SendMessage("You can not trap this.");
            }
            return;
        }

		public void SuccessfullyTrapped(Mobile from)
		{
			if (from == null || from.Deleted)
				return;

			from.SendMessage("You successfully trapped the container.");
			from.PlaySound(from.Female ? 794 : 1066);
			this.Delete();
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

	class ExplosionTinkerTrap : BaseTinkerTrap
	{
		[Constructable]
		public ExplosionTinkerTrap()
			: base()
		{
			Name = "tinker explosion trap";
			Hue = 34;
		}

		public ExplosionTinkerTrap(Serial serial)
			: base(serial)
        {
        }

		public override void OnTrap_OnTarget(Mobile from, object obj)
		{
			base.OnTrap_OnTarget(from, obj);
			LockableContainer cont = obj as LockableContainer;
			if (cont != null && cont.Locked)
			{
//				cont.TinkerTrapCreator = from;
				cont.TrapType = TrapType.ExplosionTrap;
				cont.TrapPower = CalculateExplosionTrapDamage(from);
				SuccessfullyTrapped(from);
			}

		}

		public int CalculateExplosionTrapDamage(Mobile from)
		{
			double skillvalue = from.Skills.Tinkering.Base;

			if (skillvalue >= 100)
				return (int)(80 + Utility.Random(40));
			else
				return (int)(skillvalue * 0.8 + Utility.Random(10));
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

	class PoisonTinkerTrap : BaseTinkerTrap
	{
		[Constructable]
		public PoisonTinkerTrap()
			: base()
		{
			Name = "tinker poison trap";
            Hue = 1370;
		}

		public PoisonTinkerTrap(Serial serial)
			: base(serial)
		{
		}

		public override void OnTrap_OnTarget(Mobile from, object obj)
		{
			base.OnTrap_OnTarget(from, obj);
			LockableContainer cont = obj as LockableContainer;
			if (cont != null && cont.Locked)
			{
//				cont.TinkerTrapCreator = from;
				cont.TrapType = TrapType.PoisonTrap;
				CalculatePoisonType(from, cont);
				SuccessfullyTrapped(from);
			}

		}

		public void CalculatePoisonType(Mobile from, LockableContainer cont)
		{
			if (from == null || cont == null)
				return;

			int p;
			double skillvalue = from.Skills.Tinkering.Base;

			if ( skillvalue < 70.0 )
			{
				cont.TrapLevel = 2; //Regular Poison
				cont.TrapPower = 50;
			}
			else if ( skillvalue < 80.0 )
			{
				cont.TrapLevel = 3; //Greater Poison
				cont.TrapPower = 70;
			}
			else //if (skillvalue < 90)
			{
				cont.TrapLevel = 4; //Deadly Poison
				cont.TrapPower = 85;
			}

			if ( cont.TrapLevel < 4 )
			{
				if ( skillvalue >= Utility.RandomDouble() )
				{
					cont.TrapLevel += 1;
					cont.TrapPower = 100;
				}
			}
			else if ( skillvalue >= Utility.RandomDouble() )
				cont.TrapPower = 100;

			return;
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