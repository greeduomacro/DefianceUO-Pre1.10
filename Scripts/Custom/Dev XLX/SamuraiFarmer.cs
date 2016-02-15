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
	public class SamuraiFarmer : BaseCreature
	{

		private static bool m_Talked;

		string[] kfcsay = new string[]
		{
		"We live to serve.",
		"The harvest truly is plenteous, but the laborers are few.",
		"He who labors diligently need never despair; for all things are accomplished by diligence and labor.",
		"You are not part of the clan."
		};

                public override bool IsScaryToPets{ get{ return true; } }

		[Constructable]
		public SamuraiFarmer() : base( AIType.AI_Melee, FightMode.Weakest, 10, 1, 0.175, 0.3)
		{

			Name = "Samurai Farmer";
			Title= ", Defiance Cult Clan";
			Body = 400;
                        Hue = 33820;
			SpeechHue= 1150;
			BaseSoundID = 0;
			Team = 0;
                        //new EtherealHorse().Rider = this;

			SetStr( 125, 145);
			SetDex( 130, 140);
			SetInt( 0, 0);

			SetHits(125, 145);

			SetSkill( SkillName.Tactics, 100.7, 100.4);
			SetSkill( SkillName.MagicResist, 100.4, 100.7);
			SetSkill( SkillName.Fencing, 110.4, 110.7);
			SetSkill( SkillName.Anatomy, 110.4, 110.7);
			SetSkill( SkillName.Parry, 75.1, 100.1);

                        Fame=15000;
			Karma=-15000;

			VirtualArmor= 75;

			Item Pitchfork = new Pitchfork();
			Pitchfork.Movable=false;
			Pitchfork.Hue=0;
		        //Bokuto.Name="SamuraiFarmer Bokuto";
                        EquipItem( Pitchfork );

                        Item Kasa = new Kasa();
			Kasa.Movable=false;
			Kasa.Hue=0;
			EquipItem( Kasa );

			Item Shirt = new Shirt();
			Shirt.Movable=false;
			Shirt.Hue=0;
			EquipItem( Shirt );

                        Item LeatherGloves = new LeatherGloves();
			LeatherGloves.Movable=false;
			LeatherGloves.Hue=0;
                        EquipItem( LeatherGloves );

                        Item TattsukeHakama = new TattsukeHakama();
			TattsukeHakama.Movable=false;
			TattsukeHakama.Hue=0;
		        EquipItem( TattsukeHakama );

			Item Sandals = new Sandals();
			Sandals.Movable=false;
			Sandals.Hue=0;
			EquipItem( Sandals );


			Item hair = new Item( 0x203D);
			hair.Hue = 1;
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem( hair );

			PackGold( 140, 255);
			PackMagicItems( 3, 7);
			PackMagicItems( 3, 7);
			PackMagicItems( 3, 7);
			PackMagicItems( 3, 7);

                                switch ( Utility.Random( 125 ))
        		 {
           			case 0: PackItem( new Kasa() ); break;
        		 }
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

		public SamuraiFarmer( Serial serial ) : base( serial )
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