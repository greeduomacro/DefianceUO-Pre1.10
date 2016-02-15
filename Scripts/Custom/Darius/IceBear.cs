using System;
using Server.Items;
using Server.Mobiles;
using Server;

namespace Server.Mobiles
{
	[CorpseName( "an ice bear corpse" )]
	[TypeAlias( "Server.Mobiles.Icebear" )]
	public class IceBear : BaseCreature
	{
		[Constructable]
		public IceBear() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an ice bear";
			Body = 213;
			Hue = 1154;
			Kills = 5;
			BaseSoundID = 0xA3;

			SetStr( 500, 600);
			SetDex( 40, 50 );
			SetInt( 100, 150 );

			SetHits( 7000, 9000 );

			SetDamage( 25, 30 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 25, 35 );
			SetResistance( ResistanceType.Cold, 60, 80 );
			SetResistance( ResistanceType.Poison, 15, 25 );
			SetResistance( ResistanceType.Energy, 10, 15 );

			SetSkill( SkillName.MagicResist, 200.0, 220.0 );
			SetSkill( SkillName.EvalInt, 100.0, 110.0 );
			SetSkill( SkillName.Magery, 110, 120.0 );
			SetSkill( SkillName.Tactics, 110.0, 120.0 );
			SetSkill( SkillName.Wrestling, 100.0, 110.0 );

			Fame = 9000;
			Karma = -9000;

			VirtualArmor = 60;

			//if ( Utility.Random( 100 ) < 1 ) PackItem( new IceBearMask() );

			}

		public override bool BardImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override int Meat{ get{ return 2; } }
		public override int Hides{ get{ return 16; } }

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Controlled || bc.BardTarget == this )
					damage = 0; // Immune to pets and provoked creatures
			}
		}

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			scalar = 0.0; // Immune to magic
		}

		public override void CheckReflect( Mobile caster, ref bool reflect )
		{
			reflect = true; // Every spell is reflected back to the caster
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );
			if ( Utility.Random( 100 ) < 30 ) defender.Paralyze( TimeSpan.FromSeconds( 3.0 ) ); }

		public IceBear( Serial serial ) : base( serial )
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