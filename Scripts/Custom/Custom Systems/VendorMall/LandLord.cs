using System;
using System.Collections;
using Server;


namespace Server.Mobiles
{

	public class LandLord : BaseLandLord
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos{ get { return m_SBInfos; } }


		[Constructable]
		public LandLord() : base( "LandLord")
		{
			Frozen = true;
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBLandLord() );
		}

		public LandLord( Serial serial ) : base( serial )
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
			Frozen = true;

		}
	}
}