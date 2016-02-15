//==============================================//
// Created by Dupre					//
//==============================================//
using System;
using System.Collections;
using Server;
using Server.Targeting;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	public class BlackShroud : HoodedShroudOfShadows
	{
		[Constructable]
		public BlackShroud()
		{
			Name = "a Shroud of Phasing";
			Hue = 1;
			LootType = LootType.Blessed;
		}

		public override bool OnEquip(Mobile from)
	      {
			Morph(from);
		         return base.OnEquip(from);
		}

	      public override void OnRemoved( object parent)
	      {
	        if (parent is Mobile)
	        {
	         Mobile from = (Mobile)parent;
		   UnMorph(from);
		  }

	         base.OnRemoved(parent);
      	}

      	public override void OnDoubleClick( Mobile from )
	      {
		from.SendMessage ("Only a True Black Knight may feel the full effect of this Shroud");
		}

		public bool Morph(Mobile from)
		{
	if (from.HueMod != 19999)
		{
			from.HueMod = 19999;
		}
		else
		{
		}

	if (this.Hue != 19999)
		{
			this.Hue = 19999;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.FirstValid) != null && from.FindItemOnLayer(Layer.FirstValid).Name == "Blackthorn's Blade")
		{
		from.FindItemOnLayer(Layer.FirstValid).Hue = 19999;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.TwoHanded) != null && from.FindItemOnLayer(Layer.TwoHanded).Name == "Black Knights Armour")
		{
		from.FindItemOnLayer(Layer.TwoHanded).Hue = 19999;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.Helm) != null && from.FindItemOnLayer(Layer.Helm).Name == "Black Knights Armour")
		{
		from.FindItemOnLayer(Layer.Helm).Hue = 19999;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.Shoes) != null && from.FindItemOnLayer(Layer.Shoes).Name == "Black Knights Armour")
		{
		from.FindItemOnLayer(Layer.Shoes).Hue = 19999;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.Pants) != null && from.FindItemOnLayer(Layer.Pants).Name == "Black Knights Armour")
		{
		from.FindItemOnLayer(Layer.Pants).Hue = 19999;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.Gloves) != null && from.FindItemOnLayer(Layer.Gloves).Name == "Black Knights Armour")
		{
		from.FindItemOnLayer(Layer.Gloves).Hue = 19999;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.Neck) != null && from.FindItemOnLayer(Layer.Neck).Name == "Black Knights Armour")
		{
		from.FindItemOnLayer(Layer.Neck).Hue = 19999;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.Arms) != null && from.FindItemOnLayer(Layer.Arms).Name == "Black Knights Armour")
		{
		from.FindItemOnLayer(Layer.Arms).Hue = 19999;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.InnerTorso) != null && from.FindItemOnLayer(Layer.InnerTorso).Name == "Black Knights Armour")
		{
		from.FindItemOnLayer(Layer.InnerTorso).Hue = 19999;
		}
		else
		{
		}

return true;
     	}

		public bool UnMorph(Mobile from)
		{
	if (from.Hue == 19999)
		{
			from.HueMod = -1;
		}
		else
		{
		}

	if (this.Hue == 19999)
		{
			this.Hue = 1;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.FirstValid) != null && from.FindItemOnLayer(Layer.FirstValid).Name == "Blackthorn's Blade")
		{
		from.FindItemOnLayer(Layer.FirstValid).Hue = 1;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.TwoHanded) != null && from.FindItemOnLayer(Layer.TwoHanded).Name == "Black Knights Armour")
		{
		from.FindItemOnLayer(Layer.TwoHanded).Hue = 1;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.Helm) != null && from.FindItemOnLayer(Layer.Helm).Name == "Black Knights Armour")
		{
		from.FindItemOnLayer(Layer.Helm).Hue = 1;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.Shoes) != null && from.FindItemOnLayer(Layer.Shoes).Name == "Black Knights Armour")
		{
		from.FindItemOnLayer(Layer.Shoes).Hue = 1;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.Pants) != null && from.FindItemOnLayer(Layer.Pants).Name == "Black Knights Armour")
		{
		from.FindItemOnLayer(Layer.Pants).Hue = 1;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.Gloves) != null && from.FindItemOnLayer(Layer.Gloves).Name == "Black Knights Armour")
		{
		from.FindItemOnLayer(Layer.Gloves).Hue = 1;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.Neck) != null && from.FindItemOnLayer(Layer.Neck).Name == "Black Knights Armour")
		{
		from.FindItemOnLayer(Layer.Neck).Hue = 1;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.Arms) != null && from.FindItemOnLayer(Layer.Arms).Name == "Black Knights Armour")
		{
		from.FindItemOnLayer(Layer.Arms).Hue = 1;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.InnerTorso) != null && from.FindItemOnLayer(Layer.InnerTorso).Name == "Black Knights Armour")
		{
		from.FindItemOnLayer(Layer.InnerTorso).Hue = 1;
		}
		else
		{
		}

return true;
     	}

		public BlackShroud( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}