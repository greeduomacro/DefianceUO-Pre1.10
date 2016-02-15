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
	public class CaptainBlackheart : BaseCreature
	{

		private static bool m_Talked;

		string[] kfcsay = new string[]
		{
		"Thy blood shall be spilt.",
		"FLEE PEASANT!, I am beyond your skill.",
		"You shall rot!",
		"You too shall serve the Hell Legion!"
		};
		[Constructable]
		public CaptainBlackheart() : base( AIType.AI_Mage, FightMode.Weakest, 10, 1, 0.2, 0.4)
		{

			Name = "Captain Blackheart";
			Title= "Of the Burning Legion";
			Hue= Utility.RandomSkinHue();
			Body = 400;
			SpeechHue=1161;
			BaseSoundID = 0;
			Team = 0;

			SetStr( 325, 455);
			SetDex( 100, 120);
			SetInt( 500, 550);

			SetHits(325, 455);
			SetMana(500, 550);

			SetDamage( 10, 30);

                        SetDamageType( ResistanceType.Physical, 80);
			SetDamageType( ResistanceType.Energy, 20);

			SetResistance( ResistanceType.Physical, 50, 55);
			SetResistance( ResistanceType.Cold, 40, 45);
			SetResistance( ResistanceType.Poison, 40, 45);
			SetResistance( ResistanceType.Energy, 20, 25);

			SetSkill( SkillName.Wrestling, 100.2, 100.6);
			SetSkill( SkillName.Tactics, 100.7, 100.4);
			SetSkill( SkillName.Anatomy, 100.5, 100.3);
			SetSkill( SkillName.MagicResist, 120.4, 120.7);
                        SetSkill( SkillName.Magery, 120.4, 120.7);
			SetSkill( SkillName.Swords, 140.4, 140.7);
                        SetSkill( SkillName.EvalInt, 150.4, 150.7);

                        Fame=6300;
			Karma=-10000;

			VirtualArmor= 45;

			Item PaladinSword = new PaladinSword();
			PaladinSword.Movable=false;
			PaladinSword.Hue=1150;
		      //PaladinSword.SlayerName = SlayerName.DragonSlaying;
                        EquipItem( PaladinSword );

                        Item PlateLegs = new PlateLegs();
			PlateLegs.Movable=false;
			PlateLegs.Hue=1109;
			EquipItem( PlateLegs );

			Item PlateChest = new PlateChest();
			PlateChest.Movable=false;
			PlateChest.Hue=1109;
			EquipItem( PlateChest );

                        Item PlateGloves = new PlateGloves();
			PlateGloves.Movable=false;
			PlateGloves.Hue=1109;
			EquipItem( PlateGloves );

                        Item BoneHelm = new BoneHelm();
			BoneHelm.Movable=false;
			BoneHelm.Hue=1150;
			BoneHelm.Layer = Layer.Earrings;
                        EquipItem( BoneHelm );

                        Item DeerMask = new DeerMask();
			DeerMask.Movable=false;
			DeerMask.Hue=1109;
                        EquipItem( DeerMask );

			Item PlateGorget = new PlateGorget();
			PlateGorget.Movable=false;
			PlateGorget.Hue=1109;
			EquipItem( PlateGorget );

			Item PlateArms = new PlateArms();
			PlateArms.Movable=false;
			PlateArms.Hue=1109;
			EquipItem( PlateArms );

			Item hair = new Item( 0x203c);
			hair.Hue = 1161;
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem( hair );

			PackGold( 420, 680);
			PackMagicItems( 3, 7);
                        PackJewel( 0.01 );

                                switch ( Utility.Random( 15 ))
        		 {
           			case 0: PackItem( new BloodRuby() ); break;
        		 }


       }

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool BardImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
                public override bool CanRummageCorpses{ get{ return true; } }

		public override bool ShowFameTitle{ get{ return false; } }
		public override bool ClickTitle{ get{ return false; } }

		public CaptainBlackheart( Serial serial ) : base( serial )
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