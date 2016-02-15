using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Factions;

namespace Server.Mobiles
{
	[CorpseName( "a noxious corpse" )]
	public class NoxiousWarlord : BaseCreature
	{
		public override bool IsScaryToPets{ get{ return true; } }

		[Constructable]
		public NoxiousWarlord () : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.15, 0.2 )
		{
			Name = "a noxious warlord";
			Body = 0x98;
			Hue = 1267;
			BaseSoundID = 589;

			SetStr( 767, 945 );
			SetDex( 66, 75 );
			SetInt( 46, 70 );

			SetHits( 1476, 1552 );

			SetDamage( 20, 25 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 45, 55 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.MagicResist, 125.1, 140.0 );
			SetSkill( SkillName.Tactics, 98.1, 99.6 );
			SetSkill( SkillName.Wrestling, 97.1, 99.9 );
			SetSkill( SkillName.Poisoning, 98.1, 98.9 );

			Fame = 18000;
			Karma = -18000;

			VirtualArmor = 50;

			switch ( Utility.Random( 4 ) )
			{
				case 0: PackItem( new GreaterCurePotion() ); break;
				case 1: PackItem( new GreaterPoisonPotion() ); break;
			}

			switch ( Utility.Random( 2 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 3 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 4 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}

			PackGold( 900, 1400 );

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackItem( new Bandage( Utility.RandomMinMax( 50, 100 ) ) ); break;
			}

			if ( Utility.Random( 50 ) == 0 )
				PackItem( new RareBlueCarpet( PieceType.EastEdge ));


		}

		public override int Meat{ get{ return 10; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Greater; } }
		public override double HitPoisonChance{ get{ return 0.50; } }
		public override bool Uncalmable{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 5; } }

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
				damage *= 5;
		}

		public NoxiousWarlord( Serial serial ) : base( serial )
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

		public override void OnDeath( Container c )
		{
			Item item = null;
			switch( Utility.Random(500) )
				{
			case 0: c.DropItem( item = new SilenceHelm() ); break;
			case 1: c.DropItem( item = new SilenceLeggings() ); break;
			case 2: c.DropItem( item = new SilenceNecklace() ); break;
			case 3: c.DropItem( item = new SilenceRobe() ); break;
			case 4: c.DropItem( item = new SilenceShirt() ); break;
			case 5: c.DropItem( item = new SilenceShoes() ); break;
			        }
			base.OnDeath( c );
		}
	}
}