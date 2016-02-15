using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a flesh renderer corpse" )]
	public class FleshRenderer : BaseCreature
	{
		[Constructable]
		public FleshRenderer() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a flesh renderer";
			Body = 315;
			BaseSoundID = 660;

			SetStr( 2201, 2260 );
			SetDex( 201, 210 );
			SetInt( 221, 260 );

			SetHits( 10000 );

			SetDamage( 19, 27 );

			SetDamageType( ResistanceType.Physical, 80 );
			SetDamageType( ResistanceType.Poison, 20 );

			SetResistance( ResistanceType.Physical, 80, 90 );
			SetResistance( ResistanceType.Fire, 50, 60 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 70, 80 );

			SetSkill( SkillName.MagicResist, 155.1, 160.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 90.1, 100.0 );

			Fame = 23000;
			Karma = -23000;

			VirtualArmor = 24;

			PackGem();
			//PackGold( 500, 650 );
		}

		public override bool Unprovokable{ get{ return true; } }
		public override bool Uncalmable{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool AutoDispel{ get{ return true; } }

		public override int TreasureMapLevel{ get{ return 1; } }

		public FleshRenderer( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void GenerateLoot()
		{
			//AddLoot( LootPack.UltraRich, 4 );
			AddLoot( LootPack.FilthyRich, 10 );
		}

		public override void OnDeath(Container c)
		{
			base.OnDeath(c);
			if (20 > Utility.Random(100)) //20% drop chance
			{
				int r = Utility.Random(100);
				if (r == 0) c.DropItem(new ClothingBlessDeed());
				else if (r >= 1 && r < 6) c.DropItem(new MetalChest());
				else if (r >= 6 && r < 11) c.DropItem(new DecorativeAxeNorthDeed());
				else if (r >= 11 && r < 21) c.DropItem(new BrownBearRugSouthDeed());
				else if (r >= 21 && r < 31) c.DropItem(new BrownBearRugEastDeed());
				else if (r >= 31 && r < 41) c.DropItem(new StackedArrows());
				else if (r >= 41 && r < 51) c.DropItem(new BronzeIngots());
				else if (r >= 51 && r < 61) c.DropItem(new StackedShafts());
				else if (r >= 61 && r < 71) c.DropItem(new RareFeathers());
				else c.DropItem(new BloodPentagramPart(Utility.RandomMinMax(0, 4)));
			}
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}