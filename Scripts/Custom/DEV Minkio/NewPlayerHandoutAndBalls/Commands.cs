/*
 * Copyright (c) 2006, Kai Sassmannshausen <kai@sassie.org>
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
 */

using System;
using System.Collections;
using System.Net;
using Server.Network;
using Server.Items;
using Server.Accounting;
using Server.Mobiles;

namespace Server.Scripts.Commands
{
    public class NewPlayerHandoutCommand
    {
        public static void Initialize()
        {
            Server.Commands.Register("NewPlayerHandout", AccessLevel.Administrator, new CommandEventHandler(NewPlayerHandout_OnCommand));
        }

        [Usage("NewPlayerHandout")]
        [Description("Gives out a new player skill- and statsball to new players")]
        private static void NewPlayerHandout_OnCommand(CommandEventArgs e)
        {
            ArrayList states = NetState.Instances;
            int count = 0;

            if (states.Count > 1)
            {
                for (int i = 0; i < states.Count; ++i)
                {
                    Mobile m = ((NetState)states[i]).Mobile;
                    //Account acct = m.Account as Account;

                    if (m != null && !m.Deleted && m.Backpack != null)
                    {
                        //if (m.AccessLevel == AccessLevel.Player && acct.TotalGameTime < TimeSpan.FromDays(2))
                        PlayerMobile pm = (PlayerMobile)m as PlayerMobile;
                        if (m.AccessLevel == AccessLevel.Player && pm.Young)
                        {
                            m.AddToBackpack(new NewPlayerSkillBall(m));
                            m.AddToBackpack(new NewPlayerStatsBall(m));
                            m.SendMessage(String.Format("Welcome to Defiance, {0}! A skill- and a statsball were placed in your backpack. This items only work for you!", m.Name));
                            count++;
                        }
                    }
                }
                Server.Scripts.Commands.CommandHandlers.BroadcastMessage(AccessLevel.Counselor, 0x35, String.Format("Skill-/statsballhandout for young players preformed. ({0} Players)", count));
            }
            else
            {
                if (e.Mobile != null && !e.Mobile.Deleted)
                {
                    e.Mobile.SendMessage("Yeah! You are a genius! Preforming a handout while you are the only person on the hole server...");
                }
            }
        }
    }
}