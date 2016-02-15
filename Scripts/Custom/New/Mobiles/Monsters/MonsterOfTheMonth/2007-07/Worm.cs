using System;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a worm corpse" )]
	public class Worm : BaseBoss
	{
		[Constructable]
		public Worm() : base( AIType.AI_Melee )
		{
			Name = "a worm";
			Body = 52;
			BaseSoundID = 0xDB;

			SetStr( 200, 250 );
			SetDex( 200, 250 );
			SetInt( 200, 250 );

			SetHits( 1500 );
			SetMana( 0 );

			SetDamage( 20, 25 );

			SetSkill( SkillName.Poisoning, 110, 120.0 );
			SetSkill( SkillName.MagicResist, 150.0 );
			SetSkill( SkillName.Tactics, 110, 120.0 );
			SetSkill( SkillName.Wrestling, 100, 110.0 );
			SetSkill( SkillName.Anatomy, 110.0, 120 );

			Fame = 10000;
			Karma = -10000;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 2 );
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 80 ) < 1 )
			c.DropItem( new WormStatue() );

			base.OnDeath( c );
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Lethal; } }
		public override double HitPoisonChance{ get{ return 0.80; } }
		public override int DoMoreDamageToPets{ get{ return 2; } }
		public override int DoLessDamageFromPets{ get{ return 2; } }
		public override bool BardImmune{ get{ return true; } }
		public override bool CanDestroyObstacles { get { return true; } }

		public override void OnSingleClick( Mobile from )
		{
			from.SendMessage( "This worm appears to have sharp teeth!" );

			base.OnSingleClick(from);
		}

		public Worm(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}