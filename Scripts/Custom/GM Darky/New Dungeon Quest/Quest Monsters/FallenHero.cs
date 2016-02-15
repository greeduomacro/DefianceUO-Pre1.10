using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
	public class FallenHero : BaseCreature
	{
		[Constructable]
		public FallenHero() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.15, 0.2 )
		{
			Name = "a fallen hero";
			Body = 0x306;
			Hue = 22222;
			Kills = 10;
			ShortTermMurders = 10;

			MagicDamageAbsorb = 250;

			SetStr( 386, 405 );
			SetDex( 101, 105 );

			SetHits( 1670, 1870 );

			SetDamage( 10, 14 );

			SetSkill( SkillName.Anatomy, 50.0 );
			SetSkill( SkillName.MagicResist, 103.5, 112.5 );
			SetSkill( SkillName.Swords, 125.0 );
			SetSkill( SkillName.Tactics, 125.0 );

			Fame = 18000;
			Karma = -18000;

			VirtualArmor = 60;

			VikingSword weapon = new VikingSword();
			weapon.Movable = false;
			AddItem( weapon );

			PackGold( 900, 1500 );
			PackItem( new Bandage( Utility.RandomMinMax( 5, 50 ) ) );
			PackArmor( 1, 5 );
			PackWeapon( 1, 5 );
			PackArmor( 1, 5 );
			PackWeapon( 1, 5 );
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			defender.Stam -= Utility.Random( 1, 10 );
		}

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Controlled || bc.BardTarget == this )
					damage = 0; // Immune to pets and provoked creatures
			}
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		public override bool Uncalmable{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 5; } }

		public FallenHero( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}