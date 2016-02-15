using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class LichMotm : LichLord
	{
		[Constructable]
		public LichMotm() : base()
		{
			Name = "a cursed lich";
			Hue = 570;

			SetStr( 450, 550 );
			SetDex( 160, 185 );
			SetInt( 566, 655 );

			SetHits( 400, 525 );

			SetDamage( 15, 20 );

			SetSkill( SkillName.EvalInt, 110.0 );
			SetSkill( SkillName.Magery, 110.0 );
			SetSkill( SkillName.MagicResist, 175.0, 200.0 );
			SetSkill( SkillName.Tactics, 70.0, 85.0 );
			SetSkill( SkillName.Wrestling, 80.0, 95.0 );

			Fame = 18000;
			Karma = -18000;

			VirtualArmor = 50;

			PackItem( new GnarledStaff() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.UltraRich );
			AddLoot( LootPack.MedScrolls, 5 );
		}

		public override bool OnBeforeDeath()
		{
			if ( Utility.Random( 75 ) == 0 )
				PackItem( new CursedPotion() );
			return base.OnBeforeDeath();
		}

		//Melee damage to controlled mobiles is multiplied by 6
		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)to;

				if ( bc.Controlled )
					damage *= 6;
			}
		}

		public override Poison HitPoison{ get{ return Poison.Regular; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override int TreasureMapLevel{ get{ return 5; } }

		public LichMotm( Serial serial ) : base( serial )
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