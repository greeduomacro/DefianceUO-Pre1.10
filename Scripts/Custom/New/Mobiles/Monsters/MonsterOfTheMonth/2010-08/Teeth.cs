using Server;
using System;
using Server.Mobiles;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;
using System.Collections;
using Server.Engines.IdolSystem;

namespace Server.Items
{
    public enum ToothType
    {
        Normal,
        Cursed
    }

    public class WerewolfTooth : Item
    {
        private ToothType m_ToothType;

        public ToothType ToothType
	{
		get{ return m_ToothType; }
		set
		{
			m_ToothType = value;

			if ( m_ToothType == ToothType.Normal )
				Hue = 2312;
			else
				Hue = 1194;

			 InvalidateProperties();
		}
	}

        [Constructable]
        public WerewolfTooth( ToothType type ): base( 0x3155 )
        {
            Weight = 5.0;
            Hue = 1194;
            ToothType = type;
        }

        public WerewolfTooth( Serial serial ): base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.WriteEncodedInt((int)0); // version

            writer.WriteEncodedInt( (int) m_ToothType );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadEncodedInt();

			m_ToothType = (ToothType)reader.ReadEncodedInt();
        }

        public override void OnDoubleClick( Mobile from )
        {
		from.RevealingAction();
		from.BeginTarget( 1, false, TargetFlags.None, new TargetCallback( Tooth_OnTarget ) );
        }

	public override void OnSingleClick( Mobile from )
	{
		if ( Name == null )
			LabelTo( from, m_ToothType != ToothType.Normal ? "a cursed tooth" : "a normal tooth" );
		else
			base.OnSingleClick( from );
	}

        public void Tooth_OnTarget( Mobile from, object o )
        {
            if ( from == null || from.Deleted )
                return;

            if ( o is WerewolfTooth )
            {
		WerewolfTooth tooth = o as WerewolfTooth;

		if ( !IsChildOf( from.Backpack ) || !tooth.IsChildOf( from.Backpack ) )
			from.SendMessage( "Both items must be in your backpack." );
		else
		{
			if ( tooth.ToothType != m_ToothType )
			{
				from.SendMessage( "You carefully stick both teeth together and create a statue." );
				from.PlaySound( 550 );
				from.AddToBackpack( new WerewolfStatue() );
				Delete();
				tooth.Delete();
			}
			else
				from.SendMessage( "It appears that two of the same teeth will not fit well together." );
		}
            }
            else
                from.SendMessage("That is not a tooth!");
        }
    }
}