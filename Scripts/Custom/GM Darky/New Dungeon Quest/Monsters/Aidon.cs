using System;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Seventh;
using Server.Spells.Fifth;
using Server.Engines.CannedEvil;
using Server.Engines.Quests.Doom;

namespace Server.Mobiles
{
	public class Aidon : BaseCreature
	{
		[Constructable]
		public Aidon():base( AIType.AI_Mage, FightMode.Weakest, 10, 1, 0.15, 0.2 )
		{
			Body = 400;
			Hue = 0x3F6;
			SpeechHue= 1175;
			Name = "Aidon the Archwizard";
			Kills = 10;
			ShortTermMurders = 10;

			SetStr( 356, 396 );
			SetDex( 125, 135 );
			SetInt( 830, 953 );

			SetDamage( 15, 20 );

			SetSkill( SkillName.Wrestling, 91.3, 97.8 );
			SetSkill( SkillName.Tactics, 91.5, 97.0 );
			SetSkill( SkillName.MagicResist, 140.6, 156.8);
			SetSkill( SkillName.Magery, 96.7, 99.8 );
			SetSkill( SkillName.EvalInt, 75.1, 80.1 );
			SetSkill( SkillName.Meditation, 61.1, 68.1 );

			Fame = 17500;
			Karma = -17500;

			VirtualArmor = 15;

			Item Robe = new Robe();
			Robe.Hue=2112;
			EquipItem( Robe );

                        Item SavageMask = new SavageMask();
			SavageMask.Movable=false;
			SavageMask.Hue=1175;
			EquipItem( SavageMask );

                        Item Sandals = new Sandals();
			Sandals.Movable=false;
			Sandals.Hue=1175;
			EquipItem( Sandals );

			Item GoldRing = new GoldRing();
			GoldRing.Movable=false;
			GoldRing.Hue=1360;
			EquipItem( GoldRing );

			Item hair = new Item( 0x203B);
			hair.Hue = 1072;
			hair.Layer = Layer.Hair;
			AddItem( hair );

			Item beard = new Item( 0x203E);
			beard.Hue = 1072;
			beard.Layer = Layer.FacialHair;
			AddItem( beard );

			switch ( Utility.Random( 10 ) )
			{
				case 0: PackReg( 39 ); break;
				case 1: PackReg( 33 ); break;
				case 2: PackReg( 36 ); break;
				case 3: PackReg( 36 ); break;
			}

			PackGold( 800, 1200 );

			switch ( Utility.Random( 25 ) )
			{
				case 0: PackWeapon( 4, 5 ); break;
				case 1: PackArmor( 4, 5 ); break;
			}

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 10 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 15 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}

		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }

		public Aidon( Serial serial ) : base( serial )
		{
		}

		public void SpawnAidons( Mobile target )
		{
			Map map = this.Map;

			if ( map == null )
				return;

			int aidons = 0;

			foreach ( Mobile m in this.GetMobilesInRange( 25 ) )
			{
				if ( m is AidonCopy )
					++aidons;
			}

			if ( aidons < 1 )
			{
				PlaySound( 582 );

				int newAidons = Utility.RandomMinMax( 3, 3 );

				for ( int i = 0; i < newAidons; ++i )
				{
					BaseCreature aidon;

					switch ( Utility.Random( 2 ) )
					{
						default:
						case 0:	aidon = new AidonCopy(); break;
						case 1:	aidon = new AidonCopy(); break;
					}

					aidon.Team = this.Team;

					bool validLocation = false;
					Point3D loc = this.Location;

					for ( int j = 0; !validLocation && j < 10; ++j )
					{
						int x = X + Utility.Random( 3 ) - 1;
						int y = Y + Utility.Random( 3 ) - 1;
						int z = map.GetAverageZ( x, y );

						if ( validLocation = map.CanFit( x, y, this.Z, 16, false, false ) )
							loc = new Point3D( x, y, Z );
						else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
							loc = new Point3D( x, y, z );
					}

					aidon.MoveToWorld( loc, map );
					aidon.Combatant = target;
				}
			}

			switch ( Utility.Random( 25 ) )
			{
				case 0: this.Say( "You DARE to attack me?!" ); break;
				case 1: this.Say( "You will remember this day!" ); break;
				case 2: this.Say( "I do not fall easily!" ); break;
				case 3: this.Say( "Foolish human! Prepare to die!" ); break;
				case 4: this.Say( "Fall into the darkness!" ); break;
				case 5: this.Say( "You WILL fall to me!" ); break;
				case 6: this.Say( "My magic is beyond anything you have ever seen!" ); break;
			}
		}

		public void DoSpecialAbility( Mobile target )
		{
			if ( 0.2 >= Utility.RandomDouble() ) // 20% chance to spawn more of himself.
				SpawnAidons( target );
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			DoSpecialAbility( attacker );
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			DoSpecialAbility( defender );
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