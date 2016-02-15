using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a Gauntlet creature's corpse" )]
	public class GauntletArcher : BaseCreature
	{
		[Constructable]
		public GauntletArcher() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a gauntlet archer";
			Body = Utility.RandomList( 142, 182, 767, 149, 130 );
			BaseSoundID = 0;

			SetStr( 100 );
			SetDex( 100 );
			SetInt( 100 );

			SetHits( 100 );

			SetDamage( 2, 3 );

			SetSkill( SkillName.MagicResist, 100.0 );
			SetSkill( SkillName.Tactics, 50.0 );
			SetSkill( SkillName.Archery, 20.0 );

			Fame = 1500;
			Karma = -1500;

			VirtualArmor = 30;

			AddItem( new Bow() );
		}

		public override void OnGaveMeleeAttack(Mobile defender)
		{
			base.OnGaveMeleeAttack(defender);

			defender.Stam -= 5;
			defender.SendMessage( "Your stamina is being drained!" );
		}

		public GauntletArcher( Serial serial ) : base( serial )
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