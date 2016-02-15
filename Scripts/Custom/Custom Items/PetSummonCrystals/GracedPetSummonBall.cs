using Server.ContextMenus;
using System;
using Server;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using System.Collections;
using Server.Items;
using Server.Misc;
using Server.Gumps;

namespace Server.Items
{
	public class GracedPetSummonBall : Item
	{
		public int m_Charges;

		[Constructable]
		public GracedPetSummonBall() : base(0xE2E)
		{
			Weight = 1.0;
                        Hue = 1154;
			Movable = true;
			m_Charges = 10;
			Name= "Graced Crystal Ball of Pet Summoning";
		}

		public GracedPetSummonBall( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
			writer.Write( (int) m_Charges );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			switch ( version )
			{
				case 0:
				{
					break;
				}
			}
			m_Charges = reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !(from is PlayerMobile) )
				return;
			ArrayList pets = GetPetList( (PlayerMobile)from );

			if ( pets.Count <= 0 )
			{
				from.SendMessage( "You don't have any pets." );
				return;
			}


			GracedPetSummonGump.EnsureClosed( from );
			from.SendGump( new GracedPetSummonGump( (PlayerMobile)from, this ) );
		}

		public override void OnSingleClick( Mobile from )
		{
			base.OnSingleClick( from );
			LabelTo( from, "Charges: " + m_Charges );
		}

		public void UseCharge()
		{
			m_Charges --;
			if ( m_Charges <= 0 )
				this.Delete();
		}

		public ArrayList GetPetList( PlayerMobile from )
		{
			ArrayList pets = new ArrayList();

			foreach ( Mobile m in World.Mobiles.Values )
			{
				if ( m is BaseCreature )
				{
					BaseCreature bc = (BaseCreature)m;

					if ( bc is IMount && ((IMount)bc).Rider != null )
						continue;

					if ( (bc.Controlled && bc.ControlMaster == from) )
						pets.Add( bc );
				}
			}

			return pets;
		}

		[CommandProperty( AccessLevel.Seer )]
		public int Charges
		{
			get
			{
				return m_Charges;
			}
			set
			{
				m_Charges = value;
			}
		}
	}
}