using System;
using Server.Targeting;

namespace Server.Items
{
	public class baseSeed : Item
	{
		private int m_SeedType;
		public override int LabelNumber{ get{ return 1060838; } } // ~1_val~ seed

		[CommandProperty( AccessLevel.GameMaster )]
		public int SeedType{ get{ return m_SeedType; } set{ m_SeedType = value; InvalidateProperties();} }

		[Constructable]
		public baseSeed(int color) : this( color, -9 )
		{
		}

		[Constructable]
		public baseSeed(int color, int stype) : base(0x0DCF)
		{
		/* Creates a random SeedType which determines
		   what type of plant will grow from it.
		   The random SeedTypes only spawn 1st generation plants.
		   All other plant types are obtained thru cross pollenation.
		   1st Gen plants:
		   Campion Flower(-8), Fern(0), Tribarrel Cactus(8)

		   2nd Gen Plants:
		   Lilies(-4), Water Plant(4)

		   3rd Gen Plants:
		   Snow Drops(-6), Rushes(-2), Small Palm(2), Prickly Pear Cactus(6)

		   4th Gen Plants:
		   Poppies(-7), Bulrushes(-5), Pampas Grass(-3), Elephant Ear Plant(-1),
		   Ponytail Palm(1), Century Plant(3), Snake Plant(5), Barrel Cactus(7).

		   If you want to create a specific plant type
		   use the number next to the plant name as the SeedType
		   and use the baseSeed( color, SeedType ) ctor*/

			Hue = color;
			Weight = 1;
			if (!Core.AOS)
				Name = "a seed";
			if ( stype == -9 )
				SeedType = Utility.RandomList(-8, 0, 8); //Creates a random 1st generation seedtype
			else
				SeedType = stype;
		}

		public override void AddNameProperty( ObjectPropertyList list )
		{
		if (PlantProps.Bright(Hue))
			{
			list.Add(1060839, PlantProps.Color(Hue));
			}
		else
			{
			list.Add(1060838, PlantProps.Color(Hue));
			}
		}


		public override void OnSingleClick( Mobile from )
		{
		if (!Core.AOS)
			base.OnSingleClick(from);
		}

		public override bool CheckItemUse( Mobile from, Item item )
		{
		if (item != this)
			return base.CheckItemUse(from, item);
		if (from != this.RootParent)
			{
			from.SendLocalizedMessage( 1042038 ); // You must have the object in your backpack to use it.
			return false;
			}
		return base.CheckItemUse(from, item);
		}

		public override void OnDoubleClick( Mobile from )
		{
		SendLocalizedMessageTo( from, 1053037 );//Choose a bowl of dirt to plant this seed in.
		from.Target = new InternalTarget(this);
		}

		private class InternalTarget : Target
		{
			private baseSeed m_seed;
			public InternalTarget(baseSeed Seed) : base( 6, true, TargetFlags.None )
			{
			m_seed = Seed;
			}

			protected override void OnTarget( Mobile from, object o )
			{

				if  ( o is FullPlantBowl )
				{
					FullPlantBowl pot = o as FullPlantBowl;
					if ( pot.Owner == from )
					{
					if ( pot.Water >= 0 && pot.Stage == 0 && ((!pot.Movable && pot.Parent == null) || (pot.Movable && pot.IsChildOf(from.Backpack)) || (pot.Movable && pot.IsChildOf(from.BankBox)) ) ) //Makes sure it's soft dirt and it's in bank, backpack or lockeddown
					{
					// Resource producing plants:
					// Red Leaf: Bright Red Elephant Ear Plants(-1), Bright Red Ponytail Palms(1) and Bright Red Century Plants(3)
					// Orange Petals: Bright Orange Poppies(-7), Bright Orange Bulrushes(-5) and Bright Orange Pampas Grass(-3)
					// Green Thorn: Bright Green Barrel Cacti(7) and Bright Green Snake Plants(5)

					if ( (m_seed.Hue == (int)PlantColors.BrightRed && m_seed.SeedType == -1) || (m_seed.Hue == (int)PlantColors.BrightRed && m_seed.SeedType == 1) || (m_seed.Hue == (int)PlantColors.BrightRed && m_seed.SeedType == 3) )
						pot.ResourceType = 1;// Red Leaf
					if ( (m_seed.Hue == (int)PlantColors.BrightOrange && m_seed.SeedType == -7) || (m_seed.Hue == (int)PlantColors.BrightOrange && m_seed.SeedType == -5) || (m_seed.Hue == (int)PlantColors.BrightOrange && m_seed.SeedType == -3) )
						pot.ResourceType = 2;// Orange Petals
					if ( (m_seed.Hue == (int)PlantColors.BrightGreen && m_seed.SeedType == 5) || (m_seed.Hue == (int)PlantColors.BrightGreen && m_seed.SeedType == 7) )
						pot.ResourceType = 3;// Green Thorn
					if ( pot.ResourceType != 0 )
						pot.RMax = 8;
					if ( m_seed.Hue == (int)PlantColors.Black || m_seed.Hue == (int)PlantColors.White || m_seed.Hue == (int)PlantColors.RareMagenta || m_seed.Hue == (int)PlantColors.RareFireRed || m_seed.Hue == (int)PlantColors.RareAqua || m_seed.Hue == (int)PlantColors.RarePink )
						{
						pot.SMax = 0;
						pot.Pollenated = true;
						}
					pot.SeedType = m_seed.SeedType;
					pot.SeedColor = pot.ChildColor = m_seed.Hue;
					if (m_seed is NamedSeed)
						pot.Named = true;
					pot.Stage = 1;
					pot.Start( from, pot, PlantProps.GrowthCheck );
					m_seed.Delete();
					pot.SendLocalizedMessageTo(from, 1053041 );// You plant the seed in the bowl of dirt
					}
					else if ( (!pot.Movable && pot.Parent != null) || (pot.Movable && !pot.IsChildOf(from.Backpack) && !pot.IsChildOf(from.BankBox)) )
					{
					from.SendLocalizedMessage( 1053039 );// The bowl of dirt must be in your pack, or you must lock it down.
					}
					else if ( pot.Stage > 0 && pot.Stage < 7)
					{
					pot.SendLocalizedMessageTo( from, 1041522, "This bowl of dirt already has a " + "\t" + PlantProps.Stages(pot.Stage) + "\t" + " in it!" ); // ~1~~2~~3~
					}
					else if ( pot.Stage > 7)
					{
					from.SendLocalizedMessage( 1053038 );// You must use a seed on a bowl of dirt!
					}
					else
					{
					from.SendLocalizedMessage( 1053040 );// The dirt in this bowl needs to be softened first.
					return;
					}
					}
				}
				else
				{
				from.SendLocalizedMessage( 1053038 );// You must use a seed on a bowl of dirt!
				}
			}

		}

		public baseSeed(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
			writer.Write( m_SeedType );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
				m_SeedType = (int)reader.ReadInt();
				break;
				}
			}
		}
	}



public class NamedSeed : baseSeed
	{
		public override int LabelNumber{ get{ return 1061917; } } // ~1_COLOR~ ~2_TYPE~ seed

		[Constructable]
		public NamedSeed(int color, int stype) : base( color, stype ) //Creates a random primary color seed
		{
		SeedType = stype;
		}

		public override void AddNameProperty( ObjectPropertyList list )
		{
		if (!PlantProps.Bright(Hue))
			list.Add( 1061917, PlantProps.Color(Hue) + "\t" + PlantProps.PlantName( SeedType ) ); // ~1_COLOR~ ~2_TYPE~ seed
		else
			list.Add( 1061918, PlantProps.Color(Hue) + "\t" + PlantProps.PlantName( SeedType ) ); // bright ~1_COLOR~ ~2_TYPE~ seed
		}

		public NamedSeed(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
		}
	}

public class PSeed : baseSeed
	{
		/* Creates a random 1st gen primary color seed.
		These should be the only ones used as loot on
		seed carrying creatures (i.e. Boglings) */

		[Constructable]
		public PSeed() : base( 0, -9 )
		{
			Hue = Utility.RandomList( (int)PlantColors.Plain, (int)PlantColors.Red, (int)PlantColors.Blue, (int)PlantColors.Yellow);
			Weight = 1;
			if (!Core.AOS)
				Name = "a seed";
		}

		public PSeed(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
		}
	}

public class RSeed : baseSeed
	{
		/* Creates a random rare color seed.
		These should be the only ones used as reward for
		completeing the naturalist quest without the 5th solen hive report*/

		[Constructable]
		public RSeed() : base( 0, -9 )
		{
			Hue = Utility.RandomList( (int)PlantColors.RareAqua, (int)PlantColors.RarePink, (int)PlantColors.RareMagenta);
			Weight = 1;
			SeedType = Utility.Random(17) - 8;
			if (!Core.AOS)
				Name = "a seed";
		}

		public RSeed(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
		}
	}

public class FireSeed : baseSeed
	{
		/* Creates a random 1st gen primary color seed.
		These should be the only ones used as loot on
		seed carrying creatures (i.e. Boglings) */

		[Constructable]
		public FireSeed() : base( 0, -9 )
		{
			Hue = (int)PlantColors.RareFireRed;
			Weight = 1;
			SeedType = Utility.Random(17) - 8;
			if (!Core.AOS)
				Name = "a seed";
		}

		public FireSeed(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
		}
	}
}