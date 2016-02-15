using System;
using Server;
using Server.Items;
using Server.EventPrizeSystem;

namespace Server.Mobiles
{
    [CorpseName( "a silver dragon corpse" )]
    public class SilverDragon : BaseCreature
    {
        [Constructable]
        public SilverDragon () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2 )
        {
            Name = "a silver dragon";
            Body = Utility.RandomList( 49 );

            Hue = 2401;
            BaseSoundID = 362;

            SetStr( 1198, 1225 );
            SetDex( 99, 120 );
            SetInt( 801, 899 );

            SetDamage( 29, 35 );

            SetSkill( SkillName.EvalInt, 82.8, 88.2 );
            SetSkill( SkillName.Magery, 85.8, 99.3 );
            SetSkill( SkillName.Meditation, 61.7, 64.4 );
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

            PackGold( 1100, 1350 );
            PackSlayer();
            PackSlayer();
                        PackJewel( 0.04 );
                        PackJewel( 0.01 );
                        PackJewel( 0.01 );

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

            switch ( Utility.Random( 3 ) )
            {
                case 0: PackWeapon( 1, 5 ); break;
                case 1: PackArmor( 1, 5 ); break;
            }

            switch ( Utility.Random( 10 ) )
            {
                case 0: PackWeapon( 4, 5 ); break;
                case 1: PackArmor( 4, 5 ); break;
            }

                          switch ( Utility.Random( 10 ))
                {
                       case 0: PackItem( new BronzePrizeToken() ); break;
                         }

                if ( 0.01 > Utility.RandomDouble() )
                    PackItem( new IDWand() );
       }

                public override bool HasBreath{ get{ return true; } } // fire breath enabled
                public override bool AutoDispel{ get{ return true; } }
        public override HideType HideType{ get{ return HideType.Barbed; } }
        public override int Hides{ get{ return 40; } }
        public override int Meat{ get{ return 19; } }
        public override int Scales{ get{ return 25; } }
        public override ScaleType ScaleType{ get{ return (ScaleType)Utility.Random( 4 ); } }
        public override Poison PoisonImmune{ get{ return Poison.Regular; } }
        public override int TreasureMapLevel{ get{ return 5; } }

        public override void AlterMeleeDamageTo( Mobile to, ref int damage )
        {
            if ( to is BaseCreature )
                damage *= 2;
        }

        public SilverDragon( Serial serial ) : base( serial )
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