using System;
using Server;
using Server.Items;
using Server.Targeting;
using Server.Network;
namespace Server.Gumps
{
	public class PlantStatMenu : Gump
	{
	private FullPlantBowl pot;
	public PlantStatMenu( Mobile from ) : this( from, null )
	{
	}
	public PlantStatMenu( Mobile from, Item item ) : base( 20, 20 )
	{
	if (from == null || item == null)
	return;
	pot = item as FullPlantBowl;
	//Code for determining labels

	string insect = " ";
	int ihue = 0;
	if ( pot.Insect > 0 )
	{
		insect = "+";
		if ( pot.Insect == 1 )
		{
			ihue = 0x35;
		}
		else
		{
			ihue = 0x21;
		}
	}

	string funghi = " ";
	int fhue = 0;
	if ( pot.Funghi > 0 )
	{
		funghi = "+";
		if ( pot.Funghi == 1 )
		{
			fhue = 0x35;
		}
		else
		{
			fhue = 0x21;
		}
	}

	string Poison = " ";
	int phue = 0;
	if ( pot.Poison == 1 )
		{
		Poison = "+";
		phue = 0x35;
		}
	else if ( pot.Poison == 2 )
		{
		Poison = "+";
		phue = 0x21;
		}

	string Disease = " ";
	int dhue = 0;
	if ( pot.Disease == 1 )
		{
		Disease = "+";
		dhue = 0x35;
		}
	else if ( pot.Disease == 2 )
		{
		Disease = "+";
		dhue = 0x21;
		}



	AddBackground( 50, 50, 200, 150, 0xE10 );
	AddItem( 45, 45, 0xCEF );
	AddItem( 45, 118, 0xCF0 );
	AddItem( 211, 45, 0xCEB );
	AddItem( 211, 118, 0xCEC );

	AddButton( 71, 67, 0xD4, 0xD4, 1, GumpButtonType.Reply, 0  );
	AddItem( 59, 68, 0xD08 );
	AddButton( 71, 91, 0xD4, 0xD4, 0x2, GumpButtonType.Reply, 0 );
	AddItem( 8, 96, 0x372 );
	AddButton( 71, 115, 0xD4, 0xD4, 0x3, GumpButtonType.Reply, 0 );
	AddItem( 58, 115, 0xD16 );
	AddButton( 71, 139, 0xD4, 0xD4, 0x4, GumpButtonType.Reply, 0 );
	AddItem( 59, 143, 0x1AE4 );
	AddButton( 71, 163, 0xD4, 0xD4, 0x5, GumpButtonType.Reply, 0 );
	AddItem( 55, 167, 0x1727 );
	AddImage( 209, 67, 0xD2 );
	AddButton( 209, 67, 0xD4, 0xD4, 0x6, GumpButtonType.Reply, 0 );
	AddItem( 193, 67, 0x1F9D );
	AddButton( 209, 91, 0xD4, 0xD4, 0x7, GumpButtonType.Reply, 0 );
	AddItem( 197, 91, 0xF0A );
	AddLabel( 196, 91, 0x835, pot.GPP.ToString() );
	AddButton( 209, 115, 0xD4, 0xD4, 0x8, GumpButtonType.Reply, 0 );
	AddItem( 192, 115, 0xF07 );
	AddLabel( 196, 115, 0x835, pot.GCP.ToString() );
	AddButton( 209, 139, 0xD4, 0xD4, 0x9, GumpButtonType.Reply, 0 );
	AddItem( 190, 139, 0xF0C );
	AddLabel( 196, 139, 0x835, pot.GHP.ToString() );
	AddButton( 209, 163, 0xD4, 0xD4, 0xA, GumpButtonType.Reply, 0 );
	AddItem( 193, 163, 0xF09 );
	AddLabel( 196, 163, 0x835, pot.GSP.ToString() );
	AddImage( 48, 183, 0xD2 );
	AddButton( 48, 183, 0xD4, 0xD4, 0xB, GumpButtonType.Reply, 0 );
	AddLabel( 54, 183, 0x835, "?" );
	AddButton( 232, 183, 0xD4, 0xD4, 0xC, GumpButtonType.Reply, 0 );
	AddItem( 219, 180, 0x15FD );
	AddImage( 48, 47, 0xD2 );
	AddLabel( 54, 47, 0x835, pot.Stage.ToString() );
	AddImage( 232, 47, 0xD2 );

	string grown = " ";
	int grownhue = 0;
	switch (pot.Grown)
	{
		case 0:
		{
			grown = "-";
			grownhue = 0x35;
		break;
		}
		case 1:
		{
			grown = "-";
			grownhue = 0x21;
			pot.Grown = 0;
		break;
		}
		case 2:
		{
			grown = "+";
			grownhue = 0x3;
			pot.Grown = 0;
		break;
		}
		case 3:
		{
			grown = "+";
			grownhue = 0x3F;
		break;
		}
		case 4:
		{
			grown = "!";
			grownhue = 0x21;
			pot.Grown = 0;
		break;
		}

	}
	AddLabel( 239, 47, grownhue, grown );

//Gump construction based on plant growth stage
	if ( pot.Stage < 7 ) //If before stage 6 we need to draw the pot with dirt gump
		{
		AddImage( 110, 85, 0x589 );
		AddItem( 122, 94, 0x914 );
		AddItem( 135, 94, 0x914 );
		AddItem( 120, 112, 0x914 );
		AddItem( 135, 112, 0x914 );
		}
	switch (pot.Stage)
	{
		case 6:
			{
			AddItem( 127, 112, 0xC62 );
			AddItem( 121, 117, 0xC62 );
			AddItem( 133, 117, 0xC62 );
			AddItem( 110, 100, 0xC62 );
			AddItem( 140, 100, 0xC62 );
			AddItem( 110, 130, 0xC62 );
			AddItem( 140, 130, 0xC62 );
			AddItem( 105, 115, 0xC62 );
			AddItem( 145, 115, 0xC62 );
			AddItem( 125, 90, 0xC62 );
			AddItem( 125, 135, 0xC62 );
			break;
			}
		case 5:
			{
			AddItem( 127, 112, 0xC62 );
			AddItem( 121, 117, 0xC62 );
			AddItem( 133, 117, 0xC62 );
			AddItem( 110, 100, 0xC62 );
			AddItem( 140, 100, 0xC62 );
			AddItem( 110, 130, 0xC62 );
			AddItem( 140, 130, 0xC62 );
			break;
			}
		case 4:
			{
			AddItem( 127, 112, 0xC62 );
			AddItem( 129, 85, 0xC7E );
			AddItem( 121, 117, 0xC62 );
			AddItem( 133, 117, 0xC62 );
			break;
			}
		case 3:
			{
			AddItem( 127, 112, 0xC62 );
			AddItem( 129, 85, 0xC7E );
			break;
			}
		case 2:
			{
			AddItem( 127, 112, 0xC62 );
			break;
			}
		case 1: break;
		case 0: break;
		default:
		{
		int x = 0; int y = 0;
		switch ( pot.SeedType )
			{
			case -8: x = 130; y = 96; break; // Campion Flower
			case -7: x = 130; y = 96; break; // Poppies
			case -6: x = 130; y = 106; break; // Snow Drops
			case -5: x = 115; y = 96; break; // Bulrushes
			case -4: x = 130; y = 96; break; // Lilies
			case -3: x = 122; y = 96; break; // Pampas Grass
			case -2: x = 120; y = 96; break; // Rushes
			case -1: x = 110; y = 96; break; // Elephant Ear Plant
			case 0: x = 110; y = 96; break;  // Fern
			case 1: x = 114; y = 91; break; // Ponytail Palm-
			case 2: x = 125; y = 86; break; // Small Palm
			case 3: x = 130; y = 69; break; // Century Plant
			case 4: x = 130; y = 106; break; // Water Plant
			case 5: x = 130; y = 96; break; // Snake Plant
			case 6: x = 130; y = 106; break; // Prickly Pear Cactus
			case 7: x = 130; y = 106; break; // Barrel Cactus
			case 8: x = 130; y = 106; break; // Tribarrel Cactus
			}
		AddItem( x, y, pot.ItemID ); // Plant pic
		break;
		}
	}

	AddLabel( 95, 92, ihue, insect );
	AddLabel( 95, 116, fhue, funghi );
	AddLabel( 95, 140, phue, Poison );
	AddLabel( 95, 164, dhue, Disease );

	if ( pot.Water == 1 )
		AddLabel( 196, 67, 0x35, "+" );
	else if ( pot.Water == -1 )
		AddLabel( 196, 67, 0x35, "-" );
	else if ( pot.Water > 1 )
		AddLabel( 196, 67, 0x21, "+" );
	else if ( pot.Water < -1 )
		AddLabel( 196, 67, 0x21, "-" );


	double health = (double) pot.Hits / pot.Max  ;
	if ( pot.Hits > 0 && pot.Stage > 0)
	{
	if ( health >= 0.75 )
		{
		AddItem( 93, 162, 0x1A99 );
		AddItem( 162, 162, 0x1A99 );
		AddHtmlLocalized( 129, 167, 42, 20, 1060822, 0x83E0, false, false ); // vibrant
		}
	else if ( health >= 0.5 )
		{
		AddItem( 96, 168, 0xC61 );
		AddItem( 162, 168, 0xC61 );
		AddHtmlLocalized( 129, 167, 42, 20, 1060823, 0x8200, false, false ); // healthy
		}
	else if ( health >= 0.25 )
		{
		AddItem( 91, 164, 0x18E6 );
		AddItem( 161, 164, 0x18E6 );
		AddHtmlLocalized( 132, 167, 42, 20, 1060824, 0xC207, false, false ); // wilted
		}
	else
		{
		AddItem( 92, 167, 0x1B9D );
		AddItem( 161, 167, 0x1B9D );
		AddHtmlLocalized( 136, 167, 42, 20, 1060825, 0xFC00, false, false ); // dying
		}
	}

	}

	public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;
			if (!pot.CheckItemUse(from, pot))
				return;
			Item[] Kegs = from.Backpack.FindItemsByType(typeof(PotionKeg));
			PotionKeg GPKeg = null;
			PotionKeg GCKeg = null;
			PotionKeg GHKeg = null;
			PotionKeg GSKeg = null;
			if (Kegs != null)
				{
				for (int i=0; i < Kegs.Length; i++)
					{
					switch ( ((PotionKeg)Kegs[i]).Type )
						{
						case PotionEffect.CureGreater: GCKeg = ((PotionKeg)Kegs[i]); break;
						case PotionEffect.StrengthGreater: GSKeg = ((PotionKeg)Kegs[i]); break;
						case PotionEffect.PoisonGreater: GPKeg = ((PotionKeg)Kegs[i]); break;
						case PotionEffect.HealGreater: GHKeg = ((PotionKeg)Kegs[i]); break;
						}
					}
				}
			switch ( info.ButtonID )
			{
				case 1: // resource menu
				{
				if ( pot.SeedType != -9 )
					from.SendGump( new PlantRepMenu( from, pot ) );
				else
					{
					from.SendGump( new PlantStatMenu( from, pot ) );
					from.SendLocalizedMessage( 1061885 ); // You need to plant a seed in the bowl first.
					}
				break;
				}
				case 2:
				{
				sender.Send(new CodexOfWisdom(53));//Opens Codex to Infestation Level
				from.SendGump(new PlantStatMenu( from, pot ) ); break;
				}
				case 3:
				{
				sender.Send(new CodexOfWisdom(55));//Opens Codex to Fungus Level
				from.SendGump(new PlantStatMenu( from, pot ) ); break;
				}
				case 4:
				{
				sender.Send(new CodexOfWisdom(57));//Opens Codex to Poison Level
				from.SendGump(new PlantStatMenu( from, pot ) ); break;
				}
				case 5:
				{
				sender.Send(new CodexOfWisdom(59));//Opens Codex to Disease Level
				from.SendGump(new PlantStatMenu( from, pot ) ); break;
				}
				case 6: // Add Water button
				{
				if ( BaseBeverage.ConsumeTotal( from.Backpack, BeverageType.Water, 1 ) )
					{
					from.PlaySound( 0x4e );
					pot.Water = pot.Water + 1;
					pot.SendLocalizedMessageTo( from, 1053048 ); // You soften the dirt with water.
					from.SendGump( new PlantStatMenu( from, pot ) );
					}
				else
					{
					from.SendLocalizedMessage( 1060808, PlantProps.Stages(pot.Stage) ); // Target the container you wish to use to water the ~1_val~.
					from.Target = new LuiquidTarget(pot);
					}
				break;
				}
				case 7: // Add Greater Poison button
				{
				if ( pot.GPP < 2 && pot.Stage != 0)
					{
					if ( GPKeg != null )
						{
						from.PlaySound( 0x240 );
						pot.GPP = pot.GPP + 1;
						--GPKeg.Held;
						pot.SendLocalizedMessageTo( from, 1053067 ); // You pour the potion over the plant.
						}
					else if ( from.Backpack.ConsumeTotal( typeof( GreaterPoisonPotion ), 1 ))
						{
						from.PlaySound( 0x240 );
						pot.GPP = pot.GPP + 1;
						from.AddToBackpack( new Bottle() );
						pot.SendLocalizedMessageTo( from, 1053067 ); // You pour the potion over the plant.
						}
					else
						{
						from.SendLocalizedMessage( 1061884 ); // You don't have any strong potions of that type in your pack.
						from.SendLocalizedMessage( 1060808, PlantProps.Stages(pot.Stage) ); // Target the container you wish to use to water the ~1_val~.
						from.Target = new LuiquidTarget(pot);break;
						}
					}
				else if ( pot.Stage == 0 )
					from.SendLocalizedMessage( 1061875 ); // You should only pour potions on a plant or seed!
				else
					{
					pot.SendLocalizedMessageTo( from, 1053065 ); // The plant is already soaked with this type of potion!
					}
				from.SendGump( new PlantStatMenu( from, pot ) );
				break;
				}
				case 8: // Add Greater Cure button
				{
				if ( pot.GCP < 2 && pot.Stage != 0)
					{
					if ( GCKeg != null )
						{
						from.PlaySound( 0x240 );
						pot.GCP = pot.GCP + 1;
						--GCKeg.Held;
						pot.SendLocalizedMessageTo( from, 1053067 ); // You pour the potion over the plant.
						}
					else if ( from.Backpack.ConsumeTotal( typeof( GreaterCurePotion ), 1 ))
						{
						from.PlaySound( 0x240 );
						pot.GCP = pot.GCP + 1;
						from.AddToBackpack( new Bottle() );
						pot.SendLocalizedMessageTo( from, 1053067 ); // You pour the potion over the plant.
						}
					else
						{
						from.SendLocalizedMessage( 1061884 ); // You don't have any strong potions of that type in your pack.
						from.SendLocalizedMessage( 1060808, PlantProps.Stages(pot.Stage) ); // Target the container you wish to use to water the ~1_val~.
						from.Target = new LuiquidTarget(pot);break;
						}
					}
				else if ( pot.Stage == 0 )
					from.SendLocalizedMessage( 1061875 ); // You should only pour potions on a plant or seed!
				else
					{
					pot.SendLocalizedMessageTo( from, 1053065 ); // The plant is already soaked with this type of potion!
					}
				from.SendGump( new PlantStatMenu( from, pot ) );
				break;
				}
				case 9: // Add Greater Heal button
				{
				if ( pot.GHP < 2 && pot.Stage != 0)
					{
					if ( GHKeg != null )
						{
						from.PlaySound( 0x240 );
						pot.GHP = pot.GHP + 1;
						--GHKeg.Held;
						pot.SendLocalizedMessageTo( from, 1053067 ); // You pour the potion over the plant.
						}
					else if ( from.Backpack.ConsumeTotal( typeof( GreaterHealPotion ), 1 ))
						{
						from.PlaySound( 0x240 );
						pot.GHP = pot.GHP + 1;
						from.AddToBackpack( new Bottle() );
						pot.SendLocalizedMessageTo( from, 1053067 ); // You pour the potion over the plant.
						}
					else
						{
						from.SendLocalizedMessage( 1061884 ); // You don't have any strong potions of that type in your pack.
						from.SendLocalizedMessage( 1060808, PlantProps.Stages(pot.Stage) ); // Target the container you wish to use to water the ~1_val~.
						from.Target = new LuiquidTarget(pot);break;
						}
					}
				else if ( pot.Stage == 0 )
					from.SendLocalizedMessage( 1061875 ); // You should only pour potions on a plant or seed!
				else
					pot.SendLocalizedMessageTo( from, 1053065 ); // The plant is already soaked with this type of potion!
				from.SendGump( new PlantStatMenu( from, pot ) );
				break;
				}
				case 10: // Add Greater Stregnth button
				{
				if ( pot.GSP < 2 && pot.Stage != 0)
					{
					if ( GSKeg != null )
						{
						from.PlaySound( 0x240 );
						pot.GSP = pot.GSP + 1;
						--GSKeg.Held;
						pot.SendLocalizedMessageTo( from, 1053067 ); // You pour the potion over the plant.
						}
					else if ( from.Backpack.ConsumeTotal( typeof( GreaterStrengthPotion ), 1 ))
						{
						from.PlaySound( 0x240 );
						pot.GSP = pot.GSP + 1;
						from.AddToBackpack( new Bottle() );
						pot.SendLocalizedMessageTo( from, 1053067 ); // You pour the potion over the plant.
						}
					else
						{
						from.SendLocalizedMessage( 1061884 ); // You don't have any strong potions of that type in your pack.
						from.SendLocalizedMessage( 1060808, PlantProps.Stages(pot.Stage) ); // Target the container you wish to use to water the ~1_val~.
						from.Target = new LuiquidTarget(pot);break;
						}
					}
				else if ( pot.Stage == 0 )
					from.SendLocalizedMessage( 1061875 ); // You should only pour potions on a plant or seed!
				else
					pot.SendLocalizedMessageTo( from, 1053065 ); // The plant is already soaked with this type of potion!
				from.SendGump( new PlantStatMenu( from, pot ) );
				break;
				}
				case 11: // Codex Plant Growing
				{
				from.SendGump( new PlantStatMenu( from, pot ) );
				sender.Send(new CodexOfWisdom(48));//Opens Codex to Plant Growing section
				break;
				}
				case 12: // empty pot
				{
				from.SendGump( new PlantConfirmMenu( from, pot ) ); break;
				}
			}
		}

		private class LuiquidTarget : Target
		{
			private FullPlantBowl pot;
			public LuiquidTarget(FullPlantBowl m_pot) : base( 4, true, TargetFlags.None )
			{
			pot = m_pot;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				bool full = false;
				if ( o is BaseBeverage)
					{
					BaseBeverage bb = o as BaseBeverage;
					if (bb.Content == BeverageType.Water && !bb.IsEmpty)
						{
						from.PlaySound( 0x4e );
						pot.Water = pot.Water + 1;
						pot.SendLocalizedMessageTo( from, 1053048 ); // You soften the dirt with water.
						from.SendGump( new PlantStatMenu( from, pot ) );
						return;
						}
					}

				if ( o is PotionKeg )
					{
					PotionKeg pk = o as PotionKeg;
					switch (pk.Type)
						{
						case PotionEffect.CureGreater:
							{
							if (pot.GCP ==2)
								{
								full = true;
								break;
								}
							from.PlaySound( 0x240 );
							pot.GCP++;
							--pk.Held;
							pot.SendLocalizedMessageTo( from, 1053067 ); // You pour the potion over the plant.
							from.SendGump( new PlantStatMenu( from, pot ) );
							break;
							}

						case PotionEffect.StrengthGreater:
							{
							if (pot.GSP ==2)
								{
								full = true;
								break;
								}
							from.PlaySound( 0x240 );
							pot.GSP++;
							--pk.Held;
							pot.SendLocalizedMessageTo( from, 1053067 ); // You pour the potion over the plant.
							from.SendGump( new PlantStatMenu( from, pot ) );
							break;
							}

						case PotionEffect.PoisonGreater:
							{
							if (pot.GPP ==2)
								{
								full = true;
								break;
								}
							from.PlaySound( 0x240 );
							pot.GPP++;
							--pk.Held;
							pot.SendLocalizedMessageTo( from, 1053067 ); // You pour the potion over the plant.
							from.SendGump( new PlantStatMenu( from, pot ) );
							break;
							}

						case PotionEffect.HealGreater:
							{
							if (pot.GHP ==2)
								{
								full = true;
								break;
								}
							from.PlaySound( 0x240 );
							pot.GHP++;
							--pk.Held;
							pot.SendLocalizedMessageTo( from, 1053067 ); // You pour the potion over the plant.
							from.SendGump( new PlantStatMenu( from, pot ) );
							break;
							}
						}
					if (!full)
						return;
					}
				if ( o is GreaterPoisonPotion )
					{
					if (pot.GPP ==2)
						full = true;
					else
						{
						Item p = o as Item;
						p.Consume();
						from.PlaySound( 0x240 );
						pot.GPP = pot.GPP + 1;
						from.AddToBackpack( new Bottle() );
						pot.SendLocalizedMessageTo( from, 1053067 ); // You pour the potion over the plant.
						from.SendGump( new PlantStatMenu( from, pot ) );
						return;
						}
					}
				if ( o is GreaterCurePotion )
					{
					if (pot.GCP ==2)
						full = true;
					else
						{
						Item p = o as Item;
						p.Consume();
						from.PlaySound( 0x240 );
						pot.GCP = pot.GCP + 1;
						from.AddToBackpack( new Bottle() );
						pot.SendLocalizedMessageTo( from, 1053067 ); // You pour the potion over the plant.
						from.SendGump( new PlantStatMenu( from, pot ) );
						return;
						}
					}
				if ( o is GreaterHealPotion )
					{
					if (pot.GHP ==2)
						full = true;
					else
						{
						Item p = o as Item;
						p.Consume();
						from.PlaySound( 0x240 );
						pot.GHP = pot.GHP + 1;
						from.AddToBackpack( new Bottle() );
						pot.SendLocalizedMessageTo( from, 1053067 ); // You pour the potion over the plant.
						from.SendGump( new PlantStatMenu( from, pot ) );
						return;
						}
					}
				if ( o is GreaterStrengthPotion )
					{
					if (pot.GSP ==2)
						full = true;
					else
						{
						Item p = o as Item;
						p.Consume();
						from.PlaySound( 0x240 );
						pot.GSP = pot.GSP + 1;
						from.AddToBackpack( new Bottle() );
						pot.SendLocalizedMessageTo( from, 1053067 ); // You pour the potion over the plant.
						from.SendGump( new PlantStatMenu( from, pot ) );
						return;
						}
					}
				if ( full )
					pot.SendLocalizedMessageTo( from, 1053065 ); // The plant is already soaked with this type of potion!
				else
					from.SendLocalizedMessage( 1053069 ); // You can't use that on a plant!
				from.SendGump( new PlantStatMenu( from, pot ) );
			}
		}
	}

	public class PlantConfirmMenu : Gump
	{

	private FullPlantBowl pot;
	public PlantConfirmMenu( Mobile from, FullPlantBowl item ) : base( 20, 20 )
	{
	pot = item;
	AddBackground( 50, 50, 200, 150, 0xE10 );
	AddItem( 45, 45, 0xCEF );
	AddItem( 45, 118, 0xCF0 );
	AddItem( 211, 45, 0xCEB );
	AddItem( 211, 118, 0xCEC );
	AddItem( 90, 100, 0x1602 );
	AddImage( 140, 102, 0x15E1 );
	AddItem( 160, 100, 0x15FD );
	AddLabel( 90, 70, 0x44, "Empty the bowl?" );
	AddImage( 138, 151, 0xD2 );
	AddButton( 138, 151, 0xD4, 0xD4, 0x3, GumpButtonType.Reply, 0 );
	AddLabel( 143, 151, 0x835, "?" );
	AddButton( 98, 150, 0x47E, 0x480, 0x2, GumpButtonType.Reply, 0 );
	AddButton( 168, 150, 0x481, 0x483, 0x1, GumpButtonType.Reply, 0 );
	if (pot.Stage > 0 && pot.Stage < 4)
		AddItem( 156, 130, 0xDCF );
	}

	public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;
			if (!pot.CheckItemUse(from, pot))
				return;
			switch ( info.ButtonID )
			{
				case 1: // Empty bowl
				{
				if ( pot.Stage < 4 && pot.SeedType != -9 )
					{
					from.AddToBackpack( new baseSeed( pot.SeedColor, pot.SeedType ) );
					from.AddToBackpack( new EmptyPlantBowl() );
					pot.Delete();
					}
				else if ( pot.Stage < 7 )
					{
					from.AddToBackpack( new EmptyPlantBowl() );
					pot.Delete();
					}
				else
					{
					pot.Delete();
					}
				break;
				}
				case 2: // Back to Status screen
				{
				from.SendGump( new PlantStatMenu( from, pot ) );
					break;
				}
				case 3: // Codex
				{
				from.SendGump( new PlantConfirmMenu( from, pot ) );
				sender.Send(new CodexOfWisdom(71));//Opens Codex to Emptying Bowl section
					break;
				}
			}
		}
	}

	public class PlantRepMenu : Gump
	{

	private FullPlantBowl pot;
	public PlantRepMenu( Mobile from, FullPlantBowl item ) : base( 20, 20 )
	{
	pot = item;
	AddBackground( 50, 50, 200, 150, 0xE10 );
	AddImage( 60, 90, 0xE17 );
	AddImage( 120, 90, 0xE17 );
	AddImage( 60, 145, 0xE17 );
	AddImage( 120, 145, 0xE17 );
	AddItem( 45, 45, 0xCEF );
	AddItem( 45, 118, 0xCF0 );
	AddItem( 211, 45, 0xCEB );
	AddItem( 211, 118, 0xCEC );
	AddButton( 70, 67, 0xD4, 0xD4, 1, GumpButtonType.Reply, 0 );
	AddItem( 57, 65, 0x1600 );
	AddLabel( 108, 67, 0x835, "Reproduction" );
	AddButton( 80, 116, 0xD4, 0xD4, 3, GumpButtonType.Reply, 0 );
	AddItem( 66, 117, 0x1AA2 );
	AddButton( 128, 116, 0xD4, 0xD4, 7, GumpButtonType.Reply, 0 );
	AddItem( 113, 120, 0x1021 );
	AddButton( 177, 116, 0xD4, 0xD4, 4, GumpButtonType.Reply, 0 );
	AddItem( 160, 121, 0xDCF );
	AddImage( 70, 163, 0xD2 );
	AddButton( 70, 163, 0xD4, 0xD4, 5, GumpButtonType.Reply, 0 );
	AddItem( 56, 164, 0x1AA2 );
	AddImage( 138, 163, 0xD2 );
	AddButton( 138, 163, 0xD4, 0xD4, 8, GumpButtonType.Reply, 0 );
	AddItem( 123, 167, 0x1021 );
	AddImage( 212, 163, 0xD2 );
	AddButton( 212, 163, 0xD4, 0xD4, 6, GumpButtonType.Reply, 0 );
	AddItem( 195, 168, 0xDCF );
	if (pot.Stage > 8)
		{
		AddButton( 212, 67, 0xD4, 0xD4, 2, GumpButtonType.Reply, 0 );
		AddItem( 202, 68, 0xC61 );
		AddLabel( 216, 66, 0x21, "/" );
		}
	int cc = 0;
	switch (pot.ChildColor)
		{
			case (int)PlantColors.Plain: cc = 0x835; break;
			case (int)PlantColors.Red: cc = 0x24; break;
			case (int)PlantColors.BrightRed: cc = 0x21; break;
			case (int)PlantColors.Blue: cc = 0x6; break;
			case (int)PlantColors.BrightBlue: cc = 0x3; break;
			case (int)PlantColors.Purple: cc = 0x10; break;
			case (int)PlantColors.BrightPurple: cc = 0xD; break;
			case (int)PlantColors.Orange: cc = 0x2E; break;
			case (int)PlantColors.BrightOrange: cc = 0x2B; break;
			case (int)PlantColors.Yellow: cc = 0x38; break;
			case (int)PlantColors.BrightYellow: cc = 0x35; break;
			case (int)PlantColors.Green: cc = 0x42; break;
			case (int)PlantColors.BrightGreen: cc = 0x3F; break;
			case (int)PlantColors.Black: cc = 0x0; break;
			//case (int)PlantColors.White: cc = 0x835; break;
			default: cc = pot.ChildColor; break;
		}
	if ( pot.SMax > 0 )
		AddLabel( 199, 116, cc, pot.SeedCounter + "/" + pot.SMax );
	else
		AddLabel( 204, 116, 0x21, "X");
	if (pot.ResourceType == 0)
		AddLabel( 154, 116, 0x21, "X" );
	else
		AddLabel( 149, 116, cc, pot.ResourceCounter + "/" + pot.RMax );
	if (pot.Stage < 7)
		AddLabel( 106, 116, 0x35, "-" );
	else if (!pot.Pollenated)
		AddLabel( 106, 116, 0x21, "!" );
	else
		AddLabel( 106, 116, 0x3F, "+" );//change to green

	}
	public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;
			if (!pot.CheckItemUse(from, pot))
				return;
			switch ( info.ButtonID )
			{
				case 1: // Status screen
				{
				from.SendGump( new PlantStatMenu( from, pot ) );
					break;
				}
				case 2: // Set to Decorative mode
				{
				from.SendGump( new ConfirmDecoMode ( from, pot ) );
				break;
				}
				case 3: // Codex Pollenation
				{
				from.SendGump( new PlantRepMenu( from, pot ) );
				sender.Send(new CodexOfWisdom(67));//Opens Codex to Pollenation
				break;
				}
				case 4: // Codex Seeds
				{
				from.SendGump( new PlantRepMenu( from, pot ) );
				sender.Send(new CodexOfWisdom(50));//Opens Codex to Seeds
				break;
				}
				case 5: // Pollenation
				{
				if ( pot.Hue == (int)PlantColors.White || pot.Hue == (int)PlantColors.Black || pot.Hue == (int)PlantColors.RareMagenta || pot.Hue == (int)PlantColors.RarePink || pot.Hue == (int)PlantColors.RareAqua || pot.Hue == (int)PlantColors.RareFireRed )
					pot.SendLocalizedMessageTo( from, 1053050 ); // You cannot gather pollen from a mutated plant!
				else if ( pot.Stage < 7 )
					pot.SendLocalizedMessageTo( from, 1053051 ); // You cannot gather pollen from a plant in this stage of development!
				else if ( pot.Hits < ( pot.Max / 2 ) )
					pot.SendLocalizedMessageTo( from, 1053052 ); // You cannot gather pollen from an unhealthy plant!
				else
					{
					pot.SendLocalizedMessageTo( from, 1053054 ); // Target the plant you wish to cross-pollinate to.
					from.Target = new InternalTarget( pot );
					}
				from.SendGump( new PlantRepMenu( from, pot ) );
				break;
				}
				case 6: // Gather Seeds
				{
				if ( pot.SeedCounter > 0 )
					{
					bool sc = false;
					if (pot.FiftyFifty)
						{
						if (from.PlaceInBackpack( new baseSeed( pot.ChildColor, pot.ChildType ) ))
							sc = true;
						}
					else
						{
						if (from.PlaceInBackpack( new NamedSeed( pot.ChildColor, pot.ChildType ) ))
							sc = true;
						}
					if (sc)
						{
						pot.SendLocalizedMessageTo( from, 1053063 ); // You gather seeds from the plant.
						pot.SeedCounter--;
						}
					else
						pot.SendLocalizedMessageTo( from, 1053062 ); // You attempt to gather as many seeds as you can hold, but your backpack is full.
					}
				else if ( pot.Hue == (int)PlantColors.White || pot.Hue == (int)PlantColors.Black )
					pot.SendLocalizedMessageTo( from, 1053060 ); // Mutated plants do not produce seeds!
				else
					pot.SendLocalizedMessageTo( from, 1053061 ); // This plant has no seeds to gather!
				from.SendGump( new PlantRepMenu( from, pot ) );
				break;
				}
				case 7: // Codex resources
				{
				from.SendGump( new PlantRepMenu( from, pot ) );
				sender.Send(new CodexOfWisdom(69));//Opens Codex to resources
				break;
				}
				case 8: // Gather resources
				{
				if ( pot.Hue == (int)PlantColors.White || pot.Hue == (int)PlantColors.Black )
					pot.SendLocalizedMessageTo( from, 1053055 ); // Mutated plants do not produce resources!
				else if ( pot.ResourceCounter == 0 )
					pot.SendLocalizedMessageTo( from, 1053056 ); // This plant has no resources to gather!
				else
					{
					bool rc = false;
						switch ( pot.ResourceType )
						{
							case 1:
							{
							if (from.PlaceInBackpack( new RedLeaf() ))
								rc = true;
							break;
							}
							case 2:
							{
							if (from.PlaceInBackpack( new OrangePetals() ))
								rc = true;
							break;
							}
							case 3:
							{
							if (from.PlaceInBackpack( new GreenThorn() ))
								rc = true;
							break;
							}
						}
					if (rc)
						{
						pot.SendLocalizedMessageTo( from, 1053059 ); // You gather resources from the plant.
						pot.ResourceCounter--;
						}
					else
						pot.SendLocalizedMessageTo( from, 1053058 ); // You attempt to gather as many resources as you can hold, but your backpack is full.
					}
				from.SendGump( new PlantRepMenu( from, pot ) );
				break;
				}
			}
		}
	}
	class InternalTarget : Target
		{
			private FullPlantBowl pot;
			public InternalTarget(FullPlantBowl m_pot) : base( 6, true, TargetFlags.None )
				{
				pot = m_pot;
				}

			protected override void OnTarget( Mobile from, object o )
				{

				if  ( o is FullPlantBowl )
				{
				FullPlantBowl pot2 = o as FullPlantBowl;
				if ( pot2.Pollenated )
					pot.SendLocalizedMessageTo( from, 1053072 ); // This plant has already been pollinated!
				else if ( pot2.Hue == (int)PlantColors.Black || pot2.Hue == (int)PlantColors.White || pot2.Hue == (int)PlantColors.RareMagenta || pot2.Hue == (int)PlantColors.RareAqua || pot2.Hue == (int)PlantColors.RarePink || pot2.Hue == (int)PlantColors.RareFireRed  )
					pot.SendLocalizedMessageTo( from, 1053073 ); // You cannot cross-pollinate with a mutated plant!
				else if ( pot2.Stage < 7 )
					pot.SendLocalizedMessageTo( from, 1053074 ); // This plant is not in the flowering stage. You cannot pollinate it!
				else if ( pot.Hits < ( pot.Max / 2 ) )
					pot.SendLocalizedMessageTo( from, 1053075 ); // You cannot pollinate an unhealthy plant!
				else
					{
					if ( pot2 == pot )
						{
						pot.SendLocalizedMessageTo( from, 1053071 ); // You pollinate the plant with its own pollen.
						pot.ChildType = pot.SeedType;
						pot.ChildColor = pot.SeedColor;
						pot2.Pollenated = true;
						return;
						}
					int ct = ( pot2.SeedType + pot.SeedType ) / 2 ;
					double ctd = (double)( pot2.SeedType + pot.SeedType ) / 2 ;
					if (ct != ctd)
						{
						pot2.ChildType = ct + (Utility.Random(2) * (ct < 0 ? -1 : 1));
						pot.FiftyFifty = true;
						}
					else
						pot2.ChildType = ct;
					// Red or bright red
					if ( pot.SeedColor == (int)PlantColors.Red || pot.SeedColor == (int)PlantColors.BrightRed )
						{
						if ( pot2.SeedColor == (int)PlantColors.Red || pot2.SeedColor == (int)PlantColors.BrightRed )
							pot2.ChildColor = (int)PlantColors.BrightRed;
						else if ( pot2.SeedColor == (int)PlantColors.Blue || pot2.SeedColor == (int)PlantColors.BrightBlue )
							pot2.ChildColor = (int)PlantColors.Purple;
						else if ( pot2.SeedColor == (int)PlantColors.Yellow || pot2.SeedColor == (int)PlantColors.BrightYellow )
							pot2.ChildColor = (int)PlantColors.Orange;
						else if ( pot2.SeedColor == (int)PlantColors.Plain )
							pot2.ChildColor = (int)PlantColors.Plain;
						else
							pot2.ChildColor = (int)PlantColors.Red;
						}
					// Blue or bright blue
					if ( pot.SeedColor == (int)PlantColors.Blue || pot.SeedColor == (int)PlantColors.BrightBlue )
						{
						if ( pot2.SeedColor == (int)PlantColors.Red || pot2.SeedColor == (int)PlantColors.BrightRed )
							pot2.ChildColor = (int)PlantColors.Purple;
						else if ( pot2.SeedColor == (int)PlantColors.Blue || pot2.SeedColor == (int)PlantColors.BrightBlue )
							pot2.ChildColor = (int)PlantColors.BrightBlue;
						else if ( pot2.SeedColor == (int)PlantColors.Yellow || pot2.SeedColor == (int)PlantColors.BrightYellow )
							pot2.ChildColor = (int)PlantColors.Green;
						else if ( pot2.SeedColor == (int)PlantColors.Plain )
							pot2.ChildColor = (int)PlantColors.Plain;
						else
							pot2.ChildColor = (int)PlantColors.Blue;
						}
					//Yellow or bright yellow
					if ( pot.SeedColor == (int)PlantColors.Yellow || pot.SeedColor == (int)PlantColors.BrightYellow )
						{
						if ( pot2.SeedColor == (int)PlantColors.Red || pot2.SeedColor == (int)PlantColors.BrightRed )
							pot2.ChildColor = (int)PlantColors.Orange;
						else if ( pot2.SeedColor == (int)PlantColors.Blue || pot2.SeedColor == (int)PlantColors.BrightBlue )
							pot2.ChildColor = (int)PlantColors.Green;
						else if ( pot2.SeedColor == (int)PlantColors.Yellow || pot2.SeedColor == (int)PlantColors.BrightYellow )
							pot2.ChildColor = (int)PlantColors.BrightYellow;
						else if ( pot2.SeedColor == (int)PlantColors.Plain )
							pot2.ChildColor = (int)PlantColors.Plain;
						else
							pot2.ChildColor = (int)PlantColors.Yellow;
						}
					// Purple and bright purple
					if ( pot.SeedColor == (int)PlantColors.Purple || pot.SeedColor == (int)PlantColors.BrightPurple )
						{
						if ( pot2.SeedColor == (int)PlantColors.Red || pot2.SeedColor == (int)PlantColors.BrightRed )
							pot2.ChildColor = (int)PlantColors.Red;
						else if ( pot2.SeedColor == (int)PlantColors.Blue || pot2.SeedColor == (int)PlantColors.BrightBlue )
							pot2.ChildColor = (int)PlantColors.Blue;
						else if ( pot2.SeedColor == (int)PlantColors.Yellow || pot2.SeedColor == (int)PlantColors.BrightYellow )
							pot2.ChildColor = (int)PlantColors.Yellow;
						else if ( pot2.SeedColor == (int)PlantColors.Plain )
							pot2.ChildColor = (int)PlantColors.Plain;
						else if ( pot2.SeedColor == (int)PlantColors.Purple || pot2.SeedColor == (int)PlantColors.BrightPurple )
							pot2.ChildColor = (int)PlantColors.BrightPurple;
						else if ( pot2.SeedColor == (int)PlantColors.Green || pot2.SeedColor == (int)PlantColors.BrightGreen )
							pot2.ChildColor = (int)PlantColors.Blue;
						else if ( pot2.SeedColor == (int)PlantColors.Orange || pot2.SeedColor == (int)PlantColors.BrightOrange )
							pot2.ChildColor = (int)PlantColors.Red;
						}
					// Green and bright green
					if ( pot.SeedColor == (int)PlantColors.Green || pot.SeedColor == (int)PlantColors.BrightGreen )
						{
						if ( pot2.SeedColor == (int)PlantColors.Red || pot2.SeedColor == (int)PlantColors.BrightRed )
							pot2.ChildColor = (int)PlantColors.Red;
						else if ( pot2.SeedColor == (int)PlantColors.Blue || pot2.SeedColor == (int)PlantColors.BrightBlue )
							pot2.ChildColor = (int)PlantColors.Blue;
						else if ( pot2.SeedColor == (int)PlantColors.Yellow || pot2.SeedColor == (int)PlantColors.BrightYellow )
							pot2.ChildColor = (int)PlantColors.Yellow;
						else if ( pot2.SeedColor == (int)PlantColors.Plain )
							pot2.ChildColor = (int)PlantColors.Plain;
						else if ( pot2.SeedColor == (int)PlantColors.Purple || pot2.SeedColor == (int)PlantColors.BrightPurple )
							pot2.ChildColor = (int)PlantColors.Blue;
						else if ( pot2.SeedColor == (int)PlantColors.Green || pot2.SeedColor == (int)PlantColors.BrightGreen )
							pot2.ChildColor = (int)PlantColors.BrightGreen;
						else if ( pot2.SeedColor == (int)PlantColors.Orange || pot2.SeedColor == (int)PlantColors.BrightOrange )
							pot2.ChildColor = (int)PlantColors.Yellow;
						}
					// Orange and bright Orange
					if ( pot.SeedColor == (int)PlantColors.Orange || pot.SeedColor == (int)PlantColors.BrightOrange )
						{
						if ( pot2.SeedColor == (int)PlantColors.Red || pot2.SeedColor == (int)PlantColors.BrightRed )
							pot2.ChildColor = (int)PlantColors.Red;
						else if ( pot2.SeedColor == (int)PlantColors.Blue || pot2.SeedColor == (int)PlantColors.BrightBlue )
							pot2.ChildColor = (int)PlantColors.Blue;
						else if ( pot2.SeedColor == (int)PlantColors.Yellow || pot2.SeedColor == (int)PlantColors.BrightYellow )
							pot2.ChildColor = (int)PlantColors.Yellow;
						else if ( pot2.SeedColor == (int)PlantColors.Plain )
							pot2.ChildColor = (int)PlantColors.Plain;
						else if ( pot2.SeedColor == (int)PlantColors.Purple || pot2.SeedColor == (int)PlantColors.BrightPurple )
							pot2.ChildColor = (int)PlantColors.Red;
						else if ( pot2.SeedColor == (int)PlantColors.Green || pot2.SeedColor == (int)PlantColors.BrightGreen )
							pot2.ChildColor = (int)PlantColors.Yellow;
						else if ( pot2.SeedColor == (int)PlantColors.Orange || pot2.SeedColor == (int)PlantColors.BrightOrange )
							pot2.ChildColor = (int)PlantColors.BrightOrange;
						}
					// Plain
					if ( pot.SeedColor == (int)PlantColors.Plain )
						pot2.ChildColor = (int)PlantColors.Plain;
					pot2.Pollenated = true;
					// 1 in 100 chance of a mutant
					if ( Utility.Random(100) == 1)
						pot2.ChildColor = Utility.RandomBool() ? (int)PlantColors.Black : (int)PlantColors.White;
					pot2.SendLocalizedMessageTo( from, 1053076 ); // You successfully cross-pollinate the plant.
					}

				}
				else
					{
					from.SendLocalizedMessage( 1053070 ); // You can only pollinate other specially grown plants!
					}
				}
		}

	public class ConfirmDecoMode : Gump
	{
	private FullPlantBowl pot;

		public ConfirmDecoMode( Mobile from, FullPlantBowl item ) : base( 20, 20 )
		{
			pot = item;
			AddBackground( 50, 50, 200, 150, 0xE10 );
			AddItem( 25, 45, 0xCEB );
			AddItem( 25, 118, 0xCEC );
			AddItem( 227, 45, 0xCEF );
			AddItem( 227, 118, 0xCF0 );
			AddLabel( 115, 85, 0x44, "Set plant" );
			AddLabel( 82, 105, 0x44, "to decorative mode?" );
			AddImage( 138, 141, 0xD2 );
			AddButton( 138, 141, 0xD4, 0xD4, 3, GumpButtonType.Reply, 0 );
			AddLabel( 143, 141, 0x835, "?" );
			AddButton( 98, 140, 0x47E, 0x480, 2, GumpButtonType.Reply, 0 );
			AddButton( 168, 140, 0x481, 0x483, 1, GumpButtonType.Reply, 0 );
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;
			if (!pot.CheckItemUse(from, pot))
				return;
			switch ( info.ButtonID )
			{
				case 1: // Set to Deco Mode
				{
				if ( pot.Hits > 0 )
					{
					pot.SendLocalizedMessageTo( from, 1053077 ); // You prune the plant. This plant will no longer produce resources or seeds, but will require no upkeep.
					from.AddToBackpack( new DecorativePlant(pot.ItemID, pot.Hue) );
					pot.Delete();
					}
				break;
				}
				case 2: // Back to Reproduction gump
				{
				from.SendGump( new PlantRepMenu( from, pot ) );
				break;
				}
				case 3: // Codex
				{
				from.SendGump( new ConfirmDecoMode( from, pot ) );
				sender.Send(new CodexOfWisdom(70));//Opens Codex to Decorative Mode
				break;
				}
			}
		}
	}
}