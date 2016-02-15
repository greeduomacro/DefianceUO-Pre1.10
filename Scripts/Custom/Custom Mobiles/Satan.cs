using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "the devil's  corpse" )]
	public class Satan : BaseCreature
	{
		[Constructable]
		public Satan() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Satan";
			Title = "The Dark Lord";
			Body = 401;
			BaseSoundID = 357;

			SetStr( 500 );
			SetDex( 100 );
			SetInt( 1000 );

			SetHits( 30000 );
			SetMana( 5000 );

			SetDamage( 17, 21 );

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

			SetSkill( SkillName.EvalInt, 100.0 );
			SetSkill( SkillName.Magery, 100.0 );
			SetSkill( SkillName.Meditation, 120.0 );
			SetSkill( SkillName.MagicResist, 150.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 120.0 );

			Fame = 12500;
			Karma = -28000;

			VirtualArmor = 64;

			new SkeletalMount().Rider = this;

			Item shroud = new HoodedShroudOfShadows();

			shroud.Movable = false;

			AddItem( shroud );

			Scythe weapon = new Scythe();

			weapon.Skill = SkillName.Wrestling;
			weapon.Hue = 1;
			weapon.Movable = false;

			AddItem( weapon );

			PackGem();
			PackGold( 2200, 2350 );
		}

		public override bool BardImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }

		public override int TreasureMapLevel{ get{ return 1; } }

		public Satan( Serial serial ) : base( serial )
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