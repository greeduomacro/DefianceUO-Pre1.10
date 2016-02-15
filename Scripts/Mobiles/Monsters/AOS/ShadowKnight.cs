using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a shadow knight corpse" )]
	public class ShadowKnight : BaseCreature
	{
		[Constructable]
		public ShadowKnight() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "shadow knight" );
			Title = "the shadow knight";
			Body = 311;
			BaseSoundID = 357;

			SetStr( 2250 );
			SetDex( 100 );
			SetInt( 100 );

			SetHits( 10000 );

			SetDamage( 20, 30 );

			SetDamageType( ResistanceType.Physical, 60 );
			SetDamageType( ResistanceType.Cold, 40 );

			SetResistance( ResistanceType.Physical, 90 );
			SetResistance( ResistanceType.Fire, 65 );
			SetResistance( ResistanceType.Cold, 75 );
			SetResistance( ResistanceType.Poison, 75 );
			SetResistance( ResistanceType.Energy, 55 );

			SetSkill( SkillName.Chivalry, 120.0 );
			SetSkill( SkillName.EvalInt, 100.0 );
			SetSkill( SkillName.Magery, 100.0 );
			SetSkill( SkillName.Meditation, 100.0 );
			SetSkill( SkillName.MagicResist, 120.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 100.0 );

			Fame = 25000;
			Karma = -25000;

			VirtualArmor = 54;

			PackGem();
                        PackJewel( 0.20 );
			//PackGold( 400, 550 );


		}

		public override bool BardImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool AutoDispel{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 1; } }

		public ShadowKnight( Serial serial ) : base( serial )
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