using System;
using System.Collections;
using Server;

namespace Server.Mobiles
{
public class Bountyhunter : BaseVendor
{
private ArrayList m_SBInfos = new ArrayList();
protected override ArrayList SBInfos{ get { return m_SBInfos; } }

[Constructable]
public Bountyhunter() : base( "the Bountyhunter" )
{
SetSkill( SkillName.Lumberjacking, 80.0, 100.0 );
SetSkill( SkillName.TasteID, 80.0, 100.0 );
}

public override void InitSBInfo()
{
m_SBInfos.Add( new SBBountyhunter() );
}

public override VendorShoeType ShoeType
{
get{ return VendorShoeType.Sandals; }
}

public override int GetShoeHue()
{
return 0;
}

public override void InitOutfit()
{
base.InitOutfit();

AddItem( new Server.Items.WideBrimHat( Utility.RandomNeutralHue() ) );
}

public Bountyhunter( Serial serial ) : base( serial )
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
}
}