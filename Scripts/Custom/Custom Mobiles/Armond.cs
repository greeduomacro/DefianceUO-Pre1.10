using System;
using Server.Misc;
using Server.Network;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	public class Armond : BaseCreature
	{

		[Constructable]
		public Armond():base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Body = 0x190;
			Name = "Armond";
			Title = "The Dark One";

			SetStr( 356, 396 );
			SetDex( 105, 135 );
			SetInt( 530, 653 );
			SetSkill( SkillName.Wrestling, 91.3, 97.8 );
			SetSkill( SkillName.Tactics, 91.5, 99.0 );
			SetSkill( SkillName.MagicResist, 90.6, 96.8);
			SetSkill( SkillName.Magery, 91.7, 99.0 );
			SetSkill( SkillName.EvalInt, 100.1, 100.1 );
			SetSkill( SkillName.Meditation, 121.1, 128.1 );

			VirtualArmor = 64;

			new Kirin().Rider = this;

			SetFameLevel( 8 );
			SetKarmaLevel( -8 );

			LeatherGloves gloves = new LeatherGloves();
			gloves.Hue = 1;
			AddItem( gloves );

			HoodedShroudOfShadows shroud = new HoodedShroudOfShadows();
			shroud.Hue = 1157;
			AddItem( shroud );

			Cloak cloak = new Cloak();
			cloak.Hue = 1157;
			AddItem( cloak );

			Kilt kilt = new Kilt();
			kilt.Hue = 1;
			AddItem( kilt );

			Sandals sandals = new Sandals();
			sandals.Hue = 1;
			AddItem( sandals );
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool Unprovokable{ get{ return true; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public override int HitsMax { get { return 753; } }

		public Armond( Serial serial ) : base( serial )
		{
		}

		public override bool OnBeforeDeath()
		{
			BoneMagi rm = new BoneMagi();
			rm.Team = this.Team;
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

			switch ( Utility.Random( 8 ))
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

			bag.DropItem( new Gold( 2000, 3500 ));
			rm.AddItem( bag );

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