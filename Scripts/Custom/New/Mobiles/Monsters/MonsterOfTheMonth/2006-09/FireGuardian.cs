using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a fire guardian's corpse" )]
	public class FireGuardian : BaseCreature
	{
		[Constructable]
		public FireGuardian() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a fire guardian";
			Body = 15;
			BaseSoundID = 838;
			Kills = 100;

			SetStr( 1198, 1225 );
            		SetDex( 155, 197 );
            		SetInt( 801, 899 );

			SetHits( 2000, 3000 );
			SetMana( 3000);

			SetDamage( 20, 30 );

			SetSkill( SkillName.EvalInt, 100.0, 110.0 );
            		SetSkill( SkillName.Magery, 100.0, 110.0 );
            		SetSkill( SkillName.Meditation, 81.7, 94.4 );
            		SetSkill( SkillName.MagicResist, 140.1, 183.0 );
            		SetSkill( SkillName.Tactics, 130.6, 160.0 );
            		SetSkill( SkillName.Wrestling, 150.6, 160.0 );

			Fame = 22500;
            		Karma = -22500;

			VirtualArmor = 100;

                    PackGem();
                    PackGem();
                    PackGem();

            		switch ( Utility.Random( 7 ) )
            		{
                		case 0: PackItem( new GreaterCurePotion() ); break;
                		case 1: PackItem( new GreaterPoisonPotion() ); break;
                		case 2: PackItem( new GreaterHealPotion() ); break;
                		case 3: PackItem( new GreaterStrengthPotion() ); break;
                		case 4: PackItem( new GreaterAgilityPotion() ); break;
            		}

            		PackGold( 332, 711 );
            		PackSlayer();
            		PackSlayer();

            		switch ( Utility.Random( 2 ) )
            		{
            		    case 0: PackWeapon( 0, 5 ); break;
            		    case 1: PackArmor( 0, 5 ); break;
            		}

            		switch ( Utility.Random( 2 ) )
            		{
            		    case 0: PackWeapon( 0, 5 ); break;
            		    case 1: PackArmor( 0, 5 ); break;
            		}

            		switch ( Utility.Random( 2 ) )
            		{
            		    case 0: PackWeapon( 1, 5 ); break;
            		    case 1: PackArmor( 1, 5 ); break;
            		}

            		switch ( Utility.Random( 3 ) )
            		{
            		    case 0: PackWeapon( 1, 5 ); break;
            		    case 1: PackArmor( 1, 5 ); break;
            		}

            		switch ( Utility.Random( 300 ))
            		{
            		       case 0:
                               Item brazier = new Brazier();
                               brazier.Movable = true;
                               PackItem( brazier );
                               break;
            		}

            		if ( 0.01 > Utility.RandomDouble() )
                		PackItem( new IDWand() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.MedScrolls, 2 );
		}
		public override bool AutoDispel{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override int TreasureMapLevel{ get{ return 5; } }
		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
       		{
            		if ( to is BaseCreature )
                	damage *= 2;
        	}

		public FireGuardian( Serial serial ) : base( serial )
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