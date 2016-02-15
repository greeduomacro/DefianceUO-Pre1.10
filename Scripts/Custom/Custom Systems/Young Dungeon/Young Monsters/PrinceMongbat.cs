using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Mongbat corpse" )]
	public class MongbatPrince : BaseCreature
	{
		[Constructable]
		public MongbatPrince() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a mongbat prince";
			Body = 39;
			BaseSoundID = 422;
			Hue = 2213;

			SetStr( 150, 215 );
			SetDex( 66, 75 );
			SetInt( 101, 250 );

			SetHits( 130, 150 );
			SetStam( 0 );

			SetDamage( 15, 25 );

			SetDamageType( ResistanceType.Physical, 80 );
			SetDamageType( ResistanceType.Poison, 20 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 15, 25 );
			SetResistance( ResistanceType.Cold, 10, 20 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.EvalInt, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 100.1, 125.0 );
			SetSkill( SkillName.Tactics, 45.1, 60.0 );
			SetSkill( SkillName.Wrestling, 50.1, 60.0 );

			Fame = 3500;
			Karma = -3500;

			VirtualArmor = 40;

			PackReg( 15 );
			Tamable = false;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average, 2 );
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 18 ) < 1 )
			c.AddItem( new PurpleBall() );

			base.OnDeath( c );
		}

		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		public override int TreasureMapLevel{ get{ return 2; } }

		public MongbatPrince( Serial serial ) : base( serial )
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