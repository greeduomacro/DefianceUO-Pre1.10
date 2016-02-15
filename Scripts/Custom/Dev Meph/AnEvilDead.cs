using System;
using System.Collections;
using Server;
using Server.Misc;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
	[CorpseName( "an evil dead's corpse" )]
	public class EvilDead : BaseCreature
	{
		[Constructable]
		public EvilDead () : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an evil dead";
			Body = 3;
			BaseSoundID = 471;
			Hue = 1175;


			SetStr( 100, 110 );
			SetDex( 20, 25 );
			SetInt( 500, 681 );

			SetHits( 1000, 1184 );

			SetDamage( 40, 40 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Fire, 25 );
			SetDamageType( ResistanceType.Energy, 25 );

			SetResistance( ResistanceType.Physical, 65, 80 );
			SetResistance( ResistanceType.Fire, 60, 80 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Anatomy, 80.1, 99.0 );
			SetSkill( SkillName.EvalInt, 150.5, 170.0 );
			SetSkill( SkillName.Magery, 95.5, 100.0 );
			SetSkill( SkillName.Meditation, 150.0, 150.0 );
			SetSkill( SkillName.MagicResist, 0.0, 0.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.Wrestling, 200.1, 210.0 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 0;

			PackGold( 348, 784 );
			PackItem( new BagOfReagents() );
			if ( Utility.Random( 300 ) < 1 ) PackItem( new HalloweenScareCrow() );



		}



		public override bool BardImmune{ get{ return true; } }
		public override bool AutoDispel{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override int TreasureMapLevel{ get{ return 5; } }
		public override int Meat{ get{ return 1; } }

		public EvilDead( Serial serial ) : base( serial )
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