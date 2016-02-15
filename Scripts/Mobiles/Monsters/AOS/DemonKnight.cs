using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a demon knight corpse" )]
	public class DemonKnight : BaseCreature
	{
		[Constructable]
		public DemonKnight() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "demon knight" );
			Title = "the dark father";
			Body = 318;
			BaseSoundID = 357;
			//Kills = 5
			SetStr( 2000 );
			SetDex( 150 );
			SetInt( 1500 );

			SetHits( 20000 );
			SetMana( 50000 );

			SetDamage( 18, 30 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Fire, 20 );
			SetDamageType( ResistanceType.Cold, 20 );
			SetDamageType( ResistanceType.Poison, 20 );
			SetDamageType( ResistanceType.Energy, 20 );

			SetResistance( ResistanceType.Physical, 30 );
			SetResistance( ResistanceType.Fire, 30 );
			SetResistance( ResistanceType.Cold, 30 );
			SetResistance( ResistanceType.Poison, 30 );
			SetResistance( ResistanceType.Energy, 30 );

			SetSkill( SkillName.EvalInt, 125.0 );
			SetSkill( SkillName.Magery, 150.0 );
			SetSkill( SkillName.Meditation, 125.0 );
			SetSkill( SkillName.MagicResist, 150.0 );
			SetSkill( SkillName.Tactics, 125.0 );
			SetSkill( SkillName.Wrestling, 125.0 );

			Fame = 18500;
			Karma = -18500;

			VirtualArmor = 60;

			PackGem();
			PackGem();
			PackGem();
			PackGem();
			PackGold( 1000, 3500 );
		}

		public override bool BardImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool AutoDispel{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 1; } }
		public override bool AlwaysMurderer{ get{ return true; } }

		public DemonKnight( Serial serial ) : base( serial )
		{
		}
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 2 );
			AddLoot( LootPack.FilthyRich, 2 );
		}

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
				damage *= 3;
		}

		public override void OnDeath( Container c )
		{
			base.OnDeath( c );

			if ( 0.75 > Utility.RandomDouble() )
			{
				int r = Utility.Random( 100 );
				Item drop = null;

				if ( r < 1 ) drop = new LayerSashDeed();			
				else if	( r < 4 ) drop = new SpecialQuestSandals();
				else if	( r < 6 ) drop = new RareCreamCarpet( PieceType.Centre );
				else if	( r < 8 ) drop = new RareBlueCarpet( PieceType.Centre );
				else if	( r < 10 ) drop = new RareBloodCarpet( PieceType.Centre );
				else if	( r < 12 ) drop = new BasicBlueCarpet( PieceType.Centre );
				else if	( r < 14 ) drop = new BasicPinkCarpet( PieceType.Centre );
				else if	( r < 29 ) drop = new BloodPentagramPart( Utility.Random( 5 ) );
				else if	( r < 30 ) drop = new ClothingBlessDeed();
				else if	( r < 35 ) drop = new MysteriousCloth();
				else if	( r < 40 ) drop = new SpecialHairDye();
				else if	( r < 45 ) drop = new SpecialBeardDye();
				else if	( r < 50 ) drop = new NameChangeDeed();
				else if	( r < 65 ) drop = new SkillTunic();
				else if	( r < 80 ) drop = new TamersCrook();
				else if	( r < 85 ) drop = new HeroShield();
				else if	( r < 88 ) drop = new EvilShield();
				else if	( r < 91 ) drop = new MondainHat();
				else if	( r < 94 ) drop = new PlatinGloves();
				else if	( r < 96 ) drop = new AncientSamuraiHelm();
				else if	( r < 98 ) drop = Utility.RandomBool() ? (Item)(new MirrorEast()) : (Item)(new MirrorNorth());
				else
					drop = Utility.RandomBool() ? (Item)(new BoneBenchEastPart()) : (Item)(new BoneBenchWestPart());

				c.DropItem( drop );
			}
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}