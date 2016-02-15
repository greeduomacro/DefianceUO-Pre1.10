using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.ContextMenus;
using Server.Engines.Quests;

namespace Server.Engines.IdolSystem
{
    public enum QuestType
    {
        Shame,
        Deceit,
        Destard,
        Hythloth,
        Despise,
        Covetous,
        Wrong
    }

    public class IdolQuester : BaseQuester
    {
        private QuestType m_Type;

        [CommandProperty(AccessLevel.GameMaster)]
        public QuestType Type
        {
            get
            {
                return m_Type;
            }
        }

        public override bool IsActiveVendor { get { return false; } }
        public override bool DisallowAllMoves { get { return false; } }

        [Constructable]
        public IdolQuester(QuestType type)
            : base(null)
        {
            m_Type = type;
            Name = "Summoner for " + type;
            switch (type)
            {
                case QuestType.Shame: Hue = 0x58F; break;
                case QuestType.Deceit: Hue = 0x4E2; break;
                case QuestType.Destard: Hue = 0x455; break;
                case QuestType.Hythloth: Hue = 0x482; break;
                case QuestType.Despise: Hue = 0x7DA; break;
                case QuestType.Covetous: Hue = 0x4D3; break;
                case QuestType.Wrong: Hue = 0x655; break;
            }
        }

        public IdolQuester(Serial serial)
            : base(serial)
        {
        }

        private PentagramAddon m_Altar;

        private const int AltarRange = 24;

        public PentagramAddon Altar
        {
            get
            {
                if (m_Altar == null || m_Altar.Deleted || m_Altar.Map != this.Map || !Utility.InRange(m_Altar.Location, this.Location, AltarRange))
                {
                    foreach (Item item in GetItemsInRange(AltarRange))
                    {
                        if (item is PentagramAddon)
                        {
                            m_Altar = (PentagramAddon)item;
                            break;
                        }
                    }
                }

                return m_Altar;
            }
        }

        public override void InitOutfit()
        {

            EquipItem(SetHue(new Sandals(), Utility.RandomRedHue()));
            EquipItem(SetHue(new LongPants(), Utility.RandomRedHue()));
            EquipItem(SetHue(new Doublet(), Utility.RandomRedHue()));
            EquipItem(SetHue(new LongHair(), Utility.RandomRedHue()));
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            bool IsQuestItem = false;

            Item items = dropped;

            PlayerMobile player = from as PlayerMobile;

            if (player != null)
            {
                QuestSystem qs = player.Quest;

                if (qs is SummonQuest)
                {
                    FragmentCrystal item = (FragmentCrystal)dropped;

                    if (dropped is FragmentCrystal && item.type == QuestType.Shame && m_Type == QuestType.Shame)
                    {
                        IsQuestItem = true;
                    }
                    if (dropped is FragmentCrystal && item.type == QuestType.Deceit && m_Type == QuestType.Deceit)
                    {
                        IsQuestItem = true;
                    }
                    if (dropped is FragmentCrystal && item.type == QuestType.Destard && m_Type == QuestType.Destard)
                    {
                        IsQuestItem = true;
                    }
                    if (dropped is FragmentCrystal && item.type == QuestType.Hythloth && m_Type == QuestType.Hythloth)
                    {
                        IsQuestItem = true;
                    }
                    if (dropped is FragmentCrystal && item.type == QuestType.Despise && m_Type == QuestType.Despise)
                    {
                        IsQuestItem = true;
                    }
                    if (dropped is FragmentCrystal && item.type == QuestType.Covetous && m_Type == QuestType.Covetous)
                    {
                        IsQuestItem = true;
                    }
                    if (dropped is FragmentCrystal && item.type == QuestType.Wrong && m_Type == QuestType.Wrong)
                    {
                        IsQuestItem = true;
                    }

                    if (IsQuestItem)
                    {
                        QuestObjective obj = qs.FindObjective(typeof(CollectFragmentsObjective));

                        if (obj != null && !obj.Completed)
                        {
                            int need = obj.MaxProgress - obj.CurProgress;

                            if (items.Amount < need)
                            {
                                obj.CurProgress += items.Amount;
                                items.Delete();

                                qs.ShowQuestLogUpdated();
                            }
                            else if ( obj.CurProgress + items.Amount == 1000 && (
								(WrongBoss.Active == false && m_Type == QuestType.Wrong) ||
								(HythBoss.Active == false && m_Type == QuestType.Hythloth) ||
								(DecBoss.Active == false && m_Type == QuestType.Deceit) ||
								(CoveBoss.Active == false && m_Type == QuestType.Covetous) ||
								(DestBoss.Active == false && m_Type == QuestType.Destard) ||
								(ShameBoss.Active == false && m_Type == QuestType.Shame) ||
								(DespBossTwo.Active == false && DespBoss.Active == false && m_Type == QuestType.Despise) ) )
                            {
                                obj.Complete();
                                items.Consume(need);

                                if (!items.Deleted)
                                    SayTo(from, "You have already given me all the items nessassary for the summoning");
                            }
							else
							{
								this.SayTo(from, "The Keeper has already been summoned so I cannot accept the last of your crystals");
							}
                        }
                    }
                    else
                    {
                        SayTo(from, "That is not the correct type of crystal!");
                    }
                    return false;
                }
            }
            return base.OnDragDrop(from, dropped);
        }

        public override bool CanTalkTo(PlayerMobile to)
        {
            return (to.Quest == null && QuestSystem.CanOfferQuest(to, typeof(SummonQuest)));
        }

        public override void OnTalk(PlayerMobile player, bool contextMenu)
        {
            this.Direction = GetDirectionTo(player);

            QuestSystem qs = player.Quest;

            if (qs == null && QuestSystem.CanOfferQuest(player, typeof(SummonQuest)))
            {
                Direction = GetDirectionTo(player);
                new SummonQuest(this, player).SendOffer();
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write((int)m_Type);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        m_Type = (QuestType)reader.ReadInt();

                        break;
                    }
            }
        }
    }

    public class AcceptConversation : QuestConversation
    {
        public override object Message
        {
            get
            {
                return ("Go forth and collect 1000 Crystal Fragments from this Dungeons minions");
            }
        }

        public AcceptConversation()
        {
        }

        public override void OnRead()
        {
            System.AddObjective(new CollectFragmentsObjective());
        }
    }

    public class VanquishMinibossConversation : QuestConversation
    {
        private Mobile m_MiniBoss;

	public override object Message
        {
            get
            {
                return String.Format("Now that I have sufficient fragments I shall summon the Idol Keeper for you!");
            }
        }

        public VanquishMinibossConversation()
        {
        }

        public override void OnRead()
        {
            IdolQuester quester = ((SummonQuest)System).IdolQuester;
			QuestType type = quester.Type;

            if (quester == null)
            {
                System.From.SendMessage("Internal error: unable to find idolquester. Quest unable to continue.");
                System.Cancel();
            }
            else
            {
                PentagramAddon altar = quester.Altar;

                if (altar == null)
                {
                    System.From.SendMessage("Internal error: unable to find summoning altar. Quest unable to continue.");
                    System.Cancel();
                }
                else if (
                    (!WrongBoss.Active && type == QuestType.Wrong) ||
                    (!HythBoss.Active && type == QuestType.Hythloth) ||
                    (!DecBoss.Active && type == QuestType.Deceit) ||
                    (!CoveBoss.Active && type == QuestType.Covetous) ||
                    (!DestBoss.Active && type == QuestType.Destard) ||
                    (!ShameBoss.Active && type == QuestType.Shame) ||
                    (!DespBossTwo.Active && !DespBoss.Active && type == QuestType.Despise)
                    )
                {
                    switch (type)
                    {
                        case QuestType.Shame: m_MiniBoss = new ShameBoss(); break;
                        case QuestType.Deceit: m_MiniBoss = new DecBoss(); break;
                        case QuestType.Destard: m_MiniBoss = new DestBoss(); break;
                        case QuestType.Hythloth: m_MiniBoss = new HythBoss(); break;
                        case QuestType.Despise: m_MiniBoss = new DespBoss(); break;
                        case QuestType.Covetous: m_MiniBoss = new CoveBoss(); break;
                        case QuestType.Wrong: m_MiniBoss = new WrongBoss(); break;
                    }

                    m_MiniBoss.MoveToWorld(altar.Location, altar.Map);
                    System.AddObjective(new VanquishMinibossObjective());
                }
                else
                {
                    quester.SayTo(System.From, "The Keeper is already here!");

                    System.Cancel();
                }
            }
        }
    }

    public class CollectFragmentsObjective : QuestObjective
    {
        private PentagramAddon m_Altar;

        public override object Message
        {
            get
            {
                return ("Gather 1000 Crystal Fragments from this place");
            }
        }

        public override int MaxProgress { get { return 1000; } }

        public CollectFragmentsObjective()
        {
        }

        public override void OnComplete()
        {

            IdolQuester quester = ((SummonQuest)System).IdolQuester;

            if (quester == null)
            {
                System.From.SendMessage("Internal error: unable to find idolquester. Quest unable to continue.");
                System.Cancel();
            }
            else
            {
                PentagramAddon altar = quester.Altar;

                if (altar == null)
                {
                    System.From.SendMessage("Internal error: unable to find summoning altar. Quest unable to continue.");
                    System.Cancel();
                }
                else
                {
                    System.AddConversation(new VanquishMinibossConversation());
                }
            }
        }

        public override void RenderMessage(BaseQuestGump gump)
        {
            if (CurProgress > 0 && CurProgress < MaxProgress)
                gump.AddHtmlObject(70, 130, 300, 100, "The fragments are accepted, but more are required!", BaseQuestGump.Blue, false, false);
            else
                base.RenderMessage(gump);
        }

        public override void RenderProgress(BaseQuestGump gump)
        {
            if (CurProgress > 0 && CurProgress < MaxProgress)
            {
                gump.AddHtmlObject(70, 260, 270, 100, "Number of crystal fragments collected", BaseQuestGump.Blue, false, false);

                gump.AddLabel(70, 280, 100, CurProgress.ToString());
                gump.AddLabel(100, 280, 100, "/");
                gump.AddLabel(130, 280, 100, MaxProgress.ToString());
            }
            else
            {
                base.RenderProgress(gump);
            }
        }

    }
    public class VanquishMinibossObjective : QuestObjective
    {
		private QuestType m_Type;

        public override object Message
        {
            get
            {
                return ("Now go slay the Idol Keeper!");
            }
        }

        public VanquishMinibossObjective()
        {
        }

        public override void CheckProgress()
        {
			IdolQuester quester = ((SummonQuest)System).IdolQuester;

            if (
                    (WrongBoss.Active == false && quester.Type == QuestType.Wrong) ||
                    (HythBoss.Active == false && quester.Type == QuestType.Hythloth) ||
                    (DecBoss.Active == false && quester.Type == QuestType.Deceit) ||
                    (CoveBoss.Active == false && quester.Type == QuestType.Covetous) ||
                    (DestBoss.Active == false && quester.Type == QuestType.Destard) ||
                    (ShameBoss.Active == false && quester.Type == QuestType.Shame) ||
                    (DespBossTwo.Active == false && DespBoss.Active == false && quester.Type == QuestType.Despise)
               )
                Complete();
        }

        public override void OnComplete()
        {
			IdolQuester quester = ((SummonQuest)System).IdolQuester;
            quester.SayTo(System.From, "Well done, the Keeper has been slain!");
            System.From.SendMessage("Check the Keepers corpse to see if your lucky enough to recieve an idol");
            System.Complete();
        }


        public override void ChildDeserialize(GenericReader reader)
        {
            int version = reader.ReadEncodedInt();

			m_Type = (QuestType)reader.ReadInt();
        }

        public override void ChildSerialize(GenericWriter writer)
        {
            writer.WriteEncodedInt((int)0); // version

			writer.Write((int)m_Type);
        }
    }

    public class SummonQuest : QuestSystem
    {
        private PentagramAddon m_Altar;
        private QuestType m_Type;

        private static Type[] m_TypeReferenceTable = new Type[]
			{
				typeof( IdolSystem.AcceptConversation ),
				typeof( IdolSystem.CollectFragmentsObjective ),
				typeof( IdolSystem.VanquishMinibossConversation ),
				typeof( IdolSystem.VanquishMinibossObjective )
			};

        public override Type[] TypeReferenceTable { get { return m_TypeReferenceTable; } }

        private IdolQuester m_IdolQuester;

        public IdolQuester IdolQuester
        {
            get { return m_IdolQuester; }
        }

        public override object Name
        {
            get
            {
                return ("The Summoning of the Keepers");
            }
        }

        public override object OfferMessage
        {
            get
            {
                return ("If you wish to aquire the Idol of this dungeon then you must bring to me 1000 of its Crystal Fragments. Only then i can summon the Keeper!");
            }
        }

        public override bool IsTutorial { get { return false; } }
        public override TimeSpan RestartDelay { get { return TimeSpan.Zero; } }
        public override int Picture { get { return 0x15B5; } }

        public SummonQuest(IdolQuester quester, PlayerMobile from)
            : base(from)
        {
            m_IdolQuester = quester;
        }

        public SummonQuest()
        {
        }

        private Item m_Frag;

        public override void Cancel()
        {
            base.Cancel();

            QuestObjective obj = FindObjective(typeof(CollectFragmentsObjective));
            QuestObjective objtwo = FindObjective(typeof(VanquishMinibossObjective));

            if (obj != null && obj.CurProgress > 0 && objtwo == null && !obj.Completed)
            {
                BankBox box = From.BankBox;

                if (box != null)
                {
                    IdolQuester quester = m_IdolQuester;
                    QuestType type = quester.Type;
                    switch (type)
                    {
                        case QuestType.Shame: m_Frag = new FragmentCrystal(QuestType.Shame, obj.CurProgress); break;
                        case QuestType.Deceit: m_Frag = new FragmentCrystal(QuestType.Deceit, obj.CurProgress); break;
                        case QuestType.Destard: m_Frag = new FragmentCrystal(QuestType.Destard, obj.CurProgress); break;
                        case QuestType.Hythloth: m_Frag = new FragmentCrystal(QuestType.Hythloth, obj.CurProgress); break;
                        case QuestType.Despise: m_Frag = new FragmentCrystal(QuestType.Despise, obj.CurProgress); break;
                        case QuestType.Covetous: m_Frag = new FragmentCrystal(QuestType.Covetous, obj.CurProgress); break;
                        case QuestType.Wrong: m_Frag = new FragmentCrystal(QuestType.Wrong, obj.CurProgress); break;
                    }
                    box.DropItem(m_Frag);
                    From.SendMessage("The dungeon fragments you have offered up have been returned to you.");
                }
            }
        }

        public override void Accept()
        {
            base.Accept();

            AddConversation(new AcceptConversation());
        }

        public override void ChildDeserialize(GenericReader reader)
        {
            int version = reader.ReadEncodedInt();

            m_IdolQuester = reader.ReadMobile() as IdolQuester;
        }

        public override void ChildSerialize(GenericWriter writer)
        {
            writer.WriteEncodedInt((int)0); // version

            writer.Write((Mobile)m_IdolQuester);
        }
    }
}