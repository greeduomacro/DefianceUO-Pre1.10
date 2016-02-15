using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a Gauntlet creature's corpse" )]
	public class GauntletMage : BaseCreature
	{
		[Constructable]
		public GauntletMage() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a gauntlet mage";
			Body = Utility.RandomList( 24, 148, 143, 153, 125 );
			BaseSoundID = 0;

			SetStr( 100 );
			SetDex( 100 );
			SetInt( 50000 );

			SetHits( 100 );

			SetDamage( 4, 5 );

			SetSkill( SkillName.MagicResist, 100.0 );
			SetSkill( SkillName.Meditation, 5000.0 );
			SetSkill( SkillName.Magery, 10.0 );

			Fame = 1500;
			Karma = -1500;

			VirtualArmor = 30;
		}

		public override void OnGaveMeleeAttack(Mobile defender)
		{
			base.OnGaveMeleeAttack(defender);

			defender.Stam -= 10;
			defender.SendMessage( "Your stamina is being drained!" );
		}

		public GauntletMage( Serial serial ) : base( serial )
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