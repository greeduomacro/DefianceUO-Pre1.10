using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "an boss elemental corpse" )]
	public class EleBoss : BaseCreature
	{
		[Constructable]
		public EleBoss() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "King Of Shame";
			Body = 14;
			Hue = 38;
			BaseSoundID = 268;

			SetStr( 1126, 1155 );
			SetDex( 166, 185 );
			SetInt( 171, 192 );

			SetHits( 1276, 1293 );

			SetDamage( 119, 116 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 65, 80 );
			SetResistance( ResistanceType.Fire, 70, 90 );
			SetResistance( ResistanceType.Cold, 45, 60 );
			SetResistance( ResistanceType.Poison, 45, 60 );
			SetResistance( ResistanceType.Energy, 45, 60 );

			SetSkill( SkillName.MagicResist, 90.1, 100.1 );
			SetSkill( SkillName.Tactics, 160.1, 200.0 );
			SetSkill( SkillName.Wrestling, 160.1, 200.0 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 85;
			ControlSlots = 2;

			PackGem();
			// TODO: Fertile dirt
			PackItem( new IronOre( 3 ) ); // TODO: Five small iron ore
			PackGold( 15100, 15150 );
			PackItem( new MandrakeRoot() );

                 }



		public override bool Unprovokable{ get{ return true; } }
		public override bool Uncalmable{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override int TreasureMapLevel{ get{ return 1; } }

		public EleBoss( Serial serial ) : base( serial )
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