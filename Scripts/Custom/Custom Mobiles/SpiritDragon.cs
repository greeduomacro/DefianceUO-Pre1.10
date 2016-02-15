using System;
using Server;
using Server.Items;
using Server.Engines.CannedEvil;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Spells;
using Server.Misc;
using Server.Regions;
using Server.SkillHandlers;

namespace Server.Mobiles
{
	public class SpiritDragon : BaseChampion
	{
		public override ChampionSkullType SkullType{ get{ return ChampionSkullType.Venom; } }

		[Constructable]
		public SpiritDragon() : base( AIType.AI_Melee)
		{
			Name = "Spirit Dragon";
			Kills = 5;
			Body = 46;
			BaseSoundID = 362;
			Hue = 33;
			SetStr( 155, 155 );
			SetDex( 100, 100 );
			SetInt( 450, 450 );
			SetHits( 3000 );
			SetDamage( 40, 59 );
			SetDamageType( ResistanceType.Physical, 75 );
			SetDamageType( ResistanceType.Fire, 25 );

			SetResistance( ResistanceType.Physical, 65, 75 );
			SetResistance( ResistanceType.Fire, 80, 90 );
			SetResistance( ResistanceType.Cold, 70, 80 );
			SetResistance( ResistanceType.Poison, 60, 70 );
			SetResistance( ResistanceType.Energy, 60, 70 );

			SetSkill( SkillName.MagicResist, 120.5, 130.0 );
			SetSkill( SkillName.Wrestling, 165.0, 180.0 );

			Fame = 10000;
			Karma = -10000;

			VirtualArmor = 45;

;
		}

		public override int GetIdleSound()
		{
			return 0x2D3;
		}

		public override int GetHurtSound()
		{
			return 0x2D1;
		}

		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		public override Poison HitPoison{ get{ return Poison.Greater; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool AutoDispel{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 5; } }
		public override int Meat{ get{ return 1; } }

		public override void AggressiveAction( Mobile aggressor, bool criminal)
		{

			int drained = 5;
			//aggressor.PlaySound( 0x307 );
			aggressor.FixedParticles( 0x37C4, 10, 15, 5038, 33, 2, EffectLayer.Head );
			Say( "You cannot kill the Immortal!" );
			aggressor.SendMessage( "Your Magical Powers are being drained by the mythical beast!" );
			///
			aggressor.Mana = aggressor.Mana - drained;
			aggressor.Hits = aggressor.Hits - drained;
			Hits = Hits + drained;
			Stam = Stam + drained;
			///

		}

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is Dragon || to is WhiteWyrm || to is SwampDragon || to is Drake || to is Nightmare || to is Daemon )
				damage *= 4;
		}

		public SpiritDragon( Serial serial ) : base( serial )
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