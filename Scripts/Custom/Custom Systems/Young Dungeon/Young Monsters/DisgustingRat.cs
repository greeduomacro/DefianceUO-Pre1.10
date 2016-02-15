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
	[CorpseName( "a giant rat corpse" )]
	public class DisgustingRat : BaseCreature
	{
		[Constructable]
		public DisgustingRat() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a disgusting rat";
			Body = 0xD7;
			BaseSoundID = 0x188;
			Hue = 1717;

			SetStr( 10, 40 );
			SetDex( 46, 65 );
			SetInt( 16, 30 );

			SetHits( 35, 52 );
			SetMana( 0 );

			SetDamage( 4, 8 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 15, 20 );
			SetResistance( ResistanceType.Fire, 5, 10 );
			SetResistance( ResistanceType.Poison, 25, 35 );

			SetSkill( SkillName.MagicResist, 25.1, 30.0 );
			SetSkill( SkillName.Tactics, 29.3, 44.0 );
			SetSkill( SkillName.Wrestling, 29.3, 44.0 );

			Fame = 300;
			Karma = -300;

			VirtualArmor = 18;

			Tamable = false;

			Container pack = new Backpack();
			pack.Movable = false;


		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );

		}

		public override int Meat{ get{ return 2; } }
		public override int Hides{ get{ return 6; } }

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 10 ) < 1 )
			c.AddItem( new BlueBall() );

			base.OnDeath( c );
		}



		public DisgustingRat(Serial serial) : base(serial)
		{
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