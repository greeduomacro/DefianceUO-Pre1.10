using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "an ophidian corpse" )]
	[TypeAlias( "Server.Mobiles.OphidianAvenger" )]
	public class OphidianKnight : BaseCreature
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

		private static string[] m_Names = new string[]
			{
				"an ophidian knight-errant",
				"an ophidian avenger"
			};

		[Constructable]
		public OphidianKnight() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			LastTimeSpoken = DateTime.Now;
			Name = m_Names[Utility.Random( m_Names.Length )];
			Body = 86;
			SpeechHue = 2129;
			BaseSoundID = 634;

			SetStr( 417, 595 );
			SetDex( 166, 175 );
			SetInt( 46, 70 );

			SetHits( 266, 342 );
			SetMana( 0 );

			SetDamage( 16, 19 );

			SetSkill( SkillName.Poisoning, 60.1, 80.0 );
			SetSkill( SkillName.MagicResist, 65.1, 80.0 );
			SetSkill( SkillName.Tactics, 90.1, 110.0 );
			SetSkill( SkillName.Wrestling, 90.1, 100.0 );

			Fame = 10000;
			Karma = -10000;

			VirtualArmor = 40;

			Item item = null;
			switch( Utility.Random(1000) )
		{
			case 0: PackItem( item = new Static(0x1E90) ); break;
			case 1: PackItem( item = new Static(0x1E91) ); break;
		}
			if (item != null)
			item.Movable = true;

			PackPotion();
                        PackJewel( 0.01 );
			PackGem();
			PackScroll( 1, 8 );
			PackScroll( 1, 8 );
			PackGold( 400, 500 );
			PackItem( new Arrow( Utility.RandomMinMax( 1, 10 ) ) );

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

			switch ( Utility.Random( 10 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}

				if ( 0.01 > Utility.RandomDouble() )
					PackItem( new IDWand() );

				if ( 0.02 > Utility.RandomDouble() )
					PackItem( new InvisCloak() );

		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Lethal; } }
		public override int TreasureMapLevel{ get{ return 4; } }

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

		public OphidianKnight( Serial serial ) : base( serial )
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