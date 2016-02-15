using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an old soul's corpse" )]
	public class ASoulDrainer : BaseCreature
	{
		[Constructable]
		public ASoulDrainer () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a soul drainer";
			Body = 159;
			BaseSoundID = 278;
			Hue = 1282;

			SetStr( 526, 615 );
			SetDex( 66, 85 );
			SetInt( 574, 896 );

			SetHits( 316, 369 );

			SetDamage( 25, 51 );

			SetDamageType( ResistanceType.Physical, 0 );
			SetDamageType( ResistanceType.Poison, 50 );
			SetDamageType( ResistanceType.Energy, 50 );

			SetResistance( ResistanceType.Physical, 55, 65 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 50, 60 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.EvalInt, 140.1, 168.0 );
			SetSkill( SkillName.Magery, 110.1, 120.0 );
			SetSkill( SkillName.Meditation, 100.4, 110.0 );
			SetSkill( SkillName.MagicResist, 120.1, 135.0 );
			SetSkill( SkillName.Tactics, 80.1, 100.0 );
			SetSkill( SkillName.Wrestling, 80.1, 100.0 );

			Fame = 12500;
			Karma = -12500;

			VirtualArmor = 70;
		}

		public override void GenerateLoot( bool spawning )
		{
			AddLoot( LootPack.FilthyRich, 2 );
			if ( !spawning )
				if ( Utility.Random( 100 ) < 25 ) PackItem( new SoulGem() );
		}

		public override int TreasureMapLevel{ get{ return 5; } }

		public ASoulDrainer( Serial serial ) : base( serial )
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