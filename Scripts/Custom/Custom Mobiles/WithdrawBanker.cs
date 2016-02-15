using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
	public class WithdrawBanker : BaseVendor
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos{ get { return m_SBInfos; } }

		public override NpcGuild NpcGuild{ get{ return NpcGuild.MerchantsGuild; } }

		[Constructable]
		public WithdrawBanker() : base( "the banker" )
		{
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBBanker() );
		}

		public static int GetBalance( Mobile from )
		{
			Item[] gold, checks;

			return GetBalance( from, out gold, out checks );
		}

		public static int GetBalance( Mobile from, out Item[] gold, out Item[] checks )
		{
			int balance = 0;

			Container bank = from.BankBox;

			if ( bank != null )
			{
				gold = bank.FindItemsByType( typeof( Gold ) );
				checks = bank.FindItemsByType( typeof( BankCheck ) );

				for ( int i = 0; i < gold.Length; ++i )
					balance += gold[i].Amount;

				for ( int i = 0; i < checks.Length; ++i )
					balance += ((BankCheck)checks[i]).Worth;
			}
			else
			{
				gold = checks = new Item[0];
			}

			return balance;
		}

		public static bool Withdraw( Mobile from, int amount )
		{
			Item[] gold, checks;
			int balance = GetBalance( from, out gold, out checks );

			if ( balance < amount )
				return false;

			for ( int i = 0; amount > 0 && i < gold.Length; ++i )
			{
				if ( gold[i].Amount <= amount )
				{
					amount -= gold[i].Amount;
					gold[i].Delete();
				}
				else
				{
					gold[i].Amount -= amount;
					amount = 0;
				}
			}

			for ( int i = 0; amount > 0 && i < checks.Length; ++i )
			{
				BankCheck check = (BankCheck)checks[i];

				if ( check.Worth <= amount )
				{
					amount -= check.Worth;
					check.Delete();
				}
				else
				{
					check.Worth -= amount;
					amount = 0;
				}
			}

			return true;
		}

		public static bool Deposit( Mobile from, int amount )
		{
			BankBox box = from.BankBox;

			Item item = ( amount >= 1000 ? (Item)new BankCheck( amount ) : (Item)new Gold( amount ) );

			if ( box != null && box.TryDropItem( from, item, false ) )
				return true;

			item.Delete();
			return false;
		}

		public WithdrawBanker( Serial serial ) : base( serial )
		{
		}

		public override bool HandlesOnSpeech( Mobile from )
		{
			if ( from.InRange( this.Location, 12 ) )
				return true;

			return base.HandlesOnSpeech( from );
		}

		public override void OnSpeech( SpeechEventArgs e )
		{
			if ( !e.Handled && e.Mobile.InRange( this.Location, 12 ) )
			{
				for ( int i = 0; i < e.Keywords.Length; ++i )
				{
					int keyword = e.Keywords[i];

					switch ( keyword )
					{
						case 0x0000: // *withdraw*
						{
							e.Handled = true;

							if ( e.Mobile.Criminal )
							{
								this.Say( 500389 ); // I will not do business with a criminal!
								break;
							}

							string[] split = e.Speech.Split( ' ' );

							if ( split.Length >= 2 )
							{
								int amount;

								try
								{
									amount = Convert.ToInt32( split[1] );
								}
								catch
								{
									break;
								}

								if ( amount > 5000 )
								{
									this.Say( 500381 ); // Thou canst not withdraw so much at one time!
								}
								else if ( amount > 0 )
								{
									BankBox box = e.Mobile.BankBox;

									if ( box == null || !box.ConsumeTotal( typeof( Gold ), amount ) )
									{
										this.Say( 500384 ); // Ah, art thou trying to fool me? Thou hast not so much gold!
									}
									else
									{
										e.Mobile.AddToBackpack( new Gold( amount ) );

										this.Say( 1010005 ); // Thou hast withdrawn gold from thy account.
									}
								}
							}

							break;
						}
						case 0x0001: // *balance*
						{
							e.Handled = true;

							if ( e.Mobile.Criminal )
							{
								this.Say( 500389 ); // I will not do business with a criminal!
								break;
							}

							BankBox box = e.Mobile.BankBox;

							if ( box != null )
							{
								this.Say( 1042759, box.TotalGold.ToString() ); // Thy current bank balance is ~1_AMOUNT~ gold.
							}

							break;
						}
					}
				}
			}

			base.OnSpeech( e );
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