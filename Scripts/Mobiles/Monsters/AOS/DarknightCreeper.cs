using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a darknight creeper corpse" )]
	public class DarknightCreeper : BaseCreature
	{
		[Constructable]
		public DarknightCreeper() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "darknight creeper" );
			Body = 313;
			BaseSoundID = 471;

			SetStr( 2301, 2330 );
			SetDex( 101, 110 );
			SetInt( 301, 330 );

			SetHits( 10000 );

			SetDamage( 27, 29 );

			SetDamageType( ResistanceType.Physical, 85 );
			SetDamageType( ResistanceType.Poison, 15 );

			SetResistance( ResistanceType.Physical, 60 );
			SetResistance( ResistanceType.Fire, 60 );
			SetResistance( ResistanceType.Cold, 100 );
			SetResistance( ResistanceType.Poison, 90 );
			SetResistance( ResistanceType.Energy, 75 );

			SetSkill( SkillName.EvalInt, 118.1, 120.0 );
			SetSkill( SkillName.Magery, 112.6, 120.0 );
			SetSkill( SkillName.Meditation, 150.0 );
			SetSkill( SkillName.Poisoning, 120.0 );
			SetSkill( SkillName.MagicResist, 90.1, 90.9 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 90.1, 90.9 );

			Fame = 22000;
			Karma = -22000;

			VirtualArmor = 34;

			PackGem();
                        PackJewel( 0.10 );
			//PackGold( 800, 1150 );
		}

		public override bool BardImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Lethal; } }
		public override bool AutoDispel{ get{ return true; } }

		public override int TreasureMapLevel{ get{ return 1; } }

		public DarknightCreeper( Serial serial ) : base( serial )
		{
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

		public override void Serialize(GenericWriter writer)
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