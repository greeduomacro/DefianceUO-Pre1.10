using System;
using Server;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Spells.Fourth;
using Server.Spells;

namespace Server.Mobiles
{
	[CorpseName( "a mana wisp corpse" )]
	public class ManaWisp : BaseCreature
	{
		public override InhumanSpeech SpeechType{ get{ return InhumanSpeech.Wisp; } }

		[Constructable]
		public ManaWisp() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.08, 0.08 )
		{
			Name = "a mana wisp";
			Body = 58;
			Hue = 1266;
			BaseSoundID = 466;
			Kills = 10;
			ShortTermMurders = 10;

			SetStr( 16, 25 );
			SetDex( 16, 25 );
			SetInt( 16, 25 );

			SetHits( 8, 15 );

			SetDamage( 1, 3 );

			SetSkill( SkillName.MagicResist, 100.0 );
			SetSkill( SkillName.Tactics, 150.0 );
			SetSkill( SkillName.Wrestling, 150.0 );

			Fame = 4000;
			Karma = -4000;

			VirtualArmor = 0;

			switch( Utility.Random(400) )
			{
				case 0: PackItem( new DarkIronWire() ); break;
			}

			AddItem( new LightSource() );

			PackScroll( 1, 8 );
			PackReg( Utility.RandomMinMax( 2, 5 ) );

		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 250 ) <  1 )
				c.DropItem( new DriedFlowers() );

            		base.OnDeath( c );
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool AutoDispel{ get{ return true; } }

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			defender.Mana -= Utility.Random( 15, 25 );
		}

		public ManaWisp( Serial serial ) : base( serial )
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