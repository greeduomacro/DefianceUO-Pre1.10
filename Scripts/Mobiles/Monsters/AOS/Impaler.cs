using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an impaler corpse" )]
	public class Impaler : BaseCreature
	{
		[Constructable]
		public Impaler() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "impaler" );
			Body = 306;
			BaseSoundID = 1200;

			SetStr( 2190 );
			SetDex( 45 );
			SetInt( 190 );

			SetHits( 10000 );

			SetDamage( 31, 35 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 90 );
			SetResistance( ResistanceType.Fire, 60 );
			SetResistance( ResistanceType.Cold, 75 );
			SetResistance( ResistanceType.Poison, 60 );
			SetResistance( ResistanceType.Energy, 100 );

			SetSkill( SkillName.Meditation, 120.0 );
			SetSkill( SkillName.Poisoning, 160.0 );
			SetSkill( SkillName.MagicResist, 100.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 80.0 );

			Fame = 24000;
			Karma = -24000;

			VirtualArmor = 49;

			PackGem();
                        PackJewel( 0.10 );
			//PackGold( 1200, 1350 );
		}

		public override bool BardImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return (0.8 >= Utility.RandomDouble() ? Poison.Greater : Poison.Deadly); } }
		public override bool AutoDispel{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 1; } }

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

		public Impaler( Serial serial ) : base( serial )
		{
		}

		public override void GenerateLoot()
		{
			//AddLoot( LootPack.UltraRich, 4 );
			AddLoot( LootPack.FilthyRich, 10 );
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