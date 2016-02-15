using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Multis.Deeds;

namespace Server.Multis.Deeds
{
	public abstract class AOSHouseDeed : HouseDeed
	{
		protected int m_MultiID;
		protected int m_MaxLockdowns;
		protected int m_MaxSecures;

		//[Constructable]
		public AOSHouseDeed( string size, int multiid, int maxlocksdowns, int maxsecures, Point3D offset ) : base( multiid, offset )
		{
			m_MultiID = multiid;
			Name = String.Format( "a deed for a customizable house with a size of {0} foot prints", size );
		}

		public AOSHouseDeed( Serial serial ) : base( serial )
		{
		}

		public override BaseHouse GetHouse( Mobile owner )
		{
			return new HouseFoundation( owner, m_MultiID, m_MaxLockdowns, m_MaxSecures );
		}

		//public override int LabelNumber{ get{ return 1041240; } }
		public override Rectangle2D[] Area{ get{ return TwoStoryVilla.AreaArray; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_MultiID );
			writer.Write( m_MaxLockdowns );
			writer.Write( m_MaxSecures );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			m_MultiID = reader.ReadInt();
			m_MaxLockdowns = reader.ReadInt();
			m_MaxSecures = reader.ReadInt();
		}
	}

	public class AOSHouseDeed7x12 : AOSHouseDeed
	{
		[Constructable]
		public AOSHouseDeed7x12() : base( "7x12", 0x13F1, 400, 800, new Point3D( 0, 7, 0 ) )
		{
		}

		public AOSHouseDeed7x12( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
		}
	}
	public class AOSHouseDeed12x7 : AOSHouseDeed
	{
		[Constructable]
		public AOSHouseDeed12x7() : base( "12x7", 0x1428, 400, 800, new Point3D( 0, 4, 0 ) )
		{
		}

		public AOSHouseDeed12x7( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
		}
	}

	public class AOSHouseDeed15x12 : AOSHouseDeed
	{
		[Constructable]
		public AOSHouseDeed15x12() : base( "15x12", 0x1451, 675, 1350, new Point3D( 0, 7, 0 ) )
		{
		}

		public AOSHouseDeed15x12( Serial serial ) : base( serial )
		{
		}


		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
		}
	}
	public class AOSHouseDeed12x15 : AOSHouseDeed
	{
		[Constructable]
		public AOSHouseDeed12x15() : base( "12x15", 0x1430, 675, 1350, new Point3D( 0, 8, 0 ) )
		{
		}

		public AOSHouseDeed12x15( Serial serial ) : base( serial )
		{
		}


		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
		}
	}
	public class AOSHouseDeed18x18 : AOSHouseDeed
	{
		[Constructable]
		public AOSHouseDeed18x18() : base( "18x18", 0x147B, 1059, 2119, new Point3D( 0, 10, 0 ) )
		{
		}

		public AOSHouseDeed18x18( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
		}
	}
}