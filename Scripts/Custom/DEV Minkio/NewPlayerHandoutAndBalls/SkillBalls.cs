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
 *
 * Version 1
 */

using System;
using System.Collections;
using System.Net;
using Server.Network;
using Server.Items;
using Server.Gumps;
using Server.Accounting;

namespace Server.Items
{
    public class SevenGMSkillBall : Item
    {
        protected float f_SkillValue = 100;
        protected string GumpHeadline = "7xGM Skillball - Choose your seven skills...";

		[Constructable]
		public SevenGMSkillBall() : base(3699)
		{
			Movable = true;
			Hue = 1161;
			Name = "a Seven GM SkillBall";
			LootType = LootType.Blessed;
		}

		public SevenGMSkillBall(Serial serial) : base( serial )
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

		}

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual float SkillValue {
            get { return f_SkillValue; }
            //set { f_SkillValue = value }
        }

		public override void OnDoubleClick(Mobile from)
		{

		  if (from == null || from.Deleted || from.Backpack == null)
		    return;

		  if (!IsChildOf(from.Backpack)) {
		    from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
		    return;
		  }

          if (!IsValidSkillBallUse(from))
              return;

		  from.CloseGump(typeof(SevenGMSkillBallGump));
		  from.SendGump(new SevenGMSkillBallGump(this, from, GumpHeadline));

		}

        public virtual bool IsValidSkillBallUse(Mobile from)
        {
            return true;
        }

		public virtual bool IsRestrictedSkill(SkillName i)
		{
			return false;
		}
	}

	public class LimitedSevenGMSkillBall : SevenGMSkillBall
	{
		[Constructable]
		public LimitedSevenGMSkillBall()
		{
			Movable = true;
			Hue = 1173;
			Name = "a limited 7GM Skill Ball";

            f_SkillValue = 100;
            GumpHeadline = "Limited 7xGM Skillball - Choose your seven skills...";
        }

		public LimitedSevenGMSkillBall(Serial serial) : base(serial)
		{
		}

		public override bool IsRestrictedSkill(SkillName i)
		{
			// Skills that are n/a at the gump
			return i == SkillName.Blacksmith || i == SkillName.Peacemaking || i == SkillName.Provocation ||
				i == SkillName.Musicianship || i == SkillName.Tailoring || i == SkillName.AnimalTaming ||
				i == SkillName.Lockpicking || i == SkillName.RemoveTrap || i == SkillName.Mining ||
				i == SkillName.Necromancy || i == SkillName.Focus || i == SkillName.Chivalry ||
				i == SkillName.AnimalLore || i == SkillName.Cartography || i == SkillName.Carpentry ||
				i == SkillName.Fishing || i == SkillName.Tinkering || i == SkillName.Alchemy ||
				i == SkillName.ItemID || i == SkillName.Begging || i == SkillName.Fletching ||
				i == SkillName.Veterinary || i == SkillName.Poisoning;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

		}
	}


    public class NewPlayerSkillBall : SevenGMSkillBall
    {
        private Mobile m_NewPlayer;

		[Constructable]
		public NewPlayerSkillBall(Mobile newplayer)
		{
			Movable = true;
			Hue = 1163;
			Name = "a New Player Skill Ball";

            m_NewPlayer = newplayer;

            // protected vars from parent class
            f_SkillValue = 90;
            GumpHeadline = "New Player Skillball - Choose your seven skills...";
        }

        public NewPlayerSkillBall(Serial serial) : base(serial)
		{
		}

        [CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
        public Mobile Owner
        {
            get { return m_NewPlayer; }
			set { m_NewPlayer = value; }
        }

        public override void OnSingleClick( Mobile from )
        {
            if ( Owner != null )
                LabelTo(from, String.Format("personal new player skillball for {0}", Owner.Name ));
            else
                base.OnSingleClick(from);
        }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)1); // version
            writer.Write(f_SkillValue);
            writer.Write((Mobile)m_NewPlayer);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        f_SkillValue = reader.ReadFloat();
                        m_NewPlayer = reader.ReadMobile();
                        break;
                    }
            }
		}

        public override void OnDoubleClick(Mobile from)
        {
            if ( from == null || from.Deleted || from.Backpack == null )
                return;

            if ( !IsChildOf( from.Backpack ) )
                from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
            else if ( IsValidSkillBallUse( from ) )
			{
				from.CloseGump(typeof(SevenGMSkillBallGump));
				from.SendGump(new SevenGMSkillBallGump(this, from, GumpHeadline));
			}

        }


        public override bool IsValidSkillBallUse( Mobile from )
        {
            if ( from != m_NewPlayer )
            {
                from.SendMessage("You feel the skill ball lose energy and disintegrate.");
                this.Delete();
                return false;
            }
            return true;
        }

		public override bool IsRestrictedSkill(SkillName i)
		{
			// Skills that are n/a at the gump
			return i == SkillName.Blacksmith || i == SkillName.Peacemaking || i == SkillName.Provocation ||
				i == SkillName.Musicianship || i == SkillName.Tailoring || i == SkillName.AnimalTaming ||
				i == SkillName.Lockpicking || i == SkillName.RemoveTrap || i == SkillName.Mining ||
				i == SkillName.Necromancy || i == SkillName.Focus || i == SkillName.Chivalry ||
				i == SkillName.AnimalLore || i == SkillName.Cartography || i == SkillName.Carpentry ||
				i == SkillName.Fishing || i == SkillName.Tinkering || i == SkillName.Alchemy ||
				i == SkillName.ItemID || i == SkillName.Begging || i == SkillName.Fletching ||
				i == SkillName.Veterinary || i == SkillName.Poisoning;
		}
    }

    public class WelcomeBackSkillball : NewPlayerSkillBall
    {
		[Constructable]
        public WelcomeBackSkillball( Mobile newplayer ) : base( newplayer )
		{
			Name = "a welcome back skill ball";
            f_SkillValue = 100;
            GumpHeadline = "Welcome Back 7xGM Skillball - Choose your seven skills...";
        }

        public WelcomeBackSkillball( Serial serial ) : base( serial )
		{
		}

        public override void OnSingleClick(Mobile from)
        {
            if ( Owner != null )
                LabelTo(from, String.Format("personal welcome back skillball for {0}", Owner.Name));
            else
                base.OnSingleClick(from);
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize(writer);
            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

    }
}