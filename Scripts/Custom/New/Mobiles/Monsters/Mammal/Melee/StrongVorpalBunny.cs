using System;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a vorpal bunny corpse" )]
	public class StrongVorpalBunny : BaseCreature
	{
		[Constructable]
		public StrongVorpalBunny() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Body = 205;
			Hue = 0x480;

			Name = "a vorpal bunny";

			SetStr( 15 );
			SetDex( 2000 );
			SetInt( 1000 );

			SetHits( 2000 );
			SetStam( 500 );
			SetMana( 0 );

			SetDamage( 1 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetSkill( SkillName.MagicResist, 200.0 );
			SetSkill( SkillName.Tactics, 5.0 );
			SetSkill( SkillName.Wrestling, 5.0 );

			Fame = 1000;
			Karma = 0;

			VirtualArmor = 4;

			int carrots = Utility.RandomMinMax( 5, 10 );
			PackItem( new Carrot( carrots ) );

			if ( Utility.Random( 10 ) == 0 )
				PackItem( new BrightlyColoredEggs() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.Rich );
		}

		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 1; } }
		public override bool BardImmune{ get{ return !Core.AOS; } }

		public StrongVorpalBunny( Serial serial ) : base( serial )
		{
		}

		public override int GetAttackSound()
		{
			return 0xC9;
		}

		public override int GetHurtSound()
		{
			return 0xCA;
		}

		public override int GetDeathSound()
		{
			return 0xCB;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize(writer);

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}