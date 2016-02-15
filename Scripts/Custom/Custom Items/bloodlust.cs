using System;
using Server;
using Server.Items;
using System.Collections;


namespace Server.Mobiles
{

	[CorpseName( "a godly corpse" )]
	public class BloodLust : BaseCreature
	{
		[Constructable]
		public BloodLust () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "male" );
			Body = 746;
			Title = "the KeyHolder";
			BaseSoundID = 0x482;;
			Hue = 1609;
			Kills = 5;
			SetStr( 3500, 3500 );
			SetDex( 3500, 3500 );
			SetInt( 9999, 9999 );

			SetHits( 3500, 3500 );

			SetDamage( 70, 70 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Cold, 60 );
			SetDamageType( ResistanceType.Energy, 40 );

			SetResistance( ResistanceType.Physical, 40, 50 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 50, 60 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.EvalInt, 999.0, 999.0 );
			SetSkill( SkillName.Magery, 999.0, 999.0 );
			SetSkill( SkillName.MagicResist, 999.0, 999.0 );
			SetSkill( SkillName.Tactics, 999.0, 999.0 );
			SetSkill( SkillName.Wrestling, 999.0, 999.0 );

			Fame = 1000000;
			Karma = -1000000;

			VirtualArmor = 1000;

			PackGem();
			PackGem();
			PackGold( 50000, 60000 );
			PackScroll( 3, 8 );
			PackScroll( 3, 8 );
			PackWeapon( 4, 4 );
			PackMagicItems( 4, 5, 0.80, 0.75 );
			PackMagicItems( 4, 5, 0.60, 0.45 );
			PackSlayer( 1 );

			//PackNecroScroll( 2 ); // Corpse Skin

			AddItem( new Server.Items.Robe( 1609 ) );




		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 3; } }
		public override int Meat{ get{ return 1; } }

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.SavagesAndOrcs; }
		}

		public override void AggressiveAction( Mobile aggressor, bool criminal )
		{
			base.AggressiveAction( aggressor, criminal );

			Item item = aggressor.FindItemOnLayer( Layer.Helm );

			if ( item is OrcishKinMask )
			{
				AOS.Damage( aggressor, 50, 0, 100, 0, 0, 0 );
				item.Delete();
				aggressor.FixedParticles( 0x36BD, 20, 10, 5044, EffectLayer.Head );
				aggressor.PlaySound( 0x365 );
			}
		}

		public BloodLust( Serial serial ) : base( serial )
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