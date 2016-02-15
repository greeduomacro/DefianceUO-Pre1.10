using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a rigid corpse" )]
	public class Mongah : BaseBoss
	{


		[Constructable]
		public Mongah() : base( AIType.AI_Melee, FightMode.Closest )
		{
			Name = "Mon'gah";
			Body = 107;
			BaseSoundID = 268;
			Hue = 55;

			SetStr( 226, 255 );
			SetDex( 126, 145 );
			SetInt( 71, 92 );

			SetHits( 9000 );

			SetDamage( 28 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 30, 40 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.MagicResist, 50.1, 95.0 );
			SetSkill( SkillName.Tactics, 60.1, 100.0 );
			SetSkill( SkillName.Wrestling, 60.1, 100.0 );

			Fame = 3500;
			Karma = -3500;

			VirtualArmor = 32;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Gems, 2 );
		}

		public override void OnDeath( Container c )
	  	{
			if ( Utility.Random( 100 ) <  1 )
				c.DropItem( new MongahRare() );

			base.OnDeath( c );
	  	}

		public override bool AutoDispel{ get{ return true; } }
		public override bool DoProvoPets { get { return true; } }
		public override bool DoImmuneToPets { get { return true; } }

		public Mongah( Serial serial ) : base( serial )
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