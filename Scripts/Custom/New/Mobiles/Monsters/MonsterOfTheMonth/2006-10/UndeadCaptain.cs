
using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName( "undead captain corpse" )]
    public class UndeadCaptain : BaseCreature
    {
        [Constructable]
        public UndeadCaptain () : base( AIType.AI_Melee, FightMode.Strongest, 10, 1, 0.2, 0.4 )
        {
            Name = "Undead Captain";
            Body = 0x190;
            //Hue = 0x455;
            Kills = 100;

            SetStr( 1498, 1825 );
            SetDex( 155, 197 );
            SetInt( 801, 899 );
            SetHits( 1900 );
            SetMana( 950);

            SetDamage( 3, 5 );

            SetSkill( SkillName.EvalInt, 10.0, 11.0 );
            SetSkill( SkillName.Magery, 50.0, 60.0 );
            SetSkill( SkillName.Meditation, 81.7, 94.4 );
            SetSkill( SkillName.MagicResist, 740.1, 783.0 );
            SetSkill( SkillName.Tactics, 100.6, 110.0 );
	    SetSkill( SkillName.Anatomy, 100.6, 110.0 );
	    SetSkill( SkillName.Healing, 100.6, 110.0 );
	    SetSkill( SkillName.Swords, 100.6, 120.0 );
            SetSkill( SkillName.Wrestling, 400.6, 420.0 );

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 90;

            int gems = Utility.RandomMinMax( 2, 6 );



            switch ( Utility.Random( 7 ) )
            {
                case 0: PackItem( new GreaterCurePotion() ); break;
                case 1: PackItem( new GreaterPoisonPotion() ); break;
                case 2: PackItem( new GreaterHealPotion() ); break;
                case 3: PackItem( new GreaterStrengthPotion() ); break;
                case 4: PackItem( new GreaterAgilityPotion() ); break;
            }

            AddLoot( LootPack.FilthyRich, 4 );
	    AddLoot( LootPack.FilthyRich, 4 );
	    AddLoot( LootPack.Gems, 5 );



            AddItem( new TricorneHat() );
            AddItem( new StuddedGloves() );
	    AddItem( new Cutlass() );

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

            switch ( Utility.Random( 100 ))
            {
                   case 0: PackItem( new PirateShipModel() ); break;
            }

            if ( 0.01 > Utility.RandomDouble() )
                PackItem( new IDWand() );
       }

        public override bool AutoDispel{ get{ return true; } }
	public override bool BardImmune{ get{ return true; } }
        public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
        public override int TreasureMapLevel{ get{ return 5; } }

        public override void AlterMeleeDamageTo( Mobile to, ref int damage )
        {
            if ( to is BaseCreature )
                damage *= 20;
        }

        public UndeadCaptain( Serial serial ) : base( serial )
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