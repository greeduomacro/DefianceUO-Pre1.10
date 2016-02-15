using System;
using Server;
using Server.Items;
using Server.Targeting;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "a battlemage terathan corpse" )]
	public class TerathanBattlemage : BaseCreature
	{
		public DateTime LastTimeSpoken;

		private static bool m_Talked;

		string[] say = new string[]
		{
		"CTotskritroch. Chorokit echak! Kotchochetcharrep.",
		"Tat ttochkyrach cheytt ukttu tyktitti ckatchi. Ektitti.",
		"Cko ottta!",
		"Attrichrreprrupcky. Uckickrrapka.",
		"Tcha ichich ickrraptittiktti.",
		"Tekottrich. Rych!",
		"Ckochitte. Ccku ccki.",
		"Tit tittchy.",
		"Tuttekrokcheskrit! Tcheickatt! Tte.",
		"Eck. Tticki.",
		"Ittock! Ckitiruch.",
		"Ockrroptcha. Cckecckitok",
		"Ick ttiyt cke!",
		};

		[Constructable]
		public TerathanBattlemage() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 1.0, 1.0 )
		{
			Name = "a battlemage terathan";
			Body = 72;
			Hue = 1155;
			LastTimeSpoken = DateTime.Now;
			BaseSoundID = 599;
			SpeechHue= 1549;

			SetStr( 316, 405 );
			SetDex( 96, 115 );
			SetInt( 366, 455 );

			SetHits( 3000, 3000 );
			SetMana( 500, 500 );

			SetDamage( 30, 30 );

			SetSkill( SkillName.EvalInt, 130.0, 170.0 );
			SetSkill( SkillName.Magery, 130.0, 130.0 );
			SetSkill( SkillName.MagicResist, 130.0, 130.0 );
			SetSkill( SkillName.Tactics, 130.0, 130.0 );
			SetSkill( SkillName.Wrestling, 130.0, 170.0 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 55;

			Item item = null;
			switch( Utility.Random(200) )
		{
			case 0: PackItem( item = new Static(0x14F9) ); break;
			case 1: PackItem( item = new Static(0x14F7) ); break;

		}
			if (item != null)
			{
			item.Movable = true;
			item.Weight = 11;
			}

			PackItem( new SpidersSilk( 10 ) );
			PackWeapon( 2, 5 );
			PackWeapon( 2, 5 );
			PackWeapon( 2, 5 );
			PackSlayer();

		}

		public override int TreasureMapLevel{ get{ return 5; } }

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.TerathansAndOphidians; }
		}

	        public override void OnGotMeleeAttack( Mobile attacker )
			{
			switch( Utility.Random(75) )
				{
			case 0: Say( "Cho titte ryktik. Aaah! That hurt..." ); break;
			case 1: Say( "Tta chiektat ittckyuk. Good blow!" ); break;
			case 3: Say( "Ichch! Okrek. Tyktoktukuch! Me Hurt!" ); break;
			case 4: Say( "Ech ttu iktchyut. Ouch!" ); break;
			case 5: Say( "Tchytittuk otttche. Good blow!" ); break;
			case 6: Say( "Raktikorrup. Eckrak etrrop Ouch! Me hurt!" ); break;
			case 7: Say( "Tchayk rrypchcckuich chucckeskrit. Good blow!" ); break;
			case 8: Say( "Skritccki tchuech! Tyk. Oof! That hurt!" ); break;
			case 9: Say( "Ckochiatoch! Rraptchaik at. Me Hurt!" ); break;
			case 10: Say( "Yslilythssithis sytlysh shisthythessis. Good blow!" ); break;
			case 11: Say( "Tyt ockkutcha. Rochtte. Away with thee!" ); break;
			case 12: Say( "Cckorraptykti ytt tut. Away with thee!" ); break;
			case 13: Say( "Rropockttuutuch ccko rychtte. Away with thee!" ); break;
				}
			}

			TimeSpan diff = TimeSpan.FromSeconds( 15 );
			public override void OnThink()
			{
				foreach ( Mobile m in this.GetMobilesInRange( 10 ) )
				{
					if ( m is OphidianMage || m is OphidianKnight || m is OphidianMatriarch || m is OphidianArchmage || m is OphidianWarrior )
					{
					if( TimeSpan.Compare( (DateTime.Now-LastTimeSpoken), diff ) <= 0 )
						continue;

						switch( Utility.Random(100) )
						{
						case 0:	 Say ( "Propytt ockrakttu tiktyttekckuutt." ); 			// Mine enemy is near!
							 LastTimeSpoken = DateTime.Now;
							 break;
						case 1: Say( "Ukttautt tto.");						// Slimy fang faces!
							 LastTimeSpoken = DateTime.Now;
							 break;
						case 2: Say( "Cku tut!" ); 						// The treaty is broken
							 LastTimeSpoken = DateTime.Now;
							 break;
						case 3: Say( "Echickchetekcke! Chtyt." ); 				// Nasty snakes!
							 LastTimeSpoken = DateTime.Now;
							 break;
						case 4: Say( "Tytcharrop. Chuich." );					// And so the war goeth on...
							 LastTimeSpoken = DateTime.Now;
							 break;
						case 5: Say( "Rakky! Tto." );						// Legless ones!
							 LastTimeSpoken = DateTime.Now;
							 break;
						case 6: Say( "Cckuttaack. Ke." );					// Drone killers!
							 LastTimeSpoken = DateTime.Now;
							 break;
						case 7: Say( "Rukttu ccko tchotcha." );					// Quickly must kill...
							 LastTimeSpoken = DateTime.Now;
							 break;
						case 8: Say( "Ok. Cckyett." );						// Drone killers!
							 LastTimeSpoken = DateTime.Now;
							 break;
						}

                	        	 }
				}
			}

		public TerathanBattlemage( Serial serial ) : base( serial )
		{
		}

		public override void OnMovement( Mobile m, Point3D oldLocation )
                {
         		if( m_Talked == false )
        			 {
          		 	 if ( m.InRange( this, 5 ) )
          			  {
          				m_Talked = true;
              				SayRandom( say, this );
					this.Move( GetDirectionTo( m.Location ) );
					SpamTimer t = new SpamTimer();
					t.Start();
            		         }
		 }
	}

		private class SpamTimer : Timer
	{
		public SpamTimer() : base( TimeSpan.FromSeconds( 25 ) )
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
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
		public override bool OnBeforeDeath()
			{
			switch( Utility.Random(6) )
				{
			case 0: Say( "Che achrrep. No, kill me not!" ); break;
			case 1: Say( "Lyththesthyssthisyl tysitsses. NOOooo!" ); break;
			case 2: Say( "Rekttu. Rropyttrok. No, kill me not!" ); break;
				}
			return base.OnBeforeDeath();
			}
	}
}