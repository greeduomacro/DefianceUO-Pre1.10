using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class MouldyCheese : Item
	{

		[Constructable]
		public MouldyCheese() : base( 0x97D )
		{
			Hue = 768;
			Weight = 2.0;
			Name = "mouldy cheese";
		}

		public MouldyCheese( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsChildOf( from.Backpack ) )
			{
				if ( !from.CanBeginAction( typeof( Spells.Fifth.IncognitoSpell ) ) )
				{
					//from.SendLocalizedMessage( 501698 ); // You cannot disguise yourself while incognitoed.
					from.SendMessage( "You decide against eating it while incognitoed " );
				}
				else if ( !from.CanBeginAction( typeof( Spells.Seventh.PolymorphSpell ) ) )
				{
					//from.SendLocalizedMessage( 501699 ); // You cannot disguise yourself while polymorphed.
					from.SendMessage( "You can not be polymorphed at this time" );
				}
				else if ( Spells.Necromancy.TransformationSpell.UnderTransformation( from ) )
				{
					//from.SendLocalizedMessage( 501699 ); // You cannot disguise yourself while polymorphed.
					from.SendMessage( "You decide against eating it while polymorphed" );
				}
				else if ( from.IsBodyMod || from.FindItemOnLayer( Layer.Helm ) is OrcishKinMask || from.FindItemOnLayer( Layer.Helm ) is OrcishKinRPMask )
				{
					//from.SendLocalizedMessage( 501605 ); // You are already disguised.
					from.SendMessage( "You decide against eating it while as your disguised" );
				}
				else
				{
					from.BodyMod = ( from.Female ? 186 : 185 );
					from.HueMod = 0;

					if ( from is PlayerMobile )
						((PlayerMobile)from).SavagePaintExpiration = TimeSpan.FromDays( 7.0 );

					//from.SendLocalizedMessage( 1042537 ); // You now bear the markings of the savage tribe.  Your body paint will last about a week or you can remove it with an oil cloth.
					from.SendMessage( "You now bear the markings of the undead pirates.  This will last about a week" );
					from.PlaySound( Utility.Random( 0x3A, 3 ) );
					if ( from.Body.IsHuman && !from.Mounted )
						from.Animate( 34, 5, 1, true, false, 0 );
					from.ApplyPoison( from, Poison.Lethal );
					Consume();
				}
			}
			else
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}