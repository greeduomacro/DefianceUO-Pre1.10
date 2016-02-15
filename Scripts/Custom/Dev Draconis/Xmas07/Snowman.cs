using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a snowman corpse" )]
	public class EvilSnowman : BaseCreature
	{
		[Constructable]
		public EvilSnowman() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a snowman";
			Body = 14;
			Hue = 1150;
			NameHue = 1150;
			BaseSoundID = 268;

			SetStr( 200 );
			SetDex( 100 );
			SetInt( 100 );

			SetHits( 500 );

			SetDamage( 30, 40 );

			SetSkill( SkillName.Wrestling, 400.0 );
			SetSkill( SkillName.MagicResist, 300.0 );

			VirtualArmor = 1000;

			Fame = 10000;
			Karma = -10000;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 2 );
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 100 ) < 1 )
				c.DropItem( new SnowmanStatue() );

			base.OnDeath( c );
		}

		public override int TreasureMapLevel{ get{ return 5; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }

		public override void OnDamagedBySpell( Mobile caster )
		{
			this.Hits += 75;
			caster.SendMessage( "The snowman absorbs the spell" );
			caster.PlaySound( 481 );
			base.OnDamagedBySpell( caster );
		}

		public override void Damage( int amount, Mobile from )
		{
			if ( from is PlayerMobile )
			{
				amount *= 3;
			}
			else
				amount = 0;

			base.Damage( amount, from );
		}

        public EvilSnowman(Serial serial)
            : base(serial)
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