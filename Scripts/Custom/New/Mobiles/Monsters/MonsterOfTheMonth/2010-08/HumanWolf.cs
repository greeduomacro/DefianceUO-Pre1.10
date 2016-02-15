using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
	public class HumanWolf : BaseCreature
	{
		[Constructable]
		public HumanWolf(): base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			SpeechHue = Utility.RandomDyedHue();

			Hue = Utility.RandomSkinHue();

			if ( this.Female = Utility.RandomBool() )
			{
				Body = 0x191;
				Name = NameList.RandomName( "female" );
				AddItem( new Skirt( Utility.RandomNeutralHue() ) );
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "male" );
				AddItem( new ShortPants( Utility.RandomNeutralHue() ) );
			}

			SetStr( 120, 140);
			SetDex( 76, 95);
			SetInt( 300 );

			SetDamage( 15, 30);

			SetSkill( SkillName.Fencing, 86.0, 107.5 );
			SetSkill( SkillName.Macing, 95.0, 115.5 );
			SetSkill( SkillName.MagicResist, 95.0, 117.5 );
			SetSkill( SkillName.Swords, 85.0, 97.5 );
			SetSkill( SkillName.Tactics, 75.0, 87.5 );
			SetSkill( SkillName.Wrestling, 105.0, 127.5 );

			Fame = 2500;
			Karma = -2500;

			AddItem( new Boots( Utility.RandomNeutralHue() ) );
			AddItem( new FancyShirt());
			AddItem( new Bandana());

			switch ( Utility.Random( 7 ))
			{
				case 0: AddItem( new Longsword() ); break;
				case 1: AddItem( new Cutlass() ); break;
				case 2: AddItem( new Broadsword() ); break;
				case 3: AddItem( new Axe() ); break;
				case 4: AddItem( new Club() ); break;
				case 5: AddItem( new Dagger() ); break;
				case 6: AddItem( new Spear() ); break;
			}

			//Utility.AssignRandomHair( this );

			VirtualArmor = 30;

			PackGold(700);
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool Unprovokable{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
/*
		public override void OnDeath( Container c )
	  	{
			if ( Utility.Random( 95 ) <  1 )
				c.DropItem( new WerewolfTooth( ToothType.Normal ) );

			base.OnDeath( c );
	  	}
*/

		public override bool OnBeforeDeath()
		{
			Werewolf rm = new Werewolf();
			rm.Team = this.Team;
			rm.Combatant = this.Combatant;
			rm.NoKillAwards = true;

			if ( rm.Backpack == null )
			{
				Backpack pack = new Backpack();
				pack.Movable = false;
				rm.AddItem( pack );
			}

			for ( int i = 0; i < 2; i++ )
			{
				LootPack.FilthyRich.Generate( this, rm.Backpack, true, LootPack.GetLuckChanceForKiller( this ) );
				LootPack.FilthyRich.Generate( this, rm.Backpack, false, LootPack.GetLuckChanceForKiller( this ) );
			}

			Effects.PlaySound(this, Map, GetDeathSound());
			Effects.SendLocationEffect( Location, Map, 0x3709, 30, 10, 0x496, 0 );
			Effects.SendLocationEffect( Location, Map, 0x375A, 30, 10, 0x496, 0 );
			rm.MoveToWorld( Location, Map );

			Delete();
			return false; //OnDeath will not trigger
		}

		public override void OnDamagedBySpell( Mobile from )
	  	{
			this.Combatant = from;

			if ( from.Combatant == null )
				return;

			Mobile m = from.Combatant;

			if ( m.Combatant == null )
				return;

			if ( Alive )
			switch ( Utility.Random( 3 ) )
			{
				case 0: m.Hits +=( Utility.Random( 40, 80 ) ); break;
				case 1: m.Hits += ( Utility.Random( 50, 90 ) ); break;
				case 2: m.Hits +=( Utility.Random( 100, 150 ) ); break;
			}

			from.PlaySound( 0x1F2 );
			from.SendMessage("Magic heals this creature!!!");
			m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
	  	}


		public HumanWolf( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadEncodedInt();
		}
	}
}