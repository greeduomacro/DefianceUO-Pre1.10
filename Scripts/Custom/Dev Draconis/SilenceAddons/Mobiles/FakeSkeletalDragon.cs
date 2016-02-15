using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a skeletal dragon corpse" )]
	public class FakeSkeletalDragon : BaseFakeMob
	{
		[Constructable]
		public FakeSkeletalDragon () : base( AIType.AI_Mage )
		{
			Name = "a skeletal dragon";
			Body = 104;
			BaseSoundID = 0x488;

			SetStr( 800 );
			SetDex( 800 );
			SetInt( 800 );

			SetHits( 1600 );

			SetDamage( 29, 35 );

			SetDamageType( ResistanceType.Physical, 75 );
			SetDamageType( ResistanceType.Fire, 25 );

			SetResistance( ResistanceType.Physical, 75, 80 );
			SetResistance( ResistanceType.Fire, 40, 60 );
			SetResistance( ResistanceType.Cold, 40, 60 );
			SetResistance( ResistanceType.Poison, 70, 80 );
			SetResistance( ResistanceType.Energy, 40, 60 );

			SetSkill( SkillName.EvalInt, 140.0 );
			SetSkill( SkillName.Magery, 140.0 );
			SetSkill( SkillName.MagicResist, 250.0 );
			SetSkill( SkillName.Tactics, 140.0 );
			SetSkill( SkillName.Wrestling, 140.0 );

			Fame = 1000;
			Karma = -100;

			VirtualArmor = 100;
		}

		public override bool HasBreath{ get{ return true; } }
		public override int BreathFireDamage{ get{ return 0; } }
		public override int BreathColdDamage{ get{ return 100; } }
		public override int BreathEffectHue{ get{ return 0x480; } }

		public override bool AutoDispel{ get{ return true; } }
		public override int Meat{ get{ return 19; } }
		public override int Hides{ get{ return 20; } }
		public override HideType HideType{ get{ return HideType.Barbed; } }

		public FakeSkeletalDragon( Serial serial ) : base( serial )
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