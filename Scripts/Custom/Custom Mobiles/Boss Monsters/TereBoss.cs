using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a snake corpse" )]
	public class TereBoss : BaseCreature
	{
		[Constructable]
		public TereBoss() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Queen Of The Snakes";
			Body = 87;
			Hue = 1406;
			BaseSoundID = 644;

			SetStr( 2416, 2505 );
			SetDex( 2296, 2115 );
			SetInt( 2366, 2455 );

			SetHits( 3250, 3303 );

			SetDamage( 111, 113 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 35, 100 );
			SetResistance( ResistanceType.Fire, 35, 100 );
			SetResistance( ResistanceType.Cold, 35, 100 );
			SetResistance( ResistanceType.Poison, 35, 100 );
			SetResistance( ResistanceType.Energy, 35, 100 );

			SetSkill( SkillName.EvalInt, 190.1, 200.0 );
			SetSkill( SkillName.Magery, 190.1, 200.0 );
			SetSkill( SkillName.Meditation, 195.4, 225.0 );
			SetSkill( SkillName.MagicResist, 190.1, 200.0 );
			SetSkill( SkillName.Tactics, 150.1, 270.0 );
			SetSkill( SkillName.Wrestling, 160.1, 280.0 );

			Fame = 16000;
			Karma = -16000;

			VirtualArmor = 65;

			PackGold( 22350, 22400 );
			PackPotion();
			PackScroll( 3, 7 );

                 }



		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		public override int TreasureMapLevel{ get{ return 4; } }

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.TerathansAndOphidians; }
		}

		public TereBoss( Serial serial ) : base( serial )
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