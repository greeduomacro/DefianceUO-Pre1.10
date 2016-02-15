using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "Keeper's Corpse" )]
	public class DungeonKeeper : BaseCreature
	{
		[Constructable]
		public DungeonKeeper() : base( AIType.AI_Mage, FightMode.Weakest, 10, 1, 0.2, 0.4 )
		{
			Name = "Dungeon Keeper";
			Body = 24;
			BaseSoundID = 1001;
            Hue = 22222;

			SetStr( 900, 999 );
			SetDex( 146, 165 );
			SetInt( 636, 695 );

			SetHits( 250, 303 );

			SetDamage( 6900, 7100 );

			SetDamageType( ResistanceType.Physical, 0 );
			SetDamageType( ResistanceType.Cold, 60 );
			SetDamageType( ResistanceType.Energy, 40 );

			SetResistance( ResistanceType.Physical, 140, 150 );
			SetResistance( ResistanceType.Fire, 130, 140 );
			SetResistance( ResistanceType.Cold, 150, 160 );
			SetResistance( ResistanceType.Poison, 150, 160 );
			SetResistance( ResistanceType.Energy, 140, 150 );

			SetSkill( SkillName.EvalInt, 160.1, 170.0 );
			SetSkill( SkillName.Magery, 120.1, 130.0 );
			SetSkill( SkillName.MagicResist, 320.5, 240.0 );
			SetSkill( SkillName.Tactics, 100.1, 110.0 );
			SetSkill( SkillName.Anatomy, 110.1, 120.0 );
            SetSkill( SkillName.Meditation, 350.1, 360.0);
            SetSkill( SkillName.Wrestling, 100.1, 110.0);

			Fame = 35000;
			Karma = -35000;

			VirtualArmor = 100;
			PackItem( new GnarledStaff() );
			PackNecroReg( 12, 40 );

            if (Utility.Random(50) < 1)
                PackItem(new ClothingBlessDeed());

            if (Utility.Random(235) < 1)
            {
                Item oOrkMask = new OrcishKinMask();
                ((OrcishKinMask)oOrkMask).Name = "Mask of Dungeon Keeper";
                ((OrcishKinMask)oOrkMask).Hue = 22222;
                PackItem(oOrkMask);
            }
		}
/*
		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.FeyAndUndead; }
		}
*/
		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.MedScrolls, 2 );
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		//public override bool BleedImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override int TreasureMapLevel{ get{ return 4; } }

        public DungeonKeeper(Serial serial)
            : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}