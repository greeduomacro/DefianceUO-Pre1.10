using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "an iron elemental corpse" )]
	public class IronElemental : BaseCreature
	{


		[Constructable]
		public IronElemental() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an iron elemental";
			Body = 14;
			BaseSoundID = 268;
			Hue = 2103;

			SetStr( 189, 214 );
			SetDex( 78, 100 );
			SetInt( 71, 92 );

			SetHits( 158, 200 );

			SetDamage( 12, 24 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 30, 35 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 10, 20 );
			SetResistance( ResistanceType.Poison, 15, 25 );
			SetResistance( ResistanceType.Energy, 15, 25 );

			SetSkill( SkillName.MagicResist, 80.1, 110.0 );
			SetSkill( SkillName.Tactics, 70.1, 105.0 );
			SetSkill( SkillName.Wrestling, 85.1, 100.0 );

			Fame = 5500;
			Karma = -5500;

			VirtualArmor = 100;

			PackItem( new IronIngot( 15 ) );
			PackItem( new BlackPearl() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 2 );
			AddLoot( LootPack.Gems );
		}

		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }

		public override int TreasureMapLevel{ get{ return 2; } }
		public override bool BardImmune{ get{ return true; } }

		public IronElemental( Serial serial ) : base( serial )
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