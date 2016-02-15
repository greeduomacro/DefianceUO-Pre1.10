using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "barro primordial" )]
	public class ForceNature : BaseCreature
	{
		[Constructable]
		public ForceNature() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Fuerza de la Naturaleza";
			Body = 107;
			BaseSoundID = 268;
			Hue = 0x48C;

			SetStr( 500 );
			SetDex( 500 );
			SetInt( 500 );

			SetHits( 136, 153 );

			SetDamage( 50 );

			SetDamageType( ResistanceType.Physical, 200 );
			SetDamageType( ResistanceType.Fire, 200 );
			SetDamageType( ResistanceType.Cold, 200 );
			SetDamageType( ResistanceType.Poison, 200 );
			SetDamageType( ResistanceType.Energy, 200 );

			SetResistance( ResistanceType.Physical, 30, 40 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.MagicResist, 150.1, 195.0 );
			SetSkill( SkillName.Tactics, 160.1, 250.0 );
			SetSkill( SkillName.Wrestling, 360.1, 500.0 );

			Fame = 15000;
			Karma = -3500;

			VirtualArmor = 32;

			PackGem();
			PackGem();
			PackGem();
			PackGem();
			PackGem();
			PackGem();
			PackGem();
			PackGem();
			PackGem();
			PackGem();

			PackMagicItems( 1, 5 );
		}

		public override bool AutoDispel{ get{ return true; } }

		public ForceNature( Serial serial ) : base( serial )
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