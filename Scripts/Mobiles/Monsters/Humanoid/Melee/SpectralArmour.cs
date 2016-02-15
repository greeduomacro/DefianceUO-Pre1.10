using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{

	public class SpectralArmour : BaseCreature
	{
		[Constructable]
		public SpectralArmour() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Body = 637;
			Hue = 32;
			Name = "spectral armour";
			BaseSoundID = 451;

			SetStr( 309, 333 );
			SetDex( 99, 106 );
			SetInt( 101, 110 );
			SetSkill( SkillName.Wrestling, 78.1, 95.5 );
			SetSkill( SkillName.Tactics, 91.1, 99.7 );
			SetSkill( SkillName.MagicResist, 92.4, 79 );
			SetSkill( SkillName.Swords, 78.1, 97.4);

			VirtualArmor = 40;
			SetFameLevel( 3 );
			SetKarmaLevel( 3 );





			AddItem( new Scimitar());

		}

		public override Poison PoisonImmune{ get{ return Poison.Regular; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public override int HitsMax { get { return 323; } }

		public SpectralArmour( Serial serial ) : base( serial )
		{
		}

        //Al: 5x damage to pets (excluding summons)
        public override void AlterMeleeDamageTo(Mobile to, ref int damage)
        {
            if (to is BaseCreature)
            {
                BaseCreature bc = (BaseCreature)to;
                if (bc.Controlled)
                    damage *= 5;
            }
        }

        //Al: 1/5 damage from pets (excluding summons)
        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            if (from is BaseCreature)
            {
                BaseCreature bc = (BaseCreature)from;
                if (bc.Controlled)
                    damage /= 5;
            }
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
		public override bool OnBeforeDeath()
		{
			Gold gold = new Gold(150, 250);
			gold.Map = this.Map;
			gold.Location = this.Location;

			Scimitar weapon = new Scimitar();
			weapon.DamageLevel = (WeaponDamageLevel)Utility.Random( 1, 5 );
			weapon.DurabilityLevel = (WeaponDurabilityLevel)Utility.Random( 0, 5 );
			weapon.AccuracyLevel = (WeaponAccuracyLevel)Utility.Random( 0, 5 );
			weapon.Map = this.Map;
			weapon.Location = this.Location;

			this.Delete();
			return false;
		}
	}
}