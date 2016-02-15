using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class BMregion : RegionControl
	{
		[Constructable]
		public BMregion() : base()
		{
			Name = "BomberManArea Controller";
			RegionName = "BomberManArea";
			this.AllowBenifitNPC = false;
			this.AllowBenifitPlayer = false;
			this.AllowHarmNPC = false;
			this.AllowHarmPlayer = true;
			this.AllowHousing = false;
			this.AlwaysGrey = true;
			this.CanBeDamaged = true;
			this.CanHeal = false;
			this.CanLootNPCCorpse = false;
			this.CanLootPlayerCorpse = false;
			this.CanMountEthereal = false;
			this.CannotEnter = true;
			this.CannotLootOwnCorpse = false;
			this.CannotTakeRewards = true;
			this.CannotTrade = true;
			this.CanRessurect = false;
			this.CanUsePotions = false;
			this.CanUseStuckMenu = false;
			this.DeleteCorpsesOnDeath = true;
			this.IsGuarded = false;
			this.NoFactionEffects = true;
			this.NoFameKarma = true;
			this.NoMurderCounts = true;
			this.NoPvPPoints = true;
			this.Priority = CustomRegionPriority.HighestPriority;

			int i;
			for (i = 0; i < RestrictedSkills.Length; i++ )
			{
				RestrictedSkills[i] = true;
			}

			for (i = 0; i < RestrictedSpells.Length; i++)
			{
				RestrictedSpells[i] = true;
			}
		}

		public BMregion(Serial serial) : base(serial)
		{
		}

		public override void OnDoubleClick(Mobile m)
		{
			if (m != null)
				m.SendMessage(34,"This region controler has a default configuration that fits for bomberman games. Only the area needs to defined.");

			base.OnDoubleClick(m);
		}


		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); //version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}