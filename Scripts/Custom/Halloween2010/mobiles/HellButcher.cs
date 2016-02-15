using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
	public class HellButcher : BaseCreature
	{
		[Constructable]
		public HellButcher(): base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "Hell's Butcher";
			Body = 256;
			Hue = 1175;
			BaseSoundID = 0x165;

			SetStr(300, 600);
			SetDex(200, 250);

			SetHits( 7500 );

			SetDamage (20, 30);

			SetDamageType(ResistanceType.Physical, 120);

			SetResistance(ResistanceType.Physical, 20, 25);
			SetResistance(ResistanceType.Fire, 10, 20);
			SetResistance(ResistanceType.Cold, 5, 10);
			SetResistance(ResistanceType.Poison, 5, 10);
			SetResistance(ResistanceType.Energy, 10, 15);

			SetSkill(SkillName.MagicResist, 150.0);
			SetSkill(SkillName.Tactics, 100.0);
			SetSkill(SkillName.Wrestling, 100.0);

			Fame = 2500;
			Karma = -2500;

			VirtualArmor = 100;

		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 50 ) < 1 )
			c.DropItem( new HalloweenCandle() );

			if ( Utility.Random( 10 ) < 1 )
			c.DropItem( new HalloweenStatue() );

			base.OnDeath( c );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 1 );
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool AutoDispel{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override int TreasureMapLevel{ get{ return 6; } }
		public override bool CanRummageCorpses{ get{ return true; } }

		public override bool OnBeforeDeath()
		{
			foreach ( Mobile m in GetMobilesInRange( 20 ) )
			{
				m.Freeze( TimeSpan.FromSeconds( 25.0 ) );
				m.SendMessage( "The brutal mass hits the ground with such force, stunning you!" );
			}

			return base.OnBeforeDeath();
		}

		//Melee damage from controlled mobiles is divided by 5
		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Controlled )
					damage /= 5;
			}
		}

		//Melee damage to controlled mobiles is multiplied by 2
		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)to;

				if ( bc.Controlled )
					damage *= 2;
			}
		}



		public HellButcher( Serial serial ) : base( serial )
		{
		}


		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadEncodedInt();
		}
	}
}