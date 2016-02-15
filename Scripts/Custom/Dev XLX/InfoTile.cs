using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Gumps;

namespace Server.Items
{
	public class InfoTile : Item
	{
		#region Variables
		private string m_Message;
		private int m_Range;
		private bool m_Active;

		[CommandProperty( AccessLevel.GameMaster )]
		public string Message
		{
			get { return m_Message; }
			set { m_Message = value; }
		}


		[CommandProperty( AccessLevel.GameMaster )]
		public int Range
		{
			get { return m_Range; }
			set { m_Range = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Active
		{
			get { return m_Active; }
			set { m_Active = value; InvalidateProperties(); }
		}
		#endregion

		[Constructable]
		public InfoTile() : base( 0x1F1C )
		{
			ItemID = 1305;
			Name = "Help Trigger";
			Hue = 1365;
			Visible = false;
			Movable = false;

			m_Range = 4;
			m_Active = false;
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			list.Add(1060658, "Range\t{0}", m_Range);
			list.Add(1060659, "Active\t{0}", m_Active);
		}

		public override bool HandlesOnMovement { get { return true; } }

		public override void OnMovement(Mobile m, Point3D oldLocation)
		{
			if ( m_Active && m.Player )
			{
				bool oldrange = Utility.InRange( oldLocation, Location, m_Range );
				bool newrange = Utility.InRange( m.Location, Location, m_Range );

				if ( newrange && !oldrange )
					m.SendGump( new InfoTileGump( m, Message, this.Location ) );
				else if ( !newrange && oldrange )
					m.CloseGump( typeof( InfoTileGump ) );
			}
		}

		public InfoTile( Serial serial ) : base( serial )
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)1);

			writer.Write((bool)m_Active);
			writer.Write((int)m_Range);
			writer.Write((string)m_Message);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

			m_Active = reader.ReadBool();
			m_Range = reader.ReadInt();

			if (version < 1)
				reader.ReadDateTime();

			m_Message = reader.ReadString();
		}
	}
}