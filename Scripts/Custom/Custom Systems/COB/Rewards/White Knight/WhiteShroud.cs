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
	public class WhiteShroud : HoodedShroudOfShadows
	{
		[Constructable]
		public WhiteShroud()
		{
			Name = "a Shroud of Phasing";
			Hue = 1153;
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
		from.SendMessage ("Only a True White Knight may feel the full effect of this Shroud");
		}

		public bool Morph(Mobile from)
		{
	if (from.Hue != 19999)
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

	if (from.FindItemOnLayer(Layer.FirstValid) != null && from.FindItemOnLayer(Layer.FirstValid).Name == "Dupre's Blade")
		{
		from.FindItemOnLayer(Layer.FirstValid).Hue = 19999;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.TwoHanded) != null && from.FindItemOnLayer(Layer.TwoHanded).Name == "White Knights Armour")
		{
		from.FindItemOnLayer(Layer.TwoHanded).Hue = 19999;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.Helm) != null && from.FindItemOnLayer(Layer.Helm).Name == "White Knights Armour")
		{
		from.FindItemOnLayer(Layer.Helm).Hue = 19999;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.Shoes) != null && from.FindItemOnLayer(Layer.Shoes).Name == "White Knights Armour")
		{
		from.FindItemOnLayer(Layer.Shoes).Hue = 19999;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.Pants) != null && from.FindItemOnLayer(Layer.Pants).Name == "White Knights Armour")
		{
		from.FindItemOnLayer(Layer.Pants).Hue = 19999;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.Gloves) != null && from.FindItemOnLayer(Layer.Gloves).Name == "White Knights Armour")
		{
		from.FindItemOnLayer(Layer.Gloves).Hue = 19999;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.Neck) != null && from.FindItemOnLayer(Layer.Neck).Name == "White Knights Armour")
		{
		from.FindItemOnLayer(Layer.Neck).Hue = 19999;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.Arms) != null && from.FindItemOnLayer(Layer.Arms).Name == "White Knights Armour")
		{
		from.FindItemOnLayer(Layer.Arms).Hue = 19999;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.InnerTorso) != null && from.FindItemOnLayer(Layer.InnerTorso).Name == "White Knights Armour")
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
			this.Hue = 1153;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.FirstValid) != null && from.FindItemOnLayer(Layer.FirstValid).Name == "Dupre's Blade")
		{
		from.FindItemOnLayer(Layer.FirstValid).Hue = 0;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.TwoHanded) != null && from.FindItemOnLayer(Layer.TwoHanded).Name == "White Knights Armour")
		{
		from.FindItemOnLayer(Layer.TwoHanded).Hue = 1153;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.Helm) != null && from.FindItemOnLayer(Layer.Helm).Name == "White Knights Armour")
		{
		from.FindItemOnLayer(Layer.Helm).Hue = 1153;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.Shoes) != null && from.FindItemOnLayer(Layer.Shoes).Name == "White Knights Armour")
		{
		from.FindItemOnLayer(Layer.Shoes).Hue = 1153;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.Pants) != null && from.FindItemOnLayer(Layer.Pants).Name == "White Knights Armour")
		{
		from.FindItemOnLayer(Layer.Pants).Hue = 1153;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.Gloves) != null && from.FindItemOnLayer(Layer.Gloves).Name == "White Knights Armour")
		{
		from.FindItemOnLayer(Layer.Gloves).Hue = 1153;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.Neck) != null && from.FindItemOnLayer(Layer.Neck).Name == "White Knights Armour")
		{
		from.FindItemOnLayer(Layer.Neck).Hue = 1153;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.Arms) != null && from.FindItemOnLayer(Layer.Arms).Name == "White Knights Armour")
		{
		from.FindItemOnLayer(Layer.Arms).Hue = 1153;
		}
		else
		{
		}

	if (from.FindItemOnLayer(Layer.InnerTorso) != null && from.FindItemOnLayer(Layer.InnerTorso).Name == "White Knights Armour")
		{
		from.FindItemOnLayer(Layer.InnerTorso).Hue = 1153;
		}
		else
		{
		}

return true;
     	}

		public WhiteShroud( Serial serial ) : base( serial )
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