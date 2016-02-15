using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an ophidian corpse" )]
	public class OphidianMatriarch : BaseCreature
	{
		public DateTime LastTimeSpoken;

		private static bool m_Talked;

		string[] say = new string[]
		{
		"Yts sylissstha. Thth las cesassis lith.",
		"Thayth myss.",
		"Ssi asi!",
		"Sthy ythshyisth syth!",
		"Sthicysla! Ythsialith.",
		"Alssthy thatysits kthisthsthy!.",
		"Thathasyth tistysathlylasi!",
		"Ciiss cityssil sisthsssi!",
		"Lishith ce.",
		"Lyshthyl tis. Ythsthi!",
		"Lasasalilsilasa ithasthtsi.",
		};

		[Constructable]
		public OphidianMatriarch() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			LastTimeSpoken = DateTime.Now;
			Name = "an ophidian matriarch";
			Body = 87;
			SpeechHue = 2129;
			BaseSoundID = 644;

			SetStr( 416, 505 );
			SetDex( 96, 115 );
			SetInt( 366, 455 );

			SetHits( 250, 303 );

			SetDamage( 11, 13 );

			SetSkill( SkillName.EvalInt, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 90.1, 100.0 );
			SetSkill( SkillName.Meditation, 5.4, 25.0 );
			SetSkill( SkillName.MagicResist, 90.1, 100.0 );
			SetSkill( SkillName.Tactics, 50.1, 70.0 );
			SetSkill( SkillName.Wrestling, 60.1, 80.0 );

			Fame = 16000;
			Karma = -16000;

			VirtualArmor = 50;

			Item item = null;
			switch( Utility.Random(2500) )
		{
			case 0: PackItem( item = new Static(0x9DC) ); break;
			case 1: PackItem( item = new Static(0x9DD) ); break;
			case 2: PackItem( item = new Static(0x9DF) ); break;
			case 3: PackItem( item = new Static(0x9E6) ); break;
			case 4: PackItem( item = new Static(0x9E7) ); break;
			case 5: PackItem( item = new Static(0x9E8) ); break;
		}
			if (item != null)
			item.Movable = true;

			PackPotion();
                        PackJewel( 0.02 );
			PackGem();
			PackItem( new Arrow( Utility.RandomMinMax( 1, 10 ) ) );
			PackGold( 300, 450 );
			PackScroll( 3, 7 );
			PackScroll( 3, 7 );
			PackScroll( 3, 7 );

			switch ( Utility.Random( 2 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 3 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}
			switch ( Utility.Random( 10 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}

			switch ( Utility.Random( 25 ) )
			{
				case 0: PackWeapon( 3, 5 ); break;
				case 1: PackArmor( 3, 5 ); break;
			}

				if ( 0.05 > Utility.RandomDouble() )
					PackReg( 250 );

				if ( 0.01 > Utility.RandomDouble() )
					PackItem( new IDWand() );
		}

		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		public override int TreasureMapLevel{ get{ return 5; } }

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.TerathansAndOphidians; }
		}

	        public override void OnGotMeleeAttack( Mobile attacker )
			{
			switch( Utility.Random(75) )
				{
			case 0: Say( "Sythsath ythiss sthstharais. Ouch! Me hurt!" ); break;
			case 1: Say( "Les! Sissythasi. This. Good blow!" ); break;
			case 3: Say( "Yssssyslysh athsytshy. Thou not attack me!" ); break;
			case 4: Say( "Sathasth slasthy. Me fight better!" ); break;
			case 5: Say( "Shythth. Yssssisas! Me fight better!" ); break;
			case 6: Say( "Lithytsyththasath liththyl siasasthalssiscy. Oof That hurt!" ); break;
			case 7: Say( "Ce sal. Las. Away with thee!" ); break;
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
					if ( m is TerathanDrone || m is TerathanWarrior || m is TerathanMatriarch || m is TerathanAvenger )
					{
					if( TimeSpan.Compare( (DateTime.Now-LastTimeSpoken), diff ) <= 0 )
						continue;

						switch( Utility.Random(100) )
						{
						case 0:	 Say ( "Ithra. Tysis." ); 					// Aaah... an evil one...
							 LastTimeSpoken = DateTime.Now;
							 break;
						case 1: Say( "Asth lysthayss.");					// Evil clicking things...
							 LastTimeSpoken = DateTime.Now;
							 break;
						case 2: Say( "Lyl! Cyisthsithsthitsi." ); 				// Attack! Attack!
							 LastTimeSpoken = DateTime.Now;
							 break;
						case 3: Say( "Asiisa ssysithsylshasyss." ); 				// I hate bugs...
							 LastTimeSpoken = DateTime.Now;
							 break;
						case 4: Say( "Iss ssitha." );				 		// Aaah... an evil one...
							 LastTimeSpoken = DateTime.Now;
							 break;
						case 5: Say( "Asi ra!" );						// Green monsters!
							 LastTimeSpoken = DateTime.Now;
							 break;
						case 6: Say( "Syssthysal. Tsisilasth sth." );				// Thou dast come here?
							 LastTimeSpoken = DateTime.Now;
							 break;
						case 7: Say( "Alsthil athra." );					// Kill the many-legs!
							 LastTimeSpoken = DateTime.Now;
							 break;
						case 8: Say( "Ththlissia asalyth rasthysia!" );				// Aaah... an evil one...
							 LastTimeSpoken = DateTime.Now;
							 break;
						case 9: Say( "Asth sal!" );						// Foul spiders!
							 LastTimeSpoken = DateTime.Now;
							 break;
						}

                	        	 }
				}
			}

		public OphidianMatriarch( Serial serial ) : base( serial )
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
			case 0: Say( "Sas sia. NOOooo!" ); break;
			case 1: Say( "Lishlysisthslalas. Sth. Me die!" ); break;
			case 2: Say( "Sth isskth. No, kill me not!" ); break;
				}
			return base.OnBeforeDeath();
			}
	}
}