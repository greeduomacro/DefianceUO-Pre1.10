//==============================================//
// Created by Dupre					//
//==============================================//
using System;
using System.Collections;
using Server;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Items
{
	public class BlackArms : PlateArms
	{
		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }

		[Constructable]
		public BlackArms()
		{
			Name = "Black Knights Armour";
			LootType = LootType.Blessed;
			Hue = 1;
		}
		public override bool OnEquip(Mobile from)
	      {
			UnMorph(from);
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

		public bool UnMorph(Mobile from)
		{

		if (from.FindItemOnLayer(Layer.OuterTorso) == null || from.FindItemOnLayer(Layer.OuterTorso).Name != "a Shroud of Phasing")
			{
			this.Hue = 1;
			}
			else
			{
			}
		return true;
		}
		public BlackArms( Serial serial ) : base( serial )
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