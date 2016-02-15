using System;

namespace Server.Items
{
	class CTFCrystal : Item
	{
		[Constructable]
		public CTFCrystal() : base(0x1ED0)
		{
			Light = LightType.Circle150;
			Name = "a ctf announcer";
		}

		public CTFCrystal(Serial serial) : base(serial)
		{
		}


		public override void OnDoubleClick(Mobile from)
		{
			if (from.AccessLevel < AccessLevel.GameMaster) return;
			from.SendMessage("Please target the CTF game stone you want to link this crystal to.");
			from.Target = new InternalTarget(this);
		}

		public void Announce(string message)
		{
			if (this.RootParent is Mobile)
				((Mobile)this.RootParent).SendMessage(message);
			else
				PublicOverheadMessage(Server.Network.MessageType.Regular, 0, false, message);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int) 0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}

		private class InternalTarget : Server.Targeting.Target
		{
			private CTFCrystal m_CTFCrystal;
			public InternalTarget(CTFCrystal ctfCrystal) : base(-1, false, Server.Targeting.TargetFlags.None)
			{
				m_CTFCrystal = ctfCrystal;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (m_CTFCrystal == null || m_CTFCrystal.Deleted || !(targeted is CTFGame)) return;
				((CTFGame)targeted).AddCrystal(m_CTFCrystal);
				from.SendMessage("The crystal has successfully been linked.");
			}
		}
	}
}