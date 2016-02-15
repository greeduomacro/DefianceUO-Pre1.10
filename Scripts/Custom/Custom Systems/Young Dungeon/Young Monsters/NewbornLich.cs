using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a liche's corpse" )]
	public class NewbornLich : BaseCreature
	{
		[Constructable]
		public NewbornLich() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.3)
		{
			Name = "a newborn lich";
			Body = 24;
			BaseSoundID = 0x3E9;
			Hue = 1111;

			SetStr( 171, 200 );
			SetDex( 126, 145 );
			SetInt( 276, 305 );

			SetHits( 130, 145);

			SetDamage( 15, 25);

			SetDamageType( ResistanceType.Physical, 10 );
			SetDamageType( ResistanceType.Cold, 40 );
			SetDamageType( ResistanceType.Energy, 50 );

			SetResistance( ResistanceType.Physical, 40, 60 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 55, 65 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.EvalInt, 100.0 );
			SetSkill( SkillName.Magery, 70.1, 80.0 );
			SetSkill( SkillName.Meditation, 85.1, 95.0 );
			SetSkill( SkillName.MagicResist, 80.1, 100.0 );
			SetSkill( SkillName.Tactics, 70.1, 90.0 );

			Fame = 8000;
			Karma = -8000;

			VirtualArmor = 50;
			PackItem( new GnarledStaff() );
			PackReg( 10 );
		}

		public override int TreasureMapLevel{ get{ return 4; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.MedScrolls, 1 );
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 18 ) < 1 )
			c.AddItem( new PurpleBall() );

			base.OnDeath( c );
		}

/*
		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.FeyAndUndead; }
		}
*/

		public override bool CanRummageCorpses{ get{ return true; } }
		//public override bool BleedImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }


		public NewbornLich ( Serial serial ) : base( serial )
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