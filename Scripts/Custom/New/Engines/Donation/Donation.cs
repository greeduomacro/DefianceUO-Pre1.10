using System;
using System.Collections;
using System.Threading;
using System.Data;
using System.Data.Odbc;
using System.IO;
using MySql.Data.MySqlClient;
using Server;
using Server.Accounting;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Multis.Deeds;
using Server.Misc;
using Xanthos.Evo;

namespace Server.Donation
{
	public class Donation
	{
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Item GetItem(int productNumber)
		{
			Item item = null;

			switch (productNumber)
			{
				case 16133:
					item = new EtherealHorse();
					break;
				case 16134:
					item = new EtherealLlama();
					break;
				case 16135:
					item = new EtherealOstard();
					break;
				case 16136:
					item = new KillBook();
					break;
				case 16137:
					item = new Bag();
					((Bag)item).DropItem(new SpidersSilk(50000));
					((Bag)item).DropItem(new Nightshade(50000));
					((Bag)item).DropItem(new Bloodmoss(50000));
					((Bag)item).DropItem(new MandrakeRoot(50000));
					((Bag)item).DropItem(new Ginseng(50000));
					((Bag)item).DropItem(new Garlic(50000));
					((Bag)item).DropItem(new BlackPearl(50000));
					((Bag)item).DropItem(new SulfurousAsh(50000));
					break;
				case 16138:
					item = new SpecialDonateDye();
					break;
				case 16141:
					item = new Sandals();
                                        item.Name = "magic threads";
					item.Hue = 5;
					item.LootType = LootType.Blessed;
					break;
				case 16142:
					item = new Sandals();
                                        item.Name = "magic threads";
					item.Hue = 110;
					item.LootType = LootType.Blessed;
					break;
				case 16139:
					item = new Sandals();
                                        item.Name = "magic threads";
					item.Hue = 90;
					item.LootType = LootType.Blessed;
					break;
				case 16140:
					item = new Sandals();
                                        item.Name = "magic threads";
					item.Hue = 70;
					item.LootType = LootType.Blessed;
					break;
				case 16143:
					item = new HoodedShroudOfShadows();
					item.Name = "a rare agapite shroud";
					item.Hue = 2425;
					item.LootType = LootType.Blessed;
                                        break;
				case 16144:
					item = new ValoriteDonationBox();
					break;
				case 16145:
					item = new SilverDonationBox();
					break;
				case 16146:
					item = new GoldenDonationBox();
					break;
				case 16147:
					item = new SurvivalPack();
					break;
				case 16148:
					item = new EtherealSkeletal();
					break;
				case 16149:
					item = new WeddingDeed();
					break;
				case 16150:
					item = new DonationSandals();
					break;
				case 16151:
					item = new DonationBandana();
					break;
				case 16152:
					item = new MembershipTicket();
					((MembershipTicket)item).MemberShipTime = TimeSpan.FromDays(30);
					break;
				case 16153:
					item = new MembershipTicket();
					((MembershipTicket)item).MemberShipTime = TimeSpan.FromDays(90);
					break;
				case 16154:
					item = new MembershipTicket();
					((MembershipTicket)item).MemberShipTime = TimeSpan.FromDays(180);
					break;
				case 16155:
					item = new MembershipTicket();
					((MembershipTicket)item).MemberShipTime = TimeSpan.FromDays(360);
					break;
				case 16156:
					item = new SexChangeDeed();
					item.LootType = LootType.Blessed;
                                        break;
				case 16157:
					item = new Item(5360);
					item.Name = "a Character Transfer Ticket";
                                        item.Hue = 1266;
					item.LootType = LootType.Blessed;
					break;
				case 16158:
					item = new LayerSashDeed();
                                        item.LootType = LootType.Blessed;
                                        break;
				case 16159:
					item = new EtherealUnicorn();
					break;
				case 16160:
					item = new AOSHouseDeed7x12();
					break;
				case 16161:
					item = new AOSHouseDeed12x7();
					break;
				case 16162:
					item = new AOSHouseDeed15x12();
					break;
				case 16163:
					item = new AOSHouseDeed12x15();
					break;
				case 16164:
					item = new AOSHouseDeed18x18();
					break;
				case 16165:
					item = new DonationSkillBall( 1 );
					break;
				case 16166:
					item = new DonationSkillBall( 5 );
					break;
				case 16167:
					item = new DonationSkillBall( 10 );
					break;
				case 16168:
					item = new DonationSkillBall( 25 );
					break;
				case 16169:
					item = new DonationSkillBall( 50 );
					break;
				case 16170:
					item = new Bag();
					((Bag)item).DropItem(new IronIngot(50000));
					break;
				case 16171:
					item = new Bag();
					((Bag)item).DropItem(new Board(50000));
					break;
				case 16172:
					item = new PetBondDeed();
					break;
                                case 16184:
					item = new PotionBundle();
					break;
                                case 16185:
					item = null;//new PotionBundleAoS();
					break;
                                case 16186:
					item = new ApagiteDonationBox();
					break;
                                case 16187:
					item = new VeriteDonationBox();
					break;
                                case 16188:
					item = new SerpentCrest();
					break;
                                case 16189:
					item = new IronMaiden();
					break;
                                case 16190:
					item = new Guillotine();
					break;
                                case 16191:
					item = new BigMushroom1();
					break;
                                case 16192:
					item = new BigMushroom2();
					break;
                                case 16193:
					item = new BigMushroom3();
					break;
                                case 16194:
					item = new BigMushroom4();
					break;
                                case 16195:
					item = new LillyPad1();
					break;
                                case 16196:
					item = new LillyPad2();
					break;
                                case 16197:
					item = new LillyPad3();
					break;
                                case 16198:
					item = new DonationDecorArmor1();
					break;
                                case 16199:
					item = new DonationDecorArmor2();
					break;
                                case 16200:
					item = new DonationDecorArmor3();
					break;
                                case 16201:
					item = new DonationDecorArmor4();
					break;
                                case 16242:
					item = new CastleDeed();
					item.LootType = LootType.Blessed;
					break;
                                case 16243:
					item = new KeepDeed();
					item.LootType = LootType.Blessed;
					break;
                                case 16244:
					item = new BlackHairDye();
					item.LootType = LootType.Blessed;
					break;
                                case 16245:
					item = new SpecialDonateDyeBeard();
					item.LootType = LootType.Blessed;
					break;
                                case 16246:
					item = new KillDeed();
					item.LootType = LootType.Blessed;
					break;
                                case 16247:
					item = new SkinToneDeed();
					item.LootType = LootType.Blessed;
					break;
                                case 16248:
					item = new EtherealMountDeed();
					item.LootType = LootType.Blessed;
					break;
                               case 16249:
					item = new Item(5360);
                                        item.Hue = 1266;
					item.Name = "a house teleporter ticket";
					item.LootType = LootType.Blessed;
					break;
                                case 16250:
					item = new Item(5360);
                                        item.Hue = 1266;
					item.Name = "a water house spot ticket";
					item.LootType = LootType.Blessed;
					break;
                                case 16251:
					item = new ElevenBox();
					break;
                                case 16252:
					item = new ElvenRobe();
					break;
                                case 16253:
					item = new RaffleTicket();
					break;
					case 16267:
							item = new PoisongreenDonationBox();
							break;
					case 16268:
							item = new SoulStone();
							break;
					case 16269:
							item = new SoulStoneFragment();
							break;
					case 16270:
							item = new BoneTable();
							break;
					case 16271:
							item = new BoneThrone();
							break;
					case 16272:
							item = new OneMillionBankCheckDeed();
							break;
					case 16273:
							item = new DarkblueDonationBox();
							break;
					case 16274:
					////////item = new xxx();
							break;
					case 16275:
							item = new ShadowDonationBox();
							break;
					case 16292 :
							item = new FishTankAddonDeed();
							break;
					case 16294 :
							item = new AncientBedAddonDeed();
							break;
					case 16296 :
							item = new HeroKnightShield();
							break;
					case 16297 :
							item = new StoneSculpture();
							break;
					case 16298 :
							item = new AncientFruitBowl();
							break;
					case 16299 :
							item = new AncientRobe();
							item.LootType = LootType.Blessed;
							break;
					case 16300 :
							item = new AncientShoes();
							item.LootType = LootType.Blessed;
							break;
					case 16301 :
							item = new AncientCoat();
							item.LootType = LootType.Blessed;
							break;
					case 16302 :
							item = new GardenDonationBox();
							break;
					case 16303 :
							item = new DungeonDonationBox();
							break;
					case 16304 :
							item = new UltimateDecorationDonationBox();
							break;
					case 16305 :
							item = new ChristmasDonationBox();
							break;
					case 16306 :
							item = new MetalHueBundleTicket();
							break;
					case 16307 :
							item = new SpecialHueBundleTicket();
							break;
					case 16308 :
							item = new SevenGMBag();
							break;
					case 16311 :
							item = new PokerLowRollerTicket();
							break;
					case 16312 :
							item = new PokerHighRollerTicket();
							break;
					case 16313 :
							item = new MoongateLibraryDeed();
							break;
					case 16314 :
							item = new DisplayCaseAddonDeed();
							break;
					case 16315:
					item = new TamerDonationBox();
					break;
                                case 16316:
					item = new SeedBox();
					break;
                                case 16317:
					item = new DarkRedDonationBox();
					break;
                                case 16318:
					item = new DarkGreenDonationBox();
					break;
                                case 16319:
					item = new FireDonationBox();
					break;
                                case 16320:
					item = new PinkDonationBox();
					break;
                                case 16321:
					item = new DarkBrownDonationBox();
					break;
                                case 16322:
					item = new OliveDonationBox();
					break;
                                case 16323:
					item = new HiryuTicket();
					break;
                                case 16324:
				////////item = new xxx();
					break;
                                case 16325:
					item = new ExtremeHueBundleTicket();
					break;
                                case 16326:
					item = new TribalMask();
                                        item.Name = "Mask of the Fire God";
					item.Hue = 1359;
					item.LootType = LootType.Blessed;
					break;
                                case 16327:
					item = new TribalMask();
                                        item.Name = "Mask of Velvet Purity";
					item.Hue = 1374;
					item.LootType = LootType.Blessed;
					break;
                                case 16328:
					item = new TribalMask();
                                        item.Name = "Mask of the Farseer";
					item.Hue = 1267;
					item.LootType = LootType.Blessed;
					break;
                                case 16329:
					item = new TribalMask();
                                        item.Name = "Mask of the Venom King";
					item.Hue = 1196;
					item.LootType = LootType.Blessed;
					break;
                                case 16330:
					item = new ShroudOfIllusions();
                                        item.Name = "Shroud of the Morphling Lord";
					item.Hue = 1154;
					item.LootType = LootType.Blessed;
					break;
                                case 16331:
					item = new EtherealWarSteed();
					break;
                                case 16332:
					item = new EtherealDragonSteed();
					break;
// NEW DONATIONS ITEM 20-11-08 ---------------------------------------------------------
                                	 case 16333:
					item = new TribalMask();
                                        item.Name = "Mask of the Scoundrel";
					item.Hue = 920;
					item.LootType = LootType.Blessed;
					break;

                                	 case 16334:
					item = new TribalMask();
                                        item.Name = "Mask of the Harrower";
					item.Hue = 1194;
					item.LootType = LootType.Blessed;
					break;

                               	  	 case 16335:
					item = new TribalMask();
                                        item.Name = "Mask of the Tree Lord";
					item.Hue = 1445;
					item.LootType = LootType.Blessed;
					break;

                               		 case 16336:
					item = new TribalMask();
                                        item.Name = "Mask of the Rich";
					item.Hue = 2213;
					item.LootType = LootType.Blessed;
					break;

                                	 case 16337:
					item = new TribalMask();
                                        item.Name = "Mask of the Unknown";
					item.Hue = 2055;
					item.LootType = LootType.Blessed;
					break;

                               		 case 16338:
					item = new Shoes();
                                        item.Name = "I support the shard";
					item.Hue = 1166;
					item.LootType = LootType.Blessed;
					break;

                               		 case 16339:
					item = new Shoes();
                                        item.Name = "I support the shard";
					item.Hue = 1260;
					item.LootType = LootType.Blessed;
					break;

                               		 case 16340:
					item = new Shoes();
                                        item.Name = "I support the shard";
					item.Hue = 1153;
					item.LootType = LootType.Blessed;
					break;

                                	case 16341:
					item = new Shoes();
                                        item.Name = "I support the shard";
					item.Hue = 1177;
					item.LootType = LootType.Blessed;
					break;

					  case 16342:
					item = new Shoes();
                                        item.Name = "I support the shard";
					item.Hue = 1266;
					item.LootType = LootType.Blessed;
					break;

					  case 16343:
					item = new RoyalCloak();
					item.LootType = LootType.Blessed;
					break;

					  case 16344:
					item = new OrangeDonationBox();
					break;
//// New items Feb - 2009 ////   //// New items Feb - 2009 ////   //// New items Feb - 2009 ////

 					 case 16345:
					item = new GoldRing();
                                        item.Name = "a 603-carat diamond";
					item.Hue = 2067;
					item.LootType = LootType.Blessed;
					break;

					  case 16346:
					item = new FireGlasses();
					item.LootType = LootType.Blessed;
					break;

					  case 16347:
					item = new ShroudOfmysterious();
					item.LootType = LootType.Blessed;
					break;

					  case 16348:
					item = new HoodedShroudOfShadows();
					item.Hue = 2055;
					item.LootType = LootType.Blessed;
					break;

					  case 16349:
					item = new HoodedShroudOfShadows();
					item.Hue = 2054;
					item.LootType = LootType.Blessed;
					break;

					  case 16350:
					item = new HoodedShroudOfShadows();
					item.Hue = 2053;
					item.LootType = LootType.Blessed;
					break;


                              		  case 16354:
					item = new TribalMask();
                                        item.Name = "a tribal mask to be renamed.";
					item.Hue = 37;
					item.LootType = LootType.Blessed;
					break;


                            		  case 16355:
					item = new TribalMask();
                                        item.Name = "a tribal mask to be renamed.";
					item.Hue = 9;
					item.LootType = LootType.Blessed;
					break;


                            		  case 16356:
					item = new TribalMask();
                                        item.Name = "a tribal mask to be renamed.";
					item.Hue = 39;
					item.LootType = LootType.Blessed;
					break;


                            		  case 16357:
					item = new TribalMask();
                                        item.Name = "a tribal mask to be renamed.";
					item.Hue = 92;
					item.LootType = LootType.Blessed;
					break;


                            		  case 16358:
					item = new BronzeStatueMaker();
					item.LootType = LootType.Blessed;
					break;

                            		  case 16359:
					item = new JadeStatueMaker();
					item.LootType = LootType.Blessed;

					break;

                            		  case 16360:
					item = new MarbleStatueMaker();
					item.LootType = LootType.Blessed;
					break;


                            		  case 16361:
					item = new GoldStatueMaker();
					item.LootType = LootType.Blessed;
					break;


                            		  case 16362:
					item = new BloodstoneStatueMaker();
					item.LootType = LootType.Blessed;
					break;


                            		  case 16363:
					item = new AlabasterStatueMaker();
					item.LootType = LootType.Blessed;
					break;

                            		  case 16364:
					item = new EtherealKirin();
					item.LootType = LootType.Blessed;
					break;


                            		  case 16365:
					item = new GMSafeTradeTicket();
					item.LootType = LootType.Blessed;
					break;


                            		  case 16366:
					item = new TheDarkSoldiers();
					break;


                            		  case 16367:
					item = new TheDarkLords();
					break;

                            		  case 16368:
					item = new TheDarkKnightsBundle();
					break;

					case 16373:
					item = new Bag();
					((Bag)item).DropItem(new Bottle(10000));
					break;


					case 16374:
					item = new Bag();
					((Bag)item).DropItem(new BarbedLeather(5000));
					break;


					case 16375:
					item = new Bag();
					((Bag)item).DropItem(new SpinedLeather(5000));
					break;


					case 16376:
					item = new Bag();
					((Bag)item).DropItem(new HornedLeather(5000));
					break;


					case 16377:
					item = new CheapEvoPointsDeed();
					item.LootType = LootType.Blessed;
					break;

					case 16378:
					item = new TitleChangeDeed();
					item.LootType = LootType.Blessed;
					break;

					case 16379:
					item = new OceanBlueDonationBundle();
					break;

					case 16390:
					item = new SkyBlueDonationBundle();
					break;

					case 16381:
					item = new LayeredSpellbookTicket();
					break;

					case 16391:
					item = new TeleporterDonationDeed();
					break;


				case 16392:
				item = new EtherealWarSteed();
				item.Name = "a donation ethereal mount";
				item.Hue = 1150;
				item.LootType = LootType.Blessed;
				break;

				case 16393:
				item = new EtherealWarSteed();
				item.Name = "a donation ethereal mount";
				item.Hue = 1167;
				item.LootType = LootType.Blessed;
				break;

				case 16394:
				item = new EtherealWarSteed();
				item.Name = "a donation ethereal mount";
				item.Hue = 38;
				item.LootType = LootType.Blessed;
				break;

				case 16395:
				item = new EtherealWarSteed();
				item.Name = "a donation ethereal mount";
				item.Hue = 1160;
				item.LootType = LootType.Blessed;
				break;

				case 16396:
				item = new EtherealWarSteed();
				item.Name = "a donation ethereal mount";
				item.Hue = 1161;
				item.LootType = LootType.Blessed;
				break;

				case 16397:
				item = new EtherealWarSteed();
				item.Name = "a donation ethereal mount";
				item.Hue = 1140;
				item.LootType = LootType.Blessed;
				break;

                              	case 16398:
				item = new TribalMask();
                                item.Name = "a tribal mask to be renamed.";
				item.Hue = 1167;
				item.LootType = LootType.Blessed;
				break;

                              	case 16399:
				item = new TribalMask();
                                item.Name = "a tribal mask to be renamed.";
				item.Hue = 1160;
				item.LootType = LootType.Blessed;
				break;

                              	case 16400:
				item = new TribalMask();
                                item.Name = "a tribal mask to be renamed.";
				item.Hue = 1161;
				item.LootType = LootType.Blessed;
				break;

                              	case 16401:
				item = new TribalMask();
                                item.Name = "a tribal mask to be renamed.";
				item.Hue = 1172;
				item.LootType = LootType.Blessed;
				break;

                              	case 16402:
				item = new TribalMask();
                                item.Name = "a tribal mask to be renamed.";
				item.Hue = 1165;
				item.LootType = LootType.Blessed;
				break;

                              	case 16403:
				item = new TribalMask();
                                item.Name = "a tribal mask to be renamed.";
				item.Hue = 1140;
				item.LootType = LootType.Blessed;
				break;


                              	case 16404:
				item = new DonationDeed();
                                item.Name = "a supporting deed.";
				item.Hue = 1161;
				item.LootType = LootType.Blessed;
				break;


                              	case 16405:
				item = new CTFDonationTicket();
                                item.Name = "a ctf ticket.";
				item.Hue = 1110;
				item.LootType = LootType.Blessed;
				break;

                              	case 16406:
				item = new UHSSkinDeed();
                                item.Name = "a uhs skin deed.";
				item.Hue = 1110;
				item.LootType = LootType.Blessed;
				break;

                              	case 16407:
				item = new BlackBeardHairDye();
                                item.Name = "a black beard hair dye.";
				item.Hue = 1175;
				item.LootType = LootType.Blessed;
				break;








                                default:
					item = null;
					break;
			}

			return item;
		}
		public static string GetDescription(int productNumber)
		{
			string description = "";

			switch (productNumber)
			{
				case 16133:
					description = "No-Age Ethereal Horse";
					break;
				case 16134:
					description = "No-Age Ethereal Llama";
					break;
				case 16135:
					description = "No-Age Ethereal Ostard";
					break;
				case 16136:
					description = "Kill Book";
					break;
				case 16137:
					description = "50.000 of each Reagent.";
					break;
				case 16138:
					description = "Special Donator's Hair Dye";
					break;
				case 16141:
					description = "Flashy Blue Sandals";
					break;
				case 16142:
					description = "Flashy Purple Sandals";
					break;
				case 16139:
					description = "Flashy Cyan Sandals";
					break;
				case 16140:
					description = "Flashy Green Sandals";
					break;
				case 16143:
					description = "Agapite Hooded Shroud";
					break;
				case 16144:
					description = "Valorite Donation Bundle";
					break;
				case 16145:
					description = "Silver Donation Bundle";
					break;
				case 16146:
					description = "Gold Donation Bundle";
					break;
				case 16147:
					description = "Survival Pack Bundle";
					break;
				case 16148:
					description = "Ethereal Skeletal Steed";
					break;
				case 16149:
					description = "Wedding Deed";
					break;
				case 16150:
					description = "Donation Sandals";
					break;
				case 16151:
					description = "Donation Bandana";
					break;
				case 16152:
					description = "Donation Status 1 Month";
					break;
				case 16153:
					description = "Donation Status 3 Months";
					break;
				case 16154:
					description = "Donation Status 6 Months";
					break;
				case 16155:
					description = "Donation Status 12 Months";
					break;
				case 16156:
					description = "SexChange Deed";
					break;
				case 16157:
					description = "Character Transfer Ticket";
					break;
				case 16158:
					description = "Layer BodySash Deed";
					break;
				case 16159:
					description = "No-Age Ethereal Unicorn";
					break;
				case 16160:
					description = "A customizable house deed(7x12)";
					break;
				case 16161:
					description = "A customizable house deed(12x7)";
					break;
				case 16162:
					description = "A customizable house deed(15x12)";
					break;
				case 16163:
					description = "A customizable house deed(12x15)";
					break;
				case 16164:
					description = "A customizable house deed(18x18)";
					break;
				case 16165:
					description = "A skillball worth 1 point";
					break;
				case 16166:
					description = "A skillball worth 5 point";
					break;
				case 16167:
					description = "A skillball worth 10 point";
					break;
				case 16168:
					description = "A skillball worth 25 point";
					break;
				case 16169:
					description = "A skillball worth 50 point";
					break;
				case 16170:
					description = "50,000 Iron Ingots";
					break;
				case 16171:
					description = "50,000 Wooden Boards";
					break;
				case 16172:
					description = "A Pet Bonding deed";
					break;
				case 16184:
					description = "A Potion Bundle";
					break;
				case 16185:
					description = "A Potion Bundle";
					break;
				case 16186:
					description = "A Apagite Donation Bundle";
					break;
				case 16187:
					description = "A Verite Donation Bundle";
					break;
				case 16188:
					description = "A Donation Serpent Crest";
					break;
				case 16189:
					description = "A Donation Iron Maiden";
					break;
				case 16190:
					description = "A Donation Guillotine";
					break;
				case 16191:
					description = "A Big Donation Mushroom 1";
					break;
				case 16192:
					description = "A Big Donation Mushroom 2";
					break;
				case 16193:
					description = "A Big Donation Mushroom 3";
					break;
				case 16194:
					description = "A Big Donation Mushroom 4";
					break;
				case 16195:
					description = "A Donation LillyPad 1";
					break;
				case 16196:
					description = "A Donation LillyPad 2";
					break;
				case 16197:
					description = "A Donation LillyPad 3";
					break;
				case 16198:
					description = "Donation Decoration Armor 1";
					break;
				case 16199:
					description = "Donation Decoration Armor 2";
					break;
				case 16200:
					description = "Donation Decoration Armor 3";
					break;
				case 16201:
					description = "Donation Decoration Armor 4";
					break;
				case 16242:
					description = "Castle Deed";
					break;
				case 16243:
					description = "Keep Deed";
					break;
				case 16244:
					description = "Black Hair Dye";
					break;
				case 16245:
					description = "Special Donate Beard Dye";
					break;
				case 16246:
					description = "Kill Deed";
					break;
				case 16247:
					description = "Skin Tone Deed";
					break;
				case 16248:
					description = "Ethereal Mount Deed";
					break;
				case 16249:
					description = "a house teleporter ticket";
					break;
				case 16250:
					description = "a water house spot ticket";
					break;
				case 16251:
					description = "Elven Chest";
					break;
				case 16252:
					description = "Elven Robe";
					break;
				case 16253:
					description = "A Raffle Ticket";
					break;
                                case 16267:
                                        description = "A Poisongreen Donation Bundle";
                                        break;
                                case 16268:
                                        description = "A SoulStone";
                                        break;
                                case 16269:
                                        description = "A SoulStoneFragment";
                                        break;
                                case 16270:
                                        description = "A BoneTable";
                                        break;
                                case 16271:
                                        description = "A BoneThrone";
                                        break;
                                case 16272:
                                        description = "A One Million BankCheck Deed";
                                        break;
                                case 16273:
                                        description = "A Darkblue Donation Bundle";
                                        break;
                                case 16274:
                                        description = "A Violet Donation Bundle";
                                        break;
                                case 16275:
                                        description = "A Shadow Donation Bundle";
                                        break;
                                case 16292:
                                        description = "Fish Tank Deed";
                                        break;
                                case 16294:
                                        description = "Ancient Bed Deed";
                                        break;
                                case 16296:
                                        description = "Hero Knight Shield";
                                        break;
                                case 16297:
                                        description = "Stone Sculpture";
                                        break;
                                case 16298:
                                        description = "AncientFruitBowl";
                                        break;
                                case 16299:
                                        description = "Ancient Robe";
                                        break;
                                case 16300:
                                        description = "Ancient Shoes";
                                        break;
                                case 16301:
                                        description = "Ancient Coat";
                                        break;
                                case 16302:
                                        description = "Garden Decoration Chest";
                                        break;
                                case 16303:
                                        description = "Dungeon Decoration Chest";
                                        break;
                                case 16304:
                                        description = "Ultimate Decoration Chest";
                                        break;
                                case 16305:
                                        description = "Christmas Decoration Chest";
                                        break;
                                case 16306:
                                        description = "Metal Hue Bundle Ticket";
                                        break;
                                case 16307:
                                        description = "Special Hue Bundle Ticket";
                                        break;
                                case 16308:
                                        description = "Seven GM Bag";
                                        break;
                                case 16311:
                                        description = "Poker Low Roller Ticket";
                                        break;
                                case 16312:
                                        description = "Poker High Roller Ticket";
                                        break;
                                case 16313:
                                        description = "Moongate Library Deed";
                                        break;
                                case 16314:
                                        description = "Display Case Deed";
                                        break;
                                case 16315:
                                        description = "Taming Donation Box";
                                        break;
                                case 16316:
                                        description = "Seed Box";
                                        break;
                                case 16317:
                                        description = "Dark Red Donation Box";
                                        break;
                                case 16318:
                                        description = "Dark Green Donation Box";
                                        break;
                                case 16319:
                                        description = "Fire Donation Box";
                                        break;
                                case 16320:
                                        description = "Pink Donation Box";
                                        break;
                                case 16321:
                                        description = "Dark Brown Donation Box";
                                        break;
                                case 16322:
                                        description = "Olive Donation Box";
                                        break;
                                case 16323:
                                        description = "Ethereal Hiryu Ticket";
                                        break;
                                case 16324:
                                        description = "xxx";
                                        break;
                                case 16325:
                                        description = "Extreme Hue Bundle Ticket";
                                        break;
                                case 16326:
                                        description = "Tribal Mask";
                                        break;
                                case 16327:
                                        description = "Tribal Mask";
                                        break;
                                case 16328:
                                        description = "Tribal Mask";
                                        break;
                                case 16329:
                                        description = "Tribal Mask";
                                        break;
                                case 16330:
                                        description = "Shroud of Illusions";
                                        break;
                                case 16331:
                                        description = "Ethereal War Steed";
                                        break;
                                case 16332:
                                        description = "Ethereal Dragon Steed";
                                        break;
// NEW DONATIONS ITEM 20-11-08 ---------------------------------------------------------
                                case 16333:
                                        description = "Tribal Mask";
                                        break;
                                case 16334:
                                        description = "Tribal Mask";
                                        break;
                                case 16335:
                                        description = "Tribal Mask";
                                        break;
                                case 16336:
                                        description = "Tribal Mask";
                                        break;
                                case 16337:
                                        description = "Tribal Mask";
                                        break;
                                case 16338:
                                        description = "Shoes";
                                        break;
                                case 16339:
                                        description = "Shoes";
                                        break;
                                case 16340:
                                        description = "Shoes";
                                        break;
                                case 16341:
                                        description = "Shoes";
                                        break;
                                case 16342:
                                        description = "Shoes";
                                        break;
                                case 16343:
                                        description = "Royal Cloak";
                                        break;
                                case 16344:
                                        description = "Orange Donation Box";
                                        break;

//// New items Feb - 2009 //// //// New items Feb - 2009 //// //// New items Feb - 2009 ////

                                case 16345:
                                        description = "a 603-carat diamond ring";
                                        break;

                                case 16346:
                                        description = "Fire Glasses";
                                        break;

                                case 16347:
                                        description = "a mysterious shroud";
                                        break;

                                case 16348:
                                        description = "The Third to last Darkest Hooded Robe";
                                        break;


                                case 16349:
                                        description = "The Second to last Darkest Hooded Robe";
                                        break;

                                case 16350:
                                        description = "The Darkest Hooded Robe";
                                        break;


                                case 16354:
                                        description = "Tribal Mask";
                                        break;



                                case 16355:
                                        description = "Tribal Mask";
                                        break;



                                case 16356:
                                        description = "Tribal Mask";
                                        break;



                                case 16357:
                                        description = "Tribal Mask";
                                        break;


                                case 16358:
                                        description = "Bronze Statue Maker";
                                        break;



                                case 16359:
                                        description = "Jade Statue Maker";
                                        break;



                                case 16360:
                                        description = "Marble Statue Maker";
                                        break;


                                case 16361:
                                        description = "Gold Statue Maker";
                                        break;


                                case 16362:
                                        description = "Bloodstone Statue Maker";
                                        break;


                                case 16363:
                                        description = "Alabaster Statue Maker";
                                        break;


                            	case 16364:
                                        description = "Ethereal Kirin";
					break;


                            	case 16365:
                                        description = "GM Safe Trade Ticket";
					break;

                            	case 16366:
                                        description = "New Donation Bundle";
					break;

                            	case 16367:
                                        description = "New Donation Bundle";
					break;

                            	case 16368:
                                        description = "New Donation Bundle";
					break;

                            	case 16373:
                                        description = "10,000 Empty Bottles";
					break;

                            	case 16374:
                                        description = "Leather";
					break;

                            	case 16375:
                                        description = "Leather";
					break;

                            	case 16376:
                                        description = "Leather";
					break;

                            	case 16377:
                                        description = "1 Million EP Points";
					break;

                            	case 16378:
                                        description = "Title Deed";
					break;

                            	case 16379:
                                        description = "Ocean Blue Donation Bundle";
					break;

                            	case 16390:
                                        description = "Sky Blue Donation Bundle";
					break;

                            	case 16381:
                                        description = "Layered Spell Book Deed";
					break;

                            	case 16391:
                                        description = "GM Place Teleporter Deed";
					break;

                            	case 16392:
                                        description = "Donation Ethereal";
					break;

                            	case 16393:
                                        description = "Donation Ethereal";
					break;

                            	case 16394:
                                        description = "Donation Ethereal";
					break;

                            	case 16395:
                                        description = "Donation Ethereal";
					break;

                            	case 16396:
                                        description = "Donation Ethereal";
					break;

                            	case 16397:
                                        description = "Donation Ethereal";
					break;

                            	case 16398:
                                        description = "Tribal Mask";
					break;


                            	case 16399:
                                        description = "Tribal Mask";
					break;


                            	case 16400:
                                        description = "Tribal Mask";
					break;


                            	case 16401:
                                        description = "Tribal Mask";
					break;


                            	case 16402:
                                        description = "Tribal Mask";
					break;


                            	case 16403:
                                        description = "Tribal Mask";
					break;

                            	case 16404:
                                        description = "Supporting Deed";
					break;

                            	case 16405:
                                        description = "CTF Deed";
					break;

                            	case 16406:
                                        description = "UHS Skin";
					break;

                            	case 16407:
                                        description = "Black Beard Dye";
					break;








                                default:
					description = "";
					break;

			}

			return description;
		}

		private static string m_ConString = "Server=www.defianceuo.com;Database=defiance_eshop;User ID=defiance_ODBC;password=ohyeah;port=3306";
		private static Queue m_ResultQueue;
		private static ArrayList m_ClaimedOrders;
		private static ResultTimer result;

		private static bool ms_ClaimDonationsBlocked;
		public static bool ClaimDonationsBlocked { get { return ms_ClaimDonationsBlocked; } }

		public static void Initialize()
		{
			m_ResultQueue = new Queue();
			m_ClaimedOrders = new ArrayList();
			Server.Commands.Register("ClaimDonations", AccessLevel.Player, new CommandEventHandler(ClaimDonations_OnCommand));
            Server.Commands.Register("SetDonations", AccessLevel.Administrator, new CommandEventHandler(SetDonations_OnCommand));
            Load();
			result = new ResultTimer();
			result.Start();
		}

		#region Serilization
		private static string idxPath = Path.Combine("Saves", Path.Combine("Donation", "ClaimedOrders.idx"));
                private static string binPath = Path.Combine("Saves", Path.Combine("Donation", "ClaimedOrders.bin"));

		public static void Load()
		{
            log.Info("Loading...");
            //Console.Write("Donation: Loading...");

			ms_ClaimDonationsBlocked = false;

            if (File.Exists(idxPath) && File.Exists(binPath))
            {
                // Declare and initialize reader objects.
                FileStream idx = new FileStream(idxPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                FileStream bin = new FileStream(binPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryReader idxReader = new BinaryReader(idx);
                BinaryFileReader binReader = new BinaryFileReader(new BinaryReader(bin));

                // Start by reading the number of orders from an index file
                int orderCount = idxReader.ReadInt32();
                if (orderCount == 0)
                {
                    log.Warn("Donations save does not contain any entries, [claimdonations deactivated.");
                    //Console.WriteLine("Donations save does not contain any entries, [claimdonations deactivated.");
                    ms_ClaimDonationsBlocked = true;
                }

                for (int i = 0; i < orderCount; ++i)
                {
                    ClaimedOrder c = new ClaimedOrder();
                    // Read start-position and length of current order from index file
                    long startPos = idxReader.ReadInt64();
                    int length = idxReader.ReadInt32();
                    // Point the reading stream to the proper position
                    binReader.Seek(startPos, SeekOrigin.Begin);

                    try
                    {
                        c.Deserialize(binReader);

                        if (binReader.Position != (startPos + length))
                            throw new Exception(String.Format("***** Bad serialize on ClaimedOrder[{0}] *****", i));
                    }
                    catch (Exception e)
                    {
                        log.Fatal("Error deserializing donations, [claimdonations deactivated.", e);
                        //Console.WriteLine("Error deserializing donations, [claimdonations deactivated: {0}", e.Message);
                        ms_ClaimDonationsBlocked = true;
                    }
                    m_ClaimedOrders.Add(c);
                }
                // Remember to close the streams
                idxReader.Close();
                binReader.Close();
            }
            else
            {
                log.Error("Error deserializing donations: idx/bin doesn't exist, [claimdonations deactivated.");
                //Console.WriteLine("Error deserializing donations: idx/bin doesn't exist, [claimdonations deactivated.");
                ms_ClaimDonationsBlocked = true;
            }

			//Console.WriteLine("done");
            log.Info("done.");
		}


		public static void Save()
		{
            log.Info("Saving...");
            log.Info(String.Format("idxPath: '{0}'", idxPath));
            log.Info(String.Format("binPath: '{0}'", binPath));

			if (!Directory.Exists(Path.Combine("Saves", "Donation")))
				Directory.CreateDirectory(Path.Combine("Saves", "Donation"));

			GenericWriter idx = new BinaryFileWriter(idxPath, false);
			GenericWriter bin = new BinaryFileWriter(binPath, true);

            log.Info(String.Format("m_ClaimedOrders.Count: '{0}'", m_ClaimedOrders.Count));
            idx.Write((int)m_ClaimedOrders.Count);
			foreach (ClaimedOrder claimed in m_ClaimedOrders)
			{
				long startPos = bin.Position;
				claimed.Serialize(bin);
				idx.Write((long)startPos);
				idx.Write((int)(bin.Position - startPos));
			}
			idx.Close();
			bin.Close();
            log.Info("Saving done.");
		}

		#endregion

		private class ResultTimer : Timer
		{

			public ResultTimer() : base(TimeSpan.FromSeconds( 0.5 ), TimeSpan.FromSeconds( 0.5 ))
			{
			}

			protected override void OnTick()
			{
				DatabaseResult result = null;

				Monitor.Enter(m_ResultQueue);
				if (m_ResultQueue.Count > 0)
					result = (DatabaseResult)m_ResultQueue.Dequeue();
				Monitor.Exit(m_ResultQueue);

				if (result != null && result.Mobile != null && result.Mobile.NetState != null)
				{
					result.DataItems = GetUnclaimed(result.DataItems);

					if (result.Status == ResultStatus.OK && result.DataItems.Count == 0)
					{
						result.Status = ResultStatus.NoUndeliveredDonationsFound;
					}
					if (!result.Mobile.HasGump(typeof(ClaimDonationsGump)))
					{
						result.Mobile.SendGump(new ClaimDonationsGump(0, 50, 50, result));
					}
					else
					{
						result.Mobile.SendMessage("Your claim command was canceled because you are already in the middle of claiming rewards.");
					}
				}
			}


		}

        [Usage("SetDonations <true | false>")]
        [Description("Toggles [claimdonations availability.")]
        public static void SetDonations_OnCommand(CommandEventArgs e)
        {
            if (e.Length == 1)
            {
                ms_ClaimDonationsBlocked = !e.GetBoolean(0);//Note: true leads to donations not being blocked!
                e.Mobile.SendMessage("Donations have been {0}.", ms_ClaimDonationsBlocked ? "disabled" : "enabled");
                e.Mobile.SendMessage("Warning: Donations will be reactivated on server restart!");
            }
            else
            {
                e.Mobile.SendMessage("Donations are {0}.", ms_ClaimDonationsBlocked ? "disabled" : "enabled");
                e.Mobile.SendMessage("Format: SetDonations <true | false>");
            }
        }

        [Usage("ClaimDonations")]
        [Description("Use this to claim any donation rewards")]
        public static void ClaimDonations_OnCommand(CommandEventArgs e)
        {
            if (ms_ClaimDonationsBlocked)
                e.Mobile.SendMessage("[claimdonations deactivated. Please contact an Administrator.");
            else
                CheckDonations(e.Mobile);
        }

        public static void CheckDonations(Mobile m)
		{
			//Dispatch thread
			DonationChecker dc = new DonationChecker(m);
			Thread t = new Thread(new ThreadStart(dc.BeginCheck));
			t.Name = "Worker Thread";
			t.Start();
		}


		private class DonationChecker
		{

			private Mobile m_Mobile;

			private IDbConnection con;
			private IDbCommand command;
			private IDataReader reader;

			public DonationChecker(Mobile from)
			{
				m_Mobile = from;
			}

			//SELECT s1 FROM t1 WHERE s1 IN    (SELECT s1 FROM t2);
			// SELECT order_id, product_id, amount FROM xlite_order_items WHERE order_id IN (SELECT order_id FROM xlite_orders WHERE profile_id = BAH)
			public void BeginCheck()
			{
                Console.WriteLine("Beginning of thread function");

				if (m_Mobile == null)
					return;

				DatabaseResult result = new DatabaseResult(m_Mobile);
				result.Status = ResultStatus.Unresolved;
				string accountName = (m_Mobile.Account as Account).Username;
				con = null;
				command = null;
				reader = null;

				try
				{

					string query = "SELECT DISTINCT xlite_order_items.order_id, xlite_order_items.product_id, xlite_order_items.amount " +
										"FROM xlite_order_items, xlite_orders, xlite_profiles " +
										"WHERE xlite_order_items.order_id = xlite_orders.order_id " +
										"AND xlite_orders.profile_id = xlite_profiles.profile_id " +
										"AND xlite_profiles.shipping_firstname = '" + accountName + "' " +
										"AND xlite_orders.status = 'P' " +
										"ORDER BY xlite_order_items.order_id";

					con = new MySqlConnection(m_ConString);
					con.Open();
					command = con.CreateCommand();
					command.CommandText = query;
					reader = command.ExecuteReader();

					while (reader.Read())
					{
						DataItem item = new DataItem(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2));
						Console.WriteLine("Order ID: {0}, ProductID: {1}, Amount: {2}", item.OrderID, item.ProductID, item.Amount);
						result.DataItems.Add(item);
					}
					reader.Close();
					if (result.DataItems.Count > 0)
						result.Status = ResultStatus.OK;
					else
						result.Status = ResultStatus.NoDonationsFound;

					/*
					string query = "SELECT profile_id FROM xlite_profiles WHERE shipping_firstname = '"+m_AccountName+"'";

					con = new OdbcConnection(m_ConString);
					command= new OdbcCommand(query,con);
					command.Connection.Open();
					reader = command.ExecuteReader();

					int profileID;
					if (reader.Read())
					{
						profileID = reader.GetInt32(0);
						reader.Close();

						string subQuery =	"SELECT DISTINCT xlite_order_items.order_id, xlite_order_items.product_id, xlite_order_items.amount " +
											"FROM xlite_order_items, xlite_orders " +
											"WHERE xlite_order_items.order_id = xlite_orders.order_id " +
											"AND xlite_orders.profile_id = " + profileID;

						command = new OdbcCommand(subQuery, con);
						reader = command.ExecuteReader();

						while (reader.Read())
						{
							DataItem item = new DataItem(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2));
							Console.WriteLine("Order ID: {0}, ProductID: {1}, Amount: {2}", item.OrderID, item.ProductID, item.Amount);
							result.DataItems.Add(item);

						}
						reader.Close();
						if(result.DataItems.Count > 0)
							result.Status = ResultStatus.OK;
						else
							result.Status = ResultStatus.NoDonationsFound;
					}
					else
					{
						result.Status = ResultStatus.ProfileNotFound;
					}

				*/
				}
				catch (Exception e)
				{
					result.Status = ResultStatus.DatabaseError;
					Console.WriteLine("Error in class DonationChecker : {0}", e.Message);
				}
				finally
				{
					if (con != null)
						con.Close();
				}

				Monitor.Enter(m_ResultQueue);
				m_ResultQueue.Enqueue(result);
				Monitor.Exit(m_ResultQueue);

				Console.WriteLine("End of thread function");
			}
		}

		#region DataBase result datamembers and classes
		private enum ResultStatus
		{
			Unresolved = 0,
			OK,
			NoDonationsFound,
			NoUndeliveredDonationsFound,
			DatabaseError
		}

		private class DatabaseResult
		{
			private Mobile m_Mobile;
			private ArrayList m_DataItems;
			private ResultStatus m_Status;

			public DatabaseResult(Mobile m)
			{
				m_Mobile = m;
				m_DataItems = new ArrayList();
			}

			public Mobile Mobile
			{
				get { return m_Mobile; }
				set { m_Mobile = value; }
			}

			public ResultStatus Status
			{
				get { return m_Status; }
				set { m_Status = value; }
			}

			public ArrayList DataItems
			{
				get { return m_DataItems; }
				set { m_DataItems = value; }
			}
		}
		private class DataItem
		{
			private int m_OrderID;
			private int m_ProductID;
			private int m_Amount;

			public DataItem(int orderID, int productID, int amount)
			{
				this.m_OrderID = orderID;
				this.m_ProductID = productID;
				this.m_Amount = amount;
			}
			public int OrderID
			{
				get { return m_OrderID; }
//				set { m_OrderID = value; }
			}

			public int ProductID
			{
				get { return m_ProductID; }
//				set { m_ProductID = value; }
			}

			public int Amount
			{
				get { return m_Amount; }
//				set { m_Amount = value; }
			}
		}
		private static ArrayList GetUnclaimed(ArrayList dataItems)
		{
			ArrayList toClaim = new ArrayList();

			foreach (DataItem item in dataItems)
			{
				if (!AlreadyClaimed(item.OrderID))
					toClaim.Add(item);
			}

			return toClaim;
		}
		private static bool AlreadyClaimed(int orderNum)
		{
			foreach (ClaimedOrder order in m_ClaimedOrders)
			{
				if (order.OrderID == orderNum)
					return true;
			}
			return false;
		}
		#endregion

		private static void Log(string error)
		{
		}
		private class ClaimedOrder
		{
			// Constructor for deserilization
			public ClaimedOrder()
			{
				// perhaps some kind of initialization
			}

			private int m_OrderID;
			private DateTime m_DeliverTime;
			private string m_DeliverAccountName;
			private string m_DeliverMobileName;
			private Mobile m_DeliverMobile;

			public ClaimedOrder(int order, Mobile mobile)
			{
				this.m_OrderID = order;
				this.m_DeliverTime = DateTime.Now;
				this.m_DeliverMobile = mobile;
				this.m_DeliverAccountName = (mobile.Account as Account).Username;
				this.m_DeliverMobileName = mobile.RawName;
			}

			public int OrderID
			{
				get { return m_OrderID; }
//				set { m_OrderID = value; }
			}

			public DateTime DeliverTime
			{
				get { return m_DeliverTime; }
//				set { m_DeliverTime = value; }
			}
			public Mobile DeliverMobile
			{
				get { return m_DeliverMobile; }
//				set { m_DeliverMobile = value; }
			}

			public string DeliverAccountName
			{
				get { return m_DeliverAccountName; }
//				set { m_DeliverTime = value; }
			}

			public string DeliverMobileName
			{
				get { return m_DeliverMobileName; }
//				set { m_DeliverTime = value; }
			}

			public void Serialize(GenericWriter writer)
			{
				writer.Write((int)0);
				writer.Write(m_OrderID);
				writer.Write(m_DeliverTime);
				writer.Write(m_DeliverMobile);
				writer.Write(m_DeliverAccountName);
				writer.Write(m_DeliverMobileName);
			}

			public void Deserialize(GenericReader reader)
			{
				int version = reader.ReadInt();
				m_OrderID = reader.ReadInt();
				m_DeliverTime = reader.ReadDateTime();
				m_DeliverMobile = reader.ReadMobile();
				m_DeliverAccountName = reader.ReadString();
				m_DeliverMobileName = reader.ReadString();
			}
		}

		private class ClaimDonationsGump : Gump
		{
			private DatabaseResult m_Result;
			private int m_Page;

			public ClaimDonationsGump(int page, int x, int y, DatabaseResult result) : base( x, y )
			{
				this.m_Result = result;
				this.m_Page = page;
				this.Closable = true;
				this.Disposable = true;
				this.Dragable = true;
				this.Resizable = false;
				this.AddPage(0);

				this.AddBackground(0, 0, 400, 400, 9380);

				this.AddLabel(100, 5, 1259, @"Welcome to Defiance Donation");

				if (m_Result.Status == ResultStatus.Unresolved)
				{
					this.AddLabel(100, 100, 1259, @"An unresolved error occured trying");
					this.AddLabel(100, 120, 1259, @"to claim your rewards. If you keep");
					this.AddLabel(100, 140, 1259, @"getting this message, please contact");
					this.AddLabel(100, 160, 1259, @"an administrator.");
					this.AddLabel(100, 200, 1259, @"Sorry for the inconvenience");
					this.AddButton(170, 340, 241, 242, (int)Buttons.Cancel, GumpButtonType.Reply, 0);
				}
				else if (m_Result.Status == ResultStatus.DatabaseError)
				{
					this.AddLabel(100, 100, 1259, @"A database error occured trying");
					this.AddLabel(100, 120, 1259, @"to claim your rewards. If you keep");
					this.AddLabel(100, 140, 1259, @"getting this message, please contact");
					this.AddLabel(100, 160, 1259, @"an administrator.");
					this.AddLabel(100, 200, 1259, @"Sorry for the inconvenience");
					this.AddButton(170, 340, 241, 242, (int)Buttons.Cancel, GumpButtonType.Reply, 0);
				}
				else if (m_Result.Status == ResultStatus.NoDonationsFound)
				{
					this.AddLabel(100, 60, 1259, @"The database could not find any");
					this.AddLabel(100, 80, 1259, @"donations tied to this account.");
					this.AddLabel(100, 100, 1259, @"If you believe this is a mistake,");
					this.AddLabel(100, 120, 1259, @"please contact an administrator.");
					this.AddLabel(100, 140, 1259, @"Remember that orders are tied to");
					this.AddLabel(100, 160, 1259, @"the account specified in the cart-");
					this.AddLabel(100, 180, 1259, @"system profile, at order time.");
					this.AddLabel(100, 200, 1259, @"If you have changed the account");
					this.AddLabel(100, 220, 1259, @"in your profile since your last");
					this.AddLabel(100, 240, 1259, @"order, it will not be claimable");
					this.AddLabel(100, 260, 1259, @"from this account.");

					this.AddLabel(100, 300, 1259, @"Sorry for the inconvenience");
					this.AddButton(170, 340, 241, 242, (int)Buttons.Cancel, GumpButtonType.Reply, 0);
				}
				else if (m_Result.Status == ResultStatus.NoUndeliveredDonationsFound)
				{
					this.AddLabel(100, 60, 1259, @"No unclaimed donations rewards");
					this.AddLabel(100, 80, 1259, @"could be found for this account.");
					this.AddLabel(100, 100, 1259, @"If you believe this is a mistake,");
					this.AddLabel(100, 120, 1259, @"please contact an administrator.");
					this.AddLabel(100, 140, 1259, @"Remember that orders are tied to");
					this.AddLabel(100, 160, 1259, @"the account specified in the cart-");
					this.AddLabel(100, 180, 1259, @"system profile, at order time.");
					this.AddLabel(100, 200, 1259, @"If you have changed the account");
					this.AddLabel(100, 220, 1259, @"in your profile since your last");
					this.AddLabel(100, 240, 1259, @"order, it will not be claimable");
					this.AddLabel(100, 260, 1259, @"from this account.");

					this.AddLabel(100, 300, 1259, @"Sorry for the inconvenience");
					this.AddButton(170, 340, 241, 242, (int)Buttons.Cancel, GumpButtonType.Reply, 0);
				}
				else if (m_Result.Status == ResultStatus.OK && page >= 0)
				{
					if (page > 0)
					{
						this.AddLabel(60, 375, 1149, @"Prev page");
						this.AddButton(40, 380, 9706, 9707, (int)Buttons.Previous, GumpButtonType.Reply, 0);
					}
					if (m_Result.DataItems.Count > page * 10 + 10)
					{
						this.AddLabel(270, 375, 1149, @"Next page");
						this.AddButton(330, 380, 9702, 9703, (int)Buttons.Next, GumpButtonType.Reply, 0);
					}

					this.AddLabel(40, 40, 1259, @"Congratulations, you have unclaimed donation rewards!!");
					this.AddLabel(40, 80, 1259, @"OrderID:");
					this.AddLabel(100, 80, 1259, @"Item Description:");
					this.AddLabel(320, 80, 1259, @"Amount:");

					int j = 0;
					for (int i = page * 10; i < m_Result.DataItems.Count && j < 10; i++)
					{
						int productID = ((DataItem)m_Result.DataItems[i]).ProductID;
						int orderID = ((DataItem)m_Result.DataItems[i]).OrderID;
						int amount = ((DataItem)m_Result.DataItems[i]).Amount;

						this.AddLabel(50, 100 + j * 20, 1149, @"" + orderID);
						this.AddLabel(100, 100 + j * 20, 1149, @"" + GetDescription(productID));
						this.AddLabel(320, 100 + j * 20, 1149, @"" + amount);

						j++;
					}
					this.AddLabel(80, 320, 1259, @"Claim these rewards with this character?");

					this.AddButton(120, 340, 247, 248, (int)Buttons.OK, GumpButtonType.Reply, 0);
					this.AddButton(220, 340, 241, 242, (int)Buttons.Cancel, GumpButtonType.Reply, 0);
				}
			}
			public enum Buttons
			{
				Cancel = 0,
				Previous = 1,
				Next = 2,
				OK = 3,
			}

			public override void OnResponse(NetState state, RelayInfo info)
			{
				if (state == null || state.Mobile == null)
					return;

				m_Result.DataItems = GetUnclaimed(m_Result.DataItems);

				if (m_Result.Status == ResultStatus.OK && m_Result.DataItems.Count == 0)
				{
					m_Result.Status = ResultStatus.NoUndeliveredDonationsFound;
				}

				if (info.ButtonID == (int)Buttons.Previous && m_Page > 0)
				{
					state.Mobile.SendGump(new ClaimDonationsGump(m_Page - 1, this.X, this.Y, m_Result));
				}
				else if (info.ButtonID == (int)Buttons.Next)
				{
					state.Mobile.SendGump(new ClaimDonationsGump(m_Page + 1, this.X, this.Y, m_Result));
				}
				else if (info.ButtonID == (int)Buttons.OK && m_Result.Status == ResultStatus.OK)
				{

					state.Mobile.CloseAllGumps();
					if (state.Mobile != null && !state.Mobile.Deleted && state.Mobile.NetState != null)
					{
						if (state.Mobile.BankBox == null)
						{
							state.Mobile.SendMessage("You don't seem to have a bankbox, contact a GM.");
						}
						else
						{

							ArrayList temp = new ArrayList();
							Bag bag = new Bag();
							bag.Hue = 33;
							bag.Name = "a donation claim bag";

							foreach (DataItem item in m_Result.DataItems)
							{
								//Make sure we check amount
								for (int i = 0; i < item.Amount; i++)
								{
									Item toGive = GetItem(item.ProductID);
									if (toGive != null)
									{
										bag.DropItem(toGive);
									}
									else
									{
										state.Mobile.SendMessage("An error ocurred claiming an item in order number: {0}. An errorlog has been created, please contact an administrator.");
										string error = String.Format("An error ocurred trying to fetch item for product number: {0} in order: {1} for {2}({3}).",
																	item.ProductID, item.OrderID, state.Mobile.RawName, (state.Mobile.Account as Account).Username);
										Log(error);
									}
								}

								// Register claim. We only register each order one time
								if (!temp.Contains(item.OrderID))
								{
									temp.Add(item.OrderID);
									ClaimedOrder claim = new ClaimedOrder(item.OrderID, state.Mobile);
									m_ClaimedOrders.Add(claim);
								}

							}
							state.Mobile.BankBox.DropItem(bag);
							state.Mobile.SendMessage("You have claimed your donations. They have been added to your bankbox. Thank you for donating!");
						}
					}
				}
				else
				{
					//state.Mobile.SendMessage("You could not claim the donations, because you claimed them wile this gump was open");
				}
			}
		}
	}
}