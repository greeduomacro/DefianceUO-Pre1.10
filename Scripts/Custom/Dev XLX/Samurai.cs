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
	public class Samurai : BaseCreature
	{

		private static bool m_Talked;

		string[] kfcsay = new string[]
		{
		"We live to serve.",
		"Of men who have a sense of honor, more come through alive than are slain, but from those who flee comes neither glory nor any help.",
		"Seek freedom and become captive of your desires, seek discipline and find your liberty.",
		"You are not part of the clan."
		};

                public override bool IsScaryToPets{ get{ return true; } }

		[Constructable]
		public Samurai() : base( AIType.AI_Melee, FightMode.Weakest, 10, 1, 0.175, 0.3)
		{

			Name = "Samurai Assassin";
			Title= ", Defiance Cult Clan";
			Hue= 1;
			Body = 400;
			SpeechHue= 2305;
			BaseSoundID = 0;
			Team = 0;
                        //new EtherealHorse().Rider = this;

			SetStr( 185, 215);
			SetDex( 130, 140);
			SetInt( 0, 0);

			SetHits(185, 300);

			SetSkill( SkillName.Tactics, 100.7, 100.4);
			SetSkill( SkillName.MagicResist, 191.4, 191.7);
			SetSkill( SkillName.Swords, 110.4, 110.7);
			SetSkill( SkillName.Anatomy, 110.4, 110.7);
			SetSkill( SkillName.Parry, 75.1, 100.1);

                        Fame=15000;
			Karma=-15000;

			VirtualArmor= 75;

			Item Bokuto = new Bokuto();
			Bokuto.Movable=false;
			Bokuto.Hue=1150;
		        Bokuto.Name="Samurai Bokuto";
                        EquipItem( Bokuto );

			Item Buckler = new Buckler();
			Buckler.Movable=false;
			Buckler.Hue=1253;
		        EquipItem( Buckler );

                        Item AncientSamuraiHelm = new AncientSamuraiHelm();
			AncientSamuraiHelm.Movable=false;
			AncientSamuraiHelm.Hue=0;
			EquipItem( AncientSamuraiHelm );

			Item StuddedChest = new StuddedChest();
			StuddedChest.Movable=false;
			StuddedChest.Hue=1109;
			StuddedChest.Name="Samurai Clan";
			EquipItem( StuddedChest );

                        Item BoneArms = new BoneArms();
			BoneArms.Movable=false;
			BoneArms.Hue=1109;
                        EquipItem( BoneArms );

                        Item Kamishimo = new Kamishimo();
			Kamishimo.Movable=false;
			Kamishimo.Hue=2407;
		        EquipItem( Kamishimo );

			Item Sandals = new Sandals();
			Sandals.Movable=false;
			Sandals.Hue=1;
			EquipItem( Sandals );

			Item BodySash = new BodySash();
			BodySash.Movable=false;
			BodySash.Hue=4;
			BodySash.Name="Samurai Clan Member.";
                        EquipItem( BodySash );

			Item hair = new Item( 0x203D);
			hair.Hue = 1;
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem( hair );

			PackGold( 550, 2000);
			PackMagicItems( 3, 7);
			PackMagicItems( 3, 7);
			PackMagicItems( 3, 7);
			PackMagicItems( 3, 7);

                                switch ( Utility.Random( 50 ))
        		 {
           			case 0: PackItem( new Bokuto() ); break;
        		 }
		}

		public override void OnDeath( Container c )
	  	{
			if ( Utility.Random( 75 ) <  1 )
				c.DropItem( new RareBloodCarpet( PieceType.SouthEdge ) );

			base.OnDeath( c );
	  	}

                public override bool AutoDispel{ get{ return true; } }
		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool BardImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }

		public override bool ShowFameTitle{ get{ return false; } }
		public override bool ClickTitle{ get{ return false; } }


	        public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.25 >= Utility.RandomDouble() && attacker is BaseCreature )
			{
				BaseCreature c = (BaseCreature)attacker;

				if ( c.Controlled && c.ControlMaster != null )
				{
					c.ControlTarget = c.ControlMaster;
					c.ControlOrder = OrderType.Attack;
					c.Combatant = c.ControlMaster;
				}
			}
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

		public Samurai( Serial serial ) : base( serial )
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
	}
}