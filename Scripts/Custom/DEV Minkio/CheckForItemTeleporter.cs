/*
 * Copyright (c) 2005, Kai Sassmannshausen <kai@sassie.org>
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 *
 * - Redistributions of source code must retain the above copyright
 * notice, this list of conditions and the following disclaimer.
 *
 * - Redistributions in binary form must reproduce the above copyright
 * notice, this list of conditions and the following disclaimer in the
 * documentation and/or other materials provided with the
 * distribution.
 *
 * - Neither the name of Kai Sassmannshausen, nor the names of its
 * contributors may be used to endorse or promote products derived from
 * this software without specific prior written permission.
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND
 * CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,
 * BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
 * FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE
 * COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
 * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
 * STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
 * OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * CheckForItemTeleporter
 * A Teleporter that checks for a Item in bag without consuming it
 *
 */

using System;
using Server;
using Server.Network;

namespace Server.Items
{
	public class CheckForItemTeleporter : Teleporter
	{
		private Type m_itemType;
		private Item m_item;
		private string m_Message;

		[CommandProperty( AccessLevel.GameMaster)]
		public Type CheckForItem
		{
			get{ return m_itemType; }
			set{ m_itemType = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster)]
        	public String DeclineMessage
        	{
	    		get{ return m_Message; }
		    	set{ m_Message = value; }
		}

		[Constructable]
		public CheckForItemTeleporter()
		{
		}

		public CheckForItemTeleporter( Serial serial ) : base( serial )
		{
		}

		public override bool OnMoveOver( Mobile m )
		{

			if ( m == null || m.Deleted || m.Backpack == null || m.Backpack.Deleted )
				return true;

			if ( Active && m_itemType != null )
			{
				if ( !Creatures && !m.Player )
					return true;

				m_item = m.Backpack.FindItemByType(m_itemType, true);
				if ( m_item != null )
				{
					StartTeleport(m);
					return false;
				}
				else
				{
					if ( m_Message != null && m != null)
						m.SendMessage(m_Message);
				}


			}
			return true;
		}


		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( m_Message );

                        if ( m_itemType != null )
                        {
			        writer.Write( m_itemType.ToString() );
                        }
                        else
                        {
                                writer.Write("");
                        }
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

                        string TypeString;

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
					m_Message = reader.ReadString();
					goto case 0;
				case 0:
                                        TypeString = reader.ReadString();

                                        if ( TypeString != "" )
                                        {
			                 	m_itemType = ScriptCompiler.FindTypeByFullName(TypeString);
                                        }
                                        else
                                        {
                                                m_itemType = null;
                                        }

					break;

			}
		}
	}
}