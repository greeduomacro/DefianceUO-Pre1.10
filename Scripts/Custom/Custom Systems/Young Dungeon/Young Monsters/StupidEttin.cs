using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "an ettins corpse" )]
	public class StupidEttin : BaseCreature
	{
		[Constructable]
		public StupidEttin() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a stupid ettin";
			Body = 18;
			BaseSoundID = 367;
			Hue = Utility.RandomList(0x89C,0x8A2,0x8A8,0x8AE);

			SetStr( 100, 165 );
			SetDex( 56, 75 );
			SetInt( 31, 55 );

			SetHits( 80, 120 );

			SetDamage( 7, 12 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 35, 40 );
			SetResistance( ResistanceType.Fire, 15, 25 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 15, 25 );
			SetResistance( ResistanceType.Energy, 15, 25 );

			SetSkill( SkillName.MagicResist, 40.1, 55.0 );
			SetSkill( SkillName.Tactics, 50.1, 70.0 );
			SetSkill( SkillName.Wrestling, 50.1, 60.0 );

			Fame = 3000;
			Karma = -3000;

			VirtualArmor = 38;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Potions );
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 13 ) < 1 )
			c.AddItem( new RedBall() );

			base.OnDeath( c );
		}


		public override bool CanRummageCorpses{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 1; } }
		public override int Meat{ get{ return 4; } }

		public StupidEttin( Serial serial ) : base( serial )
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