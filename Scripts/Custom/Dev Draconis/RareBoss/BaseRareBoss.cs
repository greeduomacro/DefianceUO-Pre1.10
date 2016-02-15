using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Engines.IdolSystem;
using Server.EventPrizeSystem;

namespace Server.Mobiles
{
    [CorpseName("a rare corpse")]
    public abstract class BaseRareBoss : BaseBoss
	{
		public BaseRareBoss() : base( AIType.AI_Mage )
		{
			SetStr( 500, 600 );
			SetDex( 160, 200 );
			SetInt( 600, 700 );

			SetHits( 500, 600 );

			SetDamage( 15, 18 );

			SetSkill( SkillName.EvalInt, 100.1, 110.0 );
			SetSkill( SkillName.Magery, 100.1, 110.0 );
			SetSkill( SkillName.MagicResist, 180.1, 220.0 );
			SetSkill( SkillName.Wrestling, 80.1, 100.0 );
			SetSkill( SkillName.Poisoning, 90.1, 100.0 );
			SetSkill( SkillName.Tactics, 80.1, 100.0 );
			SetSkill( SkillName.Meditation, 110.1, 120.0 );

			Fame = 20000;
			Karma = -20000;

			VirtualArmor = 70;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 2 );
			AddLoot( LootPack.HighScrolls, 2 );
		}

		public override void OnDeath( Container c )
		{
            		if (0.80 >= Utility.RandomDouble())
           		switch (Utility.Random(5))
                	{
                    		case 0: c.DropItem(new SpecialHairDye()); break;
                    		case 1: c.DropItem(new SpecialBeardDye()); break;
                    		case 2: c.DropItem(new ClothingBlessDeed()); break;
                    		case 3: c.DropItem(new NameChangeDeed()); break;
                	}
			base.OnDeath( c );
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 5; } }
		public override bool AlwaysMurderer{ get{ return true; } }

		public BaseRareBoss(AIType aiType) : this(aiType, FightMode.Closest)
		{
		}

		public BaseRareBoss(AIType aiType, FightMode mode) : base(aiType, mode)
		{
		}

		public BaseRareBoss( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}