using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	public class GreaterSkeleton : BaseRareBoss
	{
		[Constructable]
		public GreaterSkeleton() : base( AIType.AI_Melee )
		{
			Name = "a greater skeleton";
			Body = Utility.RandomList( 50, 56 );
			BaseSoundID = 0x48D;
			Hue = 999;

			SetStr( 250, 300 );

			SetHits( 150, 200 );

			SetSkill( SkillName.MagicResist, 80.1, 100.0 );
			SetSkill( SkillName.Wrestling, 50.1, 80.0 );
			SetSkill( SkillName.Poisoning, 90.1, 100.0 );
			SetSkill( SkillName.Tactics, 80.1, 100.0 );
			SetSkill( SkillName.Meditation, 110.1, 120.0 );

			switch ( Utility.Random( 5 ))
			{
				case 0: PackItem( new BoneArms() ); break;
				case 1: PackItem( new BoneChest() ); break;
				case 2: PackItem( new BoneGloves() ); break;
				case 3: PackItem( new BoneLegs() ); break;
				case 4: PackItem( new BoneHelm() ); break;
			}

			if (0.50 >= Utility.RandomDouble())
           		switch (Utility.Random(5))
                	{
                    		case 0: PackItem(new SpecialHairDye()); break;
                    		case 1: PackItem(new SpecialBeardDye()); break;
                    		case 2: PackItem(new ClothingBlessDeed()); break;
                    		case 3: PackItem(new NameChangeDeed()); break;
                	}
		}

		public override void OnDeath( Container c )
		{
			base.OnDeath( c );
		}

		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override bool ThrowAtomicBomb{ get { return true; } }

		public GreaterSkeleton( Serial serial ) : base( serial )
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