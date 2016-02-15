//
//    Name:    Dungeon Barrel  (aka Diablo Type Barrel)
//    Version: 1.01
//    Author:  RavonTUS
//
//    Play at An Nox the cure for the UO addiction
//    http://annox.no-ip.com      RavonTUS@Yahoo.com
//
//    Description: These barrels will only open if they are chopped up.
//                 You may get gold, mana, health, or you may get poisoned,
//                 blown up, or just an old broken barrel.
//
//    Distribution: This script can be freely distributed, as long as the
//                  credit notes are left intact.	This script can also be
//                  modified, as long as the credit notes are left intact.
//
//    Thanks to:    Joeku, Geezer & Axle for giving me a few pointers on my first script.
//
//    Change Log:   X-SirSly-X gave me the close barrel ID

using System;
using System.Collections;
using Server.Multis;
using Server.Mobiles;
using Server.Misc;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
    public class DungeonBarrelArea1 : Container, IChopable //,TrapableContainer
	{
		//public override int LabelNumber{ get{ return 1041064; } } // a trash barrel

		public override int DefaultMaxWeight{ get{ return 0; } } // A value of 0 signals unlimited weight

        //trying to kept it closed
        public override void OnDoubleClick( Mobile from)
        {
            from.SendLocalizedMessage(501747); // It appears to be locked.
        }

		public override int DefaultGumpID{ get{ return 0x3E; } }
		public override int DefaultDropSound{ get{ return 0x42; } }

		public override Rectangle2D Bounds
		{
			get{ return new Rectangle2D( 33, 36, 109, 112 ); }
		}

		[Constructable]
		public DungeonBarrelArea1() : base( 0xFAE) //0xE77 )
		{
			Name = "a strange barrel";  //change the name
            Hue = 1072;             //set the color
            //Locked = true;
            Movable = false;
		}

        public DungeonBarrelArea1(Serial serial) : base(serial)
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

		public void OnChop( Mobile from )
        //public override void OnDoubleClick(Mobile from) I'd rather had this work, oh well.
			{
                Effects.SendLocationEffect( Location, Map, 0x3728, 20, 10); //smoke or dust
                Effects.PlaySound( Location, Map, 573 );

                switch (Utility.Random(76))  //picks one of the following
                {
                    case 0:
			Effects.SendLocationEffect( from, from.Map, 0x113A, 20, 10 );
			from.PlaySound( 0x231 );
			from.LocalOverheadMessage( MessageType.Regular, 0x44, 1010523 ); // A toxic vapor envelops thee.
			from.ApplyPoison( from, Poison.Greater );
			break;
                    case 1:
			Effects.SendLocationEffect( from, from.Map, 0x3709, 30 );
			from.PlaySound( 0x54 );
			from.LocalOverheadMessage( MessageType.Regular, 0xEE, 1010524 ); // Searing heat scorches thy skin.
			AOS.Damage( from, from, Utility.RandomMinMax( 70, 90 ), 0, 100, 0, 0, 0 );
			break;
                    case 2:
                        new BarrelLid().MoveToWorld(Location, Map);
                        break;
                    case 3:
                        new BarrelHoops().MoveToWorld(Location, Map);
                        break;
                    case 4:
                        new BarrelStaves().MoveToWorld(Location, Map);
                        break;
                    case 5:
                        Gold g = new Gold(Utility.Random(500)); //Random amount of gold 0 - 500
                        g.MoveToWorld(Location, Map);
                        break;
                    case 6:
                        new NightSightPotion().MoveToWorld(Location, Map);
                        break;
                    case 7:
                        new GreaterCurePotion().MoveToWorld(Location, Map);
                        break;
                    case 8:
                        new TotalRefreshPotion().MoveToWorld(Location, Map);
                        break;
                    case 9:
                        new GreaterHealPotion().MoveToWorld(Location, Map);
                        break;
                    case 10:
                        new DarkIronWire().MoveToWorld(Location, Map);
                        break;
                    case 11:
                        new EnergyVortexScroll().MoveToWorld(Location, Map);
                        break;
                    case 12:
                        new SummonCreatureScroll().MoveToWorld(Location, Map);
                        break;
                    case 13:
                        new CreateFoodScroll().MoveToWorld(Location, Map);
                        break;
                    case 14:
                        new RecallScroll().MoveToWorld(Location, Map);
                        break;
                    case 15:
                        new CureScroll().MoveToWorld(Location, Map);
                        break;
                    case 16:
                        new GateTravelScroll().MoveToWorld(Location, Map);
                        break;
                    case 17:
                        new InvisibilityScroll().MoveToWorld(Location, Map);
                        break;
                    case 18:
                        new BlessScroll().MoveToWorld(Location, Map);
                        break;
                    case 19:
                        new PotionKeg().MoveToWorld(Location, Map);
                        break;
                    case 20:
                        new Bedroll().MoveToWorld(Location, Map);
                        break;
                    case 21:
                        new SturdyShovel().MoveToWorld(Location, Map);
                        break;
                    case 22:
                        new Slime().MoveToWorld(Location, Map);
                        break;
                    case 23:
                        new IDWand().MoveToWorld(Location, Map);
                        break;
                    case 24:
                        new SpecialHairDye().MoveToWorld(Location, Map);
                        break;
                    case 25:
                        new Torch().MoveToWorld(Location, Map);
                        break;
                    case 26:
                        new NameChangeDeed().MoveToWorld(Location, Map);
                        break;
                    case 27:
                        new Torso().MoveToWorld(Location, Map);
                        break;
                    case 28:
                        Gold n = new Gold(Utility.Random(5000)); //Random amount of gold 0 - 5000
                        n.MoveToWorld(Location, Map);
                        break;
                    case 29:
                        Gold m = new Gold(Utility.Random(300)); //Random amount of gold 0 - 3000
                        m.MoveToWorld(Location, Map);
                        break;
                    case 30:
                        Gold i = new Gold(Utility.Random(10000)); //Random amount of gold 0 - 10000
                        i.MoveToWorld(Location, Map);
                        break;
                    case 31:
			from.PlaySound( 0x223 );
			from.LocalOverheadMessage( MessageType.Regular, 0x62, 1010525 ); // Pain lances through thee from a sharp metal blade.
			AOS.Damage( from, from, Utility.RandomMinMax( 40, 70 ), 100, 0, 0, 0, 0 );
			break;
                    case 32:
			from.BoltEffect( 0 );
			from.LocalOverheadMessage( MessageType.Regular, 0xDA, 1010526 ); // Lightning arcs through thy body.
			AOS.Damage( from, from, Utility.RandomMinMax( 60, 80 ), 0, 0, 0, 0, 100 );
			break;
                    case 33:
			Effects.SendLocationEffect( from, from.Map, 0x113A, 20, 10 );
			from.PlaySound( 0x231 );
			from.LocalOverheadMessage( MessageType.Regular, 0x44, 1010523 ); // A toxic vapor envelops thee.
			from.ApplyPoison( from, Poison.Greater );
			break;
                    case 34:
			Effects.SendLocationEffect( from, from.Map, 0x113A, 20, 10 );
			from.PlaySound( 0x231 );
			from.LocalOverheadMessage( MessageType.Regular, 0x44, 1010523 ); // A toxic vapor envelops thee.
			from.ApplyPoison( from, Poison.Greater );
			break;
                    case 35:
                        new BarrelLid().MoveToWorld(Location, Map);
                        break;
                    case 36:
                        new BarrelStaves().MoveToWorld(Location, Map);
                        break;
                    case 37:
                        new BarrelLid().MoveToWorld(Location, Map);
                        break;
                    case 38:
			new Slime().MoveToWorld( Location, Map );
                        break;
                    case 39:
			new Slime().MoveToWorld( Location, Map );
                        break;
                    case 40:
			new Zombie().MoveToWorld( Location, Map );
                        break;
                    case 41:
			from.BoltEffect( 0 );
			from.LocalOverheadMessage( MessageType.Regular, 0xDA, 1010526 ); // Lightning arcs through thy body.
			AOS.Damage( from, from, Utility.RandomMinMax( 60, 80 ), 0, 0, 0, 0, 100 );
			break;
                    case 42:
			from.BoltEffect( 0 );
			from.LocalOverheadMessage( MessageType.Regular, 0xDA, 1010526 ); // Lightning arcs through thy body.
			AOS.Damage( from, from, Utility.RandomMinMax( 60, 80 ), 0, 0, 0, 0, 100 );
			break;
                    case 43:
                        new ContainerBones().MoveToWorld(Location, Map);
                        break;
                    case 44:
                        new FameIounStone().MoveToWorld(Location, Map);
                        break;
                    case 45:
                        new KarmaIounStone().MoveToWorld(Location, Map);
                        break;
                    case 46:
                        new AcidPool().MoveToWorld(Location, Map);
                        break;
                    case 47:
                        new DaemonGate().MoveToWorld(Location, Map);
                        break;
                    case 48:
                        new Diamond().MoveToWorld(Location, Map);
                        break;
                    case 49:
                        new Sapphire().MoveToWorld(Location, Map);
                        break;
                    case 50:
                        new Ruby().MoveToWorld(Location, Map);
                        break;
                    case 51:
                        new Amber().MoveToWorld(Location, Map);
                        break;
                    case 52:
                        new StarSapphire().MoveToWorld(Location, Map);
                        break;
                    case 53:
                        new Emerald().MoveToWorld(Location, Map);
                        break;
                    case 54:
                        new Amethyst().MoveToWorld(Location, Map);
                        break;
                    case 55:
                        new Tourmaline().MoveToWorld(Location, Map);
                        break;
                    case 56:
                        new Citrine().MoveToWorld(Location, Map);
                        break;
                    case 57:
                        new MessageInABottle().MoveToWorld(Location, Map);
                        break;
                    case 58:
			from.PlaySound( 0x223 );
			from.LocalOverheadMessage( MessageType.Regular, 0x62, 1010525 ); // Pain lances through thee from a sharp metal blade.
			AOS.Damage( from, from, Utility.RandomMinMax( 40, 90 ), 100, 0, 0, 0, 0 );
			break;
                    case 59:
			from.PlaySound( 0x223 );
			from.LocalOverheadMessage( MessageType.Regular, 0x62, 1010525 ); // Pain lances through thee from a sharp metal blade.
			AOS.Damage( from, from, Utility.RandomMinMax( 10, 90 ), 100, 0, 0, 0, 0 );
			break;
                    case 60:
			Effects.SendLocationEffect( from, from.Map, 0x3709, 30 );
			from.PlaySound( 0x54 );
			from.LocalOverheadMessage( MessageType.Regular, 0xEE, 1010524 ); // Searing heat scorches thy skin.
			AOS.Damage( from, from, Utility.RandomMinMax( 70, 90 ), 0, 100, 0, 0, 0 );
			break;
                    case 61:
			Effects.SendLocationEffect( from, from.Map, 0x3709, 30 );
			from.PlaySound( 0x54 );
			from.LocalOverheadMessage( MessageType.Regular, 0xEE, 1010524 ); // Searing heat scorches thy skin.
			AOS.Damage( from, from, Utility.RandomMinMax( 70, 90 ), 0, 100, 0, 0, 0 );
			break;
                    case 62:
			new Jwilson().MoveToWorld( Location, Map );
                        break;
                    case 63:
			new Jwilson().MoveToWorld( Location, Map );
                        break;
                    case 64:
			new Jwilson().MoveToWorld( Location, Map );
                        break;
                    case 65:
			new Jwilson().MoveToWorld( Location, Map );
                        break;
                    case 66:
			new Jwilson().MoveToWorld( Location, Map );
                        break;
                    case 67:
			new Jwilson().MoveToWorld( Location, Map );
                        break;
                    case 68:
			new Jwilson().MoveToWorld( Location, Map );
                        break;
                    case 69:
			new Jwilson().MoveToWorld( Location, Map );
                        break;
                    case 70:
			new Zombie().MoveToWorld( Location, Map );
                        break;
                    case 71:
                        new TreasureMap( 1, Map.Felucca ).MoveToWorld(Location, Map);
                        break;
                    case 72:
                        new TreasureMap( 2, Map.Felucca ).MoveToWorld(Location, Map);
                        break;
                    case 73:
                        new TreasureMap( 3, Map.Felucca ).MoveToWorld(Location, Map);
                        break;
                    case 74:
                        new TreasureMap( 4, Map.Felucca ).MoveToWorld(Location, Map);
                        break;
                    case 75:
                        new TreasureMap( 5, Map.Felucca ).MoveToWorld(Location, Map);
                        break;
                }
                Destroy();
			}
    }
}