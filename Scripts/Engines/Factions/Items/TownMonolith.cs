using System;
// jakob, we need this
using System.Collections;
// end

namespace Server.Factions
{
	public class TownMonolith : BaseMonolith
	{
		public override int DefaultLabelNumber{ get{ return 1041403; } } // A Faction Town Sigil Monolith

		// jakob, added all this
		private ArrayList m_ControlPoints = new ArrayList();

		public void AddControlPoint( ControlPoint controlPoint )
		{
			m_ControlPoints.Add( controlPoint );
			controlPoint.Owner = this.Faction;
		}

		public void RemoveControlPoint( ControlPoint controlPoint )
		{
			m_ControlPoints.Remove( controlPoint );
			controlPoint.Owner = null;
		}

		public void CaptureControlPoints( Faction f )
		{
			foreach ( ControlPoint controlPoint in m_ControlPoints )
				controlPoint.Owner = f;
		}

		public bool HasAllControlPoints( Faction faction )
		{
			foreach ( ControlPoint controlPoint in m_ControlPoints )
			{
				if ( controlPoint.Owner != faction )
					return false;
			}

			return true;
		}
		// end

		public override void OnTownChanged()
		{
			AssignName( Town == null ? null : Town.Definition.TownMonolithName );
		}

		public TownMonolith() : this( null )
		{
		}

		public TownMonolith( Town town ) : base( town, null )
		{
		}

		public TownMonolith( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			// jakob, 1 instead of 0
			writer.Write( (int) 1 ); // version
			// end

			// jakob, serialize control points
			writer.Write( m_ControlPoints.Count );
			foreach ( ControlPoint controlPoint in m_ControlPoints )
				writer.Write( controlPoint );
			// end
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				// jakob, deserialize control points
				case 1:
				{
					int count = reader.ReadInt();
					for ( int i = 0; i < count; i++ )
						m_ControlPoints.Add( reader.ReadItem() );

					break;
				}
				// end
			}
		}
	}
}