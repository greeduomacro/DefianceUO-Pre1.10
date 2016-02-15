using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a water elemental corpse" )]
	public class ADeepWaterElemental : BaseCreature
	{

		[Constructable]
		public ADeepWaterElemental () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a deep water elemental";
			Body = 16;
			BaseSoundID = 278;
			Hue = 2124;

			SetStr( 358, 405 );
			SetDex( 112, 147 );
			SetInt( 1574, 2474 );

			SetHits( 1825, 2341 );

			SetDamage( 30, 35 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 10, 25 );
			SetResistance( ResistanceType.Cold, 10, 25 );
			SetResistance( ResistanceType.Poison, 60, 70 );
			SetResistance( ResistanceType.Energy, 5, 10 );

			SetSkill( SkillName.EvalInt, 110.1, 120.0 );
			SetSkill( SkillName.Magery, 110.1, 130.0 );
			SetSkill( SkillName.MagicResist, 120.0, 120.0 );
			SetSkill( SkillName.Tactics, 99.9, 105.0 );
			SetSkill( SkillName.Wrestling, 100.0, 100.0 );

			Fame = 10500;
			Karma = -10500;

			VirtualArmor = 50;
			CanSwim = true;

			PackItem( new BlackPearl( 3 ) );
			PackItem( new Gold( 100, 200 ) );

		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 2 );
			AddLoot( LootPack.Potions );
		}

		public override bool BardImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		public override int TreasureMapLevel{ get{ return 4; } }

		public ADeepWaterElemental( Serial serial ) : base( serial )
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