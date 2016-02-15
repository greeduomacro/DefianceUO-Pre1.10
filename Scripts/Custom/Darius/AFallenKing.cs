using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a fallen corpse" )]
	public class AFallenKing : BaseCreature
	{
		[Constructable]
		public AFallenKing() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a fallen king";
			Body = 57;
			BaseSoundID = 451;
			Hue = 1159;

			SetStr( 300, 400 );
			SetDex( 100, 120 );
			SetInt( 36, 60 );

			SetHits( 890, 1140 );

			SetDamage( 35, 50 );

			SetDamageType( ResistanceType.Physical, 40 );
			SetDamageType( ResistanceType.Cold, 60 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.MagicResist, 65.1, 80.0 );
			SetSkill( SkillName.Tactics, 85.1, 100.0 );
			SetSkill( SkillName.Wrestling, 85.1, 95.0 );

			Fame = 10000;
			Karma = -10000;

			VirtualArmor = 100;

			switch ( Utility.Random( 6 ) )
			{
				case 0: PackItem( new PlateArms() ); break;
				case 1: PackItem( new PlateChest() ); break;
				case 2: PackItem( new PlateGloves() ); break;
				case 3: PackItem( new PlateGorget() ); break;
				case 4: PackItem( new PlateLegs() ); break;
				case 5: PackItem( new PlateHelm() ); break;
			}

			PackSlayer();
			PackItem( new Scimitar() );
			PackItem( new WoodenShield() );
		}

		public override void GenerateLoot( bool spawning )
		{
			AddLoot( LootPack.FilthyRich );
			if ( !spawning )
				if ( Utility.Random( 100 ) < 50 ) PackItem( new Bone( 20 ) );

		}
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override int TreasureMapLevel{ get{ return 4; } }


		public AFallenKing( Serial serial ) : base( serial )
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