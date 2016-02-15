using System;
using Server;
using Server.Items;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.First;
using Server.Spells.Second;
using Server.Spells.Third;
using Server.Spells.Fourth;
using Server.Spells.Fifth;
using Server.Spells.Sixth;
using Server.Spells.Seventh;
using Server.Misc;
using Server.Regions;
using Server.SkillHandlers;



namespace Server.Mobiles
{
	[CorpseName( "Ghastly Corpse" )]
	public class SpiritStealer : BaseCreature
	{
		[Constructable]
		public SpiritStealer () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Spirit Stealer";
			Kills = 5;
			Body = 400;
			Hue = 0;
			SetStr( 116, 150 );
			SetDex( 91, 115 );
			SetInt( 85, 150 );
			SetHits( 120, 165 );
			SetDamage( 4, 14 );

			SetDamageType( ResistanceType.Physical, 20 );

			SetResistance( ResistanceType.Physical, 28, 47 );
			SetResistance( ResistanceType.Fire, 38, 57 );
			SetResistance( ResistanceType.Cold, 38, 57 );
			SetResistance( ResistanceType.Poison, 38, 57 );
			SetResistance( ResistanceType.Energy, 38, 57 );

			SetSkill( SkillName.EvalInt, 105.1, 115.5 );
			SetSkill( SkillName.Magery, 105.1, 115.5 );
			SetSkill( SkillName.MagicResist, 95.1, 105.0 );
			SetSkill( SkillName.Tactics, 65.1, 95.0 );
			SetSkill( SkillName.Wrestling, 145.1, 155.0 );
			SetSkill( SkillName.Meditation, 155.1, 160.0 );

			Fame = 8000;
			Karma = -8000;

			VirtualArmor = 45;

			PackGem();
			PackGem();

			PackReg( 3 );
			PackItem( new Arrow( 10 ) );
			PackGold( 650, 750 );
			PackScroll( 1, 4 );
			AddItem( new Server.Items.Boots( Utility.RandomBlueHue() ) );
			AddItem( new Server.Items.Robe( Utility.RandomBlueHue() ) );
			AddItem( new Server.Items.LongHair( Utility.RandomBlueHue() ) );
		}





		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool AutoDispel{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 2; } }
		public override int Meat{ get{ return 1; } }


		public override void AggressiveAction( Mobile aggressor, bool criminal)
		{

			int drained = 0;
			//aggressor.PlaySound( 0x307 );
			aggressor.FixedParticles( 0x3789, 10, 15, 5038, 1153, 2, EffectLayer.Head );
			Say( "Regeneratus Spiritus!" );

			if ( aggressor.Mana > 20 )
			drained = 10;
			aggressor.SendMessage( "You feel your energies draining from you!" );
			///
			aggressor.Mana = aggressor.Mana - drained;
			aggressor.Stam = aggressor.Stam - drained;
			Hits = Hits + drained;
			Mana = Mana + drained;
			Stam = Stam + drained;
			///

		}

		public SpiritStealer( Serial serial ) : base( serial )
		{
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
	}
}