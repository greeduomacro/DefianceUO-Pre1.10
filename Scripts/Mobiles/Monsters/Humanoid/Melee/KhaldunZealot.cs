using System;
using Server.Misc;
using Server.Network;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	public class KhaldunZealot : BaseCreature
	{
		[Constructable]
		public KhaldunZealot():base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Body = 0x190;
			Name = "Khaldun Zealot";
			Hue = 0;

			this.InitStats(Utility.Random(359,399), Utility.Random(138,151), Utility.Random(76,97));

			this.Skills[SkillName.Wrestling].Base = Utility.Random(74,80);
			this.Skills[SkillName.Swords].Base = Utility.Random(90,95);
			this.Skills[SkillName.Anatomy].Base = Utility.Random(120,125);
			this.Skills[SkillName.MagicResist].Base = Utility.Random(90,94);
			this.Skills[SkillName.Tactics].Base = Utility.Random(90,95);

			this.Fame = Utility.Random(5000,9999);
			this.Karma = Utility.Random(-5000,-9999);
			this.VirtualArmor = 40;

			BoneArms arms = new BoneArms();
			arms.Hue = 0x3A8;
			arms.LootType = LootType.Blessed;
			AddItem( arms );

			BoneGloves gloves = new BoneGloves();
			gloves.Hue = 0x3A8;
			gloves.LootType = LootType.Blessed;
			AddItem( gloves );

			BoneChest tunic = new BoneChest();
			tunic.Hue = 0x3A8;
			tunic.LootType = LootType.Blessed;
			AddItem( tunic );
			BoneLegs legs = new BoneLegs();
			legs.Hue = 0x3A8;
			legs.LootType = LootType.Blessed;
			AddItem( legs );

			BoneHelm helm = new BoneHelm();
			helm.Hue = 0x3A8;
			helm.LootType = LootType.Blessed;
			AddItem( helm );

			AddItem( new Shoes() );
			AddItem( new Buckler());

			VikingSword weapon = new VikingSword();

			weapon.Movable = true;

			AddItem( weapon );
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool Unprovokable{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public override int HitsMax { get { return 769; } }

		public KhaldunZealot( Serial serial ) : base( serial )
		{
		}

		public override bool OnBeforeDeath()
		{
			SkeletalKnight rm = new SkeletalKnight();
			rm.Map = this.Map;
			rm.Location = this.Location;
			Effects.SendLocationEffect( Location,Map, 0x3709, 13, 0x3B2, 0 );

			Container bag = new Bag();
			switch ( Utility.Random( 9 ))
			{
				case 0: bag.DropItem( new Amber() ); break;
				case 1: bag.DropItem( new Amethyst() ); break;
				case 2: bag.DropItem( new Citrine() ); break;
				case 3: bag.DropItem( new Diamond() ); break;
				case 4: bag.DropItem( new Emerald() ); break;
				case 5: bag.DropItem( new Ruby() ); break;
				case 6: bag.DropItem( new Sapphire() ); break;
				case 7: bag.DropItem( new StarSapphire() ); break;
				case 8: bag.DropItem( new Tourmaline() ); break;
			}

			switch ( Utility.Random( 25 ))
			{
				case 0: bag.DropItem( new SpidersSilk( 3 ) ); break;
				case 1: bag.DropItem( new BlackPearl( 3 ) ); break;
				case 2: bag.DropItem( new Bloodmoss( 3 ) ); break;
				case 3: bag.DropItem( new Garlic( 3 ) ); break;
				case 4: bag.DropItem( new MandrakeRoot( 3 ) ); break;
				case 5: bag.DropItem( new Nightshade( 3 ) ); break;
				case 6: bag.DropItem( new SulfurousAsh( 3 ) ); break;
				case 7: bag.DropItem( new Ginseng( 3 ) ); break;
			}

			bag.DropItem( new Gold( 1000, 1500 ));
            if (2 > Utility.Random(100)) bag.DropItem(new BloodPentagramPart(Utility.RandomMinMax(24, 28)));
            rm.AddItem(bag);

			this.Delete();

			return false;
		}

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