using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a harpy boss corpse" )]
	public class HarpyBoss : BaseCreature
	{
		[Constructable]
		public HarpyBoss () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Queen of Covetous";
			Body = 73;
			BaseSoundID = 357;
			Hue = 1360;

			SetStr( 1000, 1000 );
			SetDex( 300, 300 );
			SetInt( 15000, 15000 );

			SetHits( 1000, 1000 );

			SetDamage( 1, 5 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Fire, 25 );
			SetDamageType( ResistanceType.Energy, 25 );

			SetResistance( ResistanceType.Physical, 65, 80 );
			SetResistance( ResistanceType.Fire, 50, 60 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Anatomy, 225.1, 250.0 );
			SetSkill( SkillName.EvalInt, 290.1, 300.0 );
			SetSkill( SkillName.Magery, 295.5, 300.0 );
			SetSkill( SkillName.Meditation, 225.1, 250.0 );
			SetSkill( SkillName.MagicResist, 90.5, 95.0 );
			SetSkill( SkillName.Tactics, 290.1, 300.0 );
			SetSkill( SkillName.Wrestling, 290.1, 300.0 );

			Fame = 24000;
			Karma = -24000;

			VirtualArmor = 90;

			PackItem( new Longsword() );
			PackGold( 600, 800 );
			PackScroll( 2, 8 );
			PackArmor( 2, 5 );
			PackWeapon( 3, 5 );
			PackWeapon( 5, 5 );
			PackSlayer();

		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override int TreasureMapLevel{ get{ return 5; } }
		public override int Meat{ get{ return 1; } }

		public HarpyBoss( Serial serial ) : base( serial )
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