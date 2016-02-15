using System;
using Server;
using Server.FSPvpPointSystem;

namespace Server.Items
{
	public class FieldArms : BaseArmor
	{
                public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Leather; } }
		//public override int OldStrBonus{ get{ return 2; } }
		public override int OldDexBonus{ get{ return 0; } }
		public override int OldIntBonus{ get{ return 0; } }
		public override int ArmorBase{ get{ return 15; } }
		public override ArmorMeditationAllowance DefMedAllowance{ get{ return ArmorMeditationAllowance.All; } }


		[Constructable]
		public FieldArms() : base( 0x1410 )
		{
                        Name = "FieldFighter Arms [Grand General]";
			Weight = 5.0;
                        Hue = 1001;
		}

		public FieldArms( Serial serial ) : base( serial )
		{
		}

				public override bool OnEquip( Mobile from )
		{
			FSPvpSystem.PvpStats ps = FSPvpSystem.GetPvpStats( from );
			if ( PvpRankInfo.GetInfo( ps.RankType ).Rank > 3 ) // Change Min Rank To Use Here!!!
				return base.OnEquip( from );
			else
			{
				from.SendMessage( "You lack the rank to use this item." );
				from.SendMessage( "The item has been removed." );
				this.Delete();
				return false;
			}
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
	}
}