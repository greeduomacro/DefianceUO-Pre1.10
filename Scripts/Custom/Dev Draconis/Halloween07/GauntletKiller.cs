using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a Gauntlet creature's corpse" )]
	public class GauntletKiller : BaseCreature
	{
		[Constructable]
		public GauntletKiller() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a gauntlet killer";
			Body = 400;
			BaseSoundID = 0;
			Hue = 1109;
			Kills = 5;

			SetStr( 100 );
			SetDex( 200 );
			SetInt( 100 );

			SetHits( 80 );

			SetDamage( 150, 200 );

			SetSkill( SkillName.MagicResist, 100.0 );
			SetSkill( SkillName.Tactics, 200.0 );
			SetSkill( SkillName.Swords, 200.0 );

			Fame = 1500;
			Karma = -1500;

			VirtualArmor = 30;

			Bardiche w = new Bardiche();
			w.Hue = 1109;
			w.Movable = false;
			w.LootType = LootType.Blessed;
			AddItem( w );
		}

		public GauntletKiller( Serial serial ) : base( serial )
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