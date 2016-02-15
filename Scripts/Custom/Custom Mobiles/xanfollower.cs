using System;
using Server;
using Server.Misc;
using Server.Network;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	public class Xanfollower : BaseCreature
	{
		public override bool ShowFameTitle{ get{ return false; } }

		[Constructable]
		public Xanfollower():base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
				Body = 400;
				Hue = 1109;
				Title = "A Follower Of Xan";
				Name = NameList.RandomName( "male" );

			if ( Female = Utility.RandomBool() )
				Body = 401;
				Hue = 1109;
				Title = "A Follower Of Xan";
				Name = NameList.RandomName( "female" );


			this.InitStats(Utility.Random(359,399), Utility.Random(138,151), Utility.Random(76,97));

			this.Skills[SkillName.Wrestling].Base = Utility.Random(120,120);
			this.Skills[SkillName.Swords].Base = Utility.Random(110,115);
			this.Skills[SkillName.Anatomy].Base = Utility.Random(120,125);
			this.Skills[SkillName.MagicResist].Base = Utility.Random(100,100);
			this.Skills[SkillName.Tactics].Base = Utility.Random(135,150);
			this.Skills[SkillName.Magery].Base = (130);
			this.Skills[SkillName.EvalInt].Base = (170);

			this.Fame = Utility.Random(5000,9999);
			this.Karma = Utility.Random(-5000,-9999);
			this.VirtualArmor = 40;

			HoodedShroudOfShadows shroud = new HoodedShroudOfShadows();
			shroud.Hue = 1109;
			shroud.LootType = LootType.Blessed;
			shroud.Name = "Follower Of Xan Shroud";
			AddItem ( shroud );

		}



		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool Unprovokable{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }



		[CommandProperty( AccessLevel.GameMaster )]
		public override int HitsMax { get { return 900; } }

		public Xanfollower( Serial serial ) : base( serial ){
		{
		}}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}