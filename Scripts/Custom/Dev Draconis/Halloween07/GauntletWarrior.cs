using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a Gauntlet creature's corpse" )]
	public class GauntletWarrior : BaseCreature
	{
		[Constructable]
		public GauntletWarrior() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a gauntlet warrior";
			Body = Utility.RandomList( 42, 17, 154, 50, 3 , 35 );
			BaseSoundID = 0;

			SetStr( 100 );
			SetDex( 100 );
			SetInt( 100 );

			SetHits( 100 );

			SetDamage( 2, 3 );

			SetSkill( SkillName.MagicResist, 100.0 );
			SetSkill( SkillName.Tactics, 50.0 );
			SetSkill( SkillName.Wrestling, 50.0 );

			Fame = 1500;
			Karma = -1500;

			VirtualArmor = 30;
		}

		public override void OnGaveMeleeAttack(Mobile defender)
		{
			base.OnGaveMeleeAttack(defender);

			defender.Poison = Poison.Greater;
			defender.Stam -= 5;
			defender.SendMessage( "Your stamina is being drained!" );
		}

		public GauntletWarrior( Serial serial ) : base( serial )
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