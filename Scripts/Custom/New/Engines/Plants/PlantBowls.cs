using System;
using Server.Targeting;
using Server.Gumps;
using Server.Mobiles;

using Server.Network;

namespace Server.Items
{
	public class EmptyPlantBowl : Item
	{
		private int m_Dirt;

		private static int[] m_DirtIDs = new int[]
			{
			0x9, 0x15,
			0x71, 0xa7,
			0xdc, 0xeb,
			0x141, 0x15c,
			0x169, 0x174,
			0x1dc, 0x1ef,
			0x272, 0x275,
			0x27e, 0x281,
			0x2d0, 0x31f,
			0x32c, 0x32f,
			0x355, 0x358,
			0x367, 0x36e,
			0x3a5, 0x3a8,
			0x547, 0x556,
			0x597, 0x59e,
			0x623, 0x63a,
			0x777, 0x791
			};

		public override int LabelNumber{ get{return 1060834;} } // a plant bowl
		[Constructable]
		public EmptyPlantBowl() : base(0x15fd)
		{
			Weight = 5;
		}

		public override void OnDoubleClick( Mobile from )
		{
		from.SendLocalizedMessage( 1053084 );//Choose a patch of dirt to scoop up.
		from.Target = new InternalTarget(this);
		}

		private class InternalTarget : Target
		{
			private EmptyPlantBowl m_pot;
			public InternalTarget(EmptyPlantBowl Pot) : base( 6, true, TargetFlags.None )
			{
			m_pot = Pot;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				int itemID;

				if  ( o is LandTarget )
					{
					itemID = from.Map.Tiles.GetLandTile( ((LandTarget)o).X, ((LandTarget)o).Y ).ID & 0x3FFF;

					for ( int i = 0; i < m_DirtIDs.Length; i += 2 )
						{
						if ( itemID >= m_DirtIDs[i] && itemID <= m_DirtIDs[i + 1] )
							{
							from.SendLocalizedMessage( 1053082 ); // You fill the bowl with fresh dirt.
							FullPlantBowl pf = new FullPlantBowl();
							pf.Owner = from;
							from.AddToBackpack( pf );
							m_pot.Delete();
							}
						}
					}
				else if ( o is FertileDirt )
					{
					Item ft = o as Item;
					if ( from.Backpack.GetAmount( typeof( FertileDirt )) >= 40 )
						{
						ft.Consume(40);
						FullPlantBowl fpot = new FullPlantBowl();
						fpot.Fertile = true;
						fpot.Owner = from;
						from.AddToBackpack( fpot );
						from.SendLocalizedMessage( 1053082 ); // You fill the bowl with fresh dirt.
						m_pot.Delete();
						}
					else if (ft.RootParent != from)
						from.SendLocalizedMessage( 1042038 ); // You must have the object in your backpack to use it.
					else
						from.SendLocalizedMessage( 1053083 ); // You need more dirt to fill a plant bowl!

					}
				else
					{
					from.SendLocalizedMessage( 1053080 );// You'll want to gather fresh dirt in order to raise a healthy plant!
					}

			}

		}

		public EmptyPlantBowl(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class FullPlantBowl : Item
	{
		private int m_SeedType, m_SeedColor, m_Stage, m_Water, m_Funghi, m_Insect, m_Poison, m_Disease, m_Hits, m_Max, m_GSP, m_GPP, m_GCP, m_GHP, m_Grown;
		private int m_ChildColor, m_SeedCounter, m_ChildType, m_ResourceType, m_ResourceCounter, m_RMax, m_SMax, m_SStage;
		private bool m_Pollenated, m_FiftyFifty, m_Named, m_Fertile;
		private Mobile m_Owner;
		private DateTime m_NextCheck;

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime NextCheck{ get{ return m_NextCheck; } set{ m_NextCheck = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner{ get{ return m_Owner; } set{ m_Owner = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int SMax{ get{ return m_SMax; } set{ m_SMax = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int RMax{ get{ return m_RMax; } set{ m_RMax = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int ResourceCounter{ get{ return m_ResourceCounter; } set{ m_ResourceCounter = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int ResourceType{ get{ return m_ResourceType; } set{ m_ResourceType = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int ChildType{ get{ return m_ChildType; } set{ m_ChildType = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int SeedCounter{ get{ return m_SeedCounter; } set{ m_SeedCounter = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int ChildColor{ get{ return m_ChildColor; } set{ m_ChildColor = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int SeedType{ get{ return m_SeedType; } set{ m_SeedType = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int SeedColor{ get{ return m_SeedColor; } set{ m_SeedColor = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int SStage{ get{ return m_SStage; } set{ m_SStage = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Stage{ get{ return m_Stage; } set{ m_Stage = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Water{ get{ return m_Water; } set{ m_Water = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Funghi{ get{ return m_Funghi; } set{ m_Funghi = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Insect{ get{ return m_Insect; } set{ m_Insect = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Poison{ get{ return m_Poison; } set{ m_Poison = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Disease{ get{ return m_Disease; } set{ m_Disease = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Hits{ get{ return m_Hits; } set{ m_Hits = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Max{ get{ return m_Max; } set{ m_Max = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int GSP{ get{ return m_GSP; } set{ m_GSP = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int GPP{ get{ return m_GPP; } set{ m_GPP = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int GCP{ get{ return m_GCP; } set{ m_GCP = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int GHP{ get{ return m_GHP; } set{ m_GHP = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Grown{ get{ return m_Grown; } set{ m_Grown = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Pollenated{ get{ return m_Pollenated; } set{ m_Pollenated = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool FiftyFifty{ get{ return m_FiftyFifty; } set{ m_FiftyFifty = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Named{ get{ return m_Named; } set{ m_Named = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Fertile{ get{ return m_Fertile; } set{ m_Fertile = value; } }

		public FullPlantBowl() : base(0x1602)
		{
			if (!Core.AOS)
				Name = "a bowl of hard dirt";
			Weight = 1;
			SeedType = -9;
			Water = -2;
			Hits = 10;
			Max = 10;
			ChildType = -9;
			Fertile = false;
			SMax = 8;
			FiftyFifty = false;
		}


		private string Waterlvl
		{
		get{
		if (Water < 0)
		return "#1060826";
		else if (Water == 0)
		return "#1060827";
		else if (Water == 1)
		return "#1060828";
		else
		return "#1060829";}
		}


		private string Health
		{
		get{
		double health = (double) Hits / Max  ;
		if ( health >= 0.75 )
			return "#1060822" ; // vibrant
		else if ( health >= 0.5 )
			return "#1060823" ; // healthy
		else if ( health >= 0.25 )
			return "#1060824" ; // wilted
		else
			return "#1060825" ; // dying
		}
		}


		public override void AddNameProperty( ObjectPropertyList list )
		{
		if (m_SeedType == -9)
			list.Add(1060830, Waterlvl);
		else if (Stage < 7 && !Named)
			{
			string plantname = Waterlvl + "\t" + Health + "\t" + PlantProps.Color(SeedColor) + "\t" + PlantProps.Stages(Stage);
			if (PlantProps.Bright(SeedColor))
				list.Add(1060832, plantname);
			else
				list.Add(1060831, plantname);
			}
		else if (Stage < 7 && Named)
			{
			string plantname = Waterlvl + "\t" + Health + "\t" + PlantProps.Color(SeedColor) + "\t" + PlantProps.PlantName(SeedType) + "\t" + PlantProps.Stages(Stage);
			if (PlantProps.Bright(SeedColor))
				list.Add(1061887, plantname);
			else
				list.Add(1061888, plantname);
			}
		else
			{
			if (PlantProps.Bright(SeedColor))
				list.Add(1061892, Health + "\t" + PlantProps.Color(SeedColor) + "\t" + PlantProps.PlantName(SeedType));
			else
				list.Add(1061890, Health + "\t" + PlantProps.Color(SeedColor) + "\t" + PlantProps.PlantName(SeedType));
			}
		}

		public override bool IsAccessibleTo( Mobile check )
		{
		if (check == Owner)
			return true;
		return base.IsAccessibleTo(check);
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.SendGump( new PlantStatMenu( from, this ) );
		}

		public override bool CheckItemUse( Mobile from, Item item )
		{
		if (item != this)
			return base.CheckItemUse(from, item);
		bool access = false;
		if (from == Owner || from == this.RootParent)
			access = true;
		else if ( this.Movable && from != this.RootParent )
			from.SendLocalizedMessage( 1053046 ); // You must have the item in your backpack or locked down in order to use it.
		else
			from.SendLocalizedMessage( 501652 ); // You cannot use this unless you are the owner.
		return access;
		}

		public override void OnSecureTrade( Mobile from, Mobile to, Mobile newOwner, bool accepted )
		{
		if (accepted)
			Owner = newOwner;
		base.OnSecureTrade( from, to, newOwner, accepted );
		}

		public virtual void Start( Mobile from, FullPlantBowl item, TimeSpan time )
		{
		new PlantTimer(from, item, time).Start();
		}

		private class PlantTimer : Timer
		{
			private Mobile from;
			private FullPlantBowl pot;

			public PlantTimer( Mobile m_from, FullPlantBowl item, TimeSpan time) : base( time, PlantProps.GrowthCheck )
			{
				from = m_from;
				pot = item;
			}

			protected override void OnTick()
			{
			pot.NextCheck = DateTime.Now + PlantProps.GrowthCheck;
			if (pot.Deleted)
				Stop();
			if (from == null)
				{
				pot.Delete();
				Stop();
				}
			PlayerMobile pm = null;
			bool contcheck = false;
			if (pot.RootParent is PlayerMobile)
				pm = (PlayerMobile)pot.RootParent;
			if (pm != null)
				contcheck = ( pot.IsChildOf(pm.Backpack) || pot.IsChildOf(pm.BankBox));
			else
				contcheck = (!pot.Movable && pot.Parent == null);
			if ( !contcheck )
				pot.Grown = 4; // Invalid Location for growth
			else
				{
				if ( pot.CheckHealth( pot ) )
					{
					if ( pot.Hits > ( pot.Max / 2 ) )
						{
						pot.Grown = 2;
						if ( pot.Fertile && pot.Stage < 6 )
							{
							if ( Utility.RandomDouble() < 0.10 )
								{
								pot.Grown = 3;
								pot.Stage++;
								pot.Max = pot.Max + 10;
								pot.Hits = pot.Hits + 10;
								}
							}

						if ( pot.Stage < 9 )
							{
							pot.Stage++;
							pot.Max = pot.Max + 10;
							pot.Hits = pot.Hits + 10;
							if ( pot.Hits > pot.Max )
								pot.Hits = pot.Max;
							}
						}
					else
						{
						pot.Grown = 1;
						return;
						}
					}
				else
					{
					if (pot.Stage > 6)
						{
						Item twigs = new Item(7069);
						twigs.Name = "dead twigs";
						twigs.Hue = pot.Hue;
						if (pot.Parent == null)
							twigs.MoveToWorld ( pot.Location, pot.Map );
						else
							from.AddToBackpack(twigs);
						pot.Delete();
						Stop();
						}
					else
						{
						Item epot = new EmptyPlantBowl();
						if (pot.Parent == null)
							epot.MoveToWorld ( pot.Location, pot.Map );
						else
							from.AddToBackpack(epot);
						pot.Delete();
						Stop();
						}
					}

				if ( pot.Stage == 4 )
					pot.ItemID = 0x1600;//Bowl of Lettuce
				if ( pot.Stage == 7 )
				{
				switch ( pot.SeedType )
					{
					case -8: pot.Hue = pot.SeedColor; pot.ItemID = 0xC83; break;
					case -7: pot.Hue = pot.SeedColor; pot.ItemID = 0xC86; break;
					case -6: pot.Hue = pot.SeedColor; pot.ItemID = 0xC88; break;
					case -5: pot.Hue = pot.SeedColor; pot.ItemID = 0xC94; break;
					case -4: pot.Hue = pot.SeedColor; pot.ItemID = 0xC8B; break;
					case -3: pot.Hue = pot.SeedColor; pot.ItemID = 0xCA5; break;
					case -2: pot.Hue = pot.SeedColor; pot.ItemID = 0xCA7; break;
					case -1: pot.Hue = pot.SeedColor; pot.ItemID = 0xC97; break;
					case 0: pot.Hue = pot.SeedColor; pot.ItemID = 0xC9F; break;
					case 1: pot.Hue = pot.SeedColor; pot.ItemID = 0xCA6; break;
					case 2: pot.Hue = pot.SeedColor; pot.ItemID = 0xC9C; break;
					case 3: pot.Hue = pot.SeedColor; pot.ItemID = 0xD31; break;
					case 4: pot.Hue = pot.SeedColor; pot.ItemID = 0xD04; break;
					case 5: pot.Hue = pot.SeedColor; pot.ItemID = 0xCA9; break;
					case 6: pot.Hue = pot.SeedColor; pot.ItemID = 0xD2C; break;
					case 7: pot.Hue = pot.SeedColor; pot.ItemID = 0xD26; break;
					case 8: pot.Hue = pot.SeedColor; pot.ItemID = 0xD27; break;
					}
				}
				if ( pot.SStage == 9 )
					{
					Item twigs = new Item(7069);
					twigs.Name = "dead twigs";
					twigs.Hue = pot.Hue;
					if (pot.Parent == null)
						twigs.MoveToWorld ( pot.Location, pot.Map );
					else
						from.AddToBackpack(twigs);
					pot.Delete();
					Stop();
					}

				if ( pot.Stage == 9 )
					{
					if (!pot.Pollenated)
						pot.Pollenated = true;
					if (pot.SStage > 0)
						{
						if ( pot.ChildType == -9 )
							{
							pot.ChildType = pot.SeedType;
							pot.ChildColor = pot.SeedColor;
							}
						if ( pot.SMax > 0 )
							{
							pot.SMax--;
							if ( pot.RMax > 0 )
								pot.RMax--;
							if ( pot.Hits > ( pot.Max / 2 ) )
								{
								pot.SeedCounter++;
								if ( pot.ResourceType > 0 )
									pot.ResourceCounter++;
								}
							}
						}
					pot.SStage++;
					}
				}
			}
		}

		public virtual bool CheckHealth(FullPlantBowl pot)
		{
		bool healthy = true;
		if ( pot.Hits > 0 )
			{
			//--------Check for cure against insects and for posion--------
			if ( (pot.GPP - pot.Insect) == 0 )
				{
				pot.GPP = 0;
				pot.Insect = 0;
				}
			else if ( (pot.GPP - pot.Insect) < 0 )
				{
				pot.Insect = pot.Insect - pot.GPP;
				pot.GPP = 0;
				healthy = false;
				}
			else
				{
				pot.Poison = pot.Poison + ( pot.GPP - pot.Insect );
				pot.GPP = 0;
				pot.Insect = 0;
				}
			//--------Check for cure against fungi and for disease--------
			if ( (pot.GCP - pot.Funghi) == 0 )
				{
				pot.GCP = 0;
				pot.Funghi = 0;
				}
			else if ( (pot.GCP - pot.Funghi) < 0)
				{
				pot.Funghi= pot.Funghi - pot.GPP;
				pot.GCP = 0;
				healthy = false;
				}
			else
				{
				pot.Disease = pot.Disease + ( pot.GCP - pot.Funghi );
				pot.GCP = 0;
				pot.Funghi = 0;
				}
			//--------Check for cure against disease and poison with heal potions-----
			if ( (pot.GHP - pot.Disease ) == 0 )
				{
				pot.GHP = 0;
				pot.Disease = pot.Disease;
				}
			else if ( (pot.GHP - pot.Disease ) < 0 )
				{
				pot.Disease = pot.Disease - pot.GHP;
				if (  pot.Disease > 2 )
					pot.Disease = 2;
				pot.GHP = 0;
				healthy = false;
				}
			else
				{
				pot.GHP = pot.GHP - pot.Disease;
				pot.Disease = pot.Disease;
				}
			if ( ( pot.GHP - pot.Poison ) == 0 )
				{
				pot.GHP = 0;
				pot.Poison = pot.Poison;
				}
			else if ( ( pot.GCP - pot.Poison ) < 0 )
				{
				pot.Poison = pot.Poison - pot.GHP ;
				if ( pot.Poison > 2 )
					pot.Poison = 2;
				pot.GHP = 0;
				healthy = false;
				}
			else
				{
				pot.GHP = pot.GHP - pot.Poison;
				pot.Poison = pot.Poison;
				}
			//--------Check for healthy plant and add strength potions and heal------
			if ( healthy )
				{
				pot.Max = pot.Max + ( pot.GSP * 2 );
				pot.Hits = pot.Hits + ( pot.GHP * 5 );
				pot.GSP = 0;
				pot.GHP = 0;
				}
			else
				{
				pot.Max = pot.Max + ( pot.GSP * 1 );
				pot.Hits = pot.Hits + ( pot.GHP * 3 );
				pot.GSP = 0;
				pot.GHP = 0;
				}

			if ( pot.Funghi > 0 )
				pot.Hits = pot.Hits - ( 5 * pot.Funghi );// Lose 5 hitpoints per level of funghi
			if ( pot.Insect > 0 )
				pot.Hits = pot.Hits - ( 5 * pot.Insect );// Lose 5 hitpoints per level of insect infestation
			if ( pot.Poison > 0 )
				pot.Hits = pot.Hits - ( 7 * pot.Poison );// Lose 7 hitpoints per level of poison
			if ( pot.Disease > 0 )
				pot.Hits = pot.Hits - ( 15 * pot.Disease );// Lose 15 hitpoints per level of disease
			if ( pot.Water < 0 )
				pot.Hits = pot.Hits - ( pot.Water * -5 );// Lose 5 hitpoints per level of dihydration
			if ( pot.Hits < 1 )
				{
				pot.Grown = 1;
				return false;
				}

			if ( pot.Funghi == 0)
				{
				if ( Utility.RandomMinMax( (pot.Water * 3), 20 ) > 13 )	// 35% chance of fungi. Chances of fungi are greater if water level is high
					pot.Funghi++; //Plant get a fungi infestation
				}
			else
				pot.Funghi = 2;

			if (  pot.Insect == 0 )
				{
				if ( Utility.RandomMinMax( (pot.Water * 3), 20 ) > (PlantProps.Bright(pot.SeedColor) ? 12 : 14) ) //30% Chance non bright colors, 40% bright colors. Chances of insects are greater if water level is high
					pot.Insect++; //Plant get an insect infestation
				}
			else
				pot.Insect = 2;
			if (PlantProps.FatPlant(pot.SeedType))
				{
				if (Utility.RandomDouble() > 0.30)
					pot.Water--;
				}
			else
				pot.Water--;
			return true;
			}
		else
			{
			pot.Grown = 1;
			return false;
			}

		}

		public FullPlantBowl(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 4);

			writer.Write( m_Fertile );
			writer.Write( m_Named );
			writer.Write( m_NextCheck );
			writer.Write( m_SStage );
			writer.Write( m_Owner );
			writer.Write( m_ChildType );
			writer.Write( m_ResourceType );
			writer.Write( m_ResourceCounter );
			writer.Write( m_RMax );
			writer.Write( m_SMax );
			writer.Write( m_ChildColor );
			writer.Write( m_SeedCounter );
			writer.Write( m_SeedColor );
			writer.Write( m_SeedType );
			writer.Write( m_Stage );
			writer.Write( m_Water );
			writer.Write( m_Funghi );
			writer.Write( m_Insect );
			writer.Write( m_Poison );
			writer.Write( m_Disease );
			writer.Write( m_Hits );
			writer.Write( m_Max );
			writer.Write( m_GSP );
			writer.Write( m_GPP );
			writer.Write( m_GCP );
			writer.Write( m_GHP );
			writer.Write( m_Grown );
			writer.Write( m_Pollenated );
			writer.Write( m_FiftyFifty );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch ( version )
			{
				case 4:
				{
				m_Fertile = reader.ReadBool();
				goto case 3;
				}
				case 3:
				{
				m_Named = reader.ReadBool();
				goto case 2;
				}
				case 2:
				{
				m_NextCheck = reader.ReadDateTime();
				goto case 1;
				}
				case 1:
				{
				m_SStage = (int)reader.ReadInt();
				goto case 0;
				}
				case 0:
				{
				m_Owner = reader.ReadMobile();
				m_ChildType = (int)reader.ReadInt();
				m_ResourceType = (int)reader.ReadInt();
				m_ResourceCounter = (int)reader.ReadInt();
				m_RMax = (int)reader.ReadInt();
				m_SMax = (int)reader.ReadInt();
				m_ChildColor = (int)reader.ReadInt();
				m_SeedCounter = (int)reader.ReadInt();
				m_SeedColor = (int)reader.ReadInt();
				m_SeedType = (int)reader.ReadInt();
				m_Stage = (int)reader.ReadInt();
				m_Water = (int)reader.ReadInt();
				m_Funghi = (int)reader.ReadInt();
				m_Insect = (int)reader.ReadInt();
				m_Poison = (int)reader.ReadInt();
				m_Disease = (int)reader.ReadInt();
				m_Hits = (int)reader.ReadInt();
				m_Max = (int)reader.ReadInt();
				m_GSP = (int)reader.ReadInt();
				m_GPP = (int)reader.ReadInt();
				m_GCP = (int)reader.ReadInt();
				m_GHP = (int)reader.ReadInt();
				m_Grown = (int)reader.ReadInt();
				m_Pollenated = reader.ReadBool();
				m_FiftyFifty = reader.ReadBool();
					break;
				}
			}
			if (m_Owner == null)
				Delete();
			else
			{
			if ( m_SeedType > -9 && m_Hits > 0 )
			{
			TimeSpan time = (TimeSpan)(NextCheck - DateTime.Now);
			if ( time < TimeSpan.Zero )
				Start(m_Owner, this, TimeSpan.Zero);
			else
				Start(m_Owner, this, time);
			}
			}
		}
	}

	public class DecorativePlant : Item
	{
		public override int LabelNumber{ get{ return 1061924; } } // a decorative plant
		[Constructable]
		public DecorativePlant(int id, int hue) : base( id )
		{
			Weight = 5;
			Hue = hue;
			Name = "a decorative plant";
			Movable = true;
			Stackable = false;
		}


		public DecorativePlant(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}