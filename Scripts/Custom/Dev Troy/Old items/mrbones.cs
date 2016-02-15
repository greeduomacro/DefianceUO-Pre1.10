using System;
using Server.Misc;
using System.Collections;

/* MrBones: RunUO script written by David, Feb 2004.
 * Special release for paid support area only.
 * Please do not distribute, including upload to RunUO.com.
 * Thank you.
 */

namespace Server.Items
{
	[FlipableAttribute( 0x1A01, 0x1A02, 0x1A03, 0x1A04 )]
	public class MrBones : Item
	{
		private bool i_Active = true;
		private int i_Range = 4;
		private DateTime i_SpeakNext = DateTime.Now;

		private string[] i_Messages =
		{
			"Beware! Beware!",
			"Turn back while you still can!",
			"The living are not welcomed here",
			"Enter if you wish to join the undead",
			"*bones rattle*",
			"*laughter*",
		};

		[CommandProperty( AccessLevel.GameMaster )]
		public string Speak1
		{
			get { return i_Messages[0]; }
			set { i_Messages[0] = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string Speak2
		{
			get { return i_Messages[1]; }
			set { i_Messages[1] = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string Speak3
		{
			get { return i_Messages[2]; }
			set { i_Messages[2] = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string Speak4
		{
			get { return i_Messages[3]; }
			set { i_Messages[3] = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string Speak5
		{
			get { return i_Messages[4]; }
			set { i_Messages[4] = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string Speak6
		{
			get { return i_Messages[5]; }
			set { i_Messages[5] = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Active
		{
			get { return i_Active; }
			set { i_Active = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Range
		{
			get { return i_Range; }
			set { i_Range = value & 0xF; }
		}

		public override bool HandlesOnMovement{ get{ return true; } }
		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if( i_Active && m.Player && !m.Hidden && m.InRange( this, i_Range ) )
			{
				if ( DateTime.Now > i_SpeakNext )
				{
					i_SpeakNext = (DateTime.Now).AddSeconds( Utility.RandomMinMax( 5, 15 ) );

					string msg = i_Messages[Utility.Random( i_Messages.Length )];
					PublicOverheadMessage( Network.MessageType.Regular, 0x3B2, false, msg );
				}
			}
		}

		[Constructable]
		public MrBones() : base( 0x1A03 )
		{
			Movable = false;
		}

		public MrBones( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
			writer.Write( i_Active );
			writer.Write( i_Range );

			for ( int i = 0; i < i_Messages.Length; ++i )
				writer.Write( i_Messages[i] );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			i_Active = reader.ReadBool();
			i_Range = reader.ReadInt();

			for ( int i = 0; i < i_Messages.Length; ++i )
				i_Messages[i] = reader.ReadString();
		}
	}
}