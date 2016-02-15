
using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName( "an ancient fire lord corpse" )]
    public class AncientFireLord : BaseCreature
    {
        [Constructable]
        public AncientFireLord () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2 )
        {
            Name = "an ancient fire lord";
            Body = Utility.RandomList( 15, 160 );
            Hue = 1256;
            BaseSoundID = 274;
            Kills = 100;

            SetStr( 1198, 1225 );
            SetDex( 135, 147 );
            SetInt( 801, 899 );
            SetHits( 1500 );
            SetMana( 950);

            SetDamage( 24, 31 );

            SetSkill( SkillName.EvalInt, 100.0, 110.0 );
            SetSkill( SkillName.Magery, 100.0, 110.0 );
            SetSkill( SkillName.Meditation, 81.7, 94.4 );
            SetSkill( SkillName.MagicResist, 120.1, 133.0 );
            SetSkill( SkillName.Tactics, 99.6, 100.0 );
            SetSkill( SkillName.Wrestling, 98.6, 99.0 );

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 90;

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
            PackJewel( 0.03 );
           // PackJewel( 0.01 );
           // PackJewel( 0.02 );

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
                   case 0: PackItem( new GoldBars() ); break;
            }

            if ( 0.01 > Utility.RandomDouble() )
                PackItem( new IDWand() );
       }

        public override bool HasBreath{ get{ return true; } } // fire breath enabled
        public override bool AutoDispel{ get{ return true; } }

        public override Poison PoisonImmune{ get{ return Poison.Regular; } }
        public override int TreasureMapLevel{ get{ return 5; } }

        public override void AlterMeleeDamageTo( Mobile to, ref int damage )
        {
            if ( to is BaseCreature )
                damage *= 2;
        }

        public AncientFireLord( Serial serial ) : base( serial )
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