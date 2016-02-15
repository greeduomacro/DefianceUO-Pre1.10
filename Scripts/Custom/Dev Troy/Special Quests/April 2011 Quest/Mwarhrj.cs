using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an disgusting corpse" )]
	public class Mwarhrj : BaseBoss
	{


		[Constructable]
		public Mwarhrj () : base( AIType.AI_Mage, FightMode.Closest )
		{
			Name = "Mwarhrj";
			Body = 22;
			BaseSoundID = 377;
			Hue = 565;

			SetStr( 296, 325 );
			SetDex( 86, 101 );
			SetInt( 310, 364 );

			SetHits( 10000 );

			SetDamage( 20, 30 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Energy, 50 );

			SetResistance( ResistanceType.Physical, 45, 55 );
			SetResistance( ResistanceType.Fire, 60, 70 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Anatomy, 62.0, 100.0 );
			SetSkill( SkillName.EvalInt, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 115.1, 130.0 );
			SetSkill( SkillName.Tactics, 80.1, 100.0 );
			SetSkill( SkillName.Wrestling, 80.1, 100.0 );

			Fame = 12500;
			Karma = -12500;

			VirtualArmor = 50;
		}


		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
		}

		public override void OnDeath( Container c )
	  	{
			if ( Utility.Random( 100 ) <  1 )
				c.DropItem( new MwarhrjRare() );

			base.OnDeath( c );
	  	}

		public override int DoMoreDamageToPets { get { return 5; } }
		public override int DoLessDamageFromPets { get { return 5; } }
		public override int DoLessMagicDamageFromPets { get { return 5; } }



		public Mwarhrj( Serial serial ) : base( serial )
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