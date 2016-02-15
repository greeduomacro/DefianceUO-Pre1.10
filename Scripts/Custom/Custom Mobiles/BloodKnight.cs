//Author:Neon Destiny
//First Alpha:11/11/03
//First part of my Matrix series
//email:NeonDestiny@hotmail.com
//ICQ:308073073
//Shard: Neon Destiny PVP




using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
	public class BloodKnight : BaseCreature
	{

		private static bool m_Talked;

		string[] kfcsay = new string[]
		{
		"Thy blood shall be spilt.",
		"Do not engage in fighting me, i am way beyond your capability.",
		"THOU SHALL PAY.",
		"Death is your next destination."
		};
		[Constructable]
		public BloodKnight() : base( AIType.AI_Mage, FightMode.Weakest, 10, 1, 0.2, 0.4)
		{

			Name = "Blood Knight";
			Title= "";
			Hue= Utility.RandomSkinHue();
			Body = 400;
			SpeechHue=1153;
			BaseSoundID = 0;
			Team = 0;

			SetStr( 275, 375);
			SetDex( 40, 75);
			SetInt( 300, 350);

			SetHits(230, 375);
			SetMana(300, 350);

			SetDamage( 10, 15);

                        SetDamageType( ResistanceType.Physical, 80);
			SetDamageType( ResistanceType.Energy, 20);

			SetResistance( ResistanceType.Physical, 50, 55);
			SetResistance( ResistanceType.Cold, 40, 45);
			SetResistance( ResistanceType.Poison, 40, 45);
			SetResistance( ResistanceType.Energy, 20, 25);

			SetSkill( SkillName.Wrestling, 100.2, 100.6);
			SetSkill( SkillName.Tactics, 100.7, 100.4);
			SetSkill( SkillName.Anatomy, 100.5, 100.3);
			SetSkill( SkillName.MagicResist, 110.4, 110.7);
                        SetSkill( SkillName.Magery, 120.4, 120.7);
			SetSkill( SkillName.Swords, 130.4, 130.7);
                        SetSkill( SkillName.EvalInt, 130.4, 130.7);

                        Fame=6300;
			Karma=-10000;

			VirtualArmor= 45;

			Item PaladinSword = new PaladinSword();
			PaladinSword.Movable=false;
			PaladinSword.Hue=2403;
		      //PaladinSword.Slayer=DragonSlaying;
                        EquipItem( PaladinSword );

                        Item DragonLegs = new DragonLegs();
			DragonLegs.Movable=false;
			DragonLegs.Hue=1175;
			EquipItem( DragonLegs );

			Item DragonChest = new DragonChest();
			DragonChest.Movable=false;
			DragonChest.Hue=1175;
			EquipItem( DragonChest );

                        Item DragonGloves = new DragonGloves();
			DragonGloves.Movable=false;
			DragonGloves.Hue=1175;
			EquipItem( DragonGloves );

                        Item DragonHelm = new DragonHelm();
			DragonHelm.Movable=false;
			DragonHelm.Hue=1175;
			EquipItem( DragonHelm );

			Item PlateGorget = new PlateGorget();
			PlateGorget.Movable=false;
			PlateGorget.Hue=1175;
			EquipItem( PlateGorget );

			Item DragonArms = new DragonArms();
			DragonArms.Movable=false;
			DragonArms.Hue=1175;
			EquipItem( DragonArms );

                        Item Sandals = new Sandals();
			Sandals.Movable=false;
			Sandals.Hue=1175;
			EquipItem( Sandals );

			Item hair = new Item( 0x203B);
			hair.Hue = 1109;
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem( hair );

			PackGold( 220, 480);
			PackMagicItems( 3, 7);
                        PackJewel( 0.01 );

                                switch ( Utility.Random( 15 ))
        		 {
           			case 0: PackItem( new BloodRuby() ); break;
        		 }


       }
	public override bool AlwaysMurderer{ get{ return true; } }
        public override bool CanRummageCorpses{ get{ return true; } }

		public BloodKnight( Serial serial ) : base( serial )
		{
		}


	 public override void OnMovement( Mobile m, Point3D oldLocation )
               {
         		if( m_Talked == false )
        		 {
          		 	 if ( m.InRange( this, 4 ) )
          			  {
          				m_Talked = true;
              				SayRandom( kfcsay, this );
				this.Move( GetDirectionTo( m.Location ) );
				SpamTimer t = new SpamTimer();
				t.Start();
            			}
		}
	}

	private class SpamTimer : Timer
	{
		public SpamTimer() : base( TimeSpan.FromSeconds( 5 ) )
		{
			Priority = TimerPriority.OneSecond;
		}

		protected override void OnTick()
		{
		m_Talked = false;
		}
	}

	private static void SayRandom( string[] say, Mobile m )
	{
		m.Say( say[Utility.Random( say.Length )] );
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