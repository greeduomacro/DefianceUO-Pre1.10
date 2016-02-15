// part of Public Chaos-Order War system
//scripted by Unclouded.. www.unclouded.tk

using System;
using Server.Network;
using Server.Prompts;
using Server.Guilds;
using Server.Multis;
using Server.Regions;

namespace Server.Items
{
	public class ChaosDeed : Item
	{

		[Constructable]
		public ChaosDeed() : base( 0x14F0 )
		{
		Weight = 0.5;
		Name = "A Chaos Deed";
		}

		public ChaosDeed( Serial serial ) : base( serial )
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


		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else
			{
				from.SendMessage( 0x35, "You are about to place a Chaosstone");
				from.SendMessage( 0x35, "Type Chaos and press enter");
				from.Prompt = new CoPrompt( this );
			}
		}

		private class CoPrompt : Prompt
		{
			private ChaosDeed m_Deed;

				public CoPrompt( ChaosDeed deed )
			{
				m_Deed = deed;
			}

			public override void OnResponse( Mobile from, string text )
			{
				if ( m_Deed.Deleted )
					return;

				if ( !m_Deed.IsChildOf( from.Backpack ) )
				{
					from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.

				}
				else
				{
							m_Deed.Delete();

							Guild guild = new Guild( from, "Chaos", "none" );

							from.Guild = guild;
							from.GuildTitle = "Guildmaster" ;

							ChaosStone stone = new ChaosStone( guild );

							stone.MoveToWorld( from.Location, from.Map );

							guild.Guildstone = stone;
							guild.Type = GuildType.Chaos;
							guild.Abbreviation = "RP";



				}


			}
		}
	}
}