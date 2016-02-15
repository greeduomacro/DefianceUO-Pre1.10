using Server.Items;


namespace Server.Multis
{
	public class MinaxFort : BaseMulti
	{
		[Constructable]
		public MinaxFort() : base( 0x5388 )
		{
		}

		public MinaxFort( Serial serial ) : base( serial )
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