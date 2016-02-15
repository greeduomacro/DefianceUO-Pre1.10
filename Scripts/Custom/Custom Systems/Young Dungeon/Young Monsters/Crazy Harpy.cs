using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a harpy corpse" )]
	public class CrazyHarpy : BaseCreature
	{
		[Constructable]
		public CrazyHarpy() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a crazy harpy";
			Body = 30;
			BaseSoundID = 402;
			Hue = 1234;

			SetStr( 60, 110 );
			SetDex( 86, 110 );
			SetInt( 51, 75 );

			SetHits( 60, 85 );

			SetDamage( 5, 7 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 25, 30 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 10, 30 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.MagicResist, 50.1, 65.0 );
			SetSkill( SkillName.Tactics, 70.1, 100.0 );
			SetSkill( SkillName.Wrestling, 60.1, 90.0 );

			Fame = 2500;
			Karma = -2500;

			VirtualArmor = 28;
		}

		public override int TreasureMapLevel{ get{ return 1; } }


		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager, 2 );
			AddLoot( LootPack.FilthyRich);
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 13 ) < 1 )
			c.AddItem( new RedBall() );

			base.OnDeath( c );
		}


		public override int GetAttackSound()
		{
			return 916;
		}

		public override int GetAngerSound()
		{
			return 916;
		}

		public override int GetDeathSound()
		{
			return 917;
		}

		public override int GetHurtSound()
		{
			return 919;
		}

		public override int GetIdleSound()
		{
			return 918;
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override int Meat{ get{ return 4; } }
		public override MeatType MeatType{ get{ return MeatType.Bird; } }
		public override int Feathers{ get{ return 50; } }

		public CrazyHarpy( Serial serial ) : base( serial )
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