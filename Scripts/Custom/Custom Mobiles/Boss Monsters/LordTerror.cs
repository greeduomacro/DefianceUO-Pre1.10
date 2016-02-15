using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a hellish corpse" )]
	public class LordTerror : BaseCreature
	{
		[Constructable]
		public LordTerror() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Marnok";
			Title = " Lord Of Terror";
			Body = 318;
			Hue = 137;
			BaseSoundID = 362;

			SetStr( 898, 1030 );
			SetDex( 68, 200 );
			SetInt( 488, 620 );

			SetHits( 858, 999 );

			SetDamage( 29, 35 );

			SetDamageType( ResistanceType.Physical, 75 );
			SetDamageType( ResistanceType.Cold, 25 );

			SetResistance( ResistanceType.Physical, 65, 75 );
			SetResistance( ResistanceType.Fire, 50, 60 );
			SetResistance( ResistanceType.Cold, 45, 55 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 50, 60 );

			SetSkill( SkillName.EvalInt, 100.1, 120.0 );
			SetSkill( SkillName.Magery, 100.1, 120.0 );
			SetSkill( SkillName.Meditation, 100.5, 75.0 );
			SetSkill( SkillName.MagicResist, 100.3, 130.0 );
			SetSkill( SkillName.Tactics, 100.6, 120.0 );
			SetSkill( SkillName.Wrestling, 100.6, 120.0 );

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 70;

			for ( int i = 0; i < 5; ++i )
				PackGem();

			PackGold( 5800, 6100 );
			PackMagicItems( 1, 5, 0.70, 0.15 );
			PackSlayer();

                 }




		public override int GetIdleSound()
		{
			return 0x2D5;
		}

		public override int GetHurtSound()
		{
			return 0x2D1;
		}

		public override bool HasBreath{ get{ return true; } } // fire breath enabled
		public override bool AutoDispel{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override Poison HitPoison{ get{ return Poison.Deadly; } }
		public override int TreasureMapLevel{ get{ return 5; } }

		public override int Meat{ get{ return 19; } }
		public override int Hides{ get{ return 20; } }
		public override int Scales{ get{ return 10; } }
		public override ScaleType ScaleType{ get{ return ScaleType.Black; } }
		public override HideType HideType{ get{ return HideType.Barbed; } }

		public LordTerror( Serial serial ) : base( serial )
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