using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Scripts.Commands
{
	public class UOAssist
	{
		public static void Initialize()
		{
			Server.Commands.Register( "Band", AccessLevel.Player, new CommandEventHandler( Band_OnCommand ) );
			Server.Commands.Register( "BandSelf", AccessLevel.Player, new CommandEventHandler( BandSelf_OnCommand ) );
			Server.Commands.Register( "Equip", AccessLevel.Player, new CommandEventHandler( Equip_OnCommand ) );
			Server.Commands.Register( "UnEquip", AccessLevel.Player, new CommandEventHandler( UnEquip_OnCommand ) );
			Server.Commands.Register( "SingleClick", AccessLevel.Player, new CommandEventHandler( SingleClick_OnCommand ) );
			Server.Commands.Register( "DoubleClick", AccessLevel.Player, new CommandEventHandler( DoubleClick_OnCommand ) );
		}

		[Usage( "Band" )]
		[Description( "Uses a bandage if any available." )]
		public static void Band_OnCommand( CommandEventArgs e )
		{
			Mobile from = e.Mobile;
			Container backpack = from.Backpack;

			if ( backpack != null )
			{
				Bandage bandage = (Bandage)backpack.FindItemByType( typeof(Bandage) );
				if ( bandage != null )
				{
					from.RevealingAction();
					from.SendLocalizedMessage( 500948 ); // Who will you use the bandages on?
					from.Target = new InternalTarget( bandage );
				}
			}
		}

		private class InternalTarget : Target
		{
			private Bandage m_Bandage;

			public InternalTarget( Bandage bandage ) : base( 1, false, TargetFlags.Beneficial )
			{
				m_Bandage = bandage;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Bandage.Deleted )
					return;

				if ( targeted is Mobile )
				{
					if ( from.InRange( m_Bandage.GetWorldLocation(), 1 ) )
					{
						if ( BandageContext.BeginHeal( from, (Mobile)targeted ) != null )
							m_Bandage.Consume();
					}
					else
						from.SendLocalizedMessage( 500295 ); // You are too far away to do that.
				}
/*
				else if ( targeted is PlagueBeastInnard )
				{
					if ( ((PlagueBeastInnard) targeted).OnBandage( from ) )
						m_Bandage.Consume();
				}
*/
				else
				{
					from.SendLocalizedMessage( 500970 ); // Bandages can not be used on that.
				}
			}
/*
			protected override void OnNonlocalTarget( Mobile from, object targeted )
			{
				if ( targeted is PlagueBeastInnard )
				{
					if ( ((PlagueBeastInnard) targeted).OnBandage( from ) )
						m_Bandage.Consume();
				}
				else
					base.OnNonlocalTarget( from, targeted );
			}
*/
		}

		[Usage( "BandSelf" )]
		[Description( "Uses a bandage on yourself if any available." )]
		public static void BandSelf_OnCommand( CommandEventArgs e )
		{
			Mobile from = e.Mobile;

			if( from != null)
			{
				Container backpack = from.Backpack;

				if( backpack != null )
				{
					Bandage bandage = (Bandage) backpack.FindItemByType( typeof( Bandage ) );

					if ( bandage != null )
					{
						Targeting.Target.Cancel( from );

						from.RevealingAction();

						if ( BandageContext.BeginHeal( from, from ) != null )
							bandage.Consume();
					}
					else
						e.Mobile.SendMessage( "Cannot find bandage." );
				}
				else
					from.SendMessage( "You don't have a Backpack." );
			}
		}

		[Usage( "Equip <type>" )]
		[Description( "Equips an item of a type specified." )]
		public static void Equip_OnCommand( CommandEventArgs e )
		{
			try
			{
				Type type = null;

				Mobile from = e.Mobile;

				if( from != null)
				{
					if (e.Arguments.Length == 1)
						type = ScriptCompiler.FindTypeByName( e.GetString( 0 ) );

					if( type != null)
					{
						Container backpack = from.Backpack;

						if( backpack != null )
						{
							Item item = backpack.FindItemByType(type);

							if (item != null && item.Movable)
							{
//								Targeting.Target.Cancel( from );
								if( from.EquipItem( item ) == true )
									from.SendMessage("Item equipped.");
								else
									from.SendMessage("Item not equipped.");
							}
							else
								from.SendMessage("Item not found.");
						}
						else
							from.SendMessage( "You don't have a Backpack." );
					}
					else
						from.SendMessage("No type with that name was found.");
				}
			}
			catch
			{
			}
		}

		[Usage( "UnEquip <layer>" )]
		[Description( "Unequips the layer specified." )]
		public static void UnEquip_OnCommand( CommandEventArgs e )
		{
			try
			{
				Layer layer = 0;

				Mobile from = e.Mobile;

				if( from != null)
				{
					if (e.Arguments.Length == 1)
					{
						switch (e.Arguments[0])
						{
//							case "Invalid":
//							case "0":
//								layer = (Layer) 0;
//								break;
							case "FirstValid":
							case "1":
								layer = (Layer) 1;
								break;
							case "TwoHanded":
							case "2":
								layer = (Layer) 2;
								break;
							case "Shoes":
							case "3":
								layer = (Layer) 3;
								break;
							case "Pants":
							case "4":
								layer = (Layer) 4;
								break;
							case "Shirt":
							case "5":
								layer = (Layer) 5;
								break;
							case "Helm":
							case "6":
								layer = (Layer) 6;
								break;
							case "Gloves":
							case "7":
								layer = (Layer) 7;
								break;
							case "Ring":
							case "8":
								layer = (Layer) 8;
								break;
//							case "Unused_x9":
//							case "9":
//								layer = (Layer) 9;
//								break;
							case "Neck":
							case "10":
								layer = (Layer) 10;
								break;
//							case "Hair":
//							case "11":
//								layer = (Layer) 11;
//								break;
							case "Waist":
							case "12":
								layer = (Layer) 12;
								break;
							case "InnerTorso":
							case "13":
								layer = (Layer) 13;
								break;
							case "Bracelet":
							case "14":
								layer = (Layer) 14;
								break;
//							case "Unused_xF":
//							case "15":
//								layer = (Layer) 15;
//								break;
//							case "FacialHair":
//							case "16":
//								layer = (Layer) 16;
//								break;
							case "MiddleTorso":
							case "17":
								layer = (Layer) 17;
								break;
							case "Earrings":
							case "18":
								layer = (Layer) 18;
								break;
							case "Arms":
							case "19":
								layer = (Layer) 19;
								break;
							case "Cloak":
							case "20":
								layer = (Layer) 20;
								break;
//							case "Backpack":
//							case "21":
//								layer = (Layer) 21;
//								break;
							case "OuterTorso":
							case "22":
								layer = (Layer) 22;
								break;
							case "OuterLegs":
							case "23":
								layer = (Layer) 23;
								break;
//							case "LastUserValid":
//							case "24":
//								layer = (Layer) 24;
//								break;
//							case "Mount":
//							case "25":
//								layer = (Layer) 25;
//								break;
//							case "ShopBuy":
//							case "26":
//								layer = (Layer) 26;
//								break;
//							case "ShopResale":
//							case "27":
//								layer = (Layer) 27;
//								break;
//							case "ShopSell":
//							case "28":
//								layer = (Layer) 28;
//								break;
//							case "Bank":
//							case "29":
//								layer = (Layer) 29;
//								break;
							default:
								from.SendMessage("Usage: UnEquip <layer>");
								from.SendMessage("[FirstValid/TwoHanded/Shoes/Pants/Shirt/Helm/Gloves/Ring/Neck/Waist/InnerTorso/Bracelet/MiddleTorso/Earrings/Arms/Cloak/OuterTorso/OuterLegs]");
								return;
						}
					}
					else
					{
						from.SendMessage("Usage: UnEquip <layer>");
						from.SendMessage("[FirstValid/TwoHanded/Shoes/Pants/Shirt/Helm/Gloves/Ring/Neck/Waist/InnerTorso/Bracelet/MiddleTorso/Earrings/Arms/Cloak/OuterTorso/OuterLegs]");
						return;
					}

					if( layer != 0)
					{
						Container backpack = from.Backpack;

						if( backpack != null )
						{
							Item item = from.FindItemOnLayer( layer );

							if (item != null && item.Movable)
							{
//								Targeting.Target.Cancel( from );
								backpack.DropItem( item );
								from.SendMessage("Item unequiped.");
							}
							else
								from.SendMessage("Item not found.");
						}
						else
							from.SendMessage( "You don't have a Backpack." );
					}
					else
						from.SendMessage("No layer with that name was found.");
				}
			}
			catch
			{
			}
		}

		[Usage( "SingleClick <type>" )]
		[Description( "Single-clicks an item of type specified in your backpack if any available." )]
		public static void SingleClick_OnCommand( CommandEventArgs e )
		{
			try
			{
				Type type = null;

				Mobile from = e.Mobile;

				if( from != null)
				{
					if (e.Arguments.Length == 1)
						type = ScriptCompiler.FindTypeByName( e.GetString( 0 ) );

					if( type != null)
					{
						Container backpack = from.Backpack;

						if( backpack != null )
						{
							Item item = backpack.FindItemByType(type);

							if (item != null)
							{
//								Targeting.Target.Cancel( from );
								from.SendMessage("Oggetto trovato.");
								item.OnSingleClick(from);
							}
							else
								from.SendMessage("Oggetto non trovato.");
						}
						else
							from.SendMessage( "Non hai il backpack!!!" );
					}
					else
						from.SendMessage("Nessun oggetto con quel nome trovato.");
				}
			}
			catch
			{
			}
		}

		[Usage( "DoubleClick <type>" )]
		[Description( "Double-clicks (uses) an item of type specified in your backpack if any available." )]
		public static void DoubleClick_OnCommand( CommandEventArgs e )
		{
			try
			{
				Type type = null;

				Mobile from = e.Mobile;

				if( from != null)
				{
					if (e.Arguments.Length == 1)
						type = ScriptCompiler.FindTypeByName( e.GetString( 0 ) );

					if( type != null)
					{
						Container backpack = from.Backpack;

						if( backpack != null )
						{
							Item item = backpack.FindItemByType(type);

							if (item != null)
							{
								Targeting.Target.Cancel( from );
								from.SendMessage("Oggetto trovato.");
								item.OnDoubleClick(from);
							}
							else
								from.SendMessage("Oggetto non trovato.");
						}
						else
							from.SendMessage( "Non hai il backpack!!!" );
					}
					else
						from.SendMessage("Nessun oggetto con quel nome trovato.");
				}
			}
			catch
			{
			}
		}
	}
}