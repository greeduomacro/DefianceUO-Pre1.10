/**
*	Ladder system by Morten Legarth (c)taunted.dk ( legarth@taunted.dk )
*	Version: v0.10 -  26-02-2005
*
*	This system has been written for use at the Blitzkrieg frees-shard
*	http://blitzkrieg.dorijan.de . Unauthorized reproduction or redistribution
*	is prohibited.
*
*							DuelObject.cs
*						-------------------------
*
*	File Description:	This core-file represents an ongoing
*						duel. It handles most of the duel
*						mechanics.
*
*/

using System;
using System.Collections;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Fifth;
using Server.Spells.Eighth;
using Server.Regions;
using Server.Factions;

namespace Server.Ladder
{
    public class DuelObject
    {
        //temp until custom system
        private int m_DuelType;
        private bool m_Potions;
        private bool m_Summoning;
        private bool m_Looting;
		private bool m_PoisonWeaps;
		private bool m_MagicWeaps;
		private bool m_Magery;
		private int m_Wager;
		private int m_Dex;


		private Mobile m_player1;
        private Mobile m_player2;
        private DateTime m_start;
        private DateTime m_end;
        private DuelTimer m_DuelTimer;
        private CountTimer m_CountTimer;
        private int m_winner;
        private ArenaControl m_Arena;
        private DateTime m_DuelTimeoutTime;


        public enum BanSkills
        {
            Anatomy = 0,
            Healing,
            Inscription,
            Poisoning,
        }

        public Mobile Player1
        {
            get { return m_player1; }
        }

        public Mobile Player2
        {
            get { return m_player2; }
        }
        public DateTime Start
        {
            get { return m_start; }
        }
        public DuelTimer Timer
        {
            get { return m_DuelTimer; }
        }

        public int DuelType
        {
            get { return m_DuelType; }
        }
        public bool Potions
        {
            get { return m_Potions; }
        }
        public bool Summoning
        {
            get { return m_Summoning; }
        }
        public bool Looting
        {
            get { return m_Looting; }
        }
		public bool PoisonedWeapons
		{
			get { return m_PoisonWeaps; }
		}
		public bool MagicWeapons
		{
			get { return m_MagicWeaps; }
		}
		public bool Magery
		{
			get { return m_Magery; }
		}
		public int MaxDex
		{
			get { return m_Dex; }
		}
		public int Wager
        {
            get { return m_Wager; }
        }

        // Constructor for deserilization
        public DuelObject()
        {
            // perhaps some kind of initialization
        }


		public DuelObject(Mobile challenger, Mobile challengee, int duelType, bool potions, bool summoning, bool looting, bool poisonWeaps, bool magicWeaps, bool magery, int wager, int dex)
		{
            this.m_DuelType = duelType;
            this.m_Potions = potions;
            this.m_Summoning = summoning;
            this.m_Looting = looting;
			this.m_PoisonWeaps = poisonWeaps;
			this.m_MagicWeaps = magicWeaps;
			this.m_Magery = magery;
			this.m_Wager = wager;
			this.m_Dex = dex;


			m_player1 = challenger;
            m_player2 = challengee;
            m_player1.SendGump(new ChallengeGump(this));
        }

        public void Begin()
        {
            if (Ladder.Duellers.Contains(m_player1))
            {
                m_player1.SendMessage("Duel has been canceled because you are already involved in a duel.");
                m_player2.SendMessage("Duel has been canceled because your opponent is already involved in a duel.");
            }
            else if (Ladder.Duellers.Contains(m_player2))
            {
                m_player2.SendMessage("Duel has been canceled because you are already involved in a duel.");
                m_player1.SendMessage("Duel has been canceled because your opponent is already involved in a duel.");
            }
//			else if (m_player1.NetState != null && m_player2.NetState != null && m_player1.NetState.Address.Equals(m_player2.NetState.Address))
//			{
//				m_player2.SendMessage("Duel has been canceled because multiclienting is not allowed.");
//				m_player1.SendMessage("Duel has been canceled because multiclienting is not allowed.");
//			}
			else if (m_player1.Region != m_player2.Region || !(m_player1.Region is CustomRegion) || !(((CustomRegion)m_player1.Region).Controller is LadderAreaControl))
            {
                m_player2.SendMessage("Duel has been canceled because you weren't both in a Ladder Area");
                m_player1.SendMessage("Duel has been canceled because you weren't both in a Ladder Area");

            }
            else if ((m_Arena = ((LadderAreaControl)((CustomRegion)m_player1.Region).Controller).GetArena()) == null)
            {
                // no arena
                m_player2.SendMessage("Duel has been canceled because there were no free arenas. If this problem persists, page a GM to make some more arenas.");
                m_player1.SendMessage("Duel has been canceled because there were no free arenas. If this problem persists, page a GM to make some more arenas.");

            }
            else if (m_player1.Spell != null || m_player2.Spell != null)
            {
                m_player2.SendMessage("Duel has been canceled because precasting spells is not allowed.");
                m_player1.SendMessage("Duel has been canceled because precasting spells is not allowed.");
            }
			else if (m_player1.RawStr != m_player1.Str || m_player1.RawDex != m_player1.Dex || m_player1.RawInt != m_player1.Int)
			{
				m_player2.SendMessage("Duel has been canceled because your opponent was pre-duel buffed, which is not allowed(Potion, spell, etc. buffs)");
				m_player1.SendMessage("Duel has been canceled because your were pre-duel buffed, which is not allowed(Potion, spell, etc. buffs)");
			}
			else if (m_player2.RawStr != m_player2.Str || m_player2.RawDex != m_player2.Dex || m_player2.RawInt != m_player2.Int)
			{
				m_player1.SendMessage("Duel has been canceled because your opponent was pre-duel buffed, which is not allowed(Potion, spell, etc. buffs)");
				m_player2.SendMessage("Duel has been canceled because your were pre-duel buffed, which is not allowed(Potion, spell, etc. buffs)");
			}
			else if (m_player1.Mana != m_player1.Int)
			{
				m_player2.SendMessage("Duel has been canceled because your opponent was not full in spirit. He needs to meditate.");
				m_player1.SendMessage("Duel has been canceled because your your spirit is not full. Meditate to full spirit before duelling.");
			}
			else if (m_player2.Mana != m_player2.Int)
			{
				m_player1.SendMessage("Duel has been canceled because your opponent was not full in spirit. He needs to meditate.");
				m_player2.SendMessage("Duel has been canceled because your your spirit is not full. Meditate to full spirit before duelling.");
			}
			else
            {

                BitArray m_RestrictedSpells = m_Arena.RestrictedSpells;
                BitArray m_RestrictedSkills = m_Arena.RestrictedSkills;
                // Set up arena
                m_Arena.InUse = true;

                // These flags are default
                m_Arena.PlayerLogoutDelay = TimeSpan.FromDays(1);
                m_Arena.AllowHousing = false;
                m_Arena.AllowSpawn = false;
                m_Arena.CanMountEthereal = false;
                m_Arena.CannotEnter = false;
                m_Arena.CanUseStuckMenu = false;
                m_Arena.ItemDecay = true;
                m_Arena.ShowEnterMessage = false;
                m_Arena.ShowExitMessage = false;
                m_Arena.CannotLootOwnCorpse = false;
                m_Arena.IsGuarded = false;

                // Ban everything until countdown is finished
                for (int i = 0; i < m_RestrictedSpells.Length; i++)
                {
                    m_RestrictedSpells[i] = true; ;
                }

                for (int i = 0; i < m_RestrictedSkills.Length; i++)
                {
                    m_RestrictedSkills[i] = true;
                }



                // Set arena flags to false until count
                m_Arena.AllowBenifitPlayer = false;
                m_Arena.AllowHarmPlayer = false;
                m_Arena.CanBeDamaged = false;
                //m_Arena.CanHeal = false;
                m_Arena.CanRessurect = false;
                m_Arena.AllowBenifitNPC = false;
                m_Arena.AllowHarmNPC = false;
                m_Arena.CanLootPlayerCorpse = false;
                m_Arena.CanLootNPCCorpse = false;
                m_Arena.CanUsePotions = false;


                // Save out location
                m_Arena.OutMap = m_player1.Map;
                m_Arena.OutLoc = m_player1.Location;


                m_start = DateTime.Now;
                Ladder.Duellers.Add(m_player1);
                Ladder.Duellers.Add(m_player2);
                Ladder.Duels.Add(this);

                // Disarm

                ArrayList equipitems = new ArrayList(m_player1.Items);
                foreach (Item item in equipitems)
                {
					if (item is BaseWeapon && !Ladder.WeapAllowed((PlayerMobile)m_player1, (BaseWeapon)item) && m_player1.Backpack != null)
					{
						m_player1.SendMessage("The weapon you are wielding is now allowed in this duel. It has been put into your backpack.");
						m_player1.Backpack.DropItem(item);
                    }
                }

                equipitems = new ArrayList(m_player2.Items);
                foreach (Item item in equipitems)
                {
					if (item is BaseWeapon && !Ladder.WeapAllowed((PlayerMobile)m_player2,(BaseWeapon)item) && m_player2.Backpack != null)
					{
						m_player2.SendMessage("The weapon you are wielding is now allowed in this duel. It has been put into your backpack.");
						m_player2.Backpack.DropItem(item);
                    }
                }

                //Dismount
                if (m_player1.Mounted)
                {
                    IMount mount = (IMount)m_player1.Mount;
                    if (mount != null)
                        mount.Rider = null;
                }
                if (m_player2.Mounted)
                {
                    IMount mount = (IMount)m_player2.Mount;
                    if (mount != null)
                        mount.Rider = null;
                }

                //Remove reflect/RA
                m_player1.MagicDamageAbsorb = 0;
                m_player2.MagicDamageAbsorb = 0;

                m_player1.MeleeDamageAbsorb = 0;
                m_player2.MeleeDamageAbsorb = 0;



				//Remove bandages/petals/petsum
                if (m_DuelType == 1)
                {
                    if (m_player1.Backpack != null && m_player1.BankBox != null)
                    {
                        Item[] bandages = m_player1.Backpack.FindItemsByType(typeof(Bandage), true);

                        for (int i = 0; i < bandages.Length; i++)
                        {
                            m_player1.BankBox.DropItem(bandages[i]);
                        }
                        Item[] petals = m_player1.Backpack.FindItemsByType(typeof(OrangePetals), true);
                        for (int i = 0; i < petals.Length; i++)
                        {
                            m_player1.BankBox.DropItem(petals[i]);
                        }

                        Item[] petBalls = m_player1.Backpack.FindItemsByType(typeof(PetSummonBall), true);
                        for (int i = 0; i < petBalls.Length; i++)
                        {
                            m_player1.BankBox.DropItem(petBalls[i]);
                        }
                    }

                    if (m_player2.Backpack != null && m_player2.BankBox != null)
                    {
                        Item[] bandages = m_player2.Backpack.FindItemsByType(typeof(Bandage), true);

                        for (int i = 0; i < bandages.Length; i++)
                        {
                            m_player2.BankBox.DropItem(bandages[i]);
                        }
                        Item[] petals = m_player2.Backpack.FindItemsByType(typeof(OrangePetals), true);
                        for (int i = 0; i < petals.Length; i++)
                        {
                            m_player2.BankBox.DropItem(petals[i]);
                        }

                        Item[] petBalls = m_player2.Backpack.FindItemsByType(typeof(PetSummonBall), true);
                        for (int i = 0; i < petBalls.Length; i++)
                        {
                            m_player2.BankBox.DropItem(petBalls[i]);
                        }
                    }
                }


                // Teleport and freeze players
                m_player1.Map = m_Arena.Map;
                m_player2.Map = m_Arena.Map;
                m_player1.Location = m_Arena.StartLoc1;
                m_player2.Location = m_Arena.StartLoc2;

                m_player1.Frozen = true;
                m_player2.Frozen = true;

				//m_player1.Hits = MaxHits;
				m_player1.Heal(100);
				m_player1.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist);
				m_player1.PlaySound(0x1F2);

				//m_player2.Hits = MaxHits;
				m_player2.Heal(100);
				m_player2.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist);
				m_player2.PlaySound(0x1F2);

				m_player1.CloseAllGumps();
                m_player2.CloseAllGumps();
                m_player1.SendMessage("Challenge accepted, the duel will begin in 5 seconds!!");
                m_player2.SendMessage("Challenge accepted, the duel will begin in 5 seconds!!");

                m_CountTimer = new CountTimer(this, 5);
                m_CountTimer.Start();
            }

        }
/*
		private bool WeapAllowed(BaseWeapon weap)
		{
			bool poisoned = weap.Poison != null && weap.PoisonCharges > 0;
			bool magic = weap.AccuracyLevel != WeaponAccuracyLevel.Regular || weap.DamageLevel != WeaponDamageLevel.Regular || weap.DurabilityLevel != WeaponDurabilityLevel.Regular;
			bool typeBanned = m_DuelType == 1 && !(weap is Dagger);
			bool factionItem = weap is IFactionItem && ((IFactionItem)weap).FactionItemState != null;
			bool tribal = weap is TribalSpear;

			return !poisoned && !magic && !typeBanned && !factionItem && !tribal;
		}

*/
        public void StartDuel()
        {

            BitArray m_RestrictedSpells = m_Arena.RestrictedSpells;
            BitArray m_RestrictedSkills = m_Arena.RestrictedSkills;

            // Set permissions
            for (int i = 0; i < m_RestrictedSpells.Length; i++)
            {						// Teleport		Recall		Mark	GateTravel					Summonings
                m_RestrictedSpells[i] = i == 21 || i == 31 || i == 44 || i == 51 || (!m_Summoning && i == 39 || i == 59 || i == 60 || i == 61 || i == 62 || i == 63) || !m_Magery;
            }

            for (int i = 0; i < m_RestrictedSkills.Length; i++)
            {
                if (m_DuelType == 1)
                {
                    m_RestrictedSkills[i] = i != (int)SkillName.Magery && i != (int)SkillName.Meditation && i != (int)SkillName.Wrestling &&
                        i != (int)SkillName.EvalInt && i != (int)SkillName.MagicResist;
                }
                else //if (m_DuelType == 2)
                {
                    m_RestrictedSkills[i] = false;
                }
            }

            // Set arena flags
            m_Arena.AllowBenifitPlayer = true;
            m_Arena.AllowHarmPlayer = true;
            m_Arena.CanBeDamaged = true;
            m_Arena.CanHeal = true;
            m_Arena.CanRessurect = true;
            m_Arena.AllowBenifitNPC = m_Summoning;
            m_Arena.AllowHarmNPC = m_Summoning;
            m_Arena.CanLootPlayerCorpse = m_Looting;
            m_Arena.CanLootNPCCorpse = m_Looting;
            m_Arena.CanUsePotions = m_Potions;

            m_player1.Frozen = false;
            m_player2.Frozen = false;

            m_DuelTimer = new DuelTimer(this, 15);
            m_DuelTimer.Start();
        }

        public void Finished(int win, DateTime end)
        {
            if (m_player1 == null || m_player2 == null)
                return;

            m_winner = win;
            m_end = end;
            if (m_winner > 0)
            {

                m_player1.SendMessage("The fight is over, you " + (m_winner == 1 ? "won " : "lost ") + "the duel. " +
                    "Registering the results in the database, hold on...");
                m_player2.SendMessage("The fight is over, you " + (m_winner == 2 ? "won " : "lost ") + "the duel. " +
                    "Registering the results in the database, hold on...");

                double difMod = (((PlayerMobile)m_player1).Honor - ((PlayerMobile)m_player2).Honor);//* (-1);

                int difficultyP1 = (int)(1000 / (1 + Math.Pow(10.0, (difMod / 400))));
                int difficultyP2 = 1000 - difficultyP1;

                int K1 = ((PlayerMobile)m_player1).Honor < 1000 ? 150 : 50;
                int K2 = ((PlayerMobile)m_player2).Honor < 1000 ? 150 : 50;

                int gain = m_winner == 1 ? ((difficultyP1 * K1) / 1000) : ((difficultyP2 * K2) / 1000);
                int loss = m_winner == 2 ? (((1000 - difficultyP1) * K1) / 1000) : (((1000 - difficultyP2) * K2) / 1000);


                // Update fast access variables
                if (m_winner == 1)
                {
                    ((PlayerMobile)m_player1).Honor += gain;
                    ((PlayerMobile)m_player2).Honor -= loss;

                    ((PlayerMobile)m_player1).HonorChange += gain;
                    ((PlayerMobile)m_player2).HonorChange -= loss;

                    ((PlayerMobile)m_player1).Wins++;
                    ((PlayerMobile)m_player2).Losses++;

                }
                else
                {
                    ((PlayerMobile)m_player2).Honor += gain;
                    ((PlayerMobile)m_player1).Honor -= loss;

                    ((PlayerMobile)m_player2).HonorChange += gain;
                    ((PlayerMobile)m_player1).HonorChange -= loss;

                    ((PlayerMobile)m_player2).Wins++;
                    ((PlayerMobile)m_player1).Losses++;
                }
                // Adjust honorchange
                Fight f;
                for (int i = Ladder.IntervalIndex; Ladder.Fights.Count > i && ((Fight)Ladder.Fights[i]).Start < DateTime.Now - Ladder.Interval; i++)
                {
                    f = (Fight)Ladder.Fights[i];
                    if (f.Winner != null)
                        ((PlayerMobile)f.Winner).HonorChange -= f.Gain;
                    if (f.Loser != null)
                        ((PlayerMobile)f.Loser).HonorChange += f.Loss;
                    Ladder.IntervalIndex = i;
                }

                //Update fight list
                Ladder.Fights.Add(new Fight((m_winner == 1 ? m_player1 : m_player2), (m_winner == 2 ? m_player1 : m_player2),
                                    m_start, m_end, gain, loss, (m_winner == 1 ? difficultyP1 : difficultyP2)));

                Ladder.Players.Sort();

                //Sashes
                for (int i = 0; i < Ladder.Players.Count; i++)
                {
                    PlayerMobile m = (PlayerMobile)Ladder.Players[i];
                    Item item = m.FindItemOnLayer(Layer.Earrings);
                    if (item != null && item is LadderSash)
                    {
                        // Sash exists. Configure it since it might have changed
                        // if player has too low rank for sash, the configure method
                        // will automatically delete the sash.
                        ((LadderSash)item).Configure(i + 1, m.Honor);
                    }
                    else if (i + 1 <= 3)
                    {
                        Item old = m.FindItemOnLayer(Layer.Earrings);
                        if (old != null)
                            m.PlaceInBackpack(old);
                        m.EquipItem(new LadderSash(i + 1, m.Honor));
                    }

                }

                if (m_player1.BankBox != null && m_player2.BankBox != null && m_Wager > 0)
                {
                    Gold gold = new Gold();
                    if (m_winner == 1)
                    {
                        if (m_player2.BankBox.ConsumeTotal(typeof(Gold), m_Wager) && m_player1.AddToBackpack(new BankCheck(m_Wager)))
                        {
                            m_player2.SendMessage("You lost {0} gold in the fight.", m_Wager);
                            m_player1.SendMessage("You won {0} gold in the fight.", m_Wager);
                        }
                        else
                        {
                            m_player2.SendMessage("There was a problem with the gold transaction, please contact a GM");
                            m_player1.SendMessage("There was a problem with the gold transaction, please contact a GM");
                        }
                    }
                    else
                    {
                        if (m_player1.BankBox.ConsumeTotal(typeof(Gold), m_Wager) && m_player2.AddToBackpack(new BankCheck(m_Wager)))
                        {
                            m_player1.SendMessage("You lost {0} gold in the fight.", m_Wager);
                            m_player2.SendMessage("You won {0} gold in the fight.", m_Wager);
                        }
                        else
                        {
                            m_player1.SendMessage("There was a problem with the gold transaction, please contact a GM");
                            m_player2.SendMessage("There was a problem with the gold transaction, please contact a GM");
                        }
                    }

                }

            }
            else if (m_winner == -1)
            {

                m_player1.SendMessage("The duel timed out");
                m_player2.SendMessage("The duel timed out");
                //this.Delete();
            }
            else if (m_winner == -2)
            {
                if (m_CountTimer != null)
                    m_CountTimer.Stop();

                m_player1.SendMessage("Someone died before the duel started. Bailing out.");
                m_player2.SendMessage("Someone died before the duel started. Bailing out.");

                m_player1.Frozen = false;
                m_player2.Frozen = false;
            }
            //Cleanup

			m_Arena.CanBeDamaged = false;

			m_player1.MagicDamageAbsorb = 1;
            m_player2.MagicDamageAbsorb = 1;

            m_player1.SendMessage("The duel is now over. This arena will be purged in 1 minute. All items in the arena will be deleted and all mobiles will be teleported out.");
            m_player2.SendMessage("The duel is now over. This arena will be purged in 1 minute. All items in the arena will be deleted and all mobiles will be teleported out.");

            if (m_Arena != null)
                m_Arena.CleanUp();

            Ladder.Duellers.Remove(m_player1);
            Ladder.Duellers.Remove(m_player2);
            Ladder.Duels.Remove(this);


        }

        public void Wall(int count)
        {
            m_player1.SendMessage("Countdown: {0}", count == 0 ? "GO!!" : count.ToString());
            m_player2.SendMessage("Countdown: {0}", count == 0 ? "GO!!" : count.ToString());

        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write((int)0);
            writer.Write(m_DuelType);
            writer.Write(m_Potions);
            writer.Write(m_Summoning);
            writer.Write(m_Looting);
            writer.Write(m_player1);
            writer.Write(m_player2);
            writer.Write(m_start);
            writer.Write(m_end);
            writer.Write(m_winner);
            writer.Write(m_Arena);

            if (m_DuelTimer == null)
                writer.Write((int)0);
            else
                writer.Write(m_DuelTimer.Ticks);

            if (m_CountTimer == null)
                writer.Write((int)0);
            else
                writer.Write(m_CountTimer.Ticks);
        }
        public void Deserialize(GenericReader reader)
        {
            int version = reader.ReadInt();
            m_DuelType = reader.ReadInt();
            m_Potions = reader.ReadBool();
            m_Summoning = reader.ReadBool();
            m_Looting = reader.ReadBool();
            m_player1 = reader.ReadMobile();
            m_player2 = reader.ReadMobile();
            m_start = reader.ReadDateTime();
            m_end = reader.ReadDateTime();
            m_winner = reader.ReadInt();
            m_Arena = (ArenaControl)reader.ReadItem();

            int duelTicks = reader.ReadInt();
            if (duelTicks > 0)
            {
                m_DuelTimer = new DuelTimer(this, duelTicks);
                m_DuelTimer.Start();
            }

            int countTicks = reader.ReadInt();
            if (countTicks > 0)
            {
                m_CountTimer = new CountTimer(this, countTicks);
                m_CountTimer.Start();
            }
        }
    }

    public class CountTimer : Timer
    {
        private DuelObject duel;
        private int count;

        public int Ticks
        {
            get { return count; }
            set { count = value; }
        }


        public CountTimer(DuelObject duel, int ticks) : base(TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ))
        {
            this.count = ticks;
            this.duel = duel;

            duel.Wall(count);
        }

        protected override void OnTick()
        {
            count--;
            duel.Wall(count);
            if (count <= 0)
            {
                this.Stop();
                duel.StartDuel();
            }
        }
    }

    public class DuelTimer : Timer
    {
        private DuelObject duel;
        private int TimeOut; //= 15; //minutes.

        public int Ticks
        {
            get { return TimeOut; }
            set { TimeOut = value; }
        }


        public DuelTimer(DuelObject duel, int ticks) : base(TimeSpan.FromMinutes( 1.0 ), TimeSpan.FromMinutes( 1.0 ))
        {
            this.TimeOut = ticks;
            this.duel = duel;
        }

        protected override void OnTick()
        {
            TimeOut--;
            if (TimeOut <= 0)
            {
                this.Stop();
                // Time ran out
                Ladder.Duellers.Remove(duel.Player1);
                Ladder.Duellers.Remove(duel.Player2);
                duel.Finished(-1, DateTime.Now);
            }
            else
            {
                // Duel is not finished
                duel.Player1.SendMessage("Duel has {0} minute(s) remaining.", TimeOut);
                duel.Player2.SendMessage("Duel has {0} minute(s) remaining.", TimeOut);
            }
        }
    }
}