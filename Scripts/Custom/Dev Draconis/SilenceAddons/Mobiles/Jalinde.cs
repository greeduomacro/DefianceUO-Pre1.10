using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.ContextMenus;
using Server.Engines.SilenceAddon;
using Server.Engines.Quests;

namespace Server.Engines.SilenceAddon
{
	public class Jalinde : BaseQuester
	{
		public override bool IsActiveVendor{ get{ return false; } }
		public override bool DisallowAllMoves{ get{ return false; } }

		[Constructable]
		public Jalinde() : base( null )
		{
			Hue = 22222;
			Body = 970;
			Name = "Ghost of Jalinde Summerdrake";
		}

		public Jalinde( Serial serial ) : base( serial )
		{
		}

		public override void InitOutfit()
		{
			EquipItem( SetHue( new HoodedShroudOfShadows(), 22222 ) );
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			PlayerMobile player = from as PlayerMobile;
			if ( dropped is CellKey )
			{
				this.Say("That looks like the type of key used on the prison doors around here!  Go and find Tear's prison and release her!");
				return false;
			}

			if ( player != null )
			{
				QuestSystem qs = player.Quest;

				if ( qs is GhostQuest )
				{
					if ( dropped is JalindeLetter )
					{
						JalindeLetter letter = (JalindeLetter)dropped;

						QuestObjective obj = qs.FindObjective( typeof( WhatHappenedToTearObjective ) );

						letter.Delete();

						SayTo(from, "A letter from Tear? Thank you kind mortal.");

						if (obj!=null) obj.Complete();
					}

					if ( dropped is SoulCrystal )
					{
						SoulCrystal crystal = (SoulCrystal)dropped;

						QuestObjective obj = qs.FindObjective( typeof( TheGatheringOfSoulCrystalsObjective ) );

						if ( obj != null && !obj.Completed )
						{
							int need = obj.MaxProgress - obj.CurProgress;

							if ( crystal.Amount < need )
							{
								obj.CurProgress += crystal.Amount;
								crystal.Delete();

								qs.ShowQuestLogUpdated();
							}
							else
							{
								obj.Complete();
								crystal.Consume( need );

								if ( !crystal.Deleted )
								{
									SayTo(from, "I already have enough soul crystals to perform the summoning.");
								}
							}
						}
						else
						{
							SayTo(from, "That is not something I can use.");
						}

						return false;
					}
				}
			}

			return base.OnDragDrop( from, dropped );
		}

		public override bool CanTalkTo( PlayerMobile to )
		{
			return ( to.Quest == null && QuestSystem.CanOfferQuest( to, typeof( GhostQuest ) ) );
		}

		public override void OnTalk( PlayerMobile player, bool contextMenu )
		{
			QuestSystem qs = player.Quest;

			if ( qs == null && QuestSystem.CanOfferQuest( player, typeof( GhostQuest ) ) )
			{
				Direction = GetDirectionTo( player );
				new GhostQuest( this, player ).SendOffer();
			}
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
	}

	public class AcceptConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				return ("Jalinde has a long story to tell, but she seems to upset to talk. Maybe if you could give her something that would explain what happened to her sister?");
			}
		}

		public AcceptConversation()
		{
		}

		public override void OnRead()
		{
			System.AddObjective( new WhatHappenedToTearObjective() );
		}
	}

	public class TheGatheringOfSoulCrystalsConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				return ("So, you say this letter was infact given to you by the Ghost? Then why tell me it was from Tear?...oh nevermind...I can see you were just trying to help, so thank you for that.  But this letter....I guess for you to understand I must finish my story...where was I? oh yes...Well after I left my sister, which as it turned out was the last time i saw her, I went to try open that secret door or this secret door I should really say. Anyway, after serveral hours i discovered the lever needed to open the door and I went inside.  What I found was this place, a place of tormented spirits that I soon learned were imprisoned here by that damned Ghost.  Before I could flee to warn my sistor the Ghost appear and locked the door.  Then barely seconds later it had killed me and trapped my soul here for his amusement.  That was....oh many years ago now....I thought my sistor would have tryed to find me or she failed to open the door and left...But as it transpired that vile Ghost turned her into a skeleton and trapped her immortal soul in the very bottom of this dungeon, a level we...I never got to discover...Now you must help me...if not for my sake but for your own!  You have discovered his dark secret and so you could be killed and trapped here too.  Now in my many years trapped away here I have learned how to bring tho ghost to the next room, and whats more when I do so he will be vulnerable!  Now I need five soul crystals, but I be damned if I know where you can get them.....");
			}
		}

		public TheGatheringOfSoulCrystalsConversation()
		{
		}

		public override void OnRead()
		{
			System.AddObjective( new TheGatheringOfSoulCrystalsObjective() );
		}
	}

	public class VanquishGhostConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				return ("With these soul crystals I will now summon that vile Ghost.  Please take care for he is powerful and I do not wish to see you trapped here with me.  Now go and the best of luck to you!");
			}
		}

		public VanquishGhostConversation()
		{
		}

		public override void OnRead()
		{
			Jalinde jalinde = ((GhostQuest)System).Jalinde;

			if ( jalinde == null )
			{
				System.From.SendMessage( "Internal error: Unable to find Jalinde. Quest unable to continue." );
				System.Cancel();
			}
			else
			{
				if ( GhostPast.Active == false )
				{
					jalinde.Say("Oulm Quelst Tu Nock Bal Ne Setty!");
					Point3D p = new Point3D( 776, 1480, -28 );

					GhostPast ghost = new GhostPast();
					ghost.Hidden = true;
					ghost.Freeze(TimeSpan.FromSeconds(1.5));
					ghost.MoveToWorld( p, jalinde.Map );
					System.From.AddToBackpack( new ItemClaimer() );

					System.AddObjective( new VanquishGhostObjective( ghost ) );
				}
				else
				{
					jalinde.SayTo( System.From, "The Ghost is already here! Go brave mortal, go and destroy him!" );

					((GhostQuest)System).WaitForSummon = true;
				}
			}
		}
	}

	public class GhostQuest : QuestSystem
	{
		private static Type[] m_TypeReferenceTable = new Type[]
			{
				typeof( SilenceAddon.AcceptConversation ),
				typeof( SilenceAddon.WhatHappenedToTearObjective ),
				typeof( SilenceAddon.TheGatheringOfSoulCrystalsObjective ),
				typeof( SilenceAddon.VanquishGhostConversation ),
				typeof( SilenceAddon.VanquishGhostObjective )
			};

		public override Type[] TypeReferenceTable{ get{ return m_TypeReferenceTable; } }

		private Jalinde m_Jalinde;
		private bool m_WaitForSummon;

		public Jalinde Jalinde
		{
			get{ return m_Jalinde; }
		}

		public bool WaitForSummon
		{
			get{ return m_WaitForSummon; }
			set{ m_WaitForSummon = value; }
		}

		public override object Name
		{
			get
			{
				return ("A tale of two Ghosts");
			}
		}

		public override object OfferMessage
		{
			get
			{
				return ("Long ago my sistor and myself discovered a large mystical dungeon, and near the enterance to this dungeon we found a Ghost.  The ghost would not tell us his name but did tell us all about this place, about its history and its secrets.  After many months of exploring Tear discovered a secret locked door that the Ghost did not reveal to us, so after my sistor showed me this door I insisted on asking the Ghost about it.  The Ghost was quite concerned that we had discovered it and warned us that inside it was certain death, but he would not tell us anything else.  I decided that very night i would enter the secret room, however my sistor did not want to go with me and pleded with me not to....I now wish i had listened to her....I miss her, I wonder what happened to her....");
			}
		}

		public override bool IsTutorial{ get{ return false; } }
		public override TimeSpan RestartDelay{ get{ return TimeSpan.Zero; } }
		public override int Picture{ get{ return 0x15B5; } }

		public override void Slice()
		{
			if ( m_WaitForSummon && m_Jalinde != null )
			{
				if ( GhostPast.Active == false )
				{
					if ( From.Map == m_Jalinde.Map && From.InRange( m_Jalinde, 15 ) )
					{
						m_WaitForSummon = false;

						AddConversation( new VanquishGhostConversation() );
					}
				}
			}

			base.Slice();
		}

		public GhostQuest( Jalinde jalinde, PlayerMobile from ) : base( from )
		{
			m_Jalinde = jalinde;
		}

		public GhostQuest()
		{
		}

		public override void Accept()
		{
			base.Accept();

			AddConversation( new AcceptConversation() );
		}

		public override void ChildDeserialize( GenericReader reader )
		{
			int version = reader.ReadEncodedInt();

			m_Jalinde = reader.ReadMobile() as Jalinde;
			m_WaitForSummon = reader.ReadBool();
		}

		public override void ChildSerialize( GenericWriter writer )
		{
			writer.WriteEncodedInt( (int) 0 ); // version

			writer.Write( (Mobile) m_Jalinde );
			writer.Write( (bool) m_WaitForSummon );
		}
	}

	public class WhatHappenedToTearObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				return ("I need to find Tear Summerdrake or something from Tear to give to Jalinde so she will tell me the rest of the story");
			}
		}

		public WhatHappenedToTearObjective()
		{
		}

		public override void OnComplete()
		{
			Jalinde jalinde = ((GhostQuest)System).Jalinde;

			jalinde.Say("What is the meaning of this?  This letter is not from Tear at all!...why you....");

			System.AddConversation( new TheGatheringOfSoulCrystalsConversation() );
		}
	}

	public class TheGatheringOfSoulCrystalsObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				return ("So the letter was an attempt to sap the remaining hope out of Jalinde.  Well it has had the opposite effect on her, now she wants me to help get her revenge! Now I wonder if any of these other spirits can help me...");
			}
		}

		public override int MaxProgress{ get{ return 5; } }

		public TheGatheringOfSoulCrystalsObjective()
		{
		}

		public override void OnComplete()
		{
			Jalinde jalinde = ((GhostQuest)System).Jalinde;

			if ( jalinde == null )
			{
				System.From.SendMessage( "Internal error: Unable to find Jalinde. Quest unable to continue." );
				System.Cancel();
			}
			else
			{
				if ( GhostPast.Active == false )
				{
					System.AddConversation( new VanquishGhostConversation() );
				}
				else
				{
					jalinde.SayTo( System.From, "I have already summoned the Ghost, now go and destroy him!" );
					((GhostQuest)System).WaitForSummon = true;
				}
			}
		}

		public override void RenderMessage( BaseQuestGump gump )
		{
			if ( CurProgress > 0 && CurProgress < MaxProgress )
				gump.AddHtmlObject( 70, 130, 300, 100, "I still need to give Jalinde more Soul Crystals for the ceremony.", BaseQuestGump.Blue, false, false );
			else
				base.RenderMessage( gump );
		}

		public override void RenderProgress( BaseQuestGump gump )
		{
			if ( CurProgress > 0 && CurProgress < MaxProgress )
			{
				gump.AddHtmlObject( 70, 260, 270, 100, "Number of Soul Crystals given to Jalinde.", BaseQuestGump.Blue, false, false );

				gump.AddLabel( 70, 280, 100, CurProgress.ToString() );
				gump.AddLabel( 100, 280, 100, "/" );
				gump.AddLabel( 130, 280, 100, MaxProgress.ToString() );
			}
			else
			{
				base.RenderProgress( gump );
			}
		}
	}

	public class VanquishGhostObjective : QuestObjective
	{
		private GhostPast m_Ghost;

		public override object Message
		{
			get
			{
				return ("The Ghost has been summoned to his lair, go and destroy him!");
			}
		}

		public VanquishGhostObjective( GhostPast ghost )
		{
			m_Ghost = ghost;
		}

		public VanquishGhostObjective()
		{
		}

		public override void CheckProgress()
		{
			if ( GhostPast.Active == false)
				Complete();
		}

		public override void OnComplete()
		{
			System.Complete();
		}

		public override void ChildDeserialize( GenericReader reader )
		{
			int version = reader.ReadEncodedInt();

			m_Ghost = reader.ReadMobile() as GhostPast;
		}

		public override void ChildSerialize( GenericWriter writer )
		{
			writer.WriteEncodedInt( (int) 0 ); // version

			writer.Write( (Mobile) m_Ghost );
		}
	}
}