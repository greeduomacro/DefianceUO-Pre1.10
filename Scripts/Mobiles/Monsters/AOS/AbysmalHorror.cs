using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an abysmal horror corpse" )]
	public class AbysmalHorror : BaseCreature
	{
		[Constructable]
		public AbysmalHorror() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an abysmal horror";
			Body = 312;
			BaseSoundID = 357;

			SetStr( 2401, 2420 );
			SetDex( 81, 90 );
			SetInt( 401, 420 );

			SetHits( 10000 );

			SetDamage( 15, 19 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Poison, 50 );

			SetResistance( ResistanceType.Physical, 30, 35 );
			SetResistance( ResistanceType.Fire, 100 );
			SetResistance( ResistanceType.Cold, 50, 55 );
			SetResistance( ResistanceType.Poison, 60, 65 );
			SetResistance( ResistanceType.Energy, 77, 80 );

			SetSkill( SkillName.EvalInt, 200.0 );
			SetSkill( SkillName.Magery, 112.6, 117.5 );
			SetSkill( SkillName.Meditation, 200.0 );
			SetSkill( SkillName.MagicResist, 117.6, 120.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 84.1, 88.0 );

			Fame = 26000;
			Karma = -26000;

			VirtualArmor = 54;

			PackGem();
                        PackJewel( 0.10 );
			PackGold( 1200, 1350 );
		}

		public override bool BardImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override int TreasureMapLevel{ get{ return 1; } }
		public override bool AutoDispel{ get{ return true; } }

		public AbysmalHorror( Serial serial ) : base( serial )
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

		public override void OnDeath(Container c)
		{
			base.OnDeath(c);
			if (20 > Utility.Random(100)) //20% drop chance
			{
				int r = Utility.Random(100);
				if (r == 0) c.DropItem(new ClothingBlessDeed());
				else if (r >= 1 && r < 6) c.DropItem(new MetalChest());
				else if (r >= 6 && r < 11) c.DropItem(new DecorativeAxeNorthDeed());
				else if (r >= 11 && r < 21) c.DropItem(new DwarvenForgeSouth2AddonDeed());
				else if (r >= 21 && r < 31) c.DropItem(new DwarvenForgeEastAddonDeed());
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