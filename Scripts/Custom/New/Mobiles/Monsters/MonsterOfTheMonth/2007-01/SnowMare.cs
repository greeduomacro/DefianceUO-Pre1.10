using System;
using Server;
using Server.Items;
using Xanthos.Evo;

namespace Server.Mobiles
{
	[CorpseName( "a snow mare corpse" )]
	public class SnowMare : BaseCreature
	{
		[Constructable]
		public SnowMare () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2 )
		{
			Body = Utility.RandomList( 116, 178, 179 );
			Hue = Utility.RandomList( 1153, 1150 );
			Name = "a snow mare";
			BaseSoundID = 0xA8;

			SetStr( 550 );
			SetDex( 100 );
			SetInt( 1000 );

			SetHits(2000, 3000);

			SetDamage( 15, 20 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Cold, 50 );

			SetResistance( ResistanceType.Physical, 55, 70 );
			SetResistance( ResistanceType.Fire, 15, 25 );
			SetResistance( ResistanceType.Cold, 80, 90 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.EvalInt, 120.0 );
			SetSkill( SkillName.Magery, 120.0 );
			SetSkill( SkillName.MagicResist, 180.0 );
			SetSkill( SkillName.Tactics, 120.0 );
			SetSkill( SkillName.Wrestling, 120.0 );
			SetSkill(SkillName.Meditation, 150.0);

			Fame = 18000;
			Karma = -18000;

			PackGold( 200, 300 );

			VirtualArmor = 40;

			if ( Utility.Random( 500 ) < 1 ) PackItem( new NightmareStatue() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Gems, Utility.Random( 1, 5 ) );
		}

		public override bool CanDestroyObstacles { get { return true; } }
		public override int TreasureMapLevel{ get{ return 5; } }
		public override int Meat{ get{ return 10; } }
		public override int Hides{ get{ return 10; } }
		public override HideType HideType{ get{ return HideType.Barbed; } }

//Taken from DragChamp.cs
		public override bool HasBreath{ get{ return true; } }
		public override int BreathComputeDamage()
		{
			return (int)50;
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );
			{
				DoHarmful( defender );
				Effects.SendMovingEffect( this, defender, 0x36E4, 7, 0, false, true, 0x480, 0 );
				defender.SendMessage( "You are hurt by the coldness of the attack!" );
				int toDrain = Utility.RandomMinMax( 20, 25 );
				defender.Damage( toDrain );
			}
		}

//Taken from Savage.cs and changed to "from"
		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
			{
				if ( from is Dragon || from is WhiteWyrm || from is Kirin || from is Drake || from is Nightmare || from is Unicorn || from is FireSteed )
				damage /= 2;

				else if ( from is EvolutionDragon || from is EvoHiryu)
				damage /= 10;
			}
		}

//Taken from Savage.cs
		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
			{
				if ( to is Dragon || to is WhiteWyrm || to is Kirin || to is Drake || to is Nightmare || to is Unicorn || to is FireSteed )
				damage *= 2;

				else if ( to is EvolutionDragon || to is EvoHiryu )
				damage *= 10;
			}
		}

//Taken from QuestBoss.cs but modified to be anti pet only
		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			if ( caster is BaseCreature )
			{
				if ( caster is Dragon || caster is WhiteWyrm || caster is Kirin || caster is Drake || caster is Nightmare || caster is Unicorn || caster is FireSteed )
				scalar = 0.50;

				else if ( caster is EvolutionDragon || caster is EvoHiryu )
				scalar = 0.0;
			}
		}

		public SnowMare( Serial serial ) : base( serial )
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