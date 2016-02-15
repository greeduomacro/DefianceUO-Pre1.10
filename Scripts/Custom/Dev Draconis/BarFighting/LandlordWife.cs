using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "an ogre corpse" )]
	public class LandlordWife : BaseCreature
	{
		[Constructable]
		public LandlordWife () : base( AIType.AI_Melee, FightMode.Closest, 10, 2, 0.2, 0.4 )
		{
			Name = "Blady";
			Title = "the landlord's wife";
			Body = 1;
			BaseSoundID = 427;
			RangePerception = 3;

			SetStr( 300 );
			SetDex( 150 );
			SetInt( 100 );

			SetHits( 10000 );

			SetDamage( 25, 35 );

			SetSkill( SkillName.MagicResist, 100.0 );
			SetSkill( SkillName.Tactics, 120.0 );
			SetSkill( SkillName.Wrestling, 130.0 );

			VirtualArmor = 32;

			Club c = new Club();
			c.MaxRange = 2;
			AddItem( c );
		}

		public override bool DisallowAllMoves{ get{ return true; } }

		public override void OnGaveMeleeAttack(Mobile defender)
		{
			base.OnGaveMeleeAttack(defender);

			string[] hit = {"Behave you scum!",
					"Keep your paws of me!",
					"Don't get behind the bar!",
					"First pay then play!",
					"Away with thee!",
					"Want another beer? Come get some!",
					"You stinking drunk!",
					"Don't need a club for pussies like you!",
					"You little creatins!!! Get out of my bar!",
					"You call that a swing?",
					"Shove it love!",
					"I've seen a frown hit harder then you!"};
			Say(hit[Utility.Random(12)] );
		}

		public LandlordWife( Serial serial ) : base( serial )
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