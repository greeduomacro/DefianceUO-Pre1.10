using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{

public class Snowpile : Item
{
[Constructable]
public Snowpile() : base( 0x913 )
{

Name = "a Pile of Snow";
Hue = 0x481;
LootType = LootType.Blessed;

}


public Snowpile( Serial serial ) : base( serial )
{
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

public override void OnSingleClick( Mobile from )
{
this.LabelTo( from, 1005578 );
}

public override void OnDoubleClick( Mobile from )
{
if (!IsChildOf(from.Backpack))
{
from.SendLocalizedMessage( 1042010 ); //You must have the object in your backpack to use it.
return;
}
else
{
if ( from.BeginAction( typeof( Snowpile ) ) )
{
from.Target = new SnowTarget( from );
from.SendLocalizedMessage( 1005575 ); // You carefully pack the snow into a ball...
}

else
{

from.SendLocalizedMessage( 1005574 );
}

}

}
private class InternalTimer : Timer
{

private Mobile m_From;

public InternalTimer( Mobile from ) : base( TimeSpan.FromSeconds( 8.0 ) )
{
m_From = from;
}

protected override void OnTick()
{

m_From.EndAction( typeof( Snowpile ) );
}
}

private class SnowTarget : Target
{
private Mobile m_Thrower;

public SnowTarget( Mobile thrower ) : base ( 10, false, TargetFlags.None )
{
m_Thrower = thrower;
}

protected override void OnTarget( Mobile from, object target )
{
if( target == from )
from.SendLocalizedMessage( 1005576 );

else if( target is Mobile)
{
Mobile m = (Mobile)target;
from.BeginAction( typeof( Snowpile ) );
from.PlaySound( 0x145 );
if ( from.Mount == null ) from.Animate( 9, 1, 1, true, false, 0 );
from.SendLocalizedMessage( 1010573 ); // You throw the snowball and hit the target!
m.SendLocalizedMessage( 1010572 ); // You have just been hit by a snowball!
Effects.SendMovingEffect( from, m, 0x36E4, 7, 0, false, true, 0x480, 0 );
}

else
{
from.SendLocalizedMessage( 1005577 );
}

new InternalTimer( from ).Start();
}
protected override void OnTargetCancel( Mobile from, TargetCancelType cancelType )
{
from.EndAction( typeof(Snowpile) );
}
}}}