using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Xanthos.Evo
{
	public class EvoSpiderEvoEgg : BaseEvoEgg
	{
		public override IEvoCreature GetEvoCreature()
		{
			return new EvoSpider( "a spider hatchling" );
		}

		[Constructable]
		public EvoSpiderEvoEgg() : base()
		{
			Name = "a disgusting spider egg";
			HatchDuration = 0.01;		// 15 minutes
			Hue = 1154;
		}

		public EvoSpiderEvoEgg( Serial serial ) : base ( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			else if ( AllowEvolution )
			{
				if ( from.Skills[SkillName.AnimalTaming].Value < 99.9 )
					from.SendMessage( "You must have 99.9 in taming to hatch this egg." );
				else if ( from.Followers > 2 )
					from.SendMessage( "You have too many followers to hatch this egg." );
				else if ( Utility.RandomDouble() <= 0.25 )
					Hatch( from );
				else
				{
					from.SendMessage( "The egg has failed to hatch and the hatchling has died." );
					Consume();
				}
			}
			else
				from.SendMessage( "This egg is not yet ready to be hatched." );
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