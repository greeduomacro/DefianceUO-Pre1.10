using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a headless corpse" )]
	public class HeadlessCreature : BaseCreature
	{
		[Constructable]
		public HeadlessCreature() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a headless creature";
			Body = 31;
			Hue = 1544;
			BaseSoundID = 0x39D;

			SetStr( 26, 50 );
			SetDex( 36, 55 );
			SetInt( 16, 30 );

			SetHits( 25, 40 );

			SetDamage( 8, 15 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 15, 20 );

			SetSkill( SkillName.MagicResist, 15.1, 20.0 );
			SetSkill( SkillName.Tactics, 25.1, 40.0 );
			SetSkill( SkillName.Wrestling, 25.1, 40.0 );

			Fame = 450;
			Karma = -450;

			VirtualArmor = 18;
			Tamable = false;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager, 3 );

		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 10 ) < 1 )
			c.AddItem( new BlueBall() );

			base.OnDeath( c );
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override int Meat{ get{ return 1; } }
		public override int TreasureMapLevel{ get{ return 2; } }


		public HeadlessCreature( Serial serial ) : base( serial )
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