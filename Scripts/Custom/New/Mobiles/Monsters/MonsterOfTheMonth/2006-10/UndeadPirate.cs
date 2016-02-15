using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
	public class UndeadPirate : BaseCreature
	{
		public override bool ClickTitle{ get{ return false; } }

		[Constructable]
		public UndeadPirate() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			SpeechHue = Utility.RandomDyedHue();
			Title = "the Undead Pirate";
			//Hue = Utility.RandomSkinHue();
			Kills = 100;

			if ( this.Female = Utility.RandomBool() )
			{
				Body = 0x191;
				Name = NameList.RandomName( "female" );
				AddItem( new ShortPants( Utility.RandomNeutralHue() ) );
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "male" );
				AddItem( new ShortPants( Utility.RandomNeutralHue() ) );
			}

			SetStr( 286, 300 );
			SetDex( 81, 95 );
			SetInt( 61, 75 );

			SetDamage( 5, 7 );

			SetSkill( SkillName.MagicResist, 125.0, 147.5 );
			SetSkill( SkillName.Swords, 165.0, 187.5 );
			SetSkill( SkillName.Tactics, 165.0, 187.5 );
			SetSkill( SkillName.Wrestling, 115.0, 137.5 );

			Fame = 1000;
			Karma = -1000;

			AddItem( new Boots( Utility.RandomNeutralHue() ) );
			AddItem( new FancyShirt());
			AddItem( new SkullCap() );
			AddItem( new Cutlass() );

			AddItem( Server.Items.Hair.GetRandomHair( Female ) );

		}

		public override bool BardImmune{ get{ return true; } }


		public override bool IsEnemy( Mobile m )
		{
			if ( m.BodyMod == 185 || m.BodyMod == 186 )
				return false;

			return base.IsEnemy( m );
		}

		public override void AggressiveAction( Mobile aggressor, bool criminal )
		{
			base.AggressiveAction( aggressor, criminal );

			if ( aggressor.BodyMod == 185 || aggressor.BodyMod == 186 )
			{
				AOS.Damage( aggressor, 50, 0, 100, 0, 0, 0 );
				aggressor.BodyMod = 0;
				aggressor.HueMod = -1;
				aggressor.FixedParticles( 0x36BD, 20, 10, 5044, EffectLayer.Head );
				aggressor.PlaySound( 0x307 );
				aggressor.SendMessage( "Your skin is scorched as the undead pirate marks burn away!" );

				if ( aggressor is PlayerMobile )
					((PlayerMobile)aggressor).SavagePaintExpiration = TimeSpan.Zero;
			}
		}






		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
		}

		public override bool AlwaysMurderer{ get{ return true; } }


		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
        	{
        	    if ( to is BaseCreature )
        	        damage *= 4;
        	}

		public UndeadPirate( Serial serial ) : base( serial )
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