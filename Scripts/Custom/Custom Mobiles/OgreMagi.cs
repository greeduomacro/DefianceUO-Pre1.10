using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "an Ogre Magi Corpse" )]
	public class OgreMagi : BaseCreature
	{
		[Constructable]
		public OgreMagi () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an Ogre Magi";
			Body = 135;
			Hue = 2116;
			BaseSoundID = 427;

		    	SetStr( 556, 654 );
			SetDex( 66, 75 );
			SetInt( 845, 1050 );

			SetHits( 476, 552 );

			SetDamage( 15, 20 );

			SetDamageType( ResistanceType.Physical, 30 );
			SetDamageType( ResistanceType.Energy, 70 );

			SetResistance( ResistanceType.Physical, 70 );
			SetResistance( ResistanceType.Fire, 48, 54 );
			SetResistance( ResistanceType.Cold, 48, 54 );
			SetResistance( ResistanceType.Poison, 48, 54 );
			SetResistance( ResistanceType.Energy, 70 );

			SetSkill( SkillName.Anatomy, 62.0, 100.0 );
			SetSkill( SkillName.EvalInt, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 115.1, 130.0 );
			SetSkill( SkillName.Tactics, 80.1, 100.0 );
			SetSkill( SkillName.Wrestling, 80.1, 100.0 );


			Fame = 12500;
			Karma = -12500;

			VirtualArmor = 50;

			PackGold( 200, 450  );
		}

		public OgreMagi( Serial serial ) : base( serial )
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