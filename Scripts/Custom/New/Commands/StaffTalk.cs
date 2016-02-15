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
 * - Neither the name of Kai Sassmannshausen nor the names of its contributors
 * may be used to endorse or promote products derived from this software without
 * specific prior written permission.
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
 *
 *  Version 1.0
 */

using System;
using Server;

namespace Server.Scripts.Commands
{

    public class asCommand
    {
        public static void Initialize()
	{
        	Server.Commands.Register( "as", AccessLevel.Administrator, new CommandEventHandler( as_OnCommand ) );
		Server.Commands.Register( "AdminTalk", AccessLevel.Administrator, new CommandEventHandler( as_OnCommand ) );
	}


        [Usage( "as" )]
	[Aliases( "AdmiTalk" )]
	[Description( "Admin talk. Same like [s, but only for Administrators." )]
	private static void as_OnCommand( CommandEventArgs e )
	{
		Server.Scripts.Commands.CommandHandlers.BroadcastMessage( AccessLevel.Administrator, e.Mobile.SpeechHue, String.Format( "AdminTalk from {0}: {1}", e.Mobile.Name, e.ArgString ));
	}
    }


    public class ssCommand
    {
        public static void Initialize()
        {
            Server.Commands.Register("ss", AccessLevel.Seer, new CommandEventHandler(ss_OnCommand));
            Server.Commands.Register("SeerTalk", AccessLevel.Seer, new CommandEventHandler(ss_OnCommand));
        }


        [Usage("ss")]
        [Aliases("SeerTalk")]
        [Description("Seer talk. Same like [s, but only for Seers and higher.")]
        private static void ss_OnCommand(CommandEventArgs e)
        {
            Server.Scripts.Commands.CommandHandlers.BroadcastMessage(AccessLevel.Seer , e.Mobile.SpeechHue, String.Format("SeerTalk from {0}: {1}", e.Mobile.Name, e.ArgString));
        }
    }


    public class gsCommand
    {
        public static void Initialize()
	{
        	Server.Commands.Register( "gs", AccessLevel.GameMaster, new CommandEventHandler( gs_OnCommand ) );
		Server.Commands.Register( "GMTalk", AccessLevel.GameMaster, new CommandEventHandler( gs_OnCommand ) );
	}

        [Usage( "gs" )]
	[Aliases( "GMTalk" )]
	[Description( "Gamemaster talk. Same like [s, but only for Gamemasters and higher." )]
	private static void gs_OnCommand( CommandEventArgs e )
	{
		Server.Scripts.Commands.CommandHandlers.BroadcastMessage( AccessLevel.GameMaster, e.Mobile.SpeechHue, String.Format( "GMTalk from {0}: {1}", e.Mobile.Name, e.ArgString ));
	}
    }

}