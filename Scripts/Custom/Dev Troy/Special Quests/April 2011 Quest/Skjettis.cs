using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a disgusting corpse" )]
	public class Skjettis : BaseBoss
	{


		[Constructable]
		public Skjettis() : base( AIType.AI_Melee, FightMode.Closest )
		{
			Name = "Skjettis";
			Body = 48;
			Hue = 879;
			BaseSoundID = 397;

			SetStr( 401, 430 );
			SetDex( 133, 152 );
			SetInt( 101, 140 );

			SetHits( 10000 );

			SetDamage( 11, 17 );

			SetDamageType( ResistanceType.Physical, 80 );
			SetDamageType( ResistanceType.Fire, 20 );

			SetResistance( ResistanceType.Physical, 45, 50 );
			SetResistance( ResistanceType.Fire, 50, 60 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.Anatomy, 11.7, 14.4 );
			SetSkill( SkillName.MagicResist, 65.1, 80.0 );
			SetSkill( SkillName.Tactics, 65.1, 88.5 );
			SetSkill( SkillName.Wrestling, 68.1, 81.7 );

			Fame = 5500;
			Karma = -5500;

			VirtualArmor = 46;


			PackReg( 3 );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.MedScrolls, 2 );
		}

		public override void OnDeath( Container c )
	  	{
			if ( Utility.Random( 100 ) <  1 )
				c.DropItem( new SkjettisRare() );

			base.OnDeath( c );
	  	}

		public override bool DoSpawnMobile { get { return true; } }
		public override bool DoEarthquake { get { return true; } }
		public override bool DoHealMobiles { get { return true; } }

		public override int Meat{ get{ return 10; } }
		public override int Hides{ get{ return 20; } }
		public override HideType HideType{ get{ return HideType.Horned; } }


		public Skjettis( Serial serial ) : base( serial )
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