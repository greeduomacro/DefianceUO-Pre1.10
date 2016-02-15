using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	public class BrotherMaynard : BaseCreature
	{
		private string m_Substring;

		static int m_Attempts;
		public static int Attempts
		{
			get{ return m_Attempts; }
			set{ m_Attempts = value; }
		}

		static bool m_SellBomb;
		public static bool SellBomb
		{
			get{ return m_SellBomb; }
			set{ m_SellBomb = value; }
		}

		static bool m_TalkedTo;
		public static bool TalkedTo
		{
			get{ return m_TalkedTo; }
			set{ m_TalkedTo = value; }
		}

		[Constructable]
		public BrotherMaynard() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Brother Maynard";
			Hue = Utility.RandomSkinHue();
			BodyValue = 400;
			m_Attempts = 0;
			m_SellBomb = true;
			m_Substring = "Holy Hand Grenade";
			m_TalkedTo = false;
			Blessed = true;

			SetStr( 150 );
			SetDex( 150 );
			SetInt( 150 );

			SetHits( 200 );

			Robe robe = new Robe();
			robe.Hue = 2213;
			robe.LootType = LootType.Blessed;
			AddItem( robe );
		}

		public override void OnSpeech( SpeechEventArgs e )
		{
			if ( !e.Handled )
			{
				Mobile m = e.Mobile;

				if ( !m.InRange( this, 2 ) )
					return;

				bool isMatch = false;

				if ( m_Substring != null && e.Speech.ToLower().IndexOf( m_Substring.ToLower() ) >= 0 )
					isMatch = true;

				if ( !isMatch )
					return;

				e.Handled = true;
				if ( BigTeethRabbit.Active == true && m_SellBomb == true )
				{
					m_TalkedTo = true;

					if ( m_Attempts == 0 )
					{
						this.Say( "So the evil creature with big pointy teeth has reappeared...For a small fee of 1000 gold pieces I shall help thy smite it!" );
					}
					else if ( m_Attempts == 1 )
					{
						this.Say( "This is the second time the divine weapon has been purchased from me regarding this evil creature, he must be a tricky one to hit!");
					}
					else if ( m_Attempts > 1 && m_Attempts < 5 )
					{
						this.Say( "I do hope you know how to use this weapon sire, this will be your {0} attempt at slaying the evil one!", m_Attempts + 1 );
					}
					else if ( m_Attempts > 4 && m_Attempts < 11)
					{
						this.Say( "Sire I really do wonder what goes on it there to warrent the use of {0} Holy Hand Grenades!", m_Attempts + 1 );
					}
					else if ( m_Attempts > 10 )
					{
						this.Say( "Sire I really must insist you kill the evil one soon, you have used {0} Grenades so far!!", m_Attempts );
					}
				}
				else if ( BigTeethRabbit.Active == true && m_SellBomb == false )
				{
					this.Say( "I have already given a fellow knight this divine weapon sire" );
				}
				else if ( BigTeethRabbit.Active == false )
				{
					this.Say( "The divine weapon you seek is only for use against the big pointy teeth rabbit and he has not been seen for some time" );
				}
			}
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			PlayerMobile player = from as PlayerMobile;

			if ( player != null )
			{
				if ( m_SellBomb == true && m_TalkedTo == true )
				{
					if ( dropped is Gold )
					{
						Gold gold = (Gold)dropped;

						if ( gold.Amount < 1000 )
						{
							this.Say( "That is not enough gold sire." );
						}
						else if ( gold.Amount > 1000 )
						{
							this.Say( "That is too much sire, I cannot accept it." );
						}
						else if ( gold.Amount == 1000 )
						{
							gold.Delete();

							SayTo( from, "Take this holy hand grenade and throw it near the creature, but remember to count to three first and not four nor two, unless proceeding to three!" );
							from.AddToBackpack( new HolyHandGrenade() );
							m_SellBomb = false;
							m_TalkedTo = false;

							if (0.10 >= Utility.RandomDouble() )
							{
								SayTo( from, "I found this while wandering the cave. It means nothing to me but maybe it means something to you" );
								from.AddToBackpack( new PoemPartsThree() );
							}
						}
					}
				}
				else if ( m_SellBomb == false && m_TalkedTo == false )
				{
					if ( dropped is Gold )
					{

						this.Say( "Why are you trying to give me gold sire?" );
					}
				}
			}
			return false;
		}

		public BrotherMaynard( Serial serial ) : base( serial )
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
	}
}