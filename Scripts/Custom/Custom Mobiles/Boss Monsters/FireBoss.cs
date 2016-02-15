using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a dragon corpse" )]
	public class FireBoss : BaseCreature
	{
		[Constructable]
		public FireBoss () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an ancient fire wyrm";
			Body = 46;
			Hue = 1157;
			BaseSoundID = 362;

			SetStr( 1096, 1185 );
			SetDex( 86, 175 );
			SetInt( 686, 775 );

			SetHits( 1658, 1711 );

			SetDamage( 129, 135 );

			SetDamageType( ResistanceType.Physical, 75 );
			SetDamageType( ResistanceType.Fire, 25 );

			SetResistance( ResistanceType.Physical, 65, 90 );
			SetResistance( ResistanceType.Fire, 80, 90 );
			SetResistance( ResistanceType.Cold, 70, 80 );
			SetResistance( ResistanceType.Poison, 60, 70 );
			SetResistance( ResistanceType.Energy, 60, 70 );

			SetSkill( SkillName.EvalInt, 180.1, 200.0 );
			SetSkill( SkillName.Magery, 180.1, 200.0 );
			SetSkill( SkillName.Meditation, 152.5, 275.0 );
			SetSkill( SkillName.MagicResist, 90.5, 100.1 );
			SetSkill( SkillName.Tactics, 197.6, 200.0 );
			SetSkill( SkillName.Wrestling, 197.6, 200.0 );

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 70;

			PackGem();
			PackGem();
			PackPotion();
			PackGold( 11700, 21100 );
			PackScroll( 2, 8 );
			PackMagicItems( 3, 5, 0.95, 0.95 );
			PackMagicItems( 4, 5, 0.80, 0.65 );
			PackMagicItems( 4, 5, 0.80, 0.65 );
			PackSlayer();

                 }




		public override int GetIdleSound()
		{
			return 0x2D3;
		}

		public override int GetHurtSound()
		{
			return 0x2D1;
		}

		public override bool HasBreath{ get{ return true; } } // fire breath enabled
		public override bool AutoDispel{ get{ return true; } }
		public override HideType HideType{ get{ return HideType.Barbed; } }
		public override int Hides{ get{ return 40; } }
		public override int Meat{ get{ return 19; } }
		public override int Scales{ get{ return 12; } }
		public override ScaleType ScaleType{ get{ return (ScaleType)Utility.Random( 4 ); } }
		public override Poison PoisonImmune{ get{ return Poison.Regular; } }
		public override int TreasureMapLevel{ get{ return 5; } }

		public FireBoss( Serial serial ) : base( serial )
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