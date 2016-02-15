/**************************************
*        Container Rename v1.0        *
*      Distro files: Container.cs     *
*                                     *
*    Created by Joeku AKA Demortris   *
*              3/2/2006               *
*                                     *
* Anyone can modify/redistribute this *
*  DO NOT REMOVE/CHANGE THIS HEADER!  *
**************************************/

using System;
using Server;
using Server.ContextMenus;
using Server.Prompts;

namespace Server.Items
{
	public class ContainerRenamePrompt : Prompt
	{
		private Mobile m_Mobile;
		private BaseContainer i_Cont;

		public ContainerRenamePrompt( Mobile m, BaseContainer cont )
		{
			m_Mobile = m;
			i_Cont = cont;
		}

		public override void OnResponse( Mobile from, string text )
		{
			text = text.Trim();

			if ( text.Length > 40 )
				text = text.Substring( 0, 40 );

			if( !i_Cont.IsChildOf( from.Backpack ) && !i_Cont.IsChildOf( from.BankBox ))
				from.SendMessage("That must be in your pack or bank box for you to rename it.");
			else if ( text.Length > 0 )
			{
				i_Cont.Name = text;
				from.SendMessage("You rename the container to '{0}'.", text);
			}
		}
	}

	public class ContainerRenameEntry : ContextMenuEntry
	{
		private Mobile m_From;
		private BaseContainer i_Cont;

		public ContainerRenameEntry( Mobile from, BaseContainer cont ) : base( 5104 )
		{
			m_From = from;
			i_Cont = cont;
		}

		public override void OnClick()
		{
			if ( i_Cont.IsChildOf( m_From.Backpack ) || i_Cont.IsChildOf( m_From.BankBox) )
			{
				m_From.SendMessage("What do you want to rename this to?");
				m_From.SendMessage("(Esc to cancel)");
				m_From.Prompt = new ContainerRenamePrompt( m_From, i_Cont );
			}
			else
				m_From.SendMessage("That must be in your pack or bank box for you to rename it.");
		}
	}
}