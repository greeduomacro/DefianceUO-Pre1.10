using System;
using System.Collections;
using Server.Items;
using Server.Spells.Seventh;
using Server.Spells.Fifth;
using Server.Spells;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
	public class InvisibleMage : BaseCreature
	{
		private static bool m_Talked;

		string[] kfcsay = new string[]
		{
		"My eyes, where are they, help me wake up from this nightmare!",
		"Why can you not see me, when i can see you?",
		"Where are you? Where am I? Where are we?",
		};

		public override bool IsScaryToPets{ get{ return true; } }

		[Constructable]
		public InvisibleMage() : base( AIType.AI_Mage, FightMode.Weakest, 10, 1, 0.2, 0.4)
		{

			Name = "an invisible mage";
			//Title= "";
			Hue= 1153;
			Body = 409;
                        //new EtherealHorse().Rider = this;

			SetStr( 175, 225);
			SetDex( 40, 75);
			SetInt( 450, 550);

			SetHits(150, 200);
			SetMana(450, 550);

			SetDamage( 8, 13 );

			SetSkill( SkillName.MagicResist, 120.0, 150.0);
                        SetSkill( SkillName.Magery, 120.4, 120.7);
                        SetSkill( SkillName.EvalInt, 260.4, 290.7);
                        SetSkill( SkillName.Meditation, 220.4, 250.7);

                        Fame=15000;
			Karma=-15000;

			VirtualArmor= 45;
		}

		public override int GetAngerSound()
		{
			return 1050;
		}

		public override int GetIdleSound()
		{
			return 1096;
		}

		public override int GetAttackSound()
		{
			return 1068;
		}

		public override int GetHurtSound()
		{
			return 1081;
		}

		public override int GetDeathSound()
		{
			return 1059;
		}

                public override bool AutoDispel{ get{ return true; } }
		public override bool BardImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }

		public override bool ShowFameTitle{ get{ return false; } }
		public override bool ClickTitle{ get{ return false; } }

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is Dragon || to is WhiteWyrm || to is SwampDragon || to is Drake || to is Nightmare || to is Daemon || to is FireSteed || to is Kirin || to is Unicorn || to is FrenziedOstard )
				damage *= 5;
		}

		public InvisibleMage( Serial serial ) : base( serial )
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
		public override bool OnBeforeDeath()
		{
			MagicWizardsHat hat = new MagicWizardsHat();
			hat.Map = this.Map;
			hat.Location = this.Location;

			Gold gold = new Gold(300, 700);
			gold.Map = this.Map;
			gold.Location = this.Location;

			Kryss weapon = new Kryss();
			weapon.DamageLevel = (WeaponDamageLevel)Utility.Random( 0, 5 );
			weapon.DurabilityLevel = (WeaponDurabilityLevel)Utility.Random( 0, 3 );
			weapon.AccuracyLevel = (WeaponAccuracyLevel)Utility.Random( 0, 4 );
			weapon.Map = this.Map;
			weapon.Location = this.Location;

			this.Delete();
			return false;
		}
	}
}