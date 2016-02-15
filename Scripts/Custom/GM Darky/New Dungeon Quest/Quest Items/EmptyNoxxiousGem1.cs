
/////////////////////////////////////////////
//Created by LostSinner & Modified by Darky//
/////////////////////////////////////////////
using System;
using System.Collections;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Items
{
	[FlipableAttribute( 0xF52, 0xF51 )]
   	public class EmptyNoxiousGem1: Item
   	{
		[Constructable]
		public EmptyNoxiousGem1() : base( 0xF52 )
		{
			Name = "Empty Noxious Gem";
			ItemID = 3873;
			Weight = 0.1;
			Hue = 1072;
			LootType = LootType.Newbied;
		}

               private void EmptyNoxiousGem1Target_Callback( Mobile from, object obj )
                {
			if( from.InRange( this.GetWorldLocation(), 2 ) == false )
			{
				from.SendLocalizedMessage( 500486 );	//That is too far away.
			}
            		else if( obj is Corpse)
            		{
               			Corpse corpse = (Corpse)obj;

               			if( corpse.Killer == from )
               			{
               				if( corpse.Owner is NoxiousArcher || corpse.Owner is NoxiousMage || corpse.Owner is NoxiousWarlord || corpse.Owner is NoxiousWarrior )
               				{
                                		from.AddToBackpack( new NoxiousEssence() );
                     				from.Hits -=70;
						from.SendMessage( "You drain the essence of the noxious" );
                     				this.Delete();
						corpse.Delete();
                     					return;
					}
					else
					{
						from.SendMessage( "This corpse is not a noxious corpse!" );
					}
				}
				else
				{
					from.SendMessage( "You did not slay this creature!" );
				}
            		}
			else
			{
				from.SendMessage( "This is not a corpse!" );
			}
		}

                public override void OnDoubleClick( Mobile from )
                {
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
                        }
                        else
                        {
                                from.BeginTarget( -1, false, TargetFlags.Harmful, new TargetCallback( EmptyNoxiousGem1Target_Callback ) );
				from.SendMessage( "Target the corpse of a Noxious." );
			}
		}

            	public EmptyNoxiousGem1( Serial serial ) : base ( serial )
            	{
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