using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Targeting;
using Server.ContextMenus;
using Server.Gumps;
using Server.Misc;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
	[CorpseName( "a jwilson corpse" )]
	public class OddSlime : BaseCreature
	{
		[Constructable]
		public OddSlime() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Hue = Utility.RandomList(0x89C,0x8A2,0x8A8,0x8AE);
			this.Body = 0x33;
			this.Name = ("an odd looking slime");
			this.VirtualArmor = 8;


			this.InitStats(Utility.Random(22,13),Utility.Random(16,6),Utility.Random(16,5));

			this.Skills[SkillName.Wrestling].Base = Utility.Random(24,17);
			this.Skills[SkillName.Tactics].Base = Utility.Random(18,14);
			this.Skills[SkillName.MagicResist].Base = Utility.Random(15,6);
			this.Skills[SkillName.Poisoning].Base = Utility.Random(31,20);

			this.Fame = Utility.Random(0,1249);
			this.Karma = Utility.Random(0,-624);

			SetHits(30,40);
			Tamable = false;
			Container pack = new Backpack();
			pack.Movable = false;
		}


		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 10 ) < 1 )
			c.AddItem( new BlueBall() );

			base.OnDeath( c );
		}


		public OddSlime(Serial serial) : base(serial)
		{
		}

		public override int GetAngerSound()
		{
			return 0x1C8;
		}

		public override int GetIdleSound()
		{
			return 0x1C9;
		}

		public override int GetAttackSound()
		{
			return 0x1CA;
		}

		public override int GetHurtSound()
		{
			return 0x1CB;
		}

		public override int GetDeathSound()
		{
			return 0x1CC;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}