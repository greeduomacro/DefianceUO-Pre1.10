// by raistlin
using System;
using Server.Network;

namespace Server.Items
{
   public class BaseVetClothing : Item
   {
      public BaseVetClothing( int itemID, Layer layer, int hue ) :  base( itemID )
      {
         Layer = layer;
         Hue = hue;
         LootType = LootType.Blessed;
      }

      public BaseVetClothing( Serial serial ) : base( serial )
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

      public override bool OnEquip( Mobile from )
      {
         if (!Core.AOS)
            from.VirtualArmor += 2;
         else
            from.AddResistanceMod( new ResistanceMod( ResistanceType.Physical, 2 ) );
         return true;
      }
      public override void OnRemoved( object parent )
      {
         if ( parent is Mobile )
         {
       if (!Core.AOS)
            ((Mobile)parent).VirtualArmor -= 2;
         else
            ((Mobile)parent).AddResistanceMod( new ResistanceMod( ResistanceType.Physical, -2 ) );
         }
      }
}


   [FlipableAttribute( 0x1515, 0x1530 )]
   public class BronzeCloak : BaseVetClothing
   {
      public override int LabelNumber{ get{ return 1041286; } }

      [Constructable]
      public BronzeCloak() : base( 0x1515, Layer.Cloak, OreInfo.Bronze.Hue )
      {
      }

      public BronzeCloak( Serial serial ) : base( serial )
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

   [FlipableAttribute( 0x1f03, 0x1f04 )]
   public class BronzeRobe : BaseVetClothing
   {
      public override int LabelNumber{ get{ return 1041287; } }

      [Constructable]
      public BronzeRobe() : base( 0x1F03, Layer.OuterTorso, OreInfo.Bronze.Hue )
      {
      }

      public BronzeRobe( Serial serial ) : base( serial )
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

   [FlipableAttribute( 0x1515, 0x1530 )]
   public class CopperCloak : BaseVetClothing
   {
      public override int LabelNumber{ get{ return 1041288; } }

      [Constructable]
      public CopperCloak() : base( 0x1515, Layer.Cloak, OreInfo.Copper.Hue )
      {
      }

      public CopperCloak( Serial serial ) : base( serial )
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

   [FlipableAttribute( 0x1f03, 0x1f04 )]
   public class CopperRobe : BaseVetClothing
   {
      public override int LabelNumber{ get{ return 1041289; } }

      [Constructable]
      public CopperRobe() : base( 0x1F03, Layer.OuterTorso, OreInfo.Copper.Hue )
      {
      }

      public CopperRobe( Serial serial ) : base( serial )
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

   [FlipableAttribute( 0x1515, 0x1530 )]
   public class AgapiteCloak : BaseVetClothing
   {
      public override int LabelNumber{ get{ return 1041290; } }

      [Constructable]
      public AgapiteCloak() : base( 0x1515, Layer.Cloak, OreInfo.Agapite.Hue )
      {
      }

      public AgapiteCloak( Serial serial ) : base( serial )
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

   [FlipableAttribute( 0x1f03, 0x1f04 )]
   public class AgapiteRobe : BaseVetClothing
   {
      public override int LabelNumber{ get{ return 1041291; } }

      [Constructable]
      public AgapiteRobe() : base( 0x1F03, Layer.OuterTorso, OreInfo.Agapite.Hue )
      {
      }

      public AgapiteRobe( Serial serial ) : base( serial )
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

   [FlipableAttribute( 0x1515, 0x1530 )]
   public class GoldCloak : BaseVetClothing
   {
      public override int LabelNumber{ get{ return 1041292; } }

      [Constructable]
      public GoldCloak() : base( 0x1515, Layer.Cloak, OreInfo.Gold.Hue )
      {
      }

      public GoldCloak( Serial serial ) : base( serial )
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

   [FlipableAttribute( 0x1f03, 0x1f04 )]
   public class GoldRobe : BaseVetClothing
   {
      public override int LabelNumber{ get{ return 1041293; } }

      [Constructable]
      public GoldRobe() : base( 0x1F03, Layer.OuterTorso, OreInfo.Gold.Hue )
      {
      }

      public GoldRobe( Serial serial ) : base( serial )
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

   [FlipableAttribute( 0x1515, 0x1530 )]
   public class VeriteCloak : BaseVetClothing
   {
      public override int LabelNumber{ get{ return 1041294; } }

      [Constructable]
      public VeriteCloak() : base( 0x1515, Layer.Cloak, OreInfo.Verite.Hue )
      {
      }

      public VeriteCloak( Serial serial ) : base( serial )
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

   [FlipableAttribute( 0x1f03, 0x1f04 )]
   public class VeriteRobe : BaseVetClothing
   {
      public override int LabelNumber{ get{ return 1041295; } }

      [Constructable]
      public VeriteRobe() : base( 0x1F03, Layer.OuterTorso, OreInfo.Verite.Hue )
      {
      }

      public VeriteRobe( Serial serial ) : base( serial )
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

   [FlipableAttribute( 0x1515, 0x1530 )]
   public class ValoriteCloak : BaseVetClothing
   {
      public override int LabelNumber{ get{ return 1041296; } }

      [Constructable]
      public ValoriteCloak() : base( 0x1515, Layer.Cloak, OreInfo.Valorite.Hue )
      {
      }

      public ValoriteCloak( Serial serial ) : base( serial )
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

   [FlipableAttribute( 0x1f03, 0x1f04 )]
   public class ValoriteRobe : BaseVetClothing
   {
      public override int LabelNumber{ get{ return 1041297; } }

      [Constructable]
      public ValoriteRobe() : base( 0x1F03, Layer.OuterTorso, OreInfo.Valorite.Hue )
      {
      }

      public ValoriteRobe( Serial serial ) : base( serial )
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
   [FlipableAttribute( 0x1515, 0x1530 )]
   public class DGrayCloak : BaseVetClothing
   {
      [Constructable]
      public DGrayCloak() : base( 0x1515, Layer.Cloak, 0x497 )
      {
         Name = "Dark Gray Cloak";
      }

      public DGrayCloak( Serial serial ) : base( serial )
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

   [FlipableAttribute( 0x1f03, 0x1f04 )]
   public class DGrayRobe : BaseVetClothing
   {
      [Constructable]
      public DGrayRobe() : base( 0x1F03, Layer.OuterTorso, 0x497 )
      {
         Name = "Dark Gray Robe";
      }

      public DGrayRobe( Serial serial ) : base( serial )
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
   [FlipableAttribute( 0x1515, 0x1530 )]
   public class IGreenCloak : BaseVetClothing
   {
      [Constructable]
      public IGreenCloak() : base( 0x1515, Layer.Cloak, 0x48F )
      {
         Name = "Ice Green Cloak";
      }

      public IGreenCloak( Serial serial ) : base( serial )
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

   [FlipableAttribute( 0x1f03, 0x1f04 )]
   public class IGreenRobe : BaseVetClothing
   {
      [Constructable]
      public IGreenRobe() : base( 0x1F03, Layer.OuterTorso, 0x48F )
      {
         Name = "Ice Green Robe";
      }

      public IGreenRobe( Serial serial ) : base( serial )
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
   [FlipableAttribute( 0x1515, 0x1530 )]
   public class IBlueCloak : BaseVetClothing
   {
      [Constructable]
      public IBlueCloak() : base( 0x1515, Layer.Cloak, 0x482 )
      {
         Name = "Ice Blue Cloak";
      }

      public IBlueCloak( Serial serial ) : base( serial )
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

   [FlipableAttribute( 0x1f03, 0x1f04 )]
   public class IBlueRobe : BaseVetClothing
   {
      [Constructable]
      public IBlueRobe() : base( 0x1F03, Layer.OuterTorso, 0x482 )
      {
         Name = "Ice Blue Robe";
      }

      public IBlueRobe( Serial serial ) : base( serial )
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
   [FlipableAttribute( 0x1515, 0x1530 )]
   public class JBlackCloak : BaseVetClothing
   {
      [Constructable]
      public JBlackCloak() : base( 0x1515, Layer.Cloak, 0x1 )
      {
         Name = "Jet Black Cloak";
      }

      public JBlackCloak( Serial serial ) : base( serial )
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

   [FlipableAttribute( 0x1f03, 0x1f04 )]
   public class JBlackRobe : BaseVetClothing
   {
      [Constructable]
      public JBlackRobe() : base( 0x1F03, Layer.OuterTorso, 0x1 )
      {
         Name = "Jet Black Robe";
      }

      public JBlackRobe( Serial serial ) : base( serial )
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
   [FlipableAttribute( 0x1515, 0x1530 )]
   public class IWhiteCloak : BaseVetClothing
   {
      [Constructable]
      public IWhiteCloak() : base( 0x1515, Layer.Cloak, 0x47E )
      {
         Name = "Ice White Cloak";
      }

      public IWhiteCloak( Serial serial ) : base( serial )
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

   [FlipableAttribute( 0x1f03, 0x1f04 )]
   public class IWhiteRobe : BaseVetClothing
   {
      [Constructable]
      public IWhiteRobe() : base( 0x1F03, Layer.OuterTorso, 0x47E )
      {
         Name = "Ice White Robe";
      }

      public IWhiteRobe( Serial serial ) : base( serial )
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
   [FlipableAttribute( 0x1515, 0x1530 )]
   public class FireCloak : BaseVetClothing
   {
      [Constructable]
      public FireCloak() : base( 0x1515, Layer.Cloak, 0x489 )
      {
         Name = "Fire Cloak";
      }

      public FireCloak( Serial serial ) : base( serial )
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

   [FlipableAttribute( 0x1f03, 0x1f04 )]
   public class FireRobe : BaseVetClothing
   {
      [Constructable]
      public FireRobe() : base( 0x1F03, Layer.OuterTorso, 0x489 )
      {
         Name = "Fire Robe";
      }

      public FireRobe( Serial serial ) : base( serial )
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