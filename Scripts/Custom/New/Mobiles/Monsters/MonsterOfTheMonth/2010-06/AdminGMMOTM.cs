using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Targeting;
using Server.ContextMenus;
using Server.Gumps;
using Server.Misc;
using Server.Network;
using Server.Spells.First;
using Server.Mobiles;



namespace Server.Mobiles
{
	[CorpseName( "an odd looking corpse" )]
	public class AdminGMMotm : BaseBoss
	{

		[Constructable]
		public AdminGMMotm() : base( AIType.AI_Mage, FightMode.Closest )
		{
			Body = 987;

			switch (Utility.Random( 11 ))
			{
			case 0:  	Name = "Cheif LiGhT";
				   	Hue = 0x847E;
				   	break;
			case 1:	Name = "Lead Dev Kamron";
					Hue = 0x847E;
					break;
			case 2:	Name = "Lead Admin Wolf";
					Hue = 0x847E;
					break;
			case 3: 	Name = "Admin Qwert";
					Hue = 0x8482;
					break;
			case 4: 	Name = "Admin Hermes";
					Hue = 0x8482;
					break;
			case 5: 	Name = "Lead GM Fee";
					Hue = 0x8485;
					break;
			case 6: 	Name = "Dev Troy";
					Hue = 0x8497;
					break;
			case 7: 	Name = "GM Phoenix";
					Hue = 0x8026;
					break;
			case 8: 	Name = "GM Jason";
					Hue = 0x8026;
					break;
			case 9: 	Name = "GM Lakota";
					Hue = 0x8026;
					break;
			case 10: 	Name = "GM Tolkin";
					Hue = 0x8026;
					break;



			}


			Team = 2;
			SetStr( 175 );
			SetDex( 175 );
			SetInt( 1000 );
			SetDamage( 350 );
			SetHits( 10000 );
			SetStam( 100 );

			SetSkill( SkillName.Magery, 150.0);
			SetSkill( SkillName.Meditation, 150.0);
			SetSkill( SkillName.MagicResist, 100.0 );
			SetSkill( SkillName.Tactics, 90.0 );
			SetSkill( SkillName.Wrestling, 90.0 );
			SetSkill( SkillName.Anatomy, 90.0 );
			SetSkill( SkillName.Fencing, 90.0 );
			SetSkill( SkillName.Macing, 90.0 );
			SetSkill( SkillName.Swords, 90.0 );

			Fame = 10000;
			Karma = -10000;


		}

		public override void OnDeath( Container c )
		{
			if (Utility.Random( 150 ) <  1 )
			c.DropItem( new StafferMOTMRare() );

            	base.OnDeath( c );
		}


		public override int CanBandageSelf { get { return 200; } }
		public override int DoMoreDamageToPets { get { return 10; } }
		public override int DoLessDamageFromPets { get { return 10; } }
		public override bool DoProvoPets { get { return true; } }
		public override bool DoEarthquake { get { return true; } }
		public override bool DoDetectHidden { get { return true; } }
		public override int CanCastReflect{ get { return 2; } }



		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 1 );
		}

		public override bool ShowFameTitle{ get{ return false; } }
		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 5; } }
		public override bool BardImmune{ get{ return true; } }

		public AdminGMMotm( Serial serial ) : base( serial )
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