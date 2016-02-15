using System;

namespace Server.Items
{
	public class BellOfCourage : Item
	{
		private static readonly string[] m_Names = new string[]
			{
				"X Lord X", "LiGhT", "Kamron",
				"Minkio", "Al", "Rasputin", "Astrid",
				"Wolf", "Alfa", "Shadow", "Kale", "Wing",
                                "Savage", "Tjalfe", "Mephiston", "Nystal",
                                "Roxi", "Draconis", "Moonshine", "Jon", "Saleon",
                                "Nymphaea", "Blady", "Hermes", "Jeremy", "Hekate"
			};

		private int m_Sound;

		[Constructable]
		public BellOfCourage() : base( 0x1C12 )
		{
			Stackable = false;
			Weight = 1.0;
			Name = m_Names[Utility.Random( m_Names.Length )];
			Hue = 0.02 >= Utility.RandomDouble() ? 1150 : Utility.RandomList( 40, 26, 136, 2113, 1501, 265, 2001, 76, 86, 1421, 2119, 1303, 515, 314 );
			LootType = LootType.Blessed;
			m_Sound = Utility.Random( 245, 18 );
		}

		public BellOfCourage( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			Effects.PlaySound( from, from.Map, m_Sound );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 );
			writer.Write( m_Sound );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
			m_Sound = reader.ReadInt();
		}
	}
}