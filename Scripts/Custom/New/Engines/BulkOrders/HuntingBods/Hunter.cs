using System;
using System.Collections;
using Server;
using Server.Engines.BulkOrders;
using Server.Items;

namespace Server.Mobiles
{
	public class Hunter : BaseHuntContractVendor
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos{ get { return m_SBInfos; } }

		[Constructable]
		public Hunter() : base( "the hunter" )
		{
			SetSkill( SkillName.Tailoring, 64.0, 100.0 );
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBHunter() );
		}

		public override void InitOutfit()
		{
			AddItem( new LeatherChest() );
			AddItem( new LeatherLegs() );
			AddItem( new LeatherArms() );
			AddItem( new Boots() );
			AddItem( new BearMask() );
			AddItem( new Quiver() );
                        AddItem( new CompositeBow() );

			int hairHue = GetHairHue();
			AddItem( new ShortHair( hairHue ) );

			switch ( Utility.Random( 5 ) )
			{
				case 0: AddItem( new LongBeard( hairHue ) ); break;
				case 1: AddItem( new MediumLongBeard( hairHue ) ); break;
				case 2: AddItem( new Vandyke( hairHue ) ); break;
				case 3: AddItem( new Mustache( hairHue ) ); break;
				case 4: AddItem( new Goatee( hairHue ) ); break;
			}

			PackGold( 100, 200 );
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			SpeechHue = Utility.RandomDyedHue();
			Hue = Utility.RandomSkinHue();

			if ( IsInvulnerable && !Core.AOS )
				NameHue = 0x35;

			Body = 0x190;
			Name = NameList.RandomName( "male" );
		}

		public Hunter( Serial serial ) : base( serial )
		{
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