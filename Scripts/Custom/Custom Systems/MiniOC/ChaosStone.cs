// part of Public Chaos-Order War system
//scripted by Unclouded.. www.unclouded.tk

using Server;
using System;
using System.IO;
using System.Collections;
using Server.Gumps;
using Server.Guilds;
using Server.Network;
using Server.Mobiles;
using Server.Factions;

namespace Server.Items
{
	public class ChaosStone : Item
	{
		private Guild m_Guild;

		public Guild Guild { get { return m_Guild; } }

		public override int LabelNumber{ get{ return 1041429; } } // a guildstone

		public ChaosStone( Guild g ) : base( 0xEDC )
		{
			m_Guild = g;
			Movable = false;
		}

		public ChaosStone( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 ); // version
			writer.Write( m_Guild );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				{
					m_Guild = reader.ReadGuild() as Guild;
					goto case 0;
				}
				case 0:
				{
					break;
				}
			}

			if ( m_Guild == null )
				this.Delete();
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			if ( m_Guild != null )
			{
				string name;

				if ( (name = m_Guild.Name) == null || (name = name.Trim()).Length <= 0 )
					name = "(unnamed)";

				list.Add( 1060802, name ); // Guild name: ~1_val~
			}
		}

		public override void OnSingleClick( Mobile from )
		{
			base.OnSingleClick( from ) ;

			string name;

			if ( m_Guild == null )
				name = "(unfounded)";
			else if ( (name = m_Guild.Name) == null || (name = name.Trim()).Length <= 0 )
				name = "(unnamed)";

			this.LabelTo( from, name );
		}

		public override void OnAfterDelete()
		{
			if ( m_Guild != null && !m_Guild.Disbanded )
				m_Guild.Disband();
		}


		public bool validJoin ( Mobile from, Guild stoneGuild )
		{
			if ( from == null || from.Deleted )
				return false;

            if (!from.InRange(GetWorldLocation(), 2))
            {
                from.SendMessage("The stone is not in range. Step closer to it.");
                return false;
            }

			if ( stoneGuild.IsMember( from ) )
			{
				from.SendMessage(0x35, "You already joined Chaos via this stone, to resign say 'I resign from my guild'.");
				return false;
			}

            if ( from.Guild != null )
            {
                from.SendMessage(0x35, "You are already member of a guild.");
                return false;
            }

			PlayerState joinerState = PlayerState.Find( from );
			if ( joinerState != null )
			{
				from.SendMessage(0x35, "You cant join this guild as a faction member.");
				return false;
			}

            PlayerMobile mp_from = (PlayerMobile)from as PlayerMobile;
			if (mp_from.NpcGuild == NpcGuild.ThievesGuild )
			{
				from.SendMessage(0x35, "You cant join this gate as a member of the thieves guild.");
				return false;
			}

            return true;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !validJoin(from, m_Guild) )
				return;

			m_Guild.AddMember( from );
			from.SendMessage( 0x35, "You are now a member of Chaos. To Resign say 'i resign from my guild'.");

			if ( from.Backpack != null )
			{
				Item m_item = from.Backpack.FindItemByType(typeof(ChaosShield), true);
				if ( m_item == null )
				{
					from.Backpack.DropItem( new ChaosShield () );
					from.SendMessage( 0x35, "You receive a Chaos Shield.");
				}
			}


		}
	}
}