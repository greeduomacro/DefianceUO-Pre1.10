using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Xanthos.Evo
{
	public class HiryuEvoEgg : BaseEvoEgg
	{
		public override IEvoCreature GetEvoCreature()
		{
			return new EvoHiryu( "a mutant hatchling" );
		}

		[Constructable]
		public HiryuEvoEgg() : base()
		{
			Name = "a mutant steed egg";
			HatchDuration = 0.01;		// 15 minutes
			//Itemid = 3165;
			Hue = 2401;
		}

		public HiryuEvoEgg( Serial serial ) : base ( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}