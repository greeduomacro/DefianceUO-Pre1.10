using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a fire elemental corpse" )]
	public class SummonedFireElemental : BaseCreature
	{
		[Constructable]
		public SummonedFireElemental () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a fire elemental";
			Body = 15;
			BaseSoundID = 274;

			SetStr( 200 );
			SetDex( 200 );
			SetInt( 100 );

			SetDamage( 9, 14 );

			SetSkill( SkillName.EvalInt, 90.0 );
			SetSkill( SkillName.Magery, 90.0 );
			SetSkill( SkillName.MagicResist, 85.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 92.0 );

			VirtualArmor = 40;
			ControlSlots = 4;

			AddItem( new LightSource() );
		}

		public SummonedFireElemental( Serial serial ) : base( serial )
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