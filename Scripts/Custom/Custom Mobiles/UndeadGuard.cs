using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Misc;

namespace Server.Mobiles
{
	[CorpseName( "a fading corpse" )]
	public class UndeadGuard : BaseCreature
	{
		[Constructable]
		public UndeadGuard() : base( AIType.AI_Mage, FightMode.Closest, 10, 1,
0.09, 0.15 )
		{
			Name = "Undead Guard";
			Body = 125;
			Hue = 1643;
			SetStr( 110, 180 );
			SetStr( 151, 180 );
			SetDex( 116, 135 );
			SetInt( 276, 305 );

			SetHits( 103, 120 );

			SetDamage( 15, 23 );

			SetDamageType( ResistanceType.Physical, 10 );
			SetDamageType( ResistanceType.Cold, 40 );
			SetDamageType( ResistanceType.Energy, 50 );

			SetResistance( ResistanceType.Physical, 40, 60 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 55, 65 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.EvalInt, 90.0, 100.1 );
			SetSkill( SkillName.Magery, 65.1, 83.0 );
			SetSkill( SkillName.Meditation, 95.0, 105.0 );
			SetSkill( SkillName.MagicResist, 80.1, 90.0 );
			SetSkill( SkillName.Tactics, 70.1, 90.0 );

			VirtualArmor = 50;
		}


		public override bool IsEnemy( Mobile m )
		{
			if ( m.Player && m.FindItemOnLayer( Layer.Ring ) is UndeadRing )
				return false;

			return base.IsEnemy( m );
		}

		public UndeadGuard( Serial serial ) : base( serial )
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