
using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName( "a vengeful tamer corpse" )]
    public class vengefultamer : BaseCreature
    {
        [Constructable]
        public vengefultamer () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2 )
        {
            //Name = "a vengeful tamer";
            Title = "the vengeful tamer";
            Body = 0x190;
            Name = NameList.RandomName("male");
            Hue = 0x455;
			//BaseSoundID = 274;
            Kills = 100;

            SetStr( 1498, 1825 );
            SetDex( 155, 197 );
            SetInt( 801, 899 );
            SetHits( 1900 );
            SetMana( 950);

            SetDamage( 27, 35 );

            SetSkill( SkillName.EvalInt, 100.0, 110.0 );
            SetSkill( SkillName.Magery, 100.0, 110.0 );
            SetSkill( SkillName.Meditation, 81.7, 94.4 );
            SetSkill( SkillName.MagicResist, 140.1, 183.0 );
            SetSkill( SkillName.Tactics, 130.6, 160.0 );
            SetSkill( SkillName.Wrestling, 150.6, 160.0 );

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 120;

            int gems = Utility.RandomMinMax( 2, 6 );

            for ( int i = 0; i < gems; ++i )
                PackGem();

            switch ( Utility.Random( 7 ) )
            {
                case 0: PackItem( new GreaterCurePotion() ); break;
                case 1: PackItem( new GreaterPoisonPotion() ); break;
                case 2: PackItem( new GreaterHealPotion() ); break;
                case 3: PackItem( new GreaterStrengthPotion() ); break;
                case 4: PackItem( new GreaterAgilityPotion() ); break;
            }

            PackGold( 732, 911 );
            PackSlayer();
            PackSlayer();
            //PackJewel( 0.01 );
            PackJewel( 0.01 );
            PackJewel( 0.01 );

            AddItem(new Shoes(Utility.RandomNeutralHue()));
            AddItem(new FancyShirt());
            AddItem(new FeatheredHat());
            AddItem(new ShepherdsCrook());
            AddItem(new LongPants(Utility.RandomNeutralHue()));

            Item hair = new Item(Utility.RandomList(0x203B, 0x2049, 0x2048, 0x204A));
            hair.Hue = Utility.RandomNondyedHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            switch ( Utility.Random( 2 ) )
            {
                case 0: PackWeapon( 0, 5 ); break;
                case 1: PackArmor( 0, 5 ); break;
            }

            switch ( Utility.Random( 2 ) )
            {
                case 0: PackWeapon( 0, 5 ); break;
                case 1: PackArmor( 0, 5 ); break;
            }

            switch ( Utility.Random( 2 ) )
            {
                case 0: PackWeapon( 1, 5 ); break;
                case 1: PackArmor( 1, 5 ); break;
            }

            switch ( Utility.Random( 3 ) )
            {
                case 0: PackWeapon( 1, 5 ); break;
                case 1: PackArmor( 1, 5 ); break;
            }

            switch ( Utility.Random( 150 ))
            {
                   case 0: PackItem( new Saddle() ); break;
            }

            if ( 0.01 > Utility.RandomDouble() )
                PackItem( new IDWand() );
       }

        public override bool AutoDispel{ get{ return true; } }

        public override Poison PoisonImmune{ get{ return Poison.Regular; } }
        public override int TreasureMapLevel{ get{ return 5; } }

        public override void AlterMeleeDamageTo( Mobile to, ref int damage )
        {
            if ( to is BaseCreature )
                damage *= 2;
        }

        public vengefultamer( Serial serial ) : base( serial )
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