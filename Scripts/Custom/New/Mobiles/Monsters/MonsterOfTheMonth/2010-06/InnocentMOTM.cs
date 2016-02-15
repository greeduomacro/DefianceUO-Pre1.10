using System;
using Server;
using Server.Mobiles;
using Server.Items;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "a victim's corpse" )]
	public class InnocentMOTM : BaseCreature
	{
		[Constructable]
		public InnocentMOTM() : base( AIType.AI_Mage, FightMode.Agressor, 10, 1, 0.1, 0.3 )
		{
			Name = "an innocent creature";
			Body = Utility.RandomList( 0xE8, 0xE9, 0x22, 0x27, 0x50, 0x65, 0x75, 0x80, 0xA9, 0xAB, 0xDC, 0xDD, 0xE7, 0xEA, 0xED, 0x15, 0x34, 0x3A, 0x41, 0x58 );
			Hue = Utility.RandomList( 9, 1000, 1111, 2003, 1175, 1154, 1159, 1195 );


			SetStr( 200, 300 );
			SetDex( 200, 300 );
			SetInt( 400, 500 );

			SetHits( 10000 );
			SetMana( 4000 );

			SetDamage( 500 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 25, 30 );
			SetResistance( ResistanceType.Cold, 10, 15 );

			SetSkill( SkillName.MagicResist, 17.6, 25.0 );
			SetSkill( SkillName.Tactics, 67.6, 85.0 );
			SetSkill( SkillName.Wrestling, 100.0 );

			Fame = 1000;
			Karma = 1000;

			VirtualArmor = 40;

			Tamable = false;
		}

		public override int Meat{ get{ return 5; } }
		public override int Hides{ get{ return 1; } }

		public override void OnDeath( Container c )
		{
			if (Utility.Random( 150 ) <  1 )
			c.DropItem( new InnocentMOTMRare() );

            	base.OnDeath( c );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 1 );
		}

		public InnocentMOTM(Serial serial) : base(serial)
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