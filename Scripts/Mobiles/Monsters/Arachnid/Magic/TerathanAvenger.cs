using System;
using Server;
using Server.Items;
using Server.Targeting;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "a terathan avenger corpse" )]
	public class TerathanAvenger : BaseCreature
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
		public TerathanAvenger() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			LastTimeSpoken = DateTime.Now;
			Name = "a terathan avenger";
			Body = 152;
			BaseSoundID = 594;
			SpeechHue= 1549;

			SetStr( 467, 645 );
			SetDex( 77, 95 );
			SetInt( 126, 150 );

			SetHits( 296, 372 );
			SetMana( 46, 70 );

			SetDamage( 18, 22 );

			SetSkill( SkillName.EvalInt, 70.3, 100.0 );
			SetSkill( SkillName.Magery, 70.3, 100.0 );
			SetSkill( SkillName.Poisoning, 60.1, 80.0 );
			SetSkill( SkillName.MagicResist, 65.1, 80.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.Wrestling, 90.1, 100.0 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 50;

			Item item = null;
			switch( Utility.Random(1000) )
		{
			case 0: PackItem( item = new Static(0x123A) ); break;
			case 1: PackItem( item = new Static(0x123B) ); break;
			case 2: PackItem( item = new Static(0x123C) ); break;
		}
			if (item != null)
			item.Movable = true;


			for ( int i = 0; i < 8; ++i )
				PackGem();

			PackGold( 400, 550 );
			PackScroll( 0, 8 );
			PackSlayer();

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

			switch ( Utility.Random( 4 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 2, 5 ); break;
			}

				if ( 0.10 > Utility.RandomDouble() )
					PackItem( new Bola() );

			if ( Utility.Random( 75 ) == 0 )
				PackItem( new RareBlueCarpet( PieceType.NECorner ));

		}

		public override int Meat{ get{ return 6; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Lethal; } }
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

		public TerathanAvenger( Serial serial ) : base( serial )
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