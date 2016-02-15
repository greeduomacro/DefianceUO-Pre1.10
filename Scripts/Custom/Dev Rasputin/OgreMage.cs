using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Factions;
using Server.EventPrizeSystem;

namespace Server.Mobiles
{
    [CorpseName( "an ogre mages corpse" )]
    public class OgreMage : BaseCreature
    {

        [Constructable]
        public OgreMage () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
        {
            Name = "an ogre mage";

            Hue = 1502;
            Body = 83;
            BaseSoundID = 427;

            SetStr( 945, 1023 );
            SetDex( 66, 75 );
            SetInt( 536, 675 );

            SetHits( 670, 792 );

            SetDamage( 25, 32 );

            SetDamageType( ResistanceType.Physical, 100 );

            SetResistance( ResistanceType.Physical, 45, 55 );
            SetResistance( ResistanceType.Fire, 30, 40 );
            SetResistance( ResistanceType.Cold, 30, 40 );
            SetResistance( ResistanceType.Poison, 40, 50 );
            SetResistance( ResistanceType.Energy, 40, 50 );

            SetSkill( SkillName.MagicResist, 125.1, 140.0 );
            SetSkill( SkillName.Tactics, 90.1, 100.0 );
            SetSkill( SkillName.Wrestling, 90.1, 100.0 );

            SetSkill( SkillName.Magery, 90.1, 110.0 );

            SetSkill( SkillName.EvalInt, 90.1, 100.0 );
            SetSkill( SkillName.Meditation, 90.1, 110.0 );


            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 75;

            Club weapon = new Club();

            weapon.DamageLevel = (WeaponDamageLevel)Utility.Random( 1, 5 );
            weapon.DurabilityLevel = (WeaponDurabilityLevel)Utility.Random( 0, 5 );
            weapon.AccuracyLevel = (WeaponAccuracyLevel)Utility.Random( 0, 5 );

            PackItem( weapon );

            PackArmor( 0, 5 );
            PackWeapon( 0, 5 );
            PackGold( 435, 592 );

            switch ( Utility.Random( 15 ))
            {
                   case 0: PackItem( new BronzePrizeToken() ); break;
            }

        }

        public override bool CanRummageCorpses{ get{ return true; } }
        public override Poison PoisonImmune{ get{ return Poison.Greater; } }
        public override int TreasureMapLevel{ get{ return 3; } }
        public override int Meat{ get{ return 2; } }

        public OgreMage( Serial serial ) : base( serial )
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