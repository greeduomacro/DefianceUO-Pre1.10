//==============================================//
// Created by Dupre					//
//==============================================//
using System;
using Server;

namespace Server.Items
{
	public class BlackthornsBlade : Katana
	{
		public override int ArtifactRarity{ get{ return 69; } }

		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }

		[Constructable]
		public BlackthornsBlade()
		{
			Name = "Blackthorn's Blade";
			Hue = 1;
			Attributes.SpellChanneling = 1;
			Attributes.WeaponSpeed = 100;
			Attributes.WeaponDamage = 100;
			Attributes.BonusStr = 50;
			Attributes.Luck = 1500;
			Attributes.RegenHits = 20;
			Attributes.DefendChance = 100;
		}

		//public override void GetDamageTypes( out int phys, out int fire, out int cold, out int pois, out int nrgy )
		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy )
		{
			fire = 20;
			cold = 20;
			nrgy = 20;
			phys = 20;
			pois = 20;
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
		public BlackthornsBlade( Serial serial ) : base( serial )
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